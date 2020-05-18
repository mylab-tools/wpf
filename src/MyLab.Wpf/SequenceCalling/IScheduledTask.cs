using System;

namespace MyLab.Wpf.SequenceCalling
{
    public interface IScheduledTask
    {
        ScheduledCallDescription CreateDescription();
    }
}