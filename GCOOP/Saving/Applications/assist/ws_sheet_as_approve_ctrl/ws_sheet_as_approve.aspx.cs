using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;
using CoreSavingLibrary.WcfNShrlon;
using System.Data;

namespace Saving.Applications.assist.ws_sheet_as_approve_ctrl
{
    public partial class ws_sheet_as_approve : PageWebSheet, WebSheet
    {
        private n_shrlonClient shrlonService;

        [JsPostBack]
        public string PostSearch { get; set; }
        [JsPostBack]
        public string PostChangeStatus { get; set; }
        

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostSearch)
            {
                dsList.ResetRow();
                dsMain.DATA[0].select_check = "0";
                dsMain.DATA[0].req_status = "8";

                string sqlwhere = "";

                if (dsMain.DATA[0].member_no != "")
                {
                    sqlwhere += " and assreqmaster.member_no like '%" + dsMain.DATA[0].member_no + "%' ";
                }
                else { sqlwhere += ""; }

                if (dsMain.DATA[0].assisttype_code != "")
                {
                    sqlwhere += " and assreqmaster.assisttype_code = '" + dsMain.DATA[0].assisttype_code + "' ";
                }
                else { sqlwhere += ""; }
                dsList.RetrieveList(sqlwhere);
            }
            else if (eventArg == PostChangeStatus)
            {
                for (int i = 0; i < dsList.RowCount; i++)
                {
                    if (dsList.DATA[i].choose_flag == "1")
                    {
                        dsList.DATA[i].REQ_STATUS = Convert.ToDecimal(dsMain.DATA[0].req_status);
                    }
                }
            }
        }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void SaveWebSheet()
        {
            decimal dec_reqStatus;
            string ls_assisDoc;

            try
            {
                for (int i = 0; i < dsList.RowCount; i++)
                {
                    if (dsList.DATA[i].choose_flag == "1")
                    {
                        dec_reqStatus = dsList.DATA[i].REQ_STATUS;
                        ls_assisDoc = dsList.DATA[i].ASSIST_DOCNO.Trim();

                        string sqlStr = @"update assreqmaster 
                            set    
                                req_status = {1} ,approve_date={3}
                            where COOP_ID = {0} and assist_docno = {2}";
                        sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, dec_reqStatus, ls_assisDoc,state.SsWorkDate);
                        WebUtil.ExeSQL(sqlStr);
                    }
                }

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                dsList.ResetRow();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ : " + ex);
            }
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                shrlonService = wcf.NShrlon;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                dsMain.DDloantype();
                //dsList.RetrieveList("");
            }
        }

        public void WebSheetLoadEnd()
        {

        }
    }

}