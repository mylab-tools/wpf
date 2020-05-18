using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyLab.Wpf.SequenceCalling
{
    public class CallSequence<T> : CallSequenceBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CallSequence{T}"/>
        /// </summary>
        internal CallSequence(TaskFactory uiTaskFactory, TaskFactory bizTaskFactory, IEnumerable<IScheduledTask> tasks)
        : base(uiTaskFactory, bizTaskFactory, tasks)
        {
        }

        public CallSequence DoUi(Action<T> act)
        {
            return new CallSequence(
                UiTaskFactory, 
                BizTaskFactory, 
                GetTaskList(new UiScheduledCallTask<T>
                {
                    UiTaskScheduler = UiTaskFactory,
                    Act = act
                }));
        }
    }

    public class CallSequence : CallSequenceBase
    {
        

        /// <summary>
        /// Initializes a new instance of <see cref="CallSequence"/>
        /// </summary>
        public CallSequence(TaskFactory uiTaskFactory, TaskFactory bizTaskFactory, IEnumerable<IScheduledTask> tasks)
        : base(uiTaskFactory, bizTaskFactory, tasks)
        {
            
        }

        public CallSequence DoUi(Action act)
        {
            return new CallSequence(
                UiTaskFactory,
                BizTaskFactory,
                GetTaskList(new UiScheduledCallTask
                {
                    UiTaskScheduler = UiTaskFactory,
                    Act = act
                }));
        }
    }

    public class CallSequenceBase : CallSequenceSource
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CallSequenceBase"/>
        /// </summary>
        public CallSequenceBase(TaskFactory uiTaskFactory, TaskFactory bizTaskFactory,
            IEnumerable<IScheduledTask> tasks)
        : base(uiTaskFactory, bizTaskFactory, tasks)
        {
            
        }

        public void Perform(Action<Exception> uiDefaultErrorHandler = null)
        {
            Perform(CancellationToken.None,uiDefaultErrorHandler);
        }

        public void Perform(CancellationToken cancellationToken, Action<Exception> uiDefaultErrorHandler = null)
        {
            var tasks = new Queue<IScheduledTask>(Tasks);

            if (tasks.Count == 0) return;
            var st = tasks.Dequeue();
            

            PerformCall(st, null, tasks, cancellationToken, uiDefaultErrorHandler);
        }

        void PerformCall(IScheduledTask scheduledTask, object state, Queue<IScheduledTask> tasks, CancellationToken cancellationToken, Action<Exception> uiDefaultErrorHandler)
        {
            var d = scheduledTask.CreateDescription();
            var t = d.TaskFactory.StartNew(() => d.Func(state));

            t.ContinueWith(pt =>
            {
                bool isC = pt.Status == TaskStatus.Canceled;
                if (!isC)
                {
                    if (pt.Result.Exception != null)
                    {
                        if (uiDefaultErrorHandler != null)
                        {
                            UiTaskFactory.StartNew(() => uiDefaultErrorHandler(pt.Result.Exception));
                        }

                        return;
                    }
                }

                if (tasks.Count == 0) return;

                var st = tasks.Dequeue();
                PerformCall(st, isC ? null : pt.Result.Result, tasks, cancellationToken, uiDefaultErrorHandler);
            });
        }
    }
}
