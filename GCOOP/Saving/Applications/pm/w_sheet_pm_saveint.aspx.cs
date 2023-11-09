using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNPm;

namespace Saving.Applications.pm
{
    public partial class w_sheet_pm_saveint : PageWebSheet, WebSheet
    {
        String pbl = "pm_investment.pbl";
        protected String GetLoan;

        private DwThDate tDwCri;

        n_pmClient svPm;// = wcf.Pm;

        public void InitJsPostBack()
        {
            tDwCri = new DwThDate(DwMain, this);
            tDwCri.Add("s_date", "s_tdate");
            tDwCri.Add("e_date", "e_tdate");
            tDwCri.Add("re_date", "re_tdate");
            GetLoan = WebUtil.JsPostBack(this, "GetLoan");
        }

        public void WebSheetLoadBegin()
        {
            svPm = wcf.NPm;
            this.ConnectSQLCA();
            if (!IsPostBack)
            {
                DwDetail.Reset();
                //DwDetail.InsertRow(0);
                DwMain.Reset();
                DwMain.InsertRow(0);
                DwMain.SetItemDateTime(1, "s_date", state.SsWorkDate);
                DwMain.SetItemDateTime(1, "e_date", state.SsWorkDate);
                DwMain.SetItemDateTime(1, "re_date", state.SsWorkDate);
            }
            else
            {
                this.RestoreContextDw(DwDetail);
                this.RestoreContextDw(DwMain);
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "GetLoan")
            {
                JsGetLoan();
            }
        }

        private void JsGetLoan()
        {
            try
            {
                DateTime s_date = DwMain.GetItemDateTime(1, "s_date");
                DateTime e_date = DwMain.GetItemDateTime(1, "e_date");
                object[] args = new object[2];
                args[0] = s_date;
                args[1] = e_date;
                try
                {
                    DwUtil.RetrieveDataWindow(DwDetail, pbl, null, args);
                    decimal test = DwDetail.GetItemDecimal(1, "prncbal");
                    CalInterest();
                }
                catch { DwDetail.Reset(); }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        public void SaveWebSheet()
        {
           
            string interest_xml = "";
            DateTime operate_date = DateTime.Now;
            DateTime entry_date = DateTime.Now;
            string entry_id = state.SsUsername;
            try
            {
                operate_date = DwMain.GetItemDateTime(1, "re_date");
            }
            catch
            {

            }
            try
            {
                interest_xml = DwDetail.Describe("Datawindow.data.XML");
            }
            catch { }

                try
                {
                    int result = svPm.of_postint_rcv_list(state.SsWsPass, interest_xml, operate_date, entry_date, entry_id);//savecalint
                    if (result == 1)
                    {
                        JsGetLoan();
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
                    }
                }
                catch(Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        public void WebSheetLoadEnd()
        {
            tDwCri.Eng2ThaiAllRow();
            DwDetail.SaveDataCache();
            DwMain.SaveDataCache();
            DwUtil.RetrieveDDDW(DwDetail, "accid_int", pbl, null);
        }

        public void CalInterest()
        {
            string cal_xml = DwDetail.Describe("Datawindow.data.XML");
            try
            {
                //string interest_xml = svPm.CalInterest(state.SsWsPass, cal_xml);
                //try
                //{
                //    DwDetail.Reset();
                //    DwDetail.ImportString(interest_xml,FileSaveAsType.Xml);
                //}
                //catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
    }
}