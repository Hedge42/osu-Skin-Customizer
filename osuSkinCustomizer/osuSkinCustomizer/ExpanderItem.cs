using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace osuSkinCustomizer
{
    public class ExpanderItem
    {
        private string path;
        private Action onClick;

        public ExpanderItem(StackPanel parent, string content, string _path, Action _onClick)
        {
            path = _path;
            onClick = _onClick;
            Button b = new Button();
            b.Content = content;
            b.Click += ButtonEventHandler;
            parent.Children.Add(b);
        }

        private void ButtonEventHandler(object sender, RoutedEventArgs e)
        {
            if (onClick != null)
                onClick.Invoke();
        }
    }
}
