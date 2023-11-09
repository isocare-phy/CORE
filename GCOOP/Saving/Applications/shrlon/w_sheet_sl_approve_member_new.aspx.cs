using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saving.WcfCommon;
using System.Globalization;
using Saving.WcfShrlon;
using Sybase.DataWindow;

namespace Saving.Applications.shrlon
{
    //public partial class w_sheet_sl_approve_member_new : System.Web.UI.Page
    public partial class w_sheet_sl_approve_member_new : PageWebSheet, WebSheet
    {
        private ShrlonClient shrlonService;
        private CommonClient commonService;
        private DwThDate tDwMain;

        protected String rowSelected;
        protected String genNewDoc;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            rowSelected = WebUtil.JsPostBack(this, "rowSelected");
            genNewDoc = WebUtil.JsPostBack(this, "genNewDoc");

            tDwMain = new DwThDate(dw_master);
            tDwMain.Add("approve_date", "approve_tdate");

        }

        public void WebSheetLoadBegin()
        {
            try
            {
                commonService = wcf.Common;
                shrlonService = wcf.Shrlon;

            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            this.ConnectSQLCA();

            if (IsPostBack)
            {
                dw_master.RestoreContext();
            }
            if (dw_master.RowCount < 1)
            {
                this.InitMemberList();
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            //เมื่อกด สร้างเลขทะเบียน
            if (eventArg == "genNewDoc")
            {
                this.getNewDocNo();
            }
        }

        public void SaveWebSheet()
        {
            SaveApvNewMenber();
            dw_master.Reset();
            InitMemberList();

        }

        public void WebSheetLoadEnd()
        {

        }

        #endregion

        private void InitMemberList()
        {
            dw_master.ImportString(shrlonService.InitApvNewMemberList(state.SsWsPass, state.SsCoopId), FileSaveAsType.Xml);



            SetApproveTDateAllRow();
            tDwMain.Eng2ThaiAllRow();

        }

        private void SetApproveTDateAllRow()
        {
            int r = dw_master.RowCount;
            for (int j = 1; j <= r; j++)
            {
                dw_master.SetItemDateTime(j, "approve_date", state.SsWorkDate);
            }
        }

        private void getNewDocNo()
        {
            int count = dw_master.RowCount;
            for (int i = 0; i < count; i++)
            {
                String lastMemberNo = "";
                int indexRow = i + 1;
                int member_type = Convert.ToInt32(dw_master.GetItemDecimal(indexRow, "member_type"));
                int appl_status = Convert.ToInt32(dw_master.GetItemDecimal(indexRow, "appl_status"));
                if (appl_status == 1 && member_type == 1)//อนุมัติ สมาชิก ปกติ
                {
                    try { lastMemberNo = commonService.GetNewDocNo(state.SsWsPass, "MBMEMBERNO"); }
                    catch { }
                    dw_master.SetItemString(indexRow, "member_no", lastMemberNo);
                    dw_master.SetItemDecimal(indexRow, "appl_status", 1);

                }
                else if (appl_status == 1 && member_type == 2)//อนุมัติ สมาชิก สมบท
                {
                    try { lastMemberNo = commonService.GetNewDocNo(state.SsWsPass, "MBMEMBERNO"); }
                    catch { }
                    dw_master.SetItemString(indexRow, "member_no", lastMemberNo);
                    dw_master.SetItemDecimal(indexRow, "appl_status", 1);
                }

            }
        }

        private void SaveApvNewMenber()
        {
            try
            {

                String dwmaster_XML = dw_master.Describe("DataWindow.Data.XML");
                String user_id = state.SsUsername;   //By boom ยังไม่มีเลขสมาชิก
                
                int result = shrlonService.SaveMBreqApv(state.SsWsPass, ref dwmaster_XML, user_id, state.SsWorkDate);
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
