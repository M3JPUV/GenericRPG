using GameLibrary;
using GenericRPG.Properties;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace GenericRPG {
  public partial class FrmArena : Form {
    private Game game;
    private Character character;
    private Inventory inventory;
    private Enemy enemy;
    private Random rand;
    private string enemytype;

    public FrmArena(Inventory inv,string type) {
      enemytype = type;
      inventory = inv;
      InitializeComponent();
    }
    private void btnEndFight_Click(object sender, EventArgs e) {
      EndFight();
    }
    private void EndFight() {
      Game.GetGame().ChangeState(GameState.ON_MAP);
      Close();
      // if enemy is boss potentially move to win state (possible future improvement)
      if (enemytype == "boss")
            {
                Game.GetGame().ChangeState(GameState.WIN);
            }
    }
    private void FrmArena_Load(object sender, EventArgs e) {
      rand = new Random();

      game = Game.GetGame();
      character = game.Character;
      // check for what type of enemy to load either regular or boss
      if (enemytype == "reg"){
         enemy = new Enemy(rand.Next(character.Level + 1), Resources.enemy, Enemy.RandName());
       }
      if (enemytype == "boss"){
        enemy = new Boss(character.Level, Resources.boss);      
      }
      // stats
      UpdateStats();

      // pictures
      picCharacter.BackgroundImage = character.Pic.BackgroundImage;
      picEnemy.BackgroundImage = enemy.Img;

      // names
      lblPlayerName.Text = character.Name;
      lblEnemyName.Text = enemy.Name;
    }
    public void UpdateStats() {
      lblPlayerLevel.Text = character.Level.ToString();
      lblPlayerHealth.Text = Math.Round(character.Health).ToString();
      lblPlayerStr.Text = Math.Round(character.Str).ToString();
      lblPlayerDef.Text = Math.Round(character.Def).ToString();
      lblPlayerMana.Text = Math.Round(character.Mana).ToString();
      lblPlayerXp.Text = Math.Round(character.XP).ToString();

      lblEnemyLevel.Text = enemy.Level.ToString();
      lblEnemyHealth.Text = Math.Round(enemy.Health).ToString();
      lblEnemyStr.Text = Math.Round(enemy.Str).ToString();
      lblEnemyDef.Text = Math.Round(enemy.Def).ToString();
      lblEnemyMana.Text = Math.Round(enemy.Mana).ToString();

      lblPlayerHealth.Text = Math.Round(character.Health).ToString();
      lblEnemyHealth.Text = Math.Round(enemy.Health).ToString();
            //Update the potion counter on screen with the other stats
            if (inventory.PotionCount() == 0)
            {
                PotionNum.Text = "You have " + inventory.PotionCount() + " potions. \nBeat enemies to get more!";
            }
            else
            {
                PotionNum.Text = "You have " + inventory.PotionCount() + " potions. \nUse them to enhance your character!";
            }

        }
    private void btnSimpleAttack_Click(object sender, EventArgs e) {
      float prevEnemyHealth = enemy.Health;
      character.SimpleAttack(enemy);
      float enemyDamage = (float)Math.Round(prevEnemyHealth - enemy.Health);
      lblEnemyDamage.Text = enemyDamage.ToString();
      lblEnemyDamage.Visible = true;
      tmrEnemyDamage.Enabled = true;
      if (enemy.Health <= 0) {
        //Generate a number 0-9 then if that number is > 4 give the player a potion
        int potion = rand.Next(10);
        if (potion > 4)
        {
            //Set the potioncount higher in the inventory
            inventory.getPotion();
            label10.Text = "You got a potion!!!";
            label10.Visible = true;
        }
        
        character.GainXP(enemy.XpDropped);
        lblEndFightMessage.Text = "You Gained " + Math.Round(enemy.XpDropped) + " xp!";
        lblEndFightMessage.Visible = true;
        Refresh();
        Thread.Sleep(1200);
        EndFight();
        //Once the fight ends (but before levelling up) reset the characters strength and def stats (in case postions were used.)
        character.ResetStrDef();
        if (character.ShouldLevelUp) {
          FrmLevelUp frmLevelUp = new FrmLevelUp();
          frmLevelUp.Show();
        }
      }
      else {
        float prevPlayerHealth = character.Health;
        enemy.SimpleAttack(character);
        float playerDamage = (float)Math.Round(prevPlayerHealth - character.Health);
        lblPlayerDamage.Text = playerDamage.ToString();
        lblPlayerDamage.Visible = true;
        tmrPlayerDamage.Enabled = true;
        if (character.Health <= 0) {
          UpdateStats();
                    character.ResetStrDef();
                    game.ChangeState(GameState.DEAD);
          lblEndFightMessage.Text = "You Were Defeated!";
          lblEndFightMessage.Visible = true;
          Refresh();
          Thread.Sleep(1200);
          EndFight();
          FrmGameOver frmGameOver = new FrmGameOver();
          frmGameOver.Show();
        }
        else {
          UpdateStats();
        }
      }
    }
    private void btnRun_Click(object sender, EventArgs e) {
            // if not boss allow normal run
            if (enemytype != "boss")
            {
                if (rand.NextDouble() < 0.25)
                {
                    lblEndFightMessage.Text = "You Ran Like a Coward!";
                    lblEndFightMessage.Visible = true;
                    Refresh();
                    Thread.Sleep(1200);
                    EndFight();
                }
                else
                {
                    enemy.SimpleAttack(character);
                    if (character.Health <= 0)
                    {
                        UpdateStats();
                        game.ChangeState(GameState.DEAD);
                        lblEndFightMessage.Text = "You Were Defeated!";
                        lblEndFightMessage.Visible = true;
                        Refresh();
                        Thread.Sleep(1200);
                        EndFight();
                        FrmGameOver frmGameOver = new FrmGameOver();
                        frmGameOver.Show();
                    }
                    else
                    {
                        UpdateStats();
                    }
                }
            }
            // if boss fight then do not allow run
            else
            {
                lblEndFightMessage.Text = "You Can't Run From This!";
                lblEndFightMessage.Visible = true;
                Refresh();
                Thread.Sleep(1200);
                lblEndFightMessage.Visible = false;
                Refresh();
            }
    }
         
    private void tmrPlayerDamage_Tick(object sender, EventArgs e) {
      lblPlayerDamage.Top -= 2;
      if (lblPlayerDamage.Top < 10) {
        lblPlayerDamage.Visible = false;
        tmrPlayerDamage.Enabled = false;
        lblPlayerDamage.Top = 52;
      }
    }

    private void tmrEnemyDamage_Tick(object sender, EventArgs e) {
      lblEnemyDamage.Top -= 2;
      if (lblEnemyDamage.Top < 10) {
        lblEnemyDamage.Visible = false;
        tmrEnemyDamage.Enabled = false;
        lblEnemyDamage.Top = 52;
      }
    }

        private void lblEndFightMessage_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (inventory.PotionCount() >= 3)
            {
                character.increaseHealth(10);
                inventory.decreasePotionCount(3);
                UpdateStats();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (inventory.PotionCount() >= 2)
            {
                character.increaseStr(4);
                inventory.decreasePotionCount(2);
                UpdateStats();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (inventory.PotionCount() >= 1)
            {
                character.increaseDef(3);
                inventory.decreasePotionCount(1);
                UpdateStats();
            }
        }
    }
}
