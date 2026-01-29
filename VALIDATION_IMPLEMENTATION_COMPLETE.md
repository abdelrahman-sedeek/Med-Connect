# Validation & Error Handling - Complete Architecture

## Your Current Setup

### ? What You Have:

1. **Validators for Each Feature** (Already Implemented)
   - `AddToFavoritesCommandValidator` - Validates PatientId, DoctorId
   - `GetFavoritesQueryValidator` - Validates PatientId
   - `SearchDoctorsQueryValidator` - Validates query parameters
   - `GetNearbyDoctorsQueryValidator` - Validates location parameters
   - And more...

2. **ValidationBehavior (Pipeline)** (Just Updated)
   - Intercepts all Commands/Queries
   - Runs validators before handler
   - Returns ResponseViewModel on validation failure

3. **GlobalExceptionMiddleware** (Just Updated)
   - Catches all unhandled exceptions
   - Converts to ResponseViewModel format
   - Proper HTTP status codes

## How Errors Flow Through Your System

### Example 1: Validation Error (Missing PatientId)

```
Request: GET /api/favorites
         (no PatientId provided)
           ?
ValidationBehavior intercepts
           ?
GetFavoritesQueryValidator runs
           ?
RuleFor(x => x.PatientId).NotEmpty() FAILS
           ?
ValidationBehavior returns ResponseViewModel with 422
           ?
Client sees:
{
  "data": null,
  "isSucsess": false,
  "status": 422,
  "message": "Validation failed. Please check the errors.",
  "errors": [
    {
      "propertyName": "PatientId",
      "message": "Please, pass a valid Patient Id"
    }
  ]
}
```

### Example 2: Business Logic Error (Patient Not Found)

```
Request: GET /api/favorites/999
         (PatientId=999)
           ?
ValidationBehavior intercepts
           ?
Validators run (PatientId=999 is valid)
           ?
GetFavoritesQueryHandler runs
           ?
Handler checks if patient exists
           ?
throw new KeyNotFoundException("Patient not found")
           ?
GlobalExceptionMiddleware catches it
           ?
Converts to ResponseViewModel with 404
           ?
Client sees:
{
  "data": null,
  "isSucsess": false,
  "status": 404,
  "message": "Patient with ID 999 not found.",
  "errors": []
}
```

### Example 3: Success Response

```
Request: GET /api/favorites/1
         (PatientId=1, exists)
           ?
ValidationBehavior intercepts
           ?
Validators run (PatientId=1 is valid)
           ?
GetFavoritesQueryHandler runs
           ?
Patient found, favorites retrieved
           ?
Handler returns ResponseViewModel<List<GetFavoritesDto>>.SuccessResponse(favorites)
           ?
Client sees:
{
  "data": [
    { "doctorId": 5, "doctorName": "Dr. Ahmed" },
    { "doctorId": 8, "doctorName": "Dr. Fatima" }
  ],
  "isSucsess": true,
  "status": 200,
  "message": "Request completed successfully",
  "errors": []
}
```

## Files Updated in This Session

### 1. **ValidationBehavior.cs** ?
   - Validates all requests before handler execution
   - Returns ResponseViewModel on validation failure
   - Status Code: 422 (Unprocessable Entity)

### 2. **GlobalExceptionMiddleware.cs** ?
   - Catches all unhandled exceptions
   - Converts specific exceptions to appropriate status codes
   - Returns all errors as ResponseViewModel
   - Status Codes:
     - 422: ValidationException
     - 404: KeyNotFoundException
     - 400: InvalidOperationException, ArgumentException
     - 500: Other exceptions

### 3. **All Controllers** ? (SearchHistory, Favorites, Doctors)
   - Return ResponseViewModel from handlers
   - Simply pass through what handlers return

### 4. **All Command/Query Handlers** ?
   - Return ResponseViewModel<T>
   - Handle business logic exceptions by throwing specific exception types

### 5. **All Command/Query Records** ?
   - IRequest<ResponseViewModel<T>> instead of IRequest<T>

## Testing Your Validation

### Test 1: Invalid PatientId (0 or negative)
```bash
curl "http://localhost:5000/api/favorites/0"
```
Expected: 422 with validation error

### Test 2: Valid PatientId, But Patient Doesn't Exist
```bash
curl "http://localhost:5000/api/favorites/999"
```
Expected: 404 Patient not found

### Test 3: Valid Request
```bash
curl "http://localhost:5000/api/favorites/1"
```
Expected: 200 with favorites data

## Key Points to Remember

1. **Every request goes through ValidationBehavior** - validates before handler runs
2. **Every response returns ResponseViewModel** - consistent format
3. **Every error has proper status code** - semantic HTTP status codes
4. **Every error is detailed** - includes error messages and property names
5. **No exceptions reach client** - GlobalExceptionMiddleware catches everything

## Your Validators Are Already Working!

You don't need to do anything additional. The system now:
- ? Validates PatientId is required and > 0
- ? Validates DoctorId is required and > 0
- ? Validates search parameters (coordinates, radius, pagination)
- ? Returns detailed error messages for each validation failure
- ? Returns proper HTTP status codes (422 for validation, 404 for not found, etc.)
