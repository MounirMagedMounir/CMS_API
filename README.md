# CMS_API  
## _A Modern and Flexible Content Management System API_

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](https://github.com/MounirMagedMounir/CMS_API)  
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)  

The **CMS_API** is a scalable and feature-rich API built using **.NET 8** and **Clean Architecture principles**. It supports JWT Authentication and User , Role , Permission ,article management, contributor roles, hierarchical comments .Perfect for modern publishing platforms and collaborative workflows.
 

---

## Features  

- **User Management:** Manage User with advanced roles.  
- **Role Management:** CRUD Operation to Manage the Role.
- **Permisssion Management:** CRUD Operation to Manage the Permission.
- **Content Management:** Manage articles with advanced contributor roles.  
- **Contributor Roles:** Multiple predefined roles such as Writer, Editor, and Publisher.
- **Clean Architecture:** Follows SOLID principles for scalability and maintainability.  
- **Authentication:** JWT-based secure session handling.  
- **Role-Based Access Control (RBAC):** Fine-grained permissions based on roles and Multiple predefined roles such as Super Admin, Admin, and User.

---

## Technologies Used  

| Technology          | Purpose                                |
|----------------------|----------------------------------------|
| **.NET 8 (C#)**      | Framework                             |
| **SQL Server**       | Database                              |
| **Entity Framework** | ORM                                   |
| **JWT Authentication** | Secure Authentication               |
| **Clean Architecture** | Scalable Design Pattern             |
| **Agile Methodology** | Iterative Development Practices      |

---

## Getting Started  

### Prerequisites  

- **.NET 8 SDK** installed.  
- **SQL Server** database.  
- Development environment like **Visual Studio** or **VS Code**.

### Installation  

1. Clone the repository:
    ```bash
    git clone https://github.com/MounirMagedMounir/CMS_API.git
    cd cms-api
    ```

2. Install dependencies:
    ```bash
    dotnet restore
    ```

3. Configure the database connection in `appsettings.json`:
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=your-server;Database=CMSAPI;User Id=your-username;Password=your-password;"
    }
    ```

4. Apply database migrations:
    ```bash
    dotnet ef database update
    ```

5. Run the application:
    ```bash
    dotnet run
    ```

---

## Modules  

### User Authentication  
- **POST** `/api/UserAuthentication/register` - User register and Verification code generation and send to Email
- **POST** `/api/UserAuthentication/EmailVerification` - verify the Verification code
- **POST** `/api/UserAuthentication/login` - User login and JWT generation
- **POST** `/api/UserAuthentication/SignOut` - User Sign out 
- **POST** `/api/UserAuthentication/RefreshToken` - Refresh JWT tokens  
- **POST** `/api/UserAuthentication/ForgetPassword` - Forget Password 

  
### Admin Authentication  
- **POST** `/api/AdminAuthentication/login` - Admin login and JWT generation
- **POST** `/api/AdminAuthentication/SignOut` - Admin Sign out 
- **POST** `/api/AdminAuthentication/RefreshToken` - Refresh JWT tokens
- 
### Roles and Permissions  
- **POST** `/api/Role/GetList?skip=1&take=3&sortBy=name&sortOrder=ase` - Retrieve all roles  
- **POST** `/api/Role/Create` - Add a new role  

### Articles  
- **POST** `/api/Article/GetList?skip=1&take=2&sortBy=name&sortOrder=ace` - Retrieve all articles  
- **POST** `/api/Article/Create` - Create a new article  
- **PUT** `/api//Article/Update` - Update an article  
- **DELETE** `/api/Article/DeleteById?ArticleId=Article_Id` - Delete an article  

### Comments  
- **GET** `/api/Comment/GetByArticleId?articleId=Article_Id` - Retrieve comments for an article  
- **POST** `/api/Comment/Add` - Add a comment  
- **DELETE** `/api/Comment/Delete?Id=Comment_Id` - Delete a comment (handles cascading deletion)  

---

### API Documentation
Interactive API documentation is available via Swagger at /swagger and PostMan Documentation. Explore all endpoints, request/response formats, and error codes in detail.
(For detailed documentation, refer to the API [Documentation](https://documenter.getpostman.com/view/25105164/2sAYBYeVcb).)

---

## Role-Based Permissions  

The API supports fine-grained role-based access control (RBAC). Predefined roles include:  
- **Super Admin:** Full access to all features.  
- **Admin:** Manage articles and contributors.  
- **User:** Limited access to view articles.
- 
Access control is enforced through policies validated at runtime.
Permissions are validated using JWT tokens with role-specific claims.

---

## Hierarchy Management  

- **Parent-Child Relationship:** Nested comments for articles.  
- **Cascading Deletion:** Automatically removes child comments when a parent is deleted.  
- **Entity Design:** The `Comment` entity includes `ParentId` and `ArticleId` for relationship management.  

---

## Contributing  

We welcome contributions to this project!  

1. Fork the repository:
    ```bash
    git clone https://github.com/your-username/CMS_API.git
    ```

2. Create a feature branch:
    ```bash
    git checkout -b feature-name
    ```

3. Commit your changes:
    ```bash
    git commit -m "Add feature-name"
    ```

4. Push to the branch:
    ```bash
    git push origin feature-name
    ```

5. Open a pull request.

---

## License  

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
