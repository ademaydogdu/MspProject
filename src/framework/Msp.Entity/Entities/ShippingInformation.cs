﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msp.Entity.Entities
{
    [Table("ShippingInformation")]
    public class ShippingInformation
    {
        [Key]
        public int RecId { get; set; }
        public int? AccId { get; set; }
        public string AdressDefinition { get; set; }
        public string Adress { get; set; }
        public string County { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Authorized { get; set; }
        public int? CompanyId { get; set; }
    }
}
