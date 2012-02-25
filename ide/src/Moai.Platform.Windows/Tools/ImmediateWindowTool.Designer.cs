namespace Moai.Platform.Windows.Tools
{
    partial class ImmediateWindowTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImmediateWindowTool));
            this.c_ImmediateTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // c_ImmediateTextBox
            // 
            this.c_ImmediateTextBox.BackColor = System.Drawing.Color.White;
            this.c_ImmediateTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.c_ImmediateTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.c_ImmediateTextBox.Location = new System.Drawing.Point(0, 0);
            this.c_ImmediateTextBox.Multiline = true;
            this.c_ImmediateTextBox.Name = "c_ImmediateTextBox";
            this.c_ImmediateTextBox.ReadOnly = true;
            this.c_ImmediateTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.c_ImmediateTextBox.Size = new System.Drawing.Size(802, 210);
            this.c_ImmediateTextBox.TabIndex = 0;
            this.c_ImmediateTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.c_ImmediateTextBox_KeyDown);
            // 
            // ImmediateWindowTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 210);
            this.Controls.Add(this.c_ImmediateTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImmediateWindowTool";
            this.TabText = "Immediate Window";
            this.Text = "Immediate Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox c_ImmediateTextBox;
    }
}