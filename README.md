# AD UserGen
## Author: Shengwei Zhu
A simple AI-assisted tool designed for onboarding new Active Directory users made with .NET 10.

## Features
- Natural-language onboarding requests parsed into structured fields
- Username format `first initial + last name` with automatic numeric suffix on collision
- Forced password change at first logon
- Passwords are never stored, logged, or included in result objects

## Tech Stack
- C# and .NET 10
- System.DirectoryServices.AccountManagement 
- Google Gemini API
- Gemini 3.1 Flash Lite
- xUnit Framework

## How it works
You type a request in a console and AD UserGen creates a real Active Directory account, with safety checks at every step.

### 1. Type a request
Type a plain-English request, no forms or special syntax.

### 2. AI reads the request
The request is sent to Gemini's Flash Lite AI model, which pulls out four things: first name, last name, department, and the organizational unit the account should go in.

### 3. Request gets validated
All fields typed in the request gets validated and the location is on an approved list. Anything unexpected is rejected.

### 4. Username and password are created
The username and password gets created. The username has a fixed naming rule and is checked against Active Directory to prevent duplicates. The password is generated locally by a secure random generator. The AI never sees it.

### 5. Account gets created
After making the request and confirming all the details filled, the account is created with a forced password change at first logon.

The AI:
- will only read text and fills in the required fields.
- never creates passwords; the generator will.
- never communicates with Active Directory.
- has its output validated.

## License
MIT
