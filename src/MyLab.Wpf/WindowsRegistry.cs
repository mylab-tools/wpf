using System;
using System.Collections.Generic;
using System.Windows;

namespace MyLab.Wpf
{
    class WindowsRegistry
    {
        private readonly IDictionary<ViewModel, Window> _vmToControlMap = new Dictionary<ViewModel, Window>();

        public void Register( Window window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            var dc = window.DataContext;

            if(dc == null)
                throw new InvalidOperationException("Window without data context");
            if (!(dc is DialogVm dialogDc))
                throw new InvalidOperationException($"Data context is not '{typeof(DialogVm).FullName}'");

            _vmToControlMap.Add(dialogDc, window);

            window.Closed += (sender, args) =>
            {
                var w = (Window) sender;
                if (w.DataContext is DialogVm dialogVm)
                {
                    switch (w.DialogResult)
                    {
                        case true:
                            dialogVm.OnClosedPositive();
                            break;
                        case null:
                            dialogVm.OnClosedCancel();
                            break;
                        default:
                            dialogVm.OnClosedNegative();
                            break;
                    }
                }

                Unregister(w);
            };
        }

        public void Unregister( Window window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            var dc = window.DataContext;

            if (dc == null)
                throw new InvalidOperationException("Window without data context");
            if (!(dc is DialogVm dialogDc))
                throw new InvalidOperationException($"Data context is not '{typeof(DialogVm).FullName}'");

            _vmToControlMap.Remove(dialogDc);
        }

        public bool TryGetWindow(ViewModel dialogVm, out Window window)
        {
            return _vmToControlMap.TryGetValue(dialogVm, out window);
        }
    }
}