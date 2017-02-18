using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace ukrpostbot.Models
{
    public class ReviewModel : TableEntity
    {
        public ReviewModel(string city, string department)
        {
            City = city;
            Department = department;
            this.PartitionKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", city, department);
            this.RowKey = Guid.NewGuid().ToString();
        }
        public string City { get; set; }
        public string Department { get; set; }
        public string TypeOfProblem { get; set; }
        public string Description { get; set; }
    }
}