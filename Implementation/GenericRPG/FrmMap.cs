﻿using GameLibrary;
using GenericRPG.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GenericRPG
{
    public partial class FrmMap : Form
    {
        private Character character;
        private Map map;
        private Game game;
        private Inventory inventory;
        public bool inCombat;

        public FrmMap(bool InCombat)
        {
            inCombat = InCombat;
            InitializeComponent();
        }

        private void FrmMap_Load(object sender, EventArgs e)    // currently, all calls to this ruin our acquired stats
        {
            game = Game.GetGame();

            map = new Map();
            inventory = new Inventory();

            character = map.LoadMap("Resources/lvl1.txt", grpMap,
               str => Resources.ResourceManager.GetObject(str) as Bitmap);

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
                    FrmArena frmArena = new FrmArena(inventory);
                    frmArena.Show();
                }
                if (game.State == GameState.LVL2) {  // if the player lands on the lvl 2 square, the old map is hidden and the new map is formed
                    this.Hide();
                    Game.GetGame().ChangeState(GameState.ON_MAP);
                    FrmMap frmMap = new FrmMap(false);
                    map = new Map();
                    character = map.LoadMap("Resources/lvl2.txt", grpMap,   // will have to resetup the "LoadMap()" function to not reset all stats
                    str => Resources.ResourceManager.GetObject(str) as Bitmap);
                    frmMap.Show();
                }
                if (game.State == GameState.LVL1) { // for when in lvl 2, go back to lvl 1 if on square
                    
                    Game.GetGame().ChangeState(GameState.ON_MAP);
                    
                    
                    character = map.LoadMap("Resources/lvl2.txt", grpMap,
                    str => Resources.ResourceManager.GetObject(str) as Bitmap);
                    Width = grpMap.Width + 25;
                    Height = grpMap.Height + 50;
                    character.BackToStart();
                }
            }
        }
    }
}
