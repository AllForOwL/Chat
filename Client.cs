using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketEngine;
using SuperWebSocket;
using SuperWebSocket.SubProtocol;
using WebSocket4Net;
using System.Diagnostics;
using Microsoft;

namespace Chat
{
    class Client
    {
        protected WebSocketServer m_WebSocketServer;
        protected AutoResetEvent m_MessageReceiveEvent = new AutoResetEvent(false);
        protected AutoResetEvent m_OpenedEvent = new AutoResetEvent(false);
        protected AutoResetEvent m_CloseEvent = new AutoResetEvent(false);
        protected string m_CurrentMessage = string.Empty;

        private WebSocketVersion m_Version = WebSocketVersion.DraftHybi00;

        private string m_Security;
        private string m_CertificateFile;
        private string m_Password;

        protected virtual string Host
        {
            get { return "ws://127.0.0.1"; }
        }

        void StartClient()
        {
            WebSocket webSocketClient = new WebSocket("ws://echo.websocket.org/");
            webSocketClient.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(webSocketClient_Error);
            webSocketClient.AllowUnstrustedCertificate = true;
            webSocketClient.Opened += new EventHandler(webSocketClient_Opened);
            webSocketClient.Closed += new EventHandler(webSocketClient_Closed);
            webSocketClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(webSocketClient_MessageReceived);
            webSocketClient.Open();

            if (!m_OpenedEvent.WaitOne(5000))
            {
            //    Assert.Fail("Failed to Opened session ontime");
            }

            //Assert.AreEqual(WebSocketState.Open, webSocketClient.State);

            for (var i = 0; i < 10; i++)
            {
                var message = Guid.NewGuid().ToString();

                webSocketClient.Send(message);

                if (!m_MessageReceiveEvent.WaitOne(5000))
                {
                   // Assert.Fail("Failed to get echo messsage on time");
                    break;
                }

                Console.WriteLine("Received echo message: {0}", m_CurrentMessage);
                //Assert.AreEqual(m_CurrentMessage, message);
            }

            webSocketClient.Close();

            if (!m_CloseEvent.WaitOne(5000))
            {
 
            }
        }

        protected void webSocketClient_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.GetType() + ":" + e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);

            if (e.Exception.InnerException != null)
            {
                Console.WriteLine(e.Exception.InnerException.GetType());
            }
        }

        public void CloseWebSocketTest()
        {
            WebSocket webSocketClient = new WebSocket(string.Format("{0}:{1}/websocket", Host, m_WebSocketServer.Config.Port), "basic", m_Version);
            webSocketClient.AllowUnstrustedCertificate = true;
            webSocketClient.Opened += new EventHandler(webSocketClient_Opened);
            webSocketClient.Closed += new EventHandler(webSocketClient_Closed);
            webSocketClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(webSocketClient_MessageReceived);
            webSocketClient.Open();

            if (!m_OpenedEvent.WaitOne(2000))
            {
 
            }
               // Assert.Fail("Failed to Opened session ontime");

            //Assert.AreEqual(WebSocketState.Open, webSocketClient.State);

            webSocketClient.Send("QUIT");

            if (!m_CloseEvent.WaitOne(1000))
            {
 
            }//Assert.Fail("Failed to close session ontime");

            //Assert.AreEqual(WebSocketState.Closed, webSocketClient.State);
        }

        protected void webSocketClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            m_CurrentMessage = e.Message;
            m_MessageReceiveEvent.Set();
        }

        protected void webSocketClient_Closed(object sender, EventArgs e)
        {
            m_CloseEvent.Set();
        }

        protected void webSocketClient_Opened(object sender, EventArgs e)
        {
            m_OpenedEvent.Set();
        }

    }
}
