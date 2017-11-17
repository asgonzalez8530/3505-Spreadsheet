using SpaceWars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceWarsView
{
    public partial class SpaceWarsForm : Form, ISpaceWarsWindow
    {
        World theWorld;
        WorldPanel worldPanel;

        public SpaceWarsForm()
        {
            InitializeComponent();
            theWorld = new World();

            worldPanel = new WorldPanel(theWorld);
            worldPanel.Location = new Point(0, 0);
            worldPanel.Size = new Size(1145, 1145);
            worldPanel.BackColor = Color.Black;
            worldPanel.Visible = true;
            this.Controls.Add(worldPanel);
        }

        public event Action enterConnectEvent;

        /// <summary>
        /// Returns the string passed in to the server text box
        /// </summary>
        public string GetServer()
        {
            return serverTextBox.Text;
        }

        /// <summary>
        /// Returns the string passed in to the name text box
        /// </summary>
        public string GetUserName()
        {
            return nameTextBox.Text;
        }

        public void SetDefaultAcceptButton()
        {
            AcceptButton = connectButton;
        }

        public void SetServerString(string s)
        {
            serverTextBox.Text = s;
        }

        public void SetUserName(string s)
        {
            nameTextBox.Text = s;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            enterConnectEvent();
        }

        /// <summary>
        /// Tells the form to set user box inactive
        /// </summary>
        public void SetUserBoxInactive()
        {
            nameTextBox.Enabled = false;
        }

        /// <summary>
        /// Tells the form to set server box inactive
        /// </summary>
        public void SetServerBoxInactive()
        {
            serverTextBox.Enabled = false;
        }

        /// <summary>
        /// Displays a messagebox with s as the message
        /// </summary>
        public void DisplayMessageBox(string s)
        {
            MessageBox.Show(s);
        }

        /// <summary>
        /// Disables connectButton
        /// </summary>
        public void SetConnectButtonInactive()
        {
            connectButton.Enabled = false;
        }

    }// end of class
}
