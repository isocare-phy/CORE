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

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_cancel_dead : PageWebSheet, WebSheet
    {
        protected String postRetreiveDwMain;

        public void InitJsPostBack()
        {
            postRetreiveDwMain = WebUtil.JsPostBack(this, "postRetreiveDwMain");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetreiveDwMain")
            {
                RetreiveDwMain();
            }
        }

        public void SaveWebSheet()
        {
            decimal sumpays = 0, balance = 0;
            string assisttype_code = "";
            string SQLSelect = "select assisttype_code , sumpays , balance from asnreqmaster where member_no = '"+HdMemberNo.Value+"'";
            Sdt dtSelect = WebUtil.QuerySdt(SQLSelect);
            while (dtSelect.Next())
            {
                assisttype_code = dtSelect.GetString("assisttype_code");
                sumpays = dtSelect.GetDecimal("sumpays");
                balance = dtSelect.GetDecimal("balance");
                sumpays = sumpays - balance;
                string SQLUpdate = "update asnreqmaster set dead_status = '0' , dead_date = to_date('01011370','ddmmyyyy'),remark ='ยกเลิกการจ่ายกรณีเสียชีวิต' , pay_status = '1' , sumpays = " + sumpays + " , pay_date = to_date('01011370','ddmmyyyy') where assisttype_code = '"+assisttype_code+"' and member_no = '" + HdMemberNo.Value + "' and dead_status = '1' ";
                WebUtil.Query(SQLUpdate);
            }
            //string SQLDel = "delete from as";
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        public void RetreiveDwMain()
        {
            string dead_status ="" , check_dead ="0";
            DwUtil.RetrieveDataWindow(DwMain, "kt_pension.pbl", null, HdMemberNo.Value);
            DwUtil.RetrieveDataWindow(DwDetail, "kt_pension.pbl", null, HdMemberNo.Value);

            string SQLCheckDead = "select dead_status from asnreqmaster where member_no = '"+HdMemberNo.Value+"' and assisttype_code in ('80','90')";
            Sdt dtCheck = WebUtil.QuerySdt(SQLCheckDead);

            while (dtCheck.Next())
            {
                dead_status = dtCheck.GetString("dead_status");
                if (dead_status == "1")
                    check_dead = "1";
            }

            if (dead_status == "0" && check_dead != "1" )
            {
                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกหมายเลขนี้ ยังไม่มีคำขอกรณีเสียชีวิต");
            }
        }
    }
}