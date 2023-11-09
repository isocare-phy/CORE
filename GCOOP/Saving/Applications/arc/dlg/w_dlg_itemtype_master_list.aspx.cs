
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
using CoreSavingLibrary.WcfNCommon;
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.arc.dlg
{
    public partial class w_dlg_itemtype_master_list : PageWebDialog, WebDialog
    {
        protected String postSaveData;
        protected String postAddrow;
        protected string pbl = "hr_constant.pbl";
        private string sqlStr;
       
        public void InitJsPostBack()
        {
            postSaveData = WebUtil.JsPostBack(this, "postSaveData");
            postAddrow = WebUtil.JsPostBack(this, "postAddrow");
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();
            if (IsPostBack)
            {

                try
                {
                    DwMain.Reset();
                    DwMain.RestoreContext();

                }
                catch { }

            }
            else
            {
                DwMain.InsertRow(0);
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, null);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSaveData")
            {
                JsPostSaveData();
            }
            else if (eventArg == "postAddrow")
            {
                JsPostAddrow();
            }
        }

        public void WebDialogLoadEnd()
        {
            DwMain.SaveDataCache();
            this.DisConnectSQLCA();
        }

        private void JsPostAddrow()
        {
            DwMain.InsertRow(0);
        }
        
        public void JsPostSaveData()
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            string domain_id, domain_name;
            for (int i = 1; i < DwMain.RowCount + 1; i++)
            {
                try
                {
                    domain_id = DwMain.GetItemString(i, "domain_id");
                }
                catch
                {
                    domain_id = "";
                }
                try
                {
                    domain_name = DwMain.GetItemString(i, "domain_name");
                }
                catch
                {
                    domain_name = "";
                }
                //save and update
                try
                {
                    sqlStr = "insert into arcucf_domain (domain_id,domain_name) values ('" + domain_id + "','" + domain_name + "')";
                    ta.Exe(sqlStr);
                }
                catch
                {
                    sqlStr = "update arcucf_domain set domain_id = '" + domain_id + "',domain_name = '" + domain_name + "' where domain_id = '" + domain_id + "'";

                    ta.Exe(sqlStr);
                }
            }
            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อยแล้ว");
            ta.Close();
        }
    }
}