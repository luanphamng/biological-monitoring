namespace DistanceSensor
{
    partial class Form1
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
            this.connect = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.disconect = new System.Windows.Forms.Button();
            this.ip3 = new System.Windows.Forms.TextBox();
            this.portInput = new System.Windows.Forms.TextBox();
            this.ip2 = new System.Windows.Forms.TextBox();
            this.ip4 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ip1 = new System.Windows.Forms.TextBox();
            this.Control = new System.Windows.Forms.GroupBox();
            this.temperature = new System.Windows.Forms.Button();
            this.temperatureDisplay = new System.Windows.Forms.RichTextBox();
            this.led = new System.Windows.Forms.PictureBox();
            this.off = new System.Windows.Forms.Button();
            this.on = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.Control.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.led)).BeginInit();
            this.SuspendLayout();
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(72, 59);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(104, 23);
            this.connect.TabIndex = 21;
            this.connect.Text = "Connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.disconect);
            this.groupBox1.Controls.Add(this.connect);
            this.groupBox1.Controls.Add(this.ip3);
            this.groupBox1.Controls.Add(this.portInput);
            this.groupBox1.Controls.Add(this.ip2);
            this.groupBox1.Controls.Add(this.ip4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ip1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 100);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connect";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // disconect
            // 
            this.disconect.Enabled = false;
            this.disconect.Location = new System.Drawing.Point(184, 59);
            this.disconect.Name = "disconect";
            this.disconect.Size = new System.Drawing.Size(102, 23);
            this.disconect.TabIndex = 22;
            this.disconect.Text = "Disconect";
            this.disconect.UseVisualStyleBackColor = true;
            this.disconect.Click += new System.EventHandler(this.disconect_Click);
            // 
            // ip3
            // 
            this.ip3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ip3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ip3.Location = new System.Drawing.Point(164, 20);
            this.ip3.Name = "ip3";
            this.ip3.Size = new System.Drawing.Size(40, 20);
            this.ip3.TabIndex = 11;
            this.ip3.Text = "1";
            // 
            // portInput
            // 
            this.portInput.BackColor = System.Drawing.SystemColors.ControlLight;
            this.portInput.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.portInput.Location = new System.Drawing.Point(291, 20);
            this.portInput.Name = "portInput";
            this.portInput.Size = new System.Drawing.Size(40, 20);
            this.portInput.TabIndex = 13;
            this.portInput.Text = "80";
            // 
            // ip2
            // 
            this.ip2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ip2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ip2.Location = new System.Drawing.Point(118, 20);
            this.ip2.Name = "ip2";
            this.ip2.Size = new System.Drawing.Size(40, 20);
            this.ip2.TabIndex = 10;
            this.ip2.Text = "168";
            // 
            // ip4
            // 
            this.ip4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ip4.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ip4.Location = new System.Drawing.Point(210, 20);
            this.ip4.Name = "ip4";
            this.ip4.Size = new System.Drawing.Size(40, 20);
            this.ip4.TabIndex = 12;
            this.ip4.Text = "14";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Port:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP address:";
            // 
            // ip1
            // 
            this.ip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ip1.Cursor = System.Windows.Forms.Cursors.Default;
            this.ip1.Location = new System.Drawing.Point(72, 20);
            this.ip1.Name = "ip1";
            this.ip1.Size = new System.Drawing.Size(40, 20);
            this.ip1.TabIndex = 9;
            this.ip1.Text = "192";
            // 
            // Control
            // 
            this.Control.Controls.Add(this.temperature);
            this.Control.Controls.Add(this.temperatureDisplay);
            this.Control.Controls.Add(this.led);
            this.Control.Controls.Add(this.off);
            this.Control.Controls.Add(this.on);
            this.Control.Location = new System.Drawing.Point(12, 128);
            this.Control.Name = "Control";
            this.Control.Size = new System.Drawing.Size(665, 433);
            this.Control.TabIndex = 23;
            this.Control.TabStop = false;
            this.Control.Text = "Control";
            // 
            // temperature
            // 
            this.temperature.Location = new System.Drawing.Point(512, 404);
            this.temperature.Name = "temperature";
            this.temperature.Size = new System.Drawing.Size(147, 23);
            this.temperature.TabIndex = 25;
            this.temperature.Text = "GET TEMPERATURE";
            this.temperature.UseVisualStyleBackColor = true;
            this.temperature.Click += new System.EventHandler(this.temperature_Click);
            // 
            // temperatureDisplay
            // 
            this.temperatureDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.temperatureDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.temperatureDisplay.Location = new System.Drawing.Point(144, 19);
            this.temperatureDisplay.Name = "temperatureDisplay";
            this.temperatureDisplay.Size = new System.Drawing.Size(515, 379);
            this.temperatureDisplay.TabIndex = 24;
            this.temperatureDisplay.Text = "";
            // 
            // led
            // 
            this.led.Image = global::DistanceSensor.Properties.Resources.off;
            this.led.InitialImage = global::DistanceSensor.Properties.Resources.off;
            this.led.Location = new System.Drawing.Point(9, 19);
            this.led.Name = "led";
            this.led.Size = new System.Drawing.Size(129, 122);
            this.led.TabIndex = 2;
            this.led.TabStop = false;
            // 
            // off
            // 
            this.off.Enabled = false;
            this.off.Location = new System.Drawing.Point(72, 161);
            this.off.Name = "off";
            this.off.Size = new System.Drawing.Size(66, 23);
            this.off.TabIndex = 1;
            this.off.Text = "OFF";
            this.off.UseVisualStyleBackColor = true;
            this.off.Click += new System.EventHandler(this.off_Click);
            // 
            // on
            // 
            this.on.Enabled = false;
            this.on.Location = new System.Drawing.Point(6, 161);
            this.on.Name = "on";
            this.on.Size = new System.Drawing.Size(60, 23);
            this.on.TabIndex = 0;
            this.on.Text = "ON";
            this.on.UseVisualStyleBackColor = true;
            this.on.Click += new System.EventHandler(this.on_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 573);
            this.Controls.Add(this.Control);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Control.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.led)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ip3;
        private System.Windows.Forms.TextBox portInput;
        private System.Windows.Forms.TextBox ip2;
        private System.Windows.Forms.TextBox ip4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ip1;
        private System.Windows.Forms.GroupBox Control;
        private System.Windows.Forms.PictureBox led;
        private System.Windows.Forms.Button off;
        private System.Windows.Forms.Button on;
        private System.Windows.Forms.Button disconect;
        private System.Windows.Forms.Button temperature;
        private System.Windows.Forms.RichTextBox temperatureDisplay;
    }
}

