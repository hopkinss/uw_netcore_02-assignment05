using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assignment05
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum Player { X=1,O };
        private bool isX = true;
        private int[,] board;

        public MainWindow()
        {
            InitializeComponent();
            board = new int[3, 3];
            SetTurnIndicator();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var square = ((Button)sender);
            if (square.Content == null)
            {
                square.Content = isX ? "X" : "O";
                var pos = square.Tag.ToString().Split(',').Select(x => int.Parse(x)).ToArray();
                board[pos[0], pos[1]] = isX ? 1 : 2;
                isX = !isX;
                SetTurnIndicator();
                HasWinner();
            }
        }

        private void SetTurnIndicator()
        {
            uxTurn.Text = $"{(isX ? "X" : "O")}'s Turn";
        }

        private bool HasWinner()
        {
            var winner = string.Empty;
            bool isWinner = false;

            // check if anyone won
            if (TryCheckBoard(out winner))
            {
                
                uxTurn.Text = $"The winner is {winner}";
                uxGrid.IsEnabled = false;
                return isWinner;
            }


            return false;
        }

        private bool TryCheckBoard(out string winner)
        {
            winner = string.Empty;
            bool isWinner = false ;

            // Vertical and horizontal
            for (int k = 0; k < board.GetLength(1); k++)
            {
                var vrow = new List<int>();
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    // evaluate each horizontal row
                    var hrow = new List<int>();
                    for (int i = 0; i < board.GetLength(0); i++)
                    {
                        hrow.Add(board[j, i]);
                    }

                    isWinner = IsRowWin(hrow);
                    winner = isWinner ? ((Player)hrow[0]).ToString() : string.Empty;
                    if (isWinner)
                        return true;

                    vrow.Add(board[j, k]);
                }

                // evaluate each vertical row
                isWinner = IsRowWin(vrow);
                winner = isWinner ? ((Player)vrow[0]).ToString() : string.Empty;
                if (isWinner)
                    return true;
            }

            // backslash diagonal
            var test = new List<int>() { board[0, 0], board[1, 1], board[2, 2] };
            if (IsRowWin(new List<int>() { board[0, 0], board[1, 1], board[2, 2] }))
            {
                winner = ((Player)board[0,0]).ToString();
                return true;
            }
           
            // Forwardslash diagonal
            if (IsRowWin(new List<int>() { board[0, 2], board[1, 1], board[2, 0] }))
            {
                winner = ((Player)board[0, 2]).ToString();
                return true;
            }

            return false ;
        }

        private bool IsRowWin(List<int> row)
        {
            // if row has a zero it cant win
            if (row.Aggregate((a, x) => a * x) == 0)
                return false;

            // if an element doenst match previous element, didnt win
            for (int i = 1; i < row.Count; i++)
            {
                if (row[i - 1] != row[i])
                    return false;
            }
            return true;
        }

        private void uxExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void uxNewGame_Click(object sender, RoutedEventArgs e)
        {
            foreach(var c in uxGrid.Children)
            {
                if (c.GetType() == typeof(Button))
                {
                    ((Button)c).Content = null;
                }
            }

            // reset board
            Array.Clear(board, 0, board.Length);
            isX = true;
            uxGrid.IsEnabled = true;
            SetTurnIndicator();
        }
    }
}
