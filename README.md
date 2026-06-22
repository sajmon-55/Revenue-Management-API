# Revenue Management System

A robust REST API application designed to manage the complex process of **Revenue Recognition** for a corporate software provider. 

## Purpose
In enterprise environments, receiving payment and recognizing revenue are two different concepts. A simple transaction (like buying coffee) is recognized immediately, but long-term software licenses and contracts require strict business rules. 

This system was built to solve this problem by clearly separating **current revenue** (actual money received and recognized from fully paid contracts) from **predicted revenue** (contracts waiting for payment). It provides a full suite of endpoints to manage clients, software products, discounts, contracts, and payments, ensuring that financial data remains consistent and compliant with internal business logic.

## Features
* **Client Management:** Supports both Individual Clients (using PESEL) and Company Clients (using KRS). Includes role-based access control where only Administrators can edit or softly delete clients.
* **Software & Contracts:** Allows creating time-bound software contracts (3 to 30 days) with optional years of extended support.
* **Smart Discount Engine:** Automatically applies the highest available promotional discount for a specific timeframe and adds an extra 5% discount for returning customers.
* **Payment Processing:** Handles partial and full payments. Automatically cancels contracts if payment is not completed before the deadline.
* **Revenue Calculation:** Calculates both actual and predicted revenue on a company-wide or per-product basis.
* **Live Currency Conversion:** Integrates with the National Bank of Poland (NBP) public API to instantly convert revenue from PLN to other currencies (e.g., USD, EUR).
* **JWT Security:** Fully secured API using JSON Web Tokens with role-based authorization.

## Tech Stack
* **Language:** C# 12
* **Framework:** .NET 10 / ASP.NET Core Web API
* **Database / ORM:** Entity Framework Core (Code-First approach)
* **Testing:** xUnit, EF Core In-Memory Database
* **Security:** JWT (JSON Web Tokens) Authentication
* **Documentation:** Swagger / OpenAPI

## Architecture & Solutions Used
* **Clean Architecture / N-Tier:** The project is logically divided into API, Application, Domain, and Infrastructure layers, ensuring the separation of concerns and maintainability.
* **Dependency Injection (DI):** Heavily utilized for loosely coupled code and easier unit testing.
* **Global Exception Handling:** Uses modern `.AddProblemDetails()` and custom middleware to catch domain exceptions (e.g., `NotFoundException`, `ConflictException`) and return standardized, clean HTTP error responses (404, 409, 400).
* **Soft Delete Pattern:** Individual clients are never physically removed from the database to preserve historical contract data. Instead, their personal data is anonymized and marked with an `IsDeleted` flag.
* **External API Integration:** Uses `HttpClient` factory to communicate with external public web services (NBP API) for real-time exchange rates.
* **Unit Testing:** Business logic (like complex discount calculations in `ContractService`) is covered by automated unit tests using xUnit and an In-Memory database to simulate real-world scenarios without touching a real SQL server.

## How to run locally
1. Clone the repository.
2. Open the solution in JetBrains Rider or Visual Studio.
3. Build the project to restore NuGet packages.
4. Run the `API` project. The database will be seeded automatically on startup (including a default Admin account).
5. Use the provided `.http` files or the Swagger UI to interact with the endpoints.
