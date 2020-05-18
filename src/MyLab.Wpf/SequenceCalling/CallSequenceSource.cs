using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyLab.Wpf.SequenceCalling
{
    public class CallSequenceSource 
    {
        protected IReadOnlyList<IScheduledTask> Tasks { get; }

        protected TaskFactory UiTaskFactory { get; }
        protected TaskFactory BizTaskFactory { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CallSequenceSource"/>
        /// </summary>
        public CallSequenceSource(TaskFactory uiTaskFactory, TaskFactory bizTaskFactory,
            IEnumerable<IScheduledTask> tasks)
        {
            UiTaskFactory = uiTaskFactory;
            BizTaskFactory = bizTaskFactory;
            Tasks = new ReadOnlyCollection<IScheduledTask>(tasks.ToList());
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CallSequenceSource"/>
        /// </summary>
        public CallSequenceSource(TaskFactory uiScheduler, TaskFactory bizTaskFactory)
            : this(uiScheduler, bizTaskFactory, Enumerable.Empty<IScheduledTask>())
        {
        }

        //public CallSequence<TRes> DoBiz<TRes>(Task<TRes> task, Action<Exception> uiErrorHandler)
        //{
        //    return new CallSequence<TRes>(
        //        UiTaskFactory,
        //        BizTaskFactory,
        //        GetTaskList(new BizScheduledTask<TRes>
        //        {
        //            TaskCreator = () => task,
        //            BizTaskFactory = BizTaskFactory,
        //            UiErrorHandler = uiErrorHandler,
        //            UiTaskFactory = UiTaskFactory
        //        }));
        //}


        public CallSequence<TRes> DoBiz<TRes>(Func<Task<TRes>> taskCreator, Action<Exception> uiErrorHandler)
        {
            return new CallSequence<TRes>(
                UiTaskFactory,
                BizTaskFactory,
                GetTaskList(new BizScheduledTask<TRes>
                {
                    TaskCreator = taskCreator,
                    BizTaskFactory = BizTaskFactory,
                    UiErrorHandler = uiErrorHandler,
                    UiTaskFactory = UiTaskFactory
                }));
        }

        public CallSequence<TRes> DoBiz<TRes>(Func<TRes> func, Action<Exception> uiErrorHandler)
        {
            return new CallSequence<TRes>(
                UiTaskFactory,
                BizTaskFactory,
                GetTaskList(new BizScheduledTask<TRes>
                {
                     Func = func,
                    BizTaskFactory = BizTaskFactory,
                    UiErrorHandler = uiErrorHandler,
                    UiTaskFactory = UiTaskFactory
                }));
        }

        public CallSequence DoBiz(Action act, Action<Exception> uiErrorHandler)
        {
            return new CallSequence(
                UiTaskFactory,
                BizTaskFactory,
                GetTaskList(new BizScheduledTask<object>
                {
                    Func = () =>
                    {
                        act();
                        return null;
                    },
                    BizTaskFactory = BizTaskFactory,
                    UiErrorHandler = uiErrorHandler,
                    UiTaskFactory = UiTaskFactory
                }));
        }
        public CallSequence<TRes> DoBiz<T, TRes>(Func<T, TRes> func, Action<Exception> uiErrorHandler)
        {
            return new CallSequence<TRes>(
                UiTaskFactory,
                BizTaskFactory,
                GetTaskList(new BizScheduledTask<T, TRes>
                {
                    Func = func,
                    BizTaskFactory = BizTaskFactory,
                    UiErrorHandler = uiErrorHandler,
                    UiTaskFactory = UiTaskFactory
                }));
        }



        protected IEnumerable<IScheduledTask> GetTaskList(IScheduledTask newTask)
        {
            return new List<IScheduledTask>(Tasks) { newTask };
        }
    }
}