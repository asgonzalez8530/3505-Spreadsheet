using Newtonsoft.Json;

namespace SpaceWars
{
    /// <summary>
    /// Star in the SpaceWars game World. Fields of this class "Opt In" for JSONObject MemberSerialization.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Star
    {
        /// <summary>
        /// Star's unique ID.
        /// </summary>
        [JsonProperty(PropertyName = "star")]
        private int ID;

        /// <summary>
        /// Star's current location information, as a 2D Vector.
        /// </summary>
        [JsonProperty]
        private Vector2D loc;

        /// <summary>
        /// Star's current mass.
        /// </summary>
        [JsonProperty]
        private double mass;

        public Star() { }

        public Star(int givenID, double givenMass, Vector2D givenLoc)
        {
            ID = givenID;
            mass = givenMass;
            loc = givenLoc;
        }


        /// <summary>
        /// Gets the location Vector
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {
            return new Vector2D(loc);
        }


        /// <summary>
        /// Gets the mass
        /// </summary>
        /// <returns></returns>
        public double GetSize()
        {
            return mass;
        }

        /// <summary>
        /// Return the Star's ID.
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return ID;
        }


        /// <summary>
        /// Sets the location Vector
        /// </summary>
        /// <param name="newLoc"></param>
        public void SetLocation(Vector2D newLoc)
        {
            loc = newLoc;
        }


        /// <summary>
        /// Sets the mass
        /// </summary>
        /// <param name="newSize"></param>
        public void SetSize(double newSize)
        {
            mass = newSize;
        }
    }
}
