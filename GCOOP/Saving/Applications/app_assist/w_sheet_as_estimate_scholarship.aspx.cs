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

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_estimate_scholarship : PageWebSheet, WebSheet
    {
        protected String getCal;
        protected String getInsert;
        protected String postChangeLevel;

        Decimal level_school = 1;

        public void InitJsPostBack()
        {
            getCal = WebUtil.JsPostBack(this, "getCal");
            getInsert = WebUtil.JsPostBack(this, "getInsert");
            postChangeLevel = WebUtil.JsPostBack(this, "postChangeLevel");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                DwCri.Reset();
                DwCri.InsertRow(0);

                //object[] arg = new object[1];
                //arg[0] = 1;

                //DwUtil.RetrieveDataWindow(DwMain, "as_estimate_scholarship.pbl", null, arg);

                //DwMain.SetTransaction(sqlca);
                //DwMain.Retrieve(1);

                //DwMain.SetSort("rank");
                //DwMain.Sort();
            }
            else
            {
                DwMain.RestoreContext();
                DwCri.RestoreContext();
            }
            DwUtil.RetrieveDDDW(DwCri, "capital_year", "as_estimate_scholarship.pbl", null);
        }

        public void CheckJsPostBack(string eventArg)
        {
            // EVENT Postback
            if (eventArg == "getCal")
            {
                GetCalEstimate();
            }
            else if (eventArg == "getInsert")
            {
                GetInsertEstimate();
            }
            else if (eventArg == "postChangeLevel")
            {
                ChangeLevel();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            //DwCri.SaveDataCache();
        }


        private void ChangeLevel()
        {
            Decimal capital_year;

            level_school = DwCri.GetItemDecimal(1, "level_school");
            capital_year = DwCri.GetItemDecimal(1, "capital_year");

            //object[] arg = new object[1];
            //arg[0] = level_school;

            //try
            //{
            //    DwUtil.RetrieveDataWindow(DwMain, "as_estimate_scholarship.pbl", null, arg);
            //}
            //catch { }
            DwMain.SetTransaction(sqlca);
            DwMain.Retrieve(level_school, capital_year);

            DwMain.SetSort("coefficient");
            DwMain.Sort();
        }

        private void GetCalEstimate()
        {
            Decimal child_gpa, salary_amt, son_count, coefficient;

            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                child_gpa = DwMain.GetItemDecimal(i, "child_gpa");
                try
                {
                    salary_amt = DwMain.GetItemDecimal(i, "salary_amt");
                }
                catch { salary_amt = 0; }
                son_count = DwMain.GetItemDecimal(i, "son_count");

                coefficient = salary_amt / (child_gpa * son_count);

                DwMain.SetItemDecimal(i, "coefficient", coefficient);
            }

            DwMain.SetSort("coefficient");
            DwMain.Sort();

            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                DwMain.SetItemDecimal(i, "rank", i);
            }

            Double cap_round, capital_year, rank, assist_amt;
            String assist_docno, strSql; ;

            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                cap_round = 1;
                capital_year = DwMain.GetItemDouble(i, "capital_year");
                coefficient = DwMain.GetItemDecimal(i, "coefficient");
                rank = DwMain.GetItemDouble(i, "rank");
                try
                {
                    assist_amt = DwMain.GetItemDouble(i, "assist_amt");
                }
                catch { assist_amt = 0; }

                assist_docno = DwMain.GetItemString(i, "assist_docno");

                strSql = @"INSERT INTO asnschoolprocess_estimate(cap_round,capital_year,assist_docno,coefficient,rank,assist_amt)
                           VALUES('" + cap_round + "','" + capital_year + "','" + assist_docno + "','" + coefficient + "','" + rank + "','" + assist_amt + "')";
                ta.Exe(strSql);
            }
            DwMain.SetTransaction(sqlca);
            DwMain.Retrieve(level_school);
        }

        private void GetInsertEstimate()
        {
            Decimal child_gpa, salary_amt, son_count, coefficient;

            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                child_gpa = DwMain.GetItemDecimal(i, "child_gpa");
                try
                {
                    salary_amt = DwMain.GetItemDecimal(i, "salary_amt");
                }
                catch { salary_amt = 0; }
                son_count = DwMain.GetItemDecimal(i, "son_count");

                coefficient = salary_amt / (child_gpa * son_count);

                DwMain.SetItemDecimal(i, "coefficient", coefficient);
            }

            DwMain.SetSort("coefficient");
            DwMain.Sort();

            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                DwMain.SetItemDecimal(i, "rank", i);
            }

            Double cap_round, capital_year, rank, assist_amt;
            String assist_docno, strSql; ;

            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                cap_round = 1;
                capital_year = DwMain.GetItemDouble(i, "capital_year");
                coefficient = DwMain.GetItemDecimal(i, "coefficient");
                rank = DwMain.GetItemDouble(i, "rank");
                try
                {
                    assist_amt = DwMain.GetItemDouble(i, "assist_amt");
                }
                catch { assist_amt = 0; }

                assist_docno = DwMain.GetItemString(i, "assist_docno");

                strSql = @"INSERT INTO asnschoolprocess(cap_round,capital_year,assist_docno,coefficient,rank,assist_amt)
                           VALUES('" + cap_round + "','" + capital_year + "','" + assist_docno + "','" + coefficient + "','" + rank + "','" + assist_amt + "')";
                ta.Exe(strSql);
            }
            DwMain.SetTransaction(sqlca);
            DwMain.Retrieve(level_school);

            LtServerMessage.Text = WebUtil.CompleteMessage("ประมวลผลเรียบร้อยแล้ว");
        }
    }
}
