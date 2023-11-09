using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using DataLibrary;
using System.Globalization;
using System.Data.OracleClient;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNCommon;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmd_ptdealermaster : PageWebSheet,WebSheet
    {
        protected string jsPostDealer;

        Sdt dt = new Sdt();
        string pbl = "cmd_dealermaster.pbl";

        public void InitJsPostBack()
        {
            jsPostDealer = WebUtil.JsPostBack(this, "jsPostDealer");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                Reset();
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostDealer":
                    PostDealer();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            string dealer_no = "", dealer_name = "", dealer_addr = "";
            string dealer_fax = "", dealer_email = "", dealer_mobilephone = "";
            string dealer_contractno1 = "", contract_phone1 = "", contract_email1 = "";
            string dealer_contractno2 = "", contract_phone2 = "", contract_email2 = "";
            string dealer_taxid = "", dealer_phone = "", status_sql = "";
            decimal dealer_status = 0;
            Sdt dt = new Sdt();
            try
            {
                dealer_no = DwMain.GetItemString(1, "dealer_no");
                try { dealer_name = DwMain.GetItemString(1, "dealer_name"); }
                catch { dealer_name = string.Empty; }
                try { dealer_addr = DwMain.GetItemString(1, "dealer_addr"); }
                catch { dealer_addr = string.Empty; }
                try { dealer_taxid = DwMain.GetItemString(1, "dealer_taxid"); }
                catch { dealer_taxid = string.Empty; }
                try { dealer_phone = DwMain.GetItemString(1, "dealer_phone"); }
                catch { dealer_phone = string.Empty; }
                try { dealer_fax = DwMain.GetItemString(1, "dealer_fax"); }
                catch { dealer_fax = string.Empty; }
                try { dealer_email = DwMain.GetItemString(1, "dealer_email"); }
                catch { dealer_email = string.Empty; }
                try { dealer_mobilephone = DwMain.GetItemString(1, "dealer_mobilephone"); }
                catch { dealer_mobilephone = string.Empty; }
                try { dealer_contractno1 = DwMain.GetItemString(1, "dealer_contractno1"); }
                catch { dealer_contractno1 = string.Empty; }
                try { contract_phone1 = DwMain.GetItemString (1, "contract_phone1"); }
                catch { contract_phone1 = string.Empty; }
                try { contract_email1 = DwMain.GetItemString(1, "contract_email1"); }
                catch { contract_email1 = string.Empty; }
                try { dealer_contractno2 = DwMain.GetItemString(1, "dealer_contractno2"); }
                catch { dealer_contractno2 = string.Empty; }
                try { contract_phone2 = DwMain.GetItemString(1, "contract_phone2"); }
                catch { contract_phone2 = string.Empty; }
                try { contract_email2 = DwMain.GetItemString(1, "contract_email2"); }
                catch { contract_email2 = string.Empty; }
                try { dealer_status = DwMain.GetItemDecimal(1, "dealer_status"); }
                catch { dealer_status = 0; }

                
                if (dealer_no == "AUTO") //new dealer for insert data
                {
                    ///เช็คความยาวเบอร์ต่างๆ
                    if (dealer_phone.Length > 50)
                    { throw new Exception("จำนวนเบอร์โทรศัพท์ยาวเกินไป กรุณาตรวจสอบ !!"); }
                    if (dealer_fax.Length > 50)
                    { throw new Exception("จำนวนเบอร์แฟกซ์เกินไป กรุณาตรวจสอบ !!"); }
                    if (dealer_mobilephone.Length > 50)
                    { throw new Exception("จำนวนเบอร์โทรศัพท์เคลื่อนที่ยาวเกินไป กรุณาตรวจสอบ !!"); }
                    ///เช็คเลขที่เสียภาษี
                    decimal rechecktaxno = CheckIdDealer(dealer_taxid);
                    if (rechecktaxno == 1)
                    {
                        n_commonClient com = wcf.NCommon;
                        dealer_no = com.of_getnewdocno(state.SsWsPass, state.SsCoopControl, "CMDDEALER");
                        DwMain.SetItemString(1, "dealer_no", dealer_no);
                        string sqlindealer = @"insert into ptucfdealer 
                                                (dealer_no, dealer_name, dealer_addr, dealer_taxid, dealer_phone, dealer_fax,
                                                dealer_email, dealer_mobilephone, dealer_contractno1, dealer_contractno2, dealer_status,
                                                contract_phone1, contract_email1, contract_phone2, contract_email2) 
                                                values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14})";
                        sqlindealer = WebUtil.SQLFormat(sqlindealer, dealer_no, dealer_name, dealer_addr, dealer_taxid, dealer_phone, dealer_fax,
                                                        dealer_email, dealer_mobilephone, dealer_contractno1, dealer_contractno2, dealer_status,
                                                        contract_phone1, contract_email1, contract_phone2, contract_email2);
                        dt = WebUtil.QuerySdt(sqlindealer);
                        status_sql = "I";
                    }
                    else
                    {
                        throw new Exception("เลขที่ผู้เสียภาษี ซ้ำ กรุณาตรวจสอบ !!!");
                    }
                }
                else //older dealer for update data
                {
                    string sqlupdealer = @"update ptucfdealer set dealer_name = {1}, dealer_addr = {2}, 
                        dealer_taxid = {3}, dealer_phone = {4}, dealer_fax = {5}, dealer_email = {6},
                        dealer_mobilephone = {7}, dealer_contractno1 = {8}, dealer_contractno2 = {9}, 
                        dealer_status = {10}, contract_phone1 = {11}, contract_email1 = {12}, 
                        contract_phone2 = {13}, contract_email2 = {14} where dealer_no = {0}";
                    sqlupdealer = WebUtil.SQLFormat(sqlupdealer, dealer_no, dealer_name, dealer_addr, dealer_taxid, dealer_phone, dealer_fax,
                                                    dealer_email, dealer_mobilephone, dealer_contractno1, dealer_contractno2, dealer_status,
                                                    contract_phone1, contract_email1, contract_phone2, contract_email2);
                    dt = WebUtil.QuerySdt(sqlupdealer);
                    status_sql = "U";
                }
                if (status_sql == "I")
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกทะเบียนคู่ค้าเลขที่ " + dealer_no + " สำเร็จ");
                    Reset();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("ปรับปรุงทะเบียนคู่ค้าเลขที่ " + dealer_no + " สำเร็จ");
                    Reset();
                }
                
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }

        private void PostDealer()
        {
            try
            {
                string dealer_no = DwMain.GetItemString(1, "dealer_no");
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, dealer_no);
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        public void Reset()
        {
            DwMain.Reset();
            DwMain.InsertRow(0);
            DwMain.SetItemString(1, "dealer_no", "AUTO");
            DwMain.SetItemDecimal(1, "dealer_status", 0);
        }

        private decimal CheckIdDealer(string dealerTaxno)
        {
            decimal checkstatus = 9, cdealertax = 0;
            string se = @"select count(dealer_taxid) as cdealertax from ptucfdealer where dealer_taxid = {0}";
            se = WebUtil.SQLFormat(se, dealerTaxno);
            dt = WebUtil.QuerySdt(se);
            if (dt.Next())
            {
                cdealertax = dt.GetDecimal("cdealertax");
                if (cdealertax > 0)
                { 
                    checkstatus = 9; 
                }
                else
                {
                    checkstatus = 1;
                }
            }
            return checkstatus;
        }
    }
}