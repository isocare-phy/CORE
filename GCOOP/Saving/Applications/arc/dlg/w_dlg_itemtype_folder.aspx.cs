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


namespace Saving.Applications.arc
{
    public partial class w_dlg_itemtype_folder : PageWebDialog, WebDialog
    {
        protected String postSaveData;
        protected String postAddrow;
        protected String postDelrow;
        protected string pbl = "hr_constant.pbl";
        private string sqlStr;
       
        public void InitJsPostBack()
        {
            postSaveData = WebUtil.JsPostBack(this, "postSaveData");
            postAddrow = WebUtil.JsPostBack(this, "postAddrow");
            postDelrow = WebUtil.JsPostBack(this, "postDelrow");
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
            else if (eventArg == "postDelrow")
            {
                JsPostDelrow();
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

        private void JsPostDelrow()
        { 
            Sta ta = new Sta(sqlca.ConnectionString);
            string folder_id;

            try
            {
                folder_id = DwMain.GetItemString(Int16.Parse(HD_Del.Value), "folder_id");
            }
            catch{ folder_id = ""; }

            try
            {
                sqlStr = @"DELETE arcucf_folder WHERE folder_id = '" + folder_id + "'";
                ta.Exe(sqlStr);

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อยแล้ว");
                ta.Close();
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถลบได้");
            }
        }
        
        public void JsPostSaveData()
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            string folder_id, folder_name;
            for (int i = 1; i < DwMain.RowCount + 1; i++)
            {
                try
                {
                    folder_id = DwMain.GetItemString(i, "folder_id");
                }
                catch
                {
                    folder_id = "";
                }
                try
                {
                    folder_name = DwMain.GetItemString(i, "folder_name");
                }
                catch
                {
                    folder_name = "";
                }
                //save and update
                try
                {
                    sqlStr = "insert into arcucf_folder (folder_id,folder_name) values ('" + folder_id + "','" + folder_name + "')";
                    ta.Exe(sqlStr);
                }
                catch
                {
                    sqlStr = "update arcucf_folder set folder_id = '" + folder_id + "',folder_name = '" + folder_name + "' where folder_id = '" + folder_id + "'";

                    ta.Exe(sqlStr);
                }
            }
            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อยแล้ว");
            ta.Close();
        }
    }
}