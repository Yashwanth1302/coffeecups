using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeCups.Model
{
   public class AddData
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Qty { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }
        
       
        [Newtonsoft.Json.JsonIgnore]
        public string NameDescDisplay { get { return Name.ToString(); } }

        [Newtonsoft.Json.JsonIgnore]
        public string QtyDisplay { get { return Qty.ToString(); } }


    }
}
