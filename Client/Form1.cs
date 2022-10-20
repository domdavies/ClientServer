using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ClientForm : Form
    {
        private Client client;
        public ClientForm(Client _client)
        {
            InitializeComponent();
            button1.Enabled = false;
            button2.Enabled = false;
            inputField.Enabled = false;
            messageWindow.Enabled = false;
            submitButton.Enabled = false;
            button3.Enabled = false;
            client = _client;
        }

        public void UpdateChatWindow(string message)
        {
            if (messageWindow.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    UpdateChatWindow(message);
                }));
            }
            else
            {
                messageWindow.Text += message + Environment.NewLine;
                messageWindow.SelectionStart = messageWindow.Text.Length;
                messageWindow.ScrollToCaret();
            }
        }

        public void UpdateClientNameWindow(string message)
        {
            if (listBox1.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    UpdateClientNameWindow(message);
                }));
            }
            else
            {
                if (!listBox1.Items.Contains(message))
                {
                    listBox1.Items.Add(message);
                }
            }
        }

        public void CreateGroupChat(string GroupName)
        {
            if (listBox2.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    UpdateClientNameWindow(GroupName);
                }));
            }
            else
            {
                if (!listBox2.Items.Contains(GroupName))
                {
                    listBox2.Items.Add(GroupName);
                }
            }
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                Packets.GroupMessage groupMessage = new Packets.GroupMessage()
                {
                    message = inputField.Text,
                    GroupName = listBox2.SelectedItem.ToString(),
                    sender = textBox1.Text
                };
                groupMessage.recipients = new List<string>();
                groupMessage.recipients.Add(textBox1.Text);
                foreach (string item in GroupMembers.CheckedItems)
                {
                    groupMessage.recipients.Add(item);
                    Console.WriteLine(item);
                }
                client.SendMessage(groupMessage);
            }
            else if (listBox1.SelectedItem.ToString() == "Chat Message")
            {
                Packets.ChatMessagePacket message = new Packets.ChatMessagePacket(inputField.Text);
                message.sender = textBox1.Text;
                client.SendMessage(message);
            }
            else
            {
                Packets.PrivateMessage message = new Packets.PrivateMessage(inputField.Text, listBox1.SelectedItem.ToString(), null);
                client.SendMessage(message);
            }
            inputField.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Connect")
            {
                if (client.Connect("127.0.0.1", 4444))
                {
                    client.Run();
                    listBox1.Items.Add("Chat Message");
                    listBox1.SelectedIndex = 0;
                    Packets.Packet name = new Packets.ClientNamePacket(textBox1.Text);
                    client.SendMessage(name);
                    button2.Enabled = true;
                    inputField.Enabled = true;
                    messageWindow.Enabled = false;
                    submitButton.Enabled = true;
                    button3.Enabled = true;
                }
                else
                {
                    Console.WriteLine("Failed to connect to server.... OOPS");
                }
                button1.Text = "Disconnect";
            }
            else
            {
                button1.Text = "Connect";
                Packets.Disconnect disconnect = new Packets.Disconnect();
                client.SendMessage(disconnect);
            }    
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //allow the current client to host a game
            Packets.ConnectToGame connect = new Packets.ConnectToGame(true, textBox1.Text);
            client.SendMessage(connect);
            client.isHost = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //allow the client to join the alreedy existing game
            Packets.ConnectToGame connect = new Packets.ConnectToGame(false, inputField.Text);
            connect.hostName = null;
            client.SendMessage(connect);
            client.isHost = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            inputField.Enabled = false;
            messageWindow.Enabled = false;
            submitButton.Enabled = false;
            button3.Enabled = false;
            listBox1.Enabled = false;
            button1.Enabled = false;
            textBox1.Enabled = false;
            button4.Enabled = false;

            GroupName.Visible = true;
            GroupMembers.Visible = true;
            GroupNameText.Visible = true;
            addGroup.Visible = true;

            foreach (string item in listBox1.Items)
            {
                if (item != textBox1.Text && item != "Chat Message")
                {
                    GroupMembers.Items.Add(item);
                }
                Console.WriteLine(item);
            }
        }

        private void addGroup_Click(object sender, EventArgs e)
        {
            CreateGroupChat(GroupNameText.Text);

            button2.Enabled = true;
            inputField.Enabled = true;
            messageWindow.Enabled = false;
            submitButton.Enabled = true;
            button3.Enabled = true;
            listBox1.Enabled = true;
            button1.Enabled = true;
            textBox1.Enabled = true;
            button4.Enabled = false;

            GroupNameText.Clear();
            GroupName.Visible = false;
            GroupMembers.Visible = false;
            GroupMembers.ClearSelected();
            GroupNameText.Visible = false;
            addGroup.Visible = false;

        }
    }
}
