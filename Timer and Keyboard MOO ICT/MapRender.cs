﻿using System;
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
        public string[] notes = new string[20];
        public MapRender(Canvas myCanvas) 
        {
            this.myCanvas = myCanvas; 
            blocks = new List<Block>();
            
        }
        public void Render((int, int) area) 
        {
            foreach (Block bl in blocks)
            {
                myCanvas.Children.Remove(bl.BlockRect);
            }
            blocks.Clear();

            string jsonString = File.ReadAllText("\\\\Mac\\Home\\Desktop\\WPF-Move-Rectangle-In-Canvas-Using-Keyboard-and-Timer-main\\Timer and Keyboard MOO ICT\\Data\\Map\\outer.json");
            
            ScreensData screens = JsonSerializer.Deserialize<ScreensData>(jsonString);
            List<string> lvl = new List<string>();
            foreach (Screen s in screens.Screens)
            {
                if (s.Position.X == area.Item1 &&  s.Position.Y == area.Item2)
                {
                    lvl = s.Layout;
                }
            }

            DrawBlocks(lvl);
            DrawNotes(area);
            
        }

        public void RenderDungeon((int, int) dungeonArea, char Dungeon)
        {
            foreach (Block bl in blocks)
            {
                myCanvas.Children.Remove(bl.BlockRect);
            }
            blocks.Clear();
            string jsonString = File.ReadAllText("\\\\Mac\\Home\\Desktop\\WPF-Move-Rectangle-In-Canvas-Using-Keyboard-and-Timer-main\\Timer and Keyboard MOO ICT\\Data\\Map\\dungeon.json");

            DungeonData dungeons = JsonSerializer.Deserialize<DungeonData>(jsonString);
            List<string> lvl = new List<string>();
            foreach (ScreenDataDungeon sd in dungeons.Dungeons)
            {
                if (sd.Dungeon == Dungeon.ToString())
                {
                    foreach (Screen s in  sd.Screens)
                    {
                        if (s.Position.X == dungeonArea.Item1 && s.Position.Y == dungeonArea.Item2)
                        {
                            lvl = s.Layout;
                        }
                    }
                }
                
            }
            DrawBlocks(lvl);
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

        private void DrawNotes((int, int) area)
        {
            string jsonString = File.ReadAllText("\\\\Mac\\Home\\Desktop\\WPF-Move-Rectangle-In-Canvas-Using-Keyboard-and-Timer-main\\Timer and Keyboard MOO ICT\\Data\\Map\\note.json");
            NotesData notesData = JsonSerializer.Deserialize<NotesData>(jsonString);
            foreach (Note note in notesData.Notes)
            {
                if (note.Position.X == area.Item1 && note.Position.Y == area.Item2 )//&& notes[note.Id] != null)
                {
                    var bl = new Block('⎕', note.ScreenPosition.X * 50, note.ScreenPosition.Y * 50);
                    bl.Id = note.Id;
                    myCanvas.Children.Add(bl.BlockRect);
                    Canvas.SetTop(bl.BlockRect, note.ScreenPosition.Y * 50);
                    Canvas.SetLeft(bl.BlockRect, note.ScreenPosition.X * 50);
                    blocks.Add(bl);

                    notes[note.Id] = note.NoteString;
                }
            }
        }
    }
}