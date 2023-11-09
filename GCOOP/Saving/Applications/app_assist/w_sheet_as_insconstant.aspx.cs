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
using Saving.WsShrlon;
using CommonLibrary;
using Saving.WsCommon;
using System.Web.Services.Protocols;
using DBAccess;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_insconstant : PageWebSheet, WebSheet
    {
        protected String jsinsertrow;
        protected String GetStatus;
        private String pbl = "as_insconstant.pbl";

        public void InitJsPostBack()
        {
            jsinsertrow = WebUtil.JsPostBack(this, "jsinsertrow");
            GetStatus = WebUtil.JsPostBack(this, "GetStatus");
        }

        public void WebSheetLoadBegin()
        {

            this.ConnectSQLCA();
            if (IsPostBack)
            {

                this.RestoreContextDw(dw_search);
                this.RestoreContextDw(dw_list);
            }
            else
            {
                dw_search.InsertRow(0);

                DwUtil.RetrieveDataWindow(dw_search, "pbl", null, null);
                DwUtil.RetrieveDataWindow(dw_list, "pbl", null, null);

            }


        }


        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsinsertrow")
            {
                jsinsertrows();

            }
            if (eventArg == "GetStatus")
            {
                JsPostGetStatus();
            }

        }

        public void SaveWebSheet()
        {
            String value = Hstatus.Value;
            String sql = "select count(instype_code) as insmax from INSLEVELCOST where instype_code='" + value + "'";
            DataTable dt = WebUtil.Query(sql);
            HdStatusRow.Value = Convert.ToString(dt.Rows[0]["insmax"]);
            if (Convert.ToInt32(HdStatusRow.Value) > 0)
            {
                int row = Convert.ToInt32(dt.Rows[0]["insmax"]) + 1;

            }

            int rowCount = dw_list.RowCount;
            int rowFirst = Convert.ToInt32(HdStatusRow.Value);

            if (rowFirst > 0)
            {
                if (rowFirst != rowCount)
                {

                    try
                    {


                        int[] rows = new int[rowCount - rowFirst];
                        int ii = 0;

                        for (int i = rowFirst; i < rowCount; i++)
                        {
                            rows[ii] = i + 1;
                            ii++;
                        }
                        DwUtil.InsertDataWindow(dw_list, pbl, "INSLEVELCOST", rows);
                        DwUtil.RetrieveDataWindow(dw_list, pbl, null, Hstatus.Value);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อย");

                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }

                }
                else
                {
                    try
                    {
                        DwUtil.UpdateDateWindow(dw_list, pbl, "INSLEVELCOST");
                        DwUtil.UpdateDateWindow(dw_search, pbl, "INSURENCETYPE");
                        DwUtil.RetrieveDataWindow(dw_search, pbl, null, Hstatus.Value);
                        DwUtil.RetrieveDataWindow(dw_list, pbl, null, Hstatus.Value);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อย");
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }

                }
            }
            else
            {
                try
                {
                    DwUtil.InsertDataWindow(dw_search, pbl, "INSURENCETYPE");
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อย");
                }
                catch (Exception ex)
                {

                    LtServerMessage.Text = WebUtil.ErrorMessage("dw_search" + ex);
                }
                try
                {
                    DwUtil.InsertDataWindow(dw_list, pbl, "INSLEVELCOST");
                    DwUtil.RetrieveDataWindow(dw_list, pbl, null, Hstatus.Value);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อย");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("dw_list" + ex);
 
                }
            }

        }

        public void WebSheetLoadEnd()
        {
            dw_search.SaveDataCache();
            dw_list.SaveDataCache();

        }
        private void JsPostGetStatus()
        {
            String instype_code = Hstatus.Value;
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt1 = new Sdt();
            String sqlStr = @"  SELECT INSTYPE_CODE  FROM INSURENCETYPE  
                         WHERE INSTYPE_CODE = '" + instype_code + "'   ";

            dt1 = ta.Query(sqlStr);
            if (dt1.GetRowCount() == 0)
            {
                dw_search.SetItemString(1, "instype_code", instype_code);

            }
            else
            {


                DwUtil.RetrieveDataWindow(dw_search, pbl, null, instype_code);
                DwUtil.RetrieveDataWindow(dw_list, pbl, null, instype_code);

            }
        }
        public void jsinsertrows()
        {
            String value = Hstatus.Value;
            dw_list.InsertRow(0);
            int rowNow = dw_list.RowCount;
            dw_list.SetItemString(rowNow, "instype_code", value);

        }
    }
}