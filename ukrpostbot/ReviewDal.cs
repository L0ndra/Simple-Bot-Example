using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ukrpostbot.Models;

namespace ukrpostbot
{
    public static class ReviewDal
    {
        private static readonly Lazy<CloudTableClient> TableClient = new Lazy<CloudTableClient>(() =>
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            return storageAccount.CreateCloudTableClient();
        }, LazyThreadSafetyMode.PublicationOnly);

        private const string TableName = "Reviews";

        public static async Task AddNewReviewToTable(ReviewModel model)
        {
            // Retrieve a reference to the table.
            CloudTable table = TableClient.Value.GetTableReference(TableName);
            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            await table.ExecuteAsync(TableOperation.Insert(model));
        }

    }
}