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

namespace Msp.App.Satis
{
    public partial class frmSatisList : DevExpress.XtraEditors.XtraForm
    {
        public frmSatisList()
        {
            InitializeComponent();
        }

        private void frmSatisList_Load(object sender, EventArgs e)
        {
            dt_Date.EditValue = DateTime.Today;
        }






    }
}