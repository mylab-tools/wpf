using System.Windows;
using System.Windows.Controls;

namespace MyLab.Wpf
{
    public static class TreeViewExtension
    {
        public static readonly DependencyProperty SelectedItemExProperty =
            DependencyProperty.RegisterAttached("SelectedItemEx",
                typeof(object), typeof(TreeViewExtension),
                new PropertyMetadata(string.Empty, OnSelectedItemExChanged));
        public static object GetSelectedItemEx(DependencyObject dp)
        {
            return dp.GetValue(SelectedItemExProperty);
        }

        public static void SetSelectedItemEx(DependencyObject dp, object value)
        {
            dp.SetValue(SelectedItemExProperty, value);
        }

        private static void OnSelectedItemExChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var treeView = (TreeView)sender;

            treeView.SelectedItemChanged -= TreeView_SelectedItemChanged;
            treeView.SelectedItemChanged += TreeView_SelectedItemChanged;
        }

        static void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetSelectedItemEx((TreeView)sender, e.NewValue);
        }
    }
}
