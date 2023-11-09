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
using CoreSavingLibrary.WcfNCommon;
using Sybase.DataWindow;

namespace Saving.Applications.arc
{
    public partial class w_sheet_hr_arc_master_setting : PageWebSheet, WebSheet
    {
        private String emplid;

        private DwThDate tDwDetail;

        protected String postNewClear;
        protected String postGetDocument;
        protected String postAddRow;
        protected String postShowDetail;
        protected String postDeleteRow;
        protected String postSearchGetMember;
        protected String postgetname;
        protected String postgetfield;
        protected String postRetrieveSetgroup;
        protected String postRetrieveSetgroupDetailDD;

        private string ls_sqlext;
        private string is_sql;

        public void InitJsPostBack()
        {
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postGetDocument = WebUtil.JsPostBack(this, "postGetDocument");
            postAddRow = WebUtil.JsPostBack(this, "postAddRow");
            postShowDetail = WebUtil.JsPostBack(this, "postShowDetail");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postSearchGetMember = WebUtil.JsPostBack(this, "postSearchGetMember");
            postgetname = WebUtil.JsPostBack(this, "postgetname");
            postgetfield = WebUtil.JsPostBack(this, "postgetfield");
            postRetrieveSetgroup = WebUtil.JsPostBack(this, "postRetrieveSetgroup");
            postRetrieveSetgroupDetailDD = WebUtil.JsPostBack(this, "postRetrieveSetgroupDetailDD");

            WebUtil.RetrieveDDDW(DwMain, "domain_id", "hr_arc_master.pbl", null);

            WebUtil.RetrieveDDDW(DwDetail, "domain_id", "hr_arc_master.pbl", null);

           

            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("date_doc", "tdate_doc");
        }

        public void WebSheetLoadBegin()
        {
            sqlca = new DwTrans();
            sqlca.Connect();
            DwList.SetTransaction(sqlca);
            is_sql = DwList.GetSqlSelect();
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            DwList.SetTransaction(sqlca);
            DwDetail.SetTransaction(sqlca);
            string sumSEQno = "";



            try
            {
                String sqlStr_seq_runauto;
                Sta ta = new Sta(sqlca.ConnectionString);
                Sdt dt_running_seqgen;

                string Onlyyear = DateTime.Now.Year.ToString();
                sqlStr_seq_runauto = @" select count(1) as max_id from arc_master where (to_char(date_doc, 'yyyy') = '" + Onlyyear + "' and paper_type = '002')";
                dt_running_seqgen = ta.Query(sqlStr_seq_runauto);

                int max_id = Int32.Parse(dt_running_seqgen.Rows[0]["max_id"].ToString());
                if (max_id != 0)
                {
                    sumSEQno = Convert.ToString(max_id + 1) + "/" + (543 + Int32.Parse(Onlyyear)).ToString();
                }
                else
                {
                    sumSEQno = "1/" + (543 + Int32.Parse(Onlyyear)).ToString();
                }

            }
            catch
            {
                LtServerMessage.Text = WebUtil.WarningMessage("ขั้นตอนคำนวณเลขที่เอกสารผิดพลาด");
            }


            if (!IsPostBack)
            {
                JspostNewClear();
                DwDetail.SetItemString(1, "entry_id", state.SsUsername);
                DwDetail.SetItemString(1, "seq_no", sumSEQno);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
                this.RestoreContextDw(DwDetail);
            }
            tDwDetail.Eng2ThaiAllRow();
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            else if (eventArg == "postGetDocument")
            {
                JspostGetDocument();
            }
            else if (eventArg == "postShowDetail")
            {
                JspostShowDetail();
            }
            else if (eventArg == "postDeleteRow")
            {
                JspostDeleteRow();
            }
            else if (eventArg == "postgetname")
            {
                //Jspostgetname();
            }
            else if (eventArg == "postSearchGetMember")
            {
                //JspostSearchGetMember();
            }
            else if (eventArg == "postgetfield")
            {
                //Jspostgetfield();
            }
            else if (eventArg == "postRetrieveSetgroup")
            {
                JspostRetrieveSetgroup();
            }
            else if (eventArg == "postRetrieveSetgroupDetailDD")
            {
                JspostRetrieveSetgroupDetailDD();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                String tdate_doc, tdate_receive = null;
                String pbl = "hr_arc_master.pbl";
                String record_no = DwDetail.GetItemString(1, "record_no").Trim();
                String paper_type, paper_no, refer, attract, title, detail, time_receive, level_speed, level_secret, member_no, place_from, domain_id, seqq_no, folder_id, group_id, receive_sent;
                //////////////////////////////////[RAHM:SECTOR]//////////////////////////////////
                String storage_date, entry_id, edit_id;// from_doc, from_namedoc, to_doc, to_namedoc,document_group,receive_sent = null>>>>STRING CUTTING FROM UPPER
                //////////////////////////////////[RAHM:SECTOR]//////////////////////////////////
                string reason_return, paper_no_return;
                Decimal amount_no = 0;
                DateTime date_doc = new DateTime();
              
                //ตัวแปร วันที่
                String date_doc_dd = null;
                String date_doc_mm = null;
                String date_doc_yyyy = null;

                

                try
                {
                    tdate_doc = DwDetail.GetItemString(1, "tdate_doc");
                }
                catch { tdate_doc = null; }

                try
                {
                    tdate_receive = DwDetail.GetItemString(1, "tdate_receive");
                }
                catch { tdate_receive = null; }

                try
                {
                    paper_type = DwDetail.GetItemString(1, "paper_type");
                }
                catch { paper_type = null; }

                try
                {
                    paper_no = DwDetail.GetItemString(1, "paper_no");
                }
                catch { paper_no = null; }

                try
                {
                    refer = DwDetail.GetItemString(1, "refer");
                }
                catch { refer = null; }

                try
                {
                    attract = DwDetail.GetItemString(1, "attract");
                }
                catch { attract = null; }
                /////////////////////////////////////////////////////
                /*****
                try
                {
                    from_doc = DwDetail.GetItemString(1, "from_doc");
                }
                catch { from_doc = null; }

                try
                {
                    to_doc = DwDetail.GetItemString(1, "to_doc");
                }
                catch { to_doc = null; }

                try
                {
                    from_namedoc = DwDetail.GetItemString(1, "from_namedoc");
                }
                catch { from_namedoc = null; }

                try
                {
                    to_namedoc = DwDetail.GetItemString(1, "to_namedoc");
                }
                catch { to_namedoc = null; }
                *****/
                /////////////////////////////////////////////////////
                try
                {
                    title = DwDetail.GetItemString(1, "title");
                }
                catch { title = null; }

                try
                {
                    detail = DwDetail.GetItemString(1, "detail");
                }
                catch { detail = null; }

                try
                {
                    amount_no = DwDetail.GetItemDecimal(1, "amount_no");
                }
                catch { amount_no = 0; }

                try
                {
                    time_receive = DwDetail.GetItemString(1, "time_receive");
                }
                catch { time_receive = null; }

                try
                {
                    level_speed = DwDetail.GetItemString(1, "level_speed");
                }
                catch { level_speed = null; }

                try
                {
                    level_secret = DwDetail.GetItemString(1, "level_secret");
                }
                catch { level_secret = null; }
                /////////////////////////////////////////////////////////////
                /*****
                try
                {
                   document_group = DwDetail.GetItemString(1, "document_group");
                }
                 catch { document_group = null; }

                
                *****/
                /////////////////////////////////////////////////////////////
                try
                {
                    receive_sent = DwDetail.GetItemString(1, "receive_sent");
                }
                catch { receive_sent = null; }

                try
                {
                    member_no = DwDetail.GetItemString(1, "member_no");
                }
                catch { member_no = null; }

                try
                {
                    place_from = DwDetail.GetItemString(1, "place_from");
                }
                catch { place_from = null; }

                try
                {
                    reason_return = DwDetail.GetItemString(1, "reason_return");
                }
                catch { reason_return = null; }

                try
                {
                    paper_no_return = DwDetail.GetItemString(1, "paper_no_return");
                }
                catch { paper_no_return = null; }
                //////////////////////////////////[RAHM:SECTOR]//////////////////////////////////
                try
                {
                    group_id = DwDetail.GetItemString(1, "group_id");
                }
                catch { group_id = null; }

                try
                {
                    storage_date = DwDetail.GetItemString(1, "storage_tdate");
                }
                catch { storage_date = null; }

                try
                {
                    domain_id = DwDetail.GetItemString(1, "domain_id");
                }
                catch { domain_id = null; }

                try
                {
                    seqq_no = DwDetail.GetItemString(1, "seq_no");
                }
                catch { seqq_no = null; }

                try
                {
                    folder_id = DwDetail.GetItemString(1, "folder_id");
                }
                catch { folder_id = null; }

                try
                {
                    edit_id = DwDetail.GetItemString(1, "edit_id");
                }
                catch { edit_id = null; }
                 

                if (folder_id == null || folder_id == "")
                {
                    folder_id = "0000";
                }

                try
                {
                    entry_id = DwDetail.GetItemString(1, "entry_id");
                }
                catch { entry_id = null; }

                //////////////////////////////////[RAHM:SECTOR]//////////////////////////////////N
                try
                {
                    date_doc = DateTime.ParseExact(DwDetail.GetItemString(1, "tdate_doc"), "ddMMyyyy", WebUtil.TH);
                    date_doc_dd = date_doc.Day.ToString();
                    date_doc_mm = date_doc.Month.ToString();
                    date_doc_yyyy = date_doc.Year.ToString();
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                /****
                try
                {
                    date_folder = DateTime.ParseExact(DwDetail.GetItemString(1, "storage_tdate"), "ddMMyyyy", WebUtil.TH);
                    date_folder_dd = date_folder.Day.ToString();
                    date_folder_mm = date_folder.Month.ToString();
                    date_folder_yyyy = date_folder.Year.ToString();
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                ****/
               

                if (record_no == "Auto" || record_no == null)
                {

                    //new record
                    String sqlStr_run_auto;
                    Sta ta = new Sta(sqlca.ConnectionString);
                    Sdt dt_running_genneratel;
                    DateTime todate = new DateTime();
                    todate = DateTime.Now;
                    String day = (todate.Day.ToString()) + "/" + (todate.Month.ToString()) + "/" + (todate.Year.ToString());

                    try
                    {
                        sqlStr_run_auto = @"SELECT MAX(arc_seq) as mxid  FROM arc_master";
                        dt_running_genneratel = ta.Query(sqlStr_run_auto);
                        if (dt_running_genneratel.Next())
                        {
                            int max = Int16.Parse(dt_running_genneratel.Rows[0]["mxid"].ToString());
                            sqlStr_run_auto = (max + 1).ToString("00000");
                        }
                        else
                        {
                            sqlStr_run_auto = "00001";
                        }
                    }
                    catch
                    {
                        sqlStr_run_auto = "00001";
                    }

                    record_no = sqlStr_run_auto;

                    //record_no = (todate.Year.ToString()) + (todate.Month.ToString()) + (todate.Day.ToString()) + "/" + run;
                    //////////////////////////////////[RAHM:SECTOR]//////////////////////////////////

                    try
                    {
                        string doc_date = "";
                      
                        try
                        {
                            doc_date = DwDetail.GetItemDate(1, "date_doc").ToString("dd/MM/yyyy");
                        }
                        catch
                        {
                            doc_date = state.SsWorkDate.ToString("dd/MM/yyyy");
                        }

                        //string date_fold = DwDetail.GetItemDate(1, "folder_date").ToString("dd/MM/yyyy");
                        String sql = @"INSERT INTO ARC_MASTER (paper_type, paper_no, record_no, level_speed, level_secret, title, arc_seq, member_no, domain_id, seq_no, date_doc, group_id, entry_id)";//
                        sql += " VALUES ('" + paper_type + "', '" + paper_no + "', '" + record_no + "','" + level_speed + "' ,'" + level_secret + "','" + title + "','" + record_no + "','" + member_no + "', '" + domain_id + "', '" + seqq_no + "', to_date('" + doc_date + "','dd/MM/yyyy'), '" + group_id + "', '" + entry_id + "')";//
                        ta.Exe(sql);

                        String sql1 = @"INSERT INTO ARC_DETAILS (arc_seq, amount_no, detail, refer, attract, paper_type, receive_sent)";
                        sql1 += " VALUES ('" + record_no + "', '" + amount_no + "', '" + detail + "', '" + refer + "' ,'" + attract + "','" + paper_type + "', '" + receive_sent + "')";
                        ta.Exe(sql1);

                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
                        JspostNewClear();
                        //JspostGetDocument();
                        ta.Close();
                    }
                    catch { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้"); }
                }

                //////////////////////////////////[RAHM:SECTOR]//////////////////////////////////N

                else
                {

                    Sta ta = new Sta(sqlca.ConnectionString);
                    String sqlStr;
                    try
                    {

                        string doc_date = "";
                       
                        try
                        {
                            doc_date = DwDetail.GetItemDate(1, "date_doc").ToString("dd/MM/yyyy");
                        }
                        catch
                        {
                            doc_date = state.SsWorkDate.ToString("dd/MM/yyyy");
                        }

                       

                        

                        if (paper_type == "002")// ถ้ามีการเปลี่ยนกลับเป็นสมุดรับ(002) 
                        {
                            sqlStr = @"  UPDATE ARC_MASTER
                             SET paper_type = '" + paper_type + "',paper_no = '" + paper_no + "', date_doc = to_date('" + doc_date + "','dd/MM/yyyy'), level_speed = '" + level_speed + "', level_secret = '" + level_secret + "',title = '" + title + "', member_no = '" + member_no + "', domain_id = '" + domain_id + "', group_id = '" + group_id + "', seq_no = '" + seqq_no + "', edit_id = '" + edit_id + "' WHERE arc_seq = '" + record_no + "'";
                            Sdt dt_update_master = ta.Query(sqlStr);
                        }
                        else
                        {
                            sqlStr = @"  UPDATE ARC_MASTER
                             SET paper_type = '" + paper_type + "',paper_no = '" + paper_no + "', date_doc = to_date('" + doc_date + "','dd/MM/yyyy'), level_speed = '" + level_speed + "', level_secret = '" + level_secret + "',title = '" + title + "', member_no = '" + member_no + "', WHERE arc_seq = '" + record_no + "'";
                            Sdt dt_update_master = ta.Query(sqlStr);
                        }
                        sqlStr = @"  UPDATE ARC_DETAILS
                             SET amount_no = '" + amount_no + "', detail = '" + detail + "', refer = '" + refer + "', attract = '" + attract + "', paper_type = '" + paper_type + "',receive_sent = '" + receive_sent + "' WHERE arc_seq = '" + record_no + "'";
                        Sdt dt_update_detail = ta.Query(sqlStr);

                        ta.Close();
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกการแก้ไขข้อมูลเสร็จเรียบร้อยแล้ว");
                        JspostNewClear();
                    }
                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                    //JspostGetMember();

                }


            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                sqlca.Disconnect();
            }
            catch { }

            if (DwMain.RowCount > 1)
            {
                DwMain.DeleteRow(DwMain.RowCount);
            }

            DwMain.SaveDataCache();
            DwList.SaveDataCache();
            DwDetail.SaveDataCache();
            tDwDetail.Eng2ThaiAllRow();
        }

        private void JspostNewClear()
        {
            string lel = "";
            DwMain.Reset();
            DwMain.InsertRow(0);
            DwList.Reset();
            DwDetail.Reset();
            DwDetail.InsertRow(0);
            DwDetail.SetItemDate(1, "date_doc", DateTime.Now);
            //DwDetail.SetItemDate(1, "date_receive", state.SsWorkDate);
            tDwDetail.Eng2ThaiAllRow();
        }
        public void JspostRetrieveSetgroup()
        {
            //string domain_id = DwMain.GetItemString(1, "domain_id");
            //WebUtil.RetrieveDDDW(DwMain, "group_id", "hr_arc_master.pbl", domain_id);
            DataWindowChild child = DwMain.GetChild("group_id");
            child.SetTransaction(sqlca);
            child.Retrieve();
            String domain_id = DwMain.GetItemString(1, "domain_id");
            child.SetFilter("domain_id ='" + domain_id + "'");
            child.Filter();
        }

        public void JspostRetrieveSetgroupDetailDD()
        {
            DataWindowChild child = DwDetail.GetChild("group_id");
            child.SetTransaction(sqlca);
            child.Retrieve();
            String domain_id = DwDetail.GetItemString(1, "domain_id");
            child.SetFilter("domain_id ='" + domain_id + "'");
            child.Filter();
        }

        private void JspostGetDocument()
        {
            String ls_typedoc, ls_year, ls_month, ls_title, ls_member_no, ls_domain_id, ls_group_id, ls_paper_no;
            String ls_letter_no, ls_letter_folder, strFilter1;


            try
            {
                ls_typedoc = DwMain.GetItemString(1, "paper_type");
            }
            catch { ls_typedoc = ""; }

            try
            {
                ls_year = Convert.ToString(Convert.ToInt32(DwMain.GetItemString(1, "year")) - 543);
            }
            catch { ls_year = ""; }

            try
            {
                ls_month = DwMain.GetItemString(1, "month");
                if (ls_month.Length == 1)
                {
                    ls_month = "0" + ls_month;
                }
            }
            catch { ls_month = ""; }

            try
            {
                ls_title = DwMain.GetItemString(1, "title");
            }
            catch { ls_title = ""; }
            try
            {
                ls_member_no = DwMain.GetItemString(1, "member_no");
            }
            catch { ls_member_no = ""; }
            try
            {
                ls_domain_id = DwMain.GetItemString(1, "domain_id");
            }
            catch { ls_domain_id = ""; }

            try
            {
                ls_group_id = DwMain.GetItemString(1, "group_id");
            }
            catch { ls_group_id = ""; }

            try
            {
                ls_letter_no = DwMain.GetItemString(1, "seq_no");
            }
            catch { ls_letter_no = ""; }

            try
            {
                ls_letter_folder = DwMain.GetItemString(1, "folder_id");
            }
            catch { ls_letter_folder = ""; }

            try
            {
                ls_paper_no = DwMain.GetItemString(1, "paper_no");
            }
            catch { ls_paper_no = ""; }

            try
            {
                if (ls_typedoc != "")
                {
                    strFilter1 = "paper_type = '" + ls_typedoc + "'";
                }
                else
                {
                    strFilter1 = "1=1";
                }

                if (ls_year != "")
                {
                    strFilter1 += " AND string(date_doc,'yyyy') = '" + ls_year + "'";
                }
                else
                {
                    strFilter1 += " AND 1=1";
                }

                if (ls_month != "")
                {
                    strFilter1 += " AND string(date_doc,'MM') = '" + ls_month + "'";
                }
                else
                {
                    strFilter1 += " AND 1=1";
                }

                if (ls_member_no != "")
                {
                    strFilter1 += " AND member_no = '" + ls_member_no + "'";
                }
                else
                {
                    strFilter1 += " AND 1=1";
                }

                if (ls_domain_id != "")
                {
                    strFilter1 += " AND domain_id = '" + ls_domain_id + "'";
                }
                else
                {
                    strFilter1 += " AND 1=1";
                }

                if (ls_group_id != "")
                {
                    strFilter1 += " AND group_id = '" + ls_group_id + "'";
                }
                else
                {
                    strFilter1 += " AND 1=1";
                }

                if (ls_letter_no != "")
                {
                    strFilter1 += " AND seq_no = '" + ls_letter_no + "'";
                }
                else
                {
                    strFilter1 += " AND 1=1";
                }

                if (ls_letter_folder != "")
                {
                    strFilter1 += " AND folder_id = '" + ls_letter_folder + "'";
                }
                /***
                if (ls_title != "")
                {
                    strFilter1 += " AND title LIKE '%" + ls_title + "%'";
                }
                else
                {
                    strFilter1 += " AND 1=1";
                }
                ***/
                if (ls_paper_no != "" || ls_title != "")
                {
                    if (ls_paper_no == "" && ls_title != "")
                    {
                        strFilter1 += " AND 1 = 1 AND title LIKE '%" + ls_title + "%'";
                    }
                    else if (ls_title == "" && ls_paper_no != "")
                    {
                        strFilter1 += " AND 1 = 1 AND paper_no LIKE '%" + ls_paper_no + "%'";
                    }
                    else
                    {
                        strFilter1 += " AND 1 = 1 AND title LIKE '%" + ls_title + "%'";
                    }
                }
                else
                {
                    strFilter1 += " AND 1=1";
                }
                //if (ls_paper_no != "" || ls_title != "")
                //{

                //    strFilter1 += " AND ((paper_no LIKE '%" + ls_paper_no + "%') AND (title LIKE '%" + ls_title + "%'))";
                //}
                //else
                //{
                //    strFilter1 += " AND 1=1";
                //}
                DwList.Retrieve();
                DwList.SetFilter(strFilter1);
                DwList.Filter();
                //Jspostgetfield();
            }
            catch
            { }
            if (DwList.RowCount <= 0)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("ไม่พบข้อมูล");
                JspostNewClear();
            }
        }

        private void JspostDeleteRow()
        {
            int RowAll = DwDetail.RowCount;
            String arc_seq = "";
            arc_seq = DwDetail.GetItemString(RowAll, "record_no");
            if (arc_seq == "")
            {
                DwDetail.DeleteRow(RowAll);
                DwList.DeleteRow(RowAll);
            }
            else
            {
                Sta ta = new Sta(sqlca.ConnectionString);
                try
                {
                    try
                    {
                        String sql = @"Delete FROM arc_details where arc_seq = '" + arc_seq + "'";
                        ta.Exe(sql);

                        String sql2 = @"Delete FROM arc_master where arc_seq = '" + arc_seq + "'";
                        ta.Exe(sql2);

                        LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเรียบร้อยแล้ว");

                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถลบเลขทะเบียน " + arc_seq + " ได้");
                    }
                }

                catch (Exception ex)
                {
                    String err = ex.ToString();

                }
                ta.Close();
            }
            //Retrieve ...
            JspostNewClear();
        }

        private void JspostShowDetail()
        {

            int row = Convert.ToInt32(HRow.Value);
            String pbl = "hr_arc_master.pbl";
            string record_no = DwList.GetItemString(row, "record_no");
            string paper_type = DwList.GetItemString(row, "paper_type");
            DwDetail.Retrieve(record_no, paper_type);
            DwDetail.SetItemString(1, "edit_id", state.SsUsername);
            //DwUtil.RetrieveDataWindow(DwDetail, pbl, tDwDetail, record_no, paper_type);

        }

    }
}