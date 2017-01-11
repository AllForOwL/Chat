using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using MySql.Data;
using System.Data.OleDb;
using MySql.Data.MySqlClient;
using Chat;

namespace Chat
{
    public partial class FormClient : Form
    {
        Client m_client = new Client();

        private List<string> m_listHistoryMessages = new List<string>();

        System.Timers.Timer m_timer;

        OleDbConnection m_connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Messages.mdb");
        OleDbCommand m_command;

        public void LoadDatabase()
        {
            m_command = m_connection.CreateCommand();

            Select();
            AddHistoryMessages();
        }

        public void AddHistoryMessages()
        {
            for (int i = 0; i < m_listHistoryMessages.Count(); i++)
            {
                ui_historyMessage.Items.Add(m_listHistoryMessages[i]);
            }
            m_listHistoryMessages.Clear();
        }

        public void Insert(string i_textMessage)
        {
            m_connection.Open();

            m_command.CommandText = "INSERT INTO Messages (Message) VALUES('" + i_textMessage + "')";
            m_command.CommandType = CommandType.Text;
            m_command.ExecuteNonQuery();

            m_connection.Close();
        }

        public void Select()
        {
            m_connection.Open();

            m_command.CommandText = "SELECT * FROM Messages";
            m_command.CommandType = CommandType.Text;

            OleDbDataReader _reader = m_command.ExecuteReader();

            while (_reader.Read())
            {
                m_listHistoryMessages.Add(_reader["Message"].ToString());
            }

            m_connection.Close();
        }

        public FormClient()
        {
            InitializeComponent();
        }

        private void FormClient_Load(object sender, EventArgs e)
        {
            m_client.StartClient();
            LoadDatabase();
            SetTimer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_client.SendMessage(ui_textMessage.Text);
            Insert(ui_textMessage.Text);
            ui_textMessage.Clear();
        }
        private void Update(Object source, ElapsedEventArgs e)
        {
            if (m_client.isNewMessages())
            {
                List<string> _list = new List<string>(m_client.GetNewMessages());
                m_client.ClearListMessages();
                for (int i = 0; i < _list.Count(); i++)
                {
                    ui_historyMessage.Items.Add(_list[i]);
                }
            }
        }
        private void SetTimer()
        {            
            m_timer = new System.Timers.Timer(1000);
            
            m_timer.Elapsed += Update;
            m_timer.AutoReset = true;
            m_timer.Enabled = true;
        }
    }
}