using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_as_scholarship_list : PageWebDialog, WebDialog
    {
        protected String postFilter;
        protected String postDelete;

        private Sta ta;
        private Sdt dt;

        public void InitJsPostBack()
        {
            postFilter = WebUtil.JsPostBack(this, "postFilter");
            postDelete = WebUtil.JsPostBack(this, "postDelete");
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();

            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();

            if (!IsPostBack)
            {
                DwCri.InsertRow(0);
                //DwMain.SetTransaction(sqlca);
                //DwMain.Retrieve();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwCri);
            }
            DwUtil.RetrieveDDDW(DwCri, "capital_year", "as_capital.pbl", null);
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postFilter")
            {
                Filter();
            }
            else if (eventArg == "postDelete")
            {
                DeleteRow();
            }
        }


        public void WebDialogLoadEnd()
        {
            DwMain.SaveDataCache();
            DwCri.SaveDataCache();
        }



        private void Filter()
        {
            String strAll, str1 = "1=1", str2 = "1=1", str3 = "1=1";
            String assist_docno, memb_name, memb_surname, member_no;
            Decimal level_school, capital_year;

            //try
            //{
            //    assist_docno = DwCri.GetItemString(1, "assist_docno");
            //}
            //catch { assist_docno = ""; }
            //try
            //{
            //    memb_name = DwCri.GetItemString(1, "memb_name");
            //}
            //catch { memb_name = ""; }
            //try
            //{
            //    memb_surname = DwCri.GetItemString(1, "memb_surname");
            //}
            //catch { memb_surname = ""; }

            //if (assist_docno != "")
            //{
            //    str1 = "assist_docno='" + assist_docno + "'";
            //}
            //if (memb_name != "")
            //{
            //    str2 = "(memb_name like '%" + memb_name + "%')";
            //}
            //if (memb_surname != "")
            //{
            //    str3 = "(memb_surname like '%" + memb_surname + "%')";
            //}
            //strAll = str1 + " and " + str2 + " and " + str3;
            //DwMain.SetFilter(strAll);
            //DwMain.Filter();

            try
            {
                level_school = DwCri.GetItemDecimal(1, "level_school");
            }
            catch { level_school = 99; }
            try
            {
                capital_year = DwCri.GetItemDecimal(1, "capital_year");
            }
            catch { capital_year = 9999; }
            try
            {
                member_no = DwCri.GetItemString(1, "member_no");
            }
            catch { member_no = ""; }

            if (level_school != 0)
            {
                str1 = "level_school=" + level_school + "";
            }
            else { str1 = "1=1"; }
            if (capital_year != 9999)
            {
                str2 = "capital_year =" + capital_year + "";
            }
            else { str2 = "1=1"; }
            if (member_no != "")
            {
                str3 = "member_no ='" + member_no + "'";
            }
            else { str3 = "1=1"; }

            DwMain.SetTransaction(sqlca);
            DwMain.Retrieve();

            strAll = str1 + " and " + str2 + " and " + str3;
            DwMain.SetFilter(strAll);
            DwMain.Filter();
        }

        private void DeleteRow()
        {
            String sqlStr, assist_docno;
            Decimal capital_year;

            assist_docno = HfRow.Value;
            capital_year = DwCri.GetItemDecimal(1, "capital_year");

            try
            {
                sqlStr = @"delete  from  asnmemsalary  
                       where assist_docno = '" + assist_docno + @"'  and
                             capital_year = '" + capital_year + "'";
                ta.Exe(sqlStr);

                sqlStr = @"delete  from  asnreqschooldet 
                       where assist_docno = '" + assist_docno + @"'  and
                             capital_year = '" + capital_year + "'";
                ta.Exe(sqlStr);

                sqlStr = @"delete  from  asnreqmaster
                       where assist_docno = '" + assist_docno + @"'  and
                             capital_year = '" + capital_year + "'";
                ta.Exe(sqlStr);

                LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเรียบร้อย");
            }
            catch
            {
                ta.RollBack();
            }
        }

    }
}
