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
    public partial class w_sheet_mb_apvchg_group : PageWebSheet, WebSheet
    {
        private ShrlonClient shrlonService;
        private DwThDate thDwMaster;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            thDwMaster = new DwThDate(dw_master, this);
            thDwMaster.Add("apv_date", "apv_tdate");
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
            SaveApvChgGroup();
            dw_master.Reset();
            InitDataWindow();
        }

        public void WebSheetLoadEnd()
        {

        }

        #endregion

        private void InitDataWindow()
        {
            try
            {
                dw_master.ImportString(shrlonService.InitApvChangeGrouplist(state.SsWsPass,state.SsCoopControl), FileSaveAsType.Xml);
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
                dw_master.SetItemDateTime(i, "apv_date", state.SsWorkDate);
            }
        }

        private void SaveApvChgGroup()
        {
            try
            {
                String dwmaster_XML = dw_master.Describe("DataWindow.Data.XML");
                String apvId = state.SsUsername;
                int result = shrlonService.SaveApvChangeGroup(state.SsWsPass,state.SsCoopId, dwmaster_XML, apvId, state.SsWorkDate);
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
