
namespace Client
{
    partial class Gameform
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
            this.game1 = new Game(this.gameClient);
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // game1
            // 
            this.game1.Location = new System.Drawing.Point(12, 103);
            this.game1.MouseHoverUpdatesOnly = false;
            this.game1.Name = "game1";
            this.game1.Size = new System.Drawing.Size(776, 477);
            this.game1.TabIndex = 0;
            this.game1.Text = "game1";
            this.game1.Click += new System.EventHandler(this.game1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(242, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Waiting for A second player ...";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Gameform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 585);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.game1);
            this.Name = "Gameform";
            this.Text = "Gameform";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Game game1;
        private System.Windows.Forms.Label label1;
    }
}