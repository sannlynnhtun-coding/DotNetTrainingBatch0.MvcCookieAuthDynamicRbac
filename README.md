# MVC Cookie Auth Dynamic RBAC

## Project Overview
This project demonstrates an **ASP.NET Core MVC** application with **Cookie Authentication** and **Dynamic Role-Based Access Control (RBAC)**. It combines the session management of cookies with a flexible, database-driven permission system. Permissions are mapped to roles in the database, allowing for real-time administrative control over user access.

## Step-by-Step Flow

### 1. Database Configuration
- The system uses a normalized database schema: `Users -> Roles -> RolePermissions -> Permissions`.
- Roles (e.g., "Admin") are assigned to users, and specific permissions (e.g., "Product.Update") are assigned to roles.

### 2. Login & Dynamic Claim Construction
- When a user logs in, the server fetches the user's role and all permissions currently linked to that role from the database.
- A `ClaimsPrincipal` is constructed where each permission is added as a claim.

### 3. Encrypted Cookie Storage
- The `ClaimsPrincipal` (containing the dynamic permissions) is encrypted and stored in an authentication cookie in the user's browser.

### 4. Continuous Authentication
- On every page load, the server decrypts the cookie to identify the user and their granted permissions without needing to hit the database for every single request (until the cookie expires or the user re-logs).

### 5. Multi-level Authorization
- **Controller/Action Level**: Uses `[Authorize]` with policies that verify permission claims.
- **View Level**: Conditionally renders HTML elements based on whether the `User` principal contains the required permission claims.

## Comparison with Static RBAC
- **Static**: Changing a user's permissions requires editing the user record or code.
- **Dynamic**: Changing a role's permissions in the `TblRolePermissions` table automatically updates access for all users in that role upon their next login.
