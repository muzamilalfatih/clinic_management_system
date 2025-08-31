# Driving & Vehicle License Department (DVLD) System

The **Clinic Management System** is a 3-tier application designed to handle clinical operations—patients, appointments, treatments, billing, and reports. It’s built with a clean **Presentation → Business Logic → Data Access** architecture for modularity, maintainability, and scalability.  

the **presentation layer here is an API application** that provides endpoints for integration with clients (web, desktop, or mobile).
## Architecture Overview

The application follows a Three-Tier Architecture, comprising:

1. Presentation Layer – Exposes RESTful endpoints for interaction   
2. Business Logic Layer – Handles the core functionality and rules  
3. Data Access Layer – Manages database interactions                                

## Technologies Used

- Language: C#  
- Framework: .NET Core , ADO.net, ASP.net
- Architecture: Three-Tier  
- Database: (SQL Server)

## Getting Started

### Prerequisites

- Visual Studio 2022 or later  
- .NET Framework (4.8)  
- SQL Server installed and running
- Access to SQL Server Management Studio (SSMS) or any SQL query tool

### Installation

1. Clone the repository  
   https://github.com/muzamilalfatih/clinic_management_system.git
2. Steps to Set Up the Database

    1. Open the schema.sql and run it.
    2. Open the seed.sql and run it.

3. Open the solution  
   Open the .sln file in Visual Studio.

4. Restore NuGet packages  
   Go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution and restore missing packages.
5. Build and run  
   Use Build > Build Solution and Debug > Start Debugging to launch the application.
6. Use the following test credentials :

    1. Username : admin.
    2. Password : admin.

## Features

- Patient Management – Add, update, and search patients
- User management: register and manage users  
- Appointments – Create, reschedule, cancel appointments 
- Treatments & Records – Store diagnoses, prescriptions, procedures
- Billing & Payments – Manage invoices and receipts
- Generate operational summaries and statistics
- Can be consumed by web, desktop, or mobile clients

## Contributing

Contributions are welcome!

1. Fork the repo  
2. Create a branch: git checkout -b feature/YourFeature  
3. Commit your changes: git commit -m "Add feature"  
4. Push the branch: git push origin feature/YourFeature  
5. Open a Pull Request

## Contact

If you have any questions or feedback, feel free to reach out:

- Email: muzamilalfatih123@gmail.com 
- GitHub: https://github.com/muzamilalfatih
