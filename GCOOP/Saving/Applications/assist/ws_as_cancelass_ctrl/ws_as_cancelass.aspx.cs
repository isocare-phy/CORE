using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.assist.ws_as_cancelass_ctrl
{
    public partial class ws_as_cancelass : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostMBNo { get; set; }
        [JsPostBack]
        public string PostAssContNo { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostMBNo)
            {
                string ls_mbno = dsMain.DATA[0].MEMBER_NO;

                ls_mbno = WebUtil.MemberNoFormat(ls_mbno);

                dsMain.DATA[0].MEMBER_NO = ls_mbno;

                this.of_setmbinfo(ls_mbno);
            }
            else if (eventArg == PostAssContNo)
            {
                string ls_acccontno;

                ls_acccontno = dsMain.DATA[0].ASSCONTRACT_NO;
                dsMain.InitAssContno(ls_acccontno);
            }
        }

        public void SaveWebSheet()
        {
            string ls_asscontno, ls_cause, ls_sql;

            ls_asscontno = dsMain.DATA[0].ASSCONTRACT_NO;
            ls_cause = dsMain.DATA[0].CANCEL_CAUSE;

            try
            {
                ls_sql = @" update asscontmaster 
                            set cancel_id = {2}, cancel_date = {3}, cancel_cause = {4}, withdrawable_amt = 0, asscont_status = -9
                            where coop_id = {0} and asscontract_no = {1} ";

                ls_sql = WebUtil.SQLFormat(ls_sql, state.SsCoopControl, ls_asscontno, state.SsUsername, state.SsWorkDate, ls_cause);
                WebUtil.ExeSQL(ls_sql);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ : " + ex);
                return;
            }

            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
            dsMain.ResetRow();
        }

        public void WebSheetLoadEnd()
        {
            throw new NotImplementedException();
        }

        private void of_setmbinfo(string as_memno)
        {
            string ls_sql;
            ls_sql = @" select 
		                        mpre.prename_desc ||mb.memb_name || '  ' || mb.memb_surname as mbname,
		                        trim( mb.membgroup_code )||' - '||mgrp.membgroup_desc as mbgroup,
		                        trim( mb.membtype_code )|| ':' || mt.membtype_desc  as mbtype
                        from	mbmembmaster mb
		                        join mbucfprename mpre on mb.prename_code = mpre.prename_code
		                        join mbucfmembgroup mgrp on mb.membgroup_code = mgrp.membgroup_code
		                        join mbucfmembtype mt on mb.membtype_code = mt.membtype_code
                        where mb.coop_id = {0} 
                        and mb.member_no = {1} ";

            ls_sql = WebUtil.SQLFormat(ls_sql, state.SsCoopControl, as_memno);
            Sdt dt = WebUtil.QuerySdt(ls_sql);
            if (!dt.Next())
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลสมาชิกเลขที่ "+as_memno+" กรุณาตรวจสอบ");
                return;
            }

            dsMain.DATA[0].mbname = dt.GetString("mbname");
            dsMain.DATA[0].mbgroup = dt.GetString("mbgroup");
            dsMain.DATA[0].mbtype = dt.GetString("mbtype");

            dsMain.DdAssContNo(as_memno);
        }

    }
}