namespace WFA_preTPI_dougoudxa_networkchat
{
    partial class udpClientForm
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
            this.readTextBox = new System.Windows.Forms.TextBox();
            this.writeTextBox = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // readTextBox
            // 
            this.readTextBox.BackColor = System.Drawing.SystemColors.Menu;
            this.readTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.readTextBox.Location = new System.Drawing.Point(12, 12);
            this.readTextBox.Multiline = true;
            this.readTextBox.Name = "readTextBox";
            this.readTextBox.ReadOnly = true;
            this.readTextBox.Size = new System.Drawing.Size(280, 387);
            this.readTextBox.TabIndex = 1;
            // 
            // writeTextBox
            // 
            this.writeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.writeTextBox.Location = new System.Drawing.Point(12, 405);
            this.writeTextBox.Name = "writeTextBox";
            this.writeTextBox.Size = new System.Drawing.Size(200, 20);
            this.writeTextBox.TabIndex = 0;
            this.writeTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.writeTextBox_KeyPress);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(217, 405);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 20);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // udpClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 437);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.writeTextBox);
            this.Controls.Add(this.readTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "udpClientForm";
            this.Text = "UDP - Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.udpClientForm_FormClosing_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox readTextBox;
        private System.Windows.Forms.TextBox writeTextBox;
        private System.Windows.Forms.Button buttonSend;
    }
}

