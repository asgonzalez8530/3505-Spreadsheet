using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceWarsView
{
    public interface ISpaceWarsWindow
    {
        // event listeners
        event Action enterConnectEvent;
        event Action<KeyEventArgs> ControlKeyDownEvent;
        event Action<KeyEventArgs> ControlKeyUpEvent;
        event Action ControlMenuClick;
        event Action AboutMenuClick;

        /// <summary>
        /// Gets the user defined user name
        /// </summary>
        string GetUserName();

        /// <summary>
        /// Gets the user defined server
        /// </summary>
        string GetServer();

        /// <summary>
        /// Sets the ServerTextBox text
        /// </summary>
        void SetServerString(string s);

        /// <summary>
        /// Sets the NameTextBox text
        /// </summary>
        void SetUserName(string s);

        /// <summary>
        /// Tells the form to define a default accept button
        /// </summary>
        void SetDefaultAcceptButton();

        /// <summary>
        /// Tells the form to set user box to active if true, inactive if false
        /// </summary>
        void ToggleUserBoxControl(bool active);

        /// <summary>
        /// Tells the form to set server box to active if true, inactive if false
        /// </summary>
        void ToggleServerBoxControl(bool active);

        /// <summary>
        /// Tells the form to set connect button to active if true, inactive if false
        /// </summary>
        void ToggleConnectButtonControl(bool active);

        /// <summary>
        /// Displays a messagebox with s as the message
        /// </summary>
        void DisplayMessageBox(string s);

        /// <summary>
        /// Gets the world object used in the world panel
        /// </summary>
        World GetWorldPanelWorld();

        /// <summary>
        /// Updates the size of the world
        /// </summary>
        void UpdateWorldSize(int worldSize);

        /// <summary>
        /// Gets the current frame timer
        /// </summary>
        System.Timers.Timer GetFrameTimer();

    }
}
