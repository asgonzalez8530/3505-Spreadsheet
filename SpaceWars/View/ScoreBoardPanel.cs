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
    public partial class ScoreBoardPanel : Panel
    {
        private World theWorld;
        private int count = 0; // counts the amount of players we have 
        
        public ScoreBoardPanel()
        {
            DoubleBuffered = true;
            theWorld = new World();
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void HealthBarDrawer(object o, PaintEventArgs e)
        {
            Ship s = o as Ship;

            int x = 10; // start drawing the score board here
            int y = 10; // start drawing the score board here

            int xPadding = 30; // end the rectangle indicated pixels before the edge of the panel
            int yPadding = 30; // Spacing between the name and the health bar and between the scores 
            int width = Width - xPadding * 2; // width of the health bar
            int height = 20; // height of the health bar

            using (SolidBrush brush = new SolidBrush(HealthBarColor(s)))
            using (SolidBrush blackBrush = new SolidBrush(Color.Black))
            using (Font font = new Font(new FontFamily("Verdana"), 16))
            {
                int adjustedY = y + count * (yPadding * 2);
                // This prints the name and score of the current ship
                e.Graphics.DrawString(s.GetName() + ": " + s.GetScore(), font, blackBrush, x, adjustedY);

                // This is the outside rectangle
                Rectangle outsideRec = new Rectangle(x, adjustedY + yPadding, width, height);
                e.Graphics.DrawRectangle(new Pen(HealthBarColor(s)), outsideRec);

                // This is the inside rectangle
                Rectangle insideRec = new Rectangle(x, adjustedY + yPadding, s.GetHP() * width , height);
                e.Graphics.FillRectangle(brush, insideRec);
            }
        }

        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs pe)
        {
            lock (theWorld)
            {
                // Draw the ships
                foreach (Ship ship in theWorld.GetAllShips())
                {
                    HealthBarDrawer(ship, pe);
                    count++;
                }
                count = 0;
            }
            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(pe);
        }

        private Color HealthBarColor(Ship s)
        {
            switch (s.GetID() % 8)
            {
                case 0:
                    return Color.Black;
                case 1:
                    return Color.Blue;
                case 2:
                    return Color.Brown;
                case 3:
                    return Color.Green;
                case 4:
                    return Color.Gray;
                case 5:
                    return Color.Red;
                case 6:
                    return Color.Purple;
                default:
                    return Color.Yellow;
            }
        }

        /// <summary>
        /// Sets the World object w to theWorld
        /// </summary>
        public void SetWorld(World w)
        {
            theWorld = w;
        }
    }
}
