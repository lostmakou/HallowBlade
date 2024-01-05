using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace ZeldaWPF
{
    public abstract class IEnemy
    {
        public Rectangle EnemyRectangle { get; set; }
        public int Health { get; set; }
        public Position position { get; set; }
        public abstract void Update(Position playerPosition);
        public void CollideWithBlocks(int dx, int dy, List<Block> blocks)
        {
            Rect PlayerHitBox = new Rect(Canvas.GetLeft(EnemyRectangle) + 5, Canvas.GetTop(EnemyRectangle) + 10, EnemyRectangle.ActualWidth - 10, EnemyRectangle.ActualHeight - 10);
            foreach (Block block in blocks)
            {

                Rect BlockHitBox = new Rect(Canvas.GetLeft(block.BlockRect), Canvas.GetTop(block.BlockRect), block.BlockRect.ActualWidth, block.BlockRect.ActualHeight);

                if (PlayerHitBox.IntersectsWith(BlockHitBox))
                {
                    if (dx > 0) // right
                        Canvas.SetLeft(EnemyRectangle, Canvas.GetLeft(block.BlockRect) - block.BlockRect.Width - 1 + 5);
                    if (dx < 0) // left
                        Canvas.SetLeft(EnemyRectangle, Canvas.GetLeft(block.BlockRect) + block.BlockRect.Width + 1 - 5);
                    if (dy > 0) // down
                        Canvas.SetTop(EnemyRectangle, Canvas.GetTop(block.BlockRect) - block.BlockRect.Height - 1);
                    if (dy < 0) // up
                        Canvas.SetTop(EnemyRectangle, Canvas.GetTop(block.BlockRect) + block.BlockRect.Height + 1 - 10);
                    //break;
                }

            }
        }
    }

    public class Ghost : IEnemy
    {
        int Speed = 2;
        Canvas canvas;
        public Ghost(int x, int y, Canvas canvas)
        {
            this.position = new Position
            {
                X = x,
                Y = y
            };
            EnemyRectangle = new Rectangle();
            EnemyRectangle.Height = 50;
            EnemyRectangle.Width = 50;
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\ghost_front1.png", UriKind.RelativeOrAbsolute)));
            EnemyRectangle.Fill = imageBrush;
            this.canvas = canvas;
            canvas.Children.Add(EnemyRectangle);
            Health = 1;
        }

        public override void Update(Position playerPosition)
        {
            double dx = playerPosition.X - position.X;
            double dy = playerPosition.Y - position.Y;
            double norm = Math.Sqrt(dx * dx + dy * dy);
            dx /= norm;
            dy /= norm;

            this.position.Y += (int)(dy * Speed);
            this.position.X += (int)(dx * Speed);
            Canvas.SetTop(EnemyRectangle, this.position.Y);
            Canvas.SetLeft(EnemyRectangle, this.position.X);

        }
    }

    public class Zombie : IEnemy
    {
        int Speed = 2;
        Canvas canvas;
        int Steps = 5;
        Direction Direction = Direction.Left;
        List<Block> blocks;

        public Zombie(int x, int y, Canvas canvas, List<Block> blocks)
        {
            this.position = new Position
            {
                X = x,
                Y = y
            };
            EnemyRectangle = new Rectangle();
            EnemyRectangle.Height = 50;
            EnemyRectangle.Width = 50;
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\zombie_front1.png", UriKind.RelativeOrAbsolute)));
            EnemyRectangle.Fill = imageBrush;
            this.canvas = canvas;
            canvas.Children.Add(EnemyRectangle);
            this.blocks = blocks;
            Health = 2;
        }

        public override void Update(Position playerPosition)
        {
            if (Steps > 0)
            {
                if (Direction == Direction.Up)
                {
                    Canvas.SetTop(EnemyRectangle, Canvas.GetTop(EnemyRectangle) - Speed);
                    CollideWithBlocks(0, -1, blocks);
                }
                else if (Direction == Direction.Down)
                {
                    Canvas.SetTop(EnemyRectangle, Canvas.GetTop(EnemyRectangle) + Speed);
                    CollideWithBlocks(0, 1, blocks);
                }
                else if (Direction == Direction.Left)
                {
                    Canvas.SetLeft(EnemyRectangle, Canvas.GetLeft(EnemyRectangle) - Speed);
                    CollideWithBlocks(-1, 0, blocks);
                }
                else
                {
                    Canvas.SetLeft(EnemyRectangle, Canvas.GetLeft(EnemyRectangle) + Speed);
                    CollideWithBlocks(1, 0, blocks);
                }
                Steps -= 1;
            }
            else
            {
                Random random = new Random();
                Type type = typeof(Direction);
                Array values = type.GetEnumValues();
                int index = random.Next(values.Length);
                Direction = (Direction)values.GetValue(index);
                Steps = random.Next(5, 100);
            }
        }

        
    }

    public class Vampire : IEnemy
    {
        Direction Direction { get; set; } = Direction.Down;
        List<Block> blocks;
        int Speed = 2;
        Canvas canvas;
        ImageBrush imageBrush;

        public Vampire(int x, int y, Canvas canvas, List<Block> blocks)
        {
            this.position = new Position
            {
                X = x,
                Y = y
            };
            EnemyRectangle = new Rectangle();
            EnemyRectangle.Height = 50;
            EnemyRectangle.Width = 50;
            imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\vampire_front1.png", UriKind.RelativeOrAbsolute)));
            EnemyRectangle.Fill = imageBrush;
            this.canvas = canvas;
            canvas.Children.Add(EnemyRectangle);
            this.blocks = blocks;
            Health = 3;
        }

        public override void Update(Position playerPosition)
        {
            /*double dx = playerPosition.X - position.X;
            double dy = playerPosition.Y - position.Y;
            double norm = Math.Sqrt(dx * dx + dy * dy);
            dx /= norm;
            dy /= norm;

            this.position.Y += (int)(dy * Speed);
            this.position.X += (int)(dx * Speed);
            Canvas.SetTop(EnemyRectangle, this.position.Y);
            Canvas.SetLeft(EnemyRectangle, this.position.X);*/

            double dx = playerPosition.X - position.X;
            double dy = playerPosition.Y - position.Y;
            bool dirToGo = (Math.Abs(dx) > Math.Abs(dy)) ? true : false;
            if (Math.Abs(dx) > 10 && Math.Abs(dy) > 10)
            {
                double norm = Math.Sqrt(dx * dx + dy * dy);





                if (dirToGo)
                {
                    if (dx > 0 && Direction != Direction.Right)
                    {
                        Direction = Direction.Right;
                        imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\vampire_side_right1.png", UriKind.RelativeOrAbsolute)));
                        EnemyRectangle.Fill = imageBrush;
                    }
                    else if (dx < 0 && Direction != Direction.Left)
                    {
                        Direction = Direction.Left;
                        imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\vampire_side_left1.png", UriKind.RelativeOrAbsolute)));
                        EnemyRectangle.Fill = imageBrush;
                    }
                    dx /= norm;
                    this.position.X += (int)(dx * Speed);
                    Canvas.SetLeft(EnemyRectangle, this.position.X);

                    //if (dx > 0)
                    //    CollideWithBlocks(1, 0);
                    //else if (dx < 0)
                    //    CollideWithBlocks(-1, 0);
                }
                else
                {
                    if (dy > 0 && Direction != Direction.Down)
                    {
                        Direction = Direction.Down;
                        imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\vampire_front1.png", UriKind.RelativeOrAbsolute)));
                        EnemyRectangle.Fill = imageBrush;
                    }
                    else if (dy < 0 && Direction != Direction.Up)
                    {
                        Direction = Direction.Up;
                        imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\vampire_back1.png", UriKind.RelativeOrAbsolute)));
                        EnemyRectangle.Fill = imageBrush;
                    }

                    dy /= norm;
                    this.position.Y += (int)(dy * Speed);
                    Canvas.SetTop(EnemyRectangle, this.position.Y);

                    //if (dy > 0)
                    //    CollideWithBlocks(0, 1);
                    //else if (dy < 0)
                    //    CollideWithBlocks(0, -1);
                }


            }

            if (dx > 0)
                CollideWithBlocks(1, 0, blocks);
            else if (dx < 0)
                CollideWithBlocks(-1, 0, blocks);
            else if (dy > 0)
                CollideWithBlocks(0, 1, blocks);
            else if (dy < 0)
                CollideWithBlocks(0, -1, blocks);
        }

       
    }

    public class Boss : IEnemy
    {
        Canvas canvas { get; set; }
        List<Block> blocks;
        List<IEnemy> enemies;
        int Speed = 2;
        ImageBrush imageBrush;
        Direction Direction { get; set; } = Direction.Down;

        public Boss(int x, int y, Canvas canvas, List<Block> blocks, List<IEnemy> enemies)
        {
            this.position = new Position
            {
                X = x,
                Y = y
            };
            EnemyRectangle = new Rectangle();
            EnemyRectangle.Height = 100;
            EnemyRectangle.Width = 100;
            imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\boss_front1.png", UriKind.RelativeOrAbsolute)));
            EnemyRectangle.Fill = imageBrush;
            this.canvas = canvas;
            canvas.Children.Add(EnemyRectangle);
            this.blocks = blocks;
            this.enemies = enemies;
            Health = 10;
        }

        public override void Update(Position playerPosition)
        {
            double dx = playerPosition.X - position.X;
            double dy = playerPosition.Y - position.Y;
            bool dirToGo = (Math.Abs(dx) > Math.Abs(dy)) ? true : false;
            if (Math.Abs(dx) > 10 && Math.Abs(dy) > 10)
            {
                double norm = Math.Sqrt(dx * dx + dy * dy);
                
                



                if (dirToGo)
                {
                    if (dx > 0 && Direction != Direction.Right)
                    {
                        Direction = Direction.Right;
                        imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\boss_side_right1.png", UriKind.RelativeOrAbsolute)));
                        EnemyRectangle.Fill = imageBrush;
                    }
                    else if (dx < 0 && Direction != Direction.Left)
                    {
                        Direction = Direction.Left;
                        imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\boss_side_left1.png", UriKind.RelativeOrAbsolute)));
                        EnemyRectangle.Fill = imageBrush;
                    }
                    dx /= norm;
                    this.position.X += (int)(dx * Speed);
                    Canvas.SetLeft(EnemyRectangle, this.position.X);

                    //if (dx > 0)
                    //    CollideWithBlocks(1, 0);
                    //else if (dx < 0)
                    //    CollideWithBlocks(-1, 0);
                }
                else
                {
                    if (dy > 0 && Direction != Direction.Down)
                    {
                        Direction = Direction.Down;
                        imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\boss_front1.png", UriKind.RelativeOrAbsolute)));
                        EnemyRectangle.Fill = imageBrush;
                    }
                    else if (dy < 0 && Direction != Direction.Up)
                    {
                        Direction = Direction.Up;
                        imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\boss_backt1.png", UriKind.RelativeOrAbsolute)));
                        EnemyRectangle.Fill = imageBrush;
                    }

                    dy /= norm;
                    this.position.Y += (int)(dy * Speed);
                    Canvas.SetTop(EnemyRectangle, this.position.Y);

                    //if (dy > 0)
                    //    CollideWithBlocks(0, 1);
                    //else if (dy < 0)
                    //    CollideWithBlocks(0, -1);
                }


            }
            
            

            if (Health == 5)
            {
                //Speed = 0;
                (int, int)[] values = {(-100, 0), (100, 0), (0, 100), (0, -100) };
                foreach(var value in values)
                {
                    var en = new Ghost(position.X + value.Item1, position.Y + value.Item2, canvas);
                    Canvas.SetTop(en.EnemyRectangle, position.Y + value.Item2);
                    Canvas.SetLeft(en.EnemyRectangle, position.X + value.Item1);
                    enemies.Add(en);
                }
                Health -= 1;
            }
            //if (Health == 1)
            //{

            //}
        }
    }
}
