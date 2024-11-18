using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;


namespace MunicipalServicesWPF
{
    // Partial class definition for ReportIssuesWindow, inheriting from Window
    public partial class ReportIssuesWindow : Window
    {
        private List<IssueReport> issues = new List<IssueReport>();

        public ReportIssuesWindow()
        {
            InitializeComponent();
        }

        // Event handler for the "Attach Media" button click event
        private void btnAttachMedia_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png|All Files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                string selectedFile = ofd.FileName;
            }
        }

        // Event handler for the "Submit" button click event
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            IssueReport newIssue = new IssueReport
            {
                Location = txtLocation.Text,
                Category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Description = new TextRange(rtbDescription.Document.ContentStart, rtbDescription.Document.ContentEnd).Text.Trim(),
            };

            // Add the new issue to the list of issues
            issues.Add(newIssue);
            MessageBox.Show("Issue reported successfully!");
            ClearForm();

            // Update the UI to show a thank-you message
            lblEngagementMessage.Content = "Thank you for helping improve our community!";
        }

        // Clears the form fields for a new issue report
        private void ClearForm()
        {
            txtLocation.Clear();
            cmbCategory.SelectedIndex = -1;
            rtbDescription.Document.Blocks.Clear();
        }


        private void rtbDescription_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void rtbSuggestion_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

    // Definition of the IssueReport class, representing an individual issue report
    public class IssueReport
    {
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string MediaFilePath { get; set; }
    }
}

