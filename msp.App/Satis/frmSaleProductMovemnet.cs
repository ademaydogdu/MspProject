﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Msp.Service.Repository;

namespace Msp.App.Satis
{
    public partial class frmSaleProductMovemnet : DevExpress.XtraEditors.XtraForm
    {
        Repository _repository;
        public frmSaleProductMovemnet()
        {
            InitializeComponent();
            _repository = new Repository();
        }
    }
}