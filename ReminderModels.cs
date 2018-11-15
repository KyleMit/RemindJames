using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Globalization;
    
    public class ReminderModel
    {
        public ReminderModel()
        {
        }

        public ReminderModel(string hour, string message)
        {
            Hour = hour;
            Message = message;
        }

        // public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Hour { get; set; }
        public string Message { get; set; }

        public int HourInt => Convert.ToInt32(Hour);
        public int HourSort => HourInt + (HourInt < 6 ? 24 : 0);
        public string HourFormatted => DateTime.ParseExact($"{Hour}:00", "H:mm", CultureInfo.InvariantCulture).ToString("h tt");

    }

    public class ReminderTableEntity : TableEntity
    {
        // public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Hour { get; set; }
        public string Message { get; set; }

    }

    public static class Utilities {
        
        public static DateTime GetEasternDateTime(ILogger log) {
            DateTime timeUtc = DateTime.UtcNow;
            DateTime estTime = DateTime.Now;

            try
            {
                TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                estTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, estZone);
                log.LogInformation($"The date and time are {estTime} {(estZone.IsDaylightSavingTime(estTime) ? estZone.DaylightName : estZone.StandardName)}.");
            }
            catch (TimeZoneNotFoundException)
            {
                log.LogInformation($"The registry does not define the Central Standard Time zone.");
            }                           
            catch (InvalidTimeZoneException)
            {
                log.LogInformation($"Registry data on the Central STandard Time zone has been corrupted.");
            }

            return estTime;
        }

    }




    public static class Mappings {

        public const string PartitionKey = "REMINDER-KEY";

        public static ReminderTableEntity ToTableEntity(this ReminderModel reminder)
        {
            return new ReminderTableEntity()
            {
                PartitionKey = PartitionKey,
                RowKey = reminder.Hour,
                // CreatedTime = reminder.CreatedTime,
                Message = reminder.Message
            };
        }

        public static ReminderModel ToModel(this ReminderTableEntity reminder)
        {
            return new ReminderModel()
            {
                Hour = reminder.RowKey,
                // CreatedTime = reminder.CreatedTime,
                Message = reminder.Message
            };
        }
    }