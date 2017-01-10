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
    public partial class FormClient : Form
    {
        Client m_client = new Client();

        public FormClient()
        {
            InitializeComponent();
        }

        private void FormClient_Load(object sender, EventArgs e)
        {
            m_client.StartClient();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_client.SendMessage(ui_textMessage.Text);
            ui_historyMessage.Items.Add(ui_textMessage.Text);
            ui_textMessage.Clear();

        }
    }
}
