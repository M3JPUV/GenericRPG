using System.Windows.Forms;
using System.IO;

namespace GameLibrary {
  public struct Position {
    public int row;
    public int col;

    /// <summary>
    /// Construct a new 2D position
    /// </summary>
    /// <param name="row">Given row or y value</param>
    /// <param name="col">Given col or x value</param>
    public Position(int row, int col) {
      this.row = row;
      this.col = col;
    }
  }

  /// <summary>
  /// This represents our player in our game
  /// </summary>
  public class Character : Mortal {
    public PictureBox Pic { get; private set; }
    private Position pos;
    private Map map;
    private Inventory inventory;
    public float XP { get; private set; }
    public bool ShouldLevelUp { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pb"></param>
    /// <param name="pos"></param>
    /// <param name="map"></param>
    public Character(PictureBox pb, Position pos, Map map) : base("Player 1", 1) {
      Pic = pb;
      this.pos = pos;
      this.map = map;
      this.inventory = new Inventory();
      ShouldLevelUp = false;
    }

    public void GainXP(float amount) {
      XP += amount;

      // every 100 experience points you gain a level
      if ((int)XP / 100 >= Level) {
        ShouldLevelUp = true;
      }
    }

    public override void LevelUp() {
      base.LevelUp();
      ShouldLevelUp = false;
    }

    public void BackToStart() {
      pos.row = map.CharacterStartRow;
      pos.col = map.CharacterStartCol;
      Position topleft = map.RowColToTopLeft(pos);
      Pic.Left = topleft.col;
      Pic.Top = topleft.row;
    }
    
    public override void ResetStats() {
      base.ResetStats();
      XP = 0;
    }
        //Retrieve the inventory instance associated with this character instance
    public Inventory getInv()
    {
        return this.inventory;
    }
    public void SetStats(string statfile){
      using (FileStream fs = new FileStream(statfile, FileMode.Open)) {
        using (StreamReader sr = new StreamReader(fs)) {
          string line = sr.ReadLine();
          int i = 0;
          float fvar;
          int ivar;
          while (line != null){
            switch (i) {
              case 0:
                float.TryParse(line, out fvar);
                Health = fvar;
                break;
              case 1:
                float.TryParse(line, out fvar);
                MaxHealth = fvar;
                break;
              case 2:
                float.TryParse(line, out fvar);
                Mana = fvar;
                break;
              case 3:
                float.TryParse(line, out fvar);
                MaxMana = fvar;
                break;
              case 4:
                float.TryParse(line, out fvar);
                Str = fvar;
                break;
              case 5:
                float.TryParse(line, out fvar);
                Def = fvar;
                break;
              case 6:
                float.TryParse(line, out fvar);
                Luck = fvar;
                break;
              case 7:
                float.TryParse(line, out fvar);
                Speed = fvar;
                break;
              case 8:
                float.TryParse(line, out fvar);
                XP = fvar;
                break;
              case 9:
                ShouldLevelUp = (line == "true" ? true : false);
                break;
              case 10:
                int.TryParse(line, out ivar);
                Level = ivar;
                break;
              case 11:
                Name = line;
                break;
            }
            line=sr.ReadLine();
            i++;
          }
        }
      }
    }

    public void Move(MoveDir dir) {
      Position newPos = pos;
      switch (dir) {
        case MoveDir.UP:
          newPos.row--;
          break;
        case MoveDir.DOWN:
          newPos.row++;
          break;
        case MoveDir.LEFT:
          newPos.col--;
          break;
        case MoveDir.RIGHT:
          newPos.col++;
          break;
      }
      if (map.IsValidPos(newPos, map.CheckX, map.CheckY, this)) {
        pos = newPos;
        Position topleft = map.RowColToTopLeft(pos);
        Pic.Left = topleft.col;
        Pic.Top = topleft.row;
      }
    }
  }
}
