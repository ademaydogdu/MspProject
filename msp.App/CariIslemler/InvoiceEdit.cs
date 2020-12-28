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
using Msp.Infrastructure;
using Msp.App.Tool;
using Msp.Service.Repository;
using Msp.Models.Models;
using Msp.Models.Models.Invoice;
using Msp.Models.Models.Utilities;
using Msp.Service.Service.Invoice;
using Msp.Service.Service.DepotStock;
using Msp.Service.Service.CurrentTransactions;
using Msp.Service.Service.App;
using Msp.Models.Models.Order;
using System.Globalization;

namespace Msp.App.CariIslemler
{
    public partial class InvoiceEdit : DevExpress.XtraEditors.XtraForm
    {
        Repository _repository;
        ParametersDTO _parameters;
        public InvoiceEdit()
        {
            InitializeComponent();
            _repository = new Repository();
            _parameters = new ParametersDTO();
        }
        public InvoiceType invoice;
        public FormOpenType _FormOpenType;
        public int RecId;
        private bool _IsOrders = false;
        MspTool MspTool = new MspTool();
        InvoiceOwnerDTO __dll_InvoiceOwner = new InvoiceOwnerDTO();
        List<InvoiceTransDTO> __dll_List_InoviceTrans = new List<InvoiceTransDTO>();
        List<UnitsDTO> _list_UnitsDTO = new List<UnitsDTO>();
        List<ProductDTO> _productlist = new List<ProductDTO>();
        List<CTransactionsDTO> _currentTransactionsList = new List<CTransactionsDTO>();
        List<CompanyDTO> _company = new List<CompanyDTO>();


        public List<KDVTaxDto> KdvOrani = new List<KDVTaxDto>
        {
            new KDVTaxDto(1, "%0", 0.00, 0),
            new KDVTaxDto(2, "%1", 0.01, 1),
            new KDVTaxDto(3, "%8", 0.08, 8),
            new KDVTaxDto(4, "%18", 0.18, 18),
        };

        #region Record

        public bool do_Validation()
        {
            bool _return = false;
            if (__dll_List_InoviceTrans.Count == 0)
            {
                MessageBox.Show("Boş Satır Kaydedilemez...");
                _return = true;
            }
            if (invoice == InvoiceType.AlisIrsaliye)
            {
                if (__dll_InvoiceOwner.IrsaliyeNo == null || __dll_InvoiceOwner.IrsaliyeNo == "")
                {
                    XtraMessageBox.Show("İrsaliye No Giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _return = true;
                } 
            }
            if (invoice == InvoiceType.AlımFaturası)
            {
                if (__dll_InvoiceOwner.EFaturaNo == null || __dll_InvoiceOwner.EFaturaNo == "")
                {
                    XtraMessageBox.Show("Fatura No Giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _return = true;

                }
            }
            return _return;
        }

        public void do_save()
        {
            try
            {
                if (do_Validation()) return;
                if (MspTool.get_Question("Kaydedilecektir Onaylıyor musunuz?"))
                {
                    var req = new InvoiceSaveRequest()
                    {
                        InvoiceOwner = __dll_InvoiceOwner,
                        InvoiceTrans = __dll_List_InoviceTrans,
                        IsOrder = _IsOrders
                    };
                    var response = _repository.Run<InvoiceService, ActionResponse<InvoiceSaveRequest>>(x => x.Save_Inovice(req));
                    if (response.ResponseType != ResponseType.Ok)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(response.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        this.Close();
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public void do_SiparisSec(int orderOwnerId)
        {
            try
            {
                if (orderOwnerId != 0)
                {
                    var _orderTrans = _repository.Run<InvoiceService, List<OrderTransDTO>>(x => x.Get_orderTrans_Invoice(orderOwnerId));
                    if (_orderTrans.Count > 0)
                    {
                        __dll_InvoiceOwner.OrderId = orderOwnerId;
                        foreach (var item in _orderTrans)
                        {
                            InvoiceTransDTO newTrans = new InvoiceTransDTO();
                            newTrans.ProductId = item.StockId;
                            newTrans.UnitID = item.BirimId;
                            newTrans.BirimFiyat = item.BirimFiyat;

                            __dll_List_InoviceTrans.Add(newTrans);
                        }
                        bs_InvoiceTrans.DataSource = __dll_List_InoviceTrans;
                        gc_invoiceTrans.RefreshDataSource();
                        _IsOrders = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        public void do_IrsaliyeSec(int InvoiceId, string _irsaliyeNo)
        {
            try
            {
                if (InvoiceId != 0)
                {
                    var _irsaliyeTrans = _repository.Run<InvoiceService, List<InvoiceTransDTO>>(x => x.Get_InovicerTranse(InvoiceId));
                    if (_irsaliyeTrans.Count > 0)
                    {
                        __dll_InvoiceOwner.IrsaliyeId = InvoiceId;
                        __dll_InvoiceOwner.IrsaliyeNo = _irsaliyeNo;
                        foreach (var item in _irsaliyeTrans)
                        {
                            InvoiceTransDTO newTrans = new InvoiceTransDTO();
                            newTrans.ProductId = item.ProductId;
                            newTrans.UnitID = item.UnitID;
                            newTrans.BirimFiyat = item.BirimFiyat;
                            __dll_List_InoviceTrans.Add(newTrans);
                        }
                        bs_InvoiceTrans.DataSource = __dll_List_InoviceTrans;
                        gc_invoiceTrans.RefreshDataSource();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion


        #region Form

        private void InvoiceEdit_Load(object sender, EventArgs e)
        {
            MspTool.Get_Layout(this);

            switch (invoice)
            {
                case InvoiceType.AlımFaturası:
                    this.Text = "Alım Faturası";
                    lytSatisFaturasi.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lc_IrsaliyeSaat.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lcgr_FatutaBilgisi.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lc_FaturaTuru.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lc_VadeGun.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lc_VadeTarih.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lc_OdemeTuru.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
                case InvoiceType.SatisFaturasi:
                    this.Text = "Satış Faturası";
                    lytSatisFaturasi.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lc_IrsaliyeSaat.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lcgr_FatutaBilgisi.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lc_FaturaTuru.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lc_VadeGun.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lc_VadeTarih.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lc_OdemeTuru.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;


                    break;
                case InvoiceType.AlisIrsaliye:
                    this.Text = "Alış İrsaliyesi";
                    lc_IrsaliyeSaat.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lcgr_FatutaBilgisi.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lc_FaturaTuru.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lc_VadeGun.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lc_VadeTarih.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lc_OdemeTuru.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                    break;
                case InvoiceType.SatisIrsaliye:
                    this.Text = "Satış İrsaliyesi";
                    lcgr_FatutaBilgisi.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lc_IrsaliyeSaat.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lc_FaturaTuru.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lc_VadeGun.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lc_VadeTarih.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lc_OdemeTuru.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                    break;
                default:
                    break;
            }


            if (_FormOpenType == FormOpenType.New)
            {
                __dll_InvoiceOwner = new InvoiceOwnerDTO();
                __dll_List_InoviceTrans = new List<InvoiceTransDTO>();
                __dll_InvoiceOwner.InvoiceType = (int)invoice;
                __dll_InvoiceOwner.FicDate = DateTime.Now;
                __dll_InvoiceOwner.VadeTarih = DateTime.Now;
            }
            if (_FormOpenType == FormOpenType.Edit)
            {

            }
            if (_FormOpenType == FormOpenType.View)
            {

            }

            _list_UnitsDTO = _repository.Run<DepotStockService, List<UnitsDTO>>(x => x.GetListUnit());
            bs_Unit.DataSource = _list_UnitsDTO;

            _productlist = _repository.Run<DepotStockService, List<ProductDTO>>(x => x.GetListProduct());
            bs_Product.DataSource = _productlist;

            _currentTransactionsList = _repository.Run<CurrentTransactionsService, List<CTransactionsDTO>>(x => x.GetListCurrentTransactions());
            bs_CariHesap.DataSource = _currentTransactionsList;

            _company = _repository.Run<StartUp, List<CompanyDTO>>(x => x.GetList_Company());
            bs_company.DataSource = _company;

            rp_KDV.DataSource = KdvOrani;
            rp_KDV.ValueMember = "Id";
            rp_KDV.DisplayMember = "Value";

            bs_InvoiceOwner.DataSource = __dll_InvoiceOwner;
            bs_InvoiceTrans.DataSource = __dll_List_InoviceTrans;

        }

        private void InvoiceEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void InvoiceEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            MspTool.do_Save_Layout(this);
        }
        #endregion

        private void bbi_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gcv_invoiceTrans.CloseEditor();
            do_save();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void bbi_SiparisCagir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FindOrderList frm = new FindOrderList();
            frm.ShowDialog();
        }

        private void gcv_invoiceTrans_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

            var oRow = (InvoiceTransDTO)gcv_invoiceTrans.GetFocusedRow();
            if (oRow != null)
            {
                if (oRow.ProductId == null) return;
                var product = _productlist.FirstOrDefault(x => x.PID == oRow.ProductId);

                if (e.Column == colProductId)
                {
                    oRow.UnitID = product.PUnitId;
                    oRow.KDV = product.PTax;
                    oRow.BirimFiyat = product.PFirstPrice;
                }
                oRow.Tutar = oRow.Quentity * oRow.BirimFiyat;
                TopTotal();
            }



        }


        private void TopTotal()
        {

            decimal AraToplam = 0;
            decimal BirimFiyatToplam = Math.Round(Convert.ToDecimal(__dll_List_InoviceTrans.Sum(x => x.BirimFiyat)), 2);
            decimal Iskonto = Math.Round((BirimFiyatToplam * Convert.ToDecimal(__dll_InvoiceOwner.Iskonto)) / 100, 2);
            decimal TotalKDV = 0;
            if (Iskonto > 0)
            {
                AraToplam = BirimFiyatToplam - Iskonto;
            }
            else
            {
                AraToplam = BirimFiyatToplam;
            }

            foreach (var item in __dll_List_InoviceTrans)
            {
                decimal Kdv = Math.Round(item.BirimFiyat.GetValueOrDefault() * (decimal)KdvOrani.FirstOrDefault(x => x.Id == (int)item.KDV).TaxOrani, 2);
                TotalKDV += Kdv;
            }


            //__dll_InvoiceOwner.topl = BirimFiyatToplam;
            __dll_InvoiceOwner.Indirim = Iskonto;
            __dll_InvoiceOwner.AraToplam = AraToplam;
            __dll_InvoiceOwner.ToplamKDV = TotalKDV;
            __dll_InvoiceOwner.GenelToplam = AraToplam + TotalKDV;
            txtToplam.EditValue = string.Format(CultureInfo.CreateSpecificCulture("tr-TR"), "{0:C}", BirimFiyatToplam);
            txtIndirim.EditValue = string.Format(CultureInfo.CreateSpecificCulture("tr-TR"), "{0:C}", Iskonto);
            txtAraToplam.EditValue = string.Format(CultureInfo.CreateSpecificCulture("tr-TR"), "{0:C}", AraToplam);
            txtToplamKDV.EditValue = string.Format(CultureInfo.CreateSpecificCulture("tr-TR"), "{0:C}", TotalKDV);
            txtTotal.EditValue = string.Format(CultureInfo.CreateSpecificCulture("tr-TR"), "{0:C}", AraToplam + TotalKDV);

        }

        private void bbi_İrsaliyeCagir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FindIrsaliyeList frm = new FindIrsaliyeList();
            frm.invoice = InvoiceType.AlisIrsaliye;
            frm.ShowDialog();
        }
    }
}