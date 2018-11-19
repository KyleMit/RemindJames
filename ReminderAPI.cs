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
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

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
            var query = new TableQuery<ReminderTableEntity>();
            var segment = await remindersTable.ExecuteQuerySegmentedAsync(query, null);

            var result = segment.Select(Mappings.ToModel)
                        .OrderBy(x=> x.HourSort);

            log.LogInformation($"Get All Reminders returned {result?.Count() ?? 0} record(s)");

            return new OkObjectResult(result);

        }


        [FunctionName("GetReminderCurrent")]
        public static async Task<IActionResult> GetReminderCurrent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "remindercurrent")]HttpRequest req,
            [Table("reminders", Connection = "AzureWebJobsStorage")] CloudTable reminderTable,
            ILogger log)
        {
            string hour = Utilities.GetEasternDateTime(log).AddMinutes(5).Hour.ToString();

            var findOperation = TableOperation.Retrieve<ReminderTableEntity>(Mappings.PartitionKey, hour);
            var findResult = await reminderTable.ExecuteAsync(findOperation);

            if (findResult.Result == null)
            {
                log.LogInformation($"Get reminder not found for hour {hour}");
                return new NotFoundResult();
            }

            var existingRow = (ReminderTableEntity)findResult.Result;
            
            log.LogInformation($"Getting current reminder found for hour {hour} with message {existingRow.Message}");

            return new OkObjectResult(existingRow.ToModel());
        }

        [FunctionName("GetReminderByHour")]
        public static IActionResult GetReminderByHour(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "reminder/{hour}")]HttpRequest req,
            [Table("reminders", Mappings.PartitionKey, "{hour}", Connection = "AzureWebJobsStorage")] ReminderTableEntity reminder,
            ILogger log, string hour)
        {
            if (reminder == null)
            {
                log.LogInformation($"Get reminder not found for hour {hour}");
                return new NotFoundResult();
            }

            log.LogInformation($"Getting reminder found by {hour} with message {reminder.Message}");

            return new OkObjectResult(reminder.ToModel());
        }

        
     
        [FunctionName("CreateReminder")]
        public static async Task<IActionResult> CreateReminder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "reminder")]HttpRequest req,
            [Table("reminders", Connection = "AzureWebJobsStorage")] IAsyncCollector<ReminderTableEntity> reminderTable,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<ReminderModel>(requestBody);
            // need to sanitize?

            await reminderTable.AddAsync(input.ToTableEntity());

            log.LogInformation($"Creating Reminder for hour {input.Hour} with message {input.Message}");

            return new OkObjectResult(input);
        }

        [FunctionName("UpdateReminder")]
        public static async Task<IActionResult> UpdateReminder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "reminder/{hour}")]HttpRequest req,
            [Table("reminders", Connection = "AzureWebJobsStorage")] CloudTable reminderTable,
            ILogger log, string hour)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedReminder = JsonConvert.DeserializeObject<ReminderModel>(requestBody);

            // get row by id
            var findOperation = TableOperation.Retrieve<ReminderTableEntity>(Mappings.PartitionKey, hour);
            var findResult = await reminderTable.ExecuteAsync(findOperation);
            
            // check if not found
            if (findResult.Result == null) { return new NotFoundResult();  }

            // update existing reminder
            var existingReminder = (ReminderTableEntity)findResult.Result;
            existingReminder.Message = updatedReminder.Message;
            
            // replace in table
            var replaceOperation = TableOperation.Replace(existingReminder);
            await reminderTable.ExecuteAsync(replaceOperation);

            log.LogInformation($"Updating reminder hour {updatedReminder.Hour} with message {updatedReminder.Message}");

            return new OkObjectResult(existingReminder.ToModel());
        }


        [FunctionName("Up")]
        public static async Task<IActionResult> Up(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "reset")]HttpRequest req,
            [Table("reminders", Connection = "AzureWebJobsStorage")] CloudTable remindersTable,
            ILogger log)
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

            log.LogInformation($"Resetting entire table and adding {defaultRecords?.Count() ?? 0} record(s)");

            return new OkResult();
        }
    }

    
    public static class ReminderFuncs {

     
        [FunctionName("SendReminder")]
        public static async Task SendReminder(
            [TimerTrigger("0 0 * * * *")]TimerInfo myTimer,
            [Table("reminders", Connection = "AzureWebJobsStorage")] CloudTable reminderTable,
            [TwilioSms(AccountSidSetting = "TwilioAccountSid",AuthTokenSetting = "TwilioAuthToken", From = "+16177670668")] IAsyncCollector<CreateMessageOptions> messages,
            ILogger log)
        {
            // get id (hour)
            string hour = Utilities.GetEasternDateTime(log).AddMinutes(5).Hour.ToString();

            // get row by id
            var findOperation = TableOperation.Retrieve<ReminderTableEntity>(Mappings.PartitionKey, hour);
            var findResult = await reminderTable.ExecuteAsync(findOperation);

            // check if not found
            if (findResult.Result == null) { return; }

            // grab current text
            var existingRow = (ReminderTableEntity)findResult.Result;
            var message = existingRow.Message;

            // clear current reminder text
            var existingReminder = (ReminderTableEntity)findResult.Result;
            existingReminder.Message = "";
            
            // update existing reminder
            var replaceOperation = TableOperation.Replace(existingReminder);
            await reminderTable.ExecuteAsync(replaceOperation);

           
            // create sms message
            var reminderPhone = Environment.GetEnvironmentVariable("ReminderNumber");
            var smsMessage = new CreateMessageOptions(new PhoneNumber(reminderPhone))
            {
                Body = message
            };
            
            await messages.AddAsync(smsMessage);
            
            log.LogInformation($"Sending reminder for hour {hour} with message {message}");
        }

        [FunctionName("InvokeMessage")]
        public static async Task InvokeMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "InvokeMessage")] HttpRequest req,
            [Table("reminders", Connection = "AzureWebJobsStorage")] CloudTable reminderTable,
            [TwilioSms(AccountSidSetting = "TwilioAccountSid",AuthTokenSetting = "TwilioAuthToken", From = "+16177670668")] IAsyncCollector<CreateMessageOptions> messages,
            ILogger log)
        {

            string message = req.Query["message"];
            string hour = req.Query["hour"];

            if (message == null) {

                if (hour == null) {
                    hour = Utilities.GetEasternDateTime(log).AddMinutes(5).Hour.ToString();
                }

                var findOperation = TableOperation.Retrieve<ReminderTableEntity>(Mappings.PartitionKey, hour);
                var findResult = await reminderTable.ExecuteAsync(findOperation);
                if (findResult.Result == null)
                {
                    return;
                }

                var existingRow = (ReminderTableEntity)findResult.Result;

                message = existingRow.Message;
            }
           

            var reminderPhone = Environment.GetEnvironmentVariable("ReminderNumber");
            var smsMessage = new CreateMessageOptions(new PhoneNumber(reminderPhone))
            {
                Body = message
            };
            
            
            await messages.AddAsync(smsMessage);

            log.LogInformation($"Invoking reminder for hour {hour} with message {message}");

        }
              
    }
}
