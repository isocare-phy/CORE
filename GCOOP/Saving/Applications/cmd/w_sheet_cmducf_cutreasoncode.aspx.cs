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
using System.Data.OracleClient;
using Sybase.DataWindow;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmducf_cutreasoncode : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ucfcutreasoncode.pbl";
        Sdt ta;

       
        protected String jsPostDelete;


        public void InitJsPostBack()
        {
            
            jsPostDelete = WebUtil.JsPostBack(this, "jsPostDelete");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwDetail.SetTransaction(sqlca);


            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwDetail.Retrieve();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                
                case "jsPostDelete":
                    PostOnDelete();
                    break;

            }
        }

        public void SaveWebSheet()
        {
            try
            {
                String cutreason_code = "", cutreason_desc;

                cutreason_desc = DwMain.GetItemString(1, "cutreason_desc").Trim();
                try
                {
                    String se = @"select max(cutreason_code)as cutreason_code from ptucfcutreasoncode";
                    ta = WebUtil.QuerySdt(se);
                    if (ta.Next())
                    {
                        cutreason_code = ta.GetString("cutreason_code");
                    }
                    if (cutreason_code == null || cutreason_code == "")
                    {
                        cutreason_code = "0";
                    }
                        cutreason_code = Convert.ToString(Convert.ToDecimal(cutreason_code) + 1);
                    
                    while (cutreason_code.Length < 2)
                    {
                        cutreason_code = "0" + cutreason_code;
                    }
                }catch{}
                try
                {
                    String insert = @"insert into ptucfcutreasoncode (cutreason_code,cutreason_desc)
                                values( {0}, {1})";
                    insert = WebUtil.SQLFormat(insert, cutreason_code, cutreason_desc);
                    ta = WebUtil.QuerySdt(insert);
                    DwDetail.Retrieve();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.SetItemString(1, "cutreason_desc",null);

                }
                catch{ }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            }

        

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        private void PostOnDelete()
        {
            String cutreasonCode = "";
            Int32 row = Convert.ToInt32(HdR.Value);
            try
            {
                cutreasonCode = DwDetail.GetItemString(row, "cutreason_code");
                String del = @"delete ptucfcutreasoncode where cutreason_code = {0}";
                del = WebUtil.SQLFormat(del, cutreasonCode);
                ta = WebUtil.QuerySdt(del);
                DwDetail.DeleteRow(row);
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + cutreasonCode + " สำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }
        
    }
}