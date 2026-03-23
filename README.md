# VendorShield

Vendor Performance & Risk Management portal built with **Blazor Server**, **ASP.NET Core Identity**, and **EF Core**.

## What it does
- Manage vendors (CRUD)
- Manage purchase orders, deliveries, and incidents
- Calculate performance & risk scores and show a dashboard
- Provide role-based access (Admin / Buyer / Viewer)

## Tech Stack
- Blazor Server (.NET 10 targeting the current project)
- ASP.NET Core Identity
- EF Core (SQL Server; LocalDB for development)

## Setup / Run
1. Ensure SQL Server LocalDB is available.
2. Update connection string if needed in `VendorShield/appsettings.json`:
   - `ConnectionStrings:VendorShieldContext`
3. Apply migrations:
   - `dotnet ef database update --project VendorShield`
4. Run the app:
   - `dotnet run --project VendorShield`

## Seeded Admin / Roles (development)
On startup, the app seeds the required roles (`Admin`, `Buyer`, `Viewer`).
If you want a default admin login, configure/admin-create it for your environment (see `Program.cs` seeding).

## Notes
- Do not commit secrets; keep sensitive values in `appsettings.*` via environment overrides or user secrets.