using PeggyTheGameApp.src;
using PeggyTheGameApp.src.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeggyTheGameApp
{
    public partial class PeggyForm : Form
    {
        GameController _gc = new GameController();

        public PeggyForm()
        {
            InitializeComponent();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            string cmd = cmdInput.Text;
            if (string.IsNullOrWhiteSpace(cmd)) { return; }

            // Reset text field
            cmdInput.Text = "";

            outputText.Text = outputText.Text.Insert(0, "\r\n");
            outputText.Text = outputText.Text.Insert(0, Utils.Capitalize(_gc.HandleCommand(cmd)));
            outputText.Text = outputText.Text.Insert(0, $" > {cmd}\r\n");

            if (_gc.CheckForEndGame())
            {
                outputText.Text = outputText.Text.Insert(0, $"{_gc.GetEndGameText()}\r\n");
                outputText.Text = outputText.Text.Insert(0, $">>>> THE END! <<<\r\n\r\n");
                cmdInput.Enabled = false;
            }
        }
    }
}
