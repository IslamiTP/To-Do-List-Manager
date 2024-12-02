using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace todolistmanagercsharp.Utils
{
    public static class GridViewSort
    {
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.RegisterAttached(
                "PropertyName",
                typeof(string),
                typeof(GridViewSort),
                new PropertyMetadata(null));

        public static string GetPropertyName(DependencyObject obj) =>
            (string)obj.GetValue(PropertyNameProperty);

        public static void SetPropertyName(DependencyObject obj, string value) =>
            obj.SetValue(PropertyNameProperty, value);

        public static readonly DependencyProperty AutoSortProperty =
            DependencyProperty.RegisterAttached(
                "AutoSort",
                typeof(bool),
                typeof(GridViewSort),
                new PropertyMetadata(false, OnAutoSortChanged));

        public static bool GetAutoSort(DependencyObject obj) =>
            (bool)obj.GetValue(AutoSortProperty);

        public static void SetAutoSort(DependencyObject obj, bool value) =>
            obj.SetValue(AutoSortProperty, value);

        private static void OnAutoSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListView listView)
            {
                if ((bool)e.NewValue)
                {
                    listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(OnColumnHeaderClick));
                }
                else
                {
                    listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(OnColumnHeaderClick));
                }
            }
        }

        private static void OnColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader header &&
                header.Column != null &&
                GetPropertyName(header.Column) is string propertyName &&
                sender is ListView listView)
            {
                var collectionView = CollectionViewSource.GetDefaultView(listView.ItemsSource);
                if (collectionView != null)
                {
                    var direction = ListSortDirection.Ascending;

                    if (collectionView.SortDescriptions.Count > 0 &&
                        collectionView.SortDescriptions[0].PropertyName == propertyName)
                    {
                        direction = collectionView.SortDescriptions[0].Direction == ListSortDirection.Ascending
                            ? ListSortDirection.Descending
                            : ListSortDirection.Ascending;
                    }

                    collectionView.SortDescriptions.Clear();
                    collectionView.SortDescriptions.Add(new SortDescription(propertyName, direction));
                }
            }
        }
    }
}