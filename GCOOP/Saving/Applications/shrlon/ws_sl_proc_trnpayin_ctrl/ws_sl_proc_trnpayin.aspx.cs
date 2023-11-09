using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using CoreSavingLibrary.WcfNShrlon;
using DataLibrary;
using System.Data;

namespace Saving.Applications.shrlon.ws_sl_proc_trnpayin_ctrl
{
    public partial class ws_sl_proc_trnpayin : PageWebSheet, WebSheet
    {

        [JsPostBack]
        public String PostCheck { get; set; }
        [JsPostBack]
        public String PostProc { get; set; }
        [JsPostBack]
        public String PostPrint { get; set; }
        public string outputProcess;
        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }


        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.DATA[0].trans_date = state.SsWorkDate;
                dsMain.DATA[0].source_system = "DIV";
                dsMain.DATA[0].entry_id = state.SsUsername;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "PostCheck")
            {
                dsList.Retrieve(dsMain.DATA[0].source_system, dsMain.DATA[0].trans_date);
            }
            else if (eventArg == "PostProc")
            {
                try
                {
                    //str_proctrnpayin astr_proctrnpayin = new str_proctrnpayin();
                    //astr_proctrnpayin.source_code = dsMain.DATA[0].source_system;
                    //astr_proctrnpayin.trans_date = dsMain.DATA[0].trans_date;
                    //astr_proctrnpayin.entry_id = state.SsUsername;
                    string xml = dsMain.ExportXml();
                    //wcf.NShrlon.of_proc_trnpayin(state.SsWsPass, ref astr_proctrnpayin);
                    outputProcess = WebUtil.runProcessing(state, "LNPOSTTRNPAYIN", xml, "", "");
                    //Hdstartslipno.Value = astr_proctrnpayin.payinslip_startno;
                    //Hdendslipno.Value = astr_proctrnpayin.payinslip_endno;
                    LtServerMessage.Text = WebUtil.CompleteMessage("ผ่านรายการสำเร็จ");

                    //select * from slslippayin where slip_date = to_date('30062017','ddmmyyyy') and ref_system = 'SGT';

                    //update slslippayin
                    //set sharestk_value = sharestk_value+ slip_amt 
                    //,slip_status = 1
                    //where slip_date = to_date('30062017','ddmmyyyy') and ref_system = 'SGT'; commit;

                    //select * from slslippayindet where payinslip_no in 
                    //(select payinslip_no from slslippayin where slip_date = to_date('30062017','ddmmyyyy') and ref_system = 'SGT');

                    //update slslippayindet
                    //set shrlontype_code = '01'
                    //,slipitem_desc = 'ซื้อหุ้นเพิ่ม'
                    //,stm_itemtype = 'SPX'
                    // where payinslip_no in 
                    //(select payinslip_no from slslippayin where slip_date = to_date('30062017','ddmmyyyy') and ref_system = 'SGT'); commit;

                    //select * from sltranspayin ;


                    /////DVI
                    //select * from slslippayin where slip_date = to_date('24122018','ddmmyyyy') and ref_system = 'DIV';
                    //update slslippayin set slip_status = 1 where slip_date = to_date('24122018','ddmmyyyy') and ref_system = 'DIV';commit;
                    //update slslippayindet set slipitem_desc = 'ชำระหนี้พิเศษ',stm_itemtype = 'LPX'
                    //where payinslip_no in (select payinslip_no from slslippayin where slip_date = to_date('24122018','ddmmyyyy') and ref_system = 'DIV'); commit;
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else if(eventArg == "PostPrint"){
                Printing.SlipNoPrintSlipSlpayin(this, Hdstartslipno.Value, Hdendslipno.Value, state.SsCoopControl); 
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {

        }
    }
}