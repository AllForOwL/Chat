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
using Chat;

namespace Chat
{
    public partial class FormClient : Form
    {
        Client m_client = new Client();
        System.Timers.Timer m_timer;

        public FormClient()
        {
            InitializeComponent();
        }

        private void FormClient_Load(object sender, EventArgs e)
        {
            m_client.StartClient();
            SetTimer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_client.SendMessage(ui_textMessage.Text);
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

/*
 * SetTimer();

      Console.WriteLine("\nPress the Enter key to exit the application...\n");
      Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
      Console.ReadLine();
      aTimer.Stop();
      aTimer.Dispose();

      Console.WriteLine("Terminating the application...");
   }

   private static void SetTimer()
   {
        // Create a timer with a two second interval.
        aTimer = new  System.Timers.Timer(2000);
        // Hook up the Elapsed event for the timer. 
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }

    private static void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                          e.SignalTime);
    }
 */