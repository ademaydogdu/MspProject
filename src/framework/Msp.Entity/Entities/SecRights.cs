﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msp.Entity.Entities
{
    [Table("SecRights")]
    public class SecRights
    {
        [Key]
        public int RecId { get; set; }
        public string UserCode { get; set; }
        public string SecCode { get; set; }
        public int? SecPreview { get; set; }
        public int? SecUpdate { get; set; }
        public int? SecInsert { get; set; }
        public int? SecDelete { get; set; }
        public int? DocumentType { get; set; }
        public string CompanyCode { get; set; }
        public int? CompanyRecId { get; set; }
    }
}
