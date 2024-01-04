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
            if (Type == '$')
            {
                ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\WPF-Move-Rectangle-In-Canvas-Using-Keyboard-and-Timer-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Water.png", UriKind.RelativeOrAbsolute)));
                BlockRect.Fill = imageBrush;
            }
            else if (Type == '#')
            {
                ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\WPF-Move-Rectangle-In-Canvas-Using-Keyboard-and-Timer-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Stone_wall.png", UriKind.RelativeOrAbsolute)));
                BlockRect.Fill = imageBrush;
            }

            else if (Type == '!')
            {
                ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\WPF-Move-Rectangle-In-Canvas-Using-Keyboard-and-Timer-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Tree.png", UriKind.RelativeOrAbsolute)));
                BlockRect.Fill = imageBrush;
            }
            else if (Type == '*')
            {
                ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\WPF-Move-Rectangle-In-Canvas-Using-Keyboard-and-Timer-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\torch.png", UriKind.RelativeOrAbsolute)));
                BlockRect.Fill = imageBrush;
            }
            else if (char.IsLetter(Type))
            {
                isDungeon = true;
                BlockRect.Fill = Brushes.Gold;
            }
        }
    }
}
