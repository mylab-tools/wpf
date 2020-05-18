using System;
using System.Threading.Tasks;

namespace MyLab.Wpf.SequenceCalling
{
    class UiScheduledCallTask<T> : IScheduledTask
    {
        public TaskFactory UiTaskScheduler { get; set; }

        public Action<T> Act { get; set; }

        ScheduledCallDescription IScheduledTask.CreateDescription()
        {
            return new ScheduledCallDescription
            {
                Func = (state) =>
                {
                    try
                    {
                        Act((T) state);
                    }
                    catch(Exception e)
                    {
                        return new CallSequenceResult(null, e);
                    }

                    return new CallSequenceResult(null, null);
                },
                TaskFactory = UiTaskScheduler
            };
        }
    }

    class UiScheduledCallTask : IScheduledTask
    {
        public TaskFactory UiTaskScheduler { get; set; }

        public Action Act { get; set; }

        ScheduledCallDescription IScheduledTask.CreateDescription()
        {
            return new ScheduledCallDescription
            {
                Func = (state) =>
                {
                    try
                    {
                        Act();
                    }
                    catch (Exception e)
                    {
                        return new CallSequenceResult(null, e);
                    }

                    return new CallSequenceResult(null, null);
                },
                TaskFactory = UiTaskScheduler
            };
        }
    }
}