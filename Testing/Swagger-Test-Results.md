# BankTask API — Swagger Test Results

## 1. Test Overview

### Project
BankTask — Banking Backend API

### Testing Method
- Swagger UI
- PowerShell HTTP requests
- Local development environment

### Base URL

```text
https://localhost:7274
```

### Main API Areas Tested

```text
Authentication
Users
Accounts
Transactions
Audit Logs
Authentication / Authorization
Validation
Business Rules
Localization
Exception Handling
Audit Logging
```

### Languages Tested

```text
en
ar
```

### Authorization

Protected endpoints were tested using:

```http
Authorization: Bearer <ACCESS_TOKEN>
```

> Never commit a real JWT access token to the repository.
> `<ACCESS_TOKEN>` is intentionally used in this documentation.

---

# 2. Result Legend

| Result | Meaning |
|---|---|
| ✅ PASS | Actual behavior matched the expected behavior |
| ❌ FAIL | Actual behavior did not match the expected behavior |
| ⚠️ DEFERRED | Issue observed but intentionally not fixed during this testing pass |
| 🔹 PREVIOUSLY VERIFIED | Tested before the current documented run and confirmed by the developer |

---

# 3. PowerShell Command Convention

The original testing session used PowerShell with `Invoke-WebRequest`.

The following format is used throughout this document as a replayable equivalent:

```powershell
$body='REQUEST_JSON_HERE'

try {
    $r = Invoke-WebRequest `
        -Uri 'https://localhost:7274/api/ENDPOINT' `
        -Method Post `
        -ContentType 'application/json' `
        -Headers @{
            'Accept-Language'='en'
            'Authorization'='Bearer <ACCESS_TOKEN>'
        } `
        -Body $body

    Write-Host "STATUS: $($r.StatusCode)"
    $reader = New-Object System.IO.StreamReader($r.RawContentStream)
    Write-Host $reader.ReadToEnd()
}
catch {
    Write-Host "STATUS: $([int]$_.Exception.Response.StatusCode)"
    $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
    Write-Host $reader.ReadToEnd()
}
```

---

# 4. Authentication

# 4.1 Signup

## AUTH-01 — Signup Happy Path

### Endpoint

```http
POST /api/Auth/signup
```

### Request

```json
{
  "fullName": "Leen Test",
  "email": "leen.validation.test@example.com",
  "password": "Password123"
}
```

### PowerShell

```powershell
$body='{"fullName":"Leen Test","email":"leen.validation.test@example.com","password":"Password123"}'

try {
    $r=Invoke-WebRequest `
        -Uri 'https://localhost:7274/api/Auth/signup' `
        -Method Post `
        -ContentType 'application/json' `
        -Headers @{'Accept-Language'='en'} `
        -Body $body

    Write-Host "STATUS: $($r.StatusCode)"
    $reader=New-Object System.IO.StreamReader($r.RawContentStream)
    Write-Host $reader.ReadToEnd()
}
catch {
    Write-Host "STATUS: $([int]$_.Exception.Response.StatusCode)"
}
```

### Expected

```text
201 Created
```

### Actual

```text
201 Created
```

### Result

✅ PASS

---

## AUTH-02 — Signup Full Name Required

### Request

```json
{
  "fullName": "",
  "email": "validation.test@example.com",
  "password": "Password123"
}
```

### Expected

```text
400 Bad Request
FULL_NAME_REQUIRED
```

### Actual

```json
{
  "errorCode": "FULL_NAME_REQUIRED",
  "message": "Full name is required.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-03 — Signup Full Name Minimum Length

### Request

```json
{
  "fullName": "Li",
  "email": "validation.test@example.com",
  "password": "Password123"
}
```

### Expected

```text
400 Bad Request
FULL_NAME_MIN_LENGTH
```

### Actual

```json
{
  "errorCode": "FULL_NAME_MIN_LENGTH",
  "message": "Full name must be at least 3 characters.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-04 — Signup Full Name Maximum Length

### Request

Full name containing more than 100 characters.

### Expected

```text
400 Bad Request
FULL_NAME_MAX_LENGTH
```

### Actual

```json
{
  "errorCode": "FULL_NAME_MAX_LENGTH",
  "message": "Full name cannot exceed 100 characters.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-05 — Signup Email Required

### Request

```json
{
  "fullName": "User Test",
  "email": "",
  "password": "Password123"
}
```

### Expected

```text
400 Bad Request
EMAIL_REQUIRED
```

### Actual

```json
{
  "errorCode": "EMAIL_REQUIRED",
  "message": "Email is required.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-06 — Signup Invalid Email Format

### Request

```json
{
  "fullName": "User Test",
  "email": "invalid-email",
  "password": "Password123"
}
```

### Expected

```text
400 Bad Request
EMAIL_INVALID
```

### Actual

```json
{
  "errorCode": "EMAIL_INVALID",
  "message": "Invalid email format.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-07 — Signup Email Maximum Length

### Request

Email longer than 150 characters.

### Expected

```text
400 Bad Request
EMAIL_MAX_LENGTH
```

### Actual

```json
{
  "errorCode": "EMAIL_MAX_LENGTH",
  "message": "Email cannot exceed 150 characters.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-08 — Signup Password Required

### Request

```json
{
  "fullName": "User Test",
  "email": "validation.test@example.com",
  "password": ""
}
```

### Expected

```text
400 Bad Request
PASSWORD_REQUIRED
```

### Actual

```json
{
  "errorCode": "PASSWORD_REQUIRED",
  "message": "Password is required.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-09 — Signup Password Minimum Length

### Request

```json
{
  "fullName": "User Test",
  "email": "validation.test@example.com",
  "password": "1234567"
}
```

### Expected

```text
400 Bad Request
PASSWORD_MIN_LENGTH
```

### Actual

```json
{
  "errorCode": "PASSWORD_MIN_LENGTH",
  "message": "Password must be at least 8 characters.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-10 — Signup Password Maximum Length

### Request

Password containing more than 100 characters.

### Expected

```text
400 Bad Request
PASSWORD_MAX_LENGTH
```

### Actual

```json
{
  "errorCode": "PASSWORD_MAX_LENGTH",
  "message": "Password cannot exceed 100 characters.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-11 — Signup Duplicate Email

### Request

```json
{
  "fullName": "Another User",
  "email": "leen.validation.test@example.com",
  "password": "Password123"
}
```

### Expected

```text
409 Conflict
USER_EMAIL_ALREADY_EXISTS
```

### Actual

```json
{
  "errorCode": "USER_EMAIL_ALREADY_EXISTS",
  "message": "A user with this email already exists.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-12 — Signup Duplicate Email + Arabic

### Request Header

```http
Accept-Language: ar
```

### Expected

```text
409 Conflict
USER_EMAIL_ALREADY_EXISTS
Arabic localized message
```

### Actual

The response returned:

```text
409 Conflict
USER_EMAIL_ALREADY_EXISTS
Arabic localized message
```

### Result

✅ PASS

---

# 4.2 Login

## AUTH-13 — Login Happy Path

### Endpoint

```http
POST /api/Auth/login
```

### Request

```json
{
  "email": "leen.validation.test@example.com",
  "password": "Password123"
}
```

### Expected

```text
200 OK
accessToken returned
```

### Actual

```text
200 OK
accessToken returned
```

### Result

✅ PASS

---

## AUTH-14 — Login Password Required

### Request

```json
{
  "email": "leen.validation.test@example.com",
  "password": ""
}
```

### Expected

```text
400 Bad Request
PASSWORD_REQUIRED
```

### Actual

```json
{
  "errorCode": "PASSWORD_REQUIRED",
  "message": "Password is required.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-15 — Login Password Minimum Length

### Request

```json
{
  "email": "leen.validation.test@example.com",
  "password": "1234567"
}
```

### Expected

```text
400 Bad Request
PASSWORD_MIN_LENGTH
```

### Actual

```json
{
  "errorCode": "PASSWORD_MIN_LENGTH",
  "message": "Password must be at least 8 characters.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-16 — Login Invalid Email

### Request

```json
{
  "email": "leen-test",
  "password": "Password123"
}
```

### Expected

```text
400 Bad Request
EMAIL_INVALID
```

### Actual

```json
{
  "errorCode": "EMAIL_INVALID",
  "message": "Invalid email format.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-17 — Login Wrong Password

### Request

```json
{
  "email": "leen.validation.test@example.com",
  "password": "WrongPassword123"
}
```

### Expected

```text
401 Unauthorized
AUTH_INVALID_CREDENTIALS
```

### Actual

```json
{
  "errorCode": "AUTH_INVALID_CREDENTIALS",
  "message": "Invalid email or password.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-18 — Login Unknown Email

### Request

```json
{
  "email": "user.does.not.exist@example.com",
  "password": "Password123"
}
```

### Expected

```text
401 Unauthorized
AUTH_INVALID_CREDENTIALS
```

### Actual

```json
{
  "errorCode": "AUTH_INVALID_CREDENTIALS",
  "message": "Invalid email or password.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## AUTH-19 — Login Invalid Credentials + Arabic

### Request Header

```http
Accept-Language: ar
```

### Expected

```text
401 Unauthorized
AUTH_INVALID_CREDENTIALS
Arabic localized message
```

### Actual

```text
401 Unauthorized
AUTH_INVALID_CREDENTIALS
Arabic localized message
```

### Result

✅ PASS

---

## AUTH-20 — Expired JWT

### Scenario

An expired access token was used with a protected endpoint.

### Actual

```text
401 Unauthorized
```

Response header contained:

```text
www-authenticate:
Bearer error="invalid_token",
error_description="The token expired ..."
```

### Result

✅ PASS

### Notes

The token expiration was expected behavior.  
This was an authentication expiration test, not a validation failure.

---

# 5. Users

## USER-01 — GET User by ID / Authorized

### Endpoint

```http
GET /api/Users/{id}
```

### Expected

```text
200 OK
```

### Actual

```text
200 OK
```

### Result

✅ PASS

---

## USER-02 — GET User by ID / Not Found

### Request

```text
00000000-0000-0000-0000-000000000001
```

### Expected

```text
404 Not Found
USER_NOT_FOUND
```

### Actual

```json
{
  "errorCode": "USER_NOT_FOUND",
  "message": "User not found.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## USER-03 — GET Users Without JWT

### Expected

```text
401 Unauthorized
```

### Actual

```text
401 Unauthorized
```

### Result

✅ PASS

---

## USER-04 — GET All Users

### Endpoint

```http
GET /api/Users
```

### Expected

```text
200 OK
```

### Actual

```text
200 OK
Users returned
```

### Result

✅ PASS

---

## USER-05 — CREATE User / Happy Path

### Endpoint

```http
POST /api/Users
```

### Request

```json
{
  "fullName": "User Test",
  "email": "user.20261001@example.com",
  "password": "Password123"
}
```

### Expected

```text
201 Created
```

### Actual

```text
201 Created
```

### Result

✅ PASS

---

## USER-06 — CREATE User / Full Name Required

### Request

```json
{
  "fullName": "",
  "email": "validation.user@example.com",
  "password": "Password123"
}
```

### Actual

```text
400 Bad Request
FULL_NAME_REQUIRED
```

### Result

✅ PASS

---

## USER-07 — CREATE User / Full Name Min Length

### Request

```json
{
  "fullName": "Li",
  "email": "user.minname@example.com",
  "password": "Password123"
}
```

### Actual

```text
400 Bad Request
FULL_NAME_MIN_LENGTH
```

### Result

✅ PASS

---

## USER-08 — CREATE User / Full Name Max Length

### Request

Full name longer than 100 characters.

### Actual

```text
400 Bad Request
FULL_NAME_MAX_LENGTH
```

### Result

✅ PASS

---

## USER-09 — CREATE User / Email Required

### Request

```json
{
  "fullName": "User Test",
  "email": "",
  "password": "Password123"
}
```

### Actual

```text
400 Bad Request
EMAIL_REQUIRED
```

### Result

✅ PASS

---

## USER-10 — CREATE User / Invalid Email

### Request

```json
{
  "fullName": "User Test",
  "email": "invalid-email",
  "password": "Password123"
}
```

### Actual

```text
400 Bad Request
EMAIL_INVALID
```

### Result

✅ PASS

---

## USER-11 — CREATE User / Email Max Length

### Request

Email longer than 150 characters.

### Actual

```text
400 Bad Request
EMAIL_MAX_LENGTH
```

### Result

✅ PASS

---

## USER-12 — CREATE User / Password Min Length

### Request

```json
{
  "fullName": "User Test",
  "email": "user.password.min@example.com",
  "password": "1234567"
}
```

### Actual

```text
400 Bad Request
PASSWORD_MIN_LENGTH
```

### Result

✅ PASS

---

## USER-13 — CREATE User / Password Max Length

### Request

Password longer than 100 characters.

### Actual

```text
400 Bad Request
PASSWORD_MAX_LENGTH
```

### Result

✅ PASS

---

## USER-14 — CREATE User / Duplicate Email

### Request

Existing email.

### Expected

```text
409 Conflict
USER_EMAIL_ALREADY_EXISTS
```

### Actual

```text
409 Conflict
USER_EMAIL_ALREADY_EXISTS
```

### Result

✅ PASS

---

## USER-15 — UPDATE User / Duplicate Email

### Scenario

An existing user was updated with an email already used by another user.

### Actual

```text
500 Internal Server Error
```

Observed SQL Server error:

```text
UQ_Users_Email
```

### Result

⚠️ DEFERRED

### Notes

This was intentionally not fixed during this testing pass.

---

## USER-16 — UPDATE User / New Email

### Scenario

Existing user updated using a new email.

### Actual

```text
500 Internal Server Error
UNEXPECTED_ERROR
```

Observed exception:

```text
System.InvalidOperationException:
Sequence contains no elements
```

Observed repository path:

```text
UserRepository.UpdateAsync
QuerySingleAsync<int>
```

### Result

⚠️ DEFERRED

---

## USER-17 — DELETE User / Existing User

### Endpoint

```http
DELETE /api/Users/{id}
```

### Actual

```text
500 Internal Server Error
UNEXPECTED_ERROR
```

Observed exception:

```text
System.InvalidOperationException:
Sequence contains no elements
```

Observed at:

```text
UserRepository.DeleteAsync
```

### Result

⚠️ DEFERRED

---

## USER-18 — DELETE User / Not Found

### Request

```text
00000000-0000-0000-0000-000000000001
```

### Expected

```text
404 Not Found
USER_NOT_FOUND
```

### Actual

```text
404 Not Found
USER_NOT_FOUND
```

### Result

✅ PASS

---

# 6. Accounts

## ACCOUNT-01 — GET All Accounts

### Endpoint

```http
GET /api/Accounts
```

### Expected

```text
200 OK
```

### Actual

```text
200 OK
Accounts returned
```

### Result

✅ PASS

### Notes

This endpoint was observed to work without JWT in the current implementation.

No authorization behavior was changed based only on assumption.

---

## ACCOUNT-02 — GET Account by ID / Existing

### Endpoint

```http
GET /api/Accounts/{id}
```

### Expected

```text
200 OK
```

### Actual

```text
200 OK
```

### Result

✅ PASS

---

## ACCOUNT-03 — GET Account by ID / Not Found

### Request

```text
00000000-0000-0000-0000-000000000001
```

### Expected

```text
404 Not Found
ACCOUNT_NOT_FOUND
```

### Actual

```json
{
  "errorCode": "ACCOUNT_NOT_FOUND",
  "message": "Account not found.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## ACCOUNT-04 — GET Account / Malformed GUID

### Request Attempt

```text
abc
```

### Result

The Swagger UI did not send the malformed GUID request to the server.

### Result

⚠️ DEFERRED

### Notes

Swagger/OpenAPI client-side validation prevented the request from being sent.

This does not prove or disprove server-side model-binding behavior.

---

## ACCOUNT-05 — CREATE Account / Happy Path

### Endpoint

```http
POST /api/Accounts
```

### Request

```json
{
  "userId": "d565e800-0403-40da-9534-2c1c86fa4bf7",
  "currency": 0,
  "accountType": 0
}
```

### Expected

```text
201 Created
```

### Actual

```text
201 Created
```

### Result

✅ PASS

---

## ACCOUNT-06 — CREATE Account / Invalid Currency

### Request

```json
{
  "userId": "d565e800-0403-40da-9534-2c1c86fa4bf7",
  "currency": 999,
  "accountType": 0
}
```

### Expected

```text
400 Bad Request
Invalid currency
```

### Actual

```text
400 Bad Request
CURRENCY_INVALID
```

### Result

✅ PASS

---

## ACCOUNT-07 — CREATE Account / Invalid Account Type

### Request

```json
{
  "userId": "d565e800-0403-40da-9534-2c1c86fa4bf7",
  "currency": 0,
  "accountType": 999
}
```

### Expected

```text
400 Bad Request
Invalid account type
```

### Actual

```text
400 Bad Request
ACCOUNT_TYPE_INVALID
```

### Result

✅ PASS

---

## ACCOUNT-08 — CREATE Account / Non-existing User

### Request

```json
{
  "userId": "00000000-0000-0000-0000-000000000001",
  "currency": 0,
  "accountType": 0
}
```

### Expected

Ideally:

```text
404 User not found
```

### Actual

```text
500 Internal Server Error
UNEXPECTED_ERROR
```

Observed SQL Server foreign key:

```text
FK_Accounts_Users
```

### Result

⚠️ DEFERRED

### Notes

The database correctly rejected the invalid foreign-key relationship, but the API did not translate it into a domain/business error.

---

## ACCOUNT-09 — CREATE Account / Empty User ID

### Request

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "currency": 0,
  "accountType": 0
}
```

### Expected

```text
400 validation/business error
```

### Actual

```text
500 Internal Server Error
UNEXPECTED_ERROR
```

Observed foreign-key failure.

### Result

⚠️ DEFERRED

---

## ACCOUNT-10 — CREATE Multiple Accounts for Same User

### Scenario

The same valid UserId was used to create another account.

### Expected

Multiple accounts are allowed by the current project relationship.

```text
User -> Accounts
One-to-many
```

### Actual

```text
201 Created
```

### Result

✅ PASS

### Notes

This is not considered a defect because the current project allows one user to have multiple accounts.

---

## ACCOUNT-11 — UPDATE Account / Valid Status

### Request

```json
{
  "status": 1
}
```

### Expected

```text
204 No Content
```

### Actual

```text
204 No Content
```

### Result

✅ PASS

---

## ACCOUNT-12 — UPDATE Account / Invalid Status

### Request

```json
{
  "status": 999
}
```

### Expected

```text
400 Bad Request
ACCOUNT_STATUS_INVALID
```

### Actual

```json
{
  "errorCode": "ACCOUNT_STATUS_INVALID",
  "message": "Invalid account status.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## ACCOUNT-13 — UPDATE Account / Not Found

### Request

```text
00000000-0000-0000-0000-000000000001
```

### Expected

```text
404 ACCOUNT_NOT_FOUND
```

### Actual

```text
404 ACCOUNT_NOT_FOUND
```

### Result

✅ PASS

---

## ACCOUNT-14 — DELETE Account / Existing

### Endpoint

```http
DELETE /api/Accounts/{id}
```

### Expected

```text
204 No Content
```

### Actual

```text
204 No Content
```

### Result

✅ PASS

### Notes

The current implementation performs a soft-close behavior by changing the account status rather than physically removing the row.

---

## ACCOUNT-15 — DELETE Account / Not Found

### Request

```text
00000000-0000-0000-0000-000000000001
```

### Expected

```text
404 ACCOUNT_NOT_FOUND
```

### Actual

```text
404 ACCOUNT_NOT_FOUND
```

### Result

✅ PASS

---

# 7. Audit Logs

## AUDIT-01 — GET All Audit Logs / Empty Initial State

### Endpoint

```http
GET /api/AuditLogs?offset=0&limit=100
```

### Expected

```text
200 OK
```

### Actual

Initially:

```json
[]
```

### Result

✅ PASS

### Notes

The endpoint itself worked. At that stage no audit records had yet been created by the tested operations.

---

## AUDIT-02 — GET Audit Logs / Negative Offset

### Request

```http
GET /api/AuditLogs?offset=-1&limit=100
```

### Actual

```text
200 OK
[]
```

### Result

✅ PASS

### Notes

Current service normalizes:

```text
offset < 0 -> 0
```

---

## AUDIT-03 — GET Audit Logs / Negative Limit

### Request

```http
GET /api/AuditLogs?offset=0&limit=-1
```

### Actual

```text
200 OK
[]
```

### Result

✅ PASS

### Notes

Current service normalizes:

```text
limit < 1 -> 1
```

---

## AUDIT-04 — GET Audit Logs / Limit = 0

### Request

```http
GET /api/AuditLogs?offset=0&limit=0
```

### Actual

```text
200 OK
[]
```

### Result

✅ PASS

### Notes

Current service normalizes zero to a safe positive limit.

---

## AUDIT-05 — GET Audit Log by ID / Not Found

### Request

```text
00000000-0000-0000-0000-000000000001
```

### Expected

```text
404 AUDIT_LOG_NOT_FOUND
```

### Actual

```text
404 AUDIT_LOG_NOT_FOUND
```

### Result

✅ PASS

---

## AUDIT-06 — USER_CREATED Audit Log

### Trigger

Successful User creation.

### Expected Audit Fields

```text
action       = USER_CREATED
entityType   = User
entityId     = created user ID
userId       = authenticated user when available
oldValues    = null
newValues    = populated
ipAddress    = populated
createdAt    = populated
eventId      = populated
```

### Actual

Audit record was returned with:

```text
action      = USER_CREATED
entityType  = User
entityId    = created user ID
oldValues   = null
newValues   = populated
ipAddress   = 127.0.0.1/32
createdAt   = populated
```

### Result

✅ PASS

---

## AUDIT-07 — USER_LOGGED_IN Audit Log

### Trigger

Successful login.

### Expected

```text
USER_LOGGED_IN
```

### Actual

Audit log list contained:

```text
action      = USER_LOGGED_IN
entityType  = User
entityId    = authenticated user ID
oldValues   = null
newValues   = null
ipAddress   = 127.0.0.1/32
createdAt   = populated
```

### Result

✅ PASS

---

## AUDIT-08 — ACCOUNT_CREATED Audit Log

### Trigger

Successful account creation.

### Expected

```text
ACCOUNT_CREATED
```

### Actual

Audit log returned:

```text
action       = ACCOUNT_CREATED
entityType   = Account
entityId     = created account ID
eventId      = populated
userId       = populated
oldValues    = null
newValues    = populated
ipAddress    = 127.0.0.1/32
createdAt    = populated
```

### Result

✅ PASS

---

## AUDIT-09 — ACCOUNT_UPDATED Audit Log

### Trigger

Successful account status update.

### Expected

```text
ACCOUNT_UPDATED
```

### Actual

Audit log returned:

```text
action       = ACCOUNT_UPDATED
entityType   = Account
entityId     = affected account ID
oldValues    = populated
newValues    = populated
ipAddress    = 127.0.0.1/32
createdAt    = populated
```

### Result

✅ PASS

---

## AUDIT-10 — ACCOUNT_DELETED Audit Log

### Trigger

Delete/close account operation.

### Expected

```text
ACCOUNT_DELETED
```

### Actual

Audit log returned:

```text
action       = ACCOUNT_DELETED
entityType   = Account
entityId     = affected account ID
oldValues    = populated
newValues    = populated
ipAddress    = 127.0.0.1/32
createdAt    = populated
```

### Result

✅ PASS

---

# 8. Transactions

# TRANSACTION TEST RUN

The current detailed transaction run started at Test 61.

---

## T61 — GET All Transactions / Happy Path

### Endpoint

```http
GET /api/Transactions
```

### Expected

```text
200 OK
```

### Actual

```text
200 OK
```

The response returned multiple transaction records.

Visible fields included:

```text
id
eventId
sourceAccountId
destinationAccountId
transactionType
amount
currency
referenceNumber
description
createdAt
```

### Result

✅ PASS

---

## T62 — GET Transaction by ID / Existing

### Endpoint

```http
GET /api/Transactions/{id}
```

### Request

```text
7011d5ec-5996-4989-981d-88a7e5ea29c1
```

### Actual

```text
200 OK
```

### Response

```json
{
  "id": "7011d5ec-5996-4989-981d-88a7e5ea29c1",
  "eventId": "3469aa07-9d49-4551-8687-5bd843b298b3",
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 3,
  "amount": 20,
  "currency": "JOD",
  "referenceNumber": "TX-f65e563cb2c47809ad4ab14666fff45",
  "description": "Amount minimum test",
  "createdAt": "2026-09-30T22:16:14.175209+00:00"
}
```

### Result

✅ PASS

### Notes

An earlier attempt briefly returned 404, but the request was repeated with the same ID and returned 200. The successful repeated execution is the verified result.

---

## T63 — GET Transaction by ID / Not Found

### Endpoint

```http
GET /api/Transactions/{id}
```

### Request

```text
00000000-0000-0000-0000-000000000001
```

### Expected

```text
404 Not Found
TRANSACTION_NOT_FOUND
```

### Actual

```json
{
  "errorCode": "TRANSACTION_NOT_FOUND",
  "message": "Transaction not found.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## T64 — CREATE Transaction / Transfer Happy Path

### Endpoint

```http
POST /api/Transactions
```

### Request

```json
{
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": 100,
  "currency": "JOD",
  "description": "Test transfer"
}
```

### PowerShell

```powershell
$body='{"sourceAccountId":"f0855639-cafc-4e5c-b49d-a45dc493a4a5","destinationAccountId":"e4b5af5f-7452-457c-bf6b-b3732403b499","transactionType":1,"amount":100,"currency":"JOD","description":"Test transfer"}'

try {
    $r=Invoke-WebRequest `
        -Uri 'https://localhost:7274/api/Transactions' `
        -Method Post `
        -ContentType 'application/json' `
        -Headers @{'Accept-Language'='en';'Authorization'='Bearer <ACCESS_TOKEN>'} `
        -Body $body

    Write-Host "STATUS: $($r.StatusCode)"
    $reader=New-Object System.IO.StreamReader($r.RawContentStream)
    Write-Host $reader.ReadToEnd()
}
catch {
    Write-Host "STATUS: $([int]$_.Exception.Response.StatusCode)"
    $reader=New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
    Write-Host $reader.ReadToEnd()
}
```

### Expected

```text
201 Created
```

### Actual

```text
201 Created
```

### Response

```json
{
  "id": "3551e8b3-561b-4fda-8ae7-1ccaf019991a",
  "eventId": "76f10a3e-a982-401f-ba9f-3e5214a6e46f",
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": 100,
  "currency": "JOD",
  "referenceNumber": "TX-07feabe048a4d21b57104ad676fc811",
  "description": "Test transfer",
  "createdAt": "2026-10-01T18:43:50.798563+00:00"
}
```

### Result

✅ PASS

---

## T65 — Audit Log for Transaction Created

### Endpoint

```http
GET /api/AuditLogs?offset=0&limit=100
```

### Expected

An audit record related to T64 should exist.

### Expected fields

```text
action      = TRANSACTION_CREATED
entityType  = Transaction
entityId    = 3551e8b3-561b-4fda-8ae7-1ccaf019991a
eventId     = populated
userId      = populated
oldValues   = null
newValues   = populated
ipAddress   = populated
createdAt   = populated
```

### Actual

The audit list contained:

```text
action      = TRANSACTION_CREATED
entityType  = Transaction
entityId    = 3551e8b3-561b-4fda-8ae7-1ccaf019991a
eventId     = populated
userId      = populated
oldValues   = null
newValues   = populated
ipAddress   = 127.0.0.1/32
createdAt   = populated
```

### Result

✅ PASS

---

## T66 — CREATE Transaction / Negative Amount

### Request

```json
{
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": -100,
  "currency": "JOD",
  "description": "Negative amount test"
}
```

### Expected

```text
400 Bad Request
TRANSACTION_AMOUNT_INVALID
```

### Actual

```json
{
  "errorCode": "TRANSACTION_AMOUNT_INVALID",
  "message": "Transaction amount must be greater than zero.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## T67 — CREATE Transaction / Amount = 0

### Request

```json
{
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": 0,
  "currency": "JOD",
  "description": "Zero amount test"
}
```

### Expected

```text
400 Bad Request
TRANSACTION_AMOUNT_INVALID
```

### Actual

```json
{
  "errorCode": "TRANSACTION_AMOUNT_INVALID",
  "message": "Transaction amount must be greater than zero.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## T68 — CREATE Transaction / Invalid Transaction Type

### Request

```json
{
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 999,
  "amount": 100,
  "currency": "JOD",
  "description": "Invalid transaction type test"
}
```

### Expected

```text
400 Bad Request
TRANSACTION_TYPE_INVALID
```

### Actual

```json
{
  "errorCode": "TRANSACTION_TYPE_INVALID",
  "message": "Invalid transaction type.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## T69 — Transfer / Both Accounts Required

### Request

```json
{
  "transactionType": 1,
  "amount": 100,
  "currency": "JOD",
  "description": "Transfer accounts required test"
}
```

### Expected

```text
400 Bad Request
TRANSFER_ACCOUNTS_REQUIRED
```

### Actual

```json
{
  "errorCode": "TRANSFER_ACCOUNTS_REQUIRED",
  "message": "Transfer requires both source and destination accounts.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## T70 — Transfer / Source and Destination Must Differ

### Request

```json
{
  "sourceAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": 100,
  "currency": "JOD",
  "description": "Same accounts transfer test"
}
```

### Expected

```text
400 Bad Request
TRANSFER_ACCOUNTS_MUST_DIFFER
```

### Actual

```json
{
  "errorCode": "TRANSFER_ACCOUNTS_MUST_DIFFER",
  "message": "Source and destination accounts must be different.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## T71 — Deposit / Destination Required

### Request

```json
{
  "transactionType": 2,
  "amount": 100,
  "currency": "JOD",
  "description": "Deposit destination required test"
}
```

### Expected

```text
400 Bad Request
DEPOSIT_DESTINATION_REQUIRED
```

### Actual

```json
{
  "errorCode": "DEPOSIT_DESTINATION_REQUIRED",
  "message": "Deposit requires a destination account.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## T72 — Withdrawal / Source Required

### Request

```json
{
  "transactionType": 3,
  "amount": 100,
  "currency": "JOD",
  "description": "Withdrawal source required test"
}
```

### Expected

```text
400 Bad Request
WITHDRAWAL_SOURCE_REQUIRED
```

### Actual

```json
{
  "errorCode": "WITHDRAWAL_SOURCE_REQUIRED",
  "message": "Withdrawal requires a source account.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## T73 — Currency Invalid / Too Short

### Request

```json
{
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": 100,
  "currency": "JO",
  "description": "Invalid currency test"
}
```

### Expected

```text
400 Bad Request
CURRENCY_INVALID
```

### Actual

```json
{
  "errorCode": "CURRENCY_INVALID",
  "message": "Currency must contain exactly 3 characters.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## T74 — Currency Required / Empty

### Request

```json
{
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": 100,
  "currency": "",
  "description": "Empty currency test"
}
```

### Expected

```text
400 Bad Request
```

### Actual

```json
{
  "errorCode": "TRANSACTION_CURRENCY_REQUIRED",
  "message": "Currency is required.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

### Notes

The first attempt of this test returned 401 because the JWT had expired.
The token was renewed and the test was then executed successfully.

---

## T75 — Currency Required / Whitespace

### Request

```json
{
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": 100,
  "currency": "   ",
  "description": "Whitespace currency test"
}
```

### Expected

```text
400 Bad Request
```

### Actual

```json
{
  "errorCode": "TRANSACTION_CURRENCY_REQUIRED",
  "message": "Currency is required.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## T76 — Currency Invalid / Too Long

### Request

```json
{
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": 100,
  "currency": "JODX",
  "description": "Long currency test"
}
```

### Expected

```text
400 Bad Request
CURRENCY_INVALID
```

### Actual

```json
{
  "errorCode": "CURRENCY_INVALID",
  "message": "Currency must contain exactly 3 characters.",
  "traceId": "<generated-trace-id>"
}
```

### Result

✅ PASS

---

## T77 — Currency Lowercase

### Request

```json
{
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": 100,
  "currency": "jod",
  "description": "Lowercase currency test"
}
```

### Expected

```text
201 Created
currency returned as JOD
```

### Actual

```text
201 Created
currency = JOD
```

### Response

```json
{
  "id": "43b2ac63-5987-42bf-9960-de0fdd4921f5",
  "eventId": "72493813-2e63-4974-90ec-2cca1ea493a6",
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": 100,
  "currency": "JOD",
  "referenceNumber": "TX-6bf670430e1c41f78c401e40ecc99040",
  "description": "Lowercase currency test",
  "createdAt": "2026-10-01T19:02:51.518465+00:00"
}
```

### Result

✅ PASS

---

## T78 — Currency With Surrounding Spaces

### Request

```json
{
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": 100,
  "currency": " JOD ",
  "description": "Currency trim test"
}
```

### Expected

Originally expected:

```text
201 Created
currency = JOD
```

### Actual

```json
{
  "errorCode": "CURRENCY_INVALID",
  "message": "Currency must contain exactly 3 characters.",
  "traceId": "<generated-trace-id>"
}
```

### Result

❌ FAIL

### Notes

The actual implementation rejected the value before it could be accepted as a trimmed three-character currency.

This was intentionally not fixed during the current testing pass.

---

## T79 — Currency Exactly 3 Characters

### Request

```json
{
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": 100,
  "currency": "JOD",
  "description": "Exact currency test"
}
```

### Expected

```text
201 Created
```

### Actual

```text
201 Created
currency = JOD
```

### Response

```json
{
  "id": "2f28788a-c1a0-45cc-af0a-be2ae4118077",
  "eventId": "1082f567-cf1b-4ed5-8875-04ca063896c3",
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 1,
  "amount": 100,
  "currency": "JOD",
  "referenceNumber": "TX-4df1fe4f0fa04ad594f339ca42286e87",
  "description": "Exact currency test",
  "createdAt": "2026-10-01T19:05:50.601557+00:00"
}
```

### Result

✅ PASS

---

## T80 — Deposit / Happy Path

### Request

```json
{
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 2,
  "amount": 50,
  "currency": "JOD",
  "description": "Deposit happy path test"
}
```

### Expected

```text
201 Created
```

### Actual

```text
201 Created
```

### Response

```json
{
  "id": "29b7cd12-9ed4-4f6a-8762-90a853aefe08",
  "eventId": "14d89314-16e9-49db-8435-b035a5c49788",
  "sourceAccountId": null,
  "destinationAccountId": "e4b5af5f-7452-457c-bf6b-b3732403b499",
  "transactionType": 2,
  "amount": 50,
  "currency": "JOD",
  "referenceNumber": "TX-22d2834f1124a729219d980c91f32cd",
  "description": "Deposit happy path test",
  "createdAt": "2026-10-01T19:07:27.422728+00:00"
}
```

### Result

✅ PASS

---

## T81 — Withdrawal / Happy Path

### Request

```json
{
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "transactionType": 3,
  "amount": 50,
  "currency": "JOD",
  "description": "Withdrawal happy path test"
}
```

### Expected

```text
201 Created
```

### Actual

```text
201 Created
```

### Response

```json
{
  "id": "e2ab82c9-32a5-4003-bd62-82864b282a1e",
  "eventId": "54a4813e-6130-44ff-a8ff-18b55bb6e75d",
  "sourceAccountId": "f0855639-cafc-4e5c-b49d-a45dc493a4a5",
  "destinationAccountId": null,
  "transactionType": 3,
  "amount": 50,
  "currency": "JOD",
  "referenceNumber": "TX-81c90dc97c3247d0a1b4ac940db50f13",
  "description": "Withdrawal happy path test",
  "createdAt": "2026-10-01T19:08:13.984698+00:00"
}
```

### Result

✅ PASS

---

# 9. Additional Transaction Tests Previously Verified

The following transaction cases were previously tested before the current detailed run and were confirmed as working.

Because the original raw Swagger/PowerShell output for these earlier executions is not preserved in the current transcript, they are explicitly marked as previously verified rather than reconstructing fake raw responses.

---

## T82 — Deposit / Audit Log

### Scenario

A successful Deposit was checked through:

```http
GET /api/AuditLogs?offset=0&limit=100
```

### Expected

Corresponding transaction audit record exists.

### Actual

Previously verified as working.

### Result

🔹 PREVIOUSLY VERIFIED

---

## T83 — Withdrawal / Audit Log

### Scenario

A successful Withdrawal was checked through:

```http
GET /api/AuditLogs?offset=0&limit=100
```

### Expected

Corresponding transaction audit record exists.

### Actual

Previously verified as working.

### Result

🔹 PREVIOUSLY VERIFIED

---

## T84 — Transfer / Source Account Missing Only

### Scenario

Transfer request sent without `sourceAccountId`.

### Expected

```text
400
TRANSFER_ACCOUNTS_REQUIRED
```

### Actual

Previously verified as working.

### Result

🔹 PREVIOUSLY VERIFIED

---

## T85 — Transfer / Destination Account Missing Only

### Scenario

Transfer request sent without `destinationAccountId`.

### Expected

```text
400
TRANSFER_ACCOUNTS_REQUIRED
```

### Actual

Previously verified as working.

### Result

🔹 PREVIOUSLY VERIFIED

---

## T86 — Transaction Authorization

### Scenario

Transaction endpoint tested without a valid JWT.

### Expected

```text
401 Unauthorized
```

### Actual

Previously verified as working.

### Result

🔹 PREVIOUSLY VERIFIED

---

# 10. Validation Rules Verified

## User / Authentication

```text
FullName:
- Required
- Minimum length = 3
- Maximum length = 100

Email:
- Required
- Valid email format
- Maximum length = 150

Password:
- Required
- Minimum length = 8
- Maximum length = 100
```

## Account

```text
Currency enum must be valid
AccountType enum must be valid
Account status must be valid
Non-existing UserId currently reaches database FK handling
```

## Transaction

```text
Amount > 0

TransactionType must be valid

Transfer:
- SourceAccountId required
- DestinationAccountId required
- SourceAccountId != DestinationAccountId

Deposit:
- DestinationAccountId required

Withdrawal:
- SourceAccountId required

Currency:
- Required
- Exactly 3 characters
- Lowercase accepted and normalized
- Surrounding spaces currently rejected
```

---

# 11. Business Rules Verified

## Transaction Amount

```text
Amount <= 0
```

returns:

```text
TRANSACTION_AMOUNT_INVALID
```

---

## Transfer Accounts

Both source and destination are required.

```text
Missing both
Missing source
Missing destination
```

were tested/verified.

---

## Transfer Source and Destination

The same account cannot be both source and destination.

```text
TRANSFER_ACCOUNTS_MUST_DIFFER
```

---

## Deposit

Deposit requires:

```text
DestinationAccountId
```

---

## Withdrawal

Withdrawal requires:

```text
SourceAccountId
```

---

# 12. Localization Verification

## English

Examples tested:

```text
EMAIL_INVALID
PASSWORD_REQUIRED
PASSWORD_MIN_LENGTH
PASSWORD_MAX_LENGTH
FULL_NAME_REQUIRED
FULL_NAME_MIN_LENGTH
FULL_NAME_MAX_LENGTH
EMAIL_MAX_LENGTH
USER_EMAIL_ALREADY_EXISTS
AUTH_INVALID_CREDENTIALS
ACCOUNT_NOT_FOUND
ACCOUNT_TYPE_INVALID
ACCOUNT_STATUS_INVALID
TRANSACTION_AMOUNT_INVALID
TRANSACTION_TYPE_INVALID
TRANSFER_ACCOUNTS_REQUIRED
TRANSFER_ACCOUNTS_MUST_DIFFER
DEPOSIT_DESTINATION_REQUIRED
WITHDRAWAL_SOURCE_REQUIRED
CURRENCY_INVALID
TRANSACTION_CURRENCY_REQUIRED
TRANSACTION_NOT_FOUND
USER_NOT_FOUND
AUDIT_LOG_NOT_FOUND
```

---

## Arabic

Arabic responses were verified using:

```http
Accept-Language: ar
```

Examples included:

```text
AUTH_INVALID_CREDENTIALS
USER_EMAIL_ALREADY_EXISTS
USER_NOT_FOUND
ACCOUNT_NOT_FOUND
CURRENCY_INVALID
```

### Result

✅ PASS

---

# 13. Authentication / Authorization Verification

## Protected User Endpoint

```text
No JWT
→ 401 Unauthorized

Valid JWT
→ 200 / expected endpoint behavior
```

✅ PASS

---

## Expired JWT

```text
Expired token
→ 401 Unauthorized
```

✅ PASS

---

## Current Account Endpoint Behavior

Account GET/POST/PUT/DELETE endpoints were tested according to their current implementation.

The current implementation was not modified merely to make authorization consistent with other entities.

---

# 14. Audit Logging Verification

After audit logging was integrated into successful operations, `GET /api/AuditLogs` returned records instead of an empty list.

Observed actions include:

```text
USER_CREATED
USER_LOGGED_IN

ACCOUNT_CREATED
ACCOUNT_UPDATED
ACCOUNT_DELETED

TRANSACTION_CREATED
```

Typical populated audit properties:

```text
Id
EventId
UserId
Action
EntityType
EntityId
OldValues
NewValues
IpAddress
CreatedAt
```

### Result

✅ PASS

---

# 15. Audit Log Data Examples

## ACCOUNT_CREATED

```text
action       = ACCOUNT_CREATED
entityType   = Account
oldValues    = null
newValues    = populated
ipAddress    = 127.0.0.1/32
createdAt    = populated
```

---

## ACCOUNT_UPDATED

```text
action       = ACCOUNT_UPDATED
entityType   = Account
oldValues    = populated
newValues    = populated
ipAddress    = 127.0.0.1/32
createdAt    = populated
```

---

## ACCOUNT_DELETED

```text
action       = ACCOUNT_DELETED
entityType   = Account
oldValues    = populated
newValues    = populated
ipAddress    = 127.0.0.1/32
createdAt    = populated
```

---

## TRANSACTION_CREATED

```text
action       = TRANSACTION_CREATED
entityType   = Transaction
oldValues    = null
newValues    = populated
ipAddress    = 127.0.0.1/32
createdAt    = populated
```

---

# 16. Known Issues / Deferred Issues

These issues were observed but intentionally not fixed during the current validation/business-rule testing pass.

---

## KI-01 — User Update / Stored Procedure Result

### Endpoint

```http
PUT /api/Users/{id}
```

### Actual

```text
500 Internal Server Error
UNEXPECTED_ERROR
```

### Observed Exception

```text
System.InvalidOperationException:
Sequence contains no elements
```

### Observed Repository

```text
UserRepository.UpdateAsync
```

### Observed Dapper Call

```text
QuerySingleAsync<int>
```

### Status

⚠️ DEFERRED

---

## KI-02 — User Update / Duplicate Email

### Endpoint

```http
PUT /api/Users/{id}
```

### Actual

```text
500 Internal Server Error
```

### Observed SQL Server constraint

```text
UQ_Users_Email
```

### Status

⚠️ DEFERRED

---

## KI-03 — User Delete / Repository Result

### Endpoint

```http
DELETE /api/Users/{id}
```

### Actual

```text
500 Internal Server Error
UNEXPECTED_ERROR
```

### Observed Exception

```text
Sequence contains no elements
```

### Status

⚠️ DEFERRED

---

## KI-04 — Create Account / Non-existing User

### Actual

```text
500 Internal Server Error
UNEXPECTED_ERROR
```

### Database error

```text
FK_Accounts_Users
```

### Status

⚠️ DEFERRED

---

## KI-05 — Create Account / Guid.Empty UserId

### Actual

```text
500 Internal Server Error
UNEXPECTED_ERROR
```

### Status

⚠️ DEFERRED

---

## KI-06 — Currency With Surrounding Spaces

### Input

```text
" JOD "
```

### Actual

```text
400
CURRENCY_INVALID
```

### Status

⚠️ DEFERRED

### Notes

The current implementation does not accept this input even though normalization logic exists elsewhere.

---

## KI-07 — Audit Log Pagination Normalization

Current behavior:

```text
offset < 0
→ normalized to 0

limit <= 0
→ normalized to 1
```

The API currently returns:

```text
200 OK
```

instead of rejecting the query values.

### Status

⚠️ DEFERRED

---

## KI-08 — Malformed GUID in Swagger UI

### Scenario

Using:

```text
abc
```

for a GUID endpoint.

### Actual

Swagger client-side validation prevented the request from reaching the server.

### Status

⚠️ DEFERRED

---

# 17. Important Observed Behavior — Transaction Balance

During an earlier transaction test, a transfer was successfully created even though the source account balance shown at the time was:

```text
0
```

### Observed Result

```text
201 Created
```

### Interpretation

The current implementation did not visibly enforce an insufficient-balance business rule during that test.

### Status

⚠️ OBSERVED / NOT CHANGED

### Notes

This documentation does not classify this as a defect because the business requirement was not confirmed during the current task.

---

# 18. Test Summary

| Area | Status |
|---|---|
| Signup Happy Path | ✅ |
| Signup Required Validation | ✅ |
| Signup Length Validation | ✅ |
| Signup Email Validation | ✅ |
| Signup Duplicate Email | ✅ |
| Login Happy Path | ✅ |
| Login Validation | ✅ |
| Login Invalid Credentials | ✅ |
| Arabic Localization | ✅ |
| User GET | ✅ |
| User GET Not Found | ✅ |
| User POST | ✅ |
| User POST Validation | ✅ |
| User POST Duplicate Email | ✅ |
| User Update | ⚠️ Deferred |
| User Delete | ⚠️ Deferred |
| Account GET | ✅ |
| Account POST | ✅ |
| Account POST Validation | ✅ |
| Account POST Invalid User | ⚠️ Deferred |
| Account PUT | ✅ |
| Account DELETE | ✅ |
| Transaction GET All | ✅ |
| Transaction GET by ID | ✅ |
| Transaction GET Not Found | ✅ |
| Transaction CREATE Transfer | ✅ |
| Transaction Amount Validation | ✅ |
| Transaction Type Validation | ✅ |
| Transfer Business Rules | ✅ |
| Deposit Business Rules | ✅ |
| Withdrawal Business Rules | ✅ |
| Currency Validation | ✅ |
| Currency Normalization | ✅ |
| Audit Log GET | ✅ |
| Audit Log Creation | ✅ |
| Audit Log Data | ✅ |
| Expired JWT | ✅ |

---

# 19. Final Verified Transaction Matrix

| Test | Scenario | Input | Expected | Actual | Result |
|---|---|---|---|---|---|
| T61 | GET all | Valid request | 200 | 200 | ✅ |
| T62 | GET by ID | Existing ID | 200 | 200 | ✅ |
| T63 | GET by ID | Non-existing ID | 404 | 404 | ✅ |
| T64 | Transfer create | Valid | 201 | 201 | ✅ |
| T65 | Transaction audit | Valid transfer | Audit record | Audit record | ✅ |
| T66 | Negative amount | -100 | 400 | 400 | ✅ |
| T67 | Zero amount | 0 | 400 | 400 | ✅ |
| T68 | Invalid type | 999 | 400 | 400 | ✅ |
| T69 | Transfer missing accounts | None | 400 | 400 | ✅ |
| T70 | Same source/destination | Same ID | 400 | 400 | ✅ |
| T71 | Deposit missing destination | None | 400 | 400 | ✅ |
| T72 | Withdrawal missing source | None | 400 | 400 | ✅ |
| T73 | Currency too short | JO | 400 | 400 | ✅ |
| T74 | Empty currency | "" | 400 | 400 | ✅ |
| T75 | Whitespace currency | "   " | 400 | 400 | ✅ |
| T76 | Currency too long | JODX | 400 | 400 | ✅ |
| T77 | Lowercase currency | jod | 201 | 201 + JOD | ✅ |
| T78 | Currency spaces | " JOD " | 201 expected | 400 | ❌ |
| T79 | Exact currency | JOD | 201 | 201 | ✅ |
| T80 | Deposit happy path | Valid | 201 | 201 | ✅ |
| T81 | Withdrawal happy path | Valid | 201 | 201 | ✅ |
| T82 | Deposit audit | Valid deposit | Audit record | Previously verified | 🔹 |
| T83 | Withdrawal audit | Valid withdrawal | Audit record | Previously verified | 🔹 |
| T84 | Transfer source missing | Missing source | 400 | Previously verified | 🔹 |
| T85 | Transfer destination missing | Missing destination | 400 | Previously verified | 🔹 |
| T86 | Transaction authorization | No valid JWT | 401 | Previously verified | 🔹 |

---

# 20. Test Completion Status

## Verified

```text
✅ Validation
✅ Business Rules
✅ Authentication
✅ Authorization behavior
✅ JWT expiration handling
✅ English error messages
✅ Arabic error messages
✅ Centralized error response format
✅ User audit logging
✅ Account audit logging
✅ Transaction audit logging
✅ Transaction creation
✅ Transaction retrieval
✅ Transaction not-found handling
```

## Deferred

```text
⚠️ User Update repository issue
⚠️ User Delete repository issue
⚠️ Duplicate email DB exception during Update
⚠️ Invalid UserId account creation handling
⚠️ Guid.Empty UserId handling
⚠️ Currency surrounding spaces
⚠️ Audit pagination input behavior
⚠️ Swagger malformed GUID client-side limitation
```

---

# 21. Documentation Notes

This file is a record of actual testing performed against the current implementation.

A test marked:

```text
✅ PASS
```

means the observed behavior matched the expected behavior for that scenario.

A test marked:

```text
❌ FAIL
```

means the implementation returned behavior different from the expected behavior used for the test.

A test marked:

```text
⚠️ DEFERRED
```

means an issue was observed but was intentionally left unchanged during the current testing pass.

A test marked:

```text
🔹 PREVIOUSLY VERIFIED
```

means it was already tested and confirmed before this detailed transaction run, but the complete raw response is not preserved in the current test transcript.

---

# 22. Security Note

Never commit:

```text
real JWT tokens
real passwords
database passwords
connection strings containing credentials
secrets
private keys
```

Use placeholders such as:

```text
<ACCESS_TOKEN>
<PASSWORD>
<CONNECTION_STRING>
```

The test documentation should remain safe to push to GitHub.