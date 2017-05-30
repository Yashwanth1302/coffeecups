using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeCups.Model
{
    public class TestingData
    {

        public string Id { get; set; }
        public string SkuCode { get; set; }

        public string ShortDesc { get; set; }

        public string SkuDesc { get; set; }
        public string BarCode { get; set; }
        public string MRP { get; set; }
        public string CatgCode { get; set; }
        public string CatgDesc { get; set; }
        public string DeptCode { get; set; }
        public string DeptDesc { get; set; }
      


        //public string Id { get; set; }
        //public DateTime DateUtc { get; set; }

        //public bool MadeAtHome { get; set; }

        //public string OS { get; set; }





        //[Newtonsoft.Json.JsonProperty("Id")]
        //public string Id { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        //[Newtonsoft.Json.JsonProperty("userId")]
        //public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the date UTC.
        /// </summary>
        /// <value>The date UTC.</value>
        //  public DateTime DateUtc { get; set;}

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CoffeeCups.CupOfCoffee"/> made at home.
        /// </summary>
        /// <value><c>true</c> if made at home; otherwise, <c>false</c>.</value>
        // public bool MadeAtHome{ get; set; }

        /// <summary>
        /// Gets or sets the OS of the user
        /// </summary>
        /// <value>The OS</value>
        //  public string OS { get; set; }

        //[Newtonsoft.Json.JsonIgnore]
        //public string DateDisplay { get { return DateUtc.ToLocalTime().ToString("d"); } }

        //[Newtonsoft.Json.JsonIgnore]
        //public string TimeDisplay { get { return DateUtc.ToLocalTime().ToString("t") + " " + OS.ToString(); } }

        //[Newtonsoft.Json.JsonIgnore]
        //public string AtHomeDisplay { get { return MadeAtHome ? "Made At Home" : string.Empty; } }

        [Newtonsoft.Json.JsonIgnore]
        public string DateDisplay { get { return SkuCode.ToString(); } }

        //[Newtonsoft.Json.JsonIgnore]
        //public string TimeDisplay { get { return DateUtc.ToLocalTime().ToString("t") + " " + OS.ToString(); } }

        [Newtonsoft.Json.JsonIgnore]
        public string AtHomeDisplay { get { return ShortDesc.ToString(); } }

        [Newtonsoft.Json.JsonIgnore]
        public string SkuDescDisplay { get { return SkuDesc.ToString(); } }
        [Newtonsoft.Json.JsonIgnore]
        public string BarCodeDisplay { get { return BarCode.ToString(); } }
        [Newtonsoft.Json.JsonIgnore]
        public string MRPDisplay { get { return MRP.ToString(); } }
        [Newtonsoft.Json.JsonIgnore]
        public string CatgCodeDisplay { get { return CatgCode.ToString(); } }
        [Newtonsoft.Json.JsonIgnore]

        public string CatgDescDisplay { get { return CatgDesc.ToString(); } }
        [Newtonsoft.Json.JsonIgnore]
        public string DeptCodeDisplay { get { return DeptCode.ToString(); } }
        [Newtonsoft.Json.JsonIgnore]
        public string DeptDescDisplay { get { return DeptDesc.ToString(); } }


    }
}
