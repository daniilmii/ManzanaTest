using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckServiceWCF.Entities
{
    public class CheckEntity:CheckBaseType
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public decimal Summ { get; set; }
        public decimal Discount { get; set; }
        public string[] Articles { get; set; }
    }
}