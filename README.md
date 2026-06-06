# MotoVerse 🏍️

MotoVerse is a modern motorcycle e-commerce platform built with ASP.NET Core Web API and Angular. The project follows Clean Architecture principles and includes AI-powered features for review analysis and product recommendations.

---

## Features

### Authentication & Authorization

* JWT Authentication
* Refresh Token Support
* Role-Based Authorization
* Secure API Endpoints

### Product Management

* Create Products
* Update Products
* Delete Products
* Product Categories
* Product Images
* Inventory Management
* Product Status Tracking

### Shopping Experience

* Browse Products
* Search Products
* Filter by Category
* Product Details Page
* Shopping Cart
* Product Ratings

### Review System

* Add Reviews
* Edit Reviews
* Delete Reviews
* Star Rating System
* Automatic Product Rating Calculation

### AI Features

* Sentiment Analysis
* Fake Review Detection
* AI Generated Customer Replies
* AI Review Summaries
* AI Product Recommendations

### Architecture

* Clean Architecture
* CQRS Pattern
* Repository Pattern
* MediatR
* AutoMapper
* FluentValidation
* Dependency Injection
* Localization

---

## Technology Stack

### Backend

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* MediatR
* AutoMapper
* FluentValidation

### Frontend

* Angular
* Angular Signals
* Reactive Forms
* Route Guards
* HTTP Interceptors

### AI Integration

* OpenAI / GitHub Models
* GPT-4o Mini

---

## Project Structure

```text
MotoVerse
│
├── API
├── Core
├── Infrastructure
├── Domain
│
└── Angular Client
```

---

## Key Features

### AI Review Analysis

Analyze customer reviews automatically and detect:

* Positive Reviews
* Negative Reviews
* Neutral Reviews
* Fake Reviews

### AI Product Recommendation

Suggest similar motorcycle products using AI-powered recommendation logic.

### AI Auto Reply

Generate automatic customer support responses based on review sentiment.

---

## Getting Started

### Backend

```bash
git clone <repository-url>

cd MotoVerse

dotnet restore

dotnet ef database update

dotnet run
```

### Frontend

```bash
cd ClientApp

npm install

ng serve
```

---

## Future Improvements

* AI Chat Assistant
* Wishlist System
* Online Payment Integration
* Order Management
* Email Notifications
* Advanced Analytics Dashboard

---

## Author

Abdullah Ali Elkholy

ASP.NET Core & Angular Developer
