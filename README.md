#################################################################
#                          ST10029256_PROG7312                  #
#################################################################

## 🧑‍💻 Created by:
**Dylan Louis Miller**  
Third-year student at Varsity College  
**Student ID:** ST10029256  

-----------------------------------------------------------------

## 🚀 Installation

Follow these steps to install the application:

1. 📥 Download the zip folder from **VC Learn**.
2. 📂 Unzip the folder to your desired location.
3. 🔧 Open the solution file `ST10029256_PROG7312.sln` in **Visual Studio**.
4. 🏗️ Build the solution by navigating to the **Build** menu.

-----------------------------------------------------------------

## ▶️ Running the Program

1. Navigate to the folder where you unzipped the project.
2. Open the `ST10029256_PROG7312` folder.
3. Open the `bin` folder.
4. Open the `Debug` folder.
5. Double-click `ST10029256_PROG7312.exe` to run the application.

-----------------------------------------------------------------

## 🌟 Welcome to Municipal Services

Municipal Services is a **WPF-based application** that provides a user-friendly platform for managing and tracking municipal service requests and community events.  
This application is designed with efficiency and simplicity in mind, offering the following key features:

- 🛠️ **Report Issues**: Submit and track municipal issues, complete with location, category, priority, and attachments.  
- 📋 **View Submitted Reports**: Access a detailed history of your submitted issues, including their status and attached media.  
- 📅 **Browse Community Events and Announcements**: Create, view, and manage community events with options to filter and search.  
- 📊 **Service Request Status**: View and manage submitted service requests, with features to filter by urgency, track specific requests by ID, and edit or delete entries.  

The application leverages **in-memory data handling**, eliminating the need for an external database, making it lightweight, efficient, and easy to deploy. Advanced data structures, such as **Red-Black Trees** and **Heaps**, ensure optimal performance and effective data management for the **Service Request Status** feature.

-----------------------------------------------------------------

## 🖥️ Using the Program

### 1. 🏠 **Home Page**  
Upon launching the application, users arrive at the Home page. From here, they can navigate to:  
- 🛠️ **Report Issues**  
- 📅 **Events & Announcements**  
- 📊 **Service Request Status**

-----------------------------------------------------------------

### 2. 🛠️ **Report Issues**  
Users can report municipal issues by providing the following details:  
- 📍  **Location**: Specify the exact location of the issue.  
- 📂 **Category**: Select a category from predefined options such as Sanitation, Roads, Utilities, or Other.  
- 📅 **Date of Issue**: Indicate when the issue was first noticed.  
- ⚠️ **Priority**: Choose the urgency of the issue (High, Medium, Low).  
- 📝 **Description**: Provide a detailed explanation of the issue (up to 50 words).  
- 📎  **Attachments**: Add images or documents to provide context for the report.  

**Key Features**:  
- 🟩  **Progress Bar**: Displays report completion status.  
- ✅ **Input Validation**: Error messages for missing or invalid information.  
- 🎉 **Confirmation Pop-Up**: Notifies users of successful submission.  
- 🔄 **Navigation Options**: Redirect to Home or Report Issues Display page.

-----------------------------------------------------------------

### 3. 📋 **Report Issues Display**  
Users can view a list of their previously submitted reports, including:  
- 📍  **Location**  
- 📂 **Category**  
- 📅 **Date of Issue**  
- 📝 **Description**  
- 📊 **Status**  
- 📎  **Attached Media** (images/documents)  

**Additional Options**:  
- 🔙 Navigate back to the **Report Issues** page for further submissions.  
- 🌟 Animations enhance visual appeal, improving the user experience.

-----------------------------------------------------------------

### 4. 📅 **Events & Announcements**  
Users can create events by entering the following information:  
- 📌 **Event Name**  
- 📂 **Category** (e.g., Conference, Workshop, Community Gathering)  
- 📅 **Event Date**  
- 📍  **Location**  
- 📝 **Description**  
- 📎  **Attachments** (optional)  

-----------------------------------------------------------------

### 5. 📋 **Events & Announcements Display**  
This section displays all created events, including:  
- 📌 **Event Name**  
- 📂 **Category**  
- 📅 **Event Date**  
- 📍  **Location**  
- 📝 **Description**  
- 📎  **Attached Media** (images/documents)  

**Key Features**:  
- 🔍 **Search**: Look for events by name.  
- 📂 **Filter**: Filter events by category or date.  
- ⭐ **Recommendations**: Suggestions based on user interaction.

-----------------------------------------------------------------

### 6. 📊 **Service Request Status Display**  
This section lists all submitted service requests, showing:  
- 🆔 **Request ID**  
- 📍  **Location**  
- 📂 **Category**  
- 📅 **Date of Issue**  
- 📝 **Description**  
- 📊 **Status**  
- 📎  **Attached Media**  

**Key Features**:  
- 📋 **View All Requests**: See a comprehensive list of service requests.  
- ⚠️ **Filter Requests**: Show only urgent or related requests.  
- 🔍 **Track Requests**: View details for a specific request using its ID.  
- 📝 **Edit/Delete Options**:  
    - ✏️ **Edit Requests**:  
        - Update the **Status** (e.g., Pending, In Progress, Resolved).  
        - Add a **Description** for additional details or progress updates.  
        - Changes are reflected on the **Report Issues Display** page.  
    - ❌ **Delete Requests**:  
        - Remove service requests that are no longer relevant.  
        - Deleted requests no longer appear on the **Service Request Status Display** page.

-----------------------------------------------------------------

## 🌟 Features

1. 🖥️ **WPF Interface**: Modern, responsive, and user-friendly.  
2. 📦 **In-Memory Data Handling**: Lightweight and efficient.  
3. 🌳 **Advanced Data Structures**:  
   -  **Red-Black Trees** for sorting and retrieval.  
   -  **Heaps** for priority-based organization.  
4. ⭐ **Recommendation Engine**: Personalized suggestions.  
5. 🔍 **Search & Filter Functionality**: Locate specific reports or events.  
6. ⚡ **Priority-Based Sorting**: Ensures high-priority issues are addressed first.  
7. 🎨 **Dynamic Animations**: Enhance user experience.  
8. ✅ **Input Validation**: Prevents errors with real-time feedback.  
9. 🟢  **Interactive Feedback**: Keeps users informed with progress bars and confirmation messages.

-----------------------------------------------------------------

## 🌐 Data Structures Explanation

### 1. 🌳 **Red-Black Trees**
- **Purpose and Role**:  
  Used in the application to store and organize service requests, ensuring efficient and reliable data management.
  
- **Efficiency in Sorting**:  
  Automatically sorts service requests in chronological or categorical order, maintaining structure without manual intervention.

- **Fast Lookup**:  
  Provides quick access to specific requests by their unique ID or attributes, enabling real-time filtering and tracking.

- **Optimized Operations**:  
  Performs insertions, deletions, and lookups in **O(log n)** time, making it suitable for managing large datasets while ensuring performance remains consistent.

- **Dynamic Balancing**:  
  Ensures that the tree remains balanced, which minimizes the depth and optimizes traversal operations.

- **Example in Use**:  
  When a user searches for related requests by category or date, the application traverses the Red-Black Tree to retrieve and display matching entries efficiently.

  -----------------------------------------------------------------

### 2. 🔺 **Heaps**
- **Purpose and Role**:  
  Focuses on prioritizing service requests based on their urgency, ensuring high-priority issues receive prompt attention.

- **Priority-Based Sorting**:  
  Utilizes a **max-heap** structure where the highest-priority requests (e.g., emergencies) are positioned at the top for immediate access.

- **Fast Access**:  
  Allows retrieval of the most urgent service request in **O(1)** time, making it ideal for ensuring critical issues are addressed first.

- **Dynamic Updates**:  
  Automatically adjusts as new requests are submitted or existing ones are updated, maintaining an accurate priority order.

- **Versatile Use Cases**:  
  - Displays urgent requests directly to municipal administrators.  
  - Organizes tasks dynamically as priorities shift over time.

- **Example in Use**:  
  If a user chooses to view "Urgent Requests" from the **Service Request Status Display**, the application uses the heap to fetch and display only the most critical issues.

These advanced data structures—**Red-Black Trees** for sorted management and **Heaps** for prioritization—form the backbone of the application's efficiency. They ensure service requests are managed, displayed, and retrieved with optimal speed and accuracy, providing a robust system for both citizens and administrators.

-----------------------------------------------------------------

### 🌐 Graphs
- **Purpose and Role**:  
  Graphs are used to represent relationships and connections between various entities, such as municipal service locations or event venues.

- **Dynamic Connections**:  
  The graph structure efficiently models nodes (e.g., locations) and edges (e.g., routes or relationships), enabling dynamic connections and navigation between related entities.

- **Optimized Pathfinding**:  
  Provides a foundation for implementing algorithms like Dijkstra's or A* for finding the shortest paths between locations.

- **Real-Time Analysis**:  
  Allows real-time analysis of connections, such as finding the nearest event venue to a user or identifying the shortest route for maintenance teams.

- **Example in Use**:  
  When a user requests the nearest event or service location, the application leverages the graph to traverse connected nodes and determine the optimal path or closest match.

-----------------------------------------------------------------

### 🔍 Graph Traversal
- **Purpose and Role**:  
  Graph traversal methods, such as Depth-First Search (DFS), are employed to navigate through the graph structure efficiently.

- **Efficient Querying**:  
  Enables the application to query relationships, such as finding all connected service points or identifying isolated nodes.

- **Optimized Resource Allocation**:  
  BFS is particularly useful for exploring all nodes at the same distance, making it ideal for tasks like scheduling or assigning resources to clustered locations.

- **Versatile Use Cases**:  
  - Identifying all events within a certain distance of a user’s location.
  - Finding isolated issues or venues needing urgent attention.
  - Determining paths for resource allocation or repair teams.

- **Example in Use**:  
  If a user searches for all service requests within a certain radius, BFS is used to explore connected nodes and gather relevant data efficiently.

------------------------------------------------------------------

## ✅ Conclusion

Municipal Services is a robust solution for managing municipal service requests and community events.  
With its responsive WPF interface, efficient in-memory data handling, and advanced data structures like **Red-Black Trees** and **Heaps**, the application ensures seamless performance and accurate data management.  
By integrating intuitive navigation, interactive features, and real-time feedback, Municipal Services empowers citizens while enhancing municipal transparency and efficiency.

-----------------------------------------------------------------

### 📜 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

-----------------------------------------------------------------

### 📬 Contact

For questions or support, reach out to:  
**Dylan Louis Miller**  
📧 Email: st10029256@vcconnect.edu.za  
🆔 Student ID: ST10029256  
