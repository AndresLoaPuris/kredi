using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kredi.Models
{
	public class Temp
	{

        public int id { get; set; }

        public string customer { get; set; }


        public float amount { get; set; }


        public string currency { get; set; }


        public System.DateTime creationDate { get; set; }


        public string rateType { get; set; }


        public string rateTime { get; set; }


        public float rateValue { get; set; }


        public string capitalization { get; set; }


        public int user_id { get; set; }
    }
}