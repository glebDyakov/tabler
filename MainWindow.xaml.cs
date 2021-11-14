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
using Microsoft.CodeAnalysis.CSharp.Scripting;

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
            formula.Focus();
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
            if(activeCell.Text.Length >= 1 && activeCell.Text[0] == '=')
            {
                //debugger.Speak("это ячейка формула");
                string expression = activeCell.Text.Substring(1);
                int repeats = 0;
                foreach (char expressionSymbol in expression)
                {
                    if (expressionSymbol == '=')
                    {
                        repeats++;
                    }
                }
                if (repeats == 0)
                {
                    double result = CSharpScript.EvaluateAsync<double>(expression).Result;
                    activeCell.Text = result.ToString();
                }
            }
            activeCell.Background = System.Windows.Media.Brushes.Transparent;
            activeCell = (TextBlock)sender;
            activeCell.Background = System.Windows.Media.Brushes.Green;
            int actualColumn = Grid.GetColumn(activeCell);
            int actualRow = Grid.GetRow(activeCell);
            activeRow.Background = System.Windows.Media.Brushes.LightGray;
            activeColumn.Background = System.Windows.Media.Brushes.LightGray;
            activeColumn = ((TextBlock)paper.Children[actualColumn]);
            activeColumn.Background = System.Windows.Media.Brushes.DarkGray;
            activeRow = ((TextBlock)paper.Children[columnsPerRow * (actualRow + 1)]);
            activeRow.Background = System.Windows.Media.Brushes.DarkGray;
            formula.Text = activeCell.Text;
        }


        private void InputFormulaHandler(object sender, TextChangedEventArgs e)
        {
            activeCell.Text = formula.Text;
        }

        private void SelectCells(object sender, MouseButtonEventArgs e)
        {
            //debugger.Speak("Выделаяю колонку или строчку");
            activeCell = (TextBlock)sender;
            int actualColumn = Grid.GetColumn(activeCell);
            int actualRow = Grid.GetRow(activeCell);
            activeColumn = ((TextBlock)paper.Children[actualColumn]);
            // выделяем строчки
            for (int rowIndex = 2; rowIndex < paper.ColumnDefinitions.Count; rowIndex++)
            {
                ((TextBlock)paper.Children[columnsPerRow * (actualRow + 1) * (rowIndex - 1)]).Background = System.Windows.Media.Brushes.DarkGray;
            }
            // сбрасываем выделенные колонки
            for (int rowIndex = 1; rowIndex < paper.ColumnDefinitions.Count; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < paper.RowDefinitions.Count - 1; columnIndex++)
                {
                    if (columnIndex == 0)
                    {
                        // нужно раскрашивать шапку колокни
                        ((TextBlock)paper.Children[rowIndex - 0 + 21 * columnIndex]).Background = System.Windows.Media.Brushes.LightGray;
                    }
                    else
                    {
                        ((TextBlock)paper.Children[rowIndex - 0 + 21 * columnIndex]).Background = System.Windows.Media.Brushes.White;
                    }
                }
            }
            // выделяем колонки
            for (int columnIndex = 1; columnIndex < paper.RowDefinitions.Count; columnIndex++)
            {
                ((TextBlock)paper.Children[paper.Children.IndexOf(activeCell) - 0 + 21 * (columnIndex - 1)]).Background = System.Windows.Media.Brushes.DarkGray;
            }
        }

        private void SelectReverseCells(object sender, MouseButtonEventArgs e)
        {
            activeCell = (TextBlock)sender;
            int actualColumn = Grid.GetColumn(activeCell);
            int actualRow = Grid.GetRow(activeCell);
            activeColumn = ((TextBlock)paper.Children[actualColumn]);
            // выделяем колонки
            for (int rowIndex = 1; rowIndex < paper.ColumnDefinitions.Count; rowIndex++)
            {
                ((TextBlock)paper.Children[rowIndex]).Background = System.Windows.Media.Brushes.DarkGray;
            }
            for (int rowIndex = 0; rowIndex < paper.ColumnDefinitions.Count; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < paper.RowDefinitions.Count - 1; columnIndex++)
                {
                    if (columnIndex == 0)
                    {
                        // нужно раскрашивать шапку колокни
                        ((TextBlock)paper.Children[rowIndex - 0 + 21 * columnIndex]).Background = System.Windows.Media.Brushes.DarkGray;
                    }
                    else
                    {
                        if (rowIndex == 0)
                        {
                            // нужно раскрашивать шапку колокни
                            ((TextBlock)paper.Children[rowIndex - 0 + 21 * columnIndex]).Background = System.Windows.Media.Brushes.LightGray;
                        }
                        else
                        {
                            ((TextBlock)paper.Children[rowIndex - 0 + 21 * columnIndex]).Background = System.Windows.Media.Brushes.White;
                        }
                    }
                }
            }
            // выделяем строчки
            for (int rowIndex = 0; rowIndex < paper.RowDefinitions.Count; rowIndex++)
            {
                ((TextBlock)paper.Children[(actualRow + 0) * (rowsPerColumn + 1) + rowIndex]).Background = System.Windows.Media.Brushes.DarkGray;
            }
        }

        private void CancelBtnHandler(object sender, MouseButtonEventArgs e)
        {
            formula.Text = "";
            Keyboard.ClearFocus();
        }

        private void ApplyBtnHandler(object sender, MouseButtonEventArgs e)
        {

        }

        private void InsertFormulaBtnHandler(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
