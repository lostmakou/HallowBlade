using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;

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
        int Steps = 10;
        Direction Direction = Direction.Left;
        public Rectangle EnemyRectangle { get; set; }
        public int Health { get; set; } = 2;
        public Position position { get; set; }

        public Zombie(int x, int y, Canvas canvas)
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
        }

        public void Update(Position playerPosition)
        {
            
        }
    }

}
