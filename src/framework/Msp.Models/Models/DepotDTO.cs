﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msp.Models.Models
{
  public  class DepotDTO
    {

        public int DID { get; set; }

        public string DepName { get; set; }
        public string DepAuthPerson { get; set; }

        public string DepAddress { get; set; }

        public string DepCity { get; set; }

        public string DepDistrict { get; set; }

        public string DepPhoneOne { get; set; }
        public string DepPhoneTwo { get; set; }

        public string DepTaxAdministration { get; set; }

        public string DepTaxNo{ get; set; }


        public DateTime? DepDate { get; set; }

        public bool? DepActive { get; set; }

        public int? CompanyRecId { get; set; }


    }
}
