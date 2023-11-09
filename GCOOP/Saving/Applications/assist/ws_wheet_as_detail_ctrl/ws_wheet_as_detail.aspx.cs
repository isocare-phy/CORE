using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.assist.ws_wheet_as_detail_ctrl
{
    public partial class ws_wheet_as_detail : PageWebSheet, WebSheet
    {
         [JsPostBack]
         public string PostRetrivememno { get; set; }  
         [JsPostBack]
         public string PostRetrivefromDDW { get; set; }  
         [JsPostBack]
         public string PostRetriveDetail { get; set; }  
     
        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsContmaster.InitDsContmaster(this);
            dsStatement.InitDsStatement(this);
            dsPayout.InitDsPayout(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)//show data first  
            {
                dsContmaster.Visible = false; 
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostRetrivememno)
            {
                string ls_memno = WebUtil.MemberNoFormat(dsMain.DATA[0].member_no.Trim());
                dsMain.RetrieveMain(ls_memno);
                dsMain.RetriveGroup();
                dsContmaster.ResetRow();
                dsStatement.ResetRow();
                dsPayout.ResetRow();
                dsContmaster.Visible = false;
            }
            else if (eventArg == PostRetriveDetail)
            {
                string ls_memno = WebUtil.MemberNoFormat(dsMain.DATA[0].member_no.Trim());
                string ls_assistcode = dsMain.DATA[0].assisttype_code;
                dsPayout.RetrieveData(ls_memno, ls_assistcode);
                string sql = @"select STM_FLAG from ASSUCFASSISTTYPE where assisttype_code='"+ls_assistcode+"' ";
                Sdt dt = WebUtil.QuerySdt(sql);
                dt.Next();
                int ln_stmflag = 0;
                ln_stmflag = dt.GetInt32("STM_FLAG");
                if (ln_stmflag == 1)
                {
                    dsContmaster.Visible = true; 
                    dsContmaster.RetrieveDetail(ls_memno, ls_assistcode);
                    dsStatement.RetrieveData(dsContmaster.DATA[0].asscontract_no.Trim());
                    dsContmaster.DDW_Contractno(ls_memno, ls_assistcode);
                 }
                else {
                    dsContmaster.ResetRow();
                    dsStatement.ResetRow();
                    dsContmaster.Visible = false;
                }                              
            }
            else if (eventArg == PostRetrivefromDDW) {
                string ls_memno = WebUtil.MemberNoFormat(dsMain.DATA[0].member_no.Trim());
                string ls_assistcode = dsMain.DATA[0].assisttype_code;
                try
                {
                    dsContmaster.Visible = true; 
                    string ls_contractno= dsContmaster.DATA[0].asscontract_no;
                    dsContmaster.RetrieveDetailFromDDW(ls_memno, ls_assistcode, ls_contractno);
                    dsStatement.RetrieveData(ls_contractno);
                    dsContmaster.DDW_Contractno(ls_memno, ls_assistcode);
                }
                catch { }
            }
        }      
        public void SaveWebSheet()
        {
            try
            {
                string ls_contractno = dsContmaster.DATA[0].asscontract_no;
                string sql_update = @"update ASSCONTMASTER set payment_status={4} where coop_id = {0} and assisttype_code={1} and asscontract_no={2} and member_no={3}";
                sql_update = WebUtil.SQLFormat(sql_update, state.SsCoopId, dsMain.DATA[0].assisttype_code, dsContmaster.DATA[0].asscontract_no, dsMain.DATA[0].member_no.Trim(), dsContmaster.DATA[0].payment_status);
                WebUtil.ExeSQL(sql_update);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
            }
            catch { LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกข้อมูลไม่สำเร็จ"); }
        }

        public void WebSheetLoadEnd()
        {
            throw new NotImplementedException();
        }
    }
}