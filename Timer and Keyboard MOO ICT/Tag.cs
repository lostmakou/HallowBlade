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
using System.Windows.Threading;

namespace Mini_games1
{

    public partial class CanvasHandlerTag
    {
        readonly Tag Game;
        Canvas canvas, mainCanvas;
        DispatcherTimer mainTimer;
        Button B0, B1, B2, B3, B4, B5, B6, B7, B8;
        public bool isWin = false; 
        public CanvasHandlerTag(Canvas canvas, Canvas mainCanvas, DispatcherTimer mainTimer)
        {
            this.canvas = canvas;
            this.mainCanvas = mainCanvas;
            this.mainTimer = mainTimer;
            B0 = canvas.FindName("B0") as Button;
            B1 = canvas.FindName("B1") as Button;
            B2 = canvas.FindName("B2") as Button;
            B3 = canvas.FindName("B3") as Button;
            B4 = canvas.FindName("B4") as Button;
            B5 = canvas.FindName("B5") as Button;
            B6 = canvas.FindName("B6") as Button;
            B7 = canvas.FindName("B7") as Button;
            B8 = canvas.FindName("B8") as Button;
            B0.Click += ButtonClick; 
            B1.Click += ButtonClick; 
            B2.Click += ButtonClick;
            B3.Click += ButtonClick;
            B4.Click += ButtonClick;
            B5.Click += ButtonClick;
            B6.Click += ButtonClick;
            B7.Click += ButtonClick;
            B8.Click += ButtonClick;

            Game = new Tag(canvas);
            Game.Array(3);
            //StartGame();
            
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            int pos = Convert.ToInt16(((Button)sender).Tag);
            Game.Movement(pos);
            Refresh();
            if (Game.Pos_check() is true)
            {
                //MessageBox.Show("Победа!");
                isWin = true;
                //gameTimer.Stop();
                canvas.Visibility = Visibility.Collapsed;
                mainCanvas.Visibility = Visibility.Visible;
                mainTimer.Start();

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

        public void StartGame()
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
