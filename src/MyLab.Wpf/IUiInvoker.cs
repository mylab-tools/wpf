using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using MyLab.Wpf.SequenceCalling;

namespace MyLab.Wpf
{
    public interface IUiInvoker
    {
        /// <summary>
        /// Invokes delegate in UI thread
        /// </summary>
        void UiInvoke(Action act);
        /// <summary>
        /// Invokes delegate in business thread
        /// </summary>
        void BizInvoke(Action act);
        /// <summary>
        /// Starts sequence of invocations
        /// </summary>
        CallSequenceSource Sequence();
    }

    class CoreUiInvoker : IUiInvoker
    {
        private readonly TaskFactory _uiTaskFactory;
        private readonly TaskFactory _bizTaskFactory;

        public CoreUiInvoker(TaskScheduler uiScheduler, TaskScheduler bizScheduler)
        {
            _uiTaskFactory = new TaskFactory(uiScheduler ?? throw new ArgumentNullException(nameof(uiScheduler)));
            _bizTaskFactory = new TaskFactory(bizScheduler ?? throw new ArgumentNullException(nameof(bizScheduler)));
        }

        public static CoreUiInvoker CreateAndInitThreads(Dispatcher applicationDispatcher)
        {
            TaskScheduler uiScheduler = null;
            TaskScheduler bizScheduler = null;

            applicationDispatcher.InvokeAsync(() =>
            {
                uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

                Thread.CurrentThread.Name = "GUI";
            }).Wait();

            Task.Run(() =>
                {
                    Thread.CurrentThread.Name = "BIZ";

                    return bizScheduler = TaskScheduler.Current;
                })
                .Wait();

            return new CoreUiInvoker(uiScheduler, bizScheduler);
        }

        public void UiInvoke(Action act)
        {
            _uiTaskFactory.StartNew(act);
        }

        public void BizInvoke(Action act)
        {
            _bizTaskFactory.StartNew(act);
        }

        public CallSequenceSource Sequence()
        {
            return new CallSequenceSource(_uiTaskFactory, _bizTaskFactory);
        }
    }
}