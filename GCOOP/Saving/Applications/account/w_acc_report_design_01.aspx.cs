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
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNAccount;
using System.Globalization;
using System.Web.Services.Protocols;

namespace Saving.Applications.account
{
    public partial class w_acc_report_design_01 : PageWebSheet,WebSheet
    {
        private String moneysheet_code;
        private n_accountClient Accsrv;
        private accFunction accF;
        protected String saveData;
        protected String changeValue;
        protected String openProcess;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            saveData = WebUtil.JsPostBack(this, "saveData");
            changeValue = WebUtil.JsPostBack(this, "changeValue");
            openProcess = WebUtil.JsPostBack(this, "openProcess");
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                Accsrv = wcf.NAccount;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            try
            {
                accF = new accFunction();
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Function ไม่ได้");
                return;
            }

            this.ConnectSQLCA();
            dw_main.SetTransaction(sqlca);
            dw_rpt.SetTransaction(sqlca);
            GetRetrieve();

            if (IsPostBack)
            {
                dw_main.RestoreContext();
                dw_rpt.RestoreContext();
            }
            if (dw_main.RowCount < 1)
            {
                dw_main.Reset();
                dw_rpt.Reset();
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "changeValue")
            {
                GetRetrieve();
            }
            if (eventArg == "openProcess")
            {
                this.OpenProcess();
            }
        }

        private void GetRetrieve()
        {
            try
            {
                moneysheet_code = HdSheetTypeCode.Value;
                dw_main.Reset();
                dw_rpt.Reset();
                dw_main.Retrieve(moneysheet_code);
                HdSheetHeadName.Value = dw_main.GetItemString(1, "report_heading");
                

                //Response.Write("<script> alert('XML = " + dw_main.GetItemString(1, "month_1_1") + "') </script>");
            }
            catch {
                moneysheet_code = "";
            }
        }

        public void SaveWebSheet()
        {
            //this.SaveData();
        }

        public void WebSheetLoadEnd()
        {
            this.DisConnectSQLCA();
        }

        #endregion

        public void OpenProcess()
        {
            try
            {
                string str_tmp = "";
                str_tmp = dw_main.Describe("DataWindow.Data.XML");
                dw_rpt.Reset();
                HdSheetHeadName.Value = dw_main.GetItemString(1, "report_heading").ToString().Trim();
                HdSheetHeadCol1.Value = dw_main.GetItemString(1, "month_1_1").ToString().Trim();
                HdSheetHeadCol2.Value = dw_main.GetItemString(1, "month_2_1").ToString().Trim();
                //str_tmp = Accsrv.GenCashFlowSheet(state.SsWsPass, str_tmp, HdSheetTypeCode.Value, state.SsCoopId);
                Int32 result = Accsrv.of_gen_balance_sheet(state.SsWsPass, str_tmp, HdSheetTypeCode.Value, state.SsCoopId,ref str_tmp);
                //Response.Write("<script> alert('accF.GetFindToLDayOfM = " + accF.GetFindToLDayOfM(HdSheetHeadCol1.Value, dw_main.GetItemString(1, "year_1"), "th") + "') </script>");
                dw_rpt.ImportString(str_tmp, FileSaveAsType.Xml);
                str_tmp = "show_header.text = '";
                str_tmp += HdSheetHeadName.Value + "~n";
                str_tmp += "" + dw_main.GetItemString(1, "moneysheet_name").ToString().Trim() + "~n";
                str_tmp += "สำหรับสิ้นสุดวันที่ " + accF.GetFindToLDayOfM(HdSheetHeadCol1.Value, dw_main.GetItemString(1, "year_1"), "th") +accF.CnvStrToThaiM(HdSheetHeadCol1.Value, 0) + dw_main.GetItemString(1, "year_1");
                if (dw_main.GetItemString(1, "total_show").ToString().Equals("2"))
                {
                    str_tmp += " และ วันที่ " + accF.GetFindToLDayOfM(HdSheetHeadCol2.Value, dw_main.GetItemString(1, "year_2"), "th") + accF.CnvStrToThaiM(HdSheetHeadCol2.Value, 0) + dw_main.GetItemString(1, "year_2");
                }
                str_tmp += "'";
                
                dw_rpt.Modify(str_tmp); 
                str_tmp = "t_1.text = '";
                str_tmp += getHDdata(dw_main.GetItemString(1, "data_1"), dw_main.GetItemString(1, "compare"), HdSheetHeadCol1.Value, dw_main.GetItemString(1, "year_1"));
                str_tmp += "'";
                dw_rpt.Modify(str_tmp);

                str_tmp = "t_2.text = '";
                str_tmp += getHDdata(dw_main.GetItemString(1, "data_2"), dw_main.GetItemString(1, "compare"), HdSheetHeadCol2.Value, dw_main.GetItemString(1, "year_2"));
                str_tmp += "'";
                dw_rpt.Modify(str_tmp);
            }catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
            
        }
        private string getHDdata(string i_d, string comp, string month, string year)
        {
            string txt = "";
            if (comp == "1")
            {
                if (i_d == "1")
                {
                    txt = "(สะสม)";
                }
                else
                {
                    txt = "(เดือน)";
                }
                
            }
            txt += accF.CnvStrToThaiM(month) + year;
            return txt;
        }
        
    }
}
