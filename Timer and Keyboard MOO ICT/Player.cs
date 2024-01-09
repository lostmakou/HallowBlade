using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace ZeldaWPF {

    class Player : INotifyPropertyChanged
    {
        public Rectangle rect;
        private int PlayerSpeed;
        Canvas myCanvas;
        MapRender mr;
        //private List<TextBlock> InfoText = new List<TextBlock>();
        private Direction Direction { get; set; } = Direction.Down;
        
        public (int, int) PreDungeonCoord;

        private int health;
        private int score;
        private (int, int) area;
        private (int, int) dungeonArea;
        private (int, int) newArea;
        private bool isMatchedSword, isMatchedKey;

        public bool IsBoySaved { get; set; } = false;

        public bool IsMatchedSword
        {
            get { return isMatchedSword; }
            set
            {
                if (isMatchedSword != value)
                {
                    isMatchedSword = value;
                    OnPropertyChanged(nameof(IsMatchedSword));
                }
            }
        }

        public bool IsMatchedKey
        {
            get { return isMatchedKey; }
            set
            {
                if (isMatchedKey != value)
                {
                    isMatchedKey = value;
                    OnPropertyChanged(nameof(IsMatchedKey));
                }
            }
        }

        public int Health
        {
            get { return health; }
            set
            {
                if (health != value)
                {
                    health = value;
                    OnPropertyChanged(nameof(Health));
                }
            }
        }

        public int Score
        {
            get { return score; }
            set
            {
                if (score != value)
                {
                    score = value;
                    OnPropertyChanged(nameof(Score));
                }
            }
        }


        public (int, int) Area
        {
            get { return area; }
            set
            {
                if (!area.Equals(value))
                {
                    area = value;
                    OnPropertyChanged(nameof(Area));
                }
            }

        }

        public (int, int) DungeonArea
        {
            get { return dungeonArea; }
            set
            {
                if (!dungeonArea.Equals(value))
                {
                    dungeonArea = value;
                    OnPropertyChanged(nameof(DungeonArea));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public bool InDungeon;
        public char Dungeon;
        //public int Money;
        public int Invincibility { get; set; }
        ImageBrush imageBrush;

        TextBlock DungeonScreen;

        public Player(Canvas canvas, MapRender mr)
        {
            rect = new Rectangle();
            rect.Height = 50;
            rect.Width = 50;
            //rect.Fill = Brushes.Red;
            myCanvas = canvas;
            myCanvas.Children.Add(rect);
            imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Grass.png", UriKind.RelativeOrAbsolute)))
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, 50, 50),
                ViewportUnits = BrushMappingMode.Absolute,
                //Viewbox = new Rect(0, 0, 10, 10)
            };
            myCanvas.Background = imageBrush;
            Canvas.SetLeft(rect, 400);
            Canvas.SetTop(rect, 300);
            imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\mchar_ver1_noweapon_front1.png", UriKind.RelativeOrAbsolute)));

            
            rect.Fill = imageBrush;

            PlayerSpeed = 5;
            Health = 25;
            Score = 0;
            Area = (0, 0);
            IsMatchedSword = false;
            IsMatchedKey = false;
            InDungeon = false;
            Invincibility = 0;
            this.mr = mr;
            mr.Render(Area);
            BringToFront();

            //OuterScreen = canvas.FindName("OuterScreen") as TextBlock;
            DungeonScreen = canvas.FindName("DungeonScreen") as TextBlock;
            //HealthInfo = canvas.FindName("Health") as TextBlock;

        }

        public void Update()
        {
            if (Invincibility > 0)
                Invincibility -= 1;
            CollideWithEnemies();
        }

        public void Move(bool goLeft, bool goRight, bool goUp, bool goDown)
        {
            if (goLeft == true /*&& Canvas.GetLeft(rect) > 5*/)
            {
                if (Direction != Direction.Left)
                {
                    Direction = Direction.Left;
                    imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\mchar_ver1_side_left1.png", UriKind.RelativeOrAbsolute)));
                    rect.Fill= imageBrush;
                }
                Canvas.SetLeft(rect, Canvas.GetLeft(rect) - PlayerSpeed);
                CollideWithBlocks(-1, 0);
            }
            else if (goRight == true /*&& Canvas.GetLeft(rect) + (rect.Width + 20) < Application.Current.MainWindow.Width*/)
            {
                if (Direction != Direction.Right)
                {
                    Direction = Direction.Right;
                    imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\mchar_ver1_noweapon_side_right1.png", UriKind.RelativeOrAbsolute)));
                    rect.Fill = imageBrush;
                }
                Canvas.SetLeft(rect, Canvas.GetLeft(rect) + PlayerSpeed);
                CollideWithBlocks(1, 0);
            }
            if (goUp == true /*&& Canvas.GetTop(rect) > 5*/)
            {
                if (Direction != Direction.Down)
                { 
                    Direction = Direction.Down;
                    imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\mchar_ver1_noweapon_back1.png", UriKind.RelativeOrAbsolute)));
                    rect.Fill = imageBrush;
                }
                Canvas.SetTop(rect, Canvas.GetTop(rect) - PlayerSpeed);
                CollideWithBlocks(0, -1);
            }
            else if (goDown == true /*&& Canvas.GetTop(rect) + (rect.Height * 2) < Application.Current.MainWindow.Height*/)
            {
                if (Direction != Direction.Up)
                {
                    Direction = Direction.Up;
                    imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\mchar_ver1_noweapon_front1.png", UriKind.RelativeOrAbsolute)));
                    rect.Fill = imageBrush;
                }
                Canvas.SetTop(rect, Canvas.GetTop(rect) + PlayerSpeed);
                CollideWithBlocks(0, 1);
            }

            if (Canvas.GetLeft(rect) + rect.ActualWidth > Application.Current.MainWindow.ActualWidth) 
            {
                Canvas.SetLeft(rect, Application.Current.MainWindow.ActualWidth);
                if (InDungeon)
                {
                    newArea = (DungeonArea.Item1 + 1, DungeonArea.Item2);
                    DungeonArea = newArea;
                    mr.RenderDungeon(DungeonArea, Dungeon);
                    //OuterScreen.Text = $"Экран подземелья: {DungeonArea.Item1} - {DungeonArea.Item2}";
                }
                else
                {
                    newArea = (Area.Item1 + 1, Area.Item2);
                    Area = newArea;
                    mr.Render(Area);
                    //OuterScreen.Text = $"Экран: {Area.Item1} - {Area.Item2}";
                }
                BringToFront();
                Canvas.SetLeft(rect, 1);

            }
            if (Canvas.GetLeft(rect) < 1)
            {
                Canvas.SetLeft(rect, 1);
                
                if (InDungeon)
                {
                    newArea = (DungeonArea.Item1 - 1, DungeonArea.Item2);
                    DungeonArea = newArea;
                    mr.RenderDungeon(DungeonArea, Dungeon);
                    //OuterScreen.Text = $"Экран подземелья: {DungeonArea.Item1} - {DungeonArea.Item2}";
                }
                else
                {
                    newArea = (Area.Item1 - 1, Area.Item2);
                    Area = newArea;
                    mr.Render(Area);
                   // OuterScreen.Text = $"Экран: {Area.Item1} - {Area.Item2}";
                }
                BringToFront();
                Canvas.SetLeft(rect, Application.Current.MainWindow.ActualWidth - rect.ActualWidth);
            }
            if (Canvas.GetTop(rect) + rect.ActualHeight > Application.Current.MainWindow.ActualHeight)
            {
                Canvas.SetTop(rect, Application.Current.MainWindow.ActualHeight);

                if (InDungeon) 
                {
                    newArea = (DungeonArea.Item1,  DungeonArea.Item2 + 1);
                    DungeonArea = newArea;
                    mr.RenderDungeon(DungeonArea, Dungeon);
                    //OuterScreen.Text = $"Экран подземелья: {DungeonArea.Item1} - {DungeonArea.Item2}";
                }
                else
                {
                    newArea = (Area.Item1, Area.Item2 + 1);
                    Area = newArea;
                    mr.Render(Area);
                    //OuterScreen.Text = $"Экран: {Area.Item1} - {Area.Item2}";
                }
                BringToFront();
                Canvas.SetTop(rect, 100);
                
            }
            if (Canvas.GetTop(rect) < 100)
            {
                Canvas.SetTop(rect, 100);
                if (InDungeon)
                {
                    newArea = (DungeonArea.Item1, dungeonArea.Item2 - 1);
                    DungeonArea = newArea;
                    mr.RenderDungeon(DungeonArea, Dungeon);
                    //OuterScreen.Text = $"Экран подземелья: {DungeonArea.Item1} - {DungeonArea.Item2}";
                }
                else
                {
                    newArea = (Area.Item1, Area.Item2 - 1);
                    Area = newArea;
                    mr.Render(Area);
                    //OuterScreen.Text = $"Экран: {Area.Item1} - {Area.Item2}";
                }
                BringToFront();
                Canvas.SetTop(rect, Application.Current.MainWindow.ActualHeight - rect.ActualHeight);
            }

        }

        public void CollideWithEnemies()
        {
            Rect PlayerHitBox = new Rect(Canvas.GetLeft(rect) + 5, Canvas.GetTop(rect) + 10, rect.ActualWidth - 10, rect.ActualHeight - 10);

            foreach (IEnemy enemy in mr.enemies)
            {
                Rect EnemyHitBox = new Rect(Canvas.GetLeft(enemy.EnemyRectangle), Canvas.GetTop(enemy.EnemyRectangle), enemy.EnemyRectangle.ActualWidth, enemy.EnemyRectangle.ActualHeight);

                if (PlayerHitBox.IntersectsWith(EnemyHitBox))
                {
                    if (Invincibility == 0)
                    {
                        Health -= 1;
                        Invincibility = 30;
                        //HealthInfo.Text = $"Здоровье: {Health}";
                    }

                }
            }
        }

        public void CollideWithBlocks(int dx, int dy)
        {
            Rect PlayerHitBox = new Rect(Canvas.GetLeft(rect)+5, Canvas.GetTop(rect)+10, rect.ActualWidth - 10, rect.ActualHeight - 10);
            foreach (Block block in mr.blocks)
            { 
                Rect BlockHitBox = new Rect(Canvas.GetLeft(block.BlockRect), Canvas.GetTop(block.BlockRect), block.BlockRect.ActualWidth, block.BlockRect.ActualHeight);

                if (PlayerHitBox.IntersectsWith(BlockHitBox))
                {
                    if (dx > 0) // right
                        Canvas.SetLeft(rect, Canvas.GetLeft(block.BlockRect) - block.BlockRect.Width - 1 + 5);
                    if (dx < 0) // left
                        Canvas.SetLeft(rect, Canvas.GetLeft(block.BlockRect) + block.BlockRect.Width + 1 - 5);
                    if (dy > 0) // down
                        Canvas.SetTop(rect, Canvas.GetTop(block.BlockRect) - block.BlockRect.Height - 1);
                    if (dy < 0) // up
                        Canvas.SetTop(rect, Canvas.GetTop(block.BlockRect) + block.BlockRect.Height + 1 - 10);
                    //break;

                    if (block.isDungeon)
                    {
                        DungeonArea = (0, 0);
                        //OuterScreen.Text = $"Экран подземелья: {DungeonArea.Item1} - {DungeonArea.Item2}";
                        DungeonScreen.Visibility = Visibility.Visible;
                        if (InDungeon)
                        {
                            Dungeon = '_';
                            InDungeon = false;
                            // Телепорт в предыдущее место
                            Canvas.SetTop(rect, PreDungeonCoord.Item2);
                            Canvas.SetLeft(rect, PreDungeonCoord.Item1);

                            mr.Render(Area);
                            BringToFront();
                            DungeonScreen.Visibility = Visibility.Collapsed;
                            //OuterScreen.Text = $"Экран: {Area.Item1} - {Area.Item2}";
                            break;
                        }
                        else
                        {
                            Dungeon = block.Type;
                            InDungeon = true;
                            // телепорт в центр экрана
                            PreDungeonCoord = ((int)Canvas.GetLeft(rect), (int)Canvas.GetTop(rect));
                            Canvas.SetTop(rect, 450);
                            Canvas.SetLeft(rect, 400);

                            imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Stone_floor.png", UriKind.RelativeOrAbsolute)))
                            {
                                TileMode = TileMode.Tile,
                                Viewport = new Rect(0, 0, 50, 50),
                                ViewportUnits = BrushMappingMode.Absolute,
                                //Viewbox = new Rect(0, 0, 10, 10)
                            };
                            myCanvas.Background = imageBrush;
                            mr.RenderDungeon(DungeonArea, Dungeon);
                            BringToFront();
                            break;
                        }
                    }
                   

                }
            }
        }

        public bool DistanceToBlock(Block block)
        {
            double dx = Canvas.GetLeft(rect) - Canvas.GetLeft(block.BlockRect);
            double dy = Canvas.GetTop(rect) - Canvas.GetTop(block.BlockRect);
            double dist = Math.Sqrt(dx * dx + dy * dy);
            if (dist < 60)
            {
                return true;
            }
            return false;

        }

        //public (int, int) PlayerPosition()
        //{
        //    return ((int)Canvas.GetLeft(rect), (int)Canvas.GetTop(rect));
        //}

        public void BringToFront()
        {
            myCanvas.Children.Remove(rect);
            myCanvas.Children.Add(rect);
        }
    }
}
