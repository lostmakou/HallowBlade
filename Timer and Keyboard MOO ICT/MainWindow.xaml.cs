﻿using System;
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

namespace ZeldaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Player Player;
        MapRender mr;
        HashSet<Key> keys = new HashSet<Key>();
        Sword sword;

        DispatcherTimer gameTimer = new DispatcherTimer();


        public MainWindow()
        {
            InitializeComponent();
            mr = new MapRender(myCanvas);
            Player = new Player(myCanvas, mr);
            
            
            myCanvas.Focus();

            gameTimer.Tick += GameTimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Start();
            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;

        }

        

        private void GameTimerEvent(object sender, EventArgs e)
        {

            //mr.Render(Player.Area);
            Player.PrintInfo();
            if (sword != null)
                if (sword.Update())
                    sword = null;


            //Canvas.SetLeft(box, Canvas.GetLeft(box) - speed);

            //if (Canvas.GetLeft(box) < 5 || Canvas.GetLeft(box) + (box.Width * 2) > Application.Current.MainWindow.Width)
            //{
            //    speed = -speed;
            //}AccessTexAp
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            keys.Add(e.Key);
            MovePlayer();
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
           keys.Remove(e.Key);
        }

        private void MovePlayer()
        {
            
            foreach (Key key in keys)
            {
                if (key == Key.A)
                    Player.Move(true, false, false, false);
                if (key == Key.D)
                    Player.Move(false, true, false, false);
                if (key == Key.W)
                    Player.Move(false, false, true, false);
                if (key == Key.S)
                    Player.Move(false, false, false, true);
                if (sword == null)
                {
                    if (key == Key.Up)
                        sword = new Sword((int)Canvas.GetLeft(Player.rect), (int)Canvas.GetTop(Player.rect), Direction.Up, myCanvas);
                    if (key == Key.Down)
                        sword = new Sword((int)Canvas.GetLeft(Player.rect), (int)Canvas.GetTop(Player.rect), Direction.Down, myCanvas);
                    if (key == Key.Left)
                        sword = new Sword((int)Canvas.GetLeft(Player.rect), (int)Canvas.GetTop(Player.rect), Direction.Left, myCanvas);
                    if (key == Key.Right)
                        sword = new Sword((int)Canvas.GetLeft(Player.rect), (int)Canvas.GetTop(Player.rect), Direction.Right, myCanvas);
                }

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            myCanvas.Visibility = Visibility.Visible;
            outer_music.IsMuted = false;
        }

        /*private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                goLeft = true;
            }
            if (e.Key == Key.D)
            {
                goRight = true;
            }
            if (e.Key == Key.W)
            {
                goUp = true;
            }
            if (e.Key == Key.S)
            {
                goDown = true;
            }
            if (e.Key == Key.X)
                Player.Area = (0, 1);
  
            if (e.Key == Key.C)
                Player.Area = (0, 0);
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                goLeft = false;
            }
            if (e.Key == Key.D)
            {
                goRight = false;
            }
            if (e.Key == Key.W)
            {
                goUp = false;
            }
            if (e.Key == Key.S)
            {
                goDown = false;
            }
        }*/
    }
}
