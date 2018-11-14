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

[CORS](https://stackoverflow.com/q/43767255/1366033)
[Delete All Records](https://stackoverflow.com/q/26326413/1366033)


#### Table Storage

[Table Storage](https://docs.microsoft.com/en-us/azure/cosmos-db/table-storage-how-to-use-dotnet#delete-a-table)
[Table Storage Batch](https://docs.microsoft.com/en-us/azure/visual-studio/vs-storage-aspnet5-getting-started-tables#insert-a-batch-of-entities)
[Table Storage Keys](https://blog.maartenballiauw.be/post/2012/10/08/what-partitionkey-and-rowkey-are-for-in-windows-azure-table-storage.html)
[Post Data w/ Postman](https://stackoverflow.com/a/45213695/1366033)

#### Vue

[Fetching Data](https://www.sitepoint.com/fetching-data-third-party-api-vue-axios/)
[Order Data](https://stackoverflow.com/a/40512856/1366033)