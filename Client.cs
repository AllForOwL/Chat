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
using System.Windows.Forms;

namespace Chat
{
    class Client
    {
        protected string Host
        {
            get { return "ws://127.0.0.1"; }
        }

        protected WebSocket m_WebSocketClient = new WebSocket(string.Format("{0}:{1}/websocket", "ws://127.0.0.1", 2012), "basic");
        protected AutoResetEvent m_MessageReceiveEvent = new AutoResetEvent(false);
        protected AutoResetEvent m_OpenedEvent = new AutoResetEvent(false);
        protected AutoResetEvent m_CloseEvent = new AutoResetEvent(false);
        protected string m_CurrentMessage = string.Empty;

        private WebSocketVersion m_Version = WebSocketVersion.DraftHybi00;

        private string m_Security;
        private string m_CertificateFile;
        private string m_Password;

        public void StartClient()
        {
            m_WebSocketClient.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(m_WebSocketClient_Error);
            m_WebSocketClient.AllowUnstrustedCertificate = true;
            m_WebSocketClient.Opened += new EventHandler(m_WebSocketClient_Opened);
            m_WebSocketClient.Closed += new EventHandler(m_WebSocketClient_Closed);
            m_WebSocketClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(m_WebSocketClient_MessageReceived);
            m_WebSocketClient.Open();

            if (!m_OpenedEvent.WaitOne(5000))
            {
                
            }

            //for (var i = 0; i < 10; i++)
            //{
            //    var message = Guid.NewGuid().ToString();

            //    m_WebSocketClient.Send(message);

            //    if (!m_MessageReceiveEvent.WaitOne(5000))
            //    {
            //       // Assert.Fail("Failed to get echo messsage on time");
            //        break;
            //    }

            //    Console.WriteLine("Received echo message: {0}", m_CurrentMessage);
            //    //Assert.AreEqual(m_CurrentMessage, message);
            //}
        }

        public void SendMessage(string i_message)
        {
            m_WebSocketClient.Send(i_message);
        }

        protected void m_WebSocketClient_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.GetType() + ":" + e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);

            if (e.Exception.InnerException != null)
            {
                Console.WriteLine(e.Exception.InnerException.GetType());
            }
        }

        public void CloseWebSocketTest()
        {
            //WebSocket m_WebSocketClient = new WebSocket(string.Format("{0}:{1}/websocket", Host, m_WebSocketClient.Config.Port), "basic", m_Version);
            //m_WebSocketClient.AllowUnstrustedCertificate = true;
            //m_WebSocketClient.Opened += new EventHandler(m_WebSocketClient_Opened);
            //m_WebSocketClient.Closed += new EventHandler(m_WebSocketClient_Closed);
            //m_WebSocketClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(m_WebSocketClient_MessageReceived);
            //m_WebSocketClient.Open();

            //if (!m_OpenedEvent.WaitOne(2000))
            //{
 
            //}
            //   // Assert.Fail("Failed to Opened session ontime");

            ////Assert.AreEqual(WebSocketState.Open, m_WebSocketClient.State);

            //m_WebSocketClient.Send("QUIT");

            //if (!m_CloseEvent.WaitOne(1000))
            //{
 
            //}//Assert.Fail("Failed to close session ontime");

            //Assert.AreEqual(WebSocketState.Closed, m_WebSocketClient.State);
        }

        protected void m_WebSocketClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            m_CurrentMessage = e.Message;
            m_MessageReceiveEvent.Set();

            MessageBox.Show(m_CurrentMessage);  
        }

        protected void m_WebSocketClient_Closed(object sender, EventArgs e)
        {
            m_CloseEvent.Set();
        }

        protected void m_WebSocketClient_Opened(object sender, EventArgs e)
        {
            m_OpenedEvent.Set();
        }

    }
}
