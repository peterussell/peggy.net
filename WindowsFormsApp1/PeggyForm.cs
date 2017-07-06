﻿using PeggyTheGameApp.src;
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

            cmdInput.Text = "";
            outputText.AppendText($" > {cmd}\r\n");
            outputText.AppendText(_gc.HandleCommand(cmd));
        }
    }
}
