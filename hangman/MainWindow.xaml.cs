using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



namespace hangman
{
    public partial class MainWindow : Window
    {
        private List<Label> ListofLabel;
        private List<Canvas> ListofBar;
        private List<UIElement> TheHangMan;
        private string WordNow;
        private int WrongGuesses;
        private readonly int MaximumGuess = 6;
        List<string> charWord = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            ListofLabel = new List<Label>();
            ListofBar = new List<Canvas>();

            CharList.Items.Clear();
            for (char i = 'А'; i <= 'Я'; ++i)
            {
                _ = CharList.Items.Add(i);
            }

            int Space = 0;
            SelectWord();
            foreach (char c in WordNow)
            {
                Label lbl = new Label();
                Canvas can = new Canvas
                {
                    Background = Brushes.Black,
                    Width = 20,
                    Height = 3,
                    Margin = new Thickness(-200 + Space, -300, 0, 0)
                };
                Grid.SetRow(can, 1);
                Grid.SetColumn(can, 1);
                _ = Grid.Children.Add(can);

                lbl.Margin = new Thickness(-200 + Space, 100, 0, 0);
                lbl.Visibility = Visibility.Hidden;
                lbl.Width = 60;
                lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                lbl.FontSize = 30;
                lbl.Content = c;

                Grid.SetRow(lbl, 1);
                Grid.SetColumn(lbl, 1);
                _ = Grid.Children.Add(lbl);

                ListofLabel.Add(lbl);
                ListofBar.Add(can);
                Space += 100;
            }

            TheHangMan = new List<UIElement>() { HangHead, HangBody, HangRHand, HangLHand, HangRLeg, HangLLeg };
            foreach (UIElement ele in TheHangMan)
            {
                ele.Visibility = Visibility.Hidden;
            }
            WrongGuesses = 0;
        }

        private void SelectWord()
        {
            string filepath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "word_list.txt");
            using (TextReader tr = new StreamReader(filepath, Encoding.ASCII))
            {
                Random r = new Random();
                var allWords = tr.ReadToEnd().Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                WordNow = allWords[r.Next(0, allWords.Length - 1)];
            }
            for (int c = 0; c < WordNow.Length; c++)
            {
                charWord.Add(WordNow[c].ToString());
            }
        }

        private bool HasWon(List<Label> Lol)
        {
            bool HasHidden = false;
            foreach (Label l in Lol)
            {
                if (l.Visibility == Visibility.Hidden)
                {
                    HasHidden = true;
                }
            }
            return !HasHidden;
        }

        private void ClearTable()
        {
            foreach (Label u in ListofLabel)
            {
                u.Content = "";
            }

            foreach (Canvas c in ListofBar)
            {
                c.Visibility = Visibility.Hidden;
            }
        }

        private void GuessButtonClick(object sender, RoutedEventArgs e)
        {
            bool flag = false;

            if (CharList.SelectedItem != null && WrongGuesses < MaximumGuess)
            {
                foreach (Label l in ListofLabel)
                {
                    if ((char)l.Content == (char)CharList.SelectedItem)
                    {
                        l.Visibility = Visibility.Visible;
                        flag = true;
                    }
                }

                if (!flag)
                {
                    DrawOneStep();
                }
                if (CharList.SelectedItem != null)
                {
                    CharList.Items.Remove(CharList.SelectedItem);
                }
            }

            if (HasWon(ListofLabel))
            {
                _ = MessageBox.Show("Победа!");
                ClearTable();
            }
        }


        private void DrawOneStep()
        {
            if (WrongGuesses < MaximumGuess)
            {
                if (TheHangMan[WrongGuesses].Visibility == Visibility.Hidden)
                {
                    TheHangMan[WrongGuesses].Visibility = Visibility.Visible;
                    WrongGuesses++;
                    if (WrongGuesses >= MaximumGuess)
                    {
                        _ = MessageBox.Show("Игра окончена!");
                        _ = MessageBox.Show("Было загадано слово " + WordNow);
                        ClearTable();
                    }
                }
            }
        }
    }
}
