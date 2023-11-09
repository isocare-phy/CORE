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
    public partial class w_sheet_kt_membinfokt : PageWebSheet, WebSheet
    {

        protected String postAccountNo;
        protected String postAssistType;
        private n_depositClient depServ;
        protected String testControls;
        private DwThDate thDwMain;
        private DwThDate thDwTab1;
        private String pblFileName = "kt_50bath.pbl";

        protected Sta ta;

        private void JsPostAccountNo()
        {
            try
            {
                String dwMemNo = DwMain.GetItemString(1, "member_no");
                String dwAssistType = DwMain.GetItemString(1, "assisttype_desc");
                //String accNo = depServ.BaseFormatAccountNo(state.SsWsPass, dwAccNo);
                object[] argsDwMain = new object[3] { dwMemNo, state.SsCoopId, dwAssistType };
                try
                {
                    DwMain.Reset();
                    DwUtil.RetrieveDataWindow(DwMain, pblFileName, thDwMain, argsDwMain);
                }
                catch 
                { 
                }
               string a = DwMain.Describe("DataWindow.Data.XML");
               if (DwMain.RowCount < 1)
               {
                   throw new Exception("ไม่พบข้อมูล");
               }
               try
               {
                   if (DwMain.GetItemString(1, "member_no") == "")
                   {
                       throw new Exception("ไม่พบข้อมูล");
                   }
               }
               catch
               {
                   LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล");
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

        private void JsPostAssistType()
        {
            String dwMemNo ="";
            String dwAssistType = "";
            try
            {
                    dwMemNo = DwMain.GetItemString(1, "member_no");
                    dwAssistType = DwMain.GetItemString(1, "assisttype_desc");

                object[] argsDwMain = new object[3] { dwMemNo, state.SsCoopId, dwAssistType };
                try
                {
                    DwMain.Reset();
                    DwUtil.RetrieveDataWindow(DwMain, pblFileName, thDwMain, argsDwMain);
                }
                catch { }

                if (DwMain.RowCount < 1)
                {
                    throw new Exception("ไม่พบข้อมูล");
                }
                try
                {
                    if (DwMain.GetItemString(1, "member_no") == "")
                    {
                        throw new Exception("ไม่พบข้อมูล");
                    }
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล");
                }
            }
            catch (SoapException ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลสมาชิก");
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลสมาชิก");
            }
        }

        public void InitJsPostBack()
        {
            postAccountNo = WebUtil.JsPostBack(this, "postAccountNo");
            postAssistType = WebUtil.JsPostBack(this, "postAssistType");
            //----------------------------------------------------------------
            thDwMain = new DwThDate(DwMain, this);
            thDwMain.Add("deptopen_date", "deptopen_tdate");
            thDwMain.Add("lastcalint_date", "lastcalint_tdate");
            thDwMain.Add("member_date", "member_tdate");
            thDwMain.Add("birth_date", "birth_tdate");
            //----------------------------------------------------------------
            thDwTab1.Add("entry_date", "entry_tdate");
            thDwTab1.Add("operate_date", "operate_tdate");
            //----------------------------------------------------------------
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);
            depServ = wcf.NDeposit;
            if (IsPostBack)
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postAccountNo")
            {
                JsPostAccountNo();
            }
            if (eventArg == "postAssistType")
            {
                JsPostAssistType();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                decimal deptclose_status = DwMain.GetItemDecimal(1, "deptclose_status");
                decimal deptmonth_status = DwMain.GetItemDecimal(1, "deptmonth_status_1");
                string deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
                if (deptclose_status == 2)
                {
                    try
                    {
                        string sqlStr = @"UPDATE  dpdeptmaster
                               SET deptclose_status = 2
                               WHERE deptaccount_no = '" + deptaccount_no + "' AND deptclose_status <> 1";
                        ta.Exe(sqlStr);
                        LtServerMessage.Text = WebUtil.CompleteMessage("เปลี่ยนสถานะเป็นรอบัญชีเรียบร้อยแล้ว");
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ" + ex);
                    }
                }
                else if (deptclose_status == 0)
                {
                    try
                    {
                        string sqlStr = @"UPDATE  dpdeptmaster
                               SET deptclose_status = 0
                               WHERE deptaccount_no = '" + deptaccount_no + "' AND deptclose_status <> 1";
                        ta.Exe(sqlStr);
                        LtServerMessage.Text = WebUtil.CompleteMessage("เปลี่ยนสถานะเป็นรอบัญชีเรียบร้อยแล้ว");
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ" + ex);
                    }
                }

                if (deptmonth_status == 1)
                {
                    try
                    {
                        //string deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
                        string sqlStr = @"UPDATE  asnreqmaster
                                        SET req_status = 6
                                        WHERE deptaccount_no = '" + deptaccount_no + "'";
                        ta.Exe(sqlStr);
                        sqlStr = @"UPDATE  dpdeptmaster
                                        SET deptmonth_status = 1
                                        WHERE deptaccount_no = '" + deptaccount_no + "'";
                        ta.Exe(sqlStr);
                        LtServerMessage.Text = WebUtil.CompleteMessage("เปลี่ยนสถานะเป็นรอบัญชีเรียบร้อยแล้ว");
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ" + ex);
                    }
                }
                else if (deptmonth_status == 0)
                {
                    try
                    {
                        //string deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
                        string sqlStr = @"UPDATE  asnreqmaster
                                        SET req_status = 1
                                        WHERE deptaccount_no = '" + deptaccount_no + "'";
                        ta.Exe(sqlStr);
                        sqlStr = @"UPDATE  dpdeptmaster
                                        SET deptmonth_status = 0
                                        WHERE deptaccount_no = '" + deptaccount_no + "'";
                        ta.Exe(sqlStr);
                        LtServerMessage.Text = WebUtil.CompleteMessage("เปลี่ยนสถานะเป็นรอบัญชีเรียบร้อยแล้ว");
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ" + ex);
                    }
                }
            }
            catch
            {

            }
        }

        public void WebSheetLoadEnd()
        {
            if (DwMain.RowCount <= 0)
            {
                DwMain.InsertRow(0);
            }
            DwMain.SaveDataCache();

            ta.Close();
        }
    }
}