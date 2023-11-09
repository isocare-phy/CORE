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
//using Saving.WcfShrlon;  //coresavinglibrary
//using Saving.WcfCommon;  //coresavinglibrary
using CoreSavingLibrary.WcfShrlon;  //coresavinglibrary
using CoreSavingLibrary.WcfCommon;  //coresavinglibrary
using Sybase.DataWindow;
using DataLibrary;		//เช็คดูอีกที
using System.Threading;

namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_collateral_master_new : PageWebSheet, WebSheet
    {
        public String app = "";
        public String gid = "";
        public String rid = "";
        protected String pdf = "";
        private DwThDate tdwhead;
        private DwThDate tdw_detail_02;
        private ShrlonClient shrlonService;  //core
        private CommonClient commonService; //core
        protected String jsPostMember;
        protected String PostMember1;
        protected String jsPostCollmast;
        protected String newClear;
        protected String jsRefresh;
        protected String postInsertRow;
        protected String jsgetmember;
        protected String jsDeleteRow;
        protected String jspostPorvince;
        protected String jsprintColl;
        protected string jsCalmortgage;
        protected string jsSetFiltermastlnreq;
        protected String jsDeleteRowReview;
        protected String jsInsertRowReview;
        protected String jsCopyDetailcollmast;
        protected String jsInsertRowowndetail;
        protected String jsCopyDetailownland;
        //==============
        private void RunProcess()
        {
            String print_id = dw_printset.GetItemString(1, "coll_printset").Trim();
            String as_mastno = Hmastno.Value;


            app = state.SsApplication;
            try
            {
                gid = "LN_PRINT";
            }
            catch { }
            try
            {
                rid = print_id;
            }
            catch { }


            if (as_mastno == null || as_mastno == "")
            {
                return;
            }
            else
            {
                ReportHelper lnv_helper = new ReportHelper();
                lnv_helper.AddArgument(as_mastno, ArgumentType.String);

                //****************************************************************

                //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF
                String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
                pdfFileName += "_" + gid + "_" + rid + ".pdf";
                pdfFileName = pdfFileName.Trim();

                //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
                try
                {
                    Saving.WcfReport.ReportClient lws_report = wcf.Report;
                    String criteriaXML = lnv_helper.PopArgumentsXML();
                    this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                    String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                    if (li_return == "true")
                    {
                        HdOpenIFrame.Value = "True";
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    return;
                }
            }
        }
        //===============
        public void InitJsPostBack()
        {
            jsPostMember = WebUtil.JsPostBack(this, "jsPostMember");
            PostMember1 = WebUtil.JsPostBack(this, "PostMember1");
            jsPostCollmast = WebUtil.JsPostBack(this, "jsPostCollmast");
            jsRefresh = WebUtil.JsPostBack(this, "jsRefresh");
            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            jsgetmember = WebUtil.JsPostBack(this, "jsgetmember");
            jsDeleteRow = WebUtil.JsPostBack(this, "jsDeleteRow");
            jspostPorvince = WebUtil.JsPostBack(this, "jspostPorvince");
            jsprintColl = WebUtil.JsPostBack(this, "jsprintColl");
            newClear = WebUtil.JsPostBack(this, "newClear");
            jsCalmortgage = WebUtil.JsPostBack(this, "jsCalmortgage");
            jsSetFiltermastlnreq = WebUtil.JsPostBack(this, "jsSetFiltermastlnreq");
            jsDeleteRowReview = WebUtil.JsPostBack(this, "jsDeleteRowReview");
            jsInsertRowReview = WebUtil.JsPostBack(this, "jsInsertRowReview");
            jsCopyDetailcollmast = WebUtil.JsPostBack(this, "jsCopyDetailcollmast");
            jsInsertRowowndetail = WebUtil.JsPostBack(this, "jsInsertRowowndetail");
            jsCopyDetailownland = WebUtil.JsPostBack(this, "jsCopyDetailownland");
            tdwhead = new DwThDate(dw_head, this);
            tdwhead.Add("mortgage_date", "mortgage_tdate");
            tdwhead.Add("redeem_date", "redeem_tdate");
            tdw_detail_02 = new DwThDate(dw_detail_02, this);
            tdw_detail_02.Add("mortgage_date", "mortgage_tdate");

        }

        public void WebSheetLoadBegin()
        {
            try
            {
                shrlonService = wcf.Shrlon; //core
                commonService = wcf.Common; //core
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            //this.ConnectSQLCA();
            //dw_collddetail.SetTransaction(sqlca);
            //dw_detail_01.SetTransaction(sqlca);
            if (IsPostBack)
            {

                try
                {
                    this.RestoreContextDw(dw_main);
                    this.RestoreContextDw(dw_list);
                    this.RestoreContextDw(dw_head);
                    this.RestoreContextDw(dw_detail);
                    this.RestoreContextDw(dw_collddetail);
                    this.RestoreContextDw(dw_detail_01);
                    this.RestoreContextDw(dw_detail_02);
                    this.RestoreContextDw(dw_review);
                    this.RestoreContextDw(dw_printset);
                                       
                }
                catch { }

            }
            if (dw_main.RowCount < 1)
            {
                dw_main.InsertRow(0);
                dw_list.InsertRow(0);
                dw_head.InsertRow(0);
                dw_collddetail.InsertRow(0);
                dw_detail_01.InsertRow(0);
                dw_detail_02.InsertRow(0);
                dw_printset.InsertRow(1);
                dw_review.InsertRow(0);

                //  dw_detail.InsertRow(0);

                dw_head.SetItemDate(1, "mortgage_date", state.SsWorkDate);
                dw_head.SetItemDate(1, "redeem_date", state.SsWorkDate);
                dw_detail_02.SetItemDate(1, "mortgage_date", state.SsWorkDate);

                tdwhead.Eng2ThaiAllRow();
                tdw_detail_02.Eng2ThaiAllRow();
               // GetDDDW();

            }

        }

	///check if it use///

        public void GetDDDW()
        {
            try
            {
               // WebUtil.RetrieveDDDW(dw_list, "collmasttype_code", "sl_collateral_master.pbl", null);

                //WebUtil.RetrieveDDDW(dw_collddetail, "pos_district", "sl_collateral_master.pbl", null);
                WebUtil.RetrieveDDDW(dw_collddetail, "pos_province", "sl_collateral_master.pbl", null);
                //WebUtil.RetrieveDDDW(dw_detail_01, "mrtg_district", "sl_collateral_master.pbl", null);
                WebUtil.RetrieveDDDW(dw_detail_01, "mrtg_province", "sl_collateral_master.pbl", null);
                //WebUtil.RetrieveDDDW(dw_detail_01, "autrz_district", "sl_collateral_master.pbl", null);
                
                WebUtil.RetrieveDDDW(dw_collddetail, "buildingtype_code", "sl_collateral_master.pbl", null);
                WebUtil.RetrieveDDDW(dw_printset, "coll_printset", "sl_collateral_master.pbl", null);

                WebUtil.RetrieveDDDW(dw_review, "pos_province", "sl_collateral_master.pbl", null);
                WebUtil.RetrieveDDDW(dw_review, "buildingtype_code", "sl_collateral_master.pbl", null);
                WebUtil.RetrieveDDDW(dw_review, "collmasttype_code", "sl_collateral_master.pbl", null);

                WebUtil.RetrieveDDDW(dw_detail_01, "autrz_province", "sl_collateral_master.pbl", null);
                WebUtil.RetrieveDDDW(dw_detail_02, "own_province", "sl_collateral_master.pbl", null);
               

            }
            catch { }

        }
        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsPostMember")
            {
                JsPostMember();
            }
            else if (eventArg == "PostMember1")
            {
                jsPostMember1();
            }
            else if (eventArg == "jsPostCollmast")
            {
                JsPostCollmast();
            }
            else if (eventArg == "newClear")
            {
                JsNewClear();
            }
            else if (eventArg == "jsRefresh")
            {

            }
            else if (eventArg == "postInsertRow")
            {
                JsPostInsertRow();
            }
            else if (eventArg == "jsgetmember")
            {
                Jsgetmember();
            }
            else if (eventArg == "jsDeleteRow")
            { JsDeleteRow(); }
            else if (eventArg == "jspostPorvince")
            {
                JspostPorvince();
            }
            else if (eventArg == "jspostPorvinceownland")
            {
                JspostPorvince();
            }
            else if (eventArg == "jsprintColl")
            {
                JsprintColl();
            }
            else if (eventArg == "jsCalmortgage") {
                JsCalmortgage();
            }
            else if (eventArg == "jsSetFiltermastlnreq") {
                JsSetFiltermastlnreq();
            }
            else if (eventArg == "jsInsertRowReview")
            {
                JsInsertRowReview();
            }
            else if (eventArg == "jsInsertRowDetail")
            {
                JsInsertRowDetail();
            }
            else if (eventArg == "jsCopyDetailcollmast")
            {
                JsCopyDetailcollmast();
            }
            else if (eventArg == "jsInsertRowowndetail")
            {
                JsInsertRowowndetail();
            }
            else if (eventArg == "jsCopyDetailownland")
            {
                JsCopyDetailownland();
            }

            
            
            
        }

        private void JsprintColl()
        {
            //Mai
            RunProcess();
            // Thread.Sleep(5000);
            Thread.Sleep(4500);

            //เด้ง Popup ออกรายงานเป็น PDF.
            String pop = "Gcoop.OpenPopup('" + pdf + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
        }


        private void JspostPorvince()
        {
            ShowDistrict();
        }

        public void SaveWebSheet()
        {
            try
            {
                str_lncollmast strlncoll = new str_lncollmast();
                strlncoll.member_no = Hfmember_no.Value;
                strlncoll.xml_collmastdet = dw_head.Describe("DataWindow.Data.XML");
                // strlncoll.xml_collmemco = dw_detail.RowCount > 0 ? dw_detail.Describe("DataWindow.Data.XML") : "";
                if (dw_detail.RowCount == 0)
                {
                    dw_detail.InsertRow(1);
                    dw_detail.SetItemString(1, "memco_no", Hfmember_no.Value);
                    dw_detail.SetItemDecimal(1, "collmastmain_flag", 1);
                    dw_detail.SetItemString(1, "coop_id", state.SsCoopId);


                }
                strlncoll.xml_collmemco = dw_detail.Describe("DataWindow.Data.XML");
                strlncoll.xml_mrtg1 = dw_collddetail.Describe("DataWindow.Data.XML");
                strlncoll.xml_mrtg2 = dw_detail_01.Describe("DataWindow.Data.XML");
                strlncoll.xml_mrtg3 = dw_detail_02.Describe("DataWindow.Data.XML");
                strlncoll.xml_prop = dw_review.Describe("DataWindow.Data.XML");

                strlncoll.entry_id = state.SsUsername;
                strlncoll.coop_id = state.SsCoopId;

                int result = shrlonService.of_savelncollmast(state.SsWsPass, ref strlncoll);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อยแล้ว");
                    //dw_review.Reset(); dw_review.InsertRow(0);

                }
                //JsPostInsertRow();
              //  dw_detail.Reset(); dw_detail.InsertRow(0);
               // JsPostMember();
                
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

        public void WebSheetLoadEnd()
        {
            dw_main.SaveDataCache();
            dw_detail.SaveDataCache();
            dw_head.SaveDataCache();
            dw_review.SaveDataCache();
            dw_detail_01.SaveDataCache();
            dw_detail_02.SaveDataCache();
            dw_reqloan.SaveDataCache();
            dw_collddetail.SaveDataCache();
            dw_printset.SaveDataCache();
            dw_list.SaveDataCache();
            WebUtil.RetrieveDDDW(dw_head, "collmasttype_code", "sl_collateral_master.pbl", null);
            WebUtil.RetrieveDDDW(dw_head, "collrelation_code", "sl_collateral_master.pbl", null);
            WebUtil.RetrieveDDDW(dw_printset, "coll_printset", "sl_collateral_master.pbl", null);
            //WebUtil.RetrieveDDDW(dw_detail_01, "autrz_province", "sl_collateral_master.pbl", null);
            //WebUtil.RetrieveDDDW(dw_detail_02, "own_province", "sl_collateral_master.pbl", null);
            tdwhead.Eng2ThaiAllRow();

            //ShowDistrict();
            //  GetDDDW();

        }
        private void jsPostMember1()
        {
            Sta ta = new Sta(new DwTrans().ConnectionString);
            String memberno = Hmemno.Value;
            String sql = @"SELECT
         MBMEMBMASTER.MEMBER_NO,   
         MBUCFPRENAME.PRENAME_DESC||MBMEMBMASTER.MEMB_NAME||' '||MBMEMBMASTER.MEMB_SURNAME AS name ,   
         MBMEMBMASTER.CARD_PERSON  
    FROM MBMEMBMASTER,          
         MBUCFPRENAME  
         
   WHERE ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and
          MBMEMBMASTER.MEMBER_NO = '" + memberno + "'  ";
            Sdt dt = ta.Query(sql);
            String memno = dt.Rows[0]["member_no"].ToString();
            String name = dt.Rows[0]["name"].ToString();
            String CARD_PERSON = dt.Rows[0]["CARD_PERSON"].ToString();

            dw_detail_01.SetItemString(1, "mrtg_memberno", memno);
            dw_detail_01.SetItemString(1, "mrtg_name", name);
            dw_detail_01.SetItemString(1, "mrtg_personid", CARD_PERSON);

        }

        private void JsPostMember()
        {
            try
            {
                str_lncollmast strlncoll = new str_lncollmast();
                strlncoll.member_no = Hfmember_no.Value;
                strlncoll.coop_id = state.SsCoopId;
                strlncoll.xml_memdet = dw_main.Describe("DataWindow.Data.XML");
                strlncoll.xml_collmastlist = dw_list.Describe("DataWindow.Data.XML");
                strlncoll.xml_review = dw_review.Describe("DataWindow.Data.XML");
                try
                {
                    strlncoll.loanreq_docno = Hdloanrequestdocno.Value;
                }
                catch {
                    strlncoll.loanreq_docno = "000";
                }

                if (strlncoll.loanreq_docno == "" || strlncoll.loanreq_docno == "000")
                {
                    DwUtil.RetrieveDataWindow(dw_main, "sl_collateral_master.pbl", null, strlncoll.member_no);
                    DwUtil.RetrieveDDDW(dw_main, "loanrequest_docno", "sl_collateral_master.pbl", strlncoll.member_no);
                }
                else
                { 
                    string[] arg = {state.SsCoopControl,strlncoll.member_no, strlncoll.loanreq_docno};
                    DwUtil.RetrieveDataWindow(dw_list, "sl_collateral_master.pbl", null, arg );
                   // DwUtil.RetrieveDDDW(dw_main, "loanrequest_docno", "sl_collateral_master.pbl", strlncoll.member_no);
                }

                //int result = shrlonService.of_initlncollmastall(state.SsWsPass, ref strlncoll);
                //if (result == 1)
                //{
                //   string collmastno  = "";
                //   string memno = Hfmember_no.Value;
                //    try
                //    {
                //        collmastno = Hmastno.Value;// dw_list.GetItemString(krow, "collmast_no");
                //    }
                //    catch {
                //        collmastno = "00";
                //    }
                //    try
                //    {
                //        dw_main.Reset();
                //        dw_main.ImportString(strlncoll.xml_memdet, FileSaveAsType.Xml);

                //        if (strlncoll.xml_collmastlist == "")
                //        {
                //            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบหลักทรัพย์ กรุณเพิ่มหลักทรัพย์ " + strlncoll.xml_collmastlist);
                //            JsPostInsertRow();
                //        }
                //        else
                //        {
                           
                //           Int16 krow = 0;
                           
                //            dw_list.Reset();
                //            dw_list.ImportString(strlncoll.xml_collmastlist, FileSaveAsType.Xml);
                          
                //            DwUtil.RetrieveDDDW(dw_list, "collmasttype_code", "sl_collateral_master.pbl", null);
                //            string[] as_arg = { memno, collmastno };
                //            DwUtil.RetrieveDataWindow(dw_reqloan, "sl_collateral_master.pbl", null, as_arg); 
                            
                //        }
                //        DwUtil.RetrieveDDDW(dw_main, "loanrequest_docno", "sl_collateral_master.pbl", memno);
                        
                //    }
                //    catch (Exception ex)
                //    {
                //        //LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบสมาชิกท่านนี้ กรุณากรอกเลขสมาชิกอีกครั้ง");
                //        dw_main.Reset(); dw_main.InsertRow(0);

                //    }
                //    if (dw_main.RowCount > 1)
                //    {
                //        dw_main.DeleteRow(dw_main.RowCount);

                //    }



                //}


            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);

            }

        }
        private void JsSetFiltermastlnreq()
        {
            try
            {
                JsPostMember();
                string memno = dw_main.GetItemString(1, "member_no");
                string loanreq_docno = Hdloanrequestdocno.Value;
                string[] arg = { state.SsCoopControl, memno, loanreq_docno };

                DwUtil.RetrieveDataWindow(dw_list, "sl_collateral_master.pbl", null, arg);
                string loanrequest_docno = Hdloanrequestdocno.Value;// dw_main.GetItemString(1, "loanrequest_docno");
                 //string sql_select = " select ref_collno, coll_amt from lnreqloancoll where loanrequest_docno = '" + loanrequest_docno + "'";
                 //   Sdt dt = WebUtil.QuerySdt( sql_select);
                 //   int countk = 0;
                 //   string ref_collno = "", doc_no = "";
                 //   while (dt.Next())
                 //   {
                 //       countk++;
                 //       ref_collno = dt.GetString("ref_collno");
                 //       if (countk == dt.GetRowCount())
                 //       {
                 //           doc_no += "'" + ref_collno + "'";
                 //       }
                 //       else {
                 //           doc_no += "'" + ref_collno + "',";
                 //       }
                 //   }

                 //   string ls_filter = "collmast_no in (" + doc_no + ")";
                 //   dw_list.SetFilter(ls_filter);
                 //   dw_list.Filter();

            }
            catch { 
            
            }
        
        }
        private void JsPostCollmast()
        {
            try
            {
                str_lncollmast strlncoll = new str_lncollmast();
                strlncoll.collmast_no = HfSlipNo.Value;
                strlncoll.coop_id = state.SsCoopId;
                string[] arg = { strlncoll.coop_id, strlncoll.collmast_no};
                string pbll = "sl_collateral_master.pbl";
                DwUtil.RetrieveDataWindow(dw_head, pbll, null, arg);
                DwUtil.RetrieveDataWindow(dw_detail, pbll, null, arg);
                DwUtil.RetrieveDataWindow(dw_collddetail, pbll, null, arg);
                DwUtil.RetrieveDataWindow(dw_detail_01, pbll, null, arg);
                DwUtil.RetrieveDataWindow(dw_detail_02, pbll, null, arg);
                DwUtil.RetrieveDataWindow(dw_review, pbll, null, arg);
                if (dw_detail_01.RowCount <= 0)
                {
                    dw_detail_01.InsertRow(0);
                }
                if (dw_detail_02.RowCount <= 0)
                {
                    dw_detail_02.InsertRow(0);
                }
                if (dw_collddetail.RowCount <= 0)
                {
                    dw_collddetail.InsertRow(0);
                }
                //WebUtil.RetrieveDDDW(dw_detail, "pos_province", "sl_collateral_master.pbl", null);
                WebUtil.RetrieveDDDW(dw_detail_01, "autrz_province", "sl_collateral_master.pbl", null);
                WebUtil.RetrieveDDDW(dw_detail_01, "mrtg_province", "sl_collateral_master.pbl", null);
                WebUtil.RetrieveDDDW(dw_detail_02, "own_province", "sl_collateral_master.pbl", null);
                WebUtil.RetrieveDDDW(dw_review, "pos_province", "sl_collateral_master.pbl", null);
                WebUtil.RetrieveDDDW(dw_collddetail, "pos_province", "sl_collateral_master.pbl", null);
                ShowDistrict();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);

            }

        }

        private void JsCalmortgage()
        {
            try
            {
                double loanrequest_amt = 0;
                string loanrequest_docno = dw_main.GetItemString(1, "loanrequest_docno");
                string collmastno = dw_list.GetItemString(1, "collmast_no");
               
                string sql_lnreq = "select loanrequest_amt from lnreqloan where loanrequest_docno = '" + loanrequest_docno + "'";
                Sdt dtlnreq = WebUtil.QuerySdt(sql_lnreq);
                if( dtlnreq.Next()){
                    loanrequest_amt = dtlnreq.GetDouble("loanrequest_amt");
                }
                string sql_coll = " select sum( nvl(coll_balance,0) + nvl(coll_lockamt,0) ) as sumcoll_amt, count( ref_collno) as count_coll from lnreqloancoll where loanrequest_docno = '" + loanrequest_docno + "' and loancolltype_code = '04'";
                Sdt dtcoll = WebUtil.QuerySdt(sql_coll);
                int count_coll = 0;
                double sumcoll_amt = 0;
                if (dtcoll.Next())
                {
                    sumcoll_amt = dtcoll.GetDouble("sumcoll_amt");
                    count_coll = Convert.ToInt16(dtcoll.GetDecimal("count_coll"));

                }
                string sql_update = "";
                if (count_coll == 1)
                {
                    sql_update = " update lncollmaster set mortgage_price = " + loanrequest_amt + " where loanrequest_docno = '" + loanrequest_docno + "' ";
                    WebUtil.ExeSQL(sql_update);
                }
                else { 
                    double coll_amt = 0, coll_percent = 0, mortgage_price = 0, coll_total = 0, loanreq_balance = 0;
                    string ref_collno = "";
                    string sql_select = " select ref_collno, coll_balance  + coll_lockamt as coll_amt from lnreqloancoll where loanrequest_docno = '" + loanrequest_docno + "' and loancolltype_code = '04' ";
                    Sdt dt = WebUtil.QuerySdt( sql_select);
                    int countk = 0;
                    loanreq_balance = loanrequest_amt;
                    while ( dt.Next())
                    {
                        countk++;
                        ref_collno = dt.GetString("ref_collno");
                        coll_amt = dt.GetDouble("coll_amt");
                        coll_percent = (coll_amt / sumcoll_amt);
                        mortgage_price = loanrequest_amt * coll_percent ;
                        if (mortgage_price > loanrequest_amt) { mortgage_price = loanrequest_amt; }
                        if ((mortgage_price % 1000) > 0)
                        {
                            mortgage_price = mortgage_price - (mortgage_price % 1000);
                        }
                        if (mortgage_price > loanreq_balance || (countk == count_coll))
                        { 
                            mortgage_price = loanreq_balance;
                        }
                        loanreq_balance = loanreq_balance - mortgage_price;
                        coll_total = coll_total + mortgage_price;
                        sql_update = " update lncollmaster set mortgage_price = " + mortgage_price + " where collmast_no = '" + ref_collno + "'";
                        WebUtil.ExeSQL(sql_update);

                    }
                
                }

            }
            catch { 
            
            }
            JsPostMember();
        }
        public void JsRefresh()
        {


        }

        //JS-EVENT
        private void JsPostInsertRow()
        {
            dw_list.Reset();
            dw_list.InsertRow(1);
            dw_detail.Reset();
            dw_head.Reset();
            dw_head.InsertRow(1);
            dw_head.SetItemDate(1, "mortgage_date", state.SsWorkDate);
            dw_head.SetItemDate(1, "redeem_date", state.SsWorkDate);

            if (dw_head.RowCount > 1)
            {
                dw_head.DeleteRow(dw_head.RowCount);
            }
            GetDDDW();

        }

        private void JsCopyDetailownland()
        {
            try
            {
                string collmast_refno = "", collmasttype_code = "", pos_moo = "", pos_tumbol = "", pos_district = "", pos_province = "";
                string card_person = "", memb_name = "", parent_name = "", mate_name = "", mooban = "", addr="",moo = "";
                try
                {
                    collmast_refno = dw_head.GetItemString(1, "collmast_refno");
                    collmasttype_code = dw_head.GetItemString(1, "collmasttype_code");

                    try
                    {
                        card_person = dw_detail_01.GetItemString(1, "mrtg_personid");
                    }
                    catch { card_person = ""; }

                    try
                    {
                        memb_name = dw_detail_01.GetItemString(1, "mrtg_name");
                    }
                    catch { memb_name = ""; }

                    try
                    {
                        parent_name = dw_detail_01.GetItemString(1, "mrtg_parentname");
                    }
                    catch { parent_name = ""; }
                     try
                    {
                        mate_name = dw_detail_01.GetItemString(1, "mrtg_matename");
                    }
                     catch { mate_name = ""; }
                     try
                     {
                         addr = dw_detail_01.GetItemString(1, "mrtg_address");
                     }
                     catch { addr = ""; }
                    try
                    {
                         moo = dw_detail_01.GetItemString(1, "mrtg_moo");
                     }
                     catch { moo = ""; }
                     try
                     {
                         mooban = dw_detail_01.GetItemString(1, "mrtg_village");
                     }
                     catch { mooban = ""; }
                    try
                    {
                        pos_tumbol = dw_detail_01.GetItemString(1, "mrtg_tumbol");
                    }
                    catch { pos_tumbol = ""; }
                    try
                    {
                        pos_district = dw_detail_01.GetItemString(1, "mrtg_district");
                    }
                    catch { pos_district = ""; }
                    try
                    {
                        pos_province = dw_detail_01.GetItemString(1, "mrtg_province");
                    }
                    catch { pos_province = ""; }
                   

                }
                catch
                {

                }

                try
                {
                    int row = dw_detail_02.RowCount;
                    dw_detail_02.SetItemString(row, "own_cardperson", card_person);
                    dw_detail_02.SetItemString(row, "own_fullname", memb_name);
                    dw_detail_02.SetItemString(row, "own_matename", mate_name);
                    dw_detail_02.SetItemString(row, "own_parentname", parent_name);
                    dw_detail_02.SetItemString(row, "own_village", mooban); 
                    dw_detail_02.SetItemString(row, "own_addr", addr);
                    dw_detail_02.SetItemString(row, "own_moo", moo);
                    dw_detail_02.SetItemString(row, "own_tumbol", pos_tumbol);
                    dw_detail_02.SetItemString(row, "own_district", pos_district);
                    dw_detail_02.SetItemString(row, "own_province", pos_province);

                    if (pos_province != "")
                    {

                        //dw_collddetail.SetItemString(1, "pos_district", "");
                        DwUtil.RetrieveDDDW(dw_detail_02, "own_district", "sl_collateral_master.pbl", pos_province);
                    }
                   
                }
                catch
                {

                }


            }
            catch
            {

            }


        }
        private void JsCopyDetailcollmast() {
            try
            {
                string collmast_refno = "", collmasttype_code = "", pos_moo = "", pos_tumbol = "", pos_district = "", pos_province = "";
                string land_docno = "", land_bookno = "", land_pageno = "", land_ravang = "", land_landno = "", land_survey = "", size_rai = "";
                string size_ngan = "", size_wa = "", buildingtype_code = "", building_name = "", roof_struc = "", building_no = "", roof_style = "", upperfloor_spec = "", building_floor = "", building_size = "";
                try
                {
                    collmast_refno = dw_head.GetItemString(1, "collmast_refno");
                    collmasttype_code = dw_head.GetItemString(1, "collmasttype_code");
                    try
                    {
                        pos_tumbol = dw_collddetail.GetItemString(1, "pos_tumbol");
                    }
                    catch { pos_tumbol = ""; }
                    try
                    {
                        pos_district = dw_collddetail.GetItemString(1, "pos_district");
                    }
                    catch { pos_province = ""; }
                    try
                    {
                        pos_province = dw_collddetail.GetItemString(1, "pos_province");
                    }
                    catch { pos_province = ""; }
                    try
                    {
                        land_docno = dw_collddetail.GetItemString(1, "land_docno");
                    }
                    catch { land_docno = ""; }
                    try
                    {
                        land_bookno = dw_collddetail.GetItemString(1, "land_bookno");
                    }
                    catch { land_bookno = ""; }
                    try
                    {
                        land_landno = dw_collddetail.GetItemString(1, "land_landno");
                    }
                    catch { land_landno = ""; }
                    try
                    {
                        land_pageno = dw_collddetail.GetItemString(1, "land_pageno");
                    }
                    catch { land_pageno = ""; }
                    try
                    {
                        land_ravang = dw_collddetail.GetItemString(1, "land_ravang");
                    }
                    catch { land_ravang = ""; }
                    try
                    {
                        land_landno = dw_collddetail.GetItemString(1, "land_landno");
                    }
                    catch { land_survey = ""; }
                    try
                    {
                        land_survey = dw_collddetail.GetItemString(1, "land_survey");
                    }
                    catch { land_survey = ""; }
                    try
                    {
                        size_rai = dw_collddetail.GetItemString(1, "size_rai");
                    }
                    catch { size_rai = ""; }
                    try
                    {
                        size_ngan = dw_collddetail.GetItemString(1, "size_ngan");
                    }
                    catch { size_ngan = ""; }
                    try
                    {
                        size_wa = dw_collddetail.GetItemString(1, "size_wa");
                    }
                    catch { size_wa = ""; }
                    try
                    {
                        pos_moo = dw_collddetail.GetItemString(1, "pos_moo");
                    }
                    catch { pos_moo = ""; }
                    try
                    {
                        buildingtype_code = dw_collddetail.GetItemString(1, "buildingtype_code");
                    }
                    catch { buildingtype_code = ""; }
                    try
                    {
                        building_name = dw_collddetail.GetItemString(1, "building_name");
                    }
                    catch { building_name = ""; }
                    try
                    {
                        building_no = dw_collddetail.GetItemString(1, "building_no");
                    }
                    catch { building_no = ""; }
                    try
                    {
                        upperfloor_spec = dw_collddetail.GetItemString(1, "upperfloor_spec");
                    }
                    catch { upperfloor_spec = ""; }
                    try
                    {
                        building_floor = dw_collddetail.GetItemString(1, "building_floor");
                    }
                    catch { building_floor = ""; }
                    try
                    {
                        building_size = dw_collddetail.GetItemString(1, "building_size");
                    }
                    catch { building_size = ""; }
                    try
                    {
                        roof_style = dw_collddetail.GetItemString(1, "roof_style");
                    }
                    catch { roof_style = ""; }
                    try
                    {
                        roof_struc = dw_collddetail.GetItemString(1, "roof_struc");
                    }
                    catch { roof_struc = ""; }


                }
                catch { 
                
                }

                try
                {
                    int row = dw_review.RowCount;
                    dw_review.SetItemString(row, "collmast_refno", collmast_refno);
                    dw_review.SetItemString(row, "collmasttype_code", collmasttype_code);
                    dw_review.SetItemString(row, "pos_moo", pos_moo);
                    dw_review.SetItemString(row, "pos_tumbol", pos_tumbol);
                    dw_review.SetItemString(row, "pos_district", pos_district);
                    dw_review.SetItemString(row, "pos_province", pos_province);
                    dw_review.SetItemString(row, "land_docno", land_docno);
                    dw_review.SetItemString(row, "land_landno", land_landno);
                    dw_review.SetItemString(row, "land_bookno", land_bookno);
                    dw_review.SetItemString(row, "land_pageno", land_pageno);
                    dw_review.SetItemString(row, "land_ravang", land_ravang);
                    dw_review.SetItemString(row, "land_survey", land_survey);
                    dw_review.SetItemString(row, "size_rai", size_rai);
                    dw_review.SetItemString(row, "size_ngan", size_ngan);
                    dw_review.SetItemString(row, "size_wa", size_wa);
                    dw_review.SetItemString(row, "buildingtype_code", buildingtype_code);
                    dw_review.SetItemString(row, "building_name", building_name);
                    dw_review.SetItemString(row, "building_no", building_no);
                    dw_review.SetItemString(row, "upperfloor_spec", upperfloor_spec);
                    dw_review.SetItemString(row, "building_floor", building_floor);
                    dw_review.SetItemString(row, "building_size", building_size);
                    dw_review.SetItemString(row, "roof_style", roof_style);
                    dw_review.SetItemString(row, "roof_struc", roof_struc);
                }
                catch { 
                
                }




            }
            catch { 
            
            }
        
        
        
        }
        private void JsInsertRowDetail()
        {
            dw_detail.InsertRow(0);
           // dw_detail.SetItemDecimal(dw_detail.RowCount, "memco_no", dw_detail.RowCount);
        }
        private void Jsgetmember()
        {
            String member_no = Hmemco_no.Value;
            int row = dw_detail.RowCount;
            String sql = @" SELECT MBMEMBMASTER.MEMBER_NO,   
                           ( MBMEMBMASTER.MEMB_NAME||'  '||  MBMEMBMASTER.MEMB_SURNAME)as MEMB_NAME
                        FROM MBMEMBMASTER       WHERE   MBMEMBMASTER.MEMBER_NO='" + member_no + "' ";
            DataTable dt = WebUtil.Query(sql);
            if (dt.Rows.Count > 0)
            {
                dw_detail.SetItemString(row, "memco_no", dt.Rows[0]["MEMBER_NO"].ToString());
                dw_detail.SetItemString(row, "mem_name", dt.Rows[0]["MEMB_NAME"].ToString());
            }
        }
        private void JsInsertRowReview()
        {
            dw_review.InsertRow(0);
            int row = dw_review.RowCount;
            string collmast_no = dw_head.GetItemString(1, "collmast_no");
            string coop_id = state.SsCoopControl;
            dw_review.SetItemString(row, "collmast_no", collmast_no);
            dw_review.SetItemString(row, "coop_id", coop_id);
            WebUtil.RetrieveDDDW(dw_review, "pos_province", "sl_collateral_master.pbl", null);
            //dw_review.SetItemDate(1, "review_date", state.SsWorkDate);
            //tdwreview.Eng2ThaiAllRow();
        }
        private void JsInsertRowowndetail()
        {
            dw_detail_02.InsertRow(0);
            int row = dw_detail_02.RowCount;
            string collmast_no = dw_head.GetItemString(1, "collmast_no");
            string coop_id = state.SsCoopControl;
            dw_detail_02.SetItemString(row, "collmast_no", collmast_no);
            dw_detail_02.SetItemString(row, "coop_id", coop_id);
            WebUtil.RetrieveDDDW(dw_detail_02, "own_province", "sl_collateral_master.pbl", null);
            //dw_review.SetItemDate(1, "review_date", state.SsWorkDate);
            //tdwreview.Eng2ThaiAllRow();
        }
        private void JsDeleteRowReview()
        {
            try
            {
                Int32 row = Convert.ToInt32(HDeleteRow.Value);
                dw_review.DeleteRow(row);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
        private void JsDeleteRow()
        {
            try
            {

                Int32 row = Convert.ToInt32(HDeleteRow.Value);
                dw_detail.DeleteRow(row);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }
        //JS-EVENT
        private void JsNewClear()
        {
            dw_main.Reset();
            dw_list.Reset();
            dw_head.Reset();
            dw_detail.Reset();
            dw_detail_01.Reset();
            dw_detail_02.Reset();
            dw_collddetail.Reset();
            dw_main.InsertRow(0);
            dw_list.InsertRow(0);
            dw_head.InsertRow(1);
            dw_collddetail.InsertRow(0);
            dw_detail_01.InsertRow(0);
            dw_detail_02.InsertRow(0);
            dw_head.SetItemDate(1, "mortgage_date", state.SsWorkDate);
            dw_head.SetItemDate(1, "redeem_date", state.SsWorkDate);
            dw_detail_02.SetItemDate(1, "mortgage_date", state.SsWorkDate);

            tdwhead.Eng2ThaiAllRow();
            tdw_detail_02.Eng2ThaiAllRow();
            tdwhead.Eng2ThaiAllRow();
            GetDDDW();

        }
        private void ShowDistrictdetland()
        {
            //แสดงอำเภอ
            try
            {
                String province = dw_review.GetItemString(1, "pos_province").Trim();

                //String mrtgProv = dw_review.GetItemString(1, "mrtg_province").Trim();
               
                if (province != "")
                {

                    //dw_collddetail.SetItemString(1, "pos_district", "");
                    DwUtil.RetrieveDDDW(dw_review, "pos_district", "sl_collateral_master.pbl", province);
                }
                //if (mrtgProv != "")
                //{
                //    //dw_detail_01.SetItemString(1, "mrtg_district", "");
                //    DwUtil.RetrieveDDDW(dw_review, "mrtg_district", "sl_collateral_master.pbl", mrtgProv);
                //}
               
            }
            catch
            { }
        }
        private void ShowDistrict()
        {
            //แสดงอำเภอ
            try
            {
                String province = dw_collddetail.GetItemString(1, "pos_province").Trim();

                String mrtgProv = dw_detail_01.GetItemString(1, "mrtg_province").Trim();
                String autrzProv = dw_detail_01.GetItemString(1, "autrz_province").Trim();
                //String posDist, mrtgDist, autrzDist = "";

                //try { posDist = dw_collddetail.GetItemString(1, "pos_district");  }
                //catch { posDist = ""; }
                //try { mrtgDist = dw_detail_01.GetItemString(1, "mrtg_district"); }
                //catch { mrtgDist = ""; }
                //try { autrzDist = dw_detail_01.GetItemString(1, "autrz_district"); }
                //catch { autrzDist = ""; }
                if (province != "")
                {
                   // DwUtil.RetrieveDDDW(dw_collddetail, "pos_province", "sl_collateral_master.pbl", null);
                    //dw_collddetail.SetItemString(1, "pos_district", "");
                    DwUtil.RetrieveDDDW(dw_collddetail, "pos_district", "sl_collateral_master.pbl", province);
                }
                if (mrtgProv != "")
                {
                    //dw_detail_01.SetItemString(1, "mrtg_district", "");
                    DwUtil.RetrieveDDDW(dw_detail_01, "mrtg_district", "sl_collateral_master.pbl", mrtgProv);
                }
                if (autrzProv != "")
                {
                    //dw_detail_01.SetItemString(1, "autrz_district", "");
                    DwUtil.RetrieveDDDW(dw_detail_01, "autrz_district", "sl_collateral_master.pbl", autrzProv);
                }
                if (dw_review.RowCount > 0)
                {
                    province = dw_review.GetItemString(1, "pos_province").Trim();

                    //String mrtgProv = dw_review.GetItemString(1, "mrtg_province").Trim();

                    if (province != "")
                    {

                        //dw_collddetail.SetItemString(1, "pos_district", "");
                        DwUtil.RetrieveDDDW(dw_review, "pos_district", "sl_collateral_master.pbl", province);
                    }
                }
                if (dw_detail_02.RowCount > 0)
                {
                    province = dw_detail_02.GetItemString(1, "own_province").Trim();

                    //String mrtgProv = dw_review.GetItemString(1, "mrtg_province").Trim();

                    if (province != "")
                    {

                        //dw_collddetail.SetItemString(1, "pos_district", "");
                        DwUtil.RetrieveDDDW(dw_detail_02, "own_district", "sl_collateral_master.pbl", province);
                    }
                }
                DwUtil.RetrieveDDDW(dw_review, "buildingtype_code", "sl_collateral_master.pbl", province);
                DwUtil.RetrieveDDDW(dw_collddetail, "buildingtype_code", "sl_collateral_master.pbl", province);
                
            }
            catch
            { }
        }


    }
}
