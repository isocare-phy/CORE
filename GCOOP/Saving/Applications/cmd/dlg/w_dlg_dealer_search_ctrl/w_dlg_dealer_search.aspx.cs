using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.cmd.dlg.w_dlg_dealer_search_ctrl
{
    public partial class w_dlg_dealer_search : PageWebDialog, WebDialog
    {

        public void InitJsPostBack()
        {
            DsCriteria.InitDsCriteria(this);
            dsList.InitDsList(this);
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                //DsCriteria.DdMembGroup();
                //DsCriteria.DdMembType();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {

        }

        public void WebDialogLoadEnd()
        {

        }

        protected void BtSearch_Click(object sender, EventArgs e)
        {
            try
            {
                String ls_dealerno = "", ls_dealername = "";
                String ls_dealeraddr = "", ls_dealertaxid = "";
                String ls_dealerphone = "";

                string ls_sqlext = "";

                ls_dealerno = WebUtil.MemberNoFormat(DsCriteria.DATA[0].dealer_no.Trim());
                DsCriteria.DATA[0].dealer_no = ls_dealerno;
                ls_dealername = DsCriteria.DATA[0].dealer_name.Trim();
                ls_dealeraddr = DsCriteria.DATA[0].dealer_addr.Trim();
                ls_dealertaxid = DsCriteria.DATA[0].dealer_taxid.Trim();
                ls_dealerphone = DsCriteria.DATA[0].dealer_phone.Trim();

                if (ls_dealerno.Length > 0)
                {
                    ls_sqlext = " and ( PTUCFDEALER.DEALER_NO = '" + ls_dealerno + "') ";
                }
                if (ls_dealername.Length > 0)
                {
                    ls_sqlext = " and (  PTUCFDEALER.DEALER_NAME like '%" + ls_dealername + "%') ";
                }
                if (ls_dealeraddr.Length > 0)
                {
                    ls_sqlext = " and (  PTUCFDEALER.DEALER_ADDR like '%" + ls_dealeraddr + "%') ";
                }
                if (ls_dealertaxid.Length > 0)
                {
                    ls_sqlext += " and ( PTUCFDEALER.DEALER_TAXID like '%" + ls_dealertaxid + "%') ";
                }
                if (ls_dealerphone.Length > 0)
                {
                    ls_sqlext += " and ( PTUCFDEALER.DEALER_PHONE like '%" + ls_dealerphone + "%') ";
                }

                string sql = sql = @"
                SELECT DISTINCT
                    PTUCFDEALER.DEALER_NO, 
                    PTUCFDEALER.DEALER_NAME, 
                    PTUCFDEALER.DEALER_ADDR, 
                    PTUCFDEALER.DEALER_TAXID, 
                    PTUCFDEALER.DEALER_PHONE
                FROM         
                    PTUCFDEALER
                WHERE     
                    ROWNUM <= 300 " + ls_sqlext;
                DataTable dt = WebUtil.Query(sql);
                dsList.ImportData(dt);
                LbCount.Text = "ดึงข้อมูล" + (dt.Rows.Count >= 300 ? "แบบสุ่ม" : "ได้") + " " + dt.Rows.Count + " รายการ";

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}