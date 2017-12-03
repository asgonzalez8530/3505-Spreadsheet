using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using System.Net;
using Communication;
using System.Text;

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
                Assert.IsTrue(connection.GetTcpListener() != null);
            }
            // make sure we stop the tcp listener before exiting method
            finally
            {
                listener.Stop();
            }

        }

        #endregion

        /********************* SocketState Class tests ***********************/
        #region SocketState tests

        /// <summary>
        /// Tests the expected state of a SocketState object when it is
        /// first initialized
        /// </summary>
        [TestMethod]
        public void SocketState_ConstructorTest()
        {
            // create a socket using to pass to a SocketState object
            Network.MakeSocket("localhost", out Socket sock, out IPAddress address);

            try
            {
                SocketState state = new SocketState(sock, -1);

                //the state should not be reporting an error
                Assert.IsFalse(state.HasError);

                // there error messgae should be an empty string
                Assert.IsTrue(state.ErrorMessage == "");

                //the size of the buffer should be 1024
                byte[] buffer = state.GetMessageBuffer();

                Assert.AreEqual(1024, buffer.Length);

                // the message buffer should be filled with 0's
                byte zeroByte = 0x0;
                for (int i = 0; i < buffer.Length; i++)
                {
                    Assert.IsTrue(buffer[i] == zeroByte);
                }

                // growable string builder which represents the message sent by server
                // should not contain anything.
                StringBuilder sb = state.GetStringBuilder();
                Assert.IsTrue(sb.ToString() == "");
                Assert.AreEqual(0, sb.Length);

                // the socket returned by GetSocket should be the same instance as
                // passed to the constructor
                Assert.AreEqual(sock, state.GetSocket());

                // We have not set a network action, but it should still not be null
                // trying to invoke this action would be bad if it was null and cause
                // test to fail
                state.InvokeNetworkAction(state);

                // ID should be same as what it was set
                Assert.AreEqual(-1, state.GetID());

            }
            // make sure we close the socket when we are done testing our state
            finally
            {
                sock.Dispose();
            }

        }

        /// <summary>
        /// Tests the HasError parameter. Should be settable and gettable
        /// </summary>
        [TestMethod]
        public void SocketState_HasErrorTrue()
        {
            // create a socket using to pass to a SocketState object
            Network.MakeSocket("localhost", out Socket sock, out IPAddress address);

            try
            {
                SocketState state = new SocketState(sock, -1);

                //the state should not be reporting an error
                Assert.IsFalse(state.HasError);

                state.HasError = true;
                Assert.IsTrue(state.HasError);

            }
            // make sure we close the socket when we are done testing our state
            finally
            {
                sock.Dispose();
            }

        }

        /// <summary>
        /// Tests the ErrorMessage parameter. Should be settable and gettable
        /// </summary>
        [TestMethod]
        public void SocketState_ErrorMessage()
        {
            // create a socket using to pass to a SocketState object
            Network.MakeSocket("localhost", out Socket sock, out IPAddress address);

            try
            {
                SocketState state = new SocketState(sock, -1);

                // there error messgae should be an empty string
                Assert.IsTrue(state.ErrorMessage == "");

                state.ErrorMessage = "An error message";

                Assert.IsTrue(state.ErrorMessage == "An error message");

            }
            // make sure we close the socket when we are done testing our state
            finally
            {
                sock.Dispose();
            }

        }

        /// <summary>
        /// Tests GetMessageBuffer() method of socket state
        /// </summary>
        [TestMethod]
        public void SocketState_GetMessageBuffer()
        {
            // create a socket using to pass to a SocketState object
            Network.MakeSocket("localhost", out Socket sock, out IPAddress address);

            try
            {
                SocketState state = new SocketState(sock, -1);

                //the size of the buffer should be 1024
                byte[] buffer = state.GetMessageBuffer();

                Assert.AreEqual(1024, buffer.Length);

                // the message buffer should be filled with 0's
                byte zeroByte = 0x0;
                for (int i = 0; i < buffer.Length; i++)
                {
                    Assert.IsTrue(buffer[i] == zeroByte);
                }

            }
            // make sure we close the socket when we are done testing our state
            finally
            {
                sock.Dispose();
            }

        }

        /// <summary>
        /// Tests GetStringBuilder() method
        /// </summary>
        [TestMethod]
        public void SocketState_GetStringBuilder()
        {
            // create a socket using to pass to a SocketState object
            Network.MakeSocket("localhost", out Socket sock, out IPAddress address);

            try
            {
                SocketState state = new SocketState(sock, -1);

                // growable string builder which represents the message sent by server
                // should not contain anything.
                StringBuilder sb = state.GetStringBuilder();
                Assert.IsTrue(sb.ToString() == "");
                Assert.AreEqual(0, sb.Length);

            }
            // make sure we close the socket when we are done testing our state
            finally
            {
                sock.Dispose();
            }

        }

        /// <summary>
        /// Tests GetSocket() method. Checks that reference to socket object is
        /// properly stored
        /// </summary>
        [TestMethod]
        public void SocketState_GetSocket()
        {
            // create a socket using to pass to a SocketState object
            Network.MakeSocket("localhost", out Socket sock, out IPAddress address);

            try
            {
                SocketState state = new SocketState(sock, -1);

                // the socket returned by GetSocket should be the same instance as
                // passed to the constructor
                Assert.AreEqual(sock, state.GetSocket());

            }
            // make sure we close the socket when we are done testing our state
            finally
            {
                sock.Dispose();
            }

        }

        /// <summary>
        /// Tests InvokeNetworkAction. It is trying to invoke an action when one
        /// is not yet set, would be bad if the delegate was null.
        /// </summary>
        [TestMethod]
        public void SocketState_InvokeNetworkActionNotSet()
        {
            // create a socket using to pass to a SocketState object
            Network.MakeSocket("localhost", out Socket sock, out IPAddress address);

            try
            {
                SocketState state = new SocketState(sock, -1);

                // We have not set a network action, but it should still not be null
                // trying to invoke this action would be bad if it was null and cause
                // test to fail
                state.InvokeNetworkAction(state);

            }
            // make sure we close the socket when we are done testing our state
            finally
            {
                sock.Dispose();
            }

        }

        /// <summary>
        /// Tests InvokeNetworkAction. It is trying to invoke an action when we
        /// set it to null, would be bad if the delegate was null and 
        /// SetNetworkAction(null) should set the action to something other
        /// than null
        /// </summary>
        [TestMethod]
        public void SocketState_SetNetworkActionNull()
        {
            // create a socket using to pass to a SocketState object
            Network.MakeSocket("localhost", out Socket sock, out IPAddress address);

            try
            {
                SocketState state = new SocketState(sock, -1);

                state.SetNetworkAction(null);

                // We have not set a network action, but it should still not be null
                // trying to invoke this action would be bad if it was null and cause
                // test to fail
                state.InvokeNetworkAction(state);

            }
            // make sure we close the socket when we are done testing our state
            finally
            {
                sock.Dispose();
            }

        }

        /// <summary>
        /// Tests InvokeNetworkAction. We will set the NetworkAction to a lambda
        /// that throws an exception to see that it was invoked since it returns
        /// nothing
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SocketState_InvokeNetworkActionThrowException()
        {
            // create a socket using to pass to a SocketState object
            Network.MakeSocket("localhost", out Socket sock, out IPAddress address);

            try
            {
                SocketState state = new SocketState(sock, -1);

                state.SetNetworkAction(x => throw new ArgumentException());

                // invoke network action, should throw argument exception
                state.InvokeNetworkAction(state);


            }
            // make sure we close the socket when we are done testing our state
            finally
            {
                sock.Dispose();
            }

        }

        //TODO: may not need SocketState.SetID(int id) and SocketStateGetID()
        /// <summary>
        /// Make sure we can set a new id with SetID method and then retrieve that id
        /// </summary>
        [TestMethod]
        public void SocketState_GetAndSetID()
        {
            // create a socket using to pass to a SocketState object
            Network.MakeSocket("localhost", out Socket sock, out IPAddress address);

            try
            {
                SocketState state = new SocketState(sock, -1);

                // ID should be same as what it was set when object was created
                Assert.AreEqual(-1, state.GetID());

                // check that setting id changes what is stored
                state.SetID(5);
                Assert.AreEqual(5, state.GetID());

            }
            // make sure we close the socket when we are done testing our state
            finally
            {
                sock.Dispose();
            }

        }

        #endregion
    }
}
