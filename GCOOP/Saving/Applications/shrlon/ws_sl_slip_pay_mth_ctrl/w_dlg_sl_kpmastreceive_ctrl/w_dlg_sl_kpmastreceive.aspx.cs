using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataLibrary;

namespace Saving.Applications.shrlon.ws_sl_slip_pay_mth_ctrl.w_dlg_sl_kpmastreceive_ctrl
{
    public partial class w_dlg_sl_kpmastreceive : PageWebDialog, WebDialog
    {
        [JsPostBack]
        public string PostRefSystem { get; set; }
        [JsPostBack]
        public string PostMemberNo { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDs(this);
            dsList.InitDs(this);
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                try
                {
                    string member_no = Request["member_no"];
                    dsMain.DATA[0].MEMBER_NO = member_no;
                }
                catch { }
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "PostRefSystem")
            {
                try
                {
                    string member_no = "";
                    member_no = dsMain.DATA[0].MEMBER_NO;
                    dsList.RetrieveList(member_no);
                }
                catch { }
            }
            else if (eventArg == "PostMemberNo")
            {
                try
                {
                    string ref_sys = "", member_no = "";
                    member_no = dsMain.DATA[0].MEMBER_NO;
                    string sql = "select * from mbmembmaster where member_no={0} and coop_id = {1}";
                    member_no = WebUtil.MemberNoFormat(member_no);
                    sql = WebUtil.SQLFormat(sql, member_no, state.SsCoopId);
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        dsMain.DATA[0].MEMBER_NO = member_no;
                        LtServerMessageDlg.Text = "";
                            dsList.RetrieveList(member_no);
                    }
                    else
                    {
                        LtServerMessageDlg.Text = WebUtil.ErrorMessage("ไม่พบเลขสมาชิกดังกล่าว กรุณาตรวจสอบเลขสมาชิก");

                    }


                }
                catch { }

            }
        }

        public void WebDialogLoadEnd()
        {
        }
    }
}