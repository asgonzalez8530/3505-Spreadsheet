using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceWars
{
    /// <summary>
    /// Handles the area where the View is drawn.
    /// </summary>
    public class DrawingPanel : Panel
    {
        /// <summary>
        /// The World for this Client. Populated by the Server.
        /// </summary>
        private World theWorld;

        /// <summary>
        /// Dictionaries of Images for ships and projectiles based on ID % 8.
        /// </summary>
        private Dictionary<int, Image> CoastingShips, ThrustingShips, Projectiles;

        /// <summary>
        /// Paintbrushes for drawing hp.
        /// </summary>
        private Dictionary<int, Color> paintBrushes;

        /// <summary>
        /// The image for our Stars.
        /// </summary>
        private Image starImage;

        /// <summary>
        /// Radius of the ships. Used to help draw.
        /// </summary>
        private int shipRadius;

        /// <summary>
        /// Constructs a DrawingPanel for SpaceWars.
        /// </summary>
        public DrawingPanel(World w)
        {
            // initializing member variables and setting up dictionaries
            theWorld = w;
            CoastingShips = MakeCoastingShips();
            ThrustingShips = MakeThrustingShips();
            Projectiles = MakeProjectiles();
            paintBrushes = MakeBrushes();
            starImage = Image.FromFile("..\\..\\..\\Resources\\Images\\star.jpg");

            // a few settings
            shipRadius = 35;
            this.BackColor = Color.Black;
            DoubleBuffered = true;
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


        /// <summary>
        /// A delegate for DrawObjectWithTransform
        /// Methods matching this delegate can draw whatever they want using e 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
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
        /// This method is invoked when the DrawingPanel needs to be re-drawn. Redraws the current state of the World.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            // iterates through ships, stars, and projectiles and draws them. Locks are to protect from these lists being modified while they are drawn.
            lock (theWorld.shipLock)
            {
                foreach (Ship s in theWorld.GetShips())
                {
                    s.GetOrientation().Normalize();

                    DrawObjectWithTransform(e, s, theWorld.GetSize(), s.GetLocation().GetX(), s.GetLocation().GetY(), s.GetOrientation().ToAngle(), ShipDrawer);
                    // if hp > 1, draw 1
                    if(s.GetHP() > 1)
                        DrawObjectWithTransform(e, s, theWorld.GetSize(), s.GetLocation().GetX() - shipRadius, s.GetLocation().GetY(), s.GetOrientation().ToAngle(), hpDrawer);
                    // if hp > 2, draw 2
                    if(s.GetHP() > 2)
                        DrawObjectWithTransform(e, s, theWorld.GetSize(), s.GetLocation().GetX(), s.GetLocation().GetY() + shipRadius, s.GetOrientation().ToAngle(), hpDrawer);
                    // if hp > 3, draw 3
                    if(s.GetHP() > 3)
                        DrawObjectWithTransform(e, s, theWorld.GetSize(), s.GetLocation().GetX() + shipRadius, s.GetLocation().GetY(), s.GetOrientation().ToAngle(), hpDrawer);
                    // if hp > 4, draw 4
                    if(s.GetHP() > 4)
                        DrawObjectWithTransform(e, s, theWorld.GetSize(), s.GetLocation().GetX(), s.GetLocation().GetY() - shipRadius, s.GetOrientation().ToAngle(), hpDrawer);
                }
            }
            lock (theWorld.starLock)
            {
                foreach (Star s in theWorld.GetStars())
                {
                    DrawObjectWithTransform(e, s, theWorld.GetSize(), s.GetLocation().GetX(), s.GetLocation().GetY(), 90, StarDrawer);
                }
            }
            lock (theWorld.projLock)
            {
                foreach (Projectile p in theWorld.GetProjectiles())
                {
                    p.GetOrientation().Normalize();


                    DrawObjectWithTransform(e, p, theWorld.GetSize(), p.GetLocation().GetX(), p.GetLocation().GetY(), p.GetOrientation().ToAngle(), ProjectileDrawer);
                }
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// Draws a projectile object from the world.
        /// </summary>
        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            Projectile p = o as Projectile;
            int projWidth = 35;

            //retrieve image from Projectiles image dictionary, based on ownerID
            Image image = Projectiles[p.GetOwnerID() % 8]; 

            Rectangle r = new Rectangle(-(projWidth / 2), -(projWidth / 2), projWidth, projWidth);
            e.Graphics.DrawImage(image, r);
        }

        /// <summary>
        /// Draws a star object from the world.
        /// </summary>
        private void StarDrawer(object o, PaintEventArgs e)
        {
            Star s = o as Star;
            int starWidth = 45 + Convert.ToInt32(s.GetSize() * 1000);

            Rectangle r = new Rectangle(-(starWidth / 2), -(starWidth / 2), starWidth, starWidth);
            e.Graphics.DrawImage(starImage, r);
        }


        /// <summary>
        /// Draws a ship object from the world.
        /// </summary>
        private void ShipDrawer(object o, PaintEventArgs e)
        {
            Ship s = o as Ship;

            if (s.GetHP() > 0)
            {
                Image image = DrawShip(s);

                Rectangle r = new Rectangle(-(shipRadius / 2), -(shipRadius / 2), shipRadius, shipRadius);
                e.Graphics.DrawImage(image, r);
            }
        }

        private void hpDrawer(object o, PaintEventArgs e)
        {
            Ship s = o as Ship;
            int hpWidth = 8;
            DrawHP(s.GetID() % 8, hpWidth, e);
        }


        /// <summary>
        /// Helper method that helps to draw the ship. Picks the correct sprite for current ship.
        /// </summary>
        /// <param name="s">a ship object</param>
        /// <returns>the corresponding Image sprite</returns>
        private Image DrawShip(Ship s)
        {
            // if coasting, return coasting
            if(!s.IsThrusting())
            {
                // retrieve image from CoastingShips image dictionary
                return CoastingShips[s.GetID() % 8];
            }
            // else, return thrusting
            else
            {
                // retrieve image from CoastingShips image dictionary
                return ThrustingShips[s.GetID() % 8];
            }
        }

        /// <summary>
        /// Helper method to draw 1 hp.
        /// </summary>
        /// <param name="shipColorID"></param>
        /// <param name="hpWidth"></param>
        /// <param name="e"></param>
        private void DrawHP(int shipColorID, int hpWidth, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (SolidBrush meBrush = new SolidBrush(paintBrushes[shipColorID]))
            {
                // draw single circle
                Rectangle r = new Rectangle(-(hpWidth / 2), -(hpWidth / 2), hpWidth, hpWidth);
                e.Graphics.FillEllipse(meBrush, r);
            }
        }


        /// <summary>
        /// Creates a dictionary of IDs to coasting ship Images.
        /// </summary>
        /// <returns>a dictionary of IDs to coasting ship Images</returns>
        private Dictionary<int, Image> MakeCoastingShips()
        {
            Dictionary<int, Image> meDictionary = new Dictionary<int, Image>();

            meDictionary.Add(0, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-coast-yellow.png"));
            meDictionary.Add(1, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-coast-blue.png"));
            meDictionary.Add(2, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-coast-brown.png"));
            meDictionary.Add(3, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-coast-green.png"));
            meDictionary.Add(4, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-coast-grey.png"));
            meDictionary.Add(5, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-coast-red.png"));
            meDictionary.Add(6, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-coast-violet.png"));
            meDictionary.Add(7, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-coast-white.png"));

            return meDictionary;
        }


        /// <summary>
        /// Creates a dictionary of IDs to thrusting ship Images.
        /// </summary>
        /// <returns>a dictionary of IDs to thrusting ship Images</returns>
        private Dictionary<int, Image> MakeThrustingShips()
        {
            Dictionary<int, Image> meDictionary = new Dictionary<int, Image>();

            meDictionary.Add(0, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-thrust-yellow.png"));
            meDictionary.Add(1, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-thrust-blue.png"));
            meDictionary.Add(2, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-thrust-brown.png"));
            meDictionary.Add(3, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-thrust-green.png"));
            meDictionary.Add(4, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-thrust-grey.png"));
            meDictionary.Add(5, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-thrust-red.png"));
            meDictionary.Add(6, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-thrust-violet.png"));
            meDictionary.Add(7, Image.FromFile("..\\..\\..\\Resources\\Images\\ship-thrust-white.png"));

            return meDictionary;
        }


        /// <summary>
        /// Creates a dictionary of IDs to projectile Images.
        /// </summary>
        /// <returns>a dictionary of IDs to projectile Images</returns>
        private Dictionary<int, Image> MakeProjectiles()
        {
            Dictionary<int, Image> meDictionary = new Dictionary<int, Image>();

            meDictionary.Add(0, Image.FromFile("..\\..\\..\\Resources\\Images\\shot-yellow.png"));
            meDictionary.Add(1, Image.FromFile("..\\..\\..\\Resources\\Images\\shot-blue.png"));
            meDictionary.Add(2, Image.FromFile("..\\..\\..\\Resources\\Images\\shot-brown.png"));
            meDictionary.Add(3, Image.FromFile("..\\..\\..\\Resources\\Images\\shot-green.png"));
            meDictionary.Add(4, Image.FromFile("..\\..\\..\\Resources\\Images\\shot-grey.png"));
            meDictionary.Add(5, Image.FromFile("..\\..\\..\\Resources\\Images\\shot-red.png"));
            meDictionary.Add(6, Image.FromFile("..\\..\\..\\Resources\\Images\\shot-violet.png"));
            meDictionary.Add(7, Image.FromFile("..\\..\\..\\Resources\\Images\\shot-white.png"));

            return meDictionary;
        }

        private Dictionary<int, Color> MakeBrushes()
        {
            Dictionary<int, Color> meDictionary = new Dictionary<int, Color>();

            meDictionary.Add(5, Color.Red);
            meDictionary.Add(1, Color.CadetBlue);
            meDictionary.Add(2, Color.BurlyWood);
            meDictionary.Add(3, Color.Green);
            meDictionary.Add(4, Color.DimGray);
            meDictionary.Add(7, Color.AntiqueWhite);
            meDictionary.Add(6, Color.Orchid);
            meDictionary.Add(0, Color.Goldenrod);

            return meDictionary;

        }
    }
}
