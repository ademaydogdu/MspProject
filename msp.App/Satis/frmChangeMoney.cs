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
    public partial class frmChangeMoney : DevExpress.XtraEditors.XtraForm
    {
        public frmChangeMoney()
        {
            InitializeComponent();
        }

        private void frmChangeMoney_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}