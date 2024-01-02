﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ZeldaWPF
{
    public enum Direction
    {
        Up, Down, Left, Right
    }

    class Sword
    {
        private Rectangle rect;
        private int swordSpeed = 20;
        private Direction dir;

        public Sword(int x, int y, Direction direction, Canvas canvas) 
        {
            rect = new Rectangle();
            rect.Height = 50;
            rect.Width = 50;
            rect.Fill = Brushes.White;
            canvas.Children.Add(rect);
            Canvas.SetTop(rect, y);
            Canvas.SetLeft(rect, x);
            this.dir = direction;
        }

        public void Update()
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
        }
    }
}
