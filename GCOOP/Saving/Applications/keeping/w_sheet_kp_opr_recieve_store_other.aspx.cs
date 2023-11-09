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
using CoreSavingLibrary.WcfNCommon;
using CoreSavingLibrary.WcfNShrlon;
using Sybase.DataWindow;
using DataLibrary;


namespace Saving.Applications.keeping
{
    public partial class w_sheet_kp_opr_recieve_store_other : PageWebSheet, WebSheet
    {
        private DwThDate tdwmain;
        private n_shrlonClient shrlonService;
        private n_commonClient commonService;
        protected String jsPostMember;
        protected String jsPostGroup;
        protected String newClear;
        protected String jsRefresh;
        protected String jsmembgroup_code;
        protected String jsCoopSelect;
        protected String jsChangmidgroupcontrol;
        void WebSheet.InitJsPostBack()
        {
            jsPostMember = WebUtil.JsPostBack(this, "jsPostMember");
            newClear = WebUtil.JsPostBack(this, "newClear");
            jsChangmidgroupcontrol = WebUtil.JsPostBack(this, "jsChangmidgroupcontrol");
            jsmembgroup_code = WebUtil.JsPostBack(this, "jsmembgroup_code");
            jsCoopSelect = WebUtil.JsPostBack(this, "jsCoopSelect");
            tdwmain = new DwThDate(dw_main, this);
            //tdwmain.Add("entry_date", "entry_tdate");

        }

        void WebSheet.WebSheetLoadBegin()
        {
            try
            {
                shrlonService = wcf.NShrlon;
                commonService = wcf.NCommon;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            this.ConnectSQLCA();
            dw_main.SetTransaction(sqlca);
            dw_detail.SetTransaction(sqlca);

            if (IsPostBack)
            {
                try
                {

                    this.RestoreContextDw(dw_main);
                    this.RestoreContextDw(dw_detail);
                    HdIsPostBack.Value = "true";
                }
                catch { }
            }
            else
            {
                dw_main.InsertRow(0);
                dw_detail.InsertRow(0);
                DwUtil.RetrieveDDDW(dw_detail, "keepothitemtype_code", "kp_recieve_store.pbl", state.SsCoopControl);
                dw_detail.SetItemDecimal(1, "keepother_type", 1);
                tdwmain.Eng2ThaiAllRow();
                HdIsPostBack.Value = "False";
                

            }
        }

        void WebSheet.CheckJsPostBack(String eventArg)
        {
            if (eventArg == "jsPostMember")
            {
                JsPostMember();
            }
            else if (eventArg == "newClear")
            {
                JsNewClear();

            }


        }




        void WebSheet.SaveWebSheet()
        {
            int j = 0; int k = 0;

            try
            {
                for (int i = 1; i <= dw_detail.RowCount; i++)
                {
                    String member_no = dw_main.GetItemString(1, "member_no");
                    decimal seq_no = dw_detail.GetItemDecimal(i, "cRow");
                    dw_detail.SetItemString(i, "coop_id", state.SsCoopControl);
                    dw_detail.SetItemString(i, "memcoop_id", state.SsCoopId);
                    dw_detail.SetItemString(i, "member_no", member_no);
                    dw_detail.SetItemDecimal(i, "seq_no", seq_no);
                    dw_detail.SetItemDateTime(i, "entry_date", state.SsWorkDate);
                    dw_detail.SetItemString(i, "entry_id", state.SsUsername);
                    if (state.SsCoopControl == "011001")
                    {
                        dw_detail.SetItemString(i, "keepitemtype_code", "INS");
                    }

                }
                for (int i = 1; i <= dw_detail.RowCount; i++)
                {
                    try
                    {
                        string recv_period = dw_detail.GetItemString(i, "startkeep_period");
                        j++;
                    }
                    catch (Exception ex)
                    {
                        j--;
                    }
                }
                for (int i = 1; i <= dw_detail.RowCount; i++)
                {
                    if (dw_detail.GetItemDecimal(i, "item_payment") == 0)
                    {
                        k--;
                    }
                    else
                    {
                        k++;
                    }
                }
                if (j < dw_detail.RowCount)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกงวดเริ่มเก็บ");
                }
                else if (k < dw_detail.RowCount)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("จำนวนเงินเป็นศูนย์");
                }
                else
                {
                    dw_detail.UpdateData();
                    JsNewClear();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                }

            }
            catch (Exception ex)
            {

                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }


        }

        void WebSheet.WebSheetLoadEnd()
        {

            dw_main.SaveDataCache();
            dw_detail.SaveDataCache();

        }
        private void JsPostMember()
        {
            String coop_id = state.SsCoopControl;
            String member_no = dw_main.GetItemString(1, "member_no");

            try
            {
                DwUtil.RetrieveDDDW(dw_detail, "keepothitemtype_code", "kp_recieve_store.pbl", state.SsCoopControl);
                DwUtil.RetrieveDataWindow(dw_main, "kp_opr_receive_store_other.pbl", null, coop_id, member_no); //dw_main.Retrieve(coop_id, member_no);
                DwUtil.RetrieveDataWindow(dw_detail, "kp_opr_receive_store_other.pbl", null, coop_id, member_no); //dw_detail.Retrieve(coop_id, member_no);
                if (dw_detail.RowCount == 0)
                {
                    dw_detail.InsertRow(0);
                    if (state.SsCoopControl == "011001")
                    {
                        dw_detail.SetItemString(1, "keepothitemtype_code", "00");
                        dw_detail.SetItemString(1, "description", "ค่าเบี้ยประกัน");
                    }
                    else
                    {
                        dw_detail.SetItemDecimal(1, "keepother_type", 1);
                    }
                }

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);

            }
        }
        //JS-EVENT
        private void JsNewClear()
        {
            dw_main.Reset();
            dw_detail.Reset();
            dw_main.InsertRow(0);
            dw_detail.InsertRow(0);
            DwUtil.RetrieveDDDW(dw_detail, "keepothitemtype_code", "kp_recieve_store.pbl", state.SsCoopControl);
            dw_detail.SetItemDecimal(1, "keepother_type", 1);
            HdIsPostBack.Value = "False";
        }
    }
}
