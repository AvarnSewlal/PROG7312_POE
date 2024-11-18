using System.Windows;

namespace MunicipalServicesWPF
{
    public partial class ServiceRequestStatusPage : Window
    {
        // Binary Search Tree for storing service requests
        private BSTree<ServiceRequest> serviceRequestsTree = new BSTree<ServiceRequest>();

        // Graph for managing relationships between service requests
        private Graph<ServiceRequest> requestGraph = new Graph<ServiceRequest>();

        public ServiceRequestStatusPage()
        {
            InitializeComponent();
            LoadSampleData();
        }

        // Loads initial data for demonstration purposes
        private void LoadSampleData()
        {
            AddServiceRequest(new ServiceRequest { RequestID = 1, Description = "Fix water leak", Status = "Pending" });
            AddServiceRequest(new ServiceRequest { RequestID = 2, Description = "Electricity issue", Status = "Completed" });
            AddServiceRequest(new ServiceRequest { RequestID = 3, Description = "Road maintenance", Status = "In Progress" });

            // Add relationships (dependencies)
            requestGraph.AddEdge(1, 2); // Request 1 depends on Request 2
            requestGraph.AddEdge(3, 1); // Request 3 depends on Request 1

            UpdateListView();
        }

        // Adds a service request to the system
        private void AddServiceRequest(ServiceRequest request)
        {
            serviceRequestsTree.Insert(request);
            requestGraph.AddVertex(request.RequestID, request);
        }

        // Updates the ListView with the current service requests
        private void UpdateListView()
        {
            var allRequests = serviceRequestsTree.InOrderTraversal();
            serviceRequestsListView.ItemsSource = allRequests;
        }

        // Adds a new request based on user input
        private void AddRequest_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(requestIdTextBox.Text, out int requestId))
            {
                AddServiceRequest(new ServiceRequest
                {
                    RequestID = requestId,
                    Description = "New request",
                    Status = "Pending"
                });
                UpdateListView();
                MessageBox.Show("Request added successfully!", "Success");
            }
            else
            {
                MessageBox.Show("Enter a valid request ID.", "Error");
            }
        }

        // Searches for a service request by ID
        private void SearchById_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(searchRequestIdTextBox.Text, out int requestId))
            {
                var request = serviceRequestsTree.Search(requestId);
                if (request != null)
                {
                    MessageBox.Show($"Request Found: {request.Description} (Status: {request.Status})", "Search Result");
                }
                else
                {
                    MessageBox.Show("Request not found.", "Search Result");
                }
            }
            else
            {
                MessageBox.Show("Enter a valid request ID.", "Error");
            }
        }

        // Displays the relationships between service requests
        private void ViewRelationships_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(requestGraph.DisplayAdjacencyList(), "Service Request Relationships");
        }
    }

    // Service Request Model
    public class ServiceRequest : IComparable<ServiceRequest>
    {
        public int RequestID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public int CompareTo(ServiceRequest other)
        {
            return RequestID.CompareTo(other.RequestID);
        }
    }

    // Binary Search Tree Node
    public class BSTNode<T> where T : IComparable<T>
    {
        public T Data { get; set; }
        public BSTNode<T> Left { get; set; }
        public BSTNode<T> Right { get; set; }
    }

    // Binary Search Tree Implementation
    public class BSTree<T> where T : IComparable<T>
    {
        private BSTNode<T> root;

        public void Insert(T data)
        {
            root = Insert(root, data);
        }

        private BSTNode<T> Insert(BSTNode<T> node, T data)
        {
            if (node == null) return new BSTNode<T> { Data = data };
            if (data.CompareTo(node.Data) < 0)
                node.Left = Insert(node.Left, data);
            else if (data.CompareTo(node.Data) > 0)
                node.Right = Insert(node.Right, data);
            return node;
        }

        public T Search(int id)
        {
            return Search(root, id);
        }

        private T Search(BSTNode<T> node, int id)
        {
            if (node == null) return default;
            if (id.CompareTo((node.Data as ServiceRequest)?.RequestID) == 0)
                return node.Data;
            return id.CompareTo((node.Data as ServiceRequest)?.RequestID) < 0 ? Search(node.Left, id) : Search(node.Right, id);
        }

        public List<T> InOrderTraversal()
        {
            var result = new List<T>();
            InOrderTraversal(root, result);
            return result;
        }

        private void InOrderTraversal(BSTNode<T> node, List<T> result)
        {
            if (node == null) return;
            InOrderTraversal(node.Left, result);
            result.Add(node.Data);
            InOrderTraversal(node.Right, result);
        }
    }

    // Graph Implementation
    public class Graph<T>
    {
        private Dictionary<int, List<int>> adjacencyList = new Dictionary<int, List<int>>();
        private Dictionary<int, T> dataMap = new Dictionary<int, T>();

        public void AddVertex(int id, T data)
        {
            if (!adjacencyList.ContainsKey(id))
            {
                adjacencyList[id] = new List<int>();
                dataMap[id] = data;
            }
        }

        public void AddEdge(int from, int to)
        {
            if (adjacencyList.ContainsKey(from) && adjacencyList.ContainsKey(to))
            {
                adjacencyList[from].Add(to);
            }
        }

        public string DisplayAdjacencyList()
        {
            return string.Join("\n", adjacencyList.Select(kv => $"{kv.Key}: {string.Join(", ", kv.Value)}"));
        }
    }
}
