using SpaceWars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceWarsView
{
    public partial class WorldPanel : Panel
    {
        private World theWorld;
        private Dictionary<int, Image> shipCoastImages; // stores all the ship images
        private Dictionary<int, Image> shipThrustImages; // stores all the thrust ship images
        private Dictionary<int, Image> starImages; // stores all the star images
        private Dictionary<int, Image> projectileImages; // stores all the projectile images
        
        public WorldPanel()
        {
            // make the panel
            InitializeComponent();
            DoubleBuffered = true;
            theWorld = new World();

            // initializes the image dictionaries
            shipCoastImages = new Dictionary<int, Image>();
            shipThrustImages = new Dictionary<int, Image>();
            starImages = new Dictionary<int, Image>();
            projectileImages = new Dictionary<int, Image>();

            // load the images up from the following directory
            string pathString = @"../../../Resources/Images/";
            LoadImages(pathString);
        }

        /// <summary>
        /// Loads all the images from the directory and stores them in the proper Dictionary
        /// </summary>
        private void LoadImages(string directory)
        {
            // get all the files from the directory
            string[] files = Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly);

            int coast = 0;
            int thrust = 0;
            int shot = 0;
            int star = 0; 
            foreach(string file in files)
            {
                Console.Out.WriteLine(file);

                if (file.Contains("coast"))
                {
                    shipCoastImages.Add(coast++, Image.FromFile(file));
                }
                else if (file.Contains("thrust"))
                {
                    shipThrustImages.Add(thrust++, Image.FromFile(file));
                }
                else if (file.Contains("shot"))
                {
                    projectileImages.Add(shot++, Image.FromFile(file));
                }
                else if (file.Contains("star"))
                {
                    starImages.Add(star++, Image.FromFile(file));
                }

            }
        }

        /// <summary>
        /// Gets the world that is being used
        /// </summary>
        public World GetWorld()
        {
            return theWorld;
        }

        internal void SetWorld(World world)
        {
            theWorld = world;
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

            // Rectangles are drawn starting from the top-left corner.
            // So if we want the rectangle centered on the player's location, we have to offset it
            // by half its size to the left (-width/2) and up (-height/2)
            Rectangle r = new Rectangle(-(s.GetWidth() / 2), -(s.GetHeight() / 2), s.GetWidth(), s.GetHeight());
            
            Image image;
            if (s.HasThrust())
            {
                image = shipThrustImages[s.GetID() % shipThrustImages.Count];
            }
            else
            {
                image = shipCoastImages[s.GetID() % shipCoastImages.Count];
            }
            
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

            // Rectangles are drawn starting from the top-left corner.
            // So if we want the rectangle centered on the player's location, we have to offset it
            // by half its size to the left (-width/2) and up (-height/2)
            Rectangle r = new Rectangle(-(p.GetWidth() / 2), -(p.GetHeight() / 2), p.GetWidth(), p.GetHeight());

            Image image = projectileImages[p.GetOwner() % projectileImages.Count];

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

            // Rectangles are drawn starting from the top-left corner.
            // So if we want the rectangle centered on the player's location, we have to offset it
            // by half its size to the left (-width/2) and up (-height/2)
            Rectangle r = new Rectangle(-(s.GetWidth() / 2), -(s.GetHeight() / 2), s.GetWidth(), s.GetHeight());

            Image image = starImages[s.GetID() % starImages.Count];
            e.Graphics.DrawImage(image, r);
        }
        
        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e)
        {
            lock (theWorld)
            {

                // draws the stars
                foreach (Star star in theWorld.GetStars())
                {
                    DrawObjectWithTransform(e, star, this.Size.Width, star.GetLocation().GetX(), star.GetLocation().GetY(), 0, StarDrawer);
                }

                // draws the ships
                foreach (Ship ship in theWorld.GetAliveShips())
                {
                    DrawObjectWithTransform(e, ship, this.Size.Width, ship.GetLocation().GetX(), ship.GetLocation().GetY(), ship.GetDirection().ToAngle(), ShipDrawer);
                }

                // draws the Projectiles
                foreach (Projectile p in theWorld.GetProjs())
                {
                    DrawObjectWithTransform(e, p, this.Size.Width, p.GetLocation().GetX(), p.GetLocation().GetY(), 0, ProjectileDrawer);
                }
                
            }
            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}
