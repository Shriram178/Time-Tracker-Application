# Estimation

Time Estimation
The project will be developed in the following phases:

| Phase                              | Estimated Time  |
|------------------------------------|-----------------|
| Requirement Analysis               | 1 day           |
| System Design                      | 1 day           |
| Development                        | 2 days          |
| &nbsp;&nbsp;&nbsp;&nbsp;- Creating Models                  | 1 hour          |
| &nbsp;&nbsp;&nbsp;&nbsp;- User Module                      | 1 hour          |
| &nbsp;&nbsp;&nbsp;&nbsp;- User Interaction                 | 1 hour          |
| &nbsp;&nbsp;&nbsp;&nbsp;- Project & Task Management Module | 3 hours         |
| &nbsp;&nbsp;&nbsp;&nbsp;- Time Tracking & Adjustment Module| 2 hours         |
| &nbsp;&nbsp;&nbsp;&nbsp;- CSV Export & Reporting Module    | 2 hours         |
| Testing                            | 1 day           |
| Deployment                         | 1 day           |
| **Total Estimated Time**           | **6 days**      |


# Design

## Problem statement

**“I need some way to track and analyze time”** 

The customer requires an application similar to Clockify to track and analyze time. Employees should be able to start and stop timers for tasks under projects, which only managers can create. Each task will store multiple time entries, and users should be able to export time-tracking data in CSV format for daily, weekly, and yearly views. Additionally, users must be able to adjust time entries without modifying the original timestamps. The application must support both predefined and custom tasks.

## Implementation/Design
### Purpose and Usage
The application allows employees to track time spent on tasks by starting and stopping timers. It also provides a way to adjust recorded time entries when needed. Users can view and export reports for specific time periods. The console-based UI enables straightforward interactions with options to manage projects, tasks, and time entries.

### Data Storage Approach
- Each user will have a dedicated folder.
- Inside each user’s folder, there will be project folders.
- Each project will contain task folders.
- Inside each task folder, there will be subtask folders.
- Each subtask will contain a `TimeEntry.csv` file storing all time entries for that subtask.
- A `TimeIndex.csv` file will exist for each user to track their own time entries efficiently.
- A `ManagerIndex.csv` will store all users' time entry metadata to allow managers to view and export reports quickly.

### File Structure Example
    │
    ├── Users/
    │   ├── John/
    │   │   ├── TimeIndex.csv  # User-specific index file
    │   │   ├── Development/
    │   │   │   ├── TaskA/
    │   │   │   │   ├── LoginPage/
    │   │   │   │   │   ├── TimeEntry.csv
    │   │   ├── Testing/
    │   │   │   ├── TaskB/
    │   │   │   │   ├── APITests/
    │   │   │   │   │   ├── TimeEntry.csv
    │
    ├── ManagerIndex.csv  # Aggregated index for all users
    ├── ExportedReports/  # Folder for exported CSV reports

### Time Entry Structure
```csv
Each TimeEntry.csv file inside a subtask contains the following columns:
StartTime, EndTime, WorkDescription, Billable, Tags, Comments
2025-03-01 09:00, 2025-03-01 11:00, Implemented login page, Yes, Feature, Initial implementation
```

### How Data is Processed
- When a user starts a timer, a new entry is added to the respective subtask’s CSV file.
- When stopping the timer, the duration is calculated and stored.
- If the user overrides a time entry, the new duration is added with a label indicating an override.
- Exporting data will generate CSV files based on selected filters (daily, weekly, yearly).
- The `ManagerIndex.csv` will allow managers to filter time entries for all users.

### Indexing for Fast Retrieval
Instead of scanning all folders for data, we maintain index files to quickly look up and filter time entries:
### 1. TimeIndex.csv (User-Specific)
- Located inside each user’s folder.
- Contains only that specific user's tasks and time entries.
- Helps the user quickly filter and retrieve their own time entries without needing global access.
### 2. ManagerIndex.csv (Global)
- A single file that aggregates data from all users.
- Allows the manager to view all user time entries in one place.
- Eliminates the need for the manager to traverse each user's folder individually.

### Index File Structure:
```csv
User,Project,Task,SubTask,FilePath,StartTime
John,Development,TaskA,LoginPage,Users/John/Development/TaskA/LoginPage/TimeEntry.csv,2025-03-01
Alice,Testing,TaskB,APITests,Users/Alice/Testing/TaskB/APITests/TimeEntry.csv,2025-02-15
```


## User Interaction in the Console UI
The console application will display a dashboard where users select options:
1. Login/Register
2. Create a Project (Manager Only)
3. Select a Project and View Tasks
4. Create a Task (custom Task)
5. Create a SubTask
6. Start/Stop Timer on a SubTask
7. Adjust Time Entry
8. Export Report
9. Logout

## CRUD Operations
- Projects: Can be created and deleted (Only by manager).
- Tasks: Can be created (custom tasks only) and deleted.
- Subtasks: Can be created and deleted.
- Time Entries: Can be created and modified, but deletion is not allowed to maintain data integrity.


## Data Storage Location
- User folders will be stored in a dedicated directory: `AppData/Users/`
- Exported CSV files will be saved in: `AppData/Exports/`



## Sequence Diagram 
![sequence_diagram](https://github.com/user-attachments/assets/d420aa5b-8233-4639-8688-7ba462b78edd)


## Design Explanation
Why Folder Structure Instead of Object Mapping?
- A folder structure allows modular processing without maintaining large in-memory lists.
- Searching and exporting become faster by directly accessing specific files instead of iterating over large object collections.
- Each user’s data is isolated, reducing processing complexity.

## Non-Functional Requirements (NFRs)
- Scalability: The application supports a folder-based structure that allows modular data expansion. Each user has an independent workspace, preventing data conflicts.
- Performance: Index files (TimeIndex.csv for users and ManagerIndex.csv for managers) enable fast filtering and lookups, reducing the need for full-folder scans.
- Application Memory & Disk Usage: The application is designed for lightweight storage, with an estimated disk space requirement starting at 100 MB, scaling based on the number of users and time entries.
- Data Integrity: Time entries cannot be deleted, ensuring that all recorded work history remains intact. Adjustments modify duration but do not overwrite the original entry.
- System Uptime: The application operates reliably in a local environment without dependency on external services, minimizing downtime.


## Alternative Design

### ID-Based Mapping (Previous Approach)
- Users, projects, and tasks were stored in lists with unique IDs.
- Relational mapping was needed to fetch time entries using these IDs.
- The system required additional processing for querying and filtering data.
- Challenges:
  - Increased complexity in a console app.
  - Required maintaining indexes and relationships manually.
 
 
This design ensures better performance and modularity while keeping the application simple and maintainable.
