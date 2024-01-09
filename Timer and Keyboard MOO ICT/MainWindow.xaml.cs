using hangman;
using Mini_games1;
using RPS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        Random random = new Random();
        Sword sword;

        public DispatcherTimer gameTimer = new DispatcherTimer();

        private CanvasHandlerRockPaperScissors RPS;
        private CanvasHandlerTag TAG;
        private CanvasHandlerHangman HANGMAN;

        Stopwatch stopwatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            mr = new MapRender(myCanvas);
            Player = new Player(myCanvas, mr);
            this.DataContext = Player;

            myCanvas.Focus();

            gameTimer.Tick += GameTimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(30);
            //gameTimer.Start();
            
            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;


            RPS = new CanvasHandlerRockPaperScissors(RockPaperScissors, myCanvas, gameTimer);
            TAG = new CanvasHandlerTag(Tag, myCanvas, gameTimer);
            HANGMAN = new CanvasHandlerHangman(Hangman, myCanvas, gameTimer);
        }

        

        private void GameTimerEvent(object sender, EventArgs e)
        {

            //mr.Render(Player.Area);
            Player.Update();
            //Player.PrintInfo();
            if (sword != null)
                if (sword.Update())
                    sword = null;
                else if (sword.isExist == true)
                {
                    Rect SwordHitBox = new Rect(Canvas.GetLeft(sword.rect), Canvas.GetTop(sword.rect), sword.rect.ActualWidth, sword.rect.ActualHeight);
                    foreach (var enemy in mr.enemies)
                    {
                        Rect EnemyHitBox = new Rect(Canvas.GetLeft(enemy.EnemyRectangle), Canvas.GetTop(enemy.EnemyRectangle), enemy.EnemyRectangle.ActualWidth, enemy.EnemyRectangle.ActualHeight);
                        if (SwordHitBox.IntersectsWith(EnemyHitBox))
                        {
                            enemy.Health -= 1;
                            //Console.WriteLine(enemy.Health.ToString());
                            if (enemy.Health <= 0)
                            {
                                if (enemy is Boss)
                                {
                                    var bl = new Block('→');
                                    myCanvas.Children.Add(bl.BlockRect);
                                    Canvas.SetLeft(bl.BlockRect, Canvas.GetLeft(enemy.EnemyRectangle));
                                    Canvas.SetTop(bl.BlockRect, Canvas.GetTop(enemy.EnemyRectangle));
                                    mr.blocks.Add(bl);
                                }
                                if (random.Next(0, 5) == 0)
                                {
                                    var bl = new Block('♥');
                                    myCanvas.Children.Add(bl.BlockRect);
                                    Canvas.SetLeft(bl.BlockRect, Canvas.GetLeft(enemy.EnemyRectangle));
                                    Canvas.SetTop(bl.BlockRect, Canvas.GetTop(enemy.EnemyRectangle));
                                    mr.blocks.Add(bl);
                                }
                                Player.Score += enemy.Score;
                                //Score.Text = $"Счет: {Player.Score}";
                                myCanvas.Children.Remove(enemy.EnemyRectangle);
                                mr.enemies.Remove(enemy);
                                break;
                            }
                            myCanvas.Children.Remove(sword.rect);
                            sword.isExist = false; 
                            break;
                        }
                    }
                }
            foreach (var en in mr.enemies)
            {
                Position pp = new Position();
                pp.X = (int)Canvas.GetLeft(Player.rect);
                pp.Y = (int)Canvas.GetTop(Player.rect);

                en.Update(pp);
                if (mr.tempEnemies.Count > 0)
                {
                    mr.enemies.AddRange(mr.tempEnemies);
                    mr.tempEnemies.Clear();
                    break;
                }
                if (en is Boss)
                {
                    var enemy = (Boss)en;
                    if (enemy.Health == 1)
                    {
                        gameTimer.Stop();
                        myCanvas.Visibility = Visibility.Collapsed;
                        RockPaperScissors.Visibility = Visibility.Visible;

                        RPS.Restart();
                        RPS.boss = enemy;
                        RPS.gameTimer.Start();
                        //if (RPS.isWin)
                        //{
                        //    enemy.Health = 5;   
                        //}
                        //else
                        //{
                        //    enemy.Health = 0;

                        //}
                    }
                }
            }

            if (Player.Health <= 0)
            {
                gameTimer.Stop();
                mr.RemoveObjects();
                myCanvas.Children.Remove(Player.rect);
                Player = new Player(myCanvas, mr);
                myCanvas.Visibility = Visibility.Collapsed;
                MainMenu.Visibility = Visibility.Visible;
                
            }
 
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
            if (gameTimer.IsEnabled)
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
                if (sword == null && Player.IsMatchedSword)
                {
                    if (key == Key.Up)
                        sword = new Sword((int)Canvas.GetLeft(Player.rect), (int)Canvas.GetTop(Player.rect), Direction.Up, myCanvas);
                    if (key == Key.Down)
                        sword = new Sword((int)Canvas.GetLeft(Player.rect), (int)Canvas.GetTop(Player.rect), Direction.Down, myCanvas);
                    if (key == Key.Left)
                        sword = new Sword((int)Canvas.GetLeft(Player.rect), (int)Canvas.GetTop(Player.rect), Direction.Left, myCanvas);
                    if (key == Key.Right)
                        sword = new Sword((int)Canvas.GetLeft(Player.rect), (int)Canvas.GetTop(Player.rect), Direction.Right, myCanvas);
                    Player.BringToFront();
                }
                if (key == Key.E)
                {
                    foreach (Block block in mr.blocks)
                    {
                        if (block.Type == '⎕')
                        {
                            if (Player.DistanceToBlock(block))
                            {
                                MessageText.Text = mr.notes[block.Id];
                                myCanvas.Visibility = Visibility.Collapsed;
                                gameTimer.Stop();
                                Message.Visibility = Visibility.Visible;

                            }
                        }
                        else if (block.Type == '♥')
                        {
                            if (Player.DistanceToBlock(block))
                            {
                                mr.blocks.Remove(block);
                                myCanvas.Children.Remove(block.BlockRect);
                                Player.Health += 1;
                                //Health.Text = $"Здоровье: {Player.Health}";
                                break;
                            }
                        }
                        else if (block.Type == '↑')
                        {
                            if (Player.DistanceToBlock(block))
                            {
                                mr.blocks.Remove(block);
                                myCanvas.Children.Remove(block.BlockRect);
                                Player.IsMatchedSword = true;
                                break;
                            }
                        }
                        else if (block.Type == '→')
                        {
                            if (Player.DistanceToBlock(block))
                            {
                                mr.blocks.Remove(block);
                                myCanvas.Children.Remove(block.BlockRect);
                                Player.IsMatchedKey = true;
                                break;
                            }
                        }
                        else if (block.Type == '☷' && Player.IsMatchedKey)
                        {
                            if (Player.DistanceToBlock(block))
                            {
                                gameTimer.Stop();
                                myCanvas.Visibility = Visibility.Collapsed;
                                Tag.Visibility = Visibility.Visible;
                                Tag.Focus();
                                TAG.StartGame();
                                    //gameTimer.Start();
                                myCanvas.Focus();
                                mr.blocks.Remove(block);
                                myCanvas.Children.Remove(block.BlockRect);

                                break;
                            }
                        }
                        else if (block.Type == '2')
                        {
                            if (Player.DistanceToBlock(block))
                            {
                                gameTimer.Stop();
                                myCanvas.Visibility = Visibility.Collapsed;
                                Message.Visibility = Visibility.Visible;
                                EndScreenPlayer.Visibility = Visibility.Visible;
                                OKMessage.Visibility = Visibility.Collapsed;
                                if (HANGMAN.isWin)
                                {
                                    MessageText.Text = "Молодец, ты спас еще одного героя.";
                                }
                                else
                                {
                                    AnoherPlayerEndPic.Visibility = Visibility.Collapsed;
                                    MessageText.Text = "Путь ты свой прошел, но один из путников не был таким удачливым и ты ему не помог.";
                                }
                                MessageText.Text += $"\nСчет: {Player.Score}";
                                MessageText.Text += $"\nВремя прохождения: {stopwatch.Elapsed.Minutes:D2}:{stopwatch.Elapsed.Seconds:D2}";
                                stopwatch.Stop();
                            }
                        }
                        else if (block.Type == '0')
                        {
                            if (Player.DistanceToBlock(block) && !HANGMAN.isPlayed)
                            {
                                gameTimer.Stop();
                                myCanvas.Visibility = Visibility.Collapsed;
                                Hangman.Visibility = Visibility.Visible;
                                HANGMAN.Start();
                                //myCanvas.Focus();
                            }
                        }
                    }
                }
                if (key == Key.Y)
                {
                    gameTimer.Stop();
                    myCanvas.Visibility = Visibility.Collapsed;
                    RockPaperScissors.Visibility = Visibility.Visible;
                    RPS.gameTimer.Start();
                }
                if (key == Key.U)
                {
                    gameTimer.Stop();
                    myCanvas.Visibility = Visibility.Collapsed;
                    Tag.Visibility = Visibility.Visible;

                }
                if (key == Key.I)
                {
                    gameTimer.Stop();
                    myCanvas.Visibility = Visibility.Collapsed;
                    Hangman.Visibility = Visibility.Visible;
                    //HANGMAN.Start();

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Message.Visibility = Visibility.Visible;
            stopwatch.Start();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Message.Visibility = Visibility.Collapsed;
            myCanvas.Visibility = Visibility.Visible;
            Conttol.Visibility = Visibility.Collapsed;
            //outer_music.Position = TimeSpan.Zero;
            //outer_music.Play();
            gameTimer.Start();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Message.Visibility = Visibility.Collapsed;
            EndScreen.Visibility = Visibility.Visible;
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
