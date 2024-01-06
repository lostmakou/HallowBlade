using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaWPF
{
    public class ScreensData
    {
        public List<Screen> Screens { get; set; }
    }

    public class Screen
    {
        public List<string> Layout { get; set; }
        public Position Position { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<ItemS> Item {  get; set; }

    }

    public class ItemS 
    {
        public string Name { get; set; }
        public Position ScreenPosition { get; set; }
        public Position Position { get; set; }
    }


    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Enemy
    {
        public string Type { get; set; }
        public Position Position { get; set; }
    }

    public class ScreenDataDungeon 
    {
        public List<Screen> Screens { get; set;}
        public string Dungeon { get; set; }
    }

    public class DungeonData
    {
        public List<ScreenDataDungeon> Dungeons { get; set; }
    }

    public class NotesData
    {
        public List<Note> Notes { get; set;}
    }

    public class Note
    {
        public string NoteString { get; set; }
        public Position ScreenPosition { get; set; }
        public Position Position { get; set; }
        public int Id { get; set; }
    }
}
