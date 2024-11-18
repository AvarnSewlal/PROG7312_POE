using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MunicipalServicesWPF
{
    public partial class LocalEvents : Window
    {
        // Data Structures
        private SortedDictionary<DateTime, List<Event>> eventsByDate = new SortedDictionary<DateTime, List<Event>>();
        private HashSet<string> eventCategories = new HashSet<string>();
        private PriorityQueue<Event, DateTime> eventQueue = new PriorityQueue<Event, DateTime>();
        private Dictionary<string, int> searchHistory = new Dictionary<string, int>();
        private Stack<Event> recentlyViewed = new Stack<Event>();
        private Queue<Event> eventNotifications = new Queue<Event>();

        // ObservableCollections for ListViews
        private ObservableCollection<Event> displayedEvents = new ObservableCollection<Event>();
        private ObservableCollection<Event> recommendedEventsCollection = new ObservableCollection<Event>();

        public LocalEvents()
        {
            InitializeComponent();
            InitializeData();
            PopulateCategoryComboBox();
            eventsListView.ItemsSource = displayedEvents;
            recommendationsListView.ItemsSource = recommendedEventsCollection;

            // Display next upcoming event
            DisplayNextUpcomingEvent();

            // Enqueue and process event notifications
            EnqueueEventNotifications();
            ProcessEventNotifications();
        }

        private void InitializeData()
        {
            // Sample Events for testing
            var sampleEvents = new List<Event>
            {
                new Event { Name = "Community Cleanup", Category = "Environment", Date = new DateTime(2024, 11, 10), Description = "Join us to clean the local park." },
                new Event { Name = "Music Concert", Category = "Entertainment", Date = new DateTime(2024, 12, 5), Description = "Live performances by local bands." },
                new Event { Name = "Tech Workshop", Category = "Education", Date = new DateTime(2024, 10, 20), Description = "Learn about the latest in technology." },
                new Event { Name = "Art Exhibition", Category = "Art", Date = new DateTime(2024, 11, 15), Description = "Showcase of local artists." },
                new Event { Name = "Food Festival", Category = "Culinary", Date = new DateTime(2024, 12, 25), Description = "Taste dishes from around the world." },

            };

            foreach (var ev in sampleEvents)
            {
                // SortedDictionary
                if (!eventsByDate.ContainsKey(ev.Date.Date))
                {
                    eventsByDate[ev.Date.Date] = new List<Event>();
                }
                eventsByDate[ev.Date.Date].Add(ev);

                // HashSet for Categories
                eventCategories.Add(ev.Category);

                // Priority Queue
                eventQueue.Enqueue(ev, ev.Date.Date);
            }
        }

        // Will Create the Category ComboBox with unique categories.

        private void PopulateCategoryComboBox()
        {
            if (eventCategories.Any())
            {
                var categories = eventCategories.OrderBy(c => c).ToList();
                categories.Insert(0, "Filter by category"); // Placeholder

                categoryComboBox.ItemsSource = categories;
                categoryComboBox.SelectedIndex = 0; // Select placeholder
            }
            else
            {
                categoryComboBox.ItemsSource = new List<string> { "No categories available" };
                categoryComboBox.SelectedIndex = 0;
            }
        }

        // Will handle the Search button click event.
        // Filter events based on search criteria and updates the ListView.
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = searchTextBox.Text.Trim().ToLower();
            string selectedCategory = categoryComboBox.SelectedItem as string;
            DateTime? selectedDate = datePicker.SelectedDate;

            if (selectedCategory == "Filter by category")
            {
                selectedCategory = null;
            }

            // Update Search History if search query is valid and not a placeholder
            if (!string.IsNullOrEmpty(searchQuery) && searchQuery != "search by event name")
            {
                if (searchHistory.ContainsKey(searchQuery))
                    searchHistory[searchQuery]++;
                else
                    searchHistory[searchQuery] = 1;
            }

            // Filter Events
            var filteredEvents = eventsByDate
                .Where(ev =>
                    (string.IsNullOrEmpty(searchQuery) || ev.Value.Any(eventItem => eventItem.Name.ToLower().Contains(searchQuery))) &&
                    (string.IsNullOrEmpty(selectedCategory) || ev.Value.Any(eventItem => eventItem.Category == selectedCategory)) &&
                    (!selectedDate.HasValue || ev.Key.Date == selectedDate.Value.Date)
                )
                .SelectMany(ev => ev.Value)
                .OrderBy(ev => ev.Date) // Sort by date
                .ToList();

            // Update ListView
            displayedEvents.Clear();
            foreach (var ev in filteredEvents)
            {
                displayedEvents.Add(ev);
            }

            // Display Recommendations
            DisplayRecommendations();
        }

        // Handles the DatePicker's SelectedDateChanged event.
        // Optionally triggers a search when the date is changed.
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // Displays recommended events based on search history.

        private void DisplayRecommendations()
        {
            // Determine top search queries
            var topSearches = searchHistory
                .OrderByDescending(sh => sh.Value)
                .Take(3)
                .Select(sh => sh.Key)
                .ToList();

            if (topSearches.Count == 0)
            {
                recommendedEventsCollection.Clear();
                return; // No recommendations to show
            }

            // Find events matching top search queries in name or category
            var recommended = eventsByDate
                .SelectMany(ev => ev.Value)
                .Where(ev => topSearches.Any(ts => ev.Name.ToLower().Contains(ts) || ev.Category.ToLower().Contains(ts)))
                .Distinct()
                .Take(5)
                .ToList();

            // Bind to recommendationsListView
            recommendedEventsCollection.Clear();
            foreach (var recEvent in recommended)
            {
                recommendedEventsCollection.Add(recEvent);
            }
        }

        // Displays the next upcoming event using the PriorityQueue.
        private void DisplayNextUpcomingEvent()
        {
            if (eventQueue.Count > 0)
            {
                var nextEvent = eventQueue.Peek(); // Peek to get without dequeuing
                MessageBox.Show($"Next Upcoming Event:\n{nextEvent.Name} on {nextEvent.Date.ToShortDateString()}", "Upcoming Event");
            }
            else
            {
                MessageBox.Show("No upcoming events.", "Information");
            }
        }

        // Enqueues events occurring within the next 7 days for notifications.
        private void EnqueueEventNotifications()
        {
            foreach (var ev in eventsByDate.Values.SelectMany(e => e))
            {
                if (ev.Date.Date <= DateTime.Now.Date.AddDays(7) && ev.Date.Date >= DateTime.Now.Date) // Events in the next 7 days
                {
                    eventNotifications.Enqueue(ev);
                }
            }
        }

        // Processes event notifications by notifying the user of upcoming events.
        private void ProcessEventNotifications()
        {
            // To prevent multiple MessageBoxes, aggregate notifications
            List<Event> upcomingEvents = new List<Event>();
            while (eventNotifications.Count > 0)
            {
                var ev = eventNotifications.Dequeue();
                upcomingEvents.Add(ev);
            }

            if (upcomingEvents.Count > 0)
            {
                string message = "Upcoming Events within the next 7 days:\n\n";
                foreach (var ev in upcomingEvents)
                {
                    message += $"{ev.Name} on {ev.Date.ToShortDateString()}\n";
                }

                MessageBox.Show(message, "Event Notifications");
            }
        }

        // Handles the SelectionChanged event of the eventsListView.
        // Tracks recently viewed events and displays event details.
        private void eventsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (eventsListView.SelectedItem is Event selectedEvent)
            {
                // Push to stack
                recentlyViewed.Push(selectedEvent);

                // Optionally, limit the stack size
                if (recentlyViewed.Count > 10)
                {
                    var tempStack = new Stack<Event>(recentlyViewed.Reverse().Skip(1));
                    recentlyViewed = new Stack<Event>(tempStack);
                }

                // Display event details or perform other actions
                MessageBox.Show($"Event Details:\nName: {selectedEvent.Name}\nCategory: {selectedEvent.Category}\nDate: {selectedEvent.Date.ToShortDateString()}\nDescription: {selectedEvent.Description}", "Event Details");
            }
        }

        // Handles the GotFocus event of the searchTextBox.
        // Clears the placeholder text when the TextBox gains focus.

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == "Search by event name")
            {
                searchTextBox.Text = "";
                searchTextBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        // Handles the LostFocus event of the searchTextBox.
        // Restores the placeholder text if the TextBox is empty when it loses focus.
        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                searchTextBox.Text = "Search by event name";
                searchTextBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        // Handles the Click event of the BackToHomepage button.
        // Navigates back to the main menu.

        private void BackToHomepage_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainMenu = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (mainMenu == null)
            {
                mainMenu = new MainWindow();
                mainMenu.Show();
            }
            else
            {
                mainMenu.Activate();
            }

            this.Close();
        }

        private void recommendationsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (recommendationsListView.SelectedItem is Event selectedRecommendedEvent)
            {
                // Display event details for the selected recommended event
                MessageBox.Show($"Recommended Event Details:\n" +
                                $"Name: {selectedRecommendedEvent.Name}\n" +
                                $"Category: {selectedRecommendedEvent.Category}\n" +
                                $"Date: {selectedRecommendedEvent.Date.ToShortDateString()}\n" +
                                $"Description: {selectedRecommendedEvent.Description}", "Recommended Event Details");
            }
        }

        private void eventsListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (eventsListView.SelectedItem is Event selectedEvent)
            {
                // Display event details for the selected event
                MessageBox.Show($"Event Details:\n" +
                                $"Name: {selectedEvent.Name}\n" +
                                $"Category: {selectedEvent.Category}\n" +
                                $"Date: {selectedEvent.Date.ToShortDateString()}\n" +
                                $"Description: {selectedEvent.Description}", "Event Details");
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }


    // Represents an Event with properties for Name, Category, Date, and Description.

    public class Event
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        // Override ToString() for better display in ListView (optional)
        public override string ToString()
        {
            return $"{Name} - {Category} - {Date.ToShortDateString()}";
        }
    }
}




