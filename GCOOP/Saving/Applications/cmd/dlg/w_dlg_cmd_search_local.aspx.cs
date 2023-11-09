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

namespace Saving.Applications.cmd
{
    public partial class w_dlg_cmd_search_local : PageWebDialog, WebDialog
    {
        protected String postSearchList;

        #region WebDialog Member

        public void InitJsPostBack()
        {
            postSearchList = WebUtil.JsPostBack(this, "postSearchList");
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();
            if (!IsPostBack)
            {
                Dw_main.InsertRow(0);
                dw_detail.InsertRow(0);
                // dw_detail.Retrieve();
            }
            else
            {
                this.RestoreContextDw(Dw_main);
                this.RestoreContextDw(dw_detail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSearchList")
            {
                SearchList();
            }
        }

        public void WebDialogLoadEnd()
        {
            Dw_main.SaveDataCache();
            dw_detail.SaveDataCache();
        }

        #endregion

        #region Function

        private void SearchList()
        {
            String ls_create_id = "", ls_product_name = "", ls_username = "", ls_locate_stock = "";
            String ls_sqlext = "", ls_temp = "", ls_sql = "";
            dw_detail.SetTransaction(sqlca);

            try { ls_create_id = Dw_main.GetItemString(1, "create_id").Trim(); }
            catch { ls_create_id = ""; }
            try { ls_locate_stock = Dw_main.GetItemString(1, "locate_stock").Trim(); }
            catch { ls_locate_stock = ""; }
            try { ls_product_name = Dw_main.GetItemString(1, "product_name").Trim(); }
            catch { ls_product_name = ""; }
            try { ls_username = Dw_main.GetItemString(1, "username").Trim(); }
            catch { ls_username = ""; }

            //if (ls_create_id.Length > 0)
            //{
            //    ls_sqlext = " and (   PTNMLITEMCREATEMAS.DOMAIN_ID= '" + ls_domain + "')and (   PTNMLITEMCREATEMAS.GROUP_ID= '" + ls_group + "') ";
            //}
            if (ls_product_name.Length > 0)
            {
                ls_sqlext += " and (  PTUCFLOCALDEA.ITEM_LIST like '%" + ls_product_name + "%') ";
            }
            if (ls_locate_stock.Length > 0)
            {
                ls_sqlext += " and (  PTUCFLOCALDEA.LOCATE_STOCK  like '%" + ls_locate_stock + "%') ";
            }
            if (ls_username.Length > 0)
            {
                ls_sqlext += " and (  PTUCFLOCALDEA.USERNAME  like '%" + ls_username + "%') ";
            }

            ls_temp = @" SELECT
                         PTNMLPRODUCT.CREATE_ID,                                                     
                         PTNMLPRODUCT.PRODUCT_NAME,                            
                         PTUCFLOCALDEA.LOCATE_STOCK,
                         PTNMLPRODUCT.PRODUCT_ID,
                         PTNMLSTOCKMAS.PRODUCT_BAL
                    FROM PTNMLPRODUCT,   
                         PTNMLSTOCKMAS,   
                         PTUCFLOCALDEA  
                   WHERE ( PTNMLPRODUCT.PRODUCT_ID = PTNMLSTOCKMAS.PRODUCT_ID ) and  
                         ( PTNMLPRODUCT.PRODUCT_ID = PTUCFLOCALDEA.PRODUCT_ID )   
                         " + ls_sqlext + @"
                         GROUP BY PTNMLPRODUCT.CREATE_ID,   
                         PTNMLPRODUCT.PRODUCT_NAME,   
                         PTUCFLOCALDEA.LOCATE_STOCK,   
                         PTNMLSTOCKMAS.PRODUCT_BAL,
                         PTNMLPRODUCT.PRODUCT_ID";
            // HSqlTemp.Value = ls_temp;
            try
            {
                dw_detail.SetSqlSelect(ls_temp);
                dw_detail.Retrieve();
            }
            catch (Exception ex) { ex.ToString(); }

        }

        #endregion
    }
}
