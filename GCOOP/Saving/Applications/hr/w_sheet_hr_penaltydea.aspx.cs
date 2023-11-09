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

namespace Saving.Applications.hr
{
    public partial class w_sheet_hr_penaltydea : PageWebSheet, WebSheet
    {
        private String emplid;


        //POSTBACK
        protected String getEmplid;
        protected String newRecord;
        protected String getDetail;
        protected String deleteRecord;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            getEmplid = WebUtil.JsPostBack(this, "getEmplid");
            newRecord = WebUtil.JsPostBack(this, "newRecord");
            getDetail = WebUtil.JsPostBack(this, "getDetail");
            deleteRecord = WebUtil.JsPostBack(this, "deleteRecord");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            DwList.SetTransaction(sqlca);
            DwDetail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                if (DwMain.RowCount < 1)
                {
                    DwMain.InsertRow(0);
                    DwList.InsertRow(0);
                    DwDetail.InsertRow(0);
                    DwMain.SetItemDate(1, "entry_date", state.SsWorkDate);
                    DwDetail.SetItemDate(1, "penalty_date", state.SsWorkDate);
                }
            }
            else
            {
                try
                {
                    //DwMain.SetItemString(1, "entry_id", state.SsUsername);
                    DwMain.RestoreContext();
                    DwList.RestoreContext();
                    DwDetail.RestoreContext();

                }
                catch { }
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "getEmplid")
            {
                GetEmplid();
            }
            else if (eventArg == "getDetail")
            {
                GetDetail();
            }
            else if (eventArg == "newRecord")
            {
                //comment because of new record , it's user already
                NewRecord();
            }
            else if (eventArg == "deleteRecord")
            {
                DeleteRecord();
            }
        }

       

        public void SaveWebSheet()
        {
            //Check values before save record
            //check new record or edit row

            try
            {
                String empl_tmp = DwMain.GetItemString(1, "emplid").Trim();
                String seq_no = DwDetail.GetItemString(1, "seq_no").Trim();
                String seq_new = "";
                String empl_new = "";

                if (seq_no == "<Auto>" || seq_no == null)
                {
                    // "new record ";
                    //ยังไม่เลือกเจ้าหน้าที่
                    if (empl_tmp == "<Auto>" || empl_tmp == null)
                    {
                        LtServerMessage.Text = "Please select employee!";
                    }
                    else
                    {
                        //new record
                        String seq_tmp = GetDocNo("HRPENALTY");
                        seq_tmp = WebUtil.Right(seq_tmp, 4);
                        seq_new = "PE" + GetYear("HREMPLFILEMAS") + seq_tmp;
                        DwDetail.SetItemString(1, "seq_no", seq_new);
                        DwDetail.SetItemString(1, "emplid", DwMain.GetItemString(1, "emplid"));
                        DwDetail.UpdateData();
                    }
                }
                else
                {
                    // edit data
                    seq_new = seq_no; ;
                    DwDetail.UpdateData();
                }
                //DwMain.SetItemDate(1, "emplbirtdate", state.SsWorkDate)

            }
            catch (Exception e)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(e.ToString());
            }

            GetEmplid();

        }

        public void WebSheetLoadEnd()
        {
            throw new NotImplementedException();
        }

        #endregion

        private void NewRecord()
        {
            DwList.InsertRow(0);
            DwDetail.Reset();
            DwDetail.InsertRow(0);
            DwDetail.SetItemString(1, "seq_no", "<Auto>");
            DwDetail.SetItemDate(1, "penalty_date", state.SsWorkDate);
        }

        private void GetEmplid()
        {
            String emplid = null;
            try
            {
                emplid = DwMain.GetItemString(1, "emplid");
                DwMain.Reset();
                DwMain.Retrieve(emplid);
                DwList.Reset();
                DwList.Retrieve(emplid);
                DwDetail.Reset();
                DwDetail.Retrieve(emplid);
                try
                {
                    DwDetail.SetFilter("seq_no = '" + DwList.GetItemString(1, "seq_no") + "'");
                    DwDetail.Filter();
                    DwDetail.SetItemString(1, "seq_no", DwList.GetItemString(1, "seq_no"));
                }
                catch
                {
                    //คนนี้ยังไม่มีรายการ
                    NewRecord();
                }

            }
            catch
            {
                emplid = null;
                DwMain.Reset();
                DwList.Reset();
                DwDetail.Reset();
                DwMain.InsertRow(0);
                DwList.InsertRow(0);
                DwDetail.InsertRow(0);
                DwList.SetItemDate(1, "penalty_date", state.SsWorkDate);
                DwDetail.SetItemDate(1, "penalty_date", state.SsWorkDate);
            }
        }

        private void GetDetail()
        {
            //รายละเอียด DwDetail
            String seq_no = null;
            try
            {
                seq_no = DwDetail.GetItemString(1, "seq_no");
                emplid = DwMain.GetItemString(1, "emplid");

                DwDetail.Reset();
                DwDetail.Retrieve(emplid);
                DwDetail.SetFilter("seq_no = '" + seq_no + "'");
                DwDetail.Filter();
                DwDetail.SetItemString(1, "seq_no", seq_no);
            }
            catch
            {
                try
                {
                    emplid = DwMain.GetItemString(1, "emplid");
                    DwMain.Reset();
                    DwMain.Retrieve(emplid);
                    DwList.Reset();
                    DwList.Retrieve(emplid);
                    DwDetail.InsertRow(0);
                }
                catch
                {
                    emplid = null;
                    seq_no = null;
                    DwMain.InsertRow(0);
                    DwList.InsertRow(0);
                    DwDetail.InsertRow(0);

                }

            }
        }

        private void DeleteRecord()
        {
            //Delete Record Manual
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                emplid = DwMain.GetItemString(1, "emplid").Trim();
                String seq_no = DwDetail.GetItemString(1, "seq_no").Trim();
                String sql = @"Delete FROM HRNMLPENALTYDEA where seq_no = '" + seq_no + "'";
                //Sta dt = ta.Query(sql);
                try
                {
                    ta.Exe(sql);
                }
                catch
                {
                    LtServerMessage.Text = "Can't Delete Record.";
                }
            }
            catch (Exception ex)
            {
                String err = ex.ToString();

            }
            ta.Close();

            //Retrieve ...
            GetEmplid();
        }

        /// <summary>
        /// ดึงค่าปีของรายการจาก cmshrlondoccontrol 
        /// </summary>
        /// <param name="doc_code">ค่า Document Code</param>
        /// <returns>ปี</returns>
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
    }
}
