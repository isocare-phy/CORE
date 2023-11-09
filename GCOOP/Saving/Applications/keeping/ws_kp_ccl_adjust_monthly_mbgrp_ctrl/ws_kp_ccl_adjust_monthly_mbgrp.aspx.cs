using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using CoreSavingLibrary.WcfNKeeping;
using DataLibrary;

namespace Saving.Applications.keeping.ws_kp_ccl_adjust_monthly_mbgrp_ctrl
{
    public partial class ws_kp_ccl_adjust_monthly_mbgrp : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string JsPostProcessCcl { get; set; }

        public void InitJsPostBack()
        {
            dsProc.InitDsProc(this);
            dsMain.InitDsMain(this);
            dsDetail.InitDsDetail(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DateTime last = wcf.NCommon.of_lastdayofmonth(state.SsWsPass, state.SsWorkDate);
                dsProc.DATA[0].RECEIVE_YEAR = (last.Year + 543);
                dsProc.DATA[0].RECEIVE_MONTH = last.Month;
                dsMain.DATA[0].ENTRY_ID = state.SsUsername;
                dsMain.DATA[0].CANCEL_ID = state.SsUsername;
                dsMain.DATA[0].COOP_ID = state.SsCoopControl;
                dsMain.DATA[0].OPERATE_DATE = state.SsWorkDate;
                dsMain.DATA[0].CANCEL_DATE = state.SsWorkDate;
                dsProc.DdMembgroup();
                dsProc.DdSlipcause();
                dsMain.DdSlslipadjtype();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "JsPostProcessCcl")
            {
                SaveWebSheet();
            }
        }

        public void SaveWebSheet()
        {
            int result = 0;
            string kpslip_no = "", recv_period = "", membgroup_code = "";
            try
            {
                recv_period = dsProc.DATA[0].RECEIVE_YEAR.ToString() + dsProc.DATA[0].RECEIVE_MONTH.ToString();
                membgroup_code = dsProc.DATA[0].MEMBGROUP_CODE;
                string sqlck = @"select * from kpmastreceive where recv_period = {0} and membgroup_code = {1} ";
                sqlck = WebUtil.SQLFormat(sqlck, recv_period, membgroup_code);
                Sdt ta = WebUtil.QuerySdt(sqlck);
                while (ta.Next())
                {
                    kpslip_no = ta.GetString("kpslip_no").Trim();
                    ///ดึงข้อมูล set ลง datastore
                    str_keep_adjust astr_keep_adjust = new str_keep_adjust();
                    astr_keep_adjust.xml_main = dsMain.ExportXml();
                    astr_keep_adjust.xml_detail = dsDetail.ExportXml();
                    
                    wcf.NKeeping.of_init_adjust_monthly(state.SsWsPass,ref astr_keep_adjust);
                    dsMain.ResetRow();
                    dsDetail.ResetRow();
                    //DwUtil.ImportData(astr_keep_adjust.xml_main, dw_main, tdwmain, FileSaveAsType.Xml);
                    //DwUtil.ImportData(astr_keep_adjust.xml_detail, dw_detail, null, FileSaveAsType.Xml);

                    dsMain.RetrieveKpmast(recv_period, kpslip_no, state.SsCoopControl);
                    dsDetail.RetrieveKpdet(recv_period, kpslip_no, state.SsCoopControl);
                    dsMain.DATA[0].ADJSLIP_DATE = state.SsWorkDate;
                    dsMain.DATA[0].SLIPRETCAUSE_CODE = dsProc.DATA[0].SLIPRETCAUSE_CODE;
                    dsMain.DATA[0].SLIPRETALL_FLAG = 1;

                    //str_keep_adjust astr_keep_adjust = new str_keep_adjust();
                    //astr_keep_adjust.xml_main = dsMain.ExportXml();
                    //astr_keep_adjust.xml_detail = dsDetail.ExportXml();
                    astr_keep_adjust.cancel_id = state.SsUsername;
                    astr_keep_adjust.operate_date = state.SsWorkDate;

                    result = wcf.NKeeping.of_save_adjust_monthly(state.SsWsPass, astr_keep_adjust);
                }
                if (result == 1)
                {
                    //JsNewClear();
                    //LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {
            
        }

    }
}