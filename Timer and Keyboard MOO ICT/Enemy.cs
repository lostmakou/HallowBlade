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
    public interface IEnemy
    {
        Rectangle EnemyRectangle { get; set; }
        int Health { get; set; }
        Position position { get; set; }
        void Update(Position playerPosition);
    }

    public class Ghost : IEnemy
    {
        int Speed = 2;
        Canvas canvas;
        public Rectangle EnemyRectangle { get; set; }
        public int Health { get; set; } = 1;
        public Position position { get; set; }

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
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\HallowBlade\\Timer and Keyboard MOO ICT\\Data\\Texture\\ghost_front1.png", UriKind.RelativeOrAbsolute)));
            EnemyRectangle.Fill = imageBrush;
            this.canvas = canvas;
            canvas.Children.Add(EnemyRectangle);
        }

        public void Update(Position playerPosition)
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
        public Rectangle EnemyRectangle { get; set; }
        public int Health { get; set; } = 2;
        public Position position { get; set; }

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
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\HallowBlade\\Timer and Keyboard MOO ICT\\Data\\Texture\\zombie_front1.png", UriKind.RelativeOrAbsolute)));
            EnemyRectangle.Fill = imageBrush;
            this.canvas = canvas;
            canvas.Children.Add(EnemyRectangle);
            this.blocks = blocks;
        }

        public void Update(Position playerPosition)
        {
            if (Steps > 0)
            {
                if (Direction == Direction.Up)
                {
                    Canvas.SetTop(EnemyRectangle, Canvas.GetTop(EnemyRectangle) - Speed);
                    CollideWithBlocks(0, -1);
                }
                else if (Direction == Direction.Down)
                {
                    Canvas.SetTop(EnemyRectangle, Canvas.GetTop(EnemyRectangle) + Speed);
                    CollideWithBlocks(0, 1);
                }
                else if (Direction == Direction.Left)
                {
                    Canvas.SetLeft(EnemyRectangle, Canvas.GetLeft(EnemyRectangle) - Speed);
                    CollideWithBlocks(-1, 0);
                }
                else
                {
                    Canvas.SetLeft(EnemyRectangle, Canvas.GetLeft(EnemyRectangle) + Speed);
                    CollideWithBlocks(1, 0);
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

        public void CollideWithBlocks(int dx, int dy)
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

    public class Vampire : IEnemy
    {
        public Rectangle EnemyRectangle { get; set; }
        public int Health { get; set; }
        public Position position { get; set; }
        List<Block> blocks;
        int Speed = 2;
        Canvas canvas;

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
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\HallowBlade\\Timer and Keyboard MOO ICT\\Data\\Texture\\vampire_front1.png", UriKind.RelativeOrAbsolute)));
            EnemyRectangle.Fill = imageBrush;
            this.canvas = canvas;
            canvas.Children.Add(EnemyRectangle);
            this.blocks = blocks;
        }

        public void Update(Position playerPosition)
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

            if (dx > 0)
                CollideWithBlocks(1, 0);
            else if (dx < 0)
                CollideWithBlocks(-1, 0);
            else if (dy > 0)
                CollideWithBlocks(0, 1);
            else if (dy < 0)
                CollideWithBlocks(0, -1);
        }

        public void CollideWithBlocks(int dx, int dy)
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
}
