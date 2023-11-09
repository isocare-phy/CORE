using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CommonLibrary;
using DBAccess;
using Saving.WsCommon;
using Saving.WsInsurance;
using Saving.WsShrlon;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_reqinsresign : PageWebSheet, WebSheet
    {
        private DwThDate tDwmain;
        protected String jsGetmember;
        protected String postGetins;
        private String pbl = "as_reqinsresign.pbl";
        public void InitJsPostBack()
        {
            jsGetmember = WebUtil.JsPostBack(this, "jsGetmember");
            postGetins = WebUtil.JsPostBack(this, "postGetins");
            tDwmain = new DwThDate(DwMain, this);
            tDwmain.Add("operate_date", "operate_tdate");
            tDwmain.Add("senddoc_date", "senddoc_tdate");
            tDwmain.Add("mbdead_date", "mbdead_tdate");
        }

        public void WebSheetLoadBegin()
        {

            this.ConnectSQLCA();
            if (IsPostBack)
            {

                try
                {
                    DwMain.RestoreContext();
                    dw_list.RestoreContext();


                }
                catch { }
            }
            else
            {
                DwMain.InsertRow(0);
                dw_list.InsertRow(0);
                DwMain.SetItemDate(1, "operate_date", state.SsWorkDate);
                DwMain.SetItemDate(1, "senddoc_date", state.SsWorkDate);
                //  DwMain.SetItemDate(1, "mbdead_date", state.SsWorkDate);
                tDwmain.Eng2ThaiAllRow();
                DwUtil.RetrieveDDDW(DwMain, "instype_code", "as_reqinsresign.pbl", null);
                DwUtil.RetrieveDDDW(DwMain, "insresign_case", "as_reqinsresign.pbl", null);
            }
            if (DwMain.RowCount < 1)

            {
                DwMain.InsertRow(0);
                dw_list.InsertRow(0);

            }

        }


        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsGetmember")
            {
                JsGetmember();
            }
            else if (eventArg == "postGetins")
            {
                JspostGetins();
            }
        }

        private void JspostGetins()
        {
            try
            {
                String membNo = DwMain.GetItemString(1, "member_no");
                membNo = WebUtil.MemberNoFormat(membNo);
                DwMain.SetItemString(1, "member_no", membNo);
                String xmlMaster = DwMain.Describe("DataWindow.Data.XML");
                Insurance insService = new Insurance();
                string[] resu = insService.InitInsuReqResign(state.SsWsPass, pbl, state.SsApplication, xmlMaster);
                string xmlDwMain = resu[0];
              
                DwMain.Reset();
                DwMain.ImportString(xmlDwMain, Sybase.DataWindow.FileSaveAsType.Xml);
                tDwmain.Eng2ThaiAllRow();
              
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
                if (dw_list.RowCount > 1)
                {
                    DwUtil.DeleteLastRow(dw_list);
                }
           
        }
        private void JsGetmember()
        {
            String member_no = hfmember.Value;
            object[] argsDwMain = new object[1] { member_no };
            DwUtil.RetrieveDataWindow(DwMain, "as_reqinsresign.pbl", tDwmain, argsDwMain);
        }

        public void SaveWebSheet()
        {
            try
            {
                Sta ta = new Sta(sqlca.ConnectionString);
                Sdt dt = new Sdt();
                String insreqresign_no = DwMain.GetItemString(1, "insreqresign_no");
                if (insreqresign_no == "auto" || insreqresign_no == null)
                {                    
                
                    DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
                    DwMain.SetItemString(1, "entry_id", state.SsUsername);
                    DwMain.SetItemString(1, "coopbranch_id", state.SsBranchId);
                    new WsInsurance.Insurance().SaveInsuReqResign(state.SsWsPass, pbl, state.SsApplication, state.SsWorkDate, DwMain.Describe("DataWindow.Data.XML"));
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อย");
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                }
            }catch(Exception ex){
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwUtil.RetrieveDDDW(DwMain, "instype_code", "as_reqinsresign.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "insresign_case", "as_reqinsresign.pbl", null);
            //DwMain.SaveDataCache();
            //dw_list.SaveDataCache();
            if (DwMain.RowCount > 1)
            {
                DwUtil.DeleteLastRow(DwMain);
            }
            if (dw_list.RowCount > 1)
            {
                DwUtil.DeleteLastRow(dw_list);
            }
        }

    }
}