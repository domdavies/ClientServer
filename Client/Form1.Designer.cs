
namespace Client
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.messageWindow = new System.Windows.Forms.RichTextBox();
            this.inputField = new System.Windows.Forms.TextBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button4 = new System.Windows.Forms.Button();
            this.GroupName = new System.Windows.Forms.Label();
            this.GroupNameText = new System.Windows.Forms.TextBox();
            this.GroupMembers = new System.Windows.Forms.CheckedListBox();
            this.addGroup = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // messageWindow
            // 
            this.messageWindow.Location = new System.Drawing.Point(179, 42);
            this.messageWindow.Name = "messageWindow";
            this.messageWindow.Size = new System.Drawing.Size(431, 353);
            this.messageWindow.TabIndex = 0;
            this.messageWindow.Text = "";
            // 
            // inputField
            // 
            this.inputField.Location = new System.Drawing.Point(179, 418);
            this.inputField.Name = "inputField";
            this.inputField.Size = new System.Drawing.Size(532, 20);
            this.inputField.TabIndex = 1;
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(717, 418);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(71, 20);
            this.submitButton.TabIndex = 2;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(616, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(172, 30);
            this.button1.TabIndex = 3;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(498, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(112, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 397);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(69, 41);
            this.button2.TabIndex = 5;
            this.button2.Text = "Host Game";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(88, 397);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(70, 41);
            this.button3.TabIndex = 6;
            this.button3.Text = "Join Game";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 42);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(146, 173);
            this.listBox1.TabIndex = 7;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(616, 369);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(155, 26);
            this.button4.TabIndex = 9;
            this.button4.Text = "Create Group";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // GroupName
            // 
            this.GroupName.AutoSize = true;
            this.GroupName.Location = new System.Drawing.Point(631, 133);
            this.GroupName.Name = "GroupName";
            this.GroupName.Size = new System.Drawing.Size(64, 13);
            this.GroupName.TabIndex = 10;
            this.GroupName.Text = "GroupName";
            this.GroupName.Visible = false;
            // 
            // GroupNameText
            // 
            this.GroupNameText.Location = new System.Drawing.Point(634, 149);
            this.GroupNameText.Name = "GroupNameText";
            this.GroupNameText.Size = new System.Drawing.Size(155, 20);
            this.GroupNameText.TabIndex = 11;
            this.GroupNameText.Visible = false;
            // 
            // GroupMembers
            // 
            this.GroupMembers.FormattingEnabled = true;
            this.GroupMembers.Location = new System.Drawing.Point(634, 175);
            this.GroupMembers.Name = "GroupMembers";
            this.GroupMembers.Size = new System.Drawing.Size(154, 169);
            this.GroupMembers.TabIndex = 12;
            this.GroupMembers.Visible = false;
            this.GroupMembers.SelectedIndexChanged += new System.EventHandler(this.GroupMembers_SelectedIndexChanged);
            // 
            // addGroup
            // 
            this.addGroup.Location = new System.Drawing.Point(680, 344);
            this.addGroup.Name = "addGroup";
            this.addGroup.Size = new System.Drawing.Size(64, 19);
            this.addGroup.TabIndex = 13;
            this.addGroup.Text = "Add";
            this.addGroup.UseVisualStyleBackColor = true;
            this.addGroup.Visible = false;
            this.addGroup.Click += new System.EventHandler(this.addGroup_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Contacts";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 218);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Groups";
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point(12, 234);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(146, 147);
            this.listBox3.TabIndex = 16;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addGroup);
            this.Controls.Add(this.GroupMembers);
            this.Controls.Add(this.GroupNameText);
            this.Controls.Add(this.GroupName);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.inputField);
            this.Controls.Add(this.messageWindow);
            this.Name = "ClientForm";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox messageWindow;
        private System.Windows.Forms.TextBox inputField;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label GroupName;
        private System.Windows.Forms.TextBox GroupNameText;
        private System.Windows.Forms.CheckedListBox GroupMembers;
        private System.Windows.Forms.Button addGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ListBox listBox3;
        public System.Windows.Forms.TextBox textBox1;
    }
}