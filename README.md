# IzzyShop - Vulnerable Web Store Application

This is a deliberately vulnerable web store application designed for security testing and demonstration purposes. It contains various security vulnerabilities that should be identified and fixed as part of security training or testing exercises.

## ⚠️ DISCLAIMER

This application is intentionally vulnerable and should **NEVER** be deployed in a production environment. It is designed solely for educational and testing purposes.

## Application Overview

IzzyShop is a web-based store application built with:
- ASP.NET Core 7.0
- Entity Framework Core
- SQL Server
- React frontend
- RESTful API

## Known Vulnerabilities

The application contains several intentional security vulnerabilities, including but not limited to:

1. SQL Injection vulnerabilities
2. Cross-Site Scripting (XSS) - both stored and reflected
3. XML External Entity (XXE) injection
4. Insecure Direct Object References (IDOR)
5. Cross-Site Request Forgery (CSRF)
6. Insecure Deserialization
7. Broken Authentication
8. Sensitive Data Exposure
9. Missing Access Controls
10. Insecure File Upload
11. Command Injection
12. Server-Side Template Injection
13. Insecure Cryptographic Storage
14. Race Conditions
15. Insecure Direct Object References

## Getting Started

### Prerequisites
- .NET 7.0 SDK
- SQL Server
- Node.js 16+
- Visual Studio 2022 or VS Code

### Setup
1. Clone the repository
2. Update the connection string in `appsettings.json`
3. Run database migrations:
   ```
   dotnet ef database update
   ```
4. Start the backend:
   ```
   dotnet run
   ```
5. In a separate terminal, start the frontend:
   ```
   cd ClientApp
   npm install
   npm start
   ```

## Project Structure

```
IzzyShop/
├── Controllers/         # API Controllers
├── Models/             # Data Models
├── Services/           # Business Logic
├── Data/              # Database Context
├── ClientApp/         # React Frontend
└── wwwroot/           # Static Files
```

## Security Testing

This application is designed to be used with various security testing tools and techniques. Some suggested approaches:

1. Manual penetration testing
2. Automated vulnerability scanning
3. Code review
4. API security testing
5. Authentication testing

## Contributing

Feel free to add more vulnerabilities or improve existing ones for educational purposes.

## License

This project is licensed under the MIT License - see the LICENSE file for details.