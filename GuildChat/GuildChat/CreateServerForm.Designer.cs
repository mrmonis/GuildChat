namespace GuildChat
{
    partial class CreateServerForm
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
            this.portNumberLabel = new System.Windows.Forms.Label();
            this.portNumberUpDown = new System.Windows.Forms.NumericUpDown();
            this.locationLabel = new System.Windows.Forms.Label();
            this.locationTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.createButton = new System.Windows.Forms.Button();
            this.browseLocationButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.portNumberUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // portNumberLabel
            // 
            this.portNumberLabel.AutoSize = true;
            this.portNumberLabel.Location = new System.Drawing.Point(12, 9);
            this.portNumberLabel.Name = "portNumberLabel";
            this.portNumberLabel.Size = new System.Drawing.Size(66, 13);
            this.portNumberLabel.TabIndex = 1;
            this.portNumberLabel.Text = "Port Number";
            // 
            // portNumberUpDown
            // 
            this.portNumberUpDown.Location = new System.Drawing.Point(15, 25);
            this.portNumberUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.portNumberUpDown.Name = "portNumberUpDown";
            this.portNumberUpDown.Size = new System.Drawing.Size(120, 20);
            this.portNumberUpDown.TabIndex = 2;
            this.portNumberUpDown.Value = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.Location = new System.Drawing.Point(12, 48);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(48, 13);
            this.locationLabel.TabIndex = 3;
            this.locationLabel.Text = "Location";
            // 
            // locationTextBox
            // 
            this.locationTextBox.Enabled = false;
            this.locationTextBox.Location = new System.Drawing.Point(12, 64);
            this.locationTextBox.Name = "locationTextBox";
            this.locationTextBox.ReadOnly = true;
            this.locationTextBox.Size = new System.Drawing.Size(215, 20);
            this.locationTextBox.TabIndex = 4;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(12, 99);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(93, 99);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 6;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // browseLocationButton
            // 
            this.browseLocationButton.Location = new System.Drawing.Point(233, 64);
            this.browseLocationButton.Name = "browseLocationButton";
            this.browseLocationButton.Size = new System.Drawing.Size(75, 23);
            this.browseLocationButton.TabIndex = 7;
            this.browseLocationButton.Text = "Browse";
            this.browseLocationButton.UseVisualStyleBackColor = true;
            this.browseLocationButton.Click += new System.EventHandler(this.browseLocationButton_Click);
            // 
            // CreateServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(319, 132);
            this.Controls.Add(this.browseLocationButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.locationTextBox);
            this.Controls.Add(this.locationLabel);
            this.Controls.Add(this.portNumberUpDown);
            this.Controls.Add(this.portNumberLabel);
            this.Name = "CreateServerForm";
            this.Text = "CreateServerForm";
            ((System.ComponentModel.ISupportInitialize)(this.portNumberUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label portNumberLabel;
        private System.Windows.Forms.NumericUpDown portNumberUpDown;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.TextBox locationTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button browseLocationButton;

    }
}