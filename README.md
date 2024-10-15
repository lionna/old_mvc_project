# Auto Service Management System

This project is designed to streamline the operations of an auto service center by managing key workflows and processes.

## Project Overview

- **Monolithic to Microservices Transition**:  
  This project began as an old monolithic application, which I am in the process of refactoring into a microservices architecture for better scalability and maintainability.

- **Data Migration**:  
  The first step was migrating the application from an older version of .NET to the latest version. During this process, I experimented with various data migration tools. Ultimately, most of the core functionality had to be migrated manually, as the tools failed to fully support the transition.

- **Frontend Modernization**:  
  To enhance the user experience, I plan to replace the traditional Razor-based frontend with modern frontend frameworks, improving responsiveness and interactivity.

- **Cloud Integration**:  
  The final deployment will utilize **Azure Cloud**, leveraging its infrastructure to ensure scalability, reliability, and seamless service management.

![Main diagram](https://github.com/user-attachments/assets/fd18833c-ab45-4dff-a048-5487f730424a)

## Overview

The system is primarily focused on process management within an auto service environment. It provides functionality for client appointments, service history tracking, spare parts and materials management, as well as scheduling regular maintenance and repairs.

![Diagram](https://github.com/user-attachments/assets/f4209953-7999-4366-92f3-1bfff00019d0)

### Key Features:
- **For Auto Service Staff**:  
  Staff can create work orders, track their status, and use calendar integrations with reminders to help schedule tasks and meet deadlines.
  
- **For Accountants**:  
  A dedicated feature for accounting tasks such as tracking financial transactions, invoicing, managing expenses and income, and generating reports. These tools help in the efficient financial management of the auto service.

### Technologies Used:
- **MVC Framework**: ASP.NET Core 8
- **ORM**: Entity Framework Core 8
- **Tests**: xUnit
- **Frontend**: Razor + Ajax + JQuery

## Use Cases

The system supports various user roles with specific use case scenarios:

### Roles:
- System Administrator
- Accountant
- Manager
- Employee
- Client

### System Administrator Use Cases:
1. System login
2. System administration
3. User and role management
4. Employee management
5. Supplier management
6. Report and schedule generation

### Accountant Use Cases:
1. System login
2. Employee management
3. Supplier management
4. Report and schedule generation

### Manager Use Cases:
1. System login
2. Order management
3. View employee schedules
4. View order details and statuses

### Employee Use Cases:
1. System login
2. View employee schedules
3. View order details and statuses

### Client Use Cases:
1. System login
2. View order status

## System Entities

During the domain analysis, 29 entity tables were identified. These entities form the foundation for data storage and organization in the system. Below is a brief overview of the key entities:

| **Entity**         | **Description**                                                                 |
|--------------------|---------------------------------------------------------------------------------|
| **Role**           | Stores various roles that can participate in the system                         |
| **User**           | Key reference table containing data about system users                          |
| **Client**         | Stores information about auto service clients                                   |
| **Employee**       | Stores employee data, including personal and employment details                 |
| **ContractToEmployee** | Tracks employee contracts, start/end dates, and contract types               |
| **Department**     | Stores information about different departments                                 |
| **Car**            | Repository for vehicle data, crucial for managing cars in the system            |
| **CarBrand**       | Stores information about car brands                                             |
| **CarColor**       | Stores information about car colors                                             |
| **CarModification**| Stores details on car modifications                                             |
| **CarSeries**      | Stores information about car series                                             |
| **ExecutingService** | Tracks service execution details, including time, cost, and responsible employee |
| **Invoice**        | Tracks supplier invoice details, including batch numbers and delivery dates      |
| **OrderService**   | Tracks information about ordered services and their status                      |
| **Provider**       | Tracks supplier information                                                     |
| **RemarkToStateCar** | Tracks remarks on the car's condition during service orders                   |
| **Service**        | Stores details about different services, including cost and duration            |
| **Post**           | Stores employee position information                                            |
| **PreRecord**      | Tracks preliminary service appointments                                         |
| **PreRecordService** | Links preliminary appointments to booked services                             |
| **ServiceType**    | Stores information about different types of services                            |
| **ServiceType-PostEmployee** | Links service types with employee positions                           |
| **SparePartMaterial** | Tracks spare parts and materials used during services                         |
| **TypeOfPayment**  | Stores different payment types                                                  |
| **UsingCustom-SpartMat** | Tracks spare part usage in specific service orders                        |
| **UsingPartMaterial** | Tracks part usage and related costs in service orders                         |
| **AcceptanceCustom-Spart** | Tracks acceptance of spare parts                                        |
| **AcceptanceInvoice** | Tracks invoices and other details related to spare parts acceptance          |
| **AcceptanceDocument** | Tracks acceptance documents, including employee and client details          |

## Development Approach

For database design, the **Code First** approach in Entity Framework Core was chosen due to its flexibility and control over database schema changes. By defining models directly in C# code using LINQ, I can efficiently manage the database structure and its migrations. This method ensures smooth deployment and updates for the application.

### Key Benefits:
- Flexible database structure management.
- Seamless database migration using code.
- Optimized database interactions for improved performance.

### Entity Framework and ORM
To efficiently work with data, a base interface was created from which multiple classes inherit. This enables the use of generic services and repositories. The base interface contains common fields shared across all derived classes, reducing code duplication and maintaining consistency in data handling.

With the help of ORM tools like Entity Framework, I created mappings between the classes and database tables, allowing the system to handle data using object-oriented principles. This also enables automatic database creation and updates when necessary.

### Database Schema
Based on the created entities, a database was generated. This approach defines the database structure using programming code and automatically creates the corresponding database. A visual representation of the database schema is shown below:

![Database Schema](https://github.com/user-attachments/assets/85e79579-c8ff-4599-87c9-d35b47fc6018)

## Class Diagram

The diagram below represents the class structure and dependencies within the system:

![Class Schema](https://github.com/user-attachments/assets/e4634f50-59ad-4f59-bc91-8e0054574f97)

## Some pages:

![Manage car colors](https://github.com/user-attachments/assets/a9020865-09d0-4de4-8654-899abecb6725)

![Diagram1](https://github.com/user-attachments/assets/1ac75c2a-10fd-42be-93c9-5ca67923e8b3)

![Diagram2](https://github.com/user-attachments/assets/91816b52-0742-49c0-ac61-63d955b2e6d7)

![Car managent](https://github.com/user-attachments/assets/0e793a00-ff8f-4025-994c-86c2e05c514e)

![Car repair](https://github.com/user-attachments/assets/0fb1530c-b973-4096-a9aa-aa5f3d61f58d)

![Report 1](https://github.com/user-attachments/assets/445145cc-c28a-4b14-b200-d8e34425a81c)

![Report 2](https://github.com/user-attachments/assets/98890a3e-b51d-4b0e-8eae-06bb07ed035d)

![Schedulrer](https://github.com/user-attachments/assets/aeccf8de-718b-4ec4-9238-9f8d14344b02)

![Car Info](https://github.com/user-attachments/assets/a186c264-9281-42b6-b698-baa4a7d02aa8)

![Admin page](https://github.com/user-attachments/assets/75c2d414-c6ca-466b-9e42-50e53649762d)

![Accountant page](https://github.com/user-attachments/assets/f575d62a-855c-47ea-83fd-eb522aaf0e4e)

![Emloyee page](https://github.com/user-attachments/assets/f95d8431-cb0f-4b36-b059-67e7a4a4fffd)

![Client page](https://github.com/user-attachments/assets/9d8131ae-eab3-45e7-8cd0-6b1096583fe2)
