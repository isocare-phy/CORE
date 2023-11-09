using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_detailins : PageWebSheet, WebSheet
    {
        private string is_pblname = "as_detailins.pbl";
        protected String postMemberNo;
        protected string postListRow;
        protected string postInsCost;
        protected string postNewIns;

        public void InitJsPostBack()
        {
            postMemberNo = WebUtil.JsPostBack(this, "postMemberNo");
            postListRow = WebUtil.JsPostBack(this, "postListRow");
            postInsCost = WebUtil.JsPostBack(this, "postInsCost");
            postNewIns = WebUtil.JsPostBack(this, "postNewIns");
        }

        public void WebSheetLoadBegin()
        {
            if (IsPostBack)
            {
                this.RestoreContextDw(dw_mbdetail);
                this.RestoreContextDw(dw_tabcontrol);
                this.RestoreContextDw(dw_list);
                this.RestoreContextDw(dw_insmaster);
                this.RestoreContextDw(dw_insstmt);
                this.RestoreContextDw(dw_insgain);
            }
            else
            {
                dw_mbdetail.InsertRow(0);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postMemberNo")
            {
                JsPostMemberNo();
            }
            else if (eventArg == "postListRow")
            {
                JsPostListRow();
            }
            else if (eventArg == "postInsCost")
            {
                JsPostInsCost();
            }
            else if (eventArg == "postNewIns")
            {
                JsPostNewIns();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string ls_newstatus = dw_tabcontrol.GetItemString(1, "insnew_status");
                if (ls_newstatus == "0")
                {
                    DwUtil.UpdateDateWindow(dw_insmaster, is_pblname, "insgroupmaster");
                    DwUtil.UpdateDateWindow(dw_insstmt, is_pblname, "insgroupstatement");
                    DwUtil.UpdateDateWindow(dw_insgain, is_pblname, "insgroupgain");
                }
                else {
                    DwUtil.InsertDataWindow(dw_insmaster, is_pblname, "insgroupmaster");
                }
                dw_tabcontrol.SetItemString(1, "insnew_status", "0");

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            dw_mbdetail.SaveDataCache();
            dw_tabcontrol.SaveDataCache();
            dw_list.SaveDataCache();
            dw_insmaster.SaveDataCache();
            dw_insstmt.SaveDataCache();
            dw_insgain.SaveDataCache();
        }

        private void JsPostMemberNo()
        {
            string ls_memberno = dw_mbdetail.GetItemString(1, "member_no");
            ls_memberno = WebUtil.MemberNoFormat(ls_memberno);
            dw_mbdetail.SetItemString(1, "member_no", ls_memberno);
            object[] args = new object[1];
            args[0] = ls_memberno;
            DwUtil.RetrieveDataWindow(dw_mbdetail, is_pblname, null, args);
            DwUtil.RetrieveDataWindow(dw_list, is_pblname, null, args);

            dw_tabcontrol.Reset();
            dw_tabcontrol.InsertRow(0);

        }

        private void JsPostListRow()
        {
            int li_row = Convert.ToInt32(HdListRow.Value);
            decimal ldc_insgrpid = dw_list.GetItemDecimal(li_row, "insgroup_id");
            object[] args = new object[] { ldc_insgrpid };
            DwUtil.RetrieveDataWindow(dw_insmaster, is_pblname, null, args);
            DwUtil.RetrieveDataWindow(dw_insstmt, is_pblname, null, args);
            DwUtil.RetrieveDataWindow(dw_insgain, is_pblname, null, args);

        }

        private void JsPostInsCost()
        { 

        }

        private void JsPostNewIns()
        {
            dw_insmaster.InsertRow(0);
            string sql = "select Max(insgroup_id) from insgroupmaster";
            DataTable dt = WebUtil.Query(sql);
            if (dt.Rows.Count > 0)
            {
                String ls_insgrpid = dt.Rows[0][0].ToString().Trim();
                dw_insmaster.SetItemDecimal(1, "insgroup_id", Convert.ToDecimal(ls_insgrpid) +1);
            }
            dw_tabcontrol.SetItemString(1, "insnew_status", "1");

            string ls_memberno = dw_mbdetail.GetItemString(1, "member_no").Trim();
            dw_insmaster.SetItemString(1, "member_no", ls_memberno);
        }
    }
}