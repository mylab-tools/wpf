using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Controls;
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
            throw new NotImplementedException();
            //var d = Create<TVm>(ctorArgs);
            //d.Owner = this;

            //return d;
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

        protected void UpdateStates()
        {
            foreach (var cmd in _commands)
                cmd.OnCanExecuteChanged();
        }

        protected ViewModelTail CreateTail()
        {
            return new ViewModelTail(this);
        }

        public class ViewModelTail
        {
            private readonly ViewModel _viewModel;

            public void PropertyChanged(string propertyName)
            {
                _viewModel.OnPropertyChanged(propertyName);
            }

            public void UpdateStates()
            {
                _viewModel.UpdateStates();
            }

            internal ViewModelTail(ViewModel viewModel)
            {
                _viewModel = viewModel;
            }
        }
    }
}