namespace Moai.Platform.Windows.Management
{
    partial class NewFileForm
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
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Moai Templates", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewFileForm));
            this.c_FileTypeListView = new System.Windows.Forms.ListView();
            this.c_ImageList = new System.Windows.Forms.ImageList(this.components);
            this.c_TemplateDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.c_NameLabel = new System.Windows.Forms.Label();
            this.c_FileNameTextBox = new System.Windows.Forms.TextBox();
            this.c_FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.c_CancelButton = new System.Windows.Forms.Button();
            this.c_OKButton = new System.Windows.Forms.Button();
            this.c_GroupBox = new System.Windows.Forms.GroupBox();
            this.c_FileNameInvalidPictureBox = new System.Windows.Forms.PictureBox();
            this.c_FileTypeInvalidPictureBox = new System.Windows.Forms.PictureBox();
            this.c_FlowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.c_FileNameInvalidPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c_FileTypeInvalidPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // c_FileTypeListView
            // 
            listViewGroup2.Header = "Moai Templates";
            listViewGroup2.Name = "lvgTemplates";
            this.c_FileTypeListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup2});
            this.c_FileTypeListView.HideSelection = false;
            this.c_FileTypeListView.LargeImageList = this.c_ImageList;
            this.c_FileTypeListView.Location = new System.Drawing.Point(12, 12);
            this.c_FileTypeListView.MultiSelect = false;
            this.c_FileTypeListView.Name = "c_FileTypeListView";
            this.c_FileTypeListView.Size = new System.Drawing.Size(592, 149);
            this.c_FileTypeListView.TabIndex = 0;
            this.c_FileTypeListView.UseCompatibleStateImageBehavior = false;
            this.c_FileTypeListView.SelectedIndexChanged += new System.EventHandler(this.c_ProjectTypeListView_SelectedIndexChanged);
            // 
            // c_ImageList
            // 
            this.c_ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("c_ImageList.ImageStream")));
            this.c_ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.c_ImageList.Images.SetKeyName(0, "LuaManaged");
            this.c_ImageList.Images.SetKeyName(1, "LuaUnmanaged");
            // 
            // c_TemplateDescriptionTextBox
            // 
            this.c_TemplateDescriptionTextBox.Location = new System.Drawing.Point(13, 167);
            this.c_TemplateDescriptionTextBox.Name = "c_TemplateDescriptionTextBox";
            this.c_TemplateDescriptionTextBox.ReadOnly = true;
            this.c_TemplateDescriptionTextBox.Size = new System.Drawing.Size(591, 20);
            this.c_TemplateDescriptionTextBox.TabIndex = 1;
            // 
            // c_NameLabel
            // 
            this.c_NameLabel.AutoSize = true;
            this.c_NameLabel.Location = new System.Drawing.Point(12, 197);
            this.c_NameLabel.Name = "c_NameLabel";
            this.c_NameLabel.Size = new System.Drawing.Size(38, 13);
            this.c_NameLabel.TabIndex = 2;
            this.c_NameLabel.Text = "Name:";
            // 
            // c_FileNameTextBox
            // 
            this.c_FileNameTextBox.Location = new System.Drawing.Point(154, 194);
            this.c_FileNameTextBox.Name = "c_FileNameTextBox";
            this.c_FileNameTextBox.Size = new System.Drawing.Size(353, 20);
            this.c_FileNameTextBox.TabIndex = 3;
            this.c_FileNameTextBox.TextChanged += new System.EventHandler(this.c_ProjectNameTextBox_TextChanged);
            // 
            // c_FlowLayoutPanel
            // 
            this.c_FlowLayoutPanel.Controls.Add(this.c_CancelButton);
            this.c_FlowLayoutPanel.Controls.Add(this.c_OKButton);
            this.c_FlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.c_FlowLayoutPanel.Location = new System.Drawing.Point(439, 239);
            this.c_FlowLayoutPanel.Name = "c_FlowLayoutPanel";
            this.c_FlowLayoutPanel.Size = new System.Drawing.Size(168, 29);
            this.c_FlowLayoutPanel.TabIndex = 4;
            // 
            // c_CancelButton
            // 
            this.c_CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.c_CancelButton.Location = new System.Drawing.Point(90, 3);
            this.c_CancelButton.Name = "c_CancelButton";
            this.c_CancelButton.Size = new System.Drawing.Size(75, 23);
            this.c_CancelButton.TabIndex = 1;
            this.c_CancelButton.Text = "Cancel";
            this.c_CancelButton.UseVisualStyleBackColor = true;
            this.c_CancelButton.Click += new System.EventHandler(this.c_CancelButton_Click);
            // 
            // c_OKButton
            // 
            this.c_OKButton.Location = new System.Drawing.Point(9, 3);
            this.c_OKButton.Name = "c_OKButton";
            this.c_OKButton.Size = new System.Drawing.Size(75, 23);
            this.c_OKButton.TabIndex = 0;
            this.c_OKButton.Text = "OK";
            this.c_OKButton.UseVisualStyleBackColor = true;
            this.c_OKButton.Click += new System.EventHandler(this.c_OKButton_Click);
            // 
            // c_GroupBox
            // 
            this.c_GroupBox.Location = new System.Drawing.Point(15, 220);
            this.c_GroupBox.Name = "c_GroupBox";
            this.c_GroupBox.Size = new System.Drawing.Size(589, 7);
            this.c_GroupBox.TabIndex = 5;
            this.c_GroupBox.TabStop = false;
            // 
            // c_FileNameInvalidPictureBox
            // 
            this.c_FileNameInvalidPictureBox.Image = Moai.IDE.Resources.Images.icon_invalid;
            this.c_FileNameInvalidPictureBox.Location = new System.Drawing.Point(513, 194);
            this.c_FileNameInvalidPictureBox.Name = "c_FileNameInvalidPictureBox";
            this.c_FileNameInvalidPictureBox.Size = new System.Drawing.Size(20, 20);
            this.c_FileNameInvalidPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.c_FileNameInvalidPictureBox.TabIndex = 12;
            this.c_FileNameInvalidPictureBox.TabStop = false;
            this.c_FileNameInvalidPictureBox.Visible = false;
            // 
            // c_FileTypeInvalidPictureBox
            // 
            this.c_FileTypeInvalidPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.c_FileTypeInvalidPictureBox.BackColor = System.Drawing.Color.White;
            this.c_FileTypeInvalidPictureBox.Image = Moai.IDE.Resources.Images.icon_invalid;
            this.c_FileTypeInvalidPictureBox.Location = new System.Drawing.Point(581, 16);
            this.c_FileTypeInvalidPictureBox.Name = "c_FileTypeInvalidPictureBox";
            this.c_FileTypeInvalidPictureBox.Size = new System.Drawing.Size(20, 20);
            this.c_FileTypeInvalidPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.c_FileTypeInvalidPictureBox.TabIndex = 16;
            this.c_FileTypeInvalidPictureBox.TabStop = false;
            this.c_FileTypeInvalidPictureBox.Visible = false;
            // 
            // NewFileForm
            // 
            this.AcceptButton = this.c_OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.c_CancelButton;
            this.ClientSize = new System.Drawing.Size(616, 281);
            this.Controls.Add(this.c_FileTypeInvalidPictureBox);
            this.Controls.Add(this.c_FileNameInvalidPictureBox);
            this.Controls.Add(this.c_GroupBox);
            this.Controls.Add(this.c_FlowLayoutPanel);
            this.Controls.Add(this.c_FileNameTextBox);
            this.Controls.Add(this.c_NameLabel);
            this.Controls.Add(this.c_TemplateDescriptionTextBox);
            this.Controls.Add(this.c_FileTypeListView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewFileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add New Item...";
            this.Load += new System.EventHandler(this.NewSolutionForm_Load);
            this.c_FlowLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.c_FileNameInvalidPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c_FileTypeInvalidPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList c_ImageList;
        private System.Windows.Forms.TextBox c_TemplateDescriptionTextBox;
        private System.Windows.Forms.Label c_NameLabel;
        private System.Windows.Forms.FlowLayoutPanel c_FlowLayoutPanel;
        private System.Windows.Forms.Button c_OKButton;
        private System.Windows.Forms.Button c_CancelButton;
        private System.Windows.Forms.GroupBox c_GroupBox;
        private System.Windows.Forms.PictureBox c_FileNameInvalidPictureBox;
        private System.Windows.Forms.PictureBox c_FileTypeInvalidPictureBox;
        private System.Windows.Forms.TextBox c_FileNameTextBox;
        private System.Windows.Forms.ListView c_FileTypeListView;
    }
}