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

namespace Saving.Applications.cmd.dlg
{
    public partial class w_dlg_cmd_itemtype_list : PageWebDialog, WebDialog
    {
        protected String postSaveData;
        protected string pbl = "cm_constant_config.pbl";
        private string sqlStr;
        #region WebDialog Members

        public void InitJsPostBack()
        {
            postSaveData = WebUtil.JsPostBack(this, "postSaveData");
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();
            if (IsPostBack)
            {

                try
                {
                    dw_list.Reset();
                    dw_list.RestoreContext();

                }
                catch { }

            }
            else  {
                dw_list.InsertRow(0);
                DwUtil.RetrieveDataWindow(dw_list, pbl, null, null);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSaveData")
            {
                JsPostSaveData();
            }
        }

        public void WebDialogLoadEnd()
        {
            dw_list.SaveDataCache();
            this.DisConnectSQLCA();
        }

        public void JsPostSaveData()
        {
                Sta ta = new Sta(sqlca.ConnectionString);
                string itemtype_id, itemtype_name, titlename;
                for(int i = 1;i<dw_list.RowCount+1;i++){
                    try
                    {
                        itemtype_id = dw_list.GetItemString(i, "itemtype_id");
                    }
                    catch {
                        itemtype_id = "";
                    }
                    try
                    {
                        itemtype_name = dw_list.GetItemString(i, "itemtype_name");
                    }
                    catch {
                        itemtype_name = "";
                    }
                    try
                    {
                        titlename = dw_list.GetItemString(i, "titlename");
                    }
                    catch {
                        titlename = "";
                    }
                //save and update
                try {
                sqlStr = "insert into ptucfitemtype (itemtype_id,itemtype_name,titlename) values ('" + itemtype_id + "','" + itemtype_name + "','" + titlename + "')";
                ta.Exe(sqlStr);
                }
                catch{
                sqlStr = "update ptucfitemtype set itemtype_name = '" + itemtype_name + "',titlename = '" + titlename + "' where itemtype_id = '" + itemtype_id + "'";
               
                ta.Exe(sqlStr);
                    }
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อยแล้ว");
                ta.Close();
        }

        #endregion
    }
}
