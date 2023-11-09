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
using Sybase.DataWindow;
using DataLibrary;
using System.Web.Services.Protocols;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_distance : PageWebSheet, WebSheet
    {
        protected String postAddDistance;
        protected String postEditDistance;
        protected String postSaveEdit;
        protected String postRefresh;



        public void InitJsPostBack()
        {
            postAddDistance = WebUtil.JsPostBack(this, "postAddDistance");
            postEditDistance = WebUtil.JsPostBack(this, "postEditDistance");
            postSaveEdit = WebUtil.JsPostBack(this, "postSaveEdit");
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                DwAdd.InsertRow(0);
                DwEdit.InsertRow(0);

                DwMain.SetTransaction(sqlca);
                DwMain.Retrieve();
                try
                {
                    DwUtil.RetrieveDDDW(DwAdd, "first_province_1", "as_seminar.pbl", null);
                    DwUtil.RetrieveDDDW(DwAdd, "second_province_1", "as_seminar.pbl", null);
                    DwUtil.RetrieveDDDW(DwEdit, "first_province_1", "as_seminar.pbl", null);
                    DwUtil.RetrieveDDDW(DwEdit, "second_province_1", "as_seminar.pbl", null);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwAdd);
                this.RestoreContextDw(DwEdit);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postAddDistance")
            {
                AddDistance();
            }
            else if (eventArg == "postEditDistance")
            {
                EditDistance();
            }
            else if (eventArg == "postSaveEdit")
            {
                SaveEdit();
            }
            else if (eventArg == "postRefresh")
            {
                Refresh();
            }
        }

        private void Refresh()
        {

        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            DwAdd.SaveDataCache();
            DwMain.SaveDataCache();
            DwEdit.SaveDataCache();
        }



        private void AddDistance()
        {
            String sqlStr, first_province, second_province, p_seq, to_where;
            Decimal Distance;
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            try
            {
                first_province = DwAdd.GetItemString(1, "first_province");
                second_province = DwAdd.GetItemString(1, "second_province");
                to_where = DwAdd.GetItemString(1, "to_where");
                Distance = DwAdd.GetItemDecimal(1, "Distance");

                if (WebUtil.Left(second_province, 1) == "0")
                {
                    second_province = WebUtil.Right(second_province, 1);
                }
                second_province = "p" + second_province;

                sqlStr = @"SELECT MAX(projectdistance.p_seq )AS p_seq
                           FROM   projectdistance
                           WHERE ( projectdistance.province_code = '" + first_province + "' ) ";
                dt = ta.Query(sqlStr);
                dt.Next();
                p_seq = dt.GetString("p_seq");
                p_seq = Convert.ToString(Convert.ToInt32(p_seq) + 1);
                p_seq = "00" + p_seq;

                sqlStr = "INSERT INTO projectdistance(province_code  ,  p_seq  ,  " + second_province + @", district )
                                VALUES ('" + first_province + "','" + p_seq + "','" + Distance + "','" + to_where + "')";
                ta.Exe(sqlStr);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                DwMain.Reset();
                DwMain.SetTransaction(sqlca);
                DwMain.Retrieve();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void EditDistance()
        {
            String sqlStr;
            String first_province, second_province, p_seq;
            Decimal Distance;

            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            first_province = DwEdit.GetItemString(1, "first_province");

            DataWindowChild DcDistrict = DwEdit.GetChild("to_where");
            DcDistrict.SetTransaction(sqlca);
            DcDistrict.Retrieve();
            DcDistrict.SetFilter("province_code='" + first_province + "'");
            DcDistrict.Filter();

            first_province = DwEdit.GetItemString(1, "first_province");
            second_province = DwEdit.GetItemString(1, "second_province");
            p_seq = DwEdit.GetItemString(1, "to_where");

            if (WebUtil.Left(second_province, 1) == "0")
            {
                second_province = WebUtil.Right(second_province, 1);
            }
            second_province = "p" + second_province;

            if (p_seq != "")
            {
                sqlStr = @"SELECT " + second_province + @" as distance
                       FROM projectdistance
                       WHERE province_code = '" + first_province + @"' and
                             p_seq = '" + p_seq + "'";
                dt = ta.Query(sqlStr);
                dt.Next();
                Distance = Convert.ToDecimal(dt.GetDouble("distance"));
                DwEdit.SetItemDecimal(1, "distance", Distance);
            }
        }

        private void SaveEdit()
        {
            String sqlStr, first_province, second_province, p_seq, to_where;
            Decimal Distance;

            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            try
            {
                first_province = DwEdit.GetItemString(1, "first_province");
                second_province = DwEdit.GetItemString(1, "second_province");
                p_seq = DwEdit.GetItemString(1, "to_where");
                Distance = DwEdit.GetItemDecimal(1, "distance");

                if (WebUtil.Left(second_province, 1) == "0")
                {
                    second_province = WebUtil.Right(second_province, 1);
                }
                second_province = "p" + second_province;

                sqlStr = @"UPDATE projectdistance
                       SET " + second_province + " = '" + Distance + @"'
                       WHERE province_code = '" + first_province + @"' and
                                     p_seq = '" + p_seq + "'";
                ta.Exe(sqlStr);

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                DwMain.Reset();
                DwMain.SetTransaction(sqlca);
                DwMain.Retrieve();
            }
            catch(Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}
