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

“I need some way to track and analyze time” 

 

The customer wants me to create an app similar to clockify , which enables their employees to track time using it (Show running timer). Each task will come under a project which only the manager can create, inside a task we will have a list of time entry .The application should generate a .csv file about the time frame took for each task in respective projects from the view page, the user can view tasks as daily, weekly and yearly ,then when he presses export, the respective sorted view we showed them must be exported. 

Lastly they need a time adjust feature, If an employee forgot to start the timer he must be able to start and adjust the duration alone (the start and stop timestamp) do not change but instead we mark the edited duration as “User Overridden from Actual duration  to Edited duration”. 

 

There are 4 predefined tasks Design, Development, Testing, Release and Process Activities ,apart from these we must also provide a function to the employees to create a custom task (eg : Task 1). 

 

Add CRUD operations to manage the TimeEntry in a Task. 


## Implementation/Design

## Design Explanation

## Alternative Design