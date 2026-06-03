# 🕹️ Retro Nexus

**Retro Nexus** is a fully-featured, responsive e-commerce web application designed for retro gaming enthusiasts. Built with ASP.NET Core MVC and Entity Framework Core, the platform provides distinct experiences for Administrators, Staff members, and Customers.

## ✨ Features

### 🛒 Customer Experience
* **Shopping Cart & Checkout:** Add games to your cart, modify quantities, and complete orders through a streamlined checkout process.
* **Order History:** View all past orders, total money spent, and tracking statuses directly from the customer profile.
* **Self-Registration:** Easily register as a new customer via the unified login page.

### 🛡️ Admin & Staff Operations
* **Role-Based Portals:** Dedicated secure portals and color-coded themes for Admin (Red) and Staff (Gold).
* **Metrics Dashboard:** Instantly view key performance indicators like total sales, revenue, low stock count, and recent orders.
* **Inventory Management:** Full CRUD (Create, Read, Update, Delete) operations for the game catalog, plus a dedicated "Low Stock" view to manage inventory efficiently.
* **Staff Management:** Administrators can easily register and remove staff members through the dedicated Staff management panel.
* **Customer Management:** Administrators can view and manage registered customers.

## 🛠️ Technology Stack

* **Backend:** C#, ASP.NET Core MVC 8.0 (or newer)
* **Database:** SQLite with Entity Framework Core
* **Frontend:** HTML5, CSS3 (Custom theming), JavaScript, Bootstrap 5
* **Architecture:** MVC (Model-View-Controller)

## 🚀 Getting Started

### Prerequisites
* [.NET SDK](https://dotnet.microsoft.com/download)
* A modern web browser
* An IDE such as Visual Studio, Visual Studio Code, or JetBrains Rider.

### Installation & Setup

1. **Clone the repository** (if applicable) or download the source code.
2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```
3. **Apply Database Migrations:**
   The project uses Entity Framework Core with a local SQLite database. To set up the database and apply the initial seed data:
   ```bash
   dotnet ef database update
   ```
4. **Run the Application:**
   ```bash
   dotnet run
   ```
5. Open your browser and navigate to the local URL (typically `https://localhost:5001` or `http://localhost:5000`).

## 🔑 Demo Credentials

The database is pre-seeded with the following accounts for testing purposes:

**Admin Account**
* **Email:** `admin@rgsdemo.com`
* **Password:** `Admin@123`

**Staff Account**
* **Email:** `staff@rgsdemo.com`
* **Password:** `Staff@123`

*(Customers can be manually registered using the Customer Registration portal).*

## 📄 License
This project is built for educational purposes.
