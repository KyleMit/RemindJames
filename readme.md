# Remind James

Azure Functions + Azure Table Storage + Twilio API + SPA to send daily crowd-sourced reminders every hour on the hour to one person imparticular in need of reminding.

## Project Setup

[Deploy to Azure using Azure Functions](https://code.visualstudio.com/tutorials/functions-extension/getting-started)

### Azure Functions

```bash
brew tap azure/functions
brew install azure-functions-core-tools
```

```bash
dotnet add package Microsoft.Azure.WebJobs.Extensions.Storage
```