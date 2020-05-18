using System;
using System.Threading.Tasks;

namespace MyLab.Wpf.SequenceCalling
{
    public class ScheduledCallDescription
    {
        public Func<object, CallSequenceResult> Func { get; set; }

        public TaskFactory TaskFactory { get; set; }
    }
}