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
using System.Globalization;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNCommon;
using System.Data.SqlClient;
namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmd_ucfconfigitem : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ptdurtslipin.pbl";
        Sdt ta;


        public void InitJsPostBack()
        {
           
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            if (!IsPostBack)
            {
                DwMain.Retrieve();
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            
        }

        public void SaveWebSheet()
        {
            String configitem_code = "", configitem_format = "", up = "";
            Decimal year_flag = 0, branch_flag = 0, dept_flag = 0;
            Decimal group_flag = 0, subgroup_flag = 0, number_flag = 0; 
            int row = 0;
            row = DwMain.RowCount;
            try
            {
                for (int i = 1; i <= row; i++)
                {
                    try { configitem_code = DwMain.GetItemString(i, "configitem_code").Trim(); }
                    catch { }
                    try { configitem_format = DwMain.GetItemString(i, "configitem_format").Trim(); }
                    catch { configitem_format = ""; }
                    try { year_flag = DwMain.GetItemDecimal(i, "year_flag"); }
                    catch { year_flag = 0; }
                    try { branch_flag = DwMain.GetItemDecimal(i, "branch_flag"); }
                    catch { branch_flag = 0; }
                    try { dept_flag = DwMain.GetItemDecimal(i, "dept_flag"); }
                    catch { dept_flag = 0; }
                    try { group_flag = DwMain.GetItemDecimal(i, "group_flag"); }
                    catch { group_flag = 0; }
                    try { subgroup_flag = DwMain.GetItemDecimal(i, "subgroup_flag"); }
                    catch { subgroup_flag = 0; }
                    try { number_flag = DwMain.GetItemDecimal(i, "number_flag"); }
                    catch { number_flag = 0; }

                    up = @"update ptucfconfigitem set configitem_format = '" + configitem_format + "', year_flag = " + year_flag + ", branch_flag = " + branch_flag + @",
                            dept_flag = " + dept_flag + ", group_flag = " + group_flag + ", subgroup_flag = "+ subgroup_flag +@",
                            number_flag = " + number_flag + " where configitem_code = '" + configitem_code + "'";
                    ta = WebUtil.QuerySdt(up);

                    if (configitem_code == "INVNO" || configitem_code == "DURNO")
                    {
                        //update cmdocumentcontrol
                    }
                }

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                DwMain.Retrieve();
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }
    }
}