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
using DataLibrary;
using Saving.WsCommon;

namespace Saving.Applications.hr
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
        //==================
        //private void JspostSearchGetMember()
        //{
        //    String emplcode = null;
        //    String emplid = null;

        //    emplcode = DwMain.GetItemString(1, "emplcode");

        //    Sta ta = new Sta(sqlca.ConnectionString);
        //    try
        //    {
        //        String sql = @"select emplid from hrnmlemplfilemas where emplcode = '" + emplcode + "'";
        //        Sdt dt = ta.Query(sql);
        //        if (dt.Next())
        //        {
        //            emplid = dt.GetString("emplid");
        //            DwMain.SetItemString(1, "emplid", emplid);
        //            postGetDocument();
        //        }
        //        else
        //        {
        //            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเพิ่มข้อมูลเจ้าหน้าที่รหัส :" + emplcode + " ที่หน้าจอข้อมูลเจ้าหน้าที่ก่อน");
        //            JspostNewClear();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        String err = ex.ToString();
        //    }

        //    ta.Close();
        //}

        //public void SetDwMasterEnable(int protect)
        //{
        //    try
        //    {
        //        if (protect == 1)
        //        {
        //            DwMain.Enabled = false;
        //        }
        //        else
        //        {
        //            DwMain.Enabled = true;
        //        }
        //        int RowAll = int.Parse(DwMain.Describe("Datawindow.Column.Count"));
        //        for (int li_index = 1; li_index <= RowAll; li_index++)
        //        {
        //            DwMain.Modify("#" + li_index.ToString() + ".protect= " + protect.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //  LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
        //    }
        //}


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
            Common comm = WsCalling.Common;
            String maxdoc = comm.GetNewDocNo(state.SsWsPass, doc_code);
            return maxdoc;
        }

        private void JspostDeleteRow()
        {
            int RowAll = DwDetail.RowCount;
            String seq_no = null;
            seq_no = DwDetail.GetItemString(RowAll, "seq_no");
            if (seq_no == "" || seq_no == "Auto")
            {
                DwDetail.DeleteRow(RowAll);
                DwList.DeleteRow(RowAll);
            }
            else
            {
                Sta ta = new Sta(sqlca.ConnectionString);
                try
                {
                    emplid = DwMain.GetItemString(1, "emplid").Trim();
                    //String seq_no = DwDetail.GetItemString(1, "seq_no").Trim();

                    String sql = @"Delete FROM HRNMLDRAWHOSPDEA where seq_no = '" + seq_no + "'";
                    try
                    {
                        ta.Exe(sql);
                        LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเรียบร้อยแล้ว");
                        JspostGetDocument();
                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถลบข้อมูลรายการ " + seq_no + " ได้");
                    }
                }

                catch (Exception ex)
                {
                    String err = ex.ToString();

                }
                ta.Close();
            }
            //Retrieve ...
            JspostGetDocument();
        }

        private void JspostShowDetail()

        {
            int row = Convert.ToInt32(HRow.Value);
            String typedoc, year, month, fromdoc, fromnamedoc, docno;
            String pbl = "hr_arc_master.pbl";
            try
            {
                typedoc = DwMain.GetItemString(1, "arc_master_paper_type");
            }
            catch { typedoc = null; }

            try
            {
                year = Convert.ToString(Convert.ToInt32(DwMain.GetItemString(1, "year")) - 543);
            }
            catch { year = null; }

            try
            {
                month = DwMain.GetItemString(1, "month");
            }
            catch { month = null; }

            try
            {
                fromdoc = DwMain.GetItemString(1, "arc_master_from_doc");
                fromdoc = fromdoc.Trim() + "%";
            }
            catch { fromdoc = null; }

            try
            {
                fromnamedoc = DwMain.GetItemString(1, "arc_master_from_namedoc");
            }
            catch { fromnamedoc = null; }

            try
            {
                docno = DwList.GetItemString(row, "paper_no");
            }
            catch { docno = null; }

            object[] arg1 = new object[6];
            arg1[0] = typedoc;
            arg1[1] = year;
            arg1[2] = month;
            arg1[3] = fromdoc;
            arg1[4] = fromnamedoc;
            arg1[5] = docno;
            DwDetail.Reset();
            DwUtil.RetrieveDataWindow(DwDetail, pbl, tDwDetail, arg1);
            //tDwDetail.Eng2ThaiAllRow();
        }

        private void JspostGetDocument()
        {
            String typedoc, year, month, fromdoc, fromnamedoc, sqlStr;
            String pbl = "hr_arc_master.pbl";
            try
            {
                typedoc = DwMain.GetItemString(1, "arc_master_paper_type");
            }
            catch { typedoc = null; }

            try
            {
                year = Convert.ToString(Convert.ToInt32(DwMain.GetItemString(1, "year")) - 543);
            }
            catch { year = null; }

            try
            {
                month = DwMain.GetItemString(1, "month");
            }
            catch { month = null; }

            try
            {
                fromdoc = DwMain.GetItemString(1, "arc_master_from_doc");
                fromdoc = fromdoc.Trim() + "%";
            }
            catch { fromdoc = null; }

            try
            {
                fromnamedoc = DwMain.GetItemString(1, "arc_master_from_namedoc");
            }
            catch { fromnamedoc = null; }

            if (typedoc == null || year == null || month == null || fromdoc == null || fromnamedoc == null)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลที่ต้องการค้นหาให้ครบ");
            }
            else
            {
                Sta ta = new Sta(sqlca.ConnectionString);

                sqlStr = @" SELECT ARC_MASTER.ARC_SEQ   
                        FROM ARC_DETAILS,ARC_MASTER
                        WHERE ( ARC_DETAILS.ARC_SEQ = ARC_MASTER.ARC_SEQ ) and  
                              ( ( ARC_DETAILS.PAPER_TYPE ='" + typedoc + "' )and ( to_char(ARC_MASTER.DATE_RECEIVE,'yyyy') = '" + year + "' ) AND ( to_char(ARC_MASTER.DATE_RECEIVE,'MM') ='" + month + "') AND  ( ARC_MASTER.FROM_DOC like '" + fromdoc + "' ) AND  ( ARC_MASTER.FROM_NAMEDOC = '" + fromnamedoc + "' ))";
                Sdt dt_docrow = ta.Query(sqlStr);
                if (dt_docrow.GetRowCount() < 0)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลเอกสารนี้ กรุณาเลือกรายการใหม่");
                    JspostNewClear();
                }
                else
                {
                    object[] arg1 = new object[5];
                    arg1[0] = typedoc;
                    arg1[1] = year;
                    arg1[2] = month;
                    arg1[3] = fromdoc;
                    arg1[4] = fromnamedoc;
                    DwList.Reset();
                    DwUtil.RetrieveDataWindow(DwList, pbl, null, arg1);
                    DwDetail.Reset();
                    DwDetail.InsertRow(0);
                    //DwUtil.RetrieveDataWindow(DwDetail, pbl, null, arg1);
                }
                ta.Exe(sqlStr);
            }
        }

        private void JspostNewClear()
        {
            DwMain.Reset();
            DwMain.InsertRow(0);
            DwList.Reset();
            DwDetail.Reset();
            DwDetail.InsertRow(0);
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

            //====================================



            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("date_doc", "tdate_doc");
            tDwDetail.Add("date_receive", "tdate_receive");
            //tDwDetail.Eng2ThaiAllRow();
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            DwList.SetTransaction(sqlca);
            DwDetail.SetTransaction(sqlca);


            if (!IsPostBack)
            {
                JspostNewClear();
                //tDwDetail.Eng2ThaiAllRow();
                // JspostRetrieveDDW();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
                this.RestoreContextDw(DwDetail);
            }
            //tDwDetail.Eng2ThaiAllRow();
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
            //else if (eventArg == "postSearchGetMember")
            //{
            //    JspostSearchGetMember();
            //}
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
                String paper_type, paper_no, refer, attract, from_doc, from_namedoc, to_doc, to_namedoc, title, detail, time_receive, level_speed, level_secret, document_group, receive_sent = null;
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
                        String sql = @"INSERT INTO ARC_MASTER (paper_type, paper_no, record_no, date_doc, date_receive, from_doc, to_doc, level_speed, level_secret, title, from_namedoc, to_namedoc, arc_seq)";
                        sql += " VALUES ('" + paper_type + "', '" + paper_no + "', '" + record_no + "', to_date('" + date_doc_dd + "/" + date_doc_mm + "/" + date_doc_yyyy + "' ,'dd/mm/yyyy'), to_date('" + date_receive_dd + "/" + date_receive_mm + "/" + date_receive_yyyy + "','dd/mm/yyyy'),'" + from_doc + "' , '" + to_doc + "', '" + level_speed + "' ,'" + level_secret + "','" + title + "','" + from_namedoc + "','" + to_namedoc + "','" + record_no + "')";
                        ta.Exe(sql);

                        String sql1 = @"INSERT INTO ARC_DETAILS (arc_seq, amount_no, time_receive, document_group, receive_sent, detail, refer, attract, paper_type)";
                        sql1 += " VALUES ('" + record_no + "'," + amount_no + ", '" + time_receive + "', '" + document_group + "','" + receive_sent + "' , '" + detail + "', '" + refer + "' ,'" + attract + "','" + paper_type + "')";
                        ta.Exe(sql1);

                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
                        //JspostGetDocument();
                        ta.Close();
                    }
                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                }
                else
                {
                    Sta ta = new Sta(sqlca.ConnectionString);
                    String sqlStr;
                    try
                    {
                        sqlStr = @"  UPDATE ARC_MASTER
                             SET paper_type = '" + paper_type + "',paper_no = '" + paper_no + "', date_doc = to_date('" + date_doc_dd + "/" + date_doc_mm + "/" + date_doc_yyyy + "' ,'dd/mm/yyyy'), date_receive = to_date('" + date_receive_dd + "/" + date_receive_mm + "/" + date_receive_yyyy + "','dd/mm/yyyy'), from_doc = '" + from_doc + "', to_doc = '" + to_doc + "', level_speed = '" + level_speed + "', level_secret = '" + level_secret + "',title = '" + title + "', from_namedoc = '" + from_namedoc + "', to_namedoc = '" + to_namedoc + "' WHERE arc_seq = '" + record_no + "'";
                        Sdt dt_update_master = ta.Query(sqlStr);

                        sqlStr = @"  UPDATE ARC_DETAILS
                             SET amount_no = '" + amount_no + "',time_receive = '" + time_receive + "', document_group = '" + document_group + "', receive_sent = '" + receive_sent + "', detail = '" + detail + "', refer = '" + refer + "', attract = '" + attract + "', paper_type = '" + paper_type + "' WHERE arc_seq = '" + record_no + "'";
                        Sdt dt_update_detail = ta.Query(sqlStr);

                        ta.Close();
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกการแก้ไขข้อมูลเสร็จเรียบร้อยแล้ว");
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
