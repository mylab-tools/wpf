    using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MyLab.Wpf
{
    static class ViewModelTypeWrapperBuilder
    {
        private static readonly AssemblyBuilder AssemblyBuilder;
        private static readonly ModuleBuilder ModuleBuilder;
        private static readonly MethodInfo PropChangeMethod;

        /// <summary>
        /// Initializes a new instance of <see cref="ViewModelTypeWrapperBuilder"/>
        /// </summary>
        static ViewModelTypeWrapperBuilder()
        {
            AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("ViewModelLib"), AssemblyBuilderAccess.Run);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule("ViewModelModule");
            PropChangeMethod = typeof(ViewModel).GetMethod("OnPropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static Type RetrieveVmTypeWrapper(Type originVmType)
        {
            var wrapperTypeName = CreateWrapperTypeName(originVmType.FullName);
            return ModuleBuilder.GetType(wrapperTypeName) ?? CreateWrapperType(wrapperTypeName, originVmType);
        }

        private static Type CreateWrapperType(string wrapperTypeName, Type originVmType)
        {
            var tb = ModuleBuilder.DefineType( wrapperTypeName,  TypeAttributes.Class | TypeAttributes.Public, originVmType);

            tb.SetCustomAttribute(
                new CustomAttributeBuilder(
                    typeof(IsVmWrapperAttribute).GetConstructor(Type.EmptyTypes),
                    new object[0]
                    ));
            
            foreach (var ctor in originVmType.GetConstructors())
                RepeatConstructor(tb, ctor);

            var props = originVmType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => (p.GetSetMethod()?.IsVirtual).GetValueOrDefault());

            foreach (var p in props)
                WrapProperty(tb, p);

            var tRes=  tb.CreateType();

            //AssemblyBuilder.Save("ViewModelLib.dll");

            return tRes;
        }

        private static void WrapProperty(TypeBuilder tb, PropertyInfo originP)
        {
            var pb = tb.DefineProperty(originP.Name, originP.Attributes, originP.PropertyType, Type.EmptyTypes);

            var attributes = MethodAttributes.Virtual | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            var originGet = originP.GetGetMethod();
            var pGet = tb.DefineMethod("get_" + originP.Name, attributes, originP.PropertyType, null);

            var gil = pGet.GetILGenerator();
            gil.Emit(OpCodes.Ldarg_0);
            gil.Emit(OpCodes.Call, originGet);
            gil.Emit(OpCodes.Ret);
            
            var originSet = originP.GetSetMethod();

            var pSet = tb.DefineMethod("set_" + originP.Name, attributes, typeof(void), new[] { originP.PropertyType });
            pSet.InitLocals = false;

            var sil = pSet.GetILGenerator();

            sil.Emit(OpCodes.Ldarg_0);
            sil.Emit(OpCodes.Ldarg_1);
            sil.Emit(OpCodes.Call, originSet);
            
            sil.Emit(OpCodes.Ldarg_0);
            sil.Emit(OpCodes.Ldstr, originP.Name);
            sil.Emit(OpCodes.Call, PropChangeMethod);

            sil.Emit(OpCodes.Ret);
            
            pb.SetGetMethod(pGet);
            pb.SetSetMethod(pSet);
        }

        private static void RepeatConstructor(TypeBuilder tb, ConstructorInfo ctor)
        {
            var parameters = ctor.GetParameters();

            var cb = tb.DefineConstructor(ctor.Attributes, ctor.CallingConvention,
                parameters.Select(p => p.ParameterType).ToArray());

            var il = cb.GetILGenerator();

            for (int i = 0; i < parameters.Length+1; i++)
                il.Emit(OpCodes.Ldarg, i);
            il.Emit(OpCodes.Call, ctor);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);
        }

        static string CreateWrapperTypeName(string originVmTypeFullName)
        {
            return $"{originVmTypeFullName}_VmWrapper";
        }
    }
}