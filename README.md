# VendorShield

Vendor Performance & Risk Management portal built with **Blazor Server**, **ASP.NET Core Identity**, and **EF Core**.

## What it does
- Manage vendors (CRUD)
- Search + filter vendors list (name, category, region, status)
- Manage purchase orders, deliveries, and incidents
- Calculate performance & risk scores and show a dashboard
- Provide role-based access (Admin / Buyer / Viewer)
- Route users to `/pending-access` when they are logged in but have no role assigned
- Admin can assign roles via `/admin/users`

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

## Seeded Roles + Admin Onboarding
On startup, the app seeds the required roles (`Admin`, `Buyer`, `Viewer`).

The app does not auto-create an admin user. You must ensure that at least one user has the `Admin` role (for example by creating a user and assigning the `Admin` role in SQL Server / SSMS one time).

After you have an `Admin`, you can manage and assign roles for all registered users in the Admin-only page: `/admin/users`.
New users who register will initially have no role and will be redirected to `/pending-access` until an Admin assigns them a role.

## Notes
- Do not commit secrets; keep sensitive values in `appsettings.*` via environment overrides or user secrets.