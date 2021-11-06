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

namespace tabler
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HoverRowHandler(object sender, MouseEventArgs e)
        {
            TextBlock currentCell = (TextBlock)sender;
            currentCell.Background = System.Windows.Media.Brushes.LightGreen;
            
        }

        private void HoutRowHandler(object sender, MouseEventArgs e)
        {
            TextBlock currentCell = (TextBlock)sender;
            ((TextBlock)currentCell).Background = System.Windows.Media.Brushes.LightGray;
        }
    }
}
