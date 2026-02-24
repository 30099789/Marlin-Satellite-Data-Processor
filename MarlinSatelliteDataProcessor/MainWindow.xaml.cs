using System;
using System.Collections.Generic;
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
        private void btnLoadDataGalileo_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtMu.Text, out int mu) && int.TryParse(txtSigma.Text, out int sigma))
            {
                LoadData();
            }
            else
            {
                MessageBox.Show("Please enter valid numeric values for Mu and Sigma.");
            }
        }

        // Added to satisfy XAML-generated hookup that expects a Button_Click handler
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Forward to the existing handler so both names work
            btnLoadDataGalileo_Click(sender, e);
        }

        private void btnSearchRecursiveA_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearchInterativeA_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSortA_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnInsertionA_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearchRecursiveB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearchInterativeB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSortB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnInsertionB_Click(object sender, RoutedEventArgs e)
        {

        }
        private void LoadData()
        {
            if (int.TryParse(txtMu.Text, out int mu) || mu >= 35 || mu <= 75 )
                if(int.TryParse(txtSigma.Text, out int sigma) || sigma >= 10 || sigma >= 20)
                {
                    var readData = new Galileo6.ReadData();
                }
            else
            {
                MessageBox.Show("Please enter valid numeric values for Mu and Sigma.");
            }
        }
        private void ShowAllData()
        {

        }
        private void NumberOfNodes()
        {

        }
        private void DisplayListBoxData()
        {
        }
        private bool SelectionSort(LinkedList<double> list)
        {
            return true;
        }
        private bool InsertionSort(LinkedList<double> list)
        {
            return true;
        }
        private bool BinarySearchRecursive(LinkedList<double> list, double target)
        {
            return true;
        }
        private bool BinarySearchIterative(LinkedList<double> list, double target)
        {
            return true;
        }

    }
}
