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
using DBAccess;
using Sybase.DataWindow;
namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_family_dead_cancel : PageWebSheet, WebSheet
    {
        String pbl = "kt_paydead.pbl";
        public void InitJsPostBack()
        {

        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwUtil.RetrieveDataWindow(dw_main, pbl, null);
            }
            else
            {
                this.RestoreContextDw(dw_main);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {

        }

        public void SaveWebSheet()
        {
            try
            {
                string cancel_id = state.SsUsername;
                string cancel_date = state.SsWorkDate.ToString("dd/MM/yyyy");
                for (int i = 1; i <= dw_main.RowCount; i++)
                {
                    string payoutslip_no = dw_main.GetItemString(i, "payoutslip_no");
                    decimal slip_status = dw_main.GetItemDecimal(i, "slip_status");

                    if (slip_status == -9)
                    {
                        string SqlUpdate = "update asnslippayout set cancel_id = '" + cancel_id + "' , cancel_date = to_date('" + cancel_date + "','dd/MM/yyyy') , slip_status = -9 where payoutslip_no = '" + payoutslip_no + "'";
                        WebUtil.Query(SqlUpdate);
                    }
                }
                dw_main.Reset();
                DwUtil.RetrieveDataWindow(dw_main, pbl, null);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.CompleteMessage(ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {
            dw_main.SaveDataCache();
        }
    }
}