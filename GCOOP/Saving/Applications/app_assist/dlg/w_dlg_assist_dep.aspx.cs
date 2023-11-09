using System;
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
using CommonLibrary;
using Sybase.DataWindow;

namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_assist_dep : PageWebDialog, WebDialog
    {

        #region WebDialog Members

        public void InitJsPostBack()
        {
        }

        public void WebDialogLoadBegin()
        {
            try
            {
                this.ConnectSQLCA();
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Database ไม่ได้"); }


            Dw_main.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                try
                {
                    String member_no = Request["member_no"].Trim();
                    String row = Request["row"].Trim();
                    HdRow.Value = row;
                    member_no = member_no.Substring(2, 6);
                    Dw_main.Reset();
                    Dw_main.Retrieve(member_no);
                    //if (divpaytype_code == "D00")
                    //{
                    //    Dw_main.SetFilter("save_rectyp2 in('SVP','SAV')");
                    //    Dw_main.Filter();
                    //}
                    //else if (divpaytype_code == "D01")
                    //{
                    //    Dw_main.SetFilter("save_rectyp2 in('FIX','FSP')");
                    //    Dw_main.Filter();
                    //}
                    //else
                    //{
                    //    Dw_main.SetFilter("save_rectyp2 in('SPC','SAV')");
                    //    Dw_main.Filter();
                    //}

                    if (Dw_main.RowCount < 1)
                    {
                        Dw_main.Reset();
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลบัญชีเงินฝาก");
                    }

                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
            }
            else
            {
                this.RestoreContextDw(Dw_main);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void WebDialogLoadEnd()
        {
            Dw_main.SaveDataCache();
        }

        #endregion
    }
}
