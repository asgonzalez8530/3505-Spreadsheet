﻿using SpaceWars;
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
        /// Tells the form to set user box to inactive
        /// </summary>
        void SetUserBoxInactive();

        /// <summary>
        /// Tells the form to set server box to inactive
        /// </summary>
        void SetServerBoxInactive();

        /// <summary>
        /// Tells the form to set enter button to inactive
        /// </summary>
        void SetConnectButtonInactive();

        /// <summary>
        /// Displays a messagebox with s as the message
        /// </summary>
        void DisplayMessageBox(string s);

        World GetWorldPanelWorld();

        void UpdateWorldSize(int worldSize);

        System.Timers.Timer GetFrameTimer();

    }
}
