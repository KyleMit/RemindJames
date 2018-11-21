# Remind James

Azure Functions + Azure Table Storage + Azure Blob Storage + Azure CDN + Twilio API + Vue SPA to send hourly, crowd-sourced, text-based reminders, updated simultaneously by everyone in the world, to one person imparticular in need of reminding.

## Resources

View the [resources.md](https://github.com/KyleMit/RemindJames/blob/master/resources.md) for some of the tutorials, quickstarts, documentation, and troubleshooting used in working on this project.

## SaaS Providers

* [Azure](https://portal.azure.com/)
* [Twilio](https://www.twilio.com/console/)

## Tools

* [**Azure Storage Explorer**](https://azure.microsoft.com/en-us/features/storage-explorer/)
* [**Postman**](https://www.getpostman.com/)
* [**Visual Studio Code**](https://code.visualstudio.com/)  
 `.vscode/extensions.json` will reccomend two extensions:  
  * [ms-azuretools.vscode-azurefunctions](https://github.com/Microsoft/vscode-azurefunctions)
  * [ms-vscode.csharp](https://github.com/OmniSharp/omnisharp-vscode)
* [**Vue Dev Tools**](https://github.com/vuejs/vue-devtools)

## Project Setup

1. Add a `local.settings.json` file (contains keys so removed by `.gitignore`)

    ```js
    {
        "IsEncrypted": false,
        "Values": {
            "AzureWebJobsStorage": "", // Azure > Function Apps > Application Settings
            "AzureSignalRConnectionString": "", // Azure > Signal R > Keys
            "TwilioAccountSid": "",    //https://www.twilio.com/console
            "TwilioAuthToken": "",     //https://www.twilio.com/console
            "TwilioPhoneNumber": "",   //https://www.twilio.com/console/phone-numbers/incoming
            "FUNCTIONS_WORKER_RUNTIME": "dotnet"
        },
        "Host": {
            "CORS": "*"
        }
    }
    ```

### SDK Dependencies

[Downlad and install the SDK for .NET Core 2.1.0+](https://www.microsoft.com/net/download/dotnet-core/2.1)

### CLI Dependencies

**macOS** - Use [Homebrew](https://brew.sh/)

```bash
brew tap azure/functions
brew install azure-functions-core-tools
```

**Windows** - Use [npm](https://nodejs.org/en/)

```bash
npm install -g azure-functions-core-tools@core
```

### Package Dependencies

* [**Extensions.Storage**](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.Storage)
* [**Extensions.Twilio**](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.Twilio)
* [**Extensions.SignalRService**](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.SignalRService)

Install via

```bash
dotnet add package Microsoft.Azure.WebJobs.Extensions.Storage
dotnet add package Microsoft.Azure.WebJobs.Extensions.Twilio
dotnet add package Microsoft.Azure.WebJobs.Extensions.SignalRService -v 1.0.0-preview1-10025
```

## Run Locally

### Running Functions

```bash
dotnet restore
dotnet build
func start
```

### Running Website

```bash
cd wwwroot
python -m SimpleHTTPServer 8002
```

## Publishing

### Azure Functions

1. [Deploy to Azure using Azure Functions](https://code.visualstudio.com/tutorials/functions-extension/getting-started)  

     ![Azure Extension - Publish](https://i.imgur.com/JmBcCMa.png)
2. Copy any params from `local.settings.json` to Functions > Settings  

    ![Azure Functions - App Settings](https://i.imgur.com/UVHASIj.png)

### Website

1. Add files to blob storage container
   * [https://remindjames.blob.core.windows.net/](https://remindjames.blob.core.windows.net/)
2. Cache bust the CDN
   * [https://remindjames.azureedge.net/](https://remindjames.azureedge.net/)
3. Should be visible on Custom Domain
   * [https://www.remindjames.com/index.html](https://www.remindjames.com/index.html)
