using Newtonsoft.Json;

namespace SpaceWars
{
    /// <summary>
    /// Projectile in the SpaceWars game World. Fields of this class "Opt In" for JSONObject MemberSerialization.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        /// <summary>
        /// Projectile's unique ID.
        /// </summary>
        [JsonProperty(PropertyName = "proj")]
        private int ID;

        /// <summary>
        /// Projectile's current location information, as a 2D Vector.
        /// </summary>
        [JsonProperty]
        private Vector2D loc;

        /// <summary>
        /// Projectile's current direction information, as a 2D Vector.
        /// </summary>
        [JsonProperty]
        private Vector2D dir;

        /// <summary>
        /// Projectile's current "alive" status.
        /// </summary>
        [JsonProperty]
        private bool alive;

        /// <summary>
        /// The ID of the Ship which fired this Projectile.
        /// </summary>
        [JsonProperty]
        private int owner;

        public Projectile() { }

        public Projectile(int OwnerID, int givenID, Vector2D giveDir, Vector2D givenLoc)
        {
            owner = OwnerID;
            ID = givenID;

            alive = true;
            loc = givenLoc;
            dir = giveDir;
        }


        /// <summary>
        /// Gets the Vector Location of this projectile.
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {
            return new Vector2D(loc);
        }
        

        /// <summary>
        /// Sets the Vector location of this projectile
        /// </summary>
        /// <param name="newLoc"></param>
        public void SetLocation(Vector2D newLoc)
        {
            loc = newLoc;
        }


        /// <summary>
        /// Gets the direction vector 
        /// </summary>
        /// <returns></returns>
        public Vector2D GetOrientation()
        {
            return new Vector2D(dir);
        }


        /// <summary>
        /// Gets the ownerID of this projectile
        /// </summary>
        /// <returns></returns>
        public int GetOwnerID()
        {
            return owner;
        }


        /// <summary>
        /// Returns whether this projectile is alive or not
        /// </summary>
        /// <returns></returns>
        public bool IsAlive()
        {
            return alive;
        }


        /// <summary>
        /// Set alive boolean to false.
        /// </summary>
        public void KillProj()
        {
            alive = false;
        }
    }
}
