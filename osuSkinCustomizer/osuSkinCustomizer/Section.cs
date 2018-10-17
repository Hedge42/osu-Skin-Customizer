using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace osuSkinCustomizer
{
    public class Section
    {
        // Controls
        public TreeViewItem item;
        public CheckBox checkBox;
        public TextBlock textBlock;

        public List<Section> children;

        public Func<string[], string[]> searchFunction;
        public Skinini.Option iniType = Skinini.Option.None;

        public Section(string text, Section parent, Func<string[], string[]> _searchFunction)
        {
            children = new List<Section>();

            checkBox = new CheckBox();
            checkBox.Checked += OnCheck;
            checkBox.Unchecked += OnCheck;

            textBlock = new TextBlock();
            textBlock.Text = text;

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Children.Add(checkBox);
            sp.Children.Add(textBlock);

            item = new TreeViewItem();
            item.Header = sp;

            if (parent != null)
                parent.Add(this);

            searchFunction = _searchFunction;
        }
        public Section(string text, Section parent, Skinini.Option type)
        {
            children = new List<Section>();

            checkBox = new CheckBox();
            checkBox.Checked += OnCheck;
            checkBox.Unchecked += OnCheck;

            textBlock = new TextBlock();
            textBlock.Text = text;

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Children.Add(checkBox);
            sp.Children.Add(textBlock);

            item = new TreeViewItem();
            item.Header = sp;

            if (parent != null)
                parent.Add(this);

            iniType = type;
        }
        public Section(string text, Section parent)
        {
            children = new List<Section>();

            // checkBox = new CheckBox();
            // checkBox.Checked += OnCheck;
            // checkBox.Unchecked += OnCheck;

            textBlock = new TextBlock();
            textBlock.Text = text;

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            // sp.Children.Add(checkBox);
            sp.Children.Add(textBlock);

            item = new TreeViewItem();
            item.Header = sp;

            if (parent != null)
                parent.Add(this);
        }

        public void Add(Section s)
        {
            item.Items.Add(s.item);
            children.Add(s);
        }

        private void OnCheck(object sender, RoutedEventArgs e)
        {
            // https://stackoverflow.com/questions/20494740/collapseall-items-of-a-treeview-within-wpf-application

            // Automatically expand?
            // item.IsExpanded = (bool)cb.IsChecked;

            // find each checkbox under this, recursively...
            foreach (Section s in children)
            {
                // Does this trigger the event?
                s.checkBox.IsChecked = (bool)checkBox.IsChecked;
            }

            Importer.UpdateDetailsAction.Invoke();

            // TODO: Check if ANY of the children are checked
            // if a child IS checked, but a parent is NOT...
            // check the parent, but ONLY the parent
            // and...
            // if NO child is checked, but the parent IS...
            // uncheck the parent
        }
    }
}
