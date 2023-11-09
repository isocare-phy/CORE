using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CommonLibrary;
using DBAccess;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using CommonLibrary.WsAssist;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_viewlog_payout_50bath : PageWebSheet, WebSheet
    {
        protected String jsPostMembNo;
        private Assist Asn;
        private string pbl = "kt_50bath.pbl";

        public void InitJsPostBack()
        {
            jsPostMembNo = WebUtil.JsPostBack(this, "jsPostMembNo");
        }

        public void WebSheetLoadBegin()
        {
            Asn = WsCalling.Assist;
            if (IsPostBack)
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwSlip);
            }
            else
            {
                DwMain.InsertRow(0);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostMembNo":
                    PostMembNo();
                    break;                    
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                String xmlSlip = DwSlip.Describe("DataWindow.Data.XML");
                string member_no = DwMain.GetItemString(1, "member_no");
                string tofrom_system = "";
                try
                {
                    tofrom_system = DwSlip.GetItemString(1, "tofrom_system");
                }
                catch { }
                bool resu = Asn.CancelPay(state.SsWsPass, xmlSlip, pbl, member_no, state.SsUsername, state.SsWorkDate, tofrom_system);
                if(resu)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการยกเลิกการจ่ายสำเร็จ");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ทำการยกเลิกาการจ่ายไม่สำเร็จ");
                }
                PostMembNo();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwSlip.SaveDataCache();
        }

        private void PostMembNo()
        {
            try
            {
                string member_no = DwMain.GetItemString(1, "member_no");
                member_no = int.Parse(member_no).ToString("00000000");
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, member_no);
                try { DwMain.GetItemString(1, "member_no"); }
                catch 
                {
                    DwSlip.Reset();
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบเลขสมาชิกนี้");
                    return;
                }

                try
                {
                    string memb_date = DwMain.GetItemDateTime(1, "birth_date").ToString("yyyy");
                    int memb_age = DateTime.Today.Year - (Convert.ToInt32(memb_date));
                    //--------------------------------------------------------------------------------------
                    int Dmemb_date = Convert.ToInt32(DwMain.GetItemDateTime(1, "birth_date").ToString("dd"));
                    int Mmemb_date = Convert.ToInt32(DwMain.GetItemDateTime(1, "birth_date").ToString("MM"));
                    int Ymemb_date = Convert.ToInt32(DwMain.GetItemDateTime(1, "birth_date").ToString("yyyy"));
                    if (DateTime.Today.Month < Mmemb_date)
                    {
                        memb_age = memb_age - 1;
                    }
                    else if (DateTime.Today.Month == Mmemb_date)
                    {
                        if (DateTime.Today.Day < Dmemb_date)
                        {
                            memb_age = memb_age - 1;
                        }
                    }
                    DwMain.SetItemString(1, "age", (memb_age + " ปี"));
                }
                catch { }
                string memb_no = "";
                try{memb_no = DwMain.GetItemString(1, "member_no");}
                catch{}
                if (memb_no != "")
                {
                    DwUtil.RetrieveDataWindow(DwSlip, pbl, null, member_no, "70");
                    try 
                    { 
                        DwSlip.GetItemString(1, "sliptype_code");
                    }
                    catch { DwSlip.Reset(); }
                }
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบเลขสมาชิกนี้");
                return;
            }
        }
    }
}