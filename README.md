# SolarWatch

## Project Overview
**SolarWatch** is a web application that provides sunrise and sunset data for cities. It allows users to access and manage sunrise and sunset information for specific locations, making it a valuable tool for various applications like planning outdoor activities, optimizing energy usage, or simply keeping track of daylight hours.

## Table of Contents
- [Getting Started](#getting-started)
- [Features](#features)
- [API Endpoints](#api-endpoints)
- [Authorization](#authorization)
- [Project Structure](#project-structure)

## Getting Started
To get started with SolarWatch, follow these steps:

1. Clone this repository to your local machine.
2. Set up your development environment with the required dependencies and configurations.
3. Build and run the project.

## Features
- **Sunrise and Sunset Data:** SolarWatch provides access to sunrise and sunset times for cities around the world.
- **City Management:** Users with admin privileges can manage city data, update existing cities, and add new ones.
- **Authorization:** Secure access to the application with role-based authorization.

## API Endpoints
SolarWatch exposes the following API endpoints:

### Get Sunrise and Sunset Data
- **GET /SolarController/GetByName**: Retrieve sunrise and sunset data for a specific city and date. Requires authorization.

### Update City Data
- **PUT /SolarController/UpdateCityData**: Update city data for an existing city. Requires admin authorization.

### Delete City Data
- **DELETE /SolarController/DeleteByName**: Delete city data for an existing city. Requires admin authorization.

## Authorization
SolarWatch implements role-based authorization:
- **Admin:** Has full access to manage cities and perform administrative tasks.
- **User:** Can retrieve sunrise and sunset data for cities.

## Project Structure
The project structure follows best practices and is organized as follows:
- **Controllers:** Contains the API controllers for handling requests.
- **DTOs:** Data Transfer Objects used in API requests and responses.
- **Models:** Defines data models, including city and sunrise/sunset data models.
- **Services:** Houses services for data retrieval and processing.
- **Repositories:** Provides data access and persistence.

Feel free to explore the codebase and make contributions to improve SolarWatch!

