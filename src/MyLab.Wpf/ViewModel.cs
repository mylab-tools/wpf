using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using MyLab.Wpf.SequenceCalling;

namespace MyLab.Wpf
{
    public class ViewModel : UiNotifier
    {
        private readonly List<VmCommand> _commands = new List<VmCommand>();

        public virtual string Title { get; set; }

        public ViewModel Owner { get; private set; }

        public VmBusyDescription Busy { get; } = new VmBusyDescription();

        //protected TaskScheduler UiTaskFactory { get; private set; }
        //protected TaskScheduler BiztaskFactory { get; private set; }

        protected TaskFactory UiTaskFactory { get; private set; }
        protected TaskFactory BizTaskFactory { get; private set; }

        protected CallSequenceSource CallSource { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ViewModel"/>
        /// </summary>
        protected ViewModel()
        {
            
        }

        public virtual void Initialize(VmInitializationContext ctx)
        {
            UiTaskFactory = new TaskFactory(ctx.UiScheduler);
            BizTaskFactory = new TaskFactory(ctx.BizScheduler);

            CallSource = new CallSequenceSource(
                new TaskFactory(ctx.UiScheduler),
                new TaskFactory(ctx.BizScheduler));
        }

        public TVm CreateChild<TVm>(params object[] ctorArgs)
            where TVm : ViewModel
        {
            var d = Create<TVm>(ctorArgs);
            d.Owner = this;

            return d;
        }

        public static T Create<T>(params object[] ctorArgs) where T : ViewModel
        {
            var wrapperType = ViewModelTypeWrapperBuilder.RetrieveVmTypeWrapper(typeof(T));

            return (T)Activator.CreateInstance(wrapperType, ctorArgs);
        }

        protected void RegisterCommand(VmCommand cmd)
        {
            _commands.Add(cmd);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            foreach (var cmd in _commands)
                cmd.OnCanExecuteChanged();
        }
    }
}