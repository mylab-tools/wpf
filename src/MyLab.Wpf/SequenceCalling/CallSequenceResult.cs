using System;

namespace MyLab.Wpf.SequenceCalling
{
    public struct CallSequenceResult
    {
        public object Result { get; }
        public Exception Exception { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CallSequenceResult"/>
        /// </summary>
        public CallSequenceResult(object result, Exception exception)
        {
            Result = result;
            Exception = exception;
        }
    }
}