using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.Json;

namespace ZeldaWPF
{
    internal class MapRender
    {
        Canvas myCanvas;
        public List<Block> blocks;
        public List<IEnemy> enemies;
        public List<IEnemy> tempEnemies;
        public string[] notes = new string[20];
        ScreensData screens;
        DungeonData dungeons;
        NotesData notesData;
        public MapRender(Canvas myCanvas) 
        {
            this.myCanvas = myCanvas; 
            blocks = new List<Block>();
            enemies = new List<IEnemy>();
            tempEnemies = new List<IEnemy>();
            string jsonString = File.ReadAllText("\\\\Mac\\Home\\Desktop\\HallowBlade\\Timer and Keyboard MOO ICT\\Data\\Map\\outer.json");
            screens = JsonSerializer.Deserialize<ScreensData>(jsonString);
            jsonString = File.ReadAllText("\\\\Mac\\Home\\Desktop\\HallowBlade\\Timer and Keyboard MOO ICT\\Data\\Map\\dungeon.json");
            dungeons = JsonSerializer.Deserialize<DungeonData>(jsonString);
            jsonString = File.ReadAllText("\\\\Mac\\Home\\Desktop\\HallowBlade\\Timer and Keyboard MOO ICT\\Data\\Map\\note.json");
            notesData = JsonSerializer.Deserialize<NotesData>(jsonString);
        }
        public void Render((int, int) area) 
        {
            RemoveObjects();

            foreach (Screen s in screens.Screens)
            {
                if (s.Position.X == area.Item1 &&  s.Position.Y == area.Item2)
                {
                    DrawBlocks(s.Layout);
                    DrawEnemies(s.Enemies);
                    DrawNotes(area);
                    break;
                }
            }
        }

        public void RenderDungeon((int, int) dungeonArea, char Dungeon)
        {
            RemoveObjects();

            foreach (ScreenDataDungeon sd in dungeons.Dungeons)
            {
                if (sd.Dungeon == Dungeon.ToString())
                {
                    foreach (Screen s in  sd.Screens)
                    {
                        if (s.Position.X == dungeonArea.Item1 && s.Position.Y == dungeonArea.Item2)
                        {
                            DrawBlocks(s.Layout);
                            DrawEnemies(s.Enemies);
                            break;
                        }
                    }
                }
            }
        }

        private void RemoveObjects()
        {
            foreach (Block bl in blocks)
                myCanvas.Children.Remove(bl.BlockRect);
            foreach (IEnemy enemy in enemies)
                myCanvas.Children.Remove(enemy.EnemyRectangle);
            blocks.Clear();
            enemies.Clear();
        }

        private void DrawBlocks(List<string> lvl)
        {
            int x = 0, y = 100;
            foreach (string row in lvl)
            {
                foreach (char col in row)
                {
                    if (col != '.' && col != '\n')
                    {
                        var bl = new Block(col, x, y);
                        this.myCanvas.Children.Add(bl.BlockRect);
                        Canvas.SetTop(bl.BlockRect, y);
                        Canvas.SetLeft(bl.BlockRect, x);
                        blocks.Add(bl);
                    }
                    x += 50;
                }
                y += 50;
                x = 0;
            }
        }

        private void DrawEnemies(List<Enemy> enemies1)
        {
            foreach (Enemy enemy in enemies1)
            {
                IEnemy en = null;
                if (enemy.Type == "Ghost")
                    en = new Ghost(enemy.Position.X * 50, 100 + enemy.Position.Y * 50, myCanvas);
                else if (enemy.Type == "Zombie")
                    en = new Zombie(enemy.Position.X * 50, 100 + enemy.Position.Y * 50, myCanvas, blocks);
                else if (enemy.Type == "Vampire")
                    en = new Vampire(enemy.Position.X * 50, 100 + enemy.Position.Y * 50, myCanvas, blocks);
                else if (enemy.Type == "Boss")
                    en = new Boss(enemy.Position.X * 50, 100 + enemy.Position.Y * 50, myCanvas, blocks, tempEnemies);
                Canvas.SetTop(en.EnemyRectangle, 100 + enemy.Position.Y * 50);
                Canvas.SetLeft(en.EnemyRectangle, enemy.Position.X * 50);
                enemies.Add(en);
            }
        }

        private void DrawNotes((int, int) area)
        {
            foreach (Note note in notesData.Notes)
            {
                if (note.ScreenPosition.X == area.Item1 && note.ScreenPosition.Y == area.Item2)//&& notes[note.Id] != null)
                {
                    var bl = new Block('⎕', note.Position.X * 50, note.Position.Y * 50);
                    bl.Id = note.Id;
                    myCanvas.Children.Add(bl.BlockRect);
                    Canvas.SetTop(bl.BlockRect, note.Position.Y * 50);
                    Canvas.SetLeft(bl.BlockRect, note.Position.X * 50);
                    blocks.Add(bl);

                    notes[note.Id] = note.NoteString;
                }
            }
        }

        private void DrawItems()
        {

        }
    }
}
