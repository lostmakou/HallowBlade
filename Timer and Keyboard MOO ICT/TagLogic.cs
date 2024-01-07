using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mini_games1
{
    internal class Tag
    {
        int[,] Field = new int[0, 0];
        int Size;
        int x0;
        int y0;
        static Random s_move = new Random();
        Canvas canvas;

        public Tag(Canvas canvas) { this.canvas = canvas; }

        //Размер массива
        public void Array(int Size)
        {
            this.Size = Size;
            Field = new int[Size, Size];
        }

        // Заполнение массива
        public void Start() 
        {
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                {
                    Field[x, y] = Coord_to_pos(x, y) + 1;
                    var btn = canvas.FindName(Button(Coord_to_pos(x, y))) as Button;
                    ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data/Texture/Tag/" + (Coord_to_pos(x, y) + 1).ToString() + ".jpg", UriKind.RelativeOrAbsolute)));
                    btn.Background = imageBrush;
                }
            x0 = Size - 1;
            y0 = Size - 1;
            Field[x0, y0] = 5;
            Random();
        }

        //Перемещение на свободное место
        public void Movement(int pos)
        {
            int x, y;
            Pos_to_coord(pos, out x, out y);
            if (Math.Abs(x0 - x) + Math.Abs(y0 - y) != 1)
                return;
            Field[x0, y0] = Field[x, y];
            var btn = canvas.FindName(Button(Coord_to_pos(x0, y0))) as Button;
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data/Texture/Tag/" + (Field[x, y]).ToString() + ".jpg", UriKind.RelativeOrAbsolute)));
            btn.Background = imageBrush;
            Field[x, y] = 0;
            x0 = x; y0 = y;
        }

        public void Random()
        {
            int a = s_move.Next(0, 4);
            int x = x0;
            int y = y0;
            switch (a)
            {
                case 0: x--; break;
                case 1: x++; break;
                case 2: y--; break;
                case 3: y++; break;
            }
            Movement(Coord_to_pos(x, y));
        }

        public bool Pos_check()
        {
            if (!(x0 == Size - 1 && y0 == Size - 1)) return false;
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                    if (!(x == Size -1 && y == Size - 1))
                        if (Field[x, y] != Coord_to_pos(x, y) + 1) return false;
            return true;
        }
        public int Get_number(int pos)
        {
            int x, y;
            Pos_to_coord(pos, out x, out y);
            if (x < 0 || x >= Size) return 0;
                if (y < 0 || y >= Size) return 0;
                    return Field[x, y];
        }

        //Перевод координат в позицию
        private int Coord_to_pos(int x, int y)
        {
            //Проверка на выход за пределы игрового поля
            if (x < 0) x = 0;
                if (x > Size - 1) x = Size -1;
                    if (y < 0) y = 0;
                        if (y > Size - 1) y = Size -1;
                            return y * Size + x;
        }

        //Перевод позиции в координаты
        private void Pos_to_coord(int pos, out int x, out int y)
        {
            if (pos < 0 ) pos = 0;
                if (pos > Size * Size - 1) pos = Size * Size -1;
                    x = pos % Size;
                    y = pos / Size;
        }


        private string Button(int pos)
        {
            switch (pos)
            {
                case 0: return "B0";
                case 1: return "B1";
                case 2: return "B2";
                case 3: return "B3";
                case 4: return "B4";
                case 5: return "B5";
                case 6: return "B6";
                case 7: return "B7";
                case 8: return "B8";
                default: return null;
            }
        }
    }
}
