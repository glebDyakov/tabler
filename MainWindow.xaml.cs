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
using System.Windows.Media.Effects;
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
        public Style outlineCellStyle = null;
        public Grid activePaper;
        public Border activePaperLabel;
        public Dictionary<Int32, String> columnLabels;

        public MainWindow()
        {
            InitializeComponent();

            activeCell = startCell;
            debugger = new SpeechSynthesizer();
            activeRow = startRow;
            activeColumn = startColumn;
            formula.Focus();
            activePaper = paper;
            activePaperLabel = paperLabel;
            columnLabels = new Dictionary<Int32, String> {
                {
                    1, "A"
                },
                {
                    2, "B"
                },
                {
                    3, "C"
                },
                {
                    4, "D"
                },
                {
                    5, "E"
                },
                {
                    6, "F"
                },
                {
                    7, "G"
                },
                {
                    8, "H"
                },
                {
                    9, "J"
                },
                {
                    10, "K"
                },
                {
                    11, "L"
                },
                {
                    12, "M"
                },
                {
                    13, "N"
                },
                {
                    14, "O"
                },
                {
                    15, "P"
                },
                {
                    16, "R"
                },
                {
                    17, "S"
                },
                {
                    18, "T"
                },
                {
                    19, "U"
                },
                {
                    20, "V"
                },
                {
                    21, "W"
                }
            };
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
            ResetReverseSelectCells();
            ResetSelectCells();
            if (activeCell.Text.Length >= 1 && activeCell.Text[0] == '=')
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

            /*Setter effectSetter = new Setter();
            effectSetter.Property = ScrollViewer.EffectProperty;
            effectSetter.Value = new DropShadowEffect
            {
                ShadowDepth = 4,
                Direction = 330,
                Color = Colors.Black,
                Opacity = 0.5,
                BlurRadius = 4
            };
            outlineCellStyle = new Style(typeof(TextBlock));
            outlineCellStyle.Setters.Add(effectSetter);
            ((TextBlock)activeCell).Resources.Add(typeof(TextBlock), outlineCellStyle);*/
            
            activeCell.Background = System.Windows.Media.Brushes.Green;
            int actualColumn = Grid.GetColumn(activeCell);
            int actualRow = Grid.GetRow(activeCell);
            activeRow.Background = System.Windows.Media.Brushes.LightGray;
            activeColumn.Background = System.Windows.Media.Brushes.LightGray;
            activeColumn = ((TextBlock)paper.Children[actualColumn]);
            activeColumn.Background = System.Windows.Media.Brushes.DarkGray;
            activeRow = ((TextBlock)paper.Children[columnsPerRow * (actualRow + 0)]);
            activeRow.Background = System.Windows.Media.Brushes.DarkGray;
            formula.Text = activeCell.Text;
            activeCellCoords.Content = columnLabels[actualColumn] + actualRow.ToString();
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
            ResetSelectCells();
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
            ResetReverseSelectCells();
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

        private void ResetSelectCells()
        {
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
        }

        private void ResetReverseSelectCells()
        {
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
        }

        private void changeColumnWidthHandler(object sender, RoutedEventArgs e)
        {
            Dialogs.ChangeColumnWidthDialog changeColumnWidthDialog = new Dialogs.ChangeColumnWidthDialog(activeCell, paper);
            changeColumnWidthDialog.Show();
        }

        private void changeRowHeightHandler(object sender, RoutedEventArgs e)
        {
            Dialogs.ChangeRowHeightDialog changeRowHeightDialog = new Dialogs.ChangeRowHeightDialog(activeCell, paper);
            changeRowHeightDialog.Show();
        }

        private void FillColorHandler(object sender, RoutedEventArgs e)
        {
            activeCell.Background = System.Windows.Media.Brushes.Red;
        }

        private void AddPaperHandler(object sender, RoutedEventArgs e)
        {

            this.ResetStyleFromPapersLabels();
            Border newPaperUnderline = new Border();
            newPaperUnderline.BorderBrush = System.Windows.Media.Brushes.DarkGreen;
            newPaperUnderline.BorderThickness = new Thickness(0, 0, 0, 2);
            TextBlock newPaperLabel = new TextBlock();
            string newPaperNumber = (papersList.Children.Count + 1).ToString();
            newPaperLabel.Text = "| Лист " + newPaperNumber;
            newPaperLabel.Width = 75;
            newPaperLabel.FontWeight = FontWeights.ExtraBold;
            newPaperLabel.Foreground = System.Windows.Media.Brushes.DarkGreen;
            papersList.Children.Add(newPaperUnderline);
            newPaperUnderline.Height = 20;
            newPaperUnderline.VerticalAlignment = VerticalAlignment.Top;
            newPaperUnderline.Child = newPaperLabel;
            newPaperUnderline.MouseEnter += SetPapersLabelHoverStyle;
            newPaperUnderline.MouseLeave += SetPapersLabelStandardStyle;
            newPaperUnderline.MouseUp += SelectPapersLabelHandler;

            ((TextBlock)newPaperUnderline.Child).Background = System.Windows.Media.Brushes.White;
            activePaperLabel = newPaperUnderline;

        }

        private void SetPapersLabelHoverStyle(object sender, MouseEventArgs e)
        {
            Border hoverablePapersLabel = (Border)sender;
            if (papersList.Children.IndexOf(activePaperLabel) != papersList.Children.IndexOf(hoverablePapersLabel))
            {
                ((TextBlock)hoverablePapersLabel.Child).FontWeight = FontWeights.ExtraBold;
            }
        }

        private void SetPapersLabelStandardStyle(object sender, MouseEventArgs e)
        {
            Border houtablePapersLabel = (Border)sender;
            if (papersList.Children.IndexOf(activePaperLabel) != papersList.Children.IndexOf(houtablePapersLabel))
            {
                ((TextBlock)houtablePapersLabel.Child).FontWeight = FontWeights.Normal;
            }
        }

        private void SelectPapersLabelHandler(object sender, MouseButtonEventArgs e)
        {
            this.ResetStyleFromPapersLabels();
            Border selectedPapersLabel = (Border)sender;
            ((TextBlock)selectedPapersLabel.Child).Background = System.Windows.Media.Brushes.White;
            ((TextBlock)selectedPapersLabel.Child).FontWeight = FontWeights.ExtraBold;
            ((TextBlock)selectedPapersLabel.Child).Foreground = System.Windows.Media.Brushes.DarkGreen;
            selectedPapersLabel.BorderThickness = new Thickness(0, 0, 0, 2);
            activePaperLabel = selectedPapersLabel;
        }

        private void ResetStyleFromPapersLabels()
        {
            foreach (Border papersListItem in papersList.Children)
            {
                ((TextBlock)papersListItem.Child).Background = System.Windows.Media.Brushes.Transparent;
                ((TextBlock)papersListItem.Child).Foreground = System.Windows.Media.Brushes.Black;
                ((TextBlock)papersListItem.Child).FontWeight = FontWeights.Normal;
                papersListItem.BorderThickness = new Thickness(0, 0, 0, 0);
            }
        }
    }
}
