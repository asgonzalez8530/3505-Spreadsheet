using System.Drawing;
using System.Windows.Forms;

namespace SpaceWars
{
    /// <summary>
    /// The panel where the scoreboard lives
    /// </summary>
    public class ScoreboardPanel: Panel
    {
        World theWorld;

        public ScoreboardPanel(World w)
        {
            theWorld = w;
            Size = new Size(200, 100);
            DoubleBuffered = true;
        }


        /// <summary>
        /// Redraws the scoreboard in order of score.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            lock (theWorld.shipLock)
            {
                using (SolidBrush brush = new SolidBrush(Color.DarkBlue))
                using (Font font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0))
                {
                    int yLocation = 0;
                    foreach (Ship s in theWorld.GetShips())
                    {
                        // resize the panel as needed
                        if (Height < yLocation + 60)
                        {
                            Height = yLocation + 60;
                        }

                        // draw score
                        e.Graphics.DrawString(s.GetName() + ": " + s.GetScore(), font, brush, new Point(0, yLocation));
                        yLocation += 30;
                    }
                }
            }
            base.OnPaint(e);
        }

    }
}
