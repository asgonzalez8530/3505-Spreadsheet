// Anastasia Gonzalez and Aaron Bellis UID: u0985898 & u0981638
// Code implemented as part of PS7 : SpaceWars client CS3500 Fall Semester
using SpaceWars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SpaceWarsView
{
    public partial class SpaceWarsForm : Form, ISpaceWarsWindow
    {
        private WorldPanel worldPanel; // panel that will draw the game
        private ScoreBoardPanel scorePanel; // panel that wil draw the score panel
        private System.Timers.Timer frameTimer; // simulates the amount of frames per millisecond

        public SpaceWarsForm()
        {
            InitializeComponent();

            // Create a WorldPanel and add it to this form
            worldPanel = new WorldPanel();
            worldPanel.Location = new Point(0, 24);
            worldPanel.Size = new Size(1145, 1145);
            worldPanel.BackColor = Color.White;
            worldPanel.Visible = true;
            this.Controls.Add(worldPanel);

            // Create a ScoreBoardPanel and add it to this form
            scorePanel = new ScoreBoardPanel();
            scorePanel.Location = new Point(763, 24);
            scorePanel.Size = new Size(266, 662);
            scorePanel.Visible = true;
            scorePanel.SetWorld(worldPanel.GetWorld());
            this.Controls.Add(scorePanel);

            // Start a new timer that will redraw the game every 15 milliseconds 
            // This should correspond to about 67 frames per second.
            ResetFrameTimer();
        }

        // Redraw the game. This method is invoked every time the "frameTimer"
        // above ticks.
        private void Redraw(object sender, ElapsedEventArgs e)
        {
            // Invalidate this form and all its children (true)
            // This will cause the form to redraw as soon as it can
            MethodInvoker newInvoker = () => this.Invalidate(true);

            // handle the object disposed exception which could occur when 
            // this window closes
            try
            {
                this.Invoke(newInvoker);
            }
            catch (InvalidOperationException)
            {
            }


        }

        

        // listeners
        public event Action enterConnectEvent;
        public event Action<KeyEventArgs> ControlKeyDownEvent;
        public event Action<KeyEventArgs> ControlKeyUpEvent;
        public event Action ControlMenuClick;
        public event Action AboutMenuClick;

        /// <summary>
        /// Gives the game model represented by theWorld object to 
        /// the view objects which need to draw it. 
        /// </summary>
        public void SetWorld(World theWorld)
        {
            worldPanel.SetWorld(theWorld);
            scorePanel.SetWorld(theWorld);
        }

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

        /// <summary>
        /// Sets the default accept button to connectButton
        /// </summary>
        public void SetDefaultAcceptButton()
        {
            AcceptButton = connectButton;
        }

        /// <summary>
        /// Sets serverTextBox text to s
        /// </summary>
        public void SetServerString(string s)
        {
            serverTextBox.Text = s;
        }

        /// <summary>
        /// Sets the nameTextBox text to s
        /// </summary>
        public void SetUserName(string s)
        {
            nameTextBox.Text = s;
        }
        
        /// <summary>
        /// Form designer defined event handler for connectButton
        /// fires enterConnectEvent
        /// </summary>
        private void connectButton_Click(object sender, EventArgs e)
        {
            enterConnectEvent();
        }

        /// <summary>
        /// Tells the form to set user box inactive
        /// </summary>
        public void ToggleUserBoxControl(bool active)
        {
            MethodInvoker newInvoker = () => nameTextBox.Enabled = active;
            // handle the object disposed exception which could occur when 
            // this window closes
            try
            {
                this.Invoke(newInvoker);
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Tells the form to set server box inactive
        /// </summary>
        public void ToggleServerBoxControl(bool active)
        {
            
            MethodInvoker newInvoker = () => serverTextBox.Enabled = active;
            // handle the object disposed exception which could occur when 
            // this window closes
            try
            {
                this.Invoke(newInvoker);
            }
            catch (InvalidOperationException)
            {
            }
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
        public void ToggleConnectButtonControl(bool active)
        {
            
            MethodInvoker newInvoker = () => connectButton.Enabled = active;
            // handle the object disposed exception which could occur when 
            // this window closes
            try
            {
                this.Invoke(newInvoker);
            }
            catch (InvalidOperationException)
            {
            }
        }

        public World GetWorldPanelWorld()
        {
            return worldPanel.GetWorld();
        }

        public void UpdateWorldSize(int worldSize)
        {
            MethodInvoker newInvoker = () => resizeWindow(worldSize);

            // handle the object disposed exception which could occur when 
            // this window closes
            try
            {
                this.Invoke(newInvoker);
            }
            catch (InvalidOperationException)
            {
            }

        }

        /// <summary>
        /// resizes the window size based of the worldSize
        /// </summary>
        /// <param name="worldSize"></param>
        private void resizeWindow(int worldSize)
        {
            // update worldPanel size
            worldPanel.Size = new Size(worldSize, worldSize);

            // set the height of the scoreboard
            scorePanel.Height = tableLayoutPanel.Height + worldSize;
            scorePanel.Location = new Point(worldSize, tableLayoutPanel.Height);
            
            // set this window height and width
            Width = worldSize + scorePanel.Width + 30;
            Height = worldSize + tableLayoutPanel.Height + 40;
        }

        /// <summary>
        /// fire keypress event
        /// </summary>
        private void SpaceWarsForm_KeyDown(object sender, KeyEventArgs e)
        {
            ControlKeyDownEvent(e);
        }

        /// <summary>
        /// fire key not pressed event
        /// </summary>
        private void SpaceWarsForm_KeyUp(object sender, KeyEventArgs e)
        {
            ControlKeyUpEvent(e);
        }

        /// <summary>
        /// Gets the current frame time
        /// </summary>
        public System.Timers.Timer GetFrameTimer()
        {
            return frameTimer;
        }

        /// <summary>
        /// fires the control menu event
        /// </summary>
        private void controls_Click(object sender, EventArgs e)
        {
            ControlMenuClick();
        }

        /// <summary>
        /// fires the about menu event
        /// </summary>
        private void about_Click(object sender, EventArgs e)
        {
            AboutMenuClick();
        }

        // Start a new timer that will redraw the game every 15 milliseconds 
        // This should correspond to about 67 frames per second.
        public void ResetFrameTimer()
        {
            if (frameTimer != null)
            {
                frameTimer.Stop();
                frameTimer.Dispose();
            }
            
            // Start a new timer that will redraw the game every 15 milliseconds 
            // This should correspond to about 67 frames per second.
            frameTimer = new System.Timers.Timer();
            frameTimer.Interval = 15;
            frameTimer.Elapsed += Redraw;
            frameTimer.Start();
        }
    }// end of class
}
