using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
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

        public TextBlock activeCell;
        public SpeechSynthesizer debugger;
        public TextBlock activeRow;
        public TextBlock activeColumn;
        public int columnsPerRow = 21;
        public int rowsPerColumn = 20;
        public MainWindow()
        {
            InitializeComponent();

            activeCell = startCell;
            debugger = new SpeechSynthesizer();
            activeRow = startRow;
            activeColumn = startColumn;
        }

        private void HoverRowHandler(object sender, MouseEventArgs e)
        {
            TextBlock currentCell = (TextBlock)sender;
            currentCell.Background = System.Windows.Media.Brushes.LightGreen;
            
        }

        private void HoutRowHandler(object sender, MouseEventArgs e)
        {
            TextBlock currentCell = (TextBlock)sender;
            if(activeRow == ((TextBlock)currentCell) || activeColumn == ((TextBlock)currentCell)) {
                ((TextBlock)currentCell).Background = System.Windows.Media.Brushes.DarkGray;
            }
            else
            {
                ((TextBlock)currentCell).Background = System.Windows.Media.Brushes.LightGray;
            }
                
        }

        private void SelectCellHandler(object sender, MouseButtonEventArgs e)
        {
            activeCell.Background = System.Windows.Media.Brushes.Transparent;
            activeCell = (TextBlock)sender;
            activeCell.Background = System.Windows.Media.Brushes.Green;
            int actualColumn = Grid.GetColumn(activeCell);
            int actualRow = Grid.GetRow(activeCell);
            /*debugger.Speak("строчка " + actualRow);
            debugger.Speak("колонка" + actualColumn);*/
            activeRow.Background = System.Windows.Media.Brushes.LightGray;
            activeColumn.Background = System.Windows.Media.Brushes.LightGray;
            activeColumn = ((TextBlock)paper.Children[actualColumn]);
            activeColumn.Background = System.Windows.Media.Brushes.DarkGray;
            activeRow = ((TextBlock)paper.Children[columnsPerRow * (actualRow + 1)]);
            activeRow.Background = System.Windows.Media.Brushes.DarkGray;
            formula.Text = activeCell.Text;
        }

        private void InputFormulaHandler(object sender, TextCompositionEventArgs e)
        {
            activeCell.Text = formula.Text;
        }
    }
}
