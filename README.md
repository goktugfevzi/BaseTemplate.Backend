# BaseTemplate


## Description

This project provides a robust infrastructure for building professional websites. If you don’t need to scale for millions of users, this template will perfectly meet your requirements, helping you start your project quickly and efficiently.
<br/>
<br/>


## Features

To begin your project, simply create your model, set up its repository and service, generate the Data Transfer Objects (DTOs), and create the controller. The rest of the infrastructure is already in place. For reference, an example model is included.

### Key Features:
- **N-Tier Architecture**: Ensures a clean separation of concerns.
- **Repository and Service Registration**: Managed with AutoC.
- **AutoMapper & FluentValidation**: Automatically map models to DTOs and validate your data.
- **Global Error Handling**: Integrated exception handler for error management.
- **Audit Logging**: Logs user actions (create, update, delete), capturing user IDs, IP addresses, affected tables, and old/new values.
- **Logging with Serilog**: Logs are stored in Elasticsearch.
- **Database Setup**: SQL Server is configured, but other databases can be added as needed.
- **CRUD Operations**: Complete CRUD functionality, including pagination and both soft and hard deletes.
- **Authentication**: Includes password hashing, JWT tokens, claims-based authentication.
- **Caching**: In-Memory caching for improved performance.
- **Docker-Compose Support**: Ready-to-use Docker Compose file for easy deployment.
<br/>
<br/>

## Technologies Used
- .NET Core 8
- AutoMapper
- FluentValidation
- Serilog
- Elasticsearch
- Docker
- SQL Server
<br/>
<br/>

## Installation

To get started with this project, clone the repository to your local machine:</br>
```sh
git clone https://github.com/goktugfevzi/BaseTemplate.Backend.git
```

<br/>

## Usage

1. Open the solution file in Visual Studio.
2. Build the solution to restore all dependencies.
3. Run the project using Docker Compose for smooth deployment.
<br/>
<br/>
<br/>

#### License
Feel free to customize the project to fit your needs. Contributions and suggestions are always welcome!

#### Acknowledgements
Thanks to my mom.


