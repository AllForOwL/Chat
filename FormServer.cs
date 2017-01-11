using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chat;


namespace Chat
{
    public partial class FormServer : Form
    {
        Server m_server = new Server();
        public FormServer()
        {
            InitializeComponent();
        }

        private void FormServer_Load(object sender, EventArgs e)
        {
            m_server.StartServer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_server.Stop();
        }
    }
}
