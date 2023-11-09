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
using CommonLibrary;
using Saving.WsInsurance;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_reqinsapnew : PageWebSheet, WebSheet
    {
        protected String postMemberNo;
        private DwThDate tDwmain;
        private String pbl = "as_reqinsapnew.pbl";

        #region WebSheet Members

        public void InitJsPostBack()
        {
            postMemberNo = WebUtil.JsPostBack(this, "postMemberNo");
            tDwmain = new DwThDate(DwMain, this);
            tDwmain.Add("inreq_date", "inreq_tdate");
            tDwmain.Add("birth_date", "birth_tdate");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDateTime(1, "inreq_date", state.SsWorkDate);
                DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
                tDwmain.Eng2ThaiAllRow();
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postMemberNo")
            {
                JsPostMemberNo();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
                DwMain.SetItemString(1, "entry_id", state.SsUsername);
                DwMain.SetItemString(1, "coopbranch_id", state.SsBranchId);
                new WsInsurance.Insurance().SaveInsuRequestNew(state.SsWsPass, pbl, state.SsApplication, state.SsWorkDate, DwMain.Describe("DataWindow.Data.XML"));

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "instype_code", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "insmemb_type", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "membgroup_code", pbl, null);
            }
            catch { }
            DwMain.SaveDataCache();
        }

        #endregion

        private void JsPostMemberNo()
        {
            try
            {
                String instype_code;
                try { instype_code = DwMain.GetItemString(1, "instype_code"); }
                catch { instype_code = ""; }
                if (instype_code == "")
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกประเภทประกัน");
                    DwMain.SetItemString(1,"member_no","");
                
                }else{
                    String membNo = DwMain.GetItemString(1, "member_no");
                    membNo = WebUtil.MemberNoFormat(membNo);
                    DwMain.SetItemString(1, "member_no", membNo);
                    String xmlMaster = DwMain.Describe("DataWindow.Data.XML");
                    Insurance insService = new Insurance();
                    string[] resu = insService.InitInsuRequestNew(state.SsWsPass, pbl, state.SsApplication, xmlMaster);
                    string xmlDwMain = resu[0];
                    string xmlDwLoan = resu[1];
                    DwMain.Reset();
                    DwMain.ImportString(xmlDwMain, Sybase.DataWindow.FileSaveAsType.Xml);
                    tDwmain.Eng2ThaiAllRow();
                    if (!string.IsNullOrEmpty(xmlDwLoan))
                    {
                        DwLoan.Reset();
                        DwLoan.ImportString(xmlDwLoan, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}
