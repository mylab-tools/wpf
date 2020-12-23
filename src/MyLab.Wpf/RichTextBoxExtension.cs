using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MyLab.Wpf
{
    public static class RichTextBoxExtension
    {
        public static readonly DependencyProperty TextBlocksProperty =
            DependencyProperty.RegisterAttached("TextBlocks", typeof(IEnumerable<Block>), typeof(RichTextBoxExtension),
                new PropertyMetadata(default(IEnumerable<Block>), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var tb = (RichTextBox)dependencyObject;
            tb.Document.Blocks.Clear();

            if (dependencyPropertyChangedEventArgs.NewValue != null)
            {
                tb.Document.Blocks.AddRange((IEnumerable<Block>)dependencyPropertyChangedEventArgs.NewValue);
            }
        }

        public static IEnumerable<Block> GetTextBlocks(RichTextBox instance)
        {
            return (IEnumerable<Block>)instance.GetValue(TextBlocksProperty);
        }

        public static void SetTextBlocks(RichTextBox instance, IEnumerable<Block> value)
        {
            instance.SetValue(TextBlocksProperty, value);
        }
    }
}
