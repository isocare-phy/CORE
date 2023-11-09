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
using CoreSavingLibrary.WcfNAccount;
using CoreSavingLibrary.WcfNCommon;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNAgency;

namespace Saving.Applications.agency
{
    public partial class w_sheet_ag_receivemem : PageWebSheet, WebSheet
    {
        protected String jsPostMember;
        protected String jsPostcalmemmain;
        str_agent as_agent = new str_agent();
        protected String newClear;
        private n_agencyClient AgencyService;
        private DwThDate tdw_detail;
        private n_commonClient commonService;

        public void InitJsPostBack()
        {
            jsPostMember = WebUtil.JsPostBack(this, "jsPostMember");
            newClear = WebUtil.JsPostBack(this, "newClear");
            jsPostcalmemmain = WebUtil.JsPostBack(this, "jsPostcalmemmain");
            tdw_detail = new DwThDate(dw_detail, this);
            tdw_detail.Add("recv_day", "recv_tday");

        }

        public void WebSheetLoadBegin()
        {
            try
            {
                AgencyService = wcf.NAgency;
                commonService = wcf.NCommon;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            this.ConnectSQLCA();
            if (!IsPostBack)
            {
                if (dw_main.RowCount < 1)
                {
                    try
                    {
                        dw_main.InsertRow(0);
                        dw_detail.InsertRow(0);
                        dw_detail.SetItemDate(1, "recv_day", state.SsWorkDate);
                        tdw_detail.Eng2ThaiAllRow();
                        DwUtil.RetrieveDDDW(dw_main, "recv_period", "agent.pbl", null);
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);

                    }
                }
            }
            else
            {
                try
                {
                    dw_main.RestoreContext();
                    dw_detail.RestoreContext();
                }
                catch { }
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsPostMember")
            {
                JsPostMember();
            }
            else if (eventArg == "jsPostcalmemmain")
            {
                JsPostcalmemmain();
            }
            else if (eventArg == "newClear")
            {
                JsNewClear();
            }

        }

        private void JsPostcalmemmain()
        {
            try
            {
                str_agent as_agentcal = new str_agent();
                as_agentcal.xml_head = TextXmlHead.Text;
                as_agentcal.column_name = Hcolumnname.Value;
                as_agentcal.amount = Convert.ToDecimal(Hrecvamt.Value);
                int result = AgencyService.of_calmemmain(state.SsWsPass, ref as_agentcal);
                if (result == 1)
                {
                    try
                    {
                        TextXmlHead2.Text = as_agentcal.xml_head;
                        DwUtil.ImportData(as_agentcal.xml_head, dw_main, null, FileSaveAsType.Xml);
                        DwUtil.RetrieveDDDW(dw_main, "recv_period", "agent.pbl", null);
                        dw_detail.SetItemDate(1, "recv_day", state.SsWorkDate);

                    }
                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("dw_main--" + ex); }

                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

        private void JsPostMember()
        {
            try
            {
                String rev_period = Hrev_period.Value;
                if ((rev_period == "") || (rev_period == null))
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาตรวจสอบข้อมูล งวด และ ทะเบียนสมาชิก");
                }
                else
                {
                    str_agent as_agent = new str_agent();
                    as_agent.xml_head = dw_main.Describe("DataWindow.Data.Xml");
                    as_agent.xml_detail = dw_detail.Describe("DataWindow.Data.Xml");
                    int result = AgencyService.of_initreceivemem(state.SsWsPass, ref as_agent);
                    if (result == 1)
                    {


                        TextXmlHead.Text = as_agent.xml_head;
                        try
                        {
                            DwUtil.ImportData(as_agent.xml_head, dw_main, null, FileSaveAsType.Xml);
                            DwUtil.RetrieveDDDW(dw_main, "recv_period", "agent.pbl", null);

                        }
                        catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("dw_main--" + ex); }
                        try
                        {

                            DwUtil.ImportData(as_agent.xml_detail, dw_detail, tdw_detail, FileSaveAsType.Xml);
                            dw_detail.ImportString(as_agent.xml_detail, FileSaveAsType.Xml);

                            int row2 = dw_detail.RowCount - 1;
                            if (dw_detail.RowCount > 1)
                            {
                                dw_detail.DeleteRow(dw_detail.RowCount - row2);
                            }
                            dw_detail.SetItemDate(dw_detail.RowCount, "recv_day", state.SsWorkDate);
                        }
                        catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("dw_detail--" + ex); }

                    }
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

        public void SaveWebSheet()
        {
            try
            {
                
                 Decimal recv_amt;
                try
                {
                    recv_amt = Convert.ToDecimal(Hrecvamt.Value);
                }
                catch { recv_amt = 0; }
                String recv_tday = dw_detail.GetItemString(1, "recv_tday");
                if (((recv_tday != "") || (recv_tday != null)) && (recv_amt != 0) )
                {
                    str_agent as_agentsaev = new str_agent();
                    as_agentsaev.xml_head = TextXmlHead2.Text;
                    as_agentsaev.xml_detail = dw_detail.Describe("DataWindow.Data.Xml");

                    int result = AgencyService.of_savereceivemem(state.SsWsPass, as_agentsaev);
                    if (result == 1)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                        JsNewClear();
                    }
                    
                }
                else
                {
                   LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาตรวจสอบข้อมูล วันที่โอน และ ยอดโอนชำระ"); 
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

        public void WebSheetLoadEnd()
        {
            DwUtil.RetrieveDDDW(dw_main, "recv_period", "agent.pbl", null);
            tdw_detail.Eng2ThaiAllRow();

        }
        //JS-EVENT
        private void JsNewClear()
        {
            dw_main.Reset();
            dw_detail.Reset();
            dw_main.InsertRow(0);
            DwUtil.RetrieveDDDW(dw_main, "recv_period", "agent.pbl", null);
            dw_detail.InsertRow(1);
            dw_detail.SetItemDate(1, "recv_day", state.SsWorkDate);
            tdw_detail.Eng2ThaiAllRow();
        }
    }
}
