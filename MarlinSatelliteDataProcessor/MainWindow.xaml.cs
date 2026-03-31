using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MarlinSatelliteDataProcessor
{
    /// <summary>
    /// Represents the main window of the application, providing user interface functionality for loading, sorting, and
    /// searching sensor data using various algorithms.
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        LinkedList<double> sensorAList = new LinkedList<double>();
        LinkedList<double> sensorBList = new LinkedList<double>();
        bool isSorted = false;
        /// <summary>
        /// Handles the Click event of the Load Data (Galileo) button, triggering the display and processing of data in
        /// the UI.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data associated with the Click event.</param>
        private void btnLoadDataGalileo_Click(object sender, RoutedEventArgs e)
        {
            ShowAllData();
            NumberOfNodes();
        }
        /// <summary>
        /// Handles the Click event for the recursive search button for sensor A, performing a recursive binary search
        /// based on the user's input and updating the UI with the results.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data associated with the Click event.</param>
        private void btnSearchRecursiveA_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchHighlights(lstSensorA);
            Stopwatch sw = Stopwatch.StartNew();
            string input = txtSensorAInput.Text;
            int index = -1;
            lstSensorA.SelectedItems.Clear();
            // Check if input is an integer or a double and perform the appropriate search
            if (int.TryParse(input, out int intTarget))
            {
                int left = 0, right = sensorAList.Count - 1;

                index = BinarySearchRecursive(sensorAList, intTarget, left, right);
                HighlightRange(sensorAList, intTarget, lstSensorA);
            }
            // If the input is a double, perform a binary search for the double value
            else if (double.TryParse(input, out double doubleTarget))
            {
                int left = 0, right = sensorAList.Count - 1;
                index = BinarySearchRecursive(sensorAList, doubleTarget, left, right);
                // If the target is found, select it in the list box; otherwise, show a message that it was not found
                if (index == -1)
                    MessageBox.Show("Target not found.");
                else
                    lstSensorA.SelectedItems.Add(lstSensorA.Items[index]); ;
            }
            // If the input is neither an integer nor a double, show an error message
            else
            {
                MessageBox.Show("Invalid input. Please enter a number.");
                return;
            }
            sw.Stop();
            toolBar.Items.Clear();
            toolBar.Items.Add(new Label { Content = $"Recursive search of sensor A completed in {sw.ElapsedMilliseconds} ms" });
            txtRecSearchA.Text = $"{sw.ElapsedMilliseconds.ToString()} ms";
        }
        /// <summary>
        /// Handles the Click event for the Search Interactive A button, performing a search for the specified value in
        /// the sensor A list and highlighting the result.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data associated with the Click event.</param>
        private void btnSearchInterativeA_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchHighlights(lstSensorA);
            Stopwatch sw = Stopwatch.StartNew();
            // Check if the input can be parsed as a double; if not, show an error message and exit
            if (!double.TryParse(txtSensorAInput.Text, out double target))
            {
                MessageBox.Show("Invalid input. Please enter a number.");
                return;
            }
            // If the input can be parsed as an integer, perform a search for the integer value and highlight the range; otherwise, perform a binary search for the double value
            if (int.TryParse(txtSensorAInput.Text, out int intTarget))
            {
                HighlightRange(sensorAList, intTarget, lstSensorA);
            }
            // If the input is a double, perform a binary search for the double value and select it in the list box if found; otherwise, show a message that it was not found
            else
            {
                int index = BinarySearchIterative(sensorAList, target);
                // If the target is found, select it in the list box; otherwise, show a message that it was not found
                if (index != -1)
                    lstSensorA.SelectedItems.Add(lstSensorA.Items[index]);
                else
                    MessageBox.Show("Target not found.");
            }
            sw.Stop();
            toolBar.Items.Clear();
            toolBar.Items.Add(new Label { Content = $"Interactive search of sensor A completed in {sw.ElapsedMilliseconds} ms" });
            txtIntSearchA.Text = $"{sw.ElapsedMilliseconds.ToString()} ms";
        }
        /// <summary>
        /// Handles the Click event of the Sort A button to perform a selection sort on the sensor A list and display
        /// the elapsed time.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Sort A button.</param>
        /// <param name="e">The event data associated with the Click event.</param>
        private void btnSortA_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            SelectionSort(sensorAList);
            sw.Stop();
            toolBar.Items.Clear();
            toolBar.Items.Add(new Label { Content = $"Selection sort of sensor A completed in {sw.ElapsedMilliseconds} ms" });
            txtSelSortA.Text = $"{sw.ElapsedMilliseconds.ToString()} ms";
        }
        /// <summary>
        /// Handles the Click event of the Insertion A button to perform an insertion sort on the sensor A data and
        /// display the elapsed time.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data associated with the Click event.</param>
        private void btnInsertionA_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            InsertionSort(sensorAList);
            sw.Stop();
            toolBar.Items.Clear();
            toolBar.Items.Add(new Label { Content = $"Insertion sort of sensor A completed in {sw.ElapsedMilliseconds} ms" });
            txtInsSortA.Text = $"{sw.ElapsedMilliseconds.ToString()} ms";
        }
        /// <summary>
        /// Handles the Click event of the Search (Recursive B) button, performing a recursive binary search on the
        /// sensor B list using the input value.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data associated with the Click event.</param>
        private void btnSearchRecursiveB_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchHighlights(lstSensorB);
            Stopwatch sw = Stopwatch.StartNew();
            string input = txtSensorBInput.Text;
            int index = -1;
            lstSensorB.SelectedItems.Clear();
            // Check if input is an integer or a double and perform the appropriate search
            if (int.TryParse(input, out int intTarget))
            {
                int left = 0, right = sensorBList.Count - 1;
                index = BinarySearchRecursive(sensorBList, intTarget, left, right);
                if (index == -1) MessageBox.Show("Target not found.");
                else HighlightRange(sensorBList, intTarget, lstSensorB);
            }
            else if (double.TryParse(input, out double doubleTarget))
            {
                int left = 0, right = sensorBList.Count - 1;
                index = BinarySearchRecursive(sensorBList, doubleTarget, left, right);
                // If the target is found, select it in the list box; otherwise, show a message that it was not found
                if (index != -1) lstSensorB.SelectedItems.Add(lstSensorB.Items[index]);
                else MessageBox.Show("Target not found.");
            }
            // If the input is neither an integer nor a double, show an error message
            else
            {
                MessageBox.Show("Invalid input. Please enter a number.");
                return;
            }
            sw.Stop();
            toolBar.Items.Clear();
            toolBar.Items.Add(new Label { Content = $"Recursive search of sensor B completed in {sw.ElapsedMilliseconds} ms" });
            txtRecSearchB.Text = $"{sw.ElapsedMilliseconds.ToString()} ms";

        }
        /// <summary>
        /// Handles the Click event of the Search button for sensor B, performing an interactive search based on the
        /// user's input and displaying the result.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Search button.</param>
        /// <param name="e">The event data associated with the Click event.</param>
        private void btnSearchInterativeB_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchHighlights(lstSensorB);
            Stopwatch sw = Stopwatch.StartNew();
            // Check if the input can be parsed as a double; if not, show an error message and exit
            if (!double.TryParse(txtSensorBInput.Text, out double target))
            {
                MessageBox.Show("Invalid input. Please enter a number.");
                return;
            }
            // If the input can be parsed as an integer, perform a search for the integer value and highlight the range; otherwise, perform a binary search for the double value
            if (int.TryParse(txtSensorBInput.Text, out int intTarget))
            {
                HighlightRange(sensorBList, intTarget, lstSensorB);
            }
            // If the input is a double, perform a binary search for the double value and select it in the list box if found; otherwise, show a message that it was not found
            else
            {
                int index = BinarySearchIterative(sensorBList, target);
                // If the target is found, select it in the list box; otherwise, show a message that it was not found
                if (index != -1)
                    lstSensorB.SelectedItems.Add(lstSensorB.Items[index]);
                else
                    MessageBox.Show("Target not found.");
            }
            sw.Stop();
            toolBar.Items.Clear();
            toolBar.Items.Add(new Label { Content = $"Interactive search of sensor B completed in {sw.ElapsedMilliseconds} ms" });
            txtIntSearchB.Text = $"{sw.ElapsedMilliseconds.ToString()} ms";
        }
        /// <summary>
        /// Handles the Click event of the Sort B button to perform a selection sort on the sensor B list and display
        /// the elapsed time.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Sort B button.</param>
        /// <param name="e">The event data associated with the Click event.</param>
        private void btnSortB_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            SelectionSort(sensorBList);
            sw.Stop();
            toolBar.Items.Clear();
            toolBar.Items.Add(new Label { Content = $"Selection sort of sensor B completed in {sw.ElapsedMilliseconds} ms" });
            txtSelSortB.Text = $"{sw.ElapsedMilliseconds.ToString()} ms";
        }
        /// <summary>
        /// Handles the Click event of the Insertion Sort button for sensor B, performing an insertion sort on the
        /// sensor B data and displaying the elapsed time.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data associated with the Click event.</param>
        private void btnInsertionB_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            InsertionSort(sensorBList);
            sw.Stop();
            toolBar.Items.Clear();
            toolBar.Items.Add(new Label { Content = $"Insertion sort of sensor B completed in {sw.ElapsedMilliseconds} ms" });
            txtInsSortB.Text = $"{sw.ElapsedMilliseconds.ToString()} ms";
        }
        /// <summary>
        /// Attempts to load sensor data using the values provided in the Mu and Sigma input fields. Prompts the user to
        /// enter valid values or use defaults if the input is invalid.
        /// </summary>
        /// <returns>true if the data is successfully loaded; otherwise, false.</returns>
        private bool LoadData()
        {
            int mu, sigma;
            const int SIZE = 400;
            // Validate the input for Mu and Sigma, ensuring they are within the specified ranges.
            // If the input is invalid, prompt the user to enter valid values or use defaults.
            while (!int.TryParse(txtMu.Text, out mu) || mu < 35 || mu > 75 ||
                   !int.TryParse(txtSigma.Text, out sigma) || sigma < 10 || sigma > 20)
            {
                var result = MessageBox.Show(
                    "Please enter valid numeric values:\nMu (35-75) and Sigma (10-20).\n\nSet defaults (Mu=50, Sigma=10)?",
                    "Warning",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                // If the user chooses to set defaults, assign default values to Mu and Sigma; otherwise, exit the method without loading data.
                if (result == MessageBoxResult.Yes)
                {
                    mu = 50;
                    sigma = 10;
                    txtMu.Text = "50";
                    txtSigma.Text = "10";
                }
                else
                {
                    return false;
                }
            }
            var readData = new Galileo6.ReadData();
            var sensorAData = readData.SensorA(mu, sigma);
            var sensorBData = readData.SensorB(mu, sigma);

            sensorAList.Clear();
            sensorBList.Clear();
            // Load the sensor data into the respective linked lists for Sensor A and Sensor B using the provided Mu and Sigma values.
            for (int i = 0; i < SIZE; i++)
            {
                sensorAList.AddLast(readData.SensorA(mu, sigma));
                sensorBList.AddLast(readData.SensorB(mu, sigma));
            }

            isSorted = false;
            return true;
        }
        /// <summary>
        /// Displays all available sensor data in the associated list controls after loading the data.
        /// </summary>
        private void ShowAllData()
        {
            // Attempt to load the data; if loading fails, exit the method without displaying data.
            if (!LoadData())
                return;

            DisplayListBoxData();

            lstViewSatelliteData.Items.Clear();

            var a = sensorAList.ToArray();
            var b = sensorBList.ToArray();
            int n = Math.Min(a.Length, b.Length);
            // Populate the ListView with the sensor data, creating a new SatelliteRow for each pair of sensor values and adding it to the ListView.
            for (int i = 0; i < n; i++)
            {
                lstViewSatelliteData.Items.Add(new SatelliteRow { SensorA = a[i], SensorB = b[i] });
            }
        }
        /// <summary>
        /// Calculates the number of nodes in both Sensor A and Sensor B lists and updates the toolbar display
        /// accordingly.
        /// </summary>
        /// <returns>A tuple containing the number of nodes in Sensor A as the first element and the number of nodes in Sensor B
        /// as the second element.</returns>
        private (int countA, int countB) NumberOfNodes()
        {
            int countA = sensorAList.Count;
            int countB = sensorBList.Count;
            toolBar.Items.Clear();
            toolBar.Items.Add(new Label { Content = $"Sensor A Nodes: {countA}" });
            toolBar.Items.Add(new Label { Content = $"Sensor B Nodes: {countB}" });
            return (countA, countB);
        }
        /// <summary>
        /// Clears and repopulates the items in the sensor data list boxes with the current values from the sensor data
        /// collections, formatted to four decimal places.
        /// </summary>
        private void DisplayListBoxData()
        {
            lstSensorA.Items.Clear();
            lstSensorB.Items.Clear();
            foreach (var data in sensorAList)
                lstSensorA.Items.Add(data.ToString("F4"));
            foreach (var data in sensorBList)
                lstSensorB.Items.Add(data.ToString("F4"));
        }
        /// <summary>
        /// Sorts the elements of the specified linked list of doubles in ascending order using the selection sort
        /// algorithm.
        /// </summary>
        /// <param name="list">The linked list of double values to sort. Must not be null.</param>
        /// <returns>true if the list was sorted; otherwise, false if the list was empty.</returns>
        private bool SelectionSort(LinkedList<double> list)
        {
            // Check if the list is empty; if it is, display an error message and return false to indicate that sorting was not performed.
            if (list.Count == 0)
            {
                MessageBox.Show("The list is empty. Please load data before sorting.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            int min = 0;
            int max = list.Count - 1;
            // Perform selection sort by iterating through the list,
            // finding the minimum element in the unsorted portion,
            // and swapping it with the first element of the unsorted portion until the entire list is sorted.
            for (int i = 0; i <= max; i++)
            {
                min = i;
                // Find the index of the minimum element in the unsorted portion of the list
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list.ElementAt(j) < list.ElementAt(min))
                        min = j;
                }
                LinkedListNode<double> minNode = list.Find(list.ElementAt(min));
                LinkedListNode<double> currentNode = list.Find(list.ElementAt(i));
                var temp = minNode.Value;
                minNode.Value = currentNode.Value;
                currentNode.Value = temp;
            }
            DisplayListBoxData();
            return isSorted = true;
        }
        /// <summary>
        /// Sorts the elements of the specified linked list of doubles in ascending order using the insertion sort
        /// algorithm.
        /// </summary>
        /// <param name="list">The linked list of double values to sort. Must not be empty.</param>
        /// <returns>true if the list was sorted; otherwise, false if the list was empty.</returns>
        private bool InsertionSort(LinkedList<double> list)
        {
            if (list.Count == 0) 
            {
                MessageBox.Show("The list is empty. Please load data before sorting.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            int max = list.Count;
            // Perform insertion sort by iterating through the list and inserting each element into its correct position in the sorted portion of the list until the entire list is sorted.
            for (int i = 0; i <= max - 1; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    if (list.ElementAt(j - 1) > list.ElementAt(j))
                    {
                        LinkedListNode<double> nodeA = list.Find(list.ElementAt(j - 1));
                        LinkedListNode<double> nodeB = list.Find(list.ElementAt(j));
                        var temp = nodeA.Value;
                        nodeA.Value = nodeB.Value;
                        nodeB.Value = temp;
                    }
                    else break;
                }
            }
            DisplayListBoxData();
            return isSorted = true;
        }
        /// <summary>
        /// Performs a recursive binary search for a specified value within a sorted linked list of doubles.
        /// </summary>
        /// <param name="list">The sorted linked list of double values to search.</param>
        /// <param name="target">The value to locate within the list.</param>
        /// <param name="min">The zero-based starting index of the range to search.</param>
        /// <param name="max">The zero-based ending index of the range to search.</param>
        /// <returns>The zero-based index of the target value if found; otherwise, -1.</returns>
        private int BinarySearchRecursive(LinkedList<double> list, double target, int min, int max)
        {
            // Check if the list is sorted before performing the binary search; if it is not sorted, display an error message and return -1 to indicate that the search cannot be performed.
            if (!isSorted)
            {
                MessageBox.Show("Please sort the list before performing a binary search.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            // If the minimum index exceeds the maximum index, it means the target value is not present in the list, so return -1 to indicate that the search was unsuccessful.
            if (min > max)
                return -1;
            int mid = (min + max) / 2;
            // Retrieve the value at the midpoint index and compare it to the target value using a tolerance for equality. If the values are approximately equal, return the midpoint index;
            // if the target value is less than the midpoint value, recursively search the left half of the list; otherwise, recursively search the right half of the list.
            double midValue = list.ElementAt(mid);
            if (Math.Abs(midValue - target) < 0.0001)
                return mid;
            else if (target < midValue)
                return BinarySearchRecursive(list, target, min, mid - 1);
            else
                return BinarySearchRecursive(list, target, mid + 1, max);
        }
        /// <summary>
        /// Searches for the specified target value within the linked list using an iterative binary search algorithm.
        /// </summary>
        /// <param name="list">The sorted linked list of double values to search.</param>
        /// <param name="target">The value to locate within the list.</param>
        /// <returns>The zero-based index of the target value if found; otherwise, -1.</returns>
        private int BinarySearchIterative(LinkedList<double> list, double target)
        {
            int min = 0;
            int max = list.Count - 1;
            // Check if the list is sorted before performing the binary search; if it is not sorted, display an error message and return -1 to indicate that the search cannot be performed.
            if (!isSorted)
            {
                MessageBox.Show("Please sort before searching", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            // Perform an iterative binary search by repeatedly calculating the midpoint index and comparing the value at that index to the target value until the target is found or the search range is exhausted.
            while (min <= max)
            {
                int mid = (min + max) / 2;
                double midValue = list.ElementAt(mid);
                // If the value at the midpoint index is approximately equal to the target value, return the midpoint index;
                // if the target value is less than the midpoint value, update the maximum index to search the left half of the list;
                // otherwise, update the minimum index to search the right half of the list.
                if (Math.Abs(midValue - target) < 0.0001)
                    return mid;
                else if (target < midValue)
                    max = mid - 1;
                else
                    min = mid + 1;
            }
            return -1;
        }
        /// <summary>
        /// Highlights items in the specified list that fall within the range starting at the target value and displays
        /// them in the provided ListBox.
        /// </summary>
        /// <param name="list">The linked list of double values to be displayed and evaluated for highlighting.</param>
        /// <param name="target">The lower bound of the range. Items greater than or equal to this value and less than this value plus one
        /// are highlighted.</param>
        /// <param name="box">The ListBox control in which the items are displayed and highlighted.</param>
        private void HighlightRange(LinkedList<double> list, double target, ListBox box)
        {
            box.Items.Clear();
            ListBoxItem? firstMatch = null;
            // Iterate through the linked list of double values, creating a ListBoxItem for each value and adding it to the ListBox.
            // If a value falls within the specified range (greater than or equal to the target and less than the target plus one),
            // its background is set to light green, and the first matching item is stored for later use in scrolling.
            foreach (var item in list)
            {
                ListBoxItem listItem = new ListBoxItem();
                listItem.Content = item.ToString("F4");
                if (item >= target && item < target + 1)
                {
                    listItem.Background = Brushes.LightGreen;
                    firstMatch ??= listItem;
                }
                box.Items.Add(listItem);
            }
            // If a matching item was found, scroll it into view and bring it to the forefront of the ListBox; otherwise,
            // display a message indicating that no values were found in the specified range.
            if (firstMatch != null)
            {
                box.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Loaded, () =>
                {
                    box.ScrollIntoView(firstMatch);
                    firstMatch.BringIntoView();
                });
            }
            else
                MessageBox.Show($"No values found in the range {target}.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// Removes any background highlighting from all items in the specified ListBox.
        /// </summary>
        /// <param name="box">The ListBox whose item highlights are to be cleared. Cannot be null.</param>
        private void ClearSearchHighlights(ListBox box)
        {
            // Check if the ListBox contains items and if the first item is a ListBoxItem;
            // if so, iterate through the items and set their background to white to clear any existing highlights.
            if (box.Items.Count > 0 && box.Items[0] is ListBoxItem)
            {
                foreach (var listItem in box.Items)
                {
                    if (listItem is ListBoxItem lbItem)
                    {
                        lbItem.Background = Brushes.White;
                    }
                }
            }
            // If the ListBox does not contain ListBoxItem objects, it is assumed that the items are simple strings, and no background clearing is necessary.
            else
            {
                foreach (var listItem in box.Items)
                {
                    if (listItem is ListBoxItem lbItem)
                    {
                        lbItem.Background = Brushes.White;
                    }
                }
            }
        }
    }
}

