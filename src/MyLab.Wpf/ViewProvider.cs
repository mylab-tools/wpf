using System;
using System.Windows.Controls;

namespace MyLab.Wpf
{
    class ViewProvider : IViewProvider
    {
        private readonly Control _control;

        public ViewProvider(Control control)
        {
            _control = control;
        }

        public I Provide<I>() where I : class
        {
            if(!typeof(I).IsInterface)
                throw new InvalidOperationException("Only interfaces are supported");

            return _control as I;
        }
    }
}