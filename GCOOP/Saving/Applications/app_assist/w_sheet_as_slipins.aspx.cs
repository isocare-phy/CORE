using System;
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
using CommonLibrary;
using Saving.WsCommon;
using Sybase.DataWindow;
using DBAccess;
using Saving.WsInsurance;


namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_slipins : PageWebSheet, WebSheet
    {
        private DwThDate tDwDetail;
        protected String jsIncSlipetc;
        protected String jsAddNewRow;
        
        private String pbl = "as_slipins.pbl";

        #region WebSheet Members

        public void InitJsPostBack()
        {
            jsIncSlipetc = WebUtil.JsPostBack(this, "jsIncSlipetc");
            jsAddNewRow = WebUtil.JsPostBack(this, "jsAddNewRow");
           

            tDwDetail = new DwThDate(dw_main, this);
            tDwDetail.Add("slip_date", "slip_tdate");
            tDwDetail.Add("operate_date", "operate_tdate");
        }

        public void WebSheetLoadBegin()
        {
            //this.ConnectSQLCA();
            //dw_main.SetTransaction(sqlca);
            //dw_detail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                try
                {
                    dw_main.InsertRow(0);
                    //DwUtil.RetrieveDDDW(dw_main, "sliptype_code", "as_slipins.pbl", null);
                    //DwUtil.RetrieveDDDW(dw_main, "moneytype_code", "as_slipins.pbl", null);
                    dw_main.SetItemDateTime(1, "operate_date", state.SsWorkDate);
                    dw_main.SetItemDateTime(1, "slip_date", state.SsWorkDate);
                                      
                    tDwDetail.Eng2ThaiAllRow();
                    //dw_main.InsertRow(0);
                    //dw_detail.InsertRow(0);
                    //tDwDetail.Eng2ThaiAllRow();

                    DwUtil.RetrieveDataWindow(dw_detail, pbl, null, "slip_no");
                    HfRowNumber.Value = dw_detail.RowCount.ToString();

                    try
                    {
                        WebUtil.RetrieveDDDW(dw_main, "sliptype_code", pbl, null);
                    }
                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                    try
                    {
                        WebUtil.RetrieveDDDW(dw_main, "moneytype_code", pbl, null);
                    }
                    catch { }
                    //DwUtil.RetrieveDDDW(dw_main, "sliptype_code", "as_slipins.pbl", null);
                    //DwUtil.RetrieveDDDW(dw_main, "moneytype_code", "as_slipins.pbl", null);
                    //dw_main.SetItemDate(1, "operate_date", state.SsWorkDate);


                }
                catch { }
            }
            else
            {
               
                this.RestoreContextDw(dw_main);

            }
            //    try
            //    {
            //        this.RestoreContextDw(dw_main);
            //        this.RestoreContextDw(dw_detail);
            //        tDwDetail.Eng2ThaiAllRow();
            //    }
            //    catch { }
            //}
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsIncSlipetc")
            {
                JsIncSlipetc();
            }
            else if (eventArg == "jsAddNewRow")
            {
                JsAddNewRow();
            }

        }

        public void SaveWebSheet()
        {
            try
            {


                dw_main.SetItemDateTime(1, "entry_date", state.SsWorkDate);
                dw_main.SetItemString(1, "entry_id", state.SsUsername);
                dw_main.SetItemString(1, "coopbranch_id", state.SsBranchId);
                //new WsInsurance.Insurance().SaveInsuSliReq(state.SsWsPass, pbl, state.SsApplication, state.SsWorkDate, dw_main.Describe("DataWindow.Data.XML"));

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }


            int rwCount = dw_detail.RowCount;
            
        }
       

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(dw_main, "sliptype_code", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(dw_main, "moneytype_code", pbl, null);
            }
            catch { }

            dw_main.SaveDataCache();
            dw_detail.SaveDataCache();
            //dw_main.Retrieve();
            //tDwDetail.Eng2ThaiAllRow();
        }

        #endregion

        private void JsIncSlipetc()
        {
            try
            {
                String membNo = dw_main.GetItemString(1, "member_no");
                membNo = WebUtil.MemberNoFormat(membNo);
                dw_main.SetItemString(1, "member_no", membNo);
                String xmlMaster = dw_main.Describe("DataWindow.Data.XML");
                Insurance insSV = new Insurance();
                //String[] resu = insSV.InitInsuSliReq(state.SsWsPass, pbl, state.SsApplication, xmlMaster);
                //Insurance insService = new Insurance();
                //string[] resu =  insService.initInsuSlipNEW(state.SsWsPass, pbl, state.SsApplication, xmlMaster);
                //string xmlDwMain = resu[0];
                //string xmlDwLoan = resu[1];
                dw_main.Reset();
                //dw_main.ImportString(xmlDwMain, Sybase.DataWindow.FileSaveAsType.Xml);
                tDwDetail.Eng2ThaiAllRow();
                //DwUtil.RetrieveDataWindow(dw_detail,pbl,null,);
                //if (!string.IsNullOrEmpty(xmlDwLoan))
                //{
                //    dw_detail.Reset();
                //    dw_detail.ImportString(xmlDwLoan, Sybase.DataWindow.FileSaveAsType.Xml);
                //}
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            //string membno = Hfmember_no.Value;
            //int rowNumber = Convert.ToInt16(HfRowNumber.Value);
            //string rateCode = dw_main.GetItemString(rowNumber, "sliptype_code");
            //dw_main.Reset();
            //dw_main.Retrieve();
            //dw_detail.Reset();
            //dw_detail.Retrieve(rateCode);
            //tDwDetail.Eng2ThaiAllRow();
        }
        private void JsAddNewRow()
        {
            dw_detail.InsertRow(0);
            String sql = "Select max(seq_no) as xseq_no from CMSHRLONSLIPDET";
            DataTable dt = WebUtil.Query(sql);
            int rw = Convert.ToInt32(dt.Rows[0]["xseq_no"]) + 1;
            
            int rwNow = dw_detail.RowCount;
            SaveWebSheet();

        }
    }
}
