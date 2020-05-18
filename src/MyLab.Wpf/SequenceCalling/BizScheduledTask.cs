    using System;
using System.Threading.Tasks;

namespace MyLab.Wpf.SequenceCalling
{
    class BizScheduledTask<T> : IScheduledTask
    {
        public Func<Task<T>> TaskCreator { get; set; }
        public Func<T> Func { get; set; }
        public Action<Exception> UiErrorHandler { get; set; }
        public TaskFactory BizTaskFactory { get; set; }
        public TaskFactory UiTaskFactory { get; set; }
        public ScheduledCallDescription CreateDescription()
        {
            return new ScheduledCallDescription
            {
                Func = (state) =>
                {
                    T result = default;
                    Exception error = null;

                    if (TaskCreator != null)
                    {
                        var t = TaskCreator();

                        try
                        {
                            if (t.Status != TaskStatus.Created)
                                t.Wait();
                            else
                                t.RunSynchronously();

                            if (t.IsFaulted)
                                error = t.Exception;
                            else
                                result = t.Result;
                        }
                        catch (Exception e)
                        {
                            error = e;
                        }
                    }
                    else
                    {
                        try
                        {
                            result = Func();
                        }
                        catch (Exception e)
                        {
                            error = e;
                        }
                    }

                    if (error != null)
                    {
                        UiTaskFactory.StartNew(() => UiErrorHandler(error));
                    }

                    return new CallSequenceResult(result, error);
                },

                TaskFactory = BizTaskFactory
            };
        }
    }

    class BizScheduledTask<T, TRes> : IScheduledTask
    {
        public Func<T, TRes> Func { get; set; }
        public Action<Exception> UiErrorHandler { get; set; }
        public TaskFactory BizTaskFactory { get; set; }
        public TaskFactory UiTaskFactory { get; set; }
        public ScheduledCallDescription CreateDescription()
        {
            return new ScheduledCallDescription
            {
                Func = (state) =>
                {
                    TRes result;
                    try
                    {
                        result = Func((T) state);
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            UiErrorHandler(e);
                        }
                        catch 
                        {
                        }

                        return new CallSequenceResult(null, e);
                    }

                    return new CallSequenceResult(result, null);
                },

                TaskFactory = BizTaskFactory
            };
        }
    }
}