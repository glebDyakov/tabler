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
using System.Windows.Shapes;

namespace tabler.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ChangeColumnWidthDialog.xaml
    /// </summary>
    public partial class ChangeColumnWidthDialog : Window
    {
        public Grid paper;
        public TextBlock activeCell;

        public ChangeColumnWidthDialog()
        {
            InitializeComponent();
        }

        public ChangeColumnWidthDialog(TextBlock activeCell, Grid paper)
        {
            InitializeComponent();

            this.activeCell = activeCell;
            this.paper = paper;

        }

        private void cancelHandler(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void okHandler(object sender, RoutedEventArgs e)
        {
            int digitalColumnWidth = int.Parse(columnWidth.Text);
            paper.ColumnDefinitions[Grid.GetColumn(activeCell)].Width = new GridLength(digitalColumnWidth);
            this.Close();
        }
    }
}
