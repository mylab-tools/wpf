using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MyLab.Wpf
{
    public static class ComboBoxExtension
    {
        public static readonly DependencyProperty UpdateSourceWhenEnterProperty =
            DependencyProperty.RegisterAttached("UpdateSourceWhenEnter",
                typeof(bool), typeof(ComboBoxExtension), new PropertyMetadata(false,
                    OnUpdateSourceChanged));

        private static void OnUpdateSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBox comboBox = d as ComboBox;

            if (comboBox == null)
                return;

            if ((bool)e.OldValue)
            {
                comboBox.KeyDown -= ComboBox_KeyDown;
                comboBox.SelectionChanged -= ComboBox_SelectionChanged;
            }

            if ((bool)e.NewValue)
            {
                comboBox.KeyDown += ComboBox_KeyDown;
                comboBox.SelectionChanged += ComboBox_SelectionChanged;
            }
        }

        static void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSource((ComboBox)sender);
        }

        static void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                UpdateSource((ComboBox)sender);
        }

        public static bool GetUpdateSourceWhenEnter(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdateSourceWhenEnterProperty);
        }

        public static void SetUpdateSourceWhenEnter(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdateSourceWhenEnterProperty, value);
        }

        static void UpdateSource(ComboBox comboBox)
        {
            BindingExpression binding = comboBox.GetBindingExpression(ComboBox.TextProperty);
            if (binding != null)
                binding.UpdateSource();
        }
    }
}
