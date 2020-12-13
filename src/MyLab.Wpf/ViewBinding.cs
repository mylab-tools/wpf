using System;

namespace MyLab.Wpf
{
    /// <summary>
    /// Determines binding form View to ViewModel
    /// </summary>
    public class ViewBinding
    {
        /// <summary>
        /// View type
        /// </summary>
        public Type View { get; set; }
        /// <summary>
        /// ViewModel type
        /// </summary>
        public Type ViewModel { get; set; }
    }
}