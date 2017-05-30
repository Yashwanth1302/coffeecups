using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeCups.Model
{
   public class TransactionData
    {
        public string Id { get; set; }
        public string Transactionid { get; set; }
        public string Productcode { get; set; }

        public string Qty { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }


        [Newtonsoft.Json.JsonIgnore]
        public string NameDescDisplay { get { return Transactionid.ToString(); } }

        [Newtonsoft.Json.JsonIgnore]
        public string ProductcodeDescDisplay { get { return Productcode.ToString(); } }


        [Newtonsoft.Json.JsonIgnore]
        public string QtyDisplay { get { return Qty.ToString(); } }


    }
}
