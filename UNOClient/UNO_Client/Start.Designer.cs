namespace UNO_Client
{
    partial class Start
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
            this.components = new System.ComponentModel.Container();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelNotice = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.timerReady = new System.Windows.Forms.Timer(this.components);
            this.timerStart = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(306, 232);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(164, 23);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(309, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Inserisci un username";
            // 
            // labelNotice
            // 
            this.labelNotice.AutoSize = true;
            this.labelNotice.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelNotice.Location = new System.Drawing.Point(259, 373);
            this.labelNotice.Name = "labelNotice";
            this.labelNotice.Size = new System.Drawing.Size(269, 28);
            this.labelNotice.TabIndex = 3;
            this.labelNotice.Text = "In attesa di un altro giocatore";
            this.labelNotice.Visible = false;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(320, 288);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(132, 53);
            this.StartButton.TabIndex = 4;
            this.StartButton.Text = "INIZIA";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // timerReady
            // 
            this.timerReady.Interval = 1000;
            this.timerReady.Tick += new System.EventHandler(this.timerReady_Tick);
            // 
            // timerStart
            // 
            this.timerStart.Interval = 1000;
            this.timerStart.Tick += new System.EventHandler(this.timerStart_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::UNO_Client.Properties.Resources.UNO_Logo;
            this.pictureBox1.Location = new System.Drawing.Point(286, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(208, 137);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.labelNotice);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.MaximumSize = new System.Drawing.Size(816, 489);
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "Start";
            this.Text = "Start";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBox textBox1;
        private Label label1;
        private Label labelNotice;
        private Button StartButton;
        private System.Windows.Forms.Timer timerReady;
        private System.Windows.Forms.Timer timerStart;
        private PictureBox pictureBox1;
    }
}