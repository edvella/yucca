# Yucca

## Overview
Yucca is a comprehensive tool for managing inventory, sales, clients, suppliers, and financial operations for small to medium-sized businesses.

## Accessing the WebAPI

The Yucca WebAPI provides endpoints to interact with the application programmatically. Swagger is used to document and test the API.

### How to Use Swagger

1. **Run the WebAPI**:
    Navigate to the `Yucca.WebAPI` project directory and start the WebAPI using the following command:
    ```bash
    dotnet run --project Yucca.WebAPI/Yucca.WebAPI.csproj
    ```

2. **Access Swagger UI**:
    Once the WebAPI is running, open your browser and navigate to https://localhost:5294/swagger
    This will open the Swagger UI, where you can explore and test the available API endpoints.

3. **Testing Endpoints**:
    Use the Swagger UI to send requests to the API.
    You can view the request and response details for each endpoint.

4. **OpenAPI Specification**:
    If you need the OpenAPI specification (JSON format), it is available at https://localhost:5294/swagger/v1/swagger.json

Example Endpoint: About API
The About API provides information about the application. You can test it using the following endpoint:
    ```
    GET /api/about
    ```

This will return details such as the application title, description, and version.