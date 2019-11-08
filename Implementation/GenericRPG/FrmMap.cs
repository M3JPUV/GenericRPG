using GameLibrary;
using GenericRPG.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace GenericRPG
{
    public partial class FrmMap : Form
    {
        private Character character;
        private Map map;
        private Game game;
        private Inventory inventory;
        public bool inCombat;
        public bool load;


        public FrmMap(bool InCombat, bool l)
        {
            load = l;
            inCombat = InCombat;
            InitializeComponent();
        }

        private void FrmMap_Load(object sender, EventArgs e)    // currently, all calls to this ruin our acquired stats
        {
            game = Game.GetGame();

            inventory = new Inventory();
            if (this.map == null)
            {
                map = new Map();
            }
            if (this.load)
            {
                if (Directory.Exists("Resources"))
                {
                    if (File.Exists("Resources/savedmap.txt") && File.Exists("Resources/savedcharacter.txt"))   // if loading a saved game
                    {
                        if (game.State == GameState.LVL2)
                        {
                            character = map.LoadMap("Resources/lvl2.txt", grpMap,
                               str => Resources.ResourceManager.GetObject(str) as Bitmap);
                        }
                        else if (game.State == GameState.LVL1)
                        {
                            character = map.LoadMap("Resources/lvl1.txt", grpMap,
                               str => Resources.ResourceManager.GetObject(str) as Bitmap);
                        }
                        else
                        {
                            character = map.LoadMap("Resources/savedmap.txt", grpMap,
                              str => Resources.ResourceManager.GetObject(str) as Bitmap
                            );
                        }

                        character.SetStats("Resources/savedcharacter.txt");
                    }
                    else if (game.State == GameState.LVL2)
                    {
                        character = map.LoadMap("Resources/lvl2.txt", grpMap,
                           str => Resources.ResourceManager.GetObject(str) as Bitmap);
                    }
                    else
                    {
                        character = map.LoadMap("Resources/lvl1.txt", grpMap,
                           str => Resources.ResourceManager.GetObject(str) as Bitmap);
                    }
                }
            }
            else if (game.State == GameState.LVL2)
            {
                character = map.LoadMap("Resources/lvl2.txt", grpMap,
                   str => Resources.ResourceManager.GetObject(str) as Bitmap);
            }
            else   // default starting map (new game)
            { 
                map = new Map();
                character = map.LoadMap("Resources/lvl1.txt", grpMap,
                   str => Resources.ResourceManager.GetObject(str) as Bitmap);
            }

            Game.GetGame().ChangeState(GameState.ON_MAP);

            Width = grpMap.Width + 25;
            Height = grpMap.Height + 50;
            game.SetCharacter(character);
        }

        private void FrmMap_KeyDown(object sender, KeyEventArgs e)
        {
            MoveDir dir = MoveDir.NO_MOVE;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    dir = MoveDir.LEFT;
                    break;
                case Keys.Right:
                    dir = MoveDir.RIGHT;
                    break;
                case Keys.Up:
                    dir = MoveDir.UP;
                    break;
                case Keys.Down:
                    dir = MoveDir.DOWN;
                    break;
            }
            if (dir != MoveDir.NO_MOVE)
            {
                character.Move(dir);
                if (game.State == GameState.FIGHTING)
                {
                    FrmArena frmArena = new FrmArena(inventory, "reg");
                    frmArena.Show();
                }
                if (game.State == GameState.LVL2) {  // if the player lands on the lvl 2 square, the old map is hidden and the new map is formed
                    FrmMap frmMap = new FrmMap(true, true);
                    frmMap.Show();
                    this.Close();
                }
                else if (game.State == GameState.LVL1) { // for when in lvl 2, go back to lvl 1 if on square
                    FrmMap frmMap = new FrmMap(true, true);
                    frmMap.Show();
                    this.Close();
                } 
                else if (game.State == GameState.TITLE_SCREEN)
                {
                    var newForm = new FrmMainMenu();
                    newForm.Show();
                    this.Close();
                }
                if (game.State == GameState.BOSS)
                {
                    FrmArena frmArena = new FrmArena(inventory, "boss");
                    frmArena.Show();
                }
            }
        }
    }
}
