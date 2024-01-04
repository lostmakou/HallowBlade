using System;
using System.Collections.Generic;
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

namespace ZeldaWPF {

    class Player
    {
        public Rectangle rect;
        private int PlayerSpeed;
        Canvas myCanvas;
        MapRender mr;
        private List<TextBlock> InfoText = new List<TextBlock>();
        private Direction Direction { get; set; } = Direction.Down;
        public int Health;
        public (int, int) Area;
        public (int, int) DungeonArea;
        public bool InDungeon;
        public char Dungeon;
        public int Money;
        public int Invincibility;
        ImageBrush imageBrush;

        public Player(Canvas canvas, MapRender mr)
        {
            rect = new Rectangle();
            rect.Height = 50;
            rect.Width = 50;
            //rect.Fill = Brushes.Red;
            myCanvas = canvas;
            myCanvas.Children.Add(rect);
            imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\WPF-Move-Rectangle-In-Canvas-Using-Keyboard-and-Timer-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Grass.png", UriKind.RelativeOrAbsolute)))
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, 50, 50),
                ViewportUnits = BrushMappingMode.Absolute,
                //Viewbox = new Rect(0, 0, 10, 10)
            };
            myCanvas.Background = imageBrush;
            Canvas.SetLeft(rect, 400);
            Canvas.SetTop(rect, 300);
            imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\HallowBlade\\Timer and Keyboard MOO ICT\\Data\\Texture\\mchar_ver1_noweapon_front1.png", UriKind.RelativeOrAbsolute)));
            rect.Fill = imageBrush;
            PlayerSpeed = 5;
            Health = 3;
            Area = (0, 0);
            InDungeon = false;
            Invincibility = 0;
            this.mr = mr;
            mr.Render(Area);
            BringToFront();
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
                    imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\HallowBlade\\Timer and Keyboard MOO ICT\\Data\\Texture\\mchar_ver1_side_left1.png", UriKind.RelativeOrAbsolute)));
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
                    imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\HallowBlade\\Timer and Keyboard MOO ICT\\Data\\Texture\\mchar_ver1_noweapon_side_right1.png", UriKind.RelativeOrAbsolute)));
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
                    imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\HallowBlade\\Timer and Keyboard MOO ICT\\Data\\Texture\\mchar_ver1_noweapon_back1.png", UriKind.RelativeOrAbsolute)));
                    rect.Fill = imageBrush;
                }
                Canvas.SetTop(rect, Canvas.GetTop(rect) - PlayerSpeed);
                CollideWithBlocks(0, -1);
            }d
            else if (goDown == true /*&& Canvas.GetTop(rect) + (rect.Height * 2) < Application.Current.MainWindow.Height*/)
            {
                if (Direction != Direction.Up)
                {
                    Direction = Direction.Up;
                    imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\HallowBlade\\Timer and Keyboard MOO ICT\\Data\\Texture\\mchar_ver1_noweapon_front1.png", UriKind.RelativeOrAbsolute)));
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
                    DungeonArea.Item1 += 1;
                    mr.RenderDungeon(DungeonArea, Dungeon);
                }
                else
                {
                    Area.Item1 += 1;
                    mr.Render(Area);
                }
                BringToFront();
                Canvas.SetLeft(rect, 1);
            }
            if (Canvas.GetLeft(rect) < 1)
            {
                Canvas.SetLeft(rect, 1);
                
                if (InDungeon)
                {
                    DungeonArea.Item1 -= 1;
                    mr.RenderDungeon(DungeonArea, Dungeon);
                }
                else
                {
                    Area.Item1 -= 1;
                    mr.Render(Area);
                }
                BringToFront();
                Canvas.SetLeft(rect, Application.Current.MainWindow.ActualWidth - rect.ActualWidth);
            }
            if (Canvas.GetTop(rect) + rect.ActualHeight > Application.Current.MainWindow.ActualHeight)
            {
                Canvas.SetTop(rect, Application.Current.MainWindow.ActualHeight);

                if (InDungeon) 
                {
                    DungeonArea.Item2 += 1;
                    mr.RenderDungeon(DungeonArea, Dungeon);
                }
                else
                {
                    Area.Item2 += 1;
                    mr.Render(Area);
                }
                BringToFront();
                Canvas.SetTop(rect, 100);
            }
            if (Canvas.GetTop(rect) < 100)
            {
                Canvas.SetTop(rect, 100);
                if (InDungeon)
                {
                    DungeonArea.Item2 -= 1;
                    mr.RenderDungeon(DungeonArea, Dungeon);
                }
                else
                {
                    Area.Item2 -= 1;
                    mr.Render(Area);
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
                        Invincibility = 100;
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
                        if (InDungeon)
                        {
                            Dungeon = '_';
                            InDungeon = false;
                            // Телепорт в предыдущее место
                            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\WPF-Move-Rectangle-In-Canvas-Using-Keyboard-and-Timer-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Grass.png", UriKind.RelativeOrAbsolute)))
                            {
                                TileMode = TileMode.Tile,
                                Viewport = new Rect(0, 0, 50, 50),
                                ViewportUnits = BrushMappingMode.Absolute,
                                //Viewbox = new Rect(0, 0, 10, 10)
                            };
                            myCanvas.Background = imageBrush;
                            mr.Render(Area);
                            BringToFront();
                            break;
                        }
                        else
                        {
                            Dungeon = block.Type;
                            InDungeon = true;
                            // телепорт в центр экрана
                            myCanvas.Background = Brushes.DimGray;
                            mr.RenderDungeon(DungeonArea, Dungeon);
                            BringToFront();
                            break;
                        }
                    }
                    else if (block.Type == '⎕') 
                    {
                        TextBlock areaText = new TextBlock();
                        areaText.Text = $"{mr.notes[block.Id]}";
                        areaText.FontSize = 20;
                        areaText.Foreground = Brushes.White;
                        myCanvas.Children.Add(areaText);
                        Canvas.SetLeft(areaText, 200);
                        Canvas.SetTop(areaText, 20);
                        myCanvas.Children.Remove(block.BlockRect);
                        mr.blocks.Remove(block);
                        break;
                    }

                }
            }
        }

        public (int, int) PlayerPosition()
        {
            return ((int)Canvas.GetLeft(rect), (int)Canvas.GetTop(rect));
        }

        public void BringToFront()
        {
            myCanvas.Children.Remove(rect);
            myCanvas.Children.Add(rect);
        }

        public void PrintInfo()
        {
            foreach (TextBlock textBlock in InfoText)
            {
                myCanvas.Children.Remove(textBlock);
            }
            InfoText.Clear();

            TextBlock areaText = new TextBlock();
            areaText.Text = $"Экран: {Area.Item1} - {Area.Item2}";
            areaText.FontSize = 20;
            areaText.Foreground = Brushes.White;
            myCanvas.Children.Add(areaText);
            Canvas.SetLeft(areaText, 30);
            Canvas.SetTop(areaText, 20);
            InfoText.Add(areaText);

            TextBlock healthText = new TextBlock();
            healthText.Text = $"Здоровье: {Health}";
            healthText.FontSize = 20;
            healthText.Foreground = Brushes.White;
            myCanvas.Children.Add(healthText);
            Canvas.SetLeft(healthText, 30);
            Canvas.SetTop(healthText, 50);
            InfoText.Add(healthText);

            TextBlock dungeonText = new TextBlock();
            dungeonText.Text = $"Экран данжа: {DungeonArea.Item1} - {DungeonArea.Item2}";
            dungeonText.FontSize = 20;
            dungeonText.Foreground = Brushes.White;
            myCanvas.Children.Add(dungeonText);
            Canvas.SetLeft(dungeonText, 200);
            Canvas.SetTop(dungeonText, 50);
            InfoText.Add(dungeonText);
        }
    }
};
