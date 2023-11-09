
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

namespace Saving.Applications.arc
{
    public partial class w_sheet_hr_arc_master : PageWebSheet, WebSheet
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

        private string ls_sqlext;
        private string is_sql;

        //==================
        private void JspostSearchGetMember()
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            string sqlStr = @"SELECT *
                    FROM mbmembmaster m1, mbucfprename m2, mbucfmembgroup m3
                    WHERE member_no ='" + Hfmember_no.Value + "' and m1.prename_code = m2.prename_code and m1.membgroup_code = m3.membgroup_code";
            Sdt dt_running1 = ta.Query(sqlStr);
            dt_running1.Next();
            string name = dt_running1.GetString("memb_name");
            string sur_name = dt_running1.GetString("memb_surname");
            string prename_desc = dt_running1.GetString("prename_desc");
            string membgroup_code = dt_running1.GetString("membgroup_code");
            string membgroup_desc = dt_running1.GetString("membgroup_desc");
            DwDetail.SetItemString(1, "member_no", Hfmember_no.Value);
            DwDetail.SetItemString(1, "long_name", prename_desc + " " + name + " " + sur_name);
            DwDetail.SetItemString(1, "memgroup", membgroup_code + " " + membgroup_desc);
        }


        protected String GetYear(String doc_code)
        {
            String yy = "";
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                String sql = @"select document_year from cmshrlondoccontrol where Document_Code = '" + doc_code + "'";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    yy = dt.GetString("document_year");
                }
                yy = WebUtil.Mid(yy, 2, 2);
            }
            catch (Exception ex)
            {
                String err = ex.ToString();
            }

            ta.Close();
            return yy;
        }

        protected String GetDocNo(String doc_code)
        {
            n_commonClient comm = wcf.NCommon;
            String maxdoc = comm.of_getnewdocno(state.SsWsPass,state.SsCoopId, doc_code);
            return maxdoc;
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
            Sta ta = new Sta(sqlca.ConnectionString);
            string sqlStr1 = @"SELECT paper_type 
                    FROM arc_master
                    WHERE record_no ='" + record_no + "'";
            Sdt dt_type = ta.Query(sqlStr1);
            dt_type.Next();
            string paper_type = dt_type.GetString("paper_type");

            ls_sqlext = "";
            DwDetail.Reset();
            DwUtil.RetrieveDataWindow(DwDetail, pbl, tDwDetail, record_no, paper_type);

            string sqlStr = @"SELECT *
                    FROM mbmembmaster m1, mbucfprename m2, mbucfmembgroup m3
                    WHERE member_no ='" + DwDetail.GetItemString(1, "member_no") + "' and m1.prename_code = m2.prename_code and m1.membgroup_code = m3.membgroup_code";
            Sdt dt_running1 = ta.Query(sqlStr);
            dt_running1.Next();
            string name = dt_running1.GetString("memb_name");
            string sur_name = dt_running1.GetString("memb_surname");
            string prename_desc = dt_running1.GetString("prename_desc");
            string membgroup_code = dt_running1.GetString("membgroup_code");
            string membgroup_desc = dt_running1.GetString("membgroup_desc");
            DwDetail.SetItemString(1, "long_name", prename_desc + " " + name + " " + sur_name);
            DwDetail.SetItemString(1, "memgroup", membgroup_code + " " + membgroup_desc);
            Jspostgetfield();
        }

        private void JspostGetDocument()
        {
            String ls_typedoc, ls_year, ls_month, ls_fromdoc, ls_fromnamedoc, ls_title, ls_member_no, sqlStr;
            String pbl = "hr_arc_master.pbl";
            try
            {
                ls_typedoc = DwMain.GetItemString(1, "arc_master_paper_type");
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
                ls_fromdoc = DwMain.GetItemString(1, "arc_master_from_doc");
                ls_fromdoc = ls_fromdoc.Trim() + "%";
            }
            catch { ls_fromdoc = ""; }

            try
            {
                ls_fromnamedoc = DwMain.GetItemString(1, "arc_master_from_namedoc");
            }
            catch { ls_fromnamedoc = ""; }

            try
            {
                ls_title = DwMain.GetItemString(1, "arc_master_title");
                ls_title = "%" + ls_title.Trim() + "%";
            }
            catch { ls_title = ""; }
            try
            {
                ls_member_no = DwMain.GetItemString(1, "member_no");
            }
            catch { ls_member_no = ""; }

            if (ls_typedoc.Length > 0)
            {
                ls_sqlext = " and ( ARC_DETAILS.PAPER_TYPE = '" + ls_typedoc + "') ";
            }

            if (ls_year.Length > 0)
            {
                ls_sqlext += " and ( to_char(ARC_MASTER.DATE_RECEIVE,'yyyy') = '" + ls_year + "') ";
            }

            if (ls_month.Length > 0)
            {
                ls_sqlext += " and ( to_char(ARC_MASTER.DATE_RECEIVE,'MM') = '" + ls_month + "') ";
            }

            if (ls_fromdoc.Length > 0)
            {
                ls_sqlext += " and (ARC_MASTER.FROM_DOC like '" + ls_fromdoc + "') ";
            }

            if (ls_fromnamedoc.Length > 0)
            {
                ls_sqlext += " and (ARC_MASTER.FROM_NAMEDOC = '" + ls_fromnamedoc + "') ";
            }

            if (ls_title.Length > 0)
            {
                ls_sqlext += " and (ARC_MASTER.TITLE like '" + ls_title + "') ";
            }

            if (ls_member_no.Length > 0)
            {
                ls_sqlext += " and (ARC_MASTER.member_no = '" + ls_member_no + "') ";
            }
            if (ls_sqlext == null) ls_sqlext = "";

            is_sql = is_sql + ls_sqlext;
            HSqlTemp.Value = is_sql;
            DwList.SetSqlSelect(HSqlTemp.Value);
            DwList.Retrieve();
            int a = DwList.RowCount;
            int i;
            if (a <= 0)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลเอกสารนี้ กรุณาเลือกรายการใหม่");
                JspostNewClear();
            }
            else
            {
                string a1 = DwList.GetItemString(1, "record_no");
                Sta ta = new Sta(sqlca.ConnectionString);
                string sqlStr1 = @"SELECT paper_type 
                    FROM arc_master
                    WHERE record_no ='" + a1 + "'";
                Sdt dt_type = ta.Query(sqlStr1);
                dt_type.Next();
                string paper_type = dt_type.GetString("paper_type");

                object[] arg2 = new object[2];
                arg2[0] = a1;
                arg2[1] = paper_type;
                DwDetail.Reset();
                DwUtil.RetrieveDataWindow(DwDetail, pbl, null, arg2);
                sqlStr = @"SELECT *
                    FROM mbmembmaster m1, mbucfprename m2, mbucfmembgroup m3
                    WHERE member_no ='" + DwDetail.GetItemString(1, "member_no") + "' and m1.prename_code = m2.prename_code and m1.membgroup_code = m3.membgroup_code";
                Sdt dt_running1 = ta.Query(sqlStr);
                dt_running1.Next();
                string name = dt_running1.GetString("memb_name");
                string sur_name = dt_running1.GetString("memb_surname");
                string prename_desc = dt_running1.GetString("prename_desc");
                string membgroup_code = dt_running1.GetString("membgroup_code");
                string membgroup_desc = dt_running1.GetString("membgroup_desc");
                DwDetail.SetItemString(1, "long_name", prename_desc + " " + name + " " + sur_name);
                DwDetail.SetItemString(1, "memgroup", membgroup_code + " " + membgroup_desc);
                Jspostgetfield();
            }

        }

        private void JspostNewClear()
        {
            DwMain.Reset();
            DwMain.InsertRow(0);
            DwList.Reset();
            DwDetail.Reset();
            DwDetail.InsertRow(0);
            DwDetail.SetItemDate(1, "date_doc", state.SsWorkDate);
            DwDetail.SetItemDate(1, "date_receive", state.SsWorkDate);
            tDwDetail.Eng2ThaiAllRow();
        }

        //==================
        #region WebSheet Members

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

            //====================================

            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("date_doc", "tdate_doc");
            tDwDetail.Add("date_receive", "tdate_receive");
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


            if (!IsPostBack)
            {
                JspostNewClear();
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
            //else if (eventArg == "postAddRow")
            //{
            //    JspostAddRow();
            //}
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
                Jspostgetname();
            }
            else if (eventArg == "postSearchGetMember")
            {
                JspostSearchGetMember();
            }
            else if (eventArg == "postgetfield")
            {
                Jspostgetfield();
            }
        }

        public void Jspostgetfield()
        {
            string paper_type = DwDetail.GetItemString(1, "paper_type");
            if (paper_type == "003")
            {
                String setting1 = DwDetail.Describe("paper_no_return.Visible");
                DwDetail.Modify("paper_no_return.Visible=true");
                String setting2 = DwDetail.Describe("reason_return.Visible");
                DwDetail.Modify("reason_return.Visible=true");
                String setting3 = DwDetail.Describe("t_27.Visible");
                DwDetail.Modify("t_27.Visible=true");
                String setting4 = DwDetail.Describe("t_26.Visible");
                DwDetail.Modify("t_26.Visible=true");
            }

            else
            {
                String setting1 = DwDetail.Describe("paper_no_return.Visible");
                DwDetail.Modify("paper_no_return.Visible=false");
                String setting2 = DwDetail.Describe("reason_return.Visible");
                DwDetail.Modify("reason_return.Visible=false");
                String setting3 = DwDetail.Describe("t_27.Visible");
                DwDetail.Modify("t_27.Visible=false");
                String setting4 = DwDetail.Describe("t_26.Visible");
                DwDetail.Modify("t_26.Visible=false");
            }
        }

        public void Jspostgetname()
        {
            try
            {
                string sqlStr;
                Sta ta = new Sta(sqlca.ConnectionString);
                sqlStr = @"SELECT *
                    FROM mbmembmaster m1, mbucfprename m2, mbucfmembgroup m3
                    WHERE member_no ='" + DwDetail.GetItemString(1, "member_no") + "' and m1.prename_code = m2.prename_code and m1.membgroup_code = m3.membgroup_code";
                Sdt dt_running1 = ta.Query(sqlStr);
                if (dt_running1.GetRowCount() == 0)
                {
                    LtServerMessage.Text = WebUtil.WarningMessage("ไม่มีเลขสมาชิกนี้");
                }
                else
                {
                    dt_running1.Next();
                    string name = dt_running1.GetString("memb_name");
                    string sur_name = dt_running1.GetString("memb_surname");
                    string prename_desc = dt_running1.GetString("prename_desc");
                    string membgroup_code = dt_running1.GetString("membgroup_code");
                    string membgroup_desc = dt_running1.GetString("membgroup_desc");
                    DwDetail.SetItemString(1, "long_name", prename_desc + " " + name + " " + sur_name);
                    DwDetail.SetItemString(1, "memgroup", membgroup_code + " " + membgroup_desc);
                }
            }
            catch { }
        }

        public void SaveWebSheet()
        {
            //Check values before save record
            //check new record or edit row
            //tDwMain.Eng2ThaiAllRow();
            try
            {
                String tdate_doc, tdate_receive = null;
                String pbl = "hr_arc_master.pbl";
                String record_no = DwDetail.GetItemString(1, "record_no").Trim();
                String paper_type, paper_no, refer, attract, from_doc, from_namedoc, to_doc, to_namedoc, title, detail, time_receive, level_speed, level_secret, document_group, member_no, place_from, receive_sent = null;
                string reason_return, paper_no_return;
                Decimal amount_no = 0;
                DateTime date_doc = new DateTime();
                DateTime date_receive = new DateTime();

                //ตัวแปร วันที่
                String date_doc_dd = null;
                String date_doc_mm = null;
                String date_doc_yyyy = null;

                String date_receive_dd = null;
                String date_receive_mm = null;
                String date_receive_yyyy = null;

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

                try
                {
                    document_group = DwDetail.GetItemString(1, "document_group");
                }
                catch { document_group = null; }

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

                try
                {
                    date_doc = DateTime.ParseExact(DwDetail.GetItemString(1, "tdate_doc"), "ddMMyyyy", WebUtil.TH);
                    date_doc_dd = date_doc.Day.ToString();
                    date_doc_mm = date_doc.Month.ToString();
                    date_doc_yyyy = date_doc.Year.ToString();
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                try
                {
                    date_receive = DateTime.ParseExact(DwDetail.GetItemString(1, "tdate_receive"), "ddMMyyyy", WebUtil.TH);
                    date_receive_dd = date_receive.Day.ToString();
                    date_receive_mm = date_receive.Month.ToString();
                    date_receive_yyyy = date_receive.Year.ToString();
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                if (record_no == "Auto" || record_no == null)
                {
                    //new record
                    String sqlStr, run;
                    Sta ta = new Sta(sqlca.ConnectionString);
                    DateTime todate = new DateTime();
                    todate = DateTime.Now;
                    String day = (todate.Day.ToString()) + "/" + (todate.Month.ToString()) + "/" + (todate.Year.ToString());
                    int length = 0, row_running = 0;
                    decimal a = 0;
                    sqlStr = @"SELECT arc_seq
                    FROM arc_master
                    WHERE date_receive = to_date('" + day + "' ,'dd/mm/yyyy')";
                    Sdt dt_running = ta.Query(sqlStr);
                    row_running = dt_running.GetRowCount();
                    row_running += 1;
                    length = Convert.ToString(row_running).Length;
                    if (length == 1)
                    {
                        run = "0" + Convert.ToString(row_running);
                    }
                    else
                    {
                        run = Convert.ToString(row_running);
                    }
                    record_no = (todate.Year.ToString()) + (todate.Month.ToString()) + (todate.Day.ToString()) + "/" + run;
                    try
                    {
                        String sql = @"INSERT INTO ARC_MASTER (paper_type, paper_no, record_no, date_doc, date_receive, from_doc, to_doc, level_speed, level_secret, title, from_namedoc, to_namedoc, arc_seq, member_no, place_from)";
                        sql += " VALUES ('" + paper_type + "', '" + paper_no + "', '" + record_no + "', to_date('" + date_doc_dd + "/" + date_doc_mm + "/" + date_doc_yyyy + "' ,'dd/mm/yyyy'), to_date('" + date_receive_dd + "/" + date_receive_mm + "/" + date_receive_yyyy + "','dd/mm/yyyy'),'" + from_doc + "' , '" + to_doc + "', '" + level_speed + "' ,'" + level_secret + "','" + title + "','" + from_namedoc + "','" + to_namedoc + "','" + record_no + "','" + member_no + "','" + place_from + "','" + reason_return + "','" + paper_no_return + "')";
                        ta.Exe(sql);

                        String sql1 = @"INSERT INTO ARC_DETAILS (arc_seq, amount_no, time_receive, document_group, receive_sent, detail, refer, attract, paper_type)";
                        sql1 += " VALUES ('" + record_no + "'," + amount_no + ", '" + time_receive + "', '" + document_group + "','" + receive_sent + "' , '" + detail + "', '" + refer + "' ,'" + attract + "','" + paper_type + "')";
                        ta.Exe(sql1);

                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
                        JspostNewClear();
                        //JspostGetDocument();
                        ta.Close();
                    }
                    catch { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้"); }
                }
                else
                {
                    Sta ta = new Sta(sqlca.ConnectionString);
                    String sqlStr;
                    try
                    {
                        if (paper_type == "001")// ถ้ามีการเปลี่ยนกลับเป็นสมุดรับ(001) เคลียร์ค่า 2 ฟิลด์ที่เพิ่มไป
                        {
                            sqlStr = @"  UPDATE ARC_MASTER
                             SET paper_type = '" + paper_type + "',paper_no = '" + paper_no + "', date_doc = to_date('" + date_doc_dd + "/" + date_doc_mm + "/" + date_doc_yyyy + "' ,'dd/mm/yyyy'), date_receive = to_date('" + date_receive_dd + "/" + date_receive_mm + "/" + date_receive_yyyy + "','dd/mm/yyyy'), from_doc = '" + from_doc + "', to_doc = '" + to_doc + "', level_speed = '" + level_speed + "', level_secret = '" + level_secret + "',title = '" + title + "', from_namedoc = '" + from_namedoc + "', to_namedoc = '" + to_namedoc + "', member_no = '" + member_no + "', place_from = '" + place_from + "', reason_return = '', paper_no_return = '' WHERE arc_seq = '" + record_no + "'";
                            Sdt dt_update_master = ta.Query(sqlStr);
                        }
                        else
                        {
                            sqlStr = @"  UPDATE ARC_MASTER
                             SET paper_type = '" + paper_type + "',paper_no = '" + paper_no + "', date_doc = to_date('" + date_doc_dd + "/" + date_doc_mm + "/" + date_doc_yyyy + "' ,'dd/mm/yyyy'), date_receive = to_date('" + date_receive_dd + "/" + date_receive_mm + "/" + date_receive_yyyy + "','dd/mm/yyyy'), from_doc = '" + from_doc + "', to_doc = '" + to_doc + "', level_speed = '" + level_speed + "', level_secret = '" + level_secret + "',title = '" + title + "', from_namedoc = '" + from_namedoc + "', to_namedoc = '" + to_namedoc + "', member_no = '" + member_no + "', place_from = '" + place_from + "', reason_return = '" + reason_return + "', paper_no_return = '" + paper_no_return + "' WHERE arc_seq = '" + record_no + "'";
                            Sdt dt_update_master = ta.Query(sqlStr);
                        }
                        sqlStr = @"  UPDATE ARC_DETAILS
                             SET amount_no = '" + amount_no + "',time_receive = '" + time_receive + "', document_group = '" + document_group + "', receive_sent = '" + receive_sent + "', detail = '" + detail + "', refer = '" + refer + "', attract = '" + attract + "', paper_type = '" + paper_type + "' WHERE arc_seq = '" + record_no + "'";
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

            //if (DwList.RowCount > 0)
            //{
            //    SetDwMasterEnable(1);
            //}
            //else
            //{
            //    SetDwMasterEnable(0);
            //}
            tDwDetail.Eng2ThaiAllRow();
            //tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
            DwDetail.SaveDataCache();

        }
        #endregion

        public string EMPLID { get; set; }
    }
}
