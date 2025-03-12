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
- Each subtask will contain a TimeEntry.csv file storing all time entries for that subtask.


### File Structure Example
    User_Folder/
    ├── User1/
    │   ├── ProjectA/
    │   │   ├── TaskA/
    │   │   │   ├── SubTask1/
    │   │   │   │   ├── TimeEntry.csv
    │   │   │   ├── SubTask2/
    │   │   │   │   ├── TimeEntry.csv
    │   │   ├── TaskB/
    │   │   │   ├── SubTask3/
    │   │   │   │   ├── TimeEntry.csv
    │   ├── ProjectB/
    │   │   ├── TaskC/
    │   │   │   ├── SubTask4/
    │   │   │   │   ├── TimeEntry.csv


### How Data is Processed
- When a user starts a timer, a new entry is added to the respective subtask’s CSV file.
- When stopping the timer, the duration is calculated and stored.
- If the user overrides a time entry, the new duration is added with a label indicating an override.
- Exporting data will generate CSV files based on selected filters (daily, weekly, yearly).

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

## Sequence Diagram 
![sequence_diagram](https://github.com/user-attachments/assets/d420aa5b-8233-4639-8688-7ba462b78edd)


## Design Explanation
Why Folder Structure Instead of Object Mapping?
- A folder structure allows modular processing without maintaining large in-memory lists.
- Searching and exporting become faster by directly accessing specific files instead of iterating over large object collections.
- Each user’s data is isolated, reducing processing complexity.


## Alternative Design

### ID-Based Mapping (Previous Approach)
- Users, projects, and tasks were stored in lists with unique IDs.
- Relational mapping was needed to fetch time entries using these IDs.
- The system required additional processing for querying and filtering data.
- Challenges:
  - Increased complexity in a console app.
  - Required maintaining indexes and relationships manually.
### Folder-Based Storage (Current Approach)
- Every entity (user, project, task) is a physical folder.
- Data retrieval is direct (no mapping required).
- Benefits:
  - Reduces in-memory operations.
  - Scales efficiently as more data is added.
  - More modular and easily extensible in a console application.
 
 
This design ensures better performance and modularity while keeping the application simple and maintainable.
