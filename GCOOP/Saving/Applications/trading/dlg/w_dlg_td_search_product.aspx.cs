using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Saving;
using DataLibrary;
using Sybase.DataWindow;


namespace Saving.Applications.trading.dlg
{
    public partial class w_dlg_td_search_product : PageWebDialog, WebDialog
    {
        protected String jsSearch;

        public void InitJsPostBack()
        {
            jsSearch = WebUtil.JsPostBack(this, "jsSearch");
        }

        public void WebDialogLoadBegin()
        {
            try
            {
                HdShetRow.Value = Request["sheetrow"].ToString().Trim();
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาดทางเทคนิค กรุณากดใหม่");
                return;
            }
            if (!IsPostBack)
            {
                DwCri.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwCri);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsSearch")
            {
                JsSearch();
            }
        }

        public void WebDialogLoadEnd()
        {
            DwCri.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        public void JsSearch()
        {
            string as_prduct_no = "";
            string as_product_desc = "";

            //เพิ่ม

//            String Sql = @"select * from tdproductmaster 
//                         where product_no = '" + product_no + "'" +
                
//            Sdt dt = WebUtil.QuerySdt(Sql);    

            //

            try
            {
                //แก้ไข 561108
                //as_prduct_no = "%" + DwCri.GetItemString(1, "product_no") + "%";
                as_prduct_no = DwCri.GetItemString(1, "product_no") + "%";
            }
            catch
            {
                as_prduct_no = "%";
            }
            try
            {
                as_product_desc = "%" + DwCri.GetItemString(1, "product_desc") + "%";
            }
            catch
            {
                as_product_desc = "%";
            }

            object[] args = new object[2];
            args[0] = as_prduct_no;
            args[1] = as_product_desc;


            DwUtil.RetrieveDataWindow(DwDetail, "dlg_td_search_product.pbl", null, args);
        }

    }
}