using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System.Collections.Generic;
using System.Linq;

namespace RemindJames
{
    public static class ReminderAPI
    {
        [FunctionName("GetReminders")]
        public static async Task<IActionResult> GetReminders(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "reminder")] HttpRequest req,
            [Table("reminders", Connection = "AzureWebJobsStorage")] CloudTable remindersTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var query = new TableQuery<ReminderTableEntity>();
            var segment = await remindersTable.ExecuteQuerySegmentedAsync(query, null);

            var result = segment.Select(Mappings.ToModel).OrderBy(x=>x.HourInt);

            return new OkObjectResult(result);

        }

        [FunctionName("GetReminder")]
        public static IActionResult GetReminder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "reminder/{id}")]HttpRequest req,
            [Table("reminders", Mappings.PartitionKey, "{id}", Connection = "AzureWebJobsStorage")] ReminderTableEntity reminder,
            TraceWriter log, string id)
        {
            log.Info("Getting todo item by id");

            if (reminder == null)
            {
                log.Info($"Reminder for hour {id} not found");
                return new NotFoundResult();
            }
            return new OkObjectResult(reminder.ToModel());
        }

        [FunctionName("CreateReminder")]
        public static async Task<IActionResult> CreateReminder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "reminder")]HttpRequest req,
            [Table("reminders", Connection = "AzureWebJobsStorage")] IAsyncCollector<ReminderTableEntity> reminderTable,
            TraceWriter log)
        {
            log.Info("Creating a new todo list item");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<ReminderModel>(requestBody);
            // need to sanitize?

            
            await reminderTable.AddAsync(input.ToTableEntity());
            return new OkObjectResult(input);
        }


        [FunctionName("Up")]
        public static async Task<IActionResult> Up(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "reset")]HttpRequest req,
            [Table("reminders", Connection = "AzureWebJobsStorage")] CloudTable remindersTable,
            TraceWriter log)
        {
            // batch operations - https://stackoverflow.com/a/53293614/1366033
            var query = new TableQuery<ReminderTableEntity>();
            var result = await remindersTable.ExecuteQuerySegmentedAsync(query, null);
            
            // Create the batch operation.
            TableBatchOperation batchDeleteOperation = new TableBatchOperation();

            foreach (var row in result)
            {
                batchDeleteOperation.Delete(row);
            }
            
            // Execute the batch operation.
            await remindersTable.ExecuteBatchAsync(batchDeleteOperation);

            

            // Create the batch operation.
            TableBatchOperation batchInsertOperation = new TableBatchOperation();

            ReminderModel[] defaultRecords = new [] {
                new ReminderModel("8", "Wake Up"),
                new ReminderModel("9", "Wake Up Now!"),
                new ReminderModel("10", "Food, Teeth, Wallet, Check"),
                new ReminderModel("11", "Survey Says..... go to school"),
                new ReminderModel("12", "Now What!?!?!"),
                new ReminderModel("13", "Shoot Mah Goot!!"),
                new ReminderModel("14", "Sometimes you play your cards, sometimes you don't"),
                new ReminderModel("15", "Frida Says 'Hi'"),
                new ReminderModel("16", "Enjoy a small break"),
                new ReminderModel("17", "Do your homework"),
                new ReminderModel("18", "Do your homework"),
                new ReminderModel("19", "Call gram/the fam and say hi"),
                new ReminderModel("20", "Did you eat dinner yet?"),
                new ReminderModel("21", "Screen's Off bud"),
                new ReminderModel("22", "Seriously, go to bed"),
            };

            foreach (var rec in defaultRecords)
            {
                batchInsertOperation.Insert(rec.ToTableEntity());
            }
            
            // Execute the batch operation.
            await remindersTable.ExecuteBatchAsync(batchInsertOperation);

            return new OkResult();
        }
    }
}
