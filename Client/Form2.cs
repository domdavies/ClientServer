using System;
using System.Windows.Forms;

namespace Client
{
    public partial class Gameform : Form
    {
        Client gameClient;
        
        public Gameform(Client _gameClient)
        {
            gameClient = _gameClient;
            InitializeComponent();
        }

        public void UpdateGameWindow(string message)
        {
            if (label1.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    UpdateGameWindow(message);
                }));
            }
            else
            {
                label1.Text = message + Environment.NewLine;
            }
        }

        private void game1_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
