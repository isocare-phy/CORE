using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using DataLibrary;
using System.Data;

namespace Saving.Applications.shrlon.ws_sl_reprint_payout_ctrl
{
    public partial class ws_sl_reprint_payout : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostRetrieve { get; set; }
        [JsPostBack]
        public string PostPrint { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.DdCode();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostRetrieve)
            {
                try
                {
                    string member_no = "";
                    string entry_id = "";
                    string sliptype_code = "";
                    string document_no_s = "";
                    string document_no_e = "";
                    DateTime slip_date_s = dsMain.DATA[0].SLIP_DATE_S;
                    DateTime slip_date_e = dsMain.DATA[0].SLIP_DATE_E;

                    member_no = dsMain.DATA[0].MEMBER_NO;
                    entry_id = dsMain.DATA[0].ENTRY_ID;
                    sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                    document_no_s = dsMain.DATA[0].document_no_e;
                    document_no_e = dsMain.DATA[0].document_no_e;

                    dsList.Retrieve(member_no, entry_id, sliptype_code, document_no_s, document_no_e, slip_date_s, slip_date_e);
                    int row = dsList.RowCount;
                    for (int i = 0; i < row; i++)
                    {
                        decimal slip_status = dsList.DATA[i].SLIP_STATUS;
                        if (slip_status < 0)
                        {
                            dsList.FindTextBox(i, "COMPUTE_1").BackColor = System.Drawing.Color.DarkGray;
                            dsList.FindTextBox(i, "document_no").BackColor = System.Drawing.Color.DarkGray;
                            dsList.FindTextBox(i, "slip_date").BackColor = System.Drawing.Color.DarkGray;
                            dsList.FindTextBox(i, "entry_date").BackColor = System.Drawing.Color.DarkGray;
                            dsList.FindTextBox(i, "member_no").BackColor = System.Drawing.Color.DarkGray;
                            dsList.FindTextBox(i, "COMPUTE_2").BackColor = System.Drawing.Color.DarkGray;
                            dsList.FindTextBox(i, "entry_id").BackColor = System.Drawing.Color.DarkGray;
                        }
                    }
                }
                catch { }
            }
            else if (eventArg == PostPrint)
            {
                string rslip = "";
                int[] prt_arr = new int[dsList.RowCount];

                string sliptype_code = "";

                for (int i = 0; i < dsList.RowCount; i++)
                {
                    if (dsList.DATA[i].checkselect == 1)
                    {
                        if (rslip == "")
                        {
                            rslip = "'" + dsList.DATA[i].PAYOUTSLIP_NO + "'";
                        }
                        else
                        {
                            rslip += ",'" + dsList.DATA[i].PAYOUTSLIP_NO + "'";
                        }
                        sliptype_code = dsList.DATA[i].SLIPTYPE_CODE;
                    }
                }

                string sqlprint = "select printslip_type, ireportpayout_obj from lnloanconstant ";
                Sdt dtp = WebUtil.QuerySdt(sqlprint);
                string printtype = "PB", ireportobj = "r_sl_slip_in_exat_a4_table";
                if (dtp.Next())
                {
                    printtype = dtp.GetString("printslip_type");
                    ireportobj = dtp.GetString("ireportpayout_obj");

                }
                else
                {
                    printtype = "PB";
                    ireportobj = "r_sl_slip_in_exat_a4_table";
                }
                try
                {
                    //Printing.PrintSlipSlIreportGsb(this, payoutslip_no, payinslip_no, state.SsCoopId);
                    Printing.PrintSlipSlInOutIreportExat(this, rslip, state.SsCoopControl, ireportobj);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                }
                
                
            }
        }

        private static string XmlReadVar(string responseData, string szVar)
        {
            int i1stLoc = responseData.IndexOf("<" + szVar + ">");
            if (i1stLoc < 0)
                return string.Empty;
            int ilstLoc = responseData.IndexOf("</" + szVar + ">");
            if (ilstLoc < 0)
                return string.Empty;
            int len = szVar.Length;
            return responseData.Substring(i1stLoc + len + 2, ilstLoc - i1stLoc - len - 2);
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {

        }
        public void PrintSlippayout(string payoutslip_no)
        {

            string sql = @"  SELECT E.PRINTRECEIPTADDR_TYPE,   
         d.prename_desc||c.memb_name||'  '||c.memb_surname as member_name,   
         E.MEMBGROUP_DESC,   
         C.MEMBTYPE_CODE,   
         ft_readtbaht( SLSLIPPAYOUT.PAYOUT_AMT) AS money_thaibaht,   
         SLSLIPPAYOUT.MEMBER_NO,   
         SLSLIPPAYOUT.DOCUMENT_NO,   
         SLSLIPPAYOUT.SLIP_DATE,   
         SLSLIPPAYOUT.SLIPTYPE_CODE,   
         SLSLIPPAYOUT.OPERATE_DATE,   
         SLSLIPPAYOUT.LOANCONTRACT_NO,   
         SLSLIPPAYOUT.RCV_PERIOD,   
         SLSLIPPAYOUT.PAYOUT_AMT,   
         SLSLIPPAYOUT.PAYOUTCLR_AMT,   
         SLSLIPPAYOUT.PAYOUTNET_AMT,   
         SLSLIPPAYOUT.MONEYTYPE_CODE,   
         SLSLIPPAYOUT.EXPENSE_BANK,   
         SLSLIPPAYOUT.EXPENSE_BRANCH,   
         SLSLIPPAYOUT.EXPENSE_ACCID,   
         SLSLIPPAYOUT.SLIP_STATUS,   
         SLSLIPPAYOUT.POSTSLIP_NO,   
         SLSLIPPAYOUT.SHRLONTYPE_CODE,   
         SLSLIPPAYOUT.BFSHRCONT_BALAMT,   
         SLSLIPPAYOUT.BFSHAREBEGIN_AMT,   
         SLSLIPPAYOUT.MEMCOOP_ID,   
         SHSHAREMASTER.SHARESTK_AMT,   
         C.ACCUM_INTEREST,   
         SHSHAREMASTER.SHARESTK_AMT * 10 as sharestk_value  ,
         SLSLIPPAYOUT.ENTRY_ID,   
         SLSLIPPAYOUT.PAYOUTSLIP_NO 
    FROM MBMEMBMASTER C,   
         MBUCFPRENAME D,   
         MBUCFMEMBGROUP E,   
         SLSLIPPAYOUT,   
         SHSHAREMASTER  
   WHERE SLSLIPPAYOUT.payoutslip_no = (" + payoutslip_no + @") 
 and ( c.prename_code = d.prename_code (+)) and  
         ( C.MEMBER_NO = SLSLIPPAYOUT.MEMBER_NO ) and  
         ( C.COOP_ID = SLSLIPPAYOUT.COOP_ID ) and  
         ( C.MEMBGROUP_CODE = E.MEMBGROUP_CODE ) and  
         ( C.COOP_ID = E.COOP_ID ) and  
         ( C.COOP_ID = SHSHAREMASTER.COOP_ID ) and  
         ( C.MEMBER_NO = SHSHAREMASTER.MEMBER_NO )    ";

            //sql = WebUtil.SQLFormat(sql, payoutslip_no);
            DataTable data = WebUtil.Query(sql);
            DataTable data2 = WebUtil.Query(sql);
            string shrlontype_code = "";
            foreach (DataRow row in data2.Rows)
            {
                shrlontype_code = row["shrlontype_code"].ToString();

            }


            if (shrlontype_code.Trim() == "10")
            {
                Printing.PrintAppletPB(this, "sl_slip_payout_emer", data);
            }
            else
            {
                Printing.PrintAppletPB(this, "sl_slip_payout", data);
            }
        }
        public void PrintSlippayoutPEA(string payoutslip_no)
        {

            string sql = @"select
mp.prename_desc,
mb.memb_name,
mb.memb_surname,
so.loancontract_no,
so.shrlontype_code,
so.payoutslip_no,
so.member_no,
so.payout_amt,
so.returnetc_amt ,
so.payoutnet_amt,
so.moneytype_code,
so.expense_bank,
cb.bank_desc,
so.expense_accid,
trim(TO_CHAR( so.slip_date, 'DD/MM/YYYY', 'NLS_CALENDAR=''THAI BUDDHA'' NLS_DATE_LANGUAGE=THAI')) as slip_date ,
nvl(si.payinslip_no,'n/a') as payinslip_no,
nvl(si.document_no,'n/a') as document_no,
nvl(sum(sid.principal_payamt),0) as sum_princ,
nvl(sum(sid.interest_payamt),0) as sum_int,
sum(CASE sid.slipitemtype_code WHEN 'LON' THEN sid.item_payamt ELSE 0 end   ) as item_lon,
sum(CASE sid.slipitemtype_code WHEN 'MUT' THEN sid.item_payamt ELSE 0 end   ) as item_mut,
 ft_readtbaht( so.payout_amt ) AS  money_thaibaht ,
sh.sharestk_amt * 10 as sharestk_value,
mb.accum_interest as accum_interest,
mp.prename_desc||mb.memb_name || ' ' ||mb.memb_surname as member_name 

from slslippayout so,slslippayin si ,slslippayindet sid,mbmembmaster mb ,mbucfprename mp,cmucfbank cb, shsharemaster sh
where so.payoutslip_no = (" + payoutslip_no + @")
and so.slipclear_no = si.payinslip_no(+)
and si.payinslip_no = sid.payinslip_no(+)
and so.coop_id = si.coop_id(+)
and so.member_no = si.member_no(+)
and sh.coop_id = mb.coop_id(+)
and sh.member_no = mb.member_no(+)
and si.coop_id = sid.coop_id (+)
and so.member_no = mb.member_no
and mb.prename_code = mp.prename_code
and so.expense_bank = cb.bank_code(+)
group by mp.prename_desc,
mb.memb_name,
mb.memb_surname,mp.prename_desc||mb.memb_name || ' ' ||mb.memb_surname as member_name ,
so.loancontract_no,
so.shrlontype_code,
so.payoutslip_no,
so.member_no,
so.payout_amt,
so.returnetc_amt ,
so.payoutnet_amt,
so.moneytype_code,
so.expense_bank,
cb.bank_desc,
so.expense_accid,
so.slip_date,
si.payinslip_no,
si.document_no,
 ft_readtbaht( so.payout_amt ) ,
sh.sharestk_amt * 10 ,
mb.accum_interest";

            //sql = WebUtil.SQLFormat(sql, payoutslip_no);
            DataTable data = WebUtil.Query(sql);
            DataTable data2 = WebUtil.Query(sql);
            string shrlontype_code = "";
            foreach (DataRow row in data2.Rows)
            {
                shrlontype_code = row["shrlontype_code"].ToString();

            }


            if (shrlontype_code.Trim() == "10")
            {
                Printing.PrintAppletPB(this, "sl_slip_payout_emer", data);
            }
            else
            {
                Printing.PrintAppletPB(this, "sl_slip_payout", data);
            }
        }
    }
}
