using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;

namespace Saving.Applications.app_assist.w_sheet_as_ucfseniormemberassist_ctrl
{
    public partial class w_sheet_as_ucfseniormemberassist : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostInsertRow { get; set; } //สำสั่ง JS postback
        [JsPostBack]
        public string PostDeleteRow { get; set; } //สำสั่ง JS postback

        public void InitJsPostBack()
        {
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsList.retrieve();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostInsertRow)
            {
                dsList.InsertAtRow(0);
                Decimal seq_no = 0, seq_no1 = 0;
                if (seq_no_old.Value != "")
                {
                    seq_no = Convert.ToDecimal(seq_no_old.Value);
                    seq_no1 = seq_no + 1;
                    seq_no_old.Value = seq_no1 + "";

                }
                else
                {
                    seq_no = MaxSeqno();
                    seq_no1 = seq_no + 1;
                    seq_no_old.Value = seq_no1 + "";

                }
                dsList.DATA[0].ENVCODE = "seniormemberassist_" + seq_no1;
                dsList.DATA[0].ENVGROUP = "senior_member_assist";
                dsList.DATA[0].SEQ_NO = seq_no1;
               
            }
            else if (eventArg == PostDeleteRow)
            {
                int Rowdelete = dsList.GetRowFocus();
                dsList.DeleteRow(Rowdelete);

            }
        }
        public Decimal MaxSeqno()
        {
            Decimal seq_no = 0;
            try
            {
                String strsql = @" SELECT max(seq_no) as seq_no FROM ASNSENVIRONMENTVAR  
                           WHERE ASNSENVIRONMENTVAR.ENVGROUP = 'senior_member_assist'";
                Sdt dtloanrcv = WebUtil.QuerySdt(strsql);

                if (dtloanrcv.GetRowCount() <= 0)
                {


                }
                else
                    if (dtloanrcv.Next())
                    {
                        seq_no = dtloanrcv.GetDecimal("seq_no");
                    }
            }
            catch (Exception ex) { }
            return seq_no;
        }

        public void SaveWebSheet()
        {
            try
            {
                ExecuteDataSource exed = new ExecuteDataSource(this);
                exed.AddRepeater(dsList);

                int result = exed.Execute();

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ"); //หากบันทึกให้ขึ้น บันทึกแล้ว
                dsList.retrieve();
                seq_no_old.Value = "";
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }

        }

        public void WebSheetLoadEnd()
        {
        }
    }
}