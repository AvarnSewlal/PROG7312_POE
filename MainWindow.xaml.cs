using System.Windows;

namespace MunicipalServicesWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Handles the "Report Issues" button click event
        private void btnReportIssues_Click(object sender, RoutedEventArgs e)
        {
            // Open the Report Issues Window
            ReportIssuesWindow reportWindow = new ReportIssuesWindow();
            reportWindow.ShowDialog();
        }

        // Handles the "Local Events" button click event
        private void btnLocalEvents_Click(object sender, RoutedEventArgs e)
        {
            // Open the Local Events Window
            LocalEvents localEventsWindow = new LocalEvents();
            localEventsWindow.ShowDialog();
        }

        // Handles the "Service Request Status" button click event
        private void btnServiceRequestStatus_Click(object sender, RoutedEventArgs e)
        {
            // Open the Service Request Status Window
            ServiceRequestStatusPage serviceRequestStatusWindow = new ServiceRequestStatusPage();
            serviceRequestStatusWindow.ShowDialog();
        }
    }
}


