using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            int left = Math.Min(0, sensorAList.Count - 1);
            int right = Math.Max(0, sensorAList.Count - 1);
            BinarySearchRecursive(sensorAList, int.Parse(txtSensorAInput.Text), left, right);

        }

        private void btnSearchInterativeA_Click(object sender, RoutedEventArgs e)
        {
            BinarySearchIterative(sensorAList, int.Parse(txtSensorAInput.Text));
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
            var left = Math.Min(0, sensorBList.Count - 1);
            var right = Math.Max(0, sensorBList.Count - 1);
            BinarySearchRecursive(sensorBList, int.Parse(txtSensorBInput.Text), left, right);
        }

        private void btnSearchInterativeB_Click(object sender, RoutedEventArgs e)
        {
            var left = Math.Min(0, sensorBList.Count - 1);
            var right = Math.Max(0, sensorBList.Count - 1);
            BinarySearchRecursive(sensorBList, int.Parse(txtSensorBInput.Text), left, right);
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
            LoadData();
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

        private void NumberOfNodes()
        {
            int countA = sensorAList.Count;
            int countB = sensorBList.Count;
            mainToolBar.Items.Clear();
            mainToolBar.Items.Add(new System.Windows.Controls.Label { Content = $"Sensor A Nodes: {countA}" });
            mainToolBar.Items.Add(new System.Windows.Controls.Label { Content = $"Sensor B Nodes: {countB}" });
        }
        private void DisplayListBoxData()
        {
            lstSensorA.Items.Clear();
            lstSensorB.Items.Clear();
            foreach (var data in sensorAList) lstSensorA.Items.Add(data.ToString("F3"));
            foreach (var data in sensorBList) lstSensorB.Items.Add(data.ToString("F3"));
        }
        private bool SelectionSort(LinkedList<double> list)
        {
            if (list.Count == 0) return false;
            int min = 0;
            int max = list.Count - 1;
            for (int i = 0; i < max; i++)
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
            int min = 0;
            int max = list.Count - 1;
            for (int i = 1; i <= max; i++)
            {
                min = i;
                for (int j = i + 1; j > 0; j--)
                {
                    if (list.ElementAt(j - 1) > list.ElementAt(j))
                    {
                        LinkedListNode<double> currentNode = list.Find(list.ElementAt(j));
                        LinkedListNode<double> previousNode = list.Find(list.ElementAt(j - 1));
                        var temp = currentNode.Value;
                        currentNode.Value = previousNode.Value;
                        previousNode.Value = temp;

                    }
                    else
                        break;
                }
            }
            DisplayListBoxData();
            return isSorted = true;
        }
        private int BinarySearchRecursive(LinkedList<double> list, int target, int left, int right)
        {
            if (isSorted)
            {
                if (left > right)
                    return -1;
                int mid = left + (right - left) / 2;
                var midValue = list.ElementAt(mid);
                if (Math.Abs(midValue - target) < 0.1)
                    return mid;
                else if (midValue < target)
                    return BinarySearchRecursive(list, target, mid + 1, right);
                else
                    return BinarySearchRecursive(list, target, left, mid - 1);
            }
            else
            {
                MessageBox.Show("Please sort the list before performing a binary search.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }

        }
        private int BinarySearchIterative(LinkedList<double> list, double target)
        {
            if (isSorted)
            {
                int left = 0, right = list.Count - 1;
                while (left <= right)
                {
                    int mid = left + (right - left) / 2;
                    var midValue = list.ElementAt(mid);
                    if (midValue == target)
                        return mid;
                    else if (midValue < target)
                        left = mid + 1;
                    else
                        right = mid - 1;
                }
                return -1;
            }
            else
            {
                MessageBox.Show("Please sort the list before performing a binary search.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;

            }
        }
    }
}
