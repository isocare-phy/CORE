using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.app_finance.dlg
{
    public partial class w_dlg_finconfrim : PageWebDialog, WebDialog
    {
        DwTrans SQLCA;
        String pbl = "finslip_spc.pbl";
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                String MemNoTxt = Dw_cri.GetItemString(1, "member_no");
                String MemNameTxt = Dw_cri.GetItemString(1, "member_name");

                DwUtil.RetrieveDataWindow(dw_data, pbl, null, state.SsWorkDate, state.SsCoopId);
                if ((MemNoTxt != "") && (MemNameTxt == ""))
                {
                    dw_data.SetFilter("member_no like '%" + MemNoTxt + "%'");
                    dw_data.Filter();
                }
                else if ((MemNameTxt != "") && (MemNoTxt == ""))
                {
                    dw_data.SetFilter("member_name like '%" + MemNameTxt + "%'");
                    dw_data.Filter();
                }
                else
                {
                    //DwUtil.RetrieveDataWindow(dw_data, pbl, null, state.SsWorkDate, state.SsCoopId);   
                }
            }
            catch
            {
                DwUtil.RetrieveDataWindow(dw_data, pbl, null, state.SsWorkDate, state.SsCoopId);
                Dw_cri.SetItemString(1, "member_no", "");
                Dw_cri.SetItemString(1, "member_name", "");
            }

        }

        public void InitJsPostBack()
        {
        }

        public void WebDialogLoadBegin()
        {
            state = new WebState(Session, Request);
            SQLCA = new DwTrans();
            SQLCA.Connect();
            dw_data.SetTransaction(SQLCA);
            //this.ConnectSQLCA();
            if (!IsPostBack)
            {
                Dw_cri.InsertRow(0);
                Dw_cri.SetItemString(1, "member_no", "");
                Dw_cri.SetItemString(1, "member_name", "");
                DwUtil.RetrieveDataWindow(dw_data, pbl, null, state.SsWorkDate, state.SsCoopId);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
           
        }
        public void WebDialogLoadEnd()
        {
            // SQLCA.Disconnect();
        }
    }
}
