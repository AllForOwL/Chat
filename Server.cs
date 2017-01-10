using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperWebSocket;
using System.Windows.Forms;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;

namespace Chat
{
    class Server
    {
        WebSocketServer m_appServer = new WebSocketServer(); 
        public void StartServer()
        {
            //Setup the appServer
            m_appServer.Setup(new ServerConfig
            {
                Port = 2012,
                Ip = "Any",
                MaxConnectionNumber = 100,
                Mode = SocketMode.Tcp,
                Name = "SuperWebSocket Server"
            }, logFactory: new ConsoleLogFactory());

            m_appServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(appServer_NewMessageReceived);

            //Try to start the appServer
            if (!m_appServer.Start())
            {
                return;
            }

            MessageBox.Show("Сервер запущений!");
        }
        public void Stop()
        {
            m_appServer.Stop();
        }
        static void appServer_NewMessageReceived(WebSocketSession session, string message)
        {
            //Send the received message back
            session.Send("From Server: " + message);

            MessageBox.Show("Server: " + message);
        }
    }
}
