using System;
using System.Collections.Generic;

namespace MyLab.Wpf
{
    class VmCommandSubscribing
    {
        readonly List<WeakReference> _handlers = new List<WeakReference>();

        public void AddHandler(EventHandler handler)
        {
            System.Windows.Input.CommandManager.RequerySuggested += handler;
            _handlers.Add(new WeakReference(handler));
        }

        public void RemoveHandler(EventHandler handler)
        {
            for (int i = _handlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = _handlers[i];
                var existingHandler = reference.Target as EventHandler;
                if ((existingHandler == null) || (existingHandler == handler))
                    _handlers.RemoveAt(i);
            }

            System.Windows.Input.CommandManager.RequerySuggested -= handler;
        }

        public void CallHandlers()
        {
            var callees = new EventHandler[_handlers.Count];
            int count = 0;

            for (int i = _handlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = _handlers[i];
                var handler = reference.Target as EventHandler;
                if (handler == null)
                    _handlers.RemoveAt(i);
                else
                {
                    callees[count] = handler;
                    count++;
                }
            }

            for (int i = 0; i < count; i++)
            {
                EventHandler handler = callees[i];
                handler(null, EventArgs.Empty);
            }
        }
    }
}