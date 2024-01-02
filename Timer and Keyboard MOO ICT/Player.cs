using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZeldaWPF {

    class Player
    {
        public Rectangle rect;
        private int PlayerSpeed;
        Canvas myCanvas;
        MapRender mr;
        private List<TextBlock> InfoText = new List<TextBlock>();
        public int Health;
        public (int, int) Area;
        public (int, int) DungeonArea;
        public bool InDungeon;
        public char Dungeon;
        public int Money;

        public Player(Canvas canvas, MapRender mr)
        {
            rect = new Rectangle();
            rect.Height = 50;
            rect.Width = 50;
            rect.Fill = Brushes.Red;
            myCanvas = canvas;
            myCanvas.Children.Add(rect);
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\WPF-Move-Rectangle-In-Canvas-Using-Keyboard-and-Timer-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Grass.png", UriKind.RelativeOrAbsolute)))
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, 50, 50),
                ViewportUnits = BrushMappingMode.Absolute,
                //Viewbox = new Rect(0, 0, 10, 10)
            };
            myCanvas.Background = imageBrush;
            Canvas.SetLeft(rect, 400);
            Canvas.SetTop(rect, 300);
            PlayerSpeed = 5;
            Health = 12;
            Area = (0, 0);
            InDungeon = false;
            this.mr = mr;
            mr.Render(Area);
            BringToFront();
        }

        public void Move(bool goLeft, bool goRight, bool goUp, bool goDown)
        {
            if (goLeft == true /*&& Canvas.GetLeft(rect) > 5*/)
            {
                Canvas.SetLeft(rect, Canvas.GetLeft(rect) - PlayerSpeed);
                Collide(-1, 0);
            }
            else if (goRight == true /*&& Canvas.GetLeft(rect) + (rect.Width + 20) < Application.Current.MainWindow.Width*/)
            {
                Canvas.SetLeft(rect, Canvas.GetLeft(rect) + PlayerSpeed);
                Collide(1, 0);
            }
            if (goUp == true /*&& Canvas.GetTop(rect) > 5*/)
            {
                Canvas.SetTop(rect, Canvas.GetTop(rect) - PlayerSpeed);
                Collide(0, -1);
            }
            else if (goDown == true /*&& Canvas.GetTop(rect) + (rect.Height * 2) < Application.Current.MainWindow.Height*/)
            {
                Canvas.SetTop(rect, Canvas.GetTop(rect) + PlayerSpeed);
                Collide(0, 1);
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

        public void Collide(int dx, int dy)
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
