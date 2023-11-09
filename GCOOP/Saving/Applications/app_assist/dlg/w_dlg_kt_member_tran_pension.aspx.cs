using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using CommonLibrary.WsShrlon;
using CommonLibrary.WsCommon;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_kt_member_tran_pension : PageWebDialog, WebDialog
    {
        private Shrlon shrlonService;
        private Common commonService;
        protected String refresh;
        protected String search;

        public void InitJsPostBack()
        {
            refresh = WebUtil.JsPostBack(this, "refresh");
            search = WebUtil.JsPostBack(this, "search");

        }

        public void WebDialogLoadBegin()
        {
            try
            {
                shrlonService = new Shrlon();
                commonService = new Common();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
            //this.ConnectSQLCA();
            if (!IsPostBack)
            {
                dw_data.InsertRow(0);
            }
            else
            {
                dw_data.RestoreContext();
            }

            if (!HfSearch.Value.Equals(""))
            {
                DwUtil.ImportData(HfSearch.Value, dw_detail, null);
            }
            if (dw_data.RowCount > 1)
            {
                dw_data.DeleteRow(dw_data.RowCount);
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "refresh")
            {
                Refresh();
            }
            if (eventArg == "search")
            {
                Search();
            }
        }

        public void WebDialogLoadEnd()
        {
            DwUtil.RetrieveDDDW(dw_data, "membgroup_no_1", "kt_50bath.pbl", null);
            if (dw_data.RowCount > 1)
            {
                dw_data.DeleteRow(dw_data.RowCount);
            }
            dw_data.SaveDataCache();
            dw_detail.SaveDataCache();

        }
        public void Refresh() { }
        protected void Search()
        {

            String ls_memno = "", ls_memname = "", ls_memsurname = "";
            String ls_memgroup = "", ls_contno = "", ls_salaryid = "";
            String ls_cardperson = "", ls_subgroup = "";
            String strSQL = "", strSQLT = "", strTemp = "";
            int rowNumber = 1;

            //            strSQL = @" SELECT MBMEMBMASTER.MEMBER_NO,   
            //         MBUCFPRENAME.PRENAME_DESC,   
            //         MBMEMBMASTER.MEMB_NAME,   
            //         MBMEMBMASTER.MEMB_SURNAME,   
            //         MBMEMBMASTER.MEMBGROUP_CODE,   
            //         MBUCFMEMBGROUP.MEMBGROUP_DESC,   
            //         MBMEMBMASTER.SUBGROUP_CODE,   
            //         MBUCFMBSUBGROUP.SUBGROUP_DESC,   
            //         MBMEMBMASTER.RESIGN_STATUS  
            //    FROM MBMEMBMASTER,   
            //         MBUCFMEMBGROUP,   
            //         MBUCFPRENAME,   
            //         MBUCFMBSUBGROUP  
            //   WHERE ( MBMEMBMASTER.MEMBGROUP_CODE = MBUCFMEMBGROUP.MEMBGROUP_CODE ) and  
            //         ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and  
            //         ( MBMEMBMASTER.SUBGROUP_CODE = MBUCFMBSUBGROUP.SUBGROUP_CODE )";
            strSQL = @" SELECT DISTINCT MBMEMBMASTER.MEMBER_NO,   
         MBUCFPRENAME.PRENAME_DESC,   
         MBMEMBMASTER.MEMB_NAME,   
         MBMEMBMASTER.MEMB_SURNAME,   
         MBMEMBMASTER.MEMBGROUP_CODE,   
         MBUCFMEMBGROUP.MEMBGROUP_DESC,   
         MBMEMBMASTER.SUBGROUP_CODE,   
         MBUCFMBSUBGROUP.SUBGROUP_DESC,   
         MBMEMBMASTER.RESIGN_STATUS
    FROM MBMEMBMASTER,   
         MBUCFMEMBGROUP,   
         MBUCFPRENAME,   
         MBUCFMBSUBGROUP ,
		ASNREQMASTER 
   WHERE (ASNREQMASTER.MEMBER_NO =  MBMEMBMASTER.MEMBER_NO) and
	( MBMEMBMASTER.MEMBGROUP_CODE = MBUCFMEMBGROUP.MEMBGROUP_CODE ) and  
         ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and  
         ( MBMEMBMASTER.SUBGROUP_CODE = MBUCFMBSUBGROUP.SUBGROUP_CODE )
 and
( MBMEMBMASTER.BIRTH_DATE is not null) and
(trunc(months_between(sysdate,MBMEMBMASTER.BIRTH_DATE))/12)>=60";
            //and (ASNREQMASTER.ASSIST_DOCNO LIKE 'PS%')

            //ls_sql = dw_detail.GetSqlSelect();

            try
            {
                ls_memno = dw_data.GetItemString(1, "member_no");
                ls_memno = string.IsNullOrEmpty(ls_memno) ? "" : ls_memno.Trim();
                if (ls_memno == "000000")
                {
                    ls_memno = "";
                }
            }
            catch
            { ls_memno = ""; }
            try
            {
                ls_cardperson = dw_data.GetItemString(rowNumber, "card_person");
                ls_cardperson = string.IsNullOrEmpty(ls_cardperson) ? "" : ls_cardperson.Trim();

            }
            catch { ls_cardperson = ""; }
            try
            {
                //ls_salaryid = dw_data.GetItemString(1, "salary_id").Trim();
                ls_salaryid = dw_data.GetItemString(rowNumber, "salary_id");
                ls_salaryid = string.IsNullOrEmpty(ls_salaryid) ? "" : ls_salaryid.Trim();

            }
            catch { ls_salaryid = ""; }
            try
            {
                //ls_memname = dw_data.GetItemString(1, "memb_name").Trim();
                ls_memname = dw_data.GetItemString(rowNumber, "memb_name");
                ls_memname = string.IsNullOrEmpty(ls_memname) ? "" : ls_memname.Trim();

            }
            catch { ls_memname = ""; }
            try
            {
                //ls_memsurname = dw_data.GetItemString(1, "memb_surname").Trim();
                ls_memsurname = dw_data.GetItemString(rowNumber, "memb_surname");
                ls_memsurname = string.IsNullOrEmpty(ls_memsurname) ? "" : ls_memsurname.Trim();

            }
            catch { ls_memsurname = ""; }
            try
            {
                //ls_memgroup = dw_data.GetItemString(1, "membgroup_no").Trim();
                ls_memgroup = dw_data.GetItemString(rowNumber, "membgroup_no");
                ls_memgroup = string.IsNullOrEmpty(ls_memgroup) ? "" : ls_memgroup.Trim();

            }
            catch { ls_memgroup = ""; }
            try
            {
                ls_subgroup = dw_data.GetItemString(1, "subgroup_code").Trim();
                ls_subgroup = dw_data.GetItemString(rowNumber, "subgroup_code");
                ls_subgroup = string.IsNullOrEmpty(ls_subgroup) ? "" : ls_subgroup.Trim();

            }
            catch { ls_subgroup = ""; }
            try
            {
                ls_contno = dw_data.GetItemString(1, "loancontract_no").Trim();
                ls_contno = dw_data.GetItemString(rowNumber, "loancontract_no");
                ls_contno = string.IsNullOrEmpty(ls_contno) ? "" : ls_contno.Trim();

            }
            catch { ls_contno = ""; }

            if (ls_memno.Length > 0)
            {
                strSQLT += " and (  mbmembmaster.member_no = '" + ls_memno + "') ";
            }
            if (ls_cardperson.Length > 0)
            {
                strSQLT += " and ( mbmembmaster.card_person like '" + ls_cardperson + "%') ";
            }
            if (ls_salaryid.Length > 0)
            {
                strSQLT += " and ( mbmembmaster.salary_id like '" + ls_salaryid + "%') ";
            }
            if (ls_memname.Length > 0)
            {
                strSQLT += " and ( mbmembmaster.memb_name like '" + ls_memname + "%') ";
            }
            if (ls_memsurname.Length > 0)
            {
                strSQLT += " and ( mbmembmaster.memb_surname like '" + ls_memsurname + "%') ";
            }
            if (ls_memgroup.Length > 0)
            {
                strSQLT += " and ( mbmembmaster.membgroup_code = '" + ls_memgroup + "') ";
            }
            if (ls_subgroup.Length > 0)
            {
                strSQLT += " and ( mbmembmaster.subgroup_code = '" + ls_subgroup + "') ";
            }
            if (ls_contno.Length > 0)
            {
                strSQLT += " and ( lncontmaster.loancontract_no like '" + ls_contno + "%') ";
            }
            try
            {
                strTemp = strSQL + strSQLT;
                HfSearch.Value = strTemp;
                DwUtil.ImportData(strTemp, dw_detail, null);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }



    }
}
