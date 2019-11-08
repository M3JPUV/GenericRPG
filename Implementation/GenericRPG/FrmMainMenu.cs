using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace GenericRPG
{
    public partial class FrmMainMenu : Form
    {
        //Init
        public FrmMainMenu()
        {
            InitializeComponent();
        }

        public void ShowMain()
        {
            this.Show();
        }
        //New game button
        private void button2_Click(object sender, EventArgs e)  // maybe we can set the GameState to LVL1 after clicking this?
        {
            //Create a new map form without loading then show it and close the menu
            var newForm = new FrmMap(false, false);
            newForm.Show();
            this.Close();
        }

        private void FrmMainMenu_Load(object sender, EventArgs e)
        {
            
        }
        //Exit game button
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Close();
        }
        //Load game button
        private void button3_Click(object sender, EventArgs e)
        {
            //Create a new map form with loading = true, show it, then close the current form
            var newForm = new FrmMap(false, true);      // open at start of game or when walking on quit space
            newForm.Show();
            this.Close();
        }
    }
}
