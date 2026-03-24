using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MarlinSatelliteDataProcessor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
        private void btnLoadDataGalileo_Click(object sender, RoutedEventArgs e)
        {
            ShowAllData();
            NumberOfNodes();

        }

        private void btnSearchRecursiveA_Click(object sender, RoutedEventArgs e)
        {
            double target = double.Parse(txtSensorAInput.Text);
            int left = 0, right = sensorAList.Count - 1;
            int index = BinarySearchRecursive(sensorAList, target, left, right);

            lstSensorA.UnselectAll();

            if (index != -1)
            {
                lstSensorA.SelectedItems.Add(lstSensorA.Items[index]);
            }
            else
            {
                MessageBox.Show("Target not found.");
            }
        }

        private void btnSearchInterativeA_Click(object sender, RoutedEventArgs e)
        {
            double target = double.Parse(txtSensorAInput.Text);

            int index = BinarySearchIterative(sensorAList, target);

            lstSensorA.UnselectAll();

            if (index != -1)
            {
                lstSensorA.SelectedItems.Add(lstSensorA.Items[index]);
            }
            else
            {
                MessageBox.Show("Target not found.");
            }
        }


        private void btnSortA_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorAList);
        }

        private void btnInsertionA_Click(object sender, RoutedEventArgs e)
        {
            InsertionSort(sensorAList);
        }

        private void btnSearchRecursiveB_Click(object sender, RoutedEventArgs e)
        {
            string input = txtSensorBInput.Text;
            int index = -1;
            lstSensorB.SelectedItems.Clear();
            if (int.TryParse(input, out int intTarget))
            {
                int left = 0, right = sensorBList.Count - 1;
                Stopwatch sw = Stopwatch.StartNew();
                index = BinarySearchRecursive(sensorBList, intTarget, left, right);
                sw.Stop();
                HighlightRange(sensorBList, intTarget, lstSensorB);
            }
            else if (double.TryParse(input, out double doubleTarget))
            {
                int left = 0, right = sensorBList.Count - 1;
                Stopwatch sw = Stopwatch.StartNew();
                index = BinarySearchRecursive(sensorBList, doubleTarget, left, right);
                lstSensorB.SelectedItems.Add(lstSensorB.Items[index]);
                sw.Stop();
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a number.");
                return;
            }
        }

        private void btnSearchInterativeB_Click(object sender, RoutedEventArgs e)
        {
            double target = double.Parse(txtSensorBInput.Text), min = target, max = target + 1;


        }

        private void btnSortB_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorBList);

        }

        private void btnInsertionB_Click(object sender, RoutedEventArgs e)
        {
            InsertionSort(sensorBList);
        }
        private bool LoadData()
        {
            int mu, sigma;
            const int SIZE = 400;
            while (!int.TryParse(txtMu.Text, out mu) || mu < 35 || mu > 75 ||
                   !int.TryParse(txtSigma.Text, out sigma) || sigma < 10 || sigma > 20)
            {
                var result = MessageBox.Show(
                    "Please enter valid numeric values:\nMu (35-75) and Sigma (10-20).\n\nSet defaults (Mu=50, Sigma=10)?",
                    "Warning",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

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

            for (int i = 0; i < SIZE; i++)
            {
                sensorAList.AddLast(readData.SensorA(mu, sigma));
                sensorBList.AddLast(readData.SensorB(mu, sigma));
            }

            isSorted = false;
            return true;
        }
        private void ShowAllData()
        {
            if (!LoadData())
                return;

            // ListBoxes
            DisplayListBoxData();

            // ListView
            lstViewSatelliteData.Items.Clear();

            var a = sensorAList.ToArray();
            var b = sensorBList.ToArray();
            int n = Math.Min(a.Length, b.Length);

            for (int i = 0; i < n; i++)
            {
                lstViewSatelliteData.Items.Add(new SatelliteRow { SensorA = a[i], SensorB = b[i] });
            }
        }
        private (int countA, int countB) NumberOfNodes()
        {
            int countA = sensorAList.Count;
            int countB = sensorBList.Count;
            mainToolBar.Items.Clear();
            mainToolBar.Items.Add(new Label { Content = $"Sensor A Nodes: {countA}" });
            mainToolBar.Items.Add(new Label { Content = $"Sensor B Nodes: {countB}" });
            return (countA, countB);
        }
        private void DisplayListBoxData()
        {
            lstSensorA.Items.Clear();
            lstSensorB.Items.Clear();
            foreach (var data in sensorAList) lstSensorA.Items.Add(data.ToString("F4"));
            foreach (var data in sensorBList) lstSensorB.Items.Add(data.ToString("F4"));
        }
        private bool SelectionSort(LinkedList<double> list)
        {
            if (list.Count == 0) return false;
            int min = 0;
            int max = list.Count - 1;
            for (int i = 0; i <= max; i++)
            {
                min = i;
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
        private bool InsertionSort(LinkedList<double> list)
        {
            if (list.Count == 0) return false;
            int max = list.Count;
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
        private int BinarySearchRecursive(LinkedList<double> list, double target, int left, int right)
        {
            if (!isSorted)
            {
                MessageBox.Show("Please sort the list before performing a binary search.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            else if (left <= right)
            {
                int mid = (left + right);
                if ((int)list.ElementAt(mid) == target)
                    return mid;
                else if (target < list.ElementAt(mid))
                    return BinarySearchRecursive(list, target, left, mid - 1);
                else
                    return BinarySearchRecursive(list, target, mid + 1, right);
            }
            return left;

        }
        private int BinarySearchIterative(LinkedList<double> list, double target)
        {
            int min = 0;
            int max = list.Count;
            if (!isSorted)
            {
                MessageBox.Show("Please sort before searching", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            while (min <= max - 1)
            {
                int mid = (min + max) / 2;
                if ((int)list.ElementAt(mid) == target)
                    return mid;
                else if (target < list.ElementAt(mid))
                    max = mid - 1;
                else
                    min = mid + 1;
            }
            return min;
        }
        private void HighlightRange(LinkedList<double> list, int target, ListBox box)
        {
            box.Items.Clear();
            foreach (var item in list)
            {
                ListBoxItem listItem = new ListBoxItem();
                listItem.Content = item;
                if ((int)item == target)
                {
                    listItem.Background = Brushes.Yellow;

                }
                box.Items.Add(listItem).ToString("F4");
            }
        }
    }
}

