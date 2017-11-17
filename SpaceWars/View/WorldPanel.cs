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
    public partial class WorldPanel : Panel
    {

        private World theWorld;

        public WorldPanel(World w)
        {
            InitializeComponent();
            DoubleBuffered = true;
            theWorld = w;
        }

        /// <summary>
        /// Helper method for DrawObjectWithTransform
        /// </summary>
        /// <param name="size">The world (and image) size</param>
        /// <param name="w">The worldspace coordinate</param>
        /// <returns></returns>
        private static int WorldSpaceToImageSpace(int size, double w)
        {
            return (int)w + size / 2;
        }

        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);


        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldSize">The size of one edge of the world (assuming the world is square)</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // Perform the transformation
            int x = WorldSpaceToImageSpace(worldSize, worldX);
            int y = WorldSpaceToImageSpace(worldSize, worldY);
            e.Graphics.TranslateTransform(x, y);
            e.Graphics.RotateTransform((float)angle);
            // Draw the object 
            drawer(o, e);
            // Then undo the transformation
            e.Graphics.ResetTransform();
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void ShipDrawer(object o, PaintEventArgs e)
        {
            Ship s = o as Ship;

            int width = 30;
            int height = 30;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None; //.AntiAlias;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;

            // Rectangles are drawn starting from the top-left corner.
            // So if we want the rectangle centered on the player's location, we have to offset it
            // by half its size to the left (-width/2) and up (-height/2)
            Rectangle r = new Rectangle(-(width / 2), -(height / 2), width, height);
            //TODO: may need to do the ship rotation here

            string pathString = @"../../../Resources/Images/";
            string imageString = "ship-";
            imageString += s.HasThrust() ? "thrust-" : "coast-";

            switch (s.GetID() % 8)
            {

                case 0:
                    imageString += "blue.png";
                    break;
                case 1:
                    imageString += "brown.png";
                    break;
                case 2:
                    imageString += "green.png";
                    break;
                case 3:
                    imageString += "grey.png";
                    break;
                case 4:
                    imageString += "red.png";
                    break;
                case 5:
                    imageString += "purple.png";
                    break;
                case 6:
                    imageString += "white.png";
                    break;
                case 7:
                    imageString += "yellow.png";
                    break;
            }

            Image image = Image.FromFile(pathString + imageString);
            e.Graphics.DrawImage(image, r);
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            Projectile p = o as Projectile;

            int width = 30;
            int height = 30;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None; //.AntiAlias;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;

            // Rectangles are drawn starting from the top-left corner.
            // So if we want the rectangle centered on the player's location, we have to offset it
            // by half its size to the left (-width/2) and up (-height/2)
            Rectangle r = new Rectangle(-(width / 2), -(height / 2), width, height);

            string pathString = @"../../../Resources/Images/";
            string imageString = "shot-";

            switch (p.GetOwner() % 8)
            {

                case 0:
                    imageString += "blue.png";
                    break;
                case 1:
                    imageString += "brown.png";
                    break;
                case 2:
                    imageString += "green.png";
                    break;
                case 3:
                    imageString += "grey.png";
                    break;
                case 4:
                    imageString += "red.png";
                    break;
                case 5:
                    imageString += "purple.png";
                    break;
                case 6:
                    imageString += "white.png";
                    break;
                case 7:
                    imageString += "yellow.png";
                    break;
            }

            Image image = Image.FromFile(pathString + imageString);
            e.Graphics.DrawImage(image, r);
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void StarDrawer(object o, PaintEventArgs e)
        {
            Star s = o as Star;

            int width = 30;
            int height = 30;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None; //.AntiAlias;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;

            // Rectangles are drawn starting from the top-left corner.
            // So if we want the rectangle centered on the player's location, we have to offset it
            // by half its size to the left (-width/2) and up (-height/2)
            // TODO: multiply by the mass of the star
            Rectangle r = new Rectangle(-(width / 2), -(height / 2), width, height);

            string pathString = @"../../../Resources/Images/";
            string imageString = "star.jpg";

            Image image = Image.FromFile(pathString + imageString);
            e.Graphics.DrawImage(image, r);
        }
        
        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e)
        {

            lock (theWorld)
            {

                // Draw the players
                foreach (Ship player in theWorld.GetShips())
                {
                    //System.Diagnostics.Debug.WriteLine("drawing player at " + p.GetLocation());
                    DrawObjectWithTransform(e, player, this.Size.Width, player.GetLocation().GetX(), player.GetLocation().GetY(), player.GetDirection().ToAngle(), ShipDrawer);
                }

                // Draw the powerups
                foreach (Projectile p in theWorld.GetProjs())
                {
                    //System.Diagnostics.Debug.WriteLine("drawing powerup at " + p.GetLocation());
                    DrawObjectWithTransform(e, p, this.Size.Width, p.GetLocation().GetX(), p.GetLocation().GetY(), 0, ProjectileDrawer);
                }
                foreach (Star star in theWorld.GetStars())
                {
                    DrawObjectWithTransform(e, star, this.Size.Width, star.GetLocation().GetX(), star.GetLocation().GetY(), 0, StarDrawer);
                }
            }
            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}
