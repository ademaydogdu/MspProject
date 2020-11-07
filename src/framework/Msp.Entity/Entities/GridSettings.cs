﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msp.Entity.Entities
{
    [Table("GridSetting")]
    public class GridSettings
    {
        [Key]
        public int RecId { get; set; }
        public string UserCode { get; set; }
        public string FormName { get; set; }
        public string GridName { get; set; }
        public byte[] XmlData { get; set; }
        public int ApplicationId { get; set; }
    }
}
