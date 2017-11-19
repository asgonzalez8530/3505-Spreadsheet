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

namespace View
{
    public partial class ScoreBoardPanel : Panel
    {
        private World theWorld;

        // TODO: resize this panel
        // TODO: comment
        // TODO: show panel
        public ScoreBoardPanel()
        {
            InitializeComponent();
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

            // TODO: fix these to be able to be resized. 
            // y stays the same
            // x = worldSize + a constant number;
            int x = -700;
            int y = 650;
            int width = 30;
            int height = 10;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;

            //foreach (Ship s in theWorld.GetShips())
            //{
                // This is the outside rectangle
                Rectangle outsideRec = new Rectangle(x, y, width * 5, height);
                e.Graphics.DrawRectangle(new Pen(HealthBarColor(s)), outsideRec);

                // This is the inside rectangle
                Rectangle insideRec = new Rectangle(x, y, width * s.GetID(), height);
                e.Graphics.FillRectangle(new SolidBrush(HealthBarColor(s)), insideRec);
            //}
        }

        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs pe)
        {
            lock (theWorld)
            {
                // Draw the ships
                foreach (Ship ship in theWorld.GetShips())
                {
                    //System.Diagnostics.Debug.WriteLine("drawing player at " + p.GetLocation());
                    //DrawObjectWithTransform(e, ship, this.Size.Width, ship.GetLocation().GetX(), ship.GetLocation().GetY(), ship.GetDirection().ToAngle(), HealthBarDrawer);
                    HealthBarDrawer(ship, pe);
                }
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
                    return Color.Violet;
                case 7:
                    return Color.Yellow;
            }
            return Color.Pink;
        }
    }
}
