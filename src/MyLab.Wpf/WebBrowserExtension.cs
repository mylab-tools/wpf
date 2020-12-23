using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MyLab.Wpf
{
    public static class WebBrowserExtension
    {
        public static readonly DependencyProperty SourceHtmlProperty =
            DependencyProperty.RegisterAttached("SourceHtml", typeof(string), typeof(WebBrowserExtension),
                new PropertyMetadata(default(string), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var wb = (WebBrowser) dependencyObject;
            if (dependencyPropertyChangedEventArgs.NewValue != null)
                wb.NavigateToString((string)dependencyPropertyChangedEventArgs.NewValue);
            else
            {
                wb.Source = null;
            }
        }

        public static string GetSourceHtml(WebBrowser instance)
        {
            return (string)instance.GetValue(SourceHtmlProperty);
        }

        public static void SetSourceHtml(WebBrowser instance, string value)
        {
            instance.SetValue(SourceHtmlProperty, value);
        }

        public static void HideScriptErrors(this WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }
    }
}
