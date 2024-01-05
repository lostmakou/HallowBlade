using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZeldaWPF;

namespace HallowBlade
{
    
    public class Item
    {
        string Name {  get; set; }
        public Rectangle ItemRectangle { get; set; }
        
        public Item(string name) 
        { 
            Name = name;
            ImageBrush imageBrush;
            ItemRectangle = new Rectangle();
            ItemRectangle.Width = 50;
            ItemRectangle.Height = 50;
            if (Name == "Sword")
            {
                imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\HallowBlade\\Timer and Keyboard MOO ICT\\Data\\Texture\\blade.png", UriKind.RelativeOrAbsolute)));
            }
            else if (Name == "Key")
            {
                imageBrush = new ImageBrush(new BitmapImage(new Uri("\\\\Mac\\Home\\Desktop\\HallowBlade\\Timer and Keyboard MOO ICT\\Data\\Texture\\Gallows.png", UriKind.RelativeOrAbsolute)));

            }
        }
    }
}
