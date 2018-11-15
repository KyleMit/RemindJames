# Remind James

Azure Functions + Azure Table Storage + Twilio API + SPA to send hourly, crowd-sourced reminders to one person imparticular in need of reminding.

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

## Project Setup

1. Add a `local.settings.json` file (contains keys so removed by `.gitignore`)

    ```json
    {
        "IsEncrypted": false,
        "Values": {
            "AzureWebJobsStorage": "", // Azure > Function Apps > Application Settings
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

```bash
dotnet add package Microsoft.Azure.WebJobs.Extensions.Storage
dotnet add package Microsoft.Azure.WebJobs.Extensions.Twilio
```

## Running Functions Locally

```bash
func host start
```

OR

```bash
cd bin\Debug\netcoreapp2.0
func start
```

## Running Website Locally

```bash
cd wwwroot
python -m SimpleHTTPServer 8080
```

## Publishing

[Deploy to Azure using Azure Functions](https://code.visualstudio.com/tutorials/functions-extension/getting-started)

![Azure Extension - Publish](https://i.imgur.com/JmBcCMa.png)

## Resources, Guides, and Troubleshooting

### Azure Functions

* [Deploy to Azure using Azure Functions](https://code.visualstudio.com/tutorials/functions-extension/getting-started)
* [Microsoft Azure Developer: Create Serverless Functions by Mark Heath](https://app.pluralsight.com/library/courses/microsoft-azure-serverless-functions-create/table-of-contents)
* [CORS](https://stackoverflow.com/q/43767255/1366033)
* [Timing Trigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer#trigger---c-example)
* [Twilio Binding](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-twilio#2x-c-example)
* [The current .NET SDK does not support targeting .NET Core 2.1. target .NET Core 2.0](https://stackoverflow.com/q/49171623/1366033)
* [No job functions found. Try making your job classes and methods public](https://stackoverflow.com/q/44643347/1366033)
* [No valid combination of account information found](https://stackoverflow.com/q/13913589/1366033)
* [Where do I get the AzureWebJobsDashboard connection string information?](https://stackoverflow.com/q/27580264/1366033)
* [Reading settings from a Azure Function](https://stackoverflow.com/q/43556311/1366033)
* [Chron Trigger Syntax](https://crontab.guru/#0_*_*_*_*)
* [Custom Domain not showing in Azure Function](https://stackoverflow.com/q/46799060/1366033)


### Azure Portal

* [Build a serverless web app in Azure](https://docs.microsoft.com/en-us/azure/functions/tutorial-static-website-serverless-api-with-database)
* [Custom Domain](https://docs.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-custom-domain)
* [Add your custom domain name using the Azure Active Directory portal](https://docs.microsoft.com/en-us/azure/active-directory/fundamentals/add-custom-domain)
* [Configuring a custom domain name for an Azure cloud service](https://docs.microsoft.com/en-us/azure/cloud-services/cloud-services-custom-domain-name-portal)
* [Best way to host a Single Page Application (SPA) in Microsoft Azure](https://medium.com/medialesson/best-way-to-host-a-single-page-application-spa-in-microsoft-azure-3e70cbd075c3)

### Azure Storage

* [Storage Pricing](https://azure.microsoft.com/en-us/pricing/details/storage/blobs/)
* [Storage Account Overview](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-overview)
* [Static Website Hosting w/ Storage v2](https://azure.microsoft.com/en-us/blog/azure-storage-static-web-hosting-public-preview/)
* [Static website hosting in Azure Storage](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-static-website)

### Azure CDN

* [Use Azure CDN to access blobs with custom domains over HTTPS](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-https-custom-domain-cdn)
* [Integrate an Azure storage account with Azure CDN](https://docs.microsoft.com/en-us/azure/cdn/cdn-create-a-storage-account-with-cdn)
* [Create an Azure CDN](https://docs.microsoft.com/en-us/azure/cdn/cdn-create-new-endpoint)
* [Host your domain in Azure DNS](https://docs.microsoft.com/en-us/azure/dns/dns-delegate-domain-azure-dns)
* [Configure HTTPS on an Azure CDN custom domain](https://docs.microsoft.com/en-us/azure/cdn/cdn-custom-ssl?tabs=option-1-default-enable-https-with-a-cdn-managed-certificate)
* [Azure CDN default document index.html](https://stackoverflow.com/questions/34413300/azure-cdn-default-document-index-html)
* [Control Azure CDN caching behavior with caching rules](https://docs.microsoft.com/en-us/azure/cdn/cdn-caching-rules)
* [Purge Cached Assets](https://docs.microsoft.com/en-us/azure/cdn/cdn-purge-endpoint)

### Twilio

* [How to Send Daily SMS Reminders Using C#, Azure Functions and Twilio](https://www.twilio.com/blog/2017/01/how-to-send-daily-sms-reminders-using-c-azure-functions-and-twilio.html)
* [Can I remove the text “Sent from the Twilio Sandbox Number” In twilio SMS](https://stackoverflow.com/q/27853675/1366033)


### .NET

* [The current .NET SDK does not support targeting .NET Core 2.1. target .NET Core 2.0](https://stackoverflow.com/q/49171623/1366033)

### Visual Studio Code

* [Change keyboard shortcut bindings in Visual Studio Code?](https://stackoverflow.com/a/33791170/1366033)

### Table Storage

* [Table Storage](https://docs.microsoft.com/en-us/azure/cosmos-db/table-storage-how-to-use-dotnet#delete-a-table)
* [Table Storage Batch](https://docs.microsoft.com/en-us/azure/visual-studio/vs-storage-aspnet5-getting-started-tables#insert-a-batch-of-entities)
* [Table Storage Keys](https://blog.maartenballiauw.be/post/2012/10/08/what-partitionkey-and-rowkey-are-for-in-windows-azure-table-storage.html)
* [Post Data w/ Postman](https://stackoverflow.com/a/45213695/1366033)
* [Delete All Records](https://stackoverflow.com/q/26326413/1366033)

### Vue

* [Fetching Data](https://www.sitepoint.com/fetching-data-third-party-api-vue-axios/)
* [Order Data](https://stackoverflow.com/a/40512856/1366033)
* [Reactive Time](https://cushionapp.com/journal/reactive-time-with-vuejs)
* [Create a countdown with vue.js an moment.js](https://stackoverflow.com/q/52048451/1366033)
* [Event Handling `v-on` directive](https://vuejs.org/v2/api/#v-on)
* [Class & Style Binding](https://vuejs.org/v2/guide/class-and-style.html)
* [Conditional Class](https://stackoverflow.com/a/43210564/1366033)
* [Add LocalStorage](https://travishorn.com/add-localstorage-to-your-vue-app-in-2-lines-of-code-56eb2c9f371b)
* [Offline States](https://github.com/filrak/vue-offline)

### LoDash

* [Order Collection](https://lodash.com/docs/4.17.10#orderBy)
* [Find Collection](https://lodash.com/docs/4.17.11#find)

### moment.js

* [Add duration to moment date](https://stackoverflow.com/a/44110303/1366033)

### Boostrap 4

* [Does bootstrap have builtin padding and margin classes?](https://stackoverflow.com/a/46329924/1366033)
* [What is class=“mb-0” in Bootstrap 4?](https://stackoverflow.com/q/41574776/1366033)
* [Spacing Utilities](https://getbootstrap.com/docs/4.1/utilities/spacing/)
* [Sticky Top](https://getbootstrap.com/docs/4.0/utilities/position/#sticky-top)
* [Feedback Icons BS 3](https://stackoverflow.com/a/18839305/1366033)
* [Form Validation](https://getbootstrap.com/docs/4.0/components/forms/#validation)