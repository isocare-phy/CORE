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
    public partial class w_dlg_cmd_group_list : PageWebDialog, WebDialog
    {
        protected String postSaveData;
        protected string pbl = "cm_constant_config.pbl";
        private string itemtype_id, domain_id;
        protected string postSetVar;
        private string sqlStr;
        #region WebDialog Members

        public void InitJsPostBack()
        {
            postSaveData = WebUtil.JsPostBack(this, "postSaveData");
            postSetVar = WebUtil.JsPostBack(this, "postSetVar");
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
            else
            {
                try
                {
                    itemtype_id = Request["itemtype_id"];
                }
                catch { }
                try
                {
                    domain_id = Request["domain_id"];
                }
                catch { }

                object[] args = new object[2];
                args[0] = itemtype_id;
                args[1] = domain_id;

                dw_list.InsertRow(0);
                DwUtil.RetrieveDataWindow(dw_list, pbl, null, args);

                HdItemtype_id.Value = itemtype_id;
                HdDomain_id.Value = domain_id;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSaveData")
            {
                JsPostSaveData();
            }
            else if (eventArg == "postSetVar")
            {
                JspostSetVar();
            }
        }

        public void WebDialogLoadEnd()
        {
            dw_list.SaveDataCache();
            JspostSetVar();
            this.DisConnectSQLCA();
        }
        
        #endregion

        public void JsPostSaveData()
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            string itemtype_id, domain_id, group_id, group_name, titlename ;
            for (int i = 1; i < dw_list.RowCount + 1; i++)
            {
                try
                {
                    itemtype_id = dw_list.GetItemString(i, "itemtype_id");
                }
                catch
                {
                    itemtype_id = "";
                }
                try
                {
                    domain_id = dw_list.GetItemString(i, "domain_id");
                }
                catch
                {
                    domain_id = "";
                }
                try
                {
                    group_id = dw_list.GetItemString(i, "group_id");
                }
                catch
                {
                    group_id = "";
                }
                try
                {
                    group_name = dw_list.GetItemString(i, "group_name");
                }
                catch
                {
                    group_name = "";
                }
                try
                {
                    titlename = dw_list.GetItemString(i, "titlename");
                }
                catch
                {
                    titlename = "";
                }
                //save and update
                try
                {
                    sqlStr = "insert into ptnmlgroup (itemtype_id,domain_id,group_id,group_name,titlename) values ('" + itemtype_id + "','" + domain_id + "','" + group_id + "','" + group_name + "','" + titlename + "')";
                    ta.Exe(sqlStr);
                }
                catch
                {
                    sqlStr = "update ptnmlgroup set group_name = '" + group_name + "',titlename = '" + titlename + "' where itemtype_id = '" + itemtype_id + "' and domain_id = '" + domain_id + "' and group_id = '" + group_id + "'";

                    ta.Exe(sqlStr);
                }
            }
            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อยแล้ว");
            ta.Close();
        }

        private void JspostSetVar()
        {
            Sdt dt;
            string itemtype_name = "", domain_name = "";
            sqlStr = "select itemtype_name from ptucfitemtype where itemtype_id = '" + HdItemtype_id.Value + "'";
            dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Next()) {
                itemtype_name = dt.Rows[0]["itemtype_name"].ToString();
            }
            sqlStr = "select domain_name from ptnmldomain where itemtype_id = '" + HdItemtype_id.Value + "' and domain_id = '"+HdDomain_id.Value+"'";
            dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Next())
            {
                domain_name = dt.Rows[0]["domain_name"].ToString();
            }
            for (int i = 1; i < dw_list.RowCount+1; i++)
            {
                dw_list.SetItemString(i, "itemtype_id", HdItemtype_id.Value);
                dw_list.SetItemString(i, "domain_id", HdDomain_id.Value);

                dw_list.SetItemString(i, "itemtype_name", itemtype_name);
                dw_list.SetItemString(i, "domain_name", domain_name);
            }
        }
    }
}
