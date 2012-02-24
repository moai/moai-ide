using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using MOAI.Templates.Files;

namespace MOAI.Management
{
    /// <summary>
    /// The new solution form.
    /// TODO: Clean up this code!
    /// </summary>
    public partial class NewFileForm : Form
    {
        private Dictionary<string, string> m_TemplateDescriptionMappings = new Dictionary<string, string>();
        private List<Type> m_TemplateTypes = new List<Type>();
        private BaseTemplate p_SelectedTemplate = null;
        private string m_PreselectedValue = null;

        public NewFileForm(string preselected)
        {
            this.InitializeComponent();

            // Set the preselected value.
            this.m_PreselectedValue = preselected;
        }

        /// <summary>
        /// The creation information generated from the form.
        /// </summary>
        public FileCreationData Result
        {
            get
            {
                FileCreationData data = new FileCreationData();
                data.Name = this.c_FileNameTextBox.Text;
                data.Template = this.p_SelectedTemplate;
                return data;
            }
        }

        /// <summary>
        /// Initializes the list of templates available.
        /// </summary>
        private void InitializeTemplates()
        {
            // Clear existing images and items.
            this.c_FileTypeListView.Items.Clear();
            this.c_ImageList.Images.Clear();

            // Use reflection to get a list of available templates in this IDE.
            this.m_TemplateTypes.Clear();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type t in types)
            {
                if (t.BaseType == typeof(BaseTemplate))
                    this.m_TemplateTypes.Add(t);
            }

            // Now add a list of templates to the new solution form.
            foreach (Type t in this.m_TemplateTypes)
            {
                BaseTemplate b = t.GetConstructor(Type.EmptyTypes).Invoke(null) as BaseTemplate;

                // Create a list view icon.
                ListViewItem lvi = new ListViewItem();
                lvi.Text = b.TemplateName;
                lvi.Group = c_FileTypeListView.Groups[0];
                lvi.Name = "Item_" + b.GetType().Name;
                if (b.TemplateIcon != null)
                {
                    this.c_ImageList.Images.Add(b.TemplateIcon);
                    lvi.ImageIndex = this.c_ImageList.Images.Count - 1;
                }
                this.m_TemplateDescriptionMappings.Add(b.TemplateName, b.TemplateDescription);
                
                // See if we should preselect it.
                if (t.FullName.EndsWith("." + this.m_PreselectedValue))
                    lvi.Selected = true;

                // Add it.
                this.c_FileTypeListView.Items.Add(lvi);
            }
        }

        /// <summary>
        /// Determines whether a particular string value contains invalid path characters.
        /// </summary>
        /// <param name="name">The string to check.</param>
        /// <returns>Whether the string contains one or more of '\/:*?"<>|'.</returns>
        private Boolean IsValidName(string name)
        {
            return (name.IndexOfAny(new char[] { '\\', '/', ':', '*', '?', '"', '<', '>', '|' }) == -1);
        }

        #region Form Events

        /// <summary>
        /// This event is raised when the Project Name text box is changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void c_ProjectNameTextBox_TextChanged(object sender, EventArgs e)
        {
            // Determine whether the current value is invalid.
            this.c_FileNameInvalidPictureBox.Visible = !this.IsValidName(this.c_FileNameTextBox.Text);
        }

        /// <summary>
        /// This event is raised when the form is loaded.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void NewSolutionForm_Load(object sender, EventArgs e)
        {
            // Initialize the template options.
            this.InitializeTemplates();

            // Select the first template by default.
            this.c_FileTypeListView.SelectedIndices.Clear();
            this.c_FileTypeListView.SelectedIndices.Add(0);
        }

        /// <summary>
        /// This event is raised when the type of template to use is changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void c_ProjectTypeListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Determine whether or not the selection is invalid.
            this.c_FileTypeInvalidPictureBox.Visible = (this.c_FileTypeListView.SelectedItems.Count == 0);

            // Only attempt to determine the description if there is one
            // item selected.
            if (this.c_FileTypeListView.SelectedItems.Count == 1)
            {
                // Get the name of the selected item and pair it up with
                // the appropriate BaseTemplate.
                string name = this.c_FileTypeListView.SelectedItems[0].Name;
                this.p_SelectedTemplate = null;
                foreach (Type t in this.m_TemplateTypes)
                    if (name == "Item_" + t.Name)
                    {
                        this.p_SelectedTemplate = t.GetConstructor(Type.EmptyTypes).Invoke(null) as BaseTemplate;
                        break;
                    }

                // Check to make sure that we've actually got a template.
                if (this.p_SelectedTemplate == null)
                {
                    this.c_FileTypeInvalidPictureBox.Visible = true;
                    this.c_TemplateDescriptionTextBox.Text = "Unknown template type selected.";
                    return;
                }

                // Update the description area.
                this.c_TemplateDescriptionTextBox.Text = "Creates " + this.p_SelectedTemplate.TemplateDescription;
            }
        }

        /// <summary>
        /// This event is raised when the OK button is pressed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void c_OKButton_Click(object sender, EventArgs e)
        {
            if (c_FileNameInvalidPictureBox.Visible || c_FileTypeInvalidPictureBox.Visible)
            {
                MessageBox.Show("One or more of the provided fields is invalid.  Make sure that all of the fields are valid and a file type is selected and try again.",
                    "Invalid Information Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// This event is raised when the Cancel button is pressed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void c_CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}
