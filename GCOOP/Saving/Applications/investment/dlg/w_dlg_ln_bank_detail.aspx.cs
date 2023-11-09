using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.investment.dlg
{
    public partial class w_dlg_sh_bank_detail : PageWebDialog, WebDialog
    {
        String pbl = "sh_memb_linkbank.pbl";
        protected String jsBankClick;
        protected String jsBranchClick;
        protected String jsTamClick;
        public void InitJsPostBack()
        {
            jsBankClick = WebUtil.JsPostBack(this, "jsBankClick");
            jsBranchClick = WebUtil.JsPostBack(this, "jsBranchClick");
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                DwUtil.RetrieveDataWindow(DwBank, pbl, null, null);

            }
            else
            {
                this.RestoreContextDw(DwBank);
                this.RestoreContextDw(DwBranch);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsBankClick":
                    JsBankClick();
                    break;
                case "jsBranchClick":
                    JsBranchClick();
                    break;
            }
        }

        public void WebDialogLoadEnd()
        {
            DwBank.SaveDataCache();
            DwBranch.SaveDataCache();
        }

        public void JsBankClick()
        {
            int r = Convert.ToInt32(HdRow.Value);
            string bank_code = "", bank_desc = "";

            try
            {
                bank_code = DwBank.GetItemString(r, "bank_code");
                bank_desc = DwBank.GetItemString(r, "bank_desc");
                HdBankCode.Value = bank_code;
                HdBankDesc.Value = bank_desc;
                DwUtil.RetrieveDataWindow(DwBranch, pbl, null, bank_code);
                Label1.Text = "กรุณาเลือกสาขา";
            }
            catch
            {
                HdBankCode.Value = "";
                HdBankDesc.Value = "";
                HdBranchCode.Value = "";
                HdBranchDesc.Value = "";
            }
        }
 
        public void JsBranchClick()
        {
            int r = Convert.ToInt32(HdRow.Value);
            string branch_code = "", branch_desc = "";

            try
            {
                branch_code = DwBranch.GetItemString(r, "branch_id");
                branch_desc = DwBranch.GetItemString(r, "branch_name");
                HdBranchCode.Value = branch_code;
                HdBranchDesc.Value = branch_desc;
                Label1.Text = "ยืนยัน";
            }
            catch
            {
                HdBranchCode.Value = "";
                HdBranchDesc.Value = "";
            }
        }
    }
}