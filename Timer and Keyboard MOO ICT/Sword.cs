using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZeldaWPF
{
    public enum Direction
    {
        Up, Down, Left, Right
    }

    class Sword
    {
        public Rectangle rect;
        private Canvas canvas;
        private int swordSpeed = 5;
        private Direction dir;
        private int timeOut = 12;
        public bool isExist = true;
        

        public Sword(int x, int y, Direction direction, Canvas canvas) 
        {
            rect = new Rectangle();
            rect.Height = 50;
            rect.Width = 50;
            rect.Fill = Brushes.White;
            this.canvas = canvas;
            canvas.Children.Add(rect);
            Canvas.SetTop(rect, y);
            Canvas.SetLeft(rect, x);
            this.dir = direction;
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture/blade_front.png", UriKind.RelativeOrAbsolute))); ;
            RotateTransform rotateTransform = new RotateTransform(0);
            if (dir == Direction.Up) 
            {
                rotateTransform = new RotateTransform(0);
            }
            else if (dir == Direction.Down)
            {
                rotateTransform = new RotateTransform(180);
            }
            else if (direction == Direction.Left)
            {
                rotateTransform = new RotateTransform(270);
            }
            else if (direction == Direction.Right)
            {
                rotateTransform = new RotateTransform(90);
            }
            imageBrush.RelativeTransform = rotateTransform;
            //rect.Fill = imageBrush;

        }

        public bool Update()
        {
            if (dir == Direction.Up) 
            {
                Canvas.SetTop(rect, Canvas.GetTop(rect) - swordSpeed);
            }
            else if (dir == Direction.Down)
            {
                Canvas.SetTop(rect, Canvas.GetTop(rect) + swordSpeed);
            }
            else if (dir == Direction.Left)
            {
                Canvas.SetLeft(rect, Canvas.GetLeft(rect) - swordSpeed);
            }
            else 
            { 
                Canvas.SetLeft(rect, Canvas.GetLeft(rect) + swordSpeed); 
            }
            timeOut -= 1;
            if (timeOut < 0)
            {
                canvas.Children.Remove(rect);
                return true;
            }
            return false;   
        }
    }
}
