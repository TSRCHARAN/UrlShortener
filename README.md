
# 🔗 URL Shortener (.NET 8 MVC)

A simple yet effective URL shortener built using **ASP.NET Core MVC**. Users can shorten long URLs and retrieve them later through a web UI, and an API endpoint allows redirection from a short URL to the original.

---

## ✨ Features

- 🔗 Shorten long URLs via UI (`/CreateShortUrl`)
- 📥 Retrieve original URLs via UI (`/GetUrl`)
- 🚀 API for redirecting to the original URL (`/api/{shortUrl}`)
- 🛡️ Clean MVC architecture
- 💾 Pluggable service layer using dependency injection

---

## 🌐 Routes

### Web Interface

- **Create a Short URL:**  
  `https://localhost:{PORT}/CreateShortUrl`

- **Retrieve Original URL:**  
  `https://localhost:{PORT}/GetUrl`

### API Redirect Endpoint

- **GET** `/api/{shortUrl}`  
  Redirects to the original long URL.

  **Example:**  
  `GET https://localhost:{PORT}/api/abc123`  
  → Redirects to `https://example.com/long-page`

> **Note:** `{PORT}` will vary based on your local machine. Check the console output or `launchSettings.json` to find the exact port the app is running on.

---

## 🛠️ How to Run

```bash
git clone https://github.com/TSRCHARAN/UrlShortener.git
cd UrlShortener
dotnet restore
dotnet run
```

- App will be available at something like:  
  `https://localhost:{PORT}`

---

## ⚙️ Configuration

This project uses an `appsettings.Template.json` file to define the default structure for your configuration, including the **database connection string**.

### Setup Steps:

1. **Create your actual appsettings file** by copying:
   ```bash
   cp appsettings.Template.json appsettings.Development.json
   ```

2. **Edit the connection string and other values** in `appsettings.Development.json` as per your local setup:
   ```json
   {
     "ConnectionStrings": {
      "MVCConnection": "DB_Connection"
    },
    "Redis": {
      "ConnectionString": "Redis_Connection"
    },
     "Logging": { ... },
     "AllowedHosts": "*"
   }
   ```

3. Make sure `appsettings.Development.json` is **NOT** committed to Git (it is ignored via `.gitignore`).

---

## 🗃️ Database Schema

You have two options to set up the database:

### Option 1: Using Entity Framework Core

If the project includes EF Core migrations, just run:

```bash
dotnet ef database update
```

> Ensure EF Core CLI tools are installed:  
> `dotnet tool install --global dotnet-ef`

### Option 2: Manual SQL Script

You can use the provided `url_db.sql` file inside UrlShortener folder to manually set up your database tables.

---

## 💡 Usage Flow

1. Go to `/CreateShortUrl` to generate a short URL.
2. Copy the generated short URL.
3. You or anyone else can access `/api/{shortUrl}` to get redirected to the original site.
4. Optionally, use `/GetUrl` to manually retrieve the long URL.

---

## 🙋 Author

Made with ❤️ by [TSR Charan](https://github.com/TSRCHARAN)

---