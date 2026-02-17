# Pay Bill Application

This is a WPF application for managing employee payments and schedules, built with .NET 8.0 and Entity Framework Core for MySQL.

## Features

### Login System
- **Secure Access**: Users must log in to access the main application.
- **Default Admin**: The system comes pre-configured with a default administrator account.
    - **Username**: `Admin`
    - **Password**: `123456`

### Employee Management
- **Add Employee**: Easily add new employees to the database with a dedicated form.
    - Accessible via the "Add Employee" button on the main dashboard.
    - Captures details such as Calling Name, Account Name, Employee Number, Bank, Branch, NIC No, and Account Number.

## Setup Instructions

### 1. Database Initialization
Before running the application, you must set up the MySQL database.

1.  Open your MySQL client (e.g., MySQL Workbench).
2.  Open the SQL script located at `WpfApp1/Data/pay_bill.sql`.
3.  Execute the script to create the `pay_bill` schema, tables (`employees`, `schedules`, `payments`, `paysheets`, `logins`), and the default admin user.

### 2. Configure Connection String
You need to update the application with your local database credentials.

1.  Open the file `WpfApp1/appsettings.json`.
2.  Locate the `ConnectionStrings` section:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "server=localhost;database=pay_bill;user=root;password=YOUR_PASSWORD"
    }
    ```
3.  Replace `YOUR_PASSWORD` with your actual MySQL root password.
4.  If your database username is different from `root` or the server is not `localhost`, update those values as well.

### 3. Build and Run
1.  Open the solution `fuel.sln` in Visual Studio.
2.  Build the solution (Ctrl+Shift+B).
3.  Run the application (F5).
4.  Log in using the default credentials (`Admin` / `123456`).
