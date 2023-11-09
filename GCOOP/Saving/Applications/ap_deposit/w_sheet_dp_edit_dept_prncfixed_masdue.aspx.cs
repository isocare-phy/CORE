using System;
using CoreSavingLibrary;
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
//using CoreSavingLibrary.WcfNDeposit; 
//using CoreSavingLibrary.WcfNCommon;

using CoreSavingLibrary.WcfNDeposit; //new deposit
using CoreSavingLibrary.WcfNCommon; //new common
using System.Web.Services.Protocols;

using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.ap_deposit
{
    public partial class w_sheet_dp_edit_dept_prncfixed_masdue : PageWebSheet, WebSheet
    {

        protected String postAccountNo;
        protected String postRefresh;

        private n_depositClient ndept; //new deposit
        private String memNo;
        private String accNo;
        private DwThDate tdw_masdue;
        private DwThDate tdw_prncfixed;


        private void JsPostAccountNo()
        {
            try
            {
                String dwAccNo = DwMain.GetItemString(1, "deptaccount_no");
                accNo = wcf.NDeposit.of_analizeaccno(state.SsWsPass, dwAccNo);
                DwMain.Reset();
                int rowRetrieve = DwMain.Retrieve(accNo, HfCoopid.Value);

                DwTabPrncfixed.Reset();


                if (rowRetrieve > 0)
                {
                    DwMain.SetItemString(1, "deptaccount_no", accNo);
                    DwMain.Modify("deptaccount_no.EditMask.Mask='" + WebUtil.GetDeptCodeMask() + "'");

                    memNo = DwMain.GetItemString(1, "member_no");

                    DwUtil.RetrieveDataWindow(DwTabPrncfixed, "dp_edit_dept.pbl", tdw_prncfixed, HfCoopid.Value, accNo);
                    //DwTabPrncfixed.Retrieve( HfCoopid.Value, accNo);


                    DwUtil.RetrieveDataWindow(DwTabMasdue, "dp_edit_dept.pbl", tdw_masdue, HfCoopid.Value, accNo);
                    //DwTabMasdue.Retrieve(HfCoopid.Value, accNo);

                }
                else
                {
                    DwMain.InsertRow(0);
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีเลขที่บัญชี : " + accNo);
                    DwTabPrncfixed.Reset();
                    DwTabMasdue.Reset();
                }
                tdw_prncfixed.Eng2ThaiAllRow();
                tdw_masdue.Eng2ThaiAllRow();

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

  
        private void JsClear()
        {
            DwMain.Reset();
            DwTabPrncfixed.Reset();
            DwTabMasdue.Reset();


            DwMain.InsertRow(0);
        }

        private void JsRefresh()
        {
            int row = int.Parse(HdRowPrncfix.Value);

            String prnc_tdate = DwTabPrncfixed.GetItemString(row, "prnc_tdate");
            DateTime prnc_date = DwTabPrncfixed.GetItemDateTime(row, "prnc_date");

            tdw_prncfixed.Eng2ThaiAllRow();
        }

        #region WebSheet Members

        public void InitJsPostBack()
        {

            postAccountNo = WebUtil.JsPostBack(this, "postAccountNo");
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");

            tdw_prncfixed = new DwThDate(DwTabPrncfixed, this);
            tdw_prncfixed.Add("prnc_date", "prnc_tdate");
            tdw_prncfixed.Add("prncdue_date", "prncdue_tdate");
            tdw_prncfixed.Add("lastcalint_date", "lastcalint_tdate");

            tdw_masdue = new DwThDate(DwTabMasdue, this);
            tdw_masdue.Add("start_date", "start_tdate");
            tdw_masdue.Add("end_date", "end_tdate");

        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            //depServ = wcf.Deposit;
            ndept = wcf.NDeposit;
            DwMain.SetTransaction(sqlca);

            DwTabPrncfixed.SetTransaction(sqlca);

            DwTabMasdue.SetTransaction(sqlca);


            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                HfCoopid.Value = state.SsCoopControl;
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwTabPrncfixed);
                this.RestoreContextDw(DwTabMasdue);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "postAccountNo": JsPostAccountNo(); break;
                case "postRefresh": JsRefresh(); break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                DwUtil.UpdateDataWindow(DwTabPrncfixed, "dp_edit_dept.pbl", "DPDEPTPRNCFIXED");
                DwUtil.UpdateDataWindow(DwTabMasdue, "dp_edit_dept.pbl", "DPDEPTMASDUE");
                JsPostAccountNo();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อย");
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูลการเปลี่ยนแปลงได้");
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwTabPrncfixed.SaveDataCache();
            DwTabMasdue.SaveDataCache();
        }


        #endregion
    }
}
