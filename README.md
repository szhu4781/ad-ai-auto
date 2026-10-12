# Active Directory User Provisioner
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