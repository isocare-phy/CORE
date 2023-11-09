using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;

namespace Saving.Applications.keeping.w_sheet_kp_adjust_kptp_ctrl
{
    public partial class w_sheet_kp_adjust_kptp : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public String postMemberNo { get; set; }
        [JsPostBack]
        public String postRecv { get; set; }
        [JsPostBack]
        public String PostDelRow { get; set; }
        [JsPostBack]
        public String postRowVal { get; set; }
        [JsPostBack]
        public String PostMember { get; set; }


        public void InitJsPostBack()
        {
            dsMain.InitDs(this);
            dsListSlip.InitDs(this);
            dsRecieveMain.InitDs(this);
            dsRecieveList.InitDs(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            String member_no = "";
            member_no = WebUtil.MemberNoFormat(dsMain.DATA[0].MEMBER_NO);
            dsMain.DATA[0].MEMBER_NO = member_no;

            if (eventArg == postMemberNo)
            {
                dsMain.RetrieveMember(member_no);
                dsMain.DATA[0].SALARY_ID = dsMain.DATA[0].SALARY_ID.Trim(); //

                //check mem type
                if (dsMain.DATA[0].MEMBER_STATUS == 1)
                { dsMain.DATA[0].member_statust = "ปกติ"; }
                else { dsMain.DATA[0].member_statust = "ลาออก"; /*เปลี่ยน text ภายหลัง*/}

                dsListSlip.ResetRow();
                dsRecieveMain.ResetRow();
                dsRecieveList.ResetRow();
                dsListSlip.Retrieve(member_no);

                for (int i = 0; i < dsListSlip.RowCount; i++)
                {
                    dsListSlip.DATA[i].NUMBER = (i + 1);
                }

            }
            else if (eventArg == postRecv)
            {
                int row = dsListSlip.GetRowFocus();
                String recv_period = dsListSlip.DATA[row].RECV_PERIOD;
                String kpslip_no = dsListSlip.DATA[row].KPSLIP_NO;
                dsRecieveMain.Retrieve(member_no, recv_period, kpslip_no); //LEK เพิ่ม Argument kpslip_no
                dsRecieveList.Retrieve(member_no, recv_period, dsRecieveMain.DATA[0].KPSLIP_NO);
                for (int i = 0; i < dsRecieveList.RowCount; i++)
                {
                    dsRecieveList.DATA[i].bfadjust_prnamt = dsRecieveList.DATA[i].PRINCIPAL_PAYMENT;
                    dsRecieveList.DATA[i].bfadjust_intamt = dsRecieveList.DATA[i].INTEREST_PAYMENT;
                    dsRecieveList.DATA[i].bfadjust_itemamt = dsRecieveList.DATA[i].ITEM_PAYMENT;
                }

            }
            else if (eventArg == PostDelRow)
            {
                int row = dsRecieveList.GetRowFocus();
                dsRecieveList.DeleteRow(row);
            }
            else if (eventArg == postRowVal)
            {

            }
            else if (eventArg == PostMember)
            {
                string memberno = WebUtil.MemberNoFormat(dsMain.DATA[0].MEMBER_NO);
                dsMain.RetrieveMember(memberno);
                dsMain.DATA[0].SALARY_ID = dsMain.DATA[0].SALARY_ID.Trim(); 

                //check mem type
                if (dsMain.DATA[0].MEMBER_STATUS == 1)
                { dsMain.DATA[0].member_statust = "ปกติ"; }
                else { dsMain.DATA[0].member_statust = "ลาออก"; /*เปลี่ยน text ภายหลัง*/}

                dsListSlip.ResetRow();
                dsRecieveMain.ResetRow();
                dsRecieveList.ResetRow();
                dsListSlip.Retrieve(member_no);

                for (int i = 0; i < dsListSlip.RowCount; i++)
                {
                    dsListSlip.DATA[i].NUMBER = (i + 1);
                }
                    
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                decimal sumall = 0;
                String coop_id = state.SsCoopControl;
                String member_no = dsMain.DATA[0].MEMBER_NO;
                String kpslip_no = dsRecieveMain.DATA[0].KPSLIP_NO;
                int row = dsListSlip.GetRowFocus();
                String recv_period = dsListSlip.DATA[row].RECV_PERIOD;
                int result = 0, li_seqno = 0;

                ExecuteDataSource exe = new ExecuteDataSource(this);
                for (int i = 0; i < dsRecieveList.RowCount; i++)
                {
                    sumall += (dsRecieveList.DATA[i].ITEM_PAYMENT * dsRecieveList.DATA[i].sign_flag);
                    li_seqno = Convert.ToInt32(dsRecieveList.DATA[i].SEQ_NO);
                    if (dsRecieveList.DATA[i].posting_flag == "Y")
                    {
                        dsRecieveList.DATA[i].bfadjust_prnamt = 0;
                        dsRecieveList.DATA[i].bfadjust_intamt = 0;
                        dsRecieveList.DATA[i].bfadjust_itemamt = 0;
                    }

                    String sql = @" update  kptempreceivedet
                                set     principal_payment = " + dsRecieveList.DATA[i].PRINCIPAL_PAYMENT + @",
                                        interest_payment = " + dsRecieveList.DATA[i].INTEREST_PAYMENT + @", 
                                        item_payment = " + dsRecieveList.DATA[i].ITEM_PAYMENT + @",
                                        keepitem_status = " + dsRecieveList.DATA[i].KEEPITEM_STATUS + @",
                                        posting_flag = '" + dsRecieveList.DATA[i].posting_flag + @"',
                                        bfadjust_prnamt = " + dsRecieveList.DATA[i].bfadjust_prnamt + @",
                                        bfadjust_intamt = " + dsRecieveList.DATA[i].bfadjust_intamt + @",
                                        bfadjust_itemamt = " + dsRecieveList.DATA[i].bfadjust_itemamt + @"
                                where   coop_id ='" + coop_id + @"'  and 
                                        seq_no = " + li_seqno + @" and
                                        member_no='" + member_no + @"' and recv_period = '" + recv_period + @"' and 
                                        kpslip_no = '" + kpslip_no + "'";
                    sql = WebUtil.SQLFormat(sql);
                    exe.SQL.Add(sql);
                    
                }
                String sql2 = @"update kptempreceive set receive_amt = " + sumall + ", money_text = FT_READTBAHT("+ sumall +")  where KPSLIP_NO =  '" + kpslip_no + "'";
                sql2 = WebUtil.SQLFormat(sql2);
                exe.SQL.Add(sql2);
                result = exe.Execute();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ (" + kpslip_no + ")");

                //UpdateKPcancel();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            //UpdateKPcancel();
        }

        private void UpdateKPcancel()
        {

            String coop_id = state.SsCoopControl;
            String member_no = dsMain.DATA[0].MEMBER_NO;
            String kpslip_no = dsRecieveMain.DATA[0].KPSLIP_NO;
            ExecuteDataSource exe = new ExecuteDataSource(this);

            String sql = @"update kptempreceivedet set posting_flag = 'N',  where KPSLIP_NO =  '" + kpslip_no + "'";
            sql = WebUtil.SQLFormat(sql);
            exe.SQL.Add(sql);
            exe.Execute();
        }
    }
}