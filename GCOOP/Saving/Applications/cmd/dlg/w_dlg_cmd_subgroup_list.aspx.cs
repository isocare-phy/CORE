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
    public partial class w_dlg_cmd_subgroup_list : PageWebDialog, WebDialog
    {
        protected String postSaveData;
        protected string pbl = "cm_constant_config.pbl";
        private string itemtype_id, domain_id,group_id;
        protected string postSetVar;
        private string sqlStr;
        protected string postDelete;

        #region WebDialog Members

        public void InitJsPostBack()
        {
            postSaveData = WebUtil.JsPostBack(this, "postSaveData");
            postSetVar = WebUtil.JsPostBack(this, "postSetVar");
            postDelete = WebUtil.JsPostBack(this, "postDelete");
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
                try
                {
                    group_id = Request["group_id"];
                }
                catch { }

                object[] args = new object[3];
                args[0] = itemtype_id;
                args[1] = domain_id;
                args[2] = group_id;

                dw_list.InsertRow(0);
                DwUtil.RetrieveDataWindow(dw_list, pbl, null, args);

                HdItemtype_id.Value = itemtype_id;
                HdDomain_id.Value = domain_id;
                HdGroup_id.Value = group_id;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSaveData")
            {
                JsPostSaveData();
            }
            else if (eventArg == "postSetVar") {
                JspostSetVar();
            }
            else if (eventArg == "postDelete") {
                JspostDelete();
            }
        }

        public void WebDialogLoadEnd()
        {
            dw_list.SaveDataCache();
            JspostSetVar();
            this.DisConnectSQLCA();
        }

        public void JsPostSaveData()
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            string itemtype_id, domain_id, group_id, subgroup_id, subgroup_name, titlename;
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
                    subgroup_id = dw_list.GetItemString(i, "subgroup_id");
                }
                catch
                {
                    subgroup_id = "";
                }
                try
                {
                    subgroup_name = dw_list.GetItemString(i, "subgroup_name");
                }
                catch
                {
                    subgroup_name = "";
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
                    sqlStr = "insert into ptnmlsubgroup (itemtype_id,domain_id,group_id,subgroup_id,subgroup_name,titlename) values ('" + itemtype_id + "','" + domain_id + "','" + group_id + "','" + subgroup_id + "','" + subgroup_name + "','" + titlename + "')";
                    ta.Exe(sqlStr);
                }
                catch
                {
                    sqlStr = "update ptnmlsubgroup set subgroup_name = '" + subgroup_name + "',titlename = '" + titlename + "' where itemtype_id = '" + itemtype_id + "' and domain_id = '" + domain_id + "' and group_id = '" + group_id + "' and subgroup_id = '" + subgroup_id + "'";
                    ta.Exe(sqlStr);
                }
            }
            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อยแล้ว");
            ta.Close();
        }
        private void JspostSetVar() {
            Sdt dt;
            string itemtype_name = "", domain_name = "",group_name = "";
            sqlStr = "select itemtype_name from ptucfitemtype where itemtype_id = '" + HdItemtype_id.Value + "'";
            dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Next())
            {
                itemtype_name = dt.Rows[0]["itemtype_name"].ToString();
            }
            sqlStr = "select domain_name from ptnmldomain where itemtype_id = '" + HdItemtype_id.Value + "' and domain_id = '" + HdDomain_id.Value + "'";
            dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Next())
            {
                domain_name = dt.Rows[0]["domain_name"].ToString();
            }
            sqlStr = "select group_name from ptnmlgroup where itemtype_id = '" + HdItemtype_id.Value + "' and domain_id = '" + HdDomain_id.Value + "' and group_id = '"+HdGroup_id.Value+"'";
            dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Next())
            {
                group_name = dt.Rows[0]["group_name"].ToString();
            }
            for (int i = 1; i < dw_list.RowCount+1; i++)
            {
                dw_list.SetItemString(i, "itemtype_id", HdItemtype_id.Value);
                dw_list.SetItemString(i, "domain_id", HdDomain_id.Value);
                dw_list.SetItemString(i, "group_id", HdGroup_id.Value);

                dw_list.SetItemString(i, "itemtype_name", itemtype_name);
                dw_list.SetItemString(i, "domain_name", domain_name);
                dw_list.SetItemString(i, "group_name", group_name);
            }
        }
        private void JspostDelete() {
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                int selectRow = int.Parse(HdSelectRow.Value);
                string itemtype_id = dw_list.GetItemString(selectRow, "itemtype_id");
                string domain_id = dw_list.GetItemString(selectRow, "domain_id");
                string group_id = dw_list.GetItemString(selectRow, "group_id");
                string subgroup_id = dw_list.GetItemString(selectRow, "subgroup_id");
                sqlStr = "delete from ptnmlsubgroup where itemtype_id = '" + itemtype_id + "' and domain_id = '" + domain_id + "' and group_id =  '" + group_id + "' and subgroup_id = '"+subgroup_id+"'";
                ta.Exe(sqlStr);
                LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเรียบร้อยแล้ว");
            }
            catch (Exception ex) {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
            ta.Close();

            dw_list.Reset();
            object[] args = new object[3];
            args[0] = HdItemtype_id.Value;
            args[1] = HdDomain_id.Value;
            args[2] = HdGroup_id.Value;

            dw_list.InsertRow(0);
            DwUtil.RetrieveDataWindow(dw_list, pbl, null, args);

        }
        #endregion
    }
}
