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

namespace Mini_games1
{

    public partial class MainWindow : Window
    {
        readonly Game Game;

        public MainWindow()
        {
            InitializeComponent();
            Game = new Game(myCanvas);
            Game.Array(3);
            StartGame();
            
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            int pos = Convert.ToInt16(((Button)sender).Tag);
            Game.Movement(pos);
            Refresh();
            if (Game.Pos_check() is true)
            {
                MessageBox.Show("Победа!");
            }
        }

        private Button Button(int pos)
        {
            switch (pos)
            {
                case 0: return B0;
                case 1: return B1;  
                case 2: return B2;
                case 3: return B3;
                case 4: return B4;
                case 5: return B5;
                case 6: return B6;
                case 7: return B7;
                case 8: return B8;
                default: return null;
            }
        }

        private void StartGame()
        {
            Game.Start();
            for (int i = 0; i < 100; i++) 
                Game.Random();
            Refresh();
        }

        private void Refresh()
        {
            for (int pos = 0; pos < 9; pos++) 
            { 
                if (Game.Get_number(pos) < 1)
                {
                    Button(pos).Visibility = Visibility.Hidden;
                }
                else
                {
                    Button(pos).Visibility = Visibility.Visible;
                }
            }
        }
    }
}
