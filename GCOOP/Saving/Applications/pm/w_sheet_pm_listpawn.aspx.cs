using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNPm;
using Sybase.DataWindow;

namespace Saving.Applications.pm
{
    public partial class w_sheet_pm_listpawn : PageWebSheet, WebSheet
    {
        String pbl = "pm_investment.pbl";
        private n_pmClient PmService;
        protected String GetLoan;
        protected String DelMainRow;
        protected String JsPostDate;
        protected String JsWarrantyChange;
        private DwThDate tDwMain;
        public void InitJsPostBack()
        {
            GetLoan = WebUtil.JsPostBack(this, "GetLoan");
            DelMainRow = WebUtil.JsPostBack(this, "DelMainRow");
            JsPostDate = WebUtil.JsPostBack(this, "JsPostDate");
            JsWarrantyChange = WebUtil.JsPostBack(this, "JsWarrantyChange");

            tDwMain = new DwThDate(DwDetail, this);
            tDwMain.Add("warranty_date", "warranty_tdate");
            tDwMain.Add("revoke_date", "revoke_tdate");
        }

        public void WebSheetLoadBegin()
        {
            PmService = wcf.NPm;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                //DwDetail.InsertRow(0);
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
            else if (eventArg == "DelMainRow")
            {
                DelRow();
            }
            else if (eventArg == "JsPostDate")
            {
                PostDateNextRow();
            }
            else if (eventArg == "JsWarrantyChange")
            {
                WarrantyChange();
            }
        }

        public void WarrantyChange()
        {
            //DateTime revoke_date = state.SsWorkDate;
            //decimal warranty_status = DwDetail.GetItemDecimal(Convert.ToInt32(HdRow.Value), "warranty_status");
            //if (warranty_status == 2)
            //{

            //    int day = DateTime.DaysInMonth(revoke_date.Year, revoke_date.Month);
            //    revoke_date = DateTime.ParseExact(day.ToString("00") + revoke_date.ToString("MMyyyy"), "ddMMyyyy", WebUtil.EN);
            //    DwDetail.SetItemDateTime(DwDetail.RowCount, "revoke_date", revoke_date);
            //}
            //else
            //{
            //    DateTime NullDate = DateTime.ParseExact("01011370", "ddMMyyyy", WebUtil.EN);
            //    //revoke_date = DateTime.ParseExact("00000000", "ddMMyyyy", WebUtil.EN);
            //    DwDetail.SetItemDateTime(DwDetail.RowCount, "revoke_date", NullDate);
            //}
        }

        public void PostDateNextRow()
        {
            DateTime warranty_date = state.SsWorkDate;
            DwDetail.InsertRow(0);
            DwDetail.SetItemDateTime(DwDetail.RowCount, "warranty_date", warranty_date);
        }

        private void JsGetLoan()
        {
            try
            {
                int result_detail = 0;
                String acc_no = DwMain.GetItemString(1, "account_no");
                try
                {
                    DwUtil.RetrieveDataWindow(DwMain, pbl, null, acc_no);
                    result_detail = 1;
                }
                catch { result_detail = 0; }

                if (result_detail == 1)
                {
                    String result = PmService.of_get_warrantylist(state.SsWsPass, state.SsCoopId, acc_no);
                    if (result != "")
                    {
                        DwDetail.Reset();
                        DwDetail.ImportString(result, FileSaveAsType.Xml);
                    }
                    else
                    {
                        DwDetail.Reset();
                        LtServerMessage.Text = WebUtil.WarningMessage("ไม่มีรายการจำนำ");
                    }
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("รหัสบัญชีไม่ถูกต้อง");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                String xml_main = DwDetail.Describe("DataWindow.Data.XML");
                string AccountNo = DwMain.GetItemString(1, "account_no");
                //AccountNo = int.Parse(AccountNo).ToString("0000000000");
                int result = PmService.of_set_warrantylist(state.SsWsPass, xml_main, state.SsCoopId, AccountNo);
                //int result = 0;
                try
                {
                    //DwUtil.UpdateDataWindow(DwDetail, pbl, "pmwarranty");
                    result = 1;
                }
                catch
                {
                    result = 0;
                }
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.Reset();
                    DwDetail.Reset();
                    DwMain.InsertRow(0);
                    //DwDetail.InsertRow(0);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
                }
                
            }
            catch(Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "invsource_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "investtype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "accid_prnc", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwDetail, "invsource_code",pbl,null);
                DwUtil.RetrieveDDDW(DwDetail, "investtype_code", pbl, null);
                
            }
            catch { }

            tDwMain.Eng2ThaiAllRow();
            DwDetail.SaveDataCache();
            DwMain.SaveDataCache();
        }

        public void DelRow()
        {
            try
            {
                string AccountNo = DwMain.GetItemString(1, "account_no");
                AccountNo = int.Parse(AccountNo).ToString("0000000000");
                
                Int16 rowcount = Convert.ToInt16(DwDetail.RowCount);                
                int result = PmService.of_del_warranty(state.SsWsPass, state.SsCoopId, AccountNo, rowcount);
                if (result == 1)
                {
                    DwDetail.DeleteRow(DwDetail.RowCount);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบแถวเรียบร้อยแล้ว");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถลบแถวได้");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}