using System;
using MyLab.Wpf;
using Xunit;

namespace UnitTests
{
    public class FactoryExpressionTypeReplacerBehavior
    {
        [Fact]
        public void ShouldCreateWithNewExpr()
        {
            //Arrange


            //Act
            var expr = FactoryExpressionTypeReplacer.Replace(() => new Base("foo"), typeof(Inheritor));

            var instance = ExpressionValueProvidingTools.GetValue(expr) as Inheritor;

            //Assert
            Assert.NotNull(instance);
            Assert.Equal("foo", instance.Val);
        }

        [Fact]
        public void ShouldCreateWithInitExpr()
        {
            //Arrange


            //Act
            var expr = FactoryExpressionTypeReplacer.Replace(() => new Base
            {
                Val = "foo"
            }, typeof(Inheritor));

            var instance = ExpressionValueProvidingTools.GetValue(expr) as Inheritor;

            //Assert
            Assert.NotNull(instance);
            Assert.Equal("foo", instance.Val);
        }

        [Fact]
        public void ShouldNotPassWhenCtorSignatureMismatch()
        {
            //Arrange


            //Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                FactoryExpressionTypeReplacer.Replace(() => new Base("foo", "bar"), typeof(Inheritor));
            });
        }

        class Base
        {
            public string Val { get; set; }

            public Base(string val)
            {
                Val = val;
            }

            public Base()
            {
                
            }

            public Base(string first, string second)
            :   this(first + second)
            {

            }
        }

        class Inheritor : Base
        {
            public Inheritor(string val)
            : base(val)
            {
                
            }

            public Inheritor()
            {
                
            }
        }
    }
}
