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
using CoreSavingLibrary.WcfNDeposit;
using System.Web.Services.Protocols;
using DataLibrary;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_membinfo : PageWebSheet, WebSheet
    {
        protected String postAccountNo;
        private n_depositClient depServ;
        protected String testControls;
        private DwThDate thDwMain;
        private DwThDate thDwTab1;
        //private DwThDate thDwTab2;
        private String pblFileName = "kt_50bath.pbl";

        protected Sta ta;

        private void JsPostAccountNo()
        {
            try
            {
                String dwAccNo = DwMain.GetItemString(1, "deptaccount_no");
                String accNo = depServ.BaseFormatAccountNo(state.SsWsPass, dwAccNo);
                object[] argsDwMain = new object[2] { accNo, state.SsCoopId };
                try
                {
                    DwMain.Reset();
                    DwUtil.RetrieveDataWindow(DwMain, pblFileName, thDwMain, argsDwMain);
                }
                catch { }
                try
                {
                    DwTab1.Reset();
                    DwUtil.RetrieveDataWindow(DwTab1, pblFileName, thDwTab1, argsDwMain);
                }
                catch { }
                //try
                //{
                //    DwTab2.Reset();
                //    DwUtil.RetrieveDataWindow(DwTab2, pblFileName, thDwTab2, argsDwMain);
                //}
                //catch { }
                //try
                //{
                //    DwTab3.Reset();
                //    DwUtil.RetrieveDataWindow(DwTab3, pblFileName, null, argsDwMain);
                //}
                //catch { }
                //try
                //{
                //    DwTab4.Reset();
                //    DwUtil.RetrieveDataWindow(DwTab4, pblFileName, null, argsDwMain);
                //}
                //catch { }
                if (DwMain.RowCount < 1)
                {
                    throw new Exception("ไม่พบเลขบัญชีดังกล่าว");
                }
                //String accFormat = depServ.ViewAccountNoFormat(state.SsWsPass, DwMain.GetItemString(1, "deptaccount_no"));
                //DwMain.SetItemString(1, "deptaccount_no", accFormat);
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        #region WebSheet Members

        public void InitJsPostBack()
        {
            postAccountNo = WebUtil.JsPostBack(this, "postAccountNo");
            //----------------------------------------------------------------
            thDwMain = new DwThDate(DwMain, this);
            thDwMain.Add("deptopen_date", "deptopen_tdate");
            thDwMain.Add("lastcalint_date", "lastcalint_tdate");
            thDwMain.Add("member_date", "member_tdate");
            thDwMain.Add("birth_date", "birth_tdate");
            //----------------------------------------------------------------
            thDwTab1 = new DwThDate(DwTab1, this);
            thDwTab1.Add("entry_date", "entry_tdate");
            thDwTab1.Add("operate_date", "operate_tdate");            
            //----------------------------------------------------------------
            //thDwTab2 = new DwThDate(DwTab2, this);
            //thDwTab2.Add("prnc_date", "prnc_tdate");
            //thDwTab2.Add("prncdue_date", "prncdue_tdate");
            //thDwTab2.Add("lastcalint_date", "lastcalint_tdate");
            //----------------------------------------------------------------
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);

            depServ = wcf.Deposit;
            //DwMain.Modify("deptaccount_no.EditMask.Mask='" + depServ.GetDeptCodeMask(state.SsWsPass) + "'");
            if (IsPostBack)
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwTab1);
                //this.RestoreContextDw(DwTab2);
                //this.RestoreContextDw(DwTab3);
                //this.RestoreContextDw(DwTab4);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postAccountNo")
            {
                JsPostAccountNo();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
                decimal deptclose_status, req_status;
                try
                {
                    deptclose_status = DwMain.GetItemDecimal(1, "deptclose_status");
                }
                catch
                {
                    deptclose_status = 0;
                }
                try
                {
                    req_status = Convert.ToDecimal(DwMain.GetItemString(1, "req_status"));
                }
                catch
                {
                    req_status = 0;
                }

                string sqlStr_dp = @"UPDATE  dpdeptmaster
                        SET deptclose_status = " + deptclose_status + @"
                        WHERE deptaccount_no = '" + deptaccount_no + "' AND deptclose_status <> 1";
                ta.Exe(sqlStr_dp);
                
                string sqlStr_asn = @"UPDATE  asnreqmaster
                        SET req_status = " + req_status + @"
                        WHERE deptaccount_no = '" + deptaccount_no + "' AND req_status <> '-11'";
                ta.Exe(sqlStr_asn);

                LtServerMessage.Text = WebUtil.CompleteMessage("เปลี่ยนสถานะเป็นรอบัญชีเรียบร้อยแล้ว");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ" + ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwTab1.PageNavigationBarSettings.Visible = (DwTab1.RowCount > 10);
            //DwTab2.PageNavigationBarSettings.Visible = (DwTab2.RowCount > 10);
            //DwTab3.PageNavigationBarSettings.Visible = (DwTab3.RowCount > 10);
            //DwTab4.PageNavigationBarSettings.Visible = (DwTab4.RowCount > 10);
            if (DwMain.RowCount <= 0)
            {
                DwMain.InsertRow(0);
            }
            DwMain.SaveDataCache();
            DwTab1.SaveDataCache();
            //DwTab2.SaveDataCache();
            //DwTab3.SaveDataCache();
            //DwTab4.SaveDataCache();

            ta.Close();
        }

        #endregion
    }
}