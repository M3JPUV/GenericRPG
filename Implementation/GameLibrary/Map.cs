using GameLibrary;
using System.IO;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace GameLibrary {
  public class Map {
    private int[,] layout;
    private const int TOP_PAD = 10;
    private const int BOUNDARY_PAD = 5;
    private const int BLOCK_SIZE = 50;
    public double encounterChance = 0;
    private Random rand;

    public bool STOP_ENCOUNTER = false;     // stop encounters

    public int CharacterStartRow { get; private set; }
    public int CharacterStartCol { get; private set; }
    private int NumRows { get { return layout.GetLength(0); } }
    private int NumCols { get { return layout.GetLength(1); } }
    public int CheckX { get; private set; }
    public int CheckY { get; private set; }
    public string CurrentMap { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapFile"></param>
        /// <param name="grpMap"></param>
        /// <param name="LoadImg"></param>
        /// <returns></returns>
        /// 
    
    public Character LoadMap(string mapFile, GroupBox grpMap, Func<string, Bitmap> LoadImg) {
      grpMap.Controls.Clear();
      // declare and initialize locals
      int top = TOP_PAD;
      int left = BOUNDARY_PAD;
      CurrentMap = mapFile;
      Console.WriteLine(CurrentMap);
      Character character = null;
      List<string> mapLines = new List<string>();

      // read from map file
      using (FileStream fs = new FileStream(mapFile, FileMode.Open)) {
        using (StreamReader sr = new StreamReader(fs)) {
          string line = sr.ReadLine();
          while (line != null) {
            mapLines.Add(line);
            line = sr.ReadLine();
          }
        }
      }

      // load map file into layout and create PictureBox objects
      layout = new int[mapLines.Count, mapLines[0].Length];
      int i = 0;
      foreach (string mapLine in mapLines) {
        int j = 0;
        foreach (char c in mapLine) {
          int val = c - '0';
          layout[i, j] = (val);
          PictureBox pb = CreateMapCell(val, LoadImg);
          if (pb != null) {
            pb.Top = top;
            pb.Left = left;
            grpMap.Controls.Add(pb);
          }
          if (val == 2) {
            CharacterStartRow = i;
            CharacterStartCol = j;
            character = new Character(pb, new Position(i, j), this);
          }
          if (val == 3) {
            CheckX = j;
            CheckY = i;
          }
          left += BLOCK_SIZE;
          j++;
        }
        left = BOUNDARY_PAD;
        top += BLOCK_SIZE;
        i++;
      }

      // resize Group
      grpMap.Width = NumCols * BLOCK_SIZE + BOUNDARY_PAD * 2;
      grpMap.Height = NumRows * BLOCK_SIZE + TOP_PAD + BOUNDARY_PAD;
      grpMap.Top = 5;
      grpMap.Left = 5;

            // initialize for game
        if (STOP_ENCOUNTER == false) {
            encounterChance = 0.15;
        }

      rand = new Random();
      Game.GetGame().ChangeState(GameState.ON_MAP);

      // return Character object from reading map
      return character;
    }

    private PictureBox CreateMapCell(int legendValue, Func<string, Bitmap> LoadImg) {
      PictureBox result = null;
      switch (legendValue) {
        // walkable
        case 0:
          break;

        // wall
        case 1:
          result = new PictureBox() {
            BackgroundImage = LoadImg("wall"),
            BackgroundImageLayout = ImageLayout.Stretch,
            Width = BLOCK_SIZE,
            Height = BLOCK_SIZE
          };
          break;

        // character
        case 2:
          result = new PictureBox() {
            BackgroundImage = LoadImg("character"),
            BackgroundImageLayout = ImageLayout.Stretch,
            Width = BLOCK_SIZE,
            Height = BLOCK_SIZE
          };
          break;

        // checkpoint
        case 3:
           result = new PictureBox()
            {
             BackgroundImage = LoadImg("checkpoint"),
             BackgroundImageLayout = ImageLayout.Stretch,
             Width = BLOCK_SIZE,
             Height = BLOCK_SIZE
           };
           break;
          

        // boss
        case 4:
          result = new PictureBox() {
            BackgroundImage = LoadImg("fightboss"),
            BackgroundImageLayout = ImageLayout.Stretch,
            Width = BLOCK_SIZE,
            Height = BLOCK_SIZE
          };
          break;

        // quit
        case 5:
          result = new PictureBox() {
            BackgroundImage = LoadImg("quitgame"),
            BackgroundImageLayout = ImageLayout.Stretch,
            Width = BLOCK_SIZE,
            Height = BLOCK_SIZE
          };
          break;

        // next level
        case 6:                             
          result = new PictureBox() {
            BackgroundImage = LoadImg("level2"),
            BackgroundImageLayout = ImageLayout.Stretch,
            Width = BLOCK_SIZE,
            Height = BLOCK_SIZE
          };
          break;
      }
      return result;
    }

    public bool IsValidPos(Position pos, int cX, int cY, Character character) {
      if (pos.row < 0 || pos.row >= NumRows ||
          pos.col < 0 || pos.col >= NumCols ||
          layout[pos.row, pos.col] == 1) {
        return false;
      }

      if (layout[pos.row, pos.col] == 6)
            {
                Game.GetGame().ChangeState(GameState.LVL2);     // checks location for lvl2 space
            }
      else if (layout[pos.row, pos.col] == 5)
            {
                Game.GetGame().ChangeState(GameState.TITLE_SCREEN);     // checks location for quit space
            }
      // check if on boss space and set gamestate to boss if true
      else if (pos.row == 3 & pos.col == 9){
        Game.GetGame().ChangeState(GameState.BOSS);
      }
      else if (pos.row == cY && pos.col == cX) {

        this.CharacterStartCol = cX;
        this.CharacterStartRow = cY;
        if(!Directory.Exists("Resources")){
          Directory.CreateDirectory("Resources");
        } else {
          if(File.Exists("Resources/savedmap.txt")){
            File.Delete("Resources/savedmap.txt");
          }
          if(File.Exists("Resources/savedcharacter.txt")){
            File.Delete("Resources/savedcharacter.txt");
          }
        }
        using (StreamWriter writer = new StreamWriter("Resources/savedcharacter.txt")){
            writer.WriteLine(character.Health);
            writer.WriteLine(character.MaxHealth);
            writer.WriteLine(character.Mana);
            writer.WriteLine(character.MaxMana);
            writer.WriteLine(character.Str);
            writer.WriteLine(character.Def);
            writer.WriteLine(character.Luck);
            writer.WriteLine(character.Speed);
            writer.WriteLine(character.XP);
            writer.WriteLine(character.ShouldLevelUp);
            writer.WriteLine(character.Level);
            writer.WriteLine(character.Name);
        }
        string[] file = new string[10];
        int lineNum = 0;
        if(this.CurrentMap == "Resources/savedmap.txt")
        {
            this.CurrentMap = "Resources/lvl2.txt";
        }
        using (StreamReader sr = new StreamReader(this.CurrentMap)) {
          string line = sr.ReadLine();
          while(line != null){
            file[lineNum] = line;
            line = sr.ReadLine();
            lineNum++;
          }
          for(int i=0; i<lineNum; i++){
            if(file[i].Contains("2")){
              int index = file[i].IndexOf("2");
              System.Text.StringBuilder strBuilder = new System.Text.StringBuilder(file[i]);
              strBuilder[index] = '0';
              file[i] = strBuilder.ToString();
            }
            if(file[i].Contains("3")){
              int index = file[i].IndexOf("3");
              System.Text.StringBuilder strBuilder = new System.Text.StringBuilder(file[i]);
              strBuilder[index] = '2';
              file[i] = strBuilder.ToString();
            }
          }
        }
        using (StreamWriter sw = new StreamWriter("Resources/savedmap.txt")) {
          for(int i=0; i<lineNum; i++){
            sw.WriteLine(file[i]);
          }
        }
      } 
        else {
            if (STOP_ENCOUNTER == false)
            {
                if (rand.NextDouble() < encounterChance)
                {
                    encounterChance = 0.15;
                    Game.GetGame().ChangeState(GameState.FIGHTING);
                }
                else
                {
                    encounterChance += 0.05;
                }
            }
        }

      return true;
    }

    public Position RowColToTopLeft(Position p) {
      return new Position(p.row * BLOCK_SIZE + TOP_PAD, p.col * BLOCK_SIZE + BOUNDARY_PAD);
    }
  }
}
