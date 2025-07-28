# Sanayii Platform Backend (ASP.NET Core Web API)

🎯 **Sanayii Platform Backend**  
A RESTful Web API powering the Sanayii Platform — connecting clients with certified artisans. Handles user management, service requests, contracts, reviews, and notifications.

---

## 📌 API Overview

This backend provides endpoints for client and artisan interactions:

- ✅ Register/login (clients & artisans)
- 📩 Submit and track service requests
- 🧾 Manage contracts and approvals
- 🗣️ Post and retrieve service reviews
- 📊 Filtering, sorting, and pagination support

Built with **ASP.NET Core Web API**, layered using Clean Architecture.

---

## 🧰 Tech Stack

- ASP.NET Core Web API (.NET 7 / .NET 8)
- Entity Framework Core (SQL Server)
- ASP.NET Identity (JWT + Refresh Tokens)
- Automapper
- SignalR for real-time notifications
- Swagger (OpenAPI) documentation
- Logging with Serilog / built‑in
- stripe Payment
- سيي
- سفقهح
---

## 🚀 Getting Started

### ✅ Prerequisites

- .NET SDK 7 or 8  
- SQL Server / LocalDB  
- Visual Studio 2022+ or VS Code  

---

### 📦 1. Clone the Repository

```bash
git clone https://github.com/AyaaMohammed/Sanayii_Platform_Backend.git
cd Sanayii_Platform_Backend
```

---

### 🔧 2. Configure Connection & Settings

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=SanayiiPlatformDB;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "Sanayii",
    "Audience": "SanayiiClient",
    "ExpiresInMinutes": 60,
    "RefreshTokenValidityDays": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

### 🧱 3. Apply EF Core Migrations

```bash
dotnet ef database update
```

---

### ▶️ 4. Run the API

```bash
dotnet run
```

Access Swagger Docs at: `https://localhost:5001/swagger`

---

## 🧩 Features & Endpoints

- **CRUD** operations for users, service requests, reviews, and artisan profiles  
- **Advanced search & filter**: by category, rating, status, date, etc.  
- **JWT + Refresh Token** authentication  
- **AI-powered review moderation** using Parity AI  
- **Real-time notifications** via SignalR (e.g. new requests, approvals)  
- **Role-based access**: Admin, Client, Artisan  
- **Pagination for large result sets**


---

## 📄 License

This backend is licensed under the **MIT License**.

---

## 👩‍💻 Author

**Aya Nafed**

---

## 📬 Contact

For questions or contributions, please open an issue on GitHub or connect via LinkedIn.
 
