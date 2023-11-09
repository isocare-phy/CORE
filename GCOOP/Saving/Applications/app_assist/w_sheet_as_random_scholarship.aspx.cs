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
using System.Timers;
using DataLibrary;
//using System.Numeric;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_random_scholarship : PageWebSheet, WebSheet
    {

        protected String postRandom;
        DataStore Ds;
        Sta ta;
        Sdt dt;

        public void InitJsPostBack()
        {
            postRandom = WebUtil.JsPostBack(this, "postRandom");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            Ds = new DataStore();
            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();

            if (!IsPostBack)
            {
                DwCri.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwCri);
            }
            DwUtil.RetrieveDDDW(DwCri, "capital_year", "as_capital.pbl", null);
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRandom")
            {
                RandomNum();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            DwCri.SaveDataCache();
        }



        private void RandomNum()
        {
            String assist_docno = "", sqlStr;
            Decimal capital_year, level_school, rank, scholarship_r1count, random_done = 0; ;

            capital_year = DwCri.GetItemDecimal(1, "capital_year");
            level_school = DwCri.GetItemDecimal(1, "level_school");

            sqlStr = @"select *
                       from asnrandom_process
                       where capital_year = '" + capital_year + @"' and
                             level_school = '" + level_school + "'";
            dt = ta.Query(sqlStr);
            int rowcount = dt.GetRowCount();
            if (rowcount > 0)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ได้ทำการสุ่มไปแล้ว");
                random_done = 1;
                if (random_done == 1)
                {
                    sqlStr = @"DELETE FROM asnrandom_process WHERE capital_year = '" + capital_year + "' and level_School= '" + level_school + "'";
                    ta.Exe(sqlStr);
                }
            }

            Ds.LibraryList = "C:/GCOOP_ALL/CAT/GCOOP/Saving/DataWindow/app_assist/as_capital.pbl";
            Ds.DataWindowObject = "dw_as_list_random";

            Ds.SetTransaction(sqlca);
            Ds.Retrieve(level_school, capital_year);

            //            sqlStr = @"SELECT scholarship_r1count
            //                               FROM asnscholarship
            //                               WHERE school_group     = '" + level_school + "'";
            //            dt = ta.Query(sqlStr);
            //            dt.Next();
            //            scholarship_r1count = Convert.ToDecimal(dt.GetDouble("scholarship_r1count"));
            scholarship_r1count = DwCri.GetItemDecimal(1, "num_random");

            int[] Ar = new int[Convert.ToInt32(scholarship_r1count)];

            for (int i = 0; i <= scholarship_r1count - 1; i++)
            {
                Random a = new Random();
                int row = a.Next(1, Ds.RowCount);

                assist_docno = Ds.GetItemString(row, "assist_docno");
                rank = i + 1;

                try
                {
                    sqlStr = @"INSERT INTO asnrandom_process(cap_round  ,  capital_year  ,  assist_docno  ,  rank  ,  level_school)
                                            VALUES('" + 1 + "','" + capital_year + "','" + assist_docno + "','" + rank + "','" + level_school + "')";
                    ta.Exe(sqlStr);
                    Ds.DeleteRow(row);
                    LtServerMessage.Text = WebUtil.CompleteMessage("สุ่มเรียบร้อยแล้ว");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }




    }
}