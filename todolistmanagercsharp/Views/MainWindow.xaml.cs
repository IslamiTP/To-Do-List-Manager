using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using todolistmanagercsharp.ViewModels;

namespace todolistmanagercsharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Enables Sorting Capabilites to the Coloum Headers
        private GridViewColumnHeader _lastHeaderClicked;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;
        // --------------------------------------------------------------------


        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new TaskViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        // <summary>
        /// Handles sorting when a column header is clicked.
        /// </summary>
        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked == null || headerClicked.Tag == null)
            {
                Console.WriteLine("No header clicked or missing Tag.");
                return;
            }

            string sortBy = headerClicked.Tag.ToString();
            Console.WriteLine($"Column Header Clicked: {sortBy}");

            ListSortDirection direction = _lastDirection;

            // Toggle sorting direction if the same column is clicked
            if (_lastHeaderClicked == headerClicked)
            {
                direction = _lastDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
            }

            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;

            if (DataContext is TaskViewModel viewModel)
            {
                Console.WriteLine($"Sorting FilteredTasks by {sortBy} in {direction} order.");
                viewModel.SortTasks(sortBy, direction);
            }
            else
            {
                Console.WriteLine("DataContext is not TaskViewModel.");
            }
        }


        // <summary>
        // Sorts the ListView items by the specified column and direction.
        // </summary>
        // <param name="sortBy">The property to sort by.</param>
        // <param name="direction">The sort direction (ascending or descending).</param>
        //private void Sort(string sortBy, ListSortDirection direction)
        //{
        //    if (DataContext is TaskViewModel viewModel)
        //    {
        //        Console.WriteLine($"Sorting by: {sortBy}, Direction: {direction}");
        //        ICollectionView dataView = CollectionViewSource.GetDefaultView(viewModel.FilteredTasks);
        //        dataView.SortDescriptions.Clear();
        //        dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
        //        dataView.Refresh();
        //    }
        //    else
        //    {
        //        Console.WriteLine("DataContext is not TaskViewModel.");
        //    }
        //}


        // Testing class bellow


        //private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        //{
        //    var headerClicked = e.OriginalSource as GridViewColumnHeader;
        //    if (headerClicked == null || headerClicked.Tag == null)
        //    {
        //        Console.WriteLine("No header clicked or missing Tag.");
        //        return;
        //    }

        //    string sortBy = headerClicked.Tag.ToString();
        //    Console.WriteLine($"Column Header Clicked: {sortBy}");

        //    ListSortDirection direction = _lastDirection;

        //    // Toggle sorting direction if the same column is clicked
        //    if (_lastHeaderClicked == headerClicked)
        //    {
        //        direction = _lastDirection == ListSortDirection.Ascending
        //            ? ListSortDirection.Descending
        //            : ListSortDirection.Ascending;
        //    }

        //    _lastHeaderClicked = headerClicked;
        //    _lastDirection = direction;

        //    Sort(sortBy, direction);
        //}
    }
}
