# Food Collections Backend

A scalable RESTful API for **Food Collections**, a mobile application that helps users save and organize their favorite restaurants and dishes into personalized collections.

Built with **ASP.NET Core 10**, **Clean Architecture**, **CQRS**, **PostgreSQL**, and **Docker**.

---

## Features

### Authentication
- User Registration
- User Login
- JWT Authentication
- Refresh Token
- Role-based Authorization (Future)

### Collections
- Create Collection
- Update Collection
- Delete Collection
- View User Collections

### Food Entries
- Save Favorite Foods
- Upload Food Images
- Restaurant Name
- Dish Name
- Address
- Price Range
- Rating
- Personal Notes
- Visit Date

### Tags
- Create Custom Tags
- Assign Multiple Tags
- Filter by Tags

### Search
- Search by Restaurant
- Search by Dish
- Search by Collection
- Search by Tags

### Future Features
- AI Restaurant Recommendation
- OCR from Food Photos
- Google Maps Integration
- Push Notifications
- Food Statistics Dashboard

---

# Architecture

The project follows **Clean Architecture**.

```
Presentation (API)
        │
        ▼
Application
        │
        ▼
Domain
        ▲
        │
Infrastructure
```

Main principles

- Clean Architecture
- CQRS
- MediatR
- Repository Pattern
- Dependency Injection
- SOLID Principles

---

# Tech Stack

| Technology | Description |
|------------|-------------|
| ASP.NET Core 10 | REST API |
| Entity Framework Core | ORM |
| PostgreSQL | Database |
| Redis | Caching |
| Docker | Containerization |
| MediatR | CQRS |
| FluentValidation | Request Validation |
| AutoMapper | Object Mapping |
| Serilog | Logging |
| Scalar | API Documentation |
| JWT | Authentication |

---

# Project Structure

```
FoodCollectionsBackend

src
│
├── FoodCollectionsBackend.API
│
├── FoodCollectionsBackend.Application
│
├── FoodCollectionsBackend.Domain
│
└── FoodCollectionsBackend.Infrastructure

tests
│
├── FoodCollectionsBackend.UnitTests
│
└── FoodCollectionsBackend.IntegrationTests
```

---

# Getting Started

## Clone repository

```bash
git clone https://github.com/yourusername/FoodCollectionsBackend.git
```

---

## Restore packages

```bash
dotnet restore
```

---

## Update database

```bash
dotnet ef database update \
--project src/FoodCollectionsBackend.Infrastructure \
--startup-project src/FoodCollectionsBackend.API
```

---

## Run API

```bash
dotnet run \
--project src/FoodCollectionsBackend.API
```

---

# Docker

Run PostgreSQL

```bash
docker compose up -d
```

---

# API Documentation

Development

```
https://localhost:5001/scalar
```

OpenAPI JSON

```
https://localhost:5001/openapi/v1.json
```

---

# Environment Variables

Example

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },

  "JwtSettings": {
    "Issuer": "",
    "Audience": "",
    "SecretKey": ""
  }
}
```

---

# Testing

Run all tests

```bash
dotnet test
```

---

# 📱 Mobile Application

Flutter Repository

https://github.com/yourusername/FoodCollections

---

# Roadmap

## Version 1

- Authentication
- Collections
- Food CRUD
- Image Upload
- Search

## Version 2

- AI Recommendation
- Google Maps
- OCR
- Food Statistics

## Version 3

- Social Features
- Share Collections
- Public Collections
- Friends

---

# License

MIT License