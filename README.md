# AngelOne SmartApi Client Library for .NET Core

## Source Code:
https://github.com/Sen-Gupta/AngelOne.SmartApi

## SmartApi Configuration

A client library demonstrating usage of the AngelOne SmartApi and WebSocket V2. 


## Prerequisites
    Latest .Net Core SDK 8.0 or above
    Visual Studio 2022 Community or above
    VS Code Latest


## Configuration
The SmartApiConfiguration section in the appsettings.json in the project AngelOne.SmartApi.Client.Sample.csproj includes essential credentials for authentication and authorization when interacting with the SmartApi.
```
"SmartApi": {
  "Credentials": {
    "ClientCode": "YourClientCode",
    "ClientPIN": "YourClientPIN",
    "TOTPCode": "YourTOTPCode",
    "APIKey": "YourAPIKey"
  }
}
```

## Credentials

ClientCode: The client code is your Angel Broking account's Client ID.
Example: "ClientCode": "DELL2023"

ClientPIN: The client PIN is angel broking account PIN.
Example: "ClientPIN": "2023"


TOTPCode: The TOTP (Time-based One-Time Password) code is used for two-factor authentication. 
It is time-sensitive and provides an additional layer of security. Watch https://www.youtube.com/watch?v=iJgfGk1uDhA 

Note: You just need need to scan the QR code from your phone camera and it will show you a code like as shown below. 
There is no need to use any kind of Authenticator app as we are going to use this with API.

Example: "TOTPCode": "2EXXXJNXYERUCPLAATDRTUHSWM"


APIKey: The API key is a secure token that serves as a unique identifier for API requests. It is used for authorization when accessing SmartApi services.
Plesae generate API key from https://smartapi.angelbroking.com/. Watch: https://www.youtube.com/watch?v=VWjwJbVw2rQ.

Example: "APIKey": "u3aTAXXX"


Make sure to keep sensitive information such as client codes, client PINs, and API keys secure. Avoid sharing them in public repositories or environments.# SmartApi

Once you have configured the SmartApi, with your credentials, you can run the application.



## Running the application
The application is divided into a library and a console application.

Just run the application using your IDE or from the command line using the following command:

In Visual Studio 2022 Community or above choose AngelOne.SmartApi.Client.Sample.csproj and run the application.

In VS Code choose AngelOne.SmartApi.Client.Sample.csproj and run the application. Using Command line dotnet run



## Nuget Package
Add the following Nuget package to your project:

```
dotnet add package AngelOne.SmartApi.Clients --version 2.0.0
```
Ensur the you update the appsettings .json with your credentials as shown above. Or look at the sample project.
