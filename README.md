# customer-crud-challenge

A coding challenge project using Angular 14 and .NET 6 to create a simple CRUD interface for customer data management, featuring session-based selection highlighting and initial data seeding.

## Overview

This project comprises two main applications: a User Interface (UI) and an Application Programming Interface (API).

### UI Application

- **Framework**: Implemented using [Angular 17](https://angular.io/).
- **Design**: Utilizes [Bootstrap](https://getbootstrap.com/) and [Angular Material](https://material.angular.io/) for styling and component design.
- **Communication**: Employs `BehaviorSubject` for reactive communication between components.
- **Dockerization**: Containerized with a Dockerfile for easy setup and deployment.


### API Application

- **Technology**: Developed in [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
- **Architecture**: Follows the Repository pattern for data access.
- **Database**: Configured with an in-memory database by default, but can be switched to any database provider supported by Entity Framework.
- **ORM**: Uses Entity Framework with a Code-First approach.
- **Dockerization**: Includes a Dockerfile for deploying the API in a containerized environment.


### Testing

Comprehensive unit tests are implemented across all layers, serving as a reference. Further tests are required for full coverage.

### Key Technologies

- **Fluent Validation**: Used for validating models.
- **AutoMapper**: Facilitates object-object mapping.
- **Serilog**: Provides logging capabilities.
- **Global Exception Handler**: Ensures centralized exception management.

### Prerequisites

- [Docker](https://www.docker.com/) installed on your machine.

## Planned Enhancement: Third-Party Authentication Integration
This project is planned to be improved by integrating a third-party authentication provider. This will enhance the security and usability of the application by allowing users to sign in using popular identity providers.


### Integrating Authentication in Angular App
To integrate third-party authentication in the Angular app, the following steps are required:

1. **Choose an Authentication Provider**: Select a provider like Keycloack, or Okta.
2. **Register the Application**: Create an application in the provider's dashboard to obtain necessary credentials (Client ID, Secret, etc.).
3. **Install SDK/Library**: Depending on the provider, install the necessary SDK or library in the Angular project.
4. **Configure Authentication Service**: Implement an authentication service in Angular using the installed SDK.
5. **Secure Routes**: Use Angular route guards to protect certain routes based on the user's authentication status.


### Integrating Authentication in the API
The API needs to be configured to handle authentication tokens and protect routes:

1. **Configure the Provider**: Set up the chosen provider in the API for validating tokens.
2. **Protect API Routes**: Implement middleware or filters to protect certain API endpoints, requiring a valid authentication token.
3. **User and Role Management**: Optionally, implement user and role management based on the tokens' claims.


## Further Planned Enhancement: UI Loader for Customer Creation

An additional enhancement for this project is the implementation of a UI loader. This feature aims to improve the user experience by displaying a loading indicator when a new customer is being created. This visual feedback is crucial for operations that require waiting for server response, ensuring users are aware the application is processing their request.

### Steps to Implement the UI Loader in Angular:

1. **Choose a Loading Indicator Library**: Decide whether to use a built-in Angular Material component, a Bootstrap spinner, or a third-party library for the loading indicator.
2. **Install Necessary Packages**: If using a third-party library, install it via npm.
3. **Implement Loading Logic**: Integrate the loading indicator in the customer creation component. Show the loader when the creation process starts and hide it upon completion or error.
4. **Test the Loader**: Ensure the loader appears and behaves correctly across different devices and browsers.
