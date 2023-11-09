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
namespace Saving.Applications.cmd.dlg
{
    public partial class w_dlg_st_product_search_disoff : PageWebDialog, WebDialog
    {
        private WebState state;
        private DwTrans sqlca;
        private string ls_sql;

        public void InitJsPostBack()
        {
           
        }

        public void WebDialogLoadBegin()
        {
            state = new WebState(Session, Request);
            if (state.IsLogin)
            {
                sqlca = new DwTrans();
                sqlca.Connect();
                Dw_main.SetTransaction(sqlca);
                dw_detail.SetTransaction(sqlca);
                ls_sql = dw_detail.GetSqlSelect();

                try
                {
                    if (!IsPostBack)
                    {
                        HSqlTemp.Value = ls_sql;
                    }
                }
                catch { }

                dw_detail.SetSqlSelect(HSqlTemp.Value);
                dw_detail.Retrieve();

            }
            else
            {

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            
        }

        public void WebDialogLoadEnd()
        {
            try
            {
                sqlca.Disconnect();
            }
            catch { }
        }
        protected void b_search_Click(object sender, EventArgs e)
        {
            String ls_product_id = "", ls_product_name = "", ls_itemtype_id = "", ls_domain = "", ls_group = "";
            String ls_sqlext = "", ls_temp = "";


            //member_no
            try
            {
                ls_product_id = Dw_main.GetItemString(1, "product_id").Trim();
                ls_domain = WebUtil.Left(ls_product_id, 02);
                ls_group = WebUtil.Right(ls_product_id, 02);

            }
            //memb_name
            catch { ls_product_id = ""; }
            try
            {
                ls_product_name = Dw_main.GetItemString(1, "product_name").Trim();

            }
            //memb_surname
            catch { ls_product_name = ""; }
            try
            {
                ls_itemtype_id = Dw_main.GetItemString(1, "itemtype_id").Trim();

            }
            catch { ls_itemtype_id = ""; }

            if (ls_product_id.Length > 0)
            {

                ls_sqlext = " and (   PTNMLITEMCREATEMAS.DOMAIN_ID= '" + ls_domain + "')and (   PTNMLITEMCREATEMAS.GROUP_ID= '" + ls_group + "') ";
            }
            if (ls_product_name.Length > 0)
            {
                ls_sqlext += " and (  PTNMLPRODUCT.PRODUCT_NAME like '%" + ls_product_name + "%') ";
            }
            if (ls_itemtype_id.Length > 0)
            {
                ls_sqlext += " and (   PTNMLITEMCREATEMAS.ITEMTYPE_ID = '" + ls_itemtype_id + "') ";
            }

            ls_temp = ls_sql + ls_sqlext;
            HSqlTemp.Value = ls_temp;
            dw_detail.SetSqlSelect(HSqlTemp.Value);
            dw_detail.Retrieve();
        }
    }
}