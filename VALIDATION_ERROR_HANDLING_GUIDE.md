# Validation Error Handling Guide

## Overview
Your application now has a **complete unified error handling system** that converts all types of errors (including validation errors) to `ResponseViewModel` format.

## How It Works

### 1. **ValidationBehavior (MediatR Pipeline)**
**Location**: `Doctor-Booking.Application/Common/Behaviours/ValidationBehavior.cs`

**What it does:**
- Runs BEFORE each Command/Query handler
- Executes all registered validators for that request
- If validation fails ? Returns `ResponseViewModel` with status 422 and error details
- If validation passes ? Proceeds to the handler

**Example Response (Validation Error):**
```json
{
  "data": null,
  "isSucsess": false,
  "status": 422,
  "message": "Validation failed. Please check the errors.",
  "errors": [
    {
      "propertyName": "PatientId",
      "message": "Patient ID is required."
    },
    {
      "propertyName": "DoctorId",
      "message": "Doctor ID is required."
    }
  ]
}
```

### 2. **GlobalExceptionMiddleware**
**Location**: `DoctorBooking/Middleware/GlobalExceptionMiddleware.cs`

**What it does:**
- Catches ANY unhandled exceptions in the application
- Converts specific exception types to appropriate HTTP status codes:
  - `ValidationException` ? 422 (Unprocessable Entity)
  - `KeyNotFoundException` ? 404 (Not Found)
  - `InvalidOperationException` ? 400 (Bad Request)
  - `ArgumentException` ? 400 (Bad Request)
  - Other exceptions ? 500 (Internal Server Error)
- Returns all errors in `ResponseViewModel` format

**Example Response (Handler Exception):**
```json
{
  "data": null,
  "isSucsess": false,
  "status": 404,
  "message": "Patient with ID 999 not found.",
  "errors": []
}
```

## Complete Flow Diagram

```
Client Request
    ?
ASP.NET Core Pipeline
    ?
GlobalExceptionMiddleware (catches exceptions)
    ?
Controller Action
    ?
MediatR Send (Command/Query)
    ?
ValidationBehavior ? Validators from Features
    ? (if validation fails)
    ? ResponseViewModel with 422 ?
    ? (if validation passes)
    ?
Handler (Command/Query Handler)
    ? (if exception)
    ? GlobalExceptionMiddleware catches it
    ? ResponseViewModel with appropriate status ?
    ? (if success)
    ?
Handler returns ResponseViewModel ?
    ?
Controller returns Ok(result)
    ?
GlobalExceptionMiddleware (pass-through if no exception)
    ?
Client Response (ResponseViewModel format)
```

## Where to See Validation Errors

### Scenario 1: Missing Required Field
**Request:**
```
GET /api/favorites/0
```

**Response (422 Unprocessable Entity):**
```json
{
  "data": null,
  "isSucsess": false,
  "status": 422,
  "message": "Validation failed. Please check the errors.",
  "errors": [
    {
      "propertyName": "PatientId",
      "message": "Patient ID must be greater than 0."
    }
  ]
}
```

### Scenario 2: Invalid Value
**Request:**
```
POST /api/favorites?patientId=abc&doctorId=xyz
```

**Response (422 Unprocessable Entity):**
```json
{
  "data": null,
  "isSucsess": false,
  "status": 422,
  "message": "Validation failed. Please check the errors.",
  "errors": [
    {
      "propertyName": "PatientId",
      "message": "Patient ID must be greater than 0."
    },
    {
      "propertyName": "DoctorId",
      "message": "Doctor ID must be greater than 0."
    }
  ]
}
```

### Scenario 3: Handler Exception (Not Found)
**Request:**
```
GET /api/favorites/999
```

**Response (404 Not Found):**
```json
{
  "data": null,
  "isSucsess": false,
  "status": 404,
  "message": "Patient with ID 999 not found.",
  "errors": []
}
```

### Scenario 4: Success
**Request:**
```
GET /api/favorites/1
```

**Response (200 OK):**
```json
{
  "data": [
    {
      "doctorId": 5,
      "doctorName": "Dr. Ahmed"
    }
  ],
  "isSucsess": true,
  "status": 200,
  "message": "Request completed successfully",
  "errors": []
}
```

## Key Features

? **Validation Errors Captured** - ValidationBehavior catches all validation failures  
? **Business Logic Errors Handled** - Handlers throw specific exceptions  
? **All Errors Unified Format** - Every response uses ResponseViewModel  
? **Proper HTTP Status Codes** - 422 for validation, 404 for not found, 400 for bad request, 500 for server errors  
? **Detailed Error Information** - Each error includes property name and message  
? **Development vs Production** - Detailed errors in development, generic in production  

## How to Add New Validators

1. **Create validator** in your feature folder:
```csharp
public class GetFavoritesQueryValidator : AbstractValidator<GetFavoritesQuery>
{
    public GetFavoritesQueryValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("Patient ID is required.")
            .GreaterThan(0).WithMessage("Patient ID must be greater than 0.");
    }
}
```

2. **Validator is automatically registered** via DependencyInjection:
```csharp
services.AddValidatorsFromAssembly(assembly);
```

3. **Validation runs automatically** before handler via ValidationBehavior

## How to Throw Exceptions in Handlers

```csharp
public async Task<ResponseViewModel<GetFavoritesDto>> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
{
    var patient = await repository.GetPatientAsync(request.PatientId);
    
    if (patient == null)
    {
        throw new KeyNotFoundException($"Patient with ID {request.PatientId} not found.");
    }
    
    // ... rest of logic
}
```

This will be caught by GlobalExceptionMiddleware and converted to ResponseViewModel with 404 status!

## Summary

Your validation error handling is now **complete and unified**:
- ? ValidationBehavior handles request validation
- ? GlobalExceptionMiddleware handles business logic exceptions
- ? All responses use ResponseViewModel format
- ? All errors are visible with detailed messages
