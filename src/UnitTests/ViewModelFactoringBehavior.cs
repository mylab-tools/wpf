using System;
using System.Collections.Generic;
using MyLab.Wpf;
using Xunit;

namespace UnitTests
{
    public class ViewModelFactoringBehavior
    {
        [Fact]
        public void ShouldCreateWithCtorParams()
        {
            //Act
            var vm = ViewModel.Create<TestVm>("foo");

            //Assert
            Assert.Equal("foo", vm.InitialValue);
            Assert.Null(vm.Value);
        }

        [Fact]
        public void ShouldCreateWithPropertyChangable()
        {
            //Arrange
            var vm = ViewModel.Create<TestVm>();
            string changedPropertyName = null;
            vm.PropertyChanged += (sender, args) => { changedPropertyName = args.PropertyName; };

            //Act
            vm.Value = "foo";

            //Assert
            Assert.Equal(nameof(TestVm.Value), changedPropertyName);
        }

        [Fact]
        public void ShouldCreateWithExpression()
        {
            //Act
            var vm = ViewModel.Create<TestVm>(() => new TestVm());

            //Assert
            Assert.Null(vm.InitialValue);
            Assert.Null(vm.Value);
        }

        [Fact]
        public void ShouldCreateWithExpressionAndCtorParams()
        {
            //Act
            var vm = ViewModel.Create<TestVm>(()=>new TestVm("foo"));

            //Assert
            Assert.Equal("foo", vm.InitialValue);
            Assert.Null(vm.Value);
        }

        [Fact]
        public void ShouldCreateWithExpressionAndCtorParamsAndPropertyAssignment()
        {
            //Act
            var vm = ViewModel.Create<TestVm>(
                () => new TestVm("foo")
                {
                    Value = "bar"
                });

            //Assert
            Assert.Equal("foo", vm.InitialValue);
            Assert.Equal("bar", vm.Value);
        }

        [Fact]
        public void ShouldCreateChangableWhenCreateWithExpr()
        {
            //Arrange
            var vm = ViewModel.Create<TestVm>(() => new TestVm("foo"));
            string changedPropertyName = null;
            vm.PropertyChanged += (sender, args) => { changedPropertyName = args.PropertyName; };

            //Act
            vm.Value = "foo";

            //Assert
            Assert.Equal(nameof(TestVm.Value), changedPropertyName);
        }

        [Fact]
        public void ShouldCreateNested()
        {
            ViewModel.Create(() =>
                new TestVm
                {
                    Title = "Yandex demo project",
                    Nested =
                    {
                        ViewModel.Create(() => new TestVm {Title = "yandex", Value = "http://yandex.ru"}),
                        ViewModel.Create(() => new TestVm {Title = "yandex-2", Value = "http://yandex.ru"}),
                        ViewModel.Create(() => new TestVm {Title = "yandex-3", Value = "http://yandex.ru"})
                    }
                }
            );
        }

        public class TestVm : ViewModel
        {
            public string InitialValue { get; set; }
            public virtual string Value { get; set; }

            public List<TestVm> Nested { get; } = new List<TestVm>();

            public TestVm(string initialValue)
            {
                InitialValue = initialValue;
            }

            public TestVm()
            {
                
            }
        }
    }
}
