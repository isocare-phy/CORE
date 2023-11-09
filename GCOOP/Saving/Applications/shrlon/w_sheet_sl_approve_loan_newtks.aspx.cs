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
using CoreSavingLibrary.WcfCommon;
using CoreSavingLibrary.WcfShrlon;
using Sybase.DataWindow;
using System.Globalization;
using System.Web.Services.Protocols;
using DataLibrary;

namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_approve_loan_newtks : PageWebSheet, WebSheet
    {
        private ShrlonClient shrlonService;
        private DwThDate thDwMaster;


        protected String jsgenReqDocNo = "";
        protected String postmember = "";
        protected String postloantype = "";
        protected String postallflag = "";
        protected string jsPostEntryid = "";
        protected string jspostdate = "";
        protected string jsPostSetFilter = "";

        #region WebSheet Members

        void WebSheet.InitJsPostBack()
        {


            jsgenReqDocNo = WebUtil.JsPostBack(this, "jsgenReqDocNo");
            postmember = WebUtil.JsPostBack(this, "postmember");
            postloantype = WebUtil.JsPostBack(this, "postloantype");
            jsPostEntryid = WebUtil.JsPostBack(this, "jsPostEntryid");
            postallflag = WebUtil.JsPostBack(this, "postallflag");
            jspostdate = WebUtil.JsPostBack(this, "jspostdate");
            jsPostSetFilter = WebUtil.JsPostBack(this, "jsPostSetFilter");
            //thDwMaster = new DwThDate(dw_master, this);
            //thDwMaster.Add("loanrequest_date", "loanrequest_tdate");
            //thDwMaster.Add("loanrcvfix_date", "loanrcvfix_tdate");
        }

        void WebSheet.WebSheetLoadBegin()
        {
            try
            {
                shrlonService = wcf.Shrlon;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            this.ConnectSQLCA();

            if (IsPostBack)
            {
                if (dw_cri.RowCount > 1)
                {
                    dw_cri.DeleteRow(dw_cri.RowCount);
                }
                dw_master.RestoreContext();
                dw_cri.RestoreContext();
            }
            else
            {
                Londbegin();
                // this.InitLnReqList();
            }
            if (dw_master.RowCount < 1)
            {
                this.InitLnReqList();

            }

        }

        private void Londbegin()
        {
            dw_cri.InsertRow(0);
            dw_cri.SetItemDecimal(1, "all_flag", 1);
            DwUtil.RetrieveDDDW(dw_cri, "loantype_code", "sl_approve_loan.pbl", null);
            dw_cri.SetItemString(1, "loantype_code", "");
            dw_cri.SetItemString(1, "member_no", "");
            dw_cri.SetItemString(1, "loanreq_start", "");
            dw_cri.SetItemString(1, "loanreq_end", "");
        }

        void WebSheet.CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsgenReqDocNo")
            {
                this.GenReqDocNo();
            }
            else if (eventArg == "postmember")
            {
                this.JsPostMember();
            }
            else if (eventArg == "postloantype")
            {
                this.JsPostLoantype();
            }
            else if (eventArg == "postallflag")
            {
                this.JsPostAllflag();
            }
            else if (eventArg == "jsPostEntryid")
            {
                JsPostEntryid();
            }
            else if (eventArg == "jspostdate")
            {
                this.JsPostLoanDate();
            }
            else if (eventArg == "jsPostSetFilter")
            {
                this.PostSetFilter();
            }
        }

        private void InitLnReqList()
        {
            try
            {
                String reqListXML = shrlonService.Initlist_lnreqapv(state.SsWsPass);

                dw_master.Reset();
                DwUtil.ImportData(reqListXML, dw_master, null, FileSaveAsType.Xml);

                dw_master.SetSort(" loanrequest_docno asc , loantype_code asc ,loancontract_no asc");
                dw_master.Sort();

            }

            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลรอทำรายการ");
            }
        }

        private void PostSetFilter()
        {
            string ls_filter = "", ls_membno = "", ls_entryid = "", ls_loantype;
            ls_membno = dw_cri.GetItemString(1, "member_no");
            try { ls_entryid = dw_cri.GetItemString(1, "entry_id"); }
            catch { ls_entryid = ""; }
            ls_loantype = dw_cri.GetItemString(1, "loantype_code");

            if (ls_entryid != "")
            {
                ls_filter += "entry_id = '" + ls_entryid + "'";
            }
            if (ls_membno != "")
            {
                ls_membno = WebUtil.MemberNoFormat(ls_membno);
                dw_cri.SetItemString(1, "member_no", ls_membno);
                if (ls_filter == "")
                {
                    ls_filter += "member_no = '" + ls_membno + "'";
                }
                else
                {
                    ls_filter += "and member_no = '" + ls_membno + "'";
                }
            }
            if (ls_loantype != "")
            {
                if (ls_filter == "")
                {
                    ls_filter += "loantype_code = '" + ls_loantype + "'";
                }
                else
                {
                    ls_filter += "and loantype_code = '" + ls_loantype + "'";
                }
            }

            dw_master.SetFilter(ls_filter);
            dw_master.Filter();
            dw_master.SetSort("loanrequesc_docno asc");
            dw_master.Sort();

        }

        private void GenReqDocNo()
        {
            //JsChklastDocno();
            int k = 0;
            int count = dw_master.RowCount;
            for (int i = 0; i < count; i++)
            {
                string req_coopid = dw_master.GetItemString(i + 1, "coop_id");

                String lncont_no = "";
                try { lncont_no = dw_master.GetItemString(i + 1, "loancontract_no"); }
                catch { lncont_no = ""; }

                String lncont_status = dw_master.GetItemString(i + 1, "loanrequest_status");
                if (lncont_status == "1")
                {
                    if (lncont_no == "")
                    {
                        String loantype_code = dw_master.GetItemString(i + 1, "loantype_code").Trim();
                        String newReqDocNo = shrlonService.GenReqDocNo(state.SsWsPass, req_coopid, loantype_code);

                        k++;
                        if (k == 1)
                        {
                            Jschklastdocnonew(newReqDocNo, loantype_code);

                        }
                        dw_master.SetItemString(i + 1, "loancontract_no", newReqDocNo);
                        //  dw_master.SetItemDateTime(i + 1, "approve_date", state.SsWorkDate);
                    }
                }
            }

        }
        private int Jschklastdocnonew(string as_contno, string as_loantype_code)
        {

            try
            {
                string lncont_no = as_contno, last_condocno = "", ls_lastdocnonew = "";// dw_master.GetItemString(i, "loancontract_no");
                int lendocno = lncont_no.Substring(4, 6).Length;//lncont_no.Length - 4
                double lastdocno = Convert.ToDouble(lncont_no.Substring(4, Convert.ToInt16(lendocno)));
                string coop_id = state.SsCoopId;
                lastdocno--;
                string ls_lastdocno = "00000000000" + lastdocno.ToString();
                ls_lastdocnonew = ls_lastdocno.Substring(ls_lastdocno.Length - lendocno, lendocno);
                String documentcode = wcf.Busscom.of_getattribloantype(state.SsWsPass, as_loantype_code, "document_code");
                //a.document_code,
                string ls_sql = @"select   max( a.document_code || substr(  b.loancontract_no,3,2)||' ' || substr( b.loancontract_no,5,6) ) as lastdocno
                        from lncontmaster b , lnloantype a 
                        where a.loantype_code = b.loantype_code and  a.coop_id = b.coop_id and a.coop_id = '" + coop_id + "' and a.document_code = '" + documentcode + "'";
                try
                {
                    Sdt dt2 = WebUtil.QuerySdt(ls_sql);

                    if (dt2.Next())
                    {
                        last_condocno = dt2.GetString("lastdocno");

                    }
                    // last_condocno = ls_lastdocno.Substring(ls_lastdocno.Length - lendocno, lendocno);
                    string lastcontno22 = lncont_no.Substring(0, 3) + ls_lastdocno + lncont_no.Substring(8, 2);
                    string last_condocno4 = last_condocno.Substring(last_condocno.Length - 4, 4);

                    if (Convert.ToDouble(last_condocno4) != lastdocno)
                    {
                        LtServerMessage.Text = WebUtil.WarningMessage("การอนุมัติเลขสัญญาเงินกู้ เลขสัญญา " + as_contno + " ไม่เป็นเลขต่อจากเลขที่ล่าสุดของก่อนหน้านั้น(เลขล่าสุด = " + last_condocno + " ) <br />กรุณาตรวจสอบด้วย");
                    }
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.WarningMessage("การอนุมัติเลขสัญญาเงินกู้ เลขสัญญา " + as_contno + " ไม่พบเลขที่ล่าสุดของก่อนหน้านั้น <br />กรุณาตรวจสอบด้วย");
                }
            }
            catch
            {

            }
            return 1;
        }
        //เพิ่ม function
        private int JsChklastDocno()
        {
            int count = dw_master.RowCount;

            for (int i = 1; i <= count; i++)
            {

                String lncont_status = dw_master.GetItemString(i, "loanrequest_status");

                String lncont_no = "";
                try
                {
                    lncont_no = dw_master.GetItemString(i, "loancontract_no");
                }
                catch
                {
                }
                //if (lncont_status == "1" && lncont_no != "")
                //{
                //    int lendocno = lncont_no.Substring(3, 6).Length;//lncont_no.Length - 4
                //    double lastdocno = Convert.ToDouble(lncont_no.Substring(3, Convert.ToInt16(lendocno)));

                //    lastdocno--;
                //    string ls_lastdocno = "00000000000" + lastdocno.ToString();
                //    ls_lastdocno = ls_lastdocno.Substring(ls_lastdocno.Length - lendocno, lendocno);
                //    string lastcontno = lncont_no.Substring(0, 3) + ls_lastdocno + lncont_no.Substring(8, 2);

                //    string ls_sql = " select * from lncontmaster where loancontract_no = '" + lastcontno + "'";
                //    Sdt dt = WebUtil.QuerySdt(ls_sql);
                //    if (dt.GetRowCount() <= 0)
                //    {
                //        LtServerMessage.Text = WebUtil.WarningMessage("การอนุมัติเลขสัญญาเงินกู้ เลขสัญญาของคนแรก ไม่เป็นเลขต่อจากเลข่ที่ล่าสุดของก่อนหน้านั้น กรุณาตรวจสอด้วย");
                //        return 1;
                //    }
                //    else
                //    {
                //        return 1;
                //    }
                //}
            }
            return 1;
        }
        private void JsPostMember()
        {
            String member_no = WebUtil.MemberNoFormat(dw_cri.GetItemString(1, "member_no"));
            dw_master.SetFilter("member_no = '" + member_no + "'");
            dw_master.Filter();
            dw_cri.SetItemString(1, "member_no", member_no);
            dw_cri.SetItemString(1, "loantype_code", "");
            dw_cri.SetItemDecimal(1, "all_flag", 0);
        }
        private void JsPostLoanDate()
        {
            String member_no = WebUtil.MemberNoFormat(dw_cri.GetItemString(1, "member_no"));
            String loanreq_start = dw_cri.GetItemString(1, "loanreq_start");
            String loanreq_end = dw_cri.GetItemString(1, "loanreq_end");
            string[] arrloanstart = loanreq_start.Split('/');
            string[] arrloanend = loanreq_end.Split('/');
            string tmpstart = (Convert.ToDecimal(arrloanstart[2]) - 543) + arrloanstart[1] + arrloanstart[0];
            string tmpend = (Convert.ToDecimal(arrloanend[2]) - 543) + arrloanend[1] + arrloanend[0];
            string filter = "string(loanrequest_date,'yyyymmdd') >= '" + tmpstart + "'";
            filter += " AND string(loanrequest_date,'yyyymmdd') <= '" + tmpend + "'";
            dw_master.SetFilter(filter);
            dw_master.Filter();
            dw_cri.SetItemString(1, "member_no", member_no);
            dw_cri.SetItemString(1, "loantype_code", "");
            dw_cri.SetItemDecimal(1, "all_flag", 0);
            dw_cri.SetItemString(1, "loanreq_start", loanreq_start);
            dw_cri.SetItemString(1, "loanreq_end", loanreq_end);
        }
        private void JsPostEntryid()
        {
            String entry_id = dw_cri.GetItemString(1, "entry_id");
            dw_master.SetFilter("entry_id = '" + entry_id + "'");
            dw_master.Filter();
            dw_master.SetSort("loanrequesc_docno asc , loancontract_no asc ");
            dw_master.Sort();
            dw_cri.SetItemString(1, "loantype_code", "");
            dw_cri.SetItemString(1, "member_no", "");
            dw_cri.SetItemDecimal(1, "all_flag", 0);

        }
        private void JsPostLoantype()
        {
            String loantype_code = dw_cri.GetItemString(1, "loantype_code");
            dw_master.SetFilter("loantype_code = '" + loantype_code + "'");
            dw_master.Filter();
            dw_master.SetSort("loanrequesc_docno asc , loancontract_no asc ");
            dw_master.Sort();
            dw_cri.SetItemString(1, "loantype_code", loantype_code);
            dw_cri.SetItemString(1, "member_no", "");
            dw_cri.SetItemDecimal(1, "all_flag", 0);
        }
        private void JsPostAllflag()
        {
            dw_cri.SetItemDecimal(1, "all_flag", Convert.ToDecimal(HfAllFlag.Value));
            InitLnReqList();
            dw_cri.SetItemString(1, "member_no", "");
            dw_cri.SetItemString(1, "loantype_code", "");
        }

        void WebSheet.SaveWebSheet()
        {
            try
            {
                int count = dw_master.RowCount + 1;
                for (int i = 1; i < count; i++)
                {
                    Decimal choose_flag;
                    try { choose_flag = dw_master.GetItemDecimal(i, "choose_flag"); }
                    catch { choose_flag = 0; }

                    string loanrequest_docno;
                    try { loanrequest_docno = dw_master.GetItemString(i, "loanrequest_docno"); }
                    catch { loanrequest_docno = ""; }

                    if (choose_flag == 1)
                    {
                        string request_status = dw_master.GetItemString(i, "loanrequest_status");
                        if (request_status == "1" || request_status == "0")
                        {
                            try
                            {
                                string as_apvid = state.SsUsername;
                                // GenReqDocNo();
                                //เพิ่ม ที่ function  SaveWebSheet()  
                                // Oh มงบอกว่าส่งหมดมันเยอะ XML คงเต็ม
                                // เลย filter ให้เหลือน้อยๆก่อนแล้วส่งไป
                                dw_master.SetFilter("loanrequest_status <> 8");
                                dw_master.Filter();
                                String ls_xml_main = dw_master.Describe("DataWindow.Data.XML");
                                String ls_xml = shrlonService.SaveLnReqRpv(state.SsWsPass, ls_xml_main, as_apvid, state.SsCoopControl, state.SsWorkDate);

                            }
                            catch (Exception ex)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                                return;
                            }
                        }
                        else if (request_status == "11")
                        {
                            Decimal loanrequest = dw_master.GetItemDecimal(i, "loanrequest_amt");
                            string sql = "update lnreqloan set loanrequest_status='11',approve_id='" + state.SsUsername + "',loanapprove_amt='" + loanrequest + "'  where loanrequest_docno = '" + loanrequest_docno + "' ";
                            sql = WebUtil.SQLFormat(sql);
                            int sql_q = WebUtil.ExeSQL(sql);

                        }
                    }
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                InitLnReqList();
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
            }
        }

        void WebSheet.WebSheetLoadEnd()
        {
            if (dw_cri.RowCount > 1)
            {
                dw_cri.DeleteRow(dw_cri.RowCount);
            }
            DwUtil.RetrieveDDDW(dw_master, "loantype_code", "sl_approve_loan.pbl", null);
            dw_master.SaveDataCache();
            dw_cri.SaveDataCache();

            dw_master.PageNavigationBarSettings.Visible = (dw_master.RowCount > 20);
        }

        #endregion
    }
}
