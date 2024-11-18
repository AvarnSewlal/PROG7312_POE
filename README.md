# MunicipalServicesWPF

## Overview

`MunicipalServicesWPF` is a WPF (Windows Presentation Foundation) application designed for managing service requests in a municipal services system. This application allows users to add service requests, search for requests by ID, and visualize relationships between requests (i.e., dependencies) using a Binary Search Tree (BST) and a Graph data structure.

The system can be useful in managing and visualizing service requests such as maintenance tasks, repairs, and inspections within municipal services. The application leverages key data structures—**Binary Search Tree (BST)** and **Graph**—to efficiently store, search, and manage the requests and their relationships.

## Features

- **Add a Service Request**: Users can input a new service request with a unique ID, description, and status.
- **Search for a Service Request**: Users can search for a specific service request by its ID.
- **View Service Request Relationships**: Users can view the relationships (dependencies) between different service requests.
- **Ordered List of Requests**: The service requests are displayed in an ordered list based on their ID using an in-order traversal of the BST.

## Data Structures Used

### 1. **Binary Search Tree (BST)**

A **Binary Search Tree (BST)** is used to store and manage the service requests. A BST is a data structure that organizes data in a tree-like structure, where each node contains a value, and each left child node contains a value less than its parent node, while each right child node contains a value greater than its parent node.

**Relevance to the app**:  
The BST ensures that service requests are stored in a way that allows for efficient searching, insertion, and traversal. The BST is used in the `ServiceRequestStatusPage` to store all the service requests in an ordered manner based on the `RequestID`. This allows users to:
- Efficiently search for a request by ID (binary search).
- Retrieve and display service requests in an ordered list using an in-order traversal.

Key Operations:
- **Insertion**: Service requests are inserted into the BST based on their `RequestID`.
- **Search**: Searching for a service request by its `RequestID` is efficient due to the properties of the BST.
- **In-order Traversal**: Displays service requests in ascending order of their IDs.

### 2. **Graph**

A **Graph** is used to represent the relationships (dependencies) between service requests. In this case, a **Directed Graph** is employed, where each node represents a service request, and each directed edge indicates that one service request depends on another.

**Relevance to the app**:  
The graph allows the app to model the dependencies between requests. For example, if one service request depends on the completion of another (e.g., "Fix water leak" depends on "Electricity issue"), this relationship can be represented as an edge in the graph. Users can visualize and manage these dependencies using the graph.

Key Operations:
- **Add Vertex**: Adds a service request to the graph as a vertex.
- **Add Edge**: Adds a dependency relationship between two service requests (i.e., one request depends on another).
- **Display Adjacency List**: Allows users to view the dependency relationships between service requests.

---

## Installation

To run the application, you will need the following:

- **Visual Studio 2022** or later.
- **.NET 6.0 SDK** or later installed on your machine.

### Steps to compile and run:

1. **Clone the Repository**:
   Clone the repository to your local machine using Git:
   ```bash
   git clone https://github.com/AvarnSewlal/PROG7312_POE
   ```

2. **Open in Visual Studio**:
   - Open Visual Studio.
   - Go to `File > Open > Project/Solution`.
   - Navigate to the cloned directory and open the `MunicipalServicesWPF.sln` file.

3. **Restore NuGet Packages**:
   - Visual Studio will automatically restore the NuGet packages when the solution is opened. If not, you can manually restore the packages by right-clicking the solution in the Solution Explorer and selecting "Restore NuGet Packages."

4. **Build the Solution**:
   - Click `Build > Build Solution` (or press `Ctrl+Shift+B`) to compile the project.

5. **Run the Application**:
   - Press `F5` or click `Debug > Start Debugging` to run the application.

---

## Using the Application

Once the application is running, you will see the main window (`ServiceRequestStatusPage`), which provides options to manage service requests.

### Key Features in the UI:

1. **Add a Service Request**:
   - Enter a unique `RequestID` in the text box.
   - Click the "Add Request" button to add a new request. The request will be added to both the BST (for storage and sorting) and the Graph (for dependency relationships).
   
2. **Search for a Service Request by ID**:
   - Enter the `RequestID` you want to search for in the search box.
   - Click the "Search" button. If the request is found, a message box will display the description and status of the request.
   
3. **View Relationships (Dependencies)**:
   - Click the "View Relationships" button to display the adjacency list of dependencies between requests in a message box. This shows which service requests are dependent on others.

### Sample Data:
When the application starts, some sample data is pre-loaded to demonstrate its functionality:
- Request 1: "Fix water leak" (Status: "Pending")
- Request 2: "Electricity issue" (Status: "Completed")
- Request 3: "Road maintenance" (Status: "In Progress")

The dependencies between requests are also set up:
- Request 1 depends on Request 2.
- Request 3 depends on Request 1.

### Error Handling:
- If you enter an invalid or duplicate `RequestID`, the app will show an error message.
- If no request with the given `RequestID` is found during a search, a "Request not found" message is displayed.

---

## Code Explanation

### `ServiceRequest` Model:
The `ServiceRequest` class represents a service request with the following properties:
- `RequestID`: A unique identifier for the request.
- `Description`: A description of the request (e.g., "Fix water leak").
- `Status`: The current status of the request (e.g., "Pending", "Completed").

The `CompareTo` method is implemented to allow the `ServiceRequest` objects to be compared based on the `RequestID` for insertion into the BST.

### `BSTree<T>`:
This class implements the Binary Search Tree (BST) data structure:
- **Insert**: Adds a new service request into the BST while maintaining its sorted order based on the `RequestID`.
- **Search**: Searches for a service request by `RequestID` using the binary search algorithm.
- **InOrderTraversal**: Traverses the BST in-order (left, root, right) to return all service requests in ascending order of their `RequestID`.

### `Graph<T>`:
The `Graph` class models the dependency relationships between service requests:
- **AddVertex**: Adds a new service request (as a vertex) to the graph.
- **AddEdge**: Adds a directed edge between two requests, indicating that one request depends on another.
- **DisplayAdjacencyList**: Displays the adjacency list, showing all the relationships between requests.

### `ServiceRequestStatusPage.xaml.cs`:
This is the main window class that handles UI interactions and integrates the BST and Graph data structures. It provides functionality to add, search, and display service requests and their dependencies.

---

## Example

After starting the application:

1. **Add New Request**: 
   - Enter `4` in the `RequestID` text box, click "Add Request," and the request is added.
   
2. **Search for a Request**: 
   - Enter `1` in the search box and click "Search" to find and display the "Fix water leak" request.

3. **View Relationships**: 
   - Click "View Relationships" to see the following output:
   ```
   1: 2
   3: 1
   ```

   This means:
   - Request 1 depends on Request 2.
   - Request 3 depends on Request 1.

---
Contributing
 To contribute:
1.	Fork the repository.
2.	Create a new branch for your feature or bugfix.
3.	Submit a pull request.

Updates to part 1:
Changes to the UI has been made to give it a cleaner look as well as comments have been added to the xmal.cs files for part 1.
Sample data used in the events page:
 
This data is preloaded to demonstrate the LocalEvents page functionality.

![WhatsApp Image 2024-10-15 at 21 39 02](https://github.com/user-attachments/assets/ec80fcd4-38c7-4385-80bb-ee0737e31cce)
For issues or questions, feel free to contact ST10083252@vcconnect.edu.za
