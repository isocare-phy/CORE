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
using System.Web.Services.Protocols;
using CoreSavingLibrary.WcfNDeposit;

namespace Saving.Applications.ap_deposit
{
    public partial class w_sheet_dp_ins_depttrans : PageWebSheet, WebSheet
    {
        protected String postAccountNo;
        protected String postDeptAmt;
        protected String deleteDepttran;

        private DwThDate thDwMain;
        private String pblFileName = "dp_depttrans.pbl";
        private static String CheckOperation = "";
        protected String postNew;
        private decimal SEQ_MAX ;

        private void JsPostAccountNo()
        {
            try
            {

                String dwAccNo = DwMain.GetItemString(1, "deptaccount_no");
                dwAccNo = wcf.NDeposit.of_analizeaccno(state.SsWsPass, dwAccNo);
                try
                {

                    DwUtil.RetrieveDataWindow(DwMain, pblFileName, thDwMain, state.SsCoopId, dwAccNo);
                    if (!string.IsNullOrEmpty(dwAccNo))
                    {
                        DwDetail.Reset();
                        DwUtil.RetrieveDataWindow(DwDetail, "dp_depttrans.pbl", null,state.SsCoopId, dwAccNo);
                    }

                    thDwMain.Eng2ThaiAllRow();
                }
                catch { }

                if (DwMain.RowCount < 1)
                {
                    CheckOperation = "INS";
                    string sql = "select deptaccount_no,member_no ,deptaccount_name,prncbal from dpdeptmaster where deptaccount_no = '" + dwAccNo + "'";
                    DataTable dt = WebUtil.Query(sql);
                    DwMain.InsertRow(0);
                    if (dt.Rows.Count > 0)
                    {
                        DwMain.SetItemString(1, "deptaccount_no", dt.Rows[0]["deptaccount_no"].ToString());
                        DwMain.SetItemString(1, "member_no", dt.Rows[0]["member_no"].ToString());
                        DwMain.SetItemString(1, "deptaccount_name", dt.Rows[0]["deptaccount_name"].ToString());
                        DwMain.SetItemDecimal(1, "prncbal", Convert.ToDecimal(dt.Rows[0]["prncbal"].ToString()));
                        DwMain.SetItemDateTime(1, "tran_date", state.SsWorkDate);
                        DwMain.SetItemDecimal(1, "deptitem_amt", 0);
                        thDwMain.Eng2ThaiAllRow();
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบเลขบัญชื " + dwAccNo);
                    }
                }
                else
                {
                    //CheckOperation = "UPD";
                    CheckOperation = "INS";
                    DwMain.SetItemDecimal(1, "deptitem_amt", 0);
                    DwMain.SetItemString(1, "system_code", null);

                }
                String accFormat = WebUtil.ViewAccountNoFormat(DwMain.GetItemString(1, "deptaccount_no"));
                DwMain.SetItemString(1, "deptaccount_no", accFormat);
                //     DwMain.SetItemDecimal(1, "cross_coopflag", CrossFlg);
                //     DwMain.SetItemString(1, "dwcrosscoop", HfCoopid.Value);
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
            postDeptAmt = WebUtil.JsPostBack(this, "postDeptAmt");
            postNew = WebUtil.JsPostBack(this, "postNew");
            deleteDepttran = WebUtil.JsPostBack(this, "deleteDepttran");
            //----------------------------------------------------------------
            thDwMain = new DwThDate(DwMain, this);
            thDwMain.Add("tran_date", "tran_tdate");

            //----------------------------------------------------------------
        }

        public void WebSheetLoadBegin()
        {


            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            if (IsPostBack)
            {
                this.RestoreContextDw(DwMain);
            }
            else
            {
                DwMain.InsertRow(0);

            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postAccountNo")
            {
                JsPostAccountNo();
            }

            else if (eventArg == "postNew")
            {
                JspostNew();
            }
            else if (eventArg == "postDeptAmt")
            {
                JspostDeptAmt();
            }
            else if (eventArg == "deleteDepttran")
            {                            
               JspostDelDepttran();               
            }

        }


        public void SaveWebSheet()
        {
            string AccNo = wcf.NDeposit.of_analizeaccno(state.SsWsPass, DwMain.GetItemString(1, "deptaccount_no"));
            string COOP_ID = state.SsCoopId;
            string MEMCOOP_ID = state.SsCoopId;
            string MEMBER_NO = DwMain.GetItemString(1, "member_no");
            string SYSTEM_CODE = DwMain.GetItemString(1, "system_code");            
            decimal TRAN_YEAR = state.SsWorkDate.Year;
            DateTime TRAN_DATE = state.SsWorkDate;    
            String TRAN_DATE_SEQ = WebUtil.ConvertDateThaiToEng(DwMain, "tran_tdate", null);
            DateTime TRAN_DATE_D =  Convert.ToDateTime(TRAN_DATE_SEQ);
            DataTable dt_seq = WebUtil.Query("select max(seq_no) from dpdepttran where deptaccount_no = '" + AccNo + "' " +
                        "       and MEMBER_NO = '" + MEMBER_NO + "' " +
                        "       and COOP_ID = '" + COOP_ID + "' " +                     
                        "       and TRAN_DATE = to_date('" + TRAN_DATE_D.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy')");
           
           
            String SEQ_NO_S = dt_seq.Rows.Count > 0 ? dt_seq.Rows[0][0].ToString() : "";

            if (SEQ_NO_S == null || SEQ_NO_S == "")
            {
                SEQ_NO_S = "0";
            }
            else
            {
                SEQ_NO_S = SEQ_NO_S;
            }

            if (dt_seq.Rows.Count > 0)
            {
                SEQ_MAX = Convert.ToDecimal(SEQ_NO_S);
                if (SEQ_MAX > 0)
                {
                    SEQ_MAX += 1;
                }
                else
                {
                    SEQ_MAX = 1;
                }

            }

            decimal SEQ_NO = SEQ_MAX;
            decimal OLD_BALANC = 0;
            decimal OLD_ACCUINT = 0;
            decimal DEPTITEM_AMT = DwMain.GetItemDecimal(1, "DEPTITEM_AMT");
            decimal INT_AMT = 0;
            decimal NEW_ACCUINT = 0;
            decimal NEW_BALANC = 0;
            decimal TRAN_STATUS = 0;
            decimal DIVIDEN_AMT = 0;
            decimal AVERAGE_AMT = 0;
            string BRANCH_OPERATE = "001";
            decimal SEQUEST_STATUS = 0;
            decimal SEQUEST_AMT = 0;

            if (CheckOperation == "INS")
            {
                try
                {
                    string sql = "INSERT INTO DPDEPTTRAN " +
                                 " ( deptaccount_no ,COOP_ID,MEMCOOP_ID,MEMBER_NO,SYSTEM_CODE,TRAN_YEAR," +
                                 " TRAN_DATE,SEQ_NO,OLD_BALANC,OLD_ACCUINT,DEPTITEM_AMT,INT_AMT," +
                                 " NEW_ACCUINT,NEW_BALANC,TRAN_STATUS,DIVIDEN_AMT,AVERAGE_AMT," +
                                 " BRANCH_OPERATE,SEQUEST_STATUS,SEQUEST_AMT ) VALUES " +
                                 " ( '" + AccNo + "','" + COOP_ID + "','" + MEMCOOP_ID + "','" + MEMBER_NO + "','" + SYSTEM_CODE + "'," + TRAN_YEAR + "," +
                                 "  to_date('" + TRAN_DATE.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy')," + SEQ_NO + "," + OLD_BALANC + "," + OLD_ACCUINT + " ," + DEPTITEM_AMT + "," +
                                 "  " + INT_AMT + "," + NEW_ACCUINT + "," + NEW_BALANC + "," + TRAN_STATUS + "," + DIVIDEN_AMT + "," + AVERAGE_AMT + "," +
                                 "  '" + BRANCH_OPERATE + "'," + SEQUEST_STATUS + "," + SEQUEST_AMT + ")";
                    WebUtil.Query(sql);
                    JspostNew();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกรายการได้ " + " " + ex);
                }
            }
            else if (CheckOperation == "UPD")
            {
                try
                {
                    string sql = " update DPDEPTTRAN " +
                                 " set DEPTITEM_AMT = " + DEPTITEM_AMT + " " +
                                 " where deptaccount_no = '" + AccNo + "' " +
                                 "       and MEMBER_NO = '" + MEMBER_NO + "' " +
                                 "       and COOP_ID = '" + COOP_ID + "' " +
                                 "       and SYSTEM_CODE = '" + SYSTEM_CODE + "' " +
                                 "       and SEQ_NO = " + SEQ_NO + " " +
                                 "       and TRAN_STATUS = " + TRAN_STATUS + "";
                    WebUtil.Query(sql);
                    JspostNew();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกรายการได้ " + " " + ex);
                }
            }

        }

        public void WebSheetLoadEnd()
        {
            try
            {

                if (DwMain.RowCount <= 0)
                {
                    DwMain.InsertRow(0);
                }
            }
            catch { }
            thDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();

        }

        #endregion


        private void JspostNew()
        {
            DwMain.Reset();
            DwDetail.Reset();
            DwMain.InsertRow(0);

            HdIsPostBack.Value = "false";
            //string coop_control = state.SsCoopControl;
            //DwUtil.RetrieveDDDW(DwMain, "dwcrosscoop",pblFileName, coop_control);
        }

        private void JspostDeptAmt()
        {
            decimal deptamt = DwMain.GetItemDecimal(1, "deptitem_amt");
            if (deptamt < 0)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ยอดทำรายการติดลบ ไม่สามารถทำรายการได้");
            }
        }

        private void JspostDelDepttran()
        {           
            int detailRow = int.Parse(HdRow.Value);            
            decimal TRAN_STATUS = -9;
            try
            {          
                if (detailRow > 0)
                {
                    string Account_No = DwDetail.GetItemString(detailRow, "dpdepttran_deptaccount_no");
                    string MEMBER_NO = DwDetail.GetItemString(detailRow, "dpdepttran_member_no");
                    string COOP_ID = DwDetail.GetItemString(detailRow, "dpdepttran_coop_id");
                    string SYSTEM_CODE = DwDetail.GetItemString(detailRow, "dpdepttran_system_code");
                    decimal Seq_No = DwDetail.GetItemDecimal(detailRow, "dpdepttran_seq_no");
                    decimal DEPTITEM_AMT = DwDetail.GetItemDecimal(detailRow, "dpdepttran_deptitem_amt");
                    DateTime TRAN_DATE = DwDetail.GetItemDateTime(detailRow, "dpdepttran_tran_date");

                    try
                    {
                        string sql = " update DPDEPTTRAN " +
                                  " set TRAN_STATUS = " + TRAN_STATUS + " " +
                                  " where deptaccount_no = '" + Account_No + "' " +
                                  "       and MEMBER_NO = '" + MEMBER_NO + "' " +
                                  "       and COOP_ID = '" + COOP_ID + "' " +
                                  "       and SYSTEM_CODE = '" + SYSTEM_CODE + "' " +
                                  "       and SEQ_NO = " + Seq_No + " " +                             
                                  "       and DEPTITEM_AMT = " + DEPTITEM_AMT + "";     
                
                        WebUtil.Query(sql);
                        JspostNew();
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกรายการได้ " + " " + ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
       
        }

    }
}