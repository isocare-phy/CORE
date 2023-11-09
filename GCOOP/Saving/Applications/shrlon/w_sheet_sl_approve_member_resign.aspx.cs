using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Saving.WcfCommon;
using Saving.WcfShrlon;
using Sybase.DataWindow;

namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_approve_member_resign : PageWebSheet, WebSheet
    {
        private ShrlonClient shrlonService;
        private DwThDate thDwMaster;



        public void InitJsPostBack()
        {
            thDwMaster = new DwThDate(dw_master, this);
            thDwMaster.Add("approve_date", "approve_tdate");
            thDwMaster.Add("resignreq_date", "resignreq_tdate");
        }

        public void WebSheetLoadBegin()
        {

            try
            {
                shrlonService = wcf.Shrlon;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }

            if (IsPostBack)
            {
                dw_master.RestoreContext();
            }
            else
            {
                InitDataWindow();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {

        }

        public void SaveWebSheet()
        {
            int row = dw_master.RowCount;
            if (row > 0)
            {
                SaveApvChgGroup();
                dw_master.Reset();
                InitDataWindow();
            }
        }

        public void WebSheetLoadEnd()
        {

        }



        private void InitDataWindow()
        {
            try
            {

                dw_master.ImportString(shrlonService.InitApvListResign(state.SsWsPass,state.SsCoopId), FileSaveAsType.Xml);
                this.SetAvpDateAllRow();
                thDwMaster.Eng2ThaiAllRow();
            }
            catch { }

        }

        private void SetAvpDateAllRow()
        {
            int allRow = dw_master.RowCount;
            for (int i = 1; i <= allRow; i++)
            {
                dw_master.SetItemDateTime(i, "approve_date", state.SsWorkDate);
            }
        }

        private void SaveApvChgGroup()
        {
            try
            {

                String dwmaster_XML = dw_master.Describe("DataWindow.Data.XML");
                String apvId = state.SsUsername;
                int result = shrlonService.PostApvListResign(state.SsWsPass, ref dwmaster_XML, apvId, state.SsWorkDate);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("สำเร็จ");
                }
                else { LtServerMessage.Text = "ไม่สำเร็จ"; }

            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.ToString()); }
        }
    }
}
