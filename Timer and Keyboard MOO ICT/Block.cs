using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ZeldaWPF
{
    public class Block
    {
        public Rectangle BlockRect { get; set; }
        public char Type { get; set; }
        public bool isDungeon;
        public int Id;
        public Block(char type, int x, int y)
        {
            BlockRect = new Rectangle();
            BlockRect.Width = 50;
            BlockRect.Height = 50;
            //ImageBrush blockTexture = new ImageBrush();
            //blockTexture.ImageSource = new BitmapImage(new Uri());
            BlockRect.Fill = Brushes.Black;
            this.Type = type;
            SetTexture();
        }
        public void SetTexture()
        {
            ImageBrush imageBrush = new ImageBrush();
            if (Type == '$')
                imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Water.png", UriKind.RelativeOrAbsolute)));
            else if (Type == '#')
                imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Stone_wall.png", UriKind.RelativeOrAbsolute)));
            else if (Type == '!')
                imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Tree.png", UriKind.RelativeOrAbsolute)));
            else if (Type == '*')
                imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\torch.png", UriKind.RelativeOrAbsolute)));
            else if (Type == '%')
                imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Stone.png", UriKind.RelativeOrAbsolute)));
            else if (Type == '(')
                imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Coffin.png", UriKind.RelativeOrAbsolute)));
            else if (Type == ')')
                imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Cross.png", UriKind.RelativeOrAbsolute)));
            else if (Type == '<')
                imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Web_left.png", UriKind.RelativeOrAbsolute)));
            else if (Type == '>')
                imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Web_right.png", UriKind.RelativeOrAbsolute)));
            else if (Type == '|')
                imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Web_center.png", UriKind.RelativeOrAbsolute)));
            else if (Type == '⎕')
                imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Stone_with_a_note.png", UriKind.RelativeOrAbsolute)));
            else if (char.IsLetter(Type))
            {
                isDungeon = true;
                BlockRect.Fill = Brushes.Gold;
            }
            BlockRect.Fill = imageBrush;
        }
    }
}
