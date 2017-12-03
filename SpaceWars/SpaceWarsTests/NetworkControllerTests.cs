using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using System.Net;
using Communication;

namespace SpaceWarsTests
{
    [TestClass]
    public class NetworkControllerTests
    {

        /******************* ConnectionState Class tests *********************/
        #region ConnectionState tests

        /// <summary>
        /// Tests the expected state of a ConnectionState object when it is
        /// first initialized
        /// </summary>
        [TestMethod]
        public void ConnectionState_ConstructorTest()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 11000);
            try
            {
                ConnectionState connection = new ConnectionState(listener, null);

                // the TcpListener object stored by connection should be the same
                // instance as the one it was passed. 
                Assert.IsTrue(connection.GetTcpListener().Equals(listener));

                // even though the NetworkAction passed to connection was null, the 
                // NetworkAction stored by connection should NOT be null
                Assert.IsTrue(connection.GetNetworkAction() != null);
            }
            // make sure we stop the tcp listener before exiting method
            finally
            {
                listener.Stop();
            }

        }

        /// <summary>
        /// Tests that the expected exception is thrown when attemting 
        /// to create a ConnectionState object with a null TcpListener
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConnectionState_ConstructorTestNullListener()
        {
            ConnectionState connection = new ConnectionState(null, null);
        }

        /// <summary>
        /// Tests that when NetworkAction is passed to a ConnectionState object,
        /// it is not stored as null
        /// </summary>
        [TestMethod]
        public void ConnectionState_GetNetworkAction()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 11000);
            try
            {
                ConnectionState connection = new ConnectionState(listener, x => { });

                // even though the NetworkAction passed to connection was null, the 
                // NetworkAction stored by connection should NOT be null
                Assert.IsTrue(connection.GetNetworkAction() != null);
            }
            // make sure we stop the tcp listener before exiting method
            finally
            {
                listener.Stop();
            }

        }

        /// <summary>
        /// Tests when a null NetworkAction is passed to a ConnectionState object,
        /// it is not stored as null
        /// </summary>
        [TestMethod]
        public void ConnectionState_GetNetworkActionNull()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 11000);
            try
            {
                ConnectionState connection = new ConnectionState(listener, null);

                // even though the NetworkAction passed to connection was null, the 
                // NetworkAction stored by connection should NOT be null
                Assert.IsTrue(connection.GetNetworkAction() != null);
            }
            // make sure we stop the tcp listener before exiting method
            finally
            {
                listener.Stop();
            }

        }

        /// <summary>
        /// Tests that the TcpListener reference returned by the GetTcpListener()
        /// method is the same as the one passed to it.
        /// </summary>
        [TestMethod]
        public void ConnectionState_GetTcpListener()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 11000);
            try
            {
                ConnectionState connection = new ConnectionState(listener, null);

                // the TcpListener object stored by connection should be the same
                // instance as the one it was passed. 
                Assert.IsTrue(connection.GetTcpListener().Equals(listener));
            }
            // make sure we stop the tcp listener before exiting method
            finally
            {
                listener.Stop();
            }

        }

        /// <summary>
        /// Tests that the TcpListener reference returned by the GetTcpListener()
        /// method is not null.
        /// </summary>
        [TestMethod]
        public void ConnectionState_GetTcpListenerNotNull()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 11000);
            try
            {
                ConnectionState connection = new ConnectionState(listener, null);

                // the TcpListener object stored by connection should be the same
                // instance as the one it was passed. 
                Assert.IsTrue(connection.GetTcpListener()!= null);
            }
            // make sure we stop the tcp listener before exiting method
            finally
            {
                listener.Stop();
            }

        }

        #endregion


    }
}
