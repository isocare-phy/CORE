
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
    public partial class w_dlg_itemtype_secondary : PageWebDialog, WebDialog
    {
      
        protected String postSaveData;
        protected String postAddrow;
        protected string pbl = "hr_constant.pbl";
        private string sqlStr;
        private string itemtype_id;
        Sta ta;


        public void InitJsPostBack()
        {
            postSaveData = WebUtil.JsPostBack(this, "postSaveData");
            postAddrow = WebUtil.JsPostBack(this, "postAddrow");
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);
            if (IsPostBack)
            {

                try
                {
                    DwSecond.Reset();
                    DwSecond.RestoreContext();

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
                DwSecond.InsertRow(0);
                DwUtil.RetrieveDataWindow(DwSecond, pbl, null, itemtype_id);
                HdItemtype_id.Value = itemtype_id;
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
            DwSecond.SaveDataCache();
            JspostSetVar();
            this.DisConnectSQLCA();
            ta.Close();
        }

        private void JsPostAddrow()
        {
            DwSecond.InsertRow(0);
            Generator();
        }

        private void Generator()
        {
            string domain_id = "";
            int getrow ;
            /***
            DataTable dt_g;
            string domain_id = "";
            string sqlStr = "SELECT MAX(group_id) as group_id FROM arcucf_group where domain_id = '" + HdItemtype_id.Value + "'";
            dt_g = ta.Query(sqlStr);
            string max_val = dt_g.Rows[0]["group_id"].ToString();
            if (max_val == "")
            {
                domain_id = HdItemtype_id.Value + "001";
            }
            else
            {
                domain_id = HdItemtype_id.Value + (Int16.Parse(max_val.Substring(2, 3)) + Int16.Parse("1")).ToString("000");
            }
            DwSecond.SetItemString(DwSecond.RowCount, "group_id", domain_id);//+1.ToString("000");
            ***/
            int row = DwSecond.RowCount - 1;
             getrow = row;

            if (row != 0)
            {
                string last_val = DwSecond.GetItemString(row, "group_id");
                domain_id = HdItemtype_id.Value + (Int16.Parse(last_val.Substring(2, 3)) + Int16.Parse("1")).ToString("000");
            }
            else 
            {
                domain_id = HdItemtype_id.Value + "001";
            }
            DwSecond.SetItemString(DwSecond.RowCount, "group_id", domain_id);//+1.ToString("000");
        }

        public void JsPostSaveData()
        {
          
            string domain_id, domain_name, group_id, group_name;
            for (int i = 1; i < DwSecond.RowCount + 1; i++)
            {
                try
                {
                    domain_id = DwSecond.GetItemString(i, "domain_id");
                }
                catch
                {
                    domain_id = "";
                }
                try
                {
                    domain_name = DwSecond.GetItemString(i, "domain_name");
                }
                catch
                {
                    domain_name = "";
                }

                try
                {
                    group_id = DwSecond.GetItemString(i, "group_id");
                }
                catch
                {
                    group_id = "";
                }

                try
                {
                    group_name = DwSecond.GetItemString(i, "group_name");
                }
                catch
                {
                    group_name = "";
                }
                //save and update
                
                try
                {
                    sqlStr = "insert into arcucf_group (domain_id,group_id,group_name) values ('" + HdItemtype_id.Value + "', '" + group_id + "', '" + group_name + "')";
                    ta.Exe(sqlStr);
                }
                catch
                {
                    sqlStr = "update arcucf_group set group_name = '" + group_name + "' where domain_id = '" + HdItemtype_id.Value + "' and group_id = '" + group_id + "'";

                    ta.Exe(sqlStr);
                }
                 
            }
            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อยแล้ว");
            ta.Close();
             
        }
        private void JspostSetVar()
        {
            Sdt dtDomain;
            string domain_name = "";
            sqlStr = "select domain_name from arcucf_domain where domain_id = '" + HdItemtype_id.Value + "'";
            dtDomain = WebUtil.QuerySdt(sqlStr);
            if (dtDomain.Next())
            {
                domain_name = dtDomain.Rows[0]["domain_name"].ToString();
            }
            for (int i = 1; i < DwSecond.RowCount + 1; i++)
            {
                DwSecond.SetItemString(i, "domain_name", domain_name);
                DwSecond.SetItemString(i, "domain_id", HdItemtype_id.Value);
            }
        }
    }
}

       