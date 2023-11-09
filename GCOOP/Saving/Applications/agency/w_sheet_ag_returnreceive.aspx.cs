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
using Sybase.DataWindow;
using CoreSavingLibrary.WcfAgency;
using CoreSavingLibrary.WcfCommon;
using System.Web.Services.Protocols;
using DataLibrary;
using System.Globalization;

namespace Saving.Applications.agency
{
    public partial class w_sheet_ag_returnreceive : PageWebSheet,WebSheet 
    {
        private AgencyClient  agencyService;
        private DwThDate tDw_detail;
        protected String postNewClear;
        protected String postInitReturnReceive;
        protected String postret_amt;
        protected String postInitMember;
        //============================
        private void JspostInitMember()
        {
            try
            {
                string year_period = "";
                string member_no = "";
                try
                {
                    year_period = Dw_main.GetItemString(1, "recv_period");
                }
                catch { }

                try
                {
                    member_no = Dw_main.GetItemString(1, "member_no");
                }
                catch { }


                if (year_period == "" || member_no == "")
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ");
                }
                else
                {

                    try
                    {
                        String xml_head = Dw_main.Describe("Datawindow.Data.Xml");
                        String[] resultXML = agencyService.InitReturnReceive(state.SsWsPass, xml_head);
                        if (resultXML[1] == "" || resultXML[1] == null)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                            JspostNewClear();
                        }
                        else
                        {
                            Dw_main.Reset();
                            Dw_main.ImportString(resultXML[0], FileSaveAsType.Xml);

                            if (resultXML[0] != "")
                            {
                                Dw_detail.Reset();
                                DwUtil.ImportData(resultXML[1], Dw_detail, null, FileSaveAsType.Xml);
                                Dw_detail.SetItemDate(1, "ret_day", state.SsWorkDate);
                                tDw_detail.Eng2ThaiAllRow();
                            }
                        }
                    }
                    catch (SoapException ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        private void Jspostret_amt()
        {
            try
            {
                string year_period = "";
                string member_no = "";
                decimal ret_amt = 0;
                try
                {
                    year_period = Dw_main.GetItemString(1, "recv_period");
                }
                catch { }

                try
                {
                    member_no = Dw_main.GetItemString(1, "member_no");
                }
                catch { }


                if (year_period == "" || member_no == "")
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ");
                }
                else
                {
                    ret_amt = Dw_detail.GetItemDecimal(1, "ret_amt");


                    try
                    {
                        String xml_head = Dw_main.Describe("Datawindow.Data.Xml");
                        String column_name = "ret_all_amt";
                        String resultXML = agencyService.Cal_MemMain(state.SsWsPass, xml_head, column_name, ret_amt);

                        if (resultXML != "")
                        {
                            DwUtil.ImportData(resultXML, Dw_main , null, FileSaveAsType.Xml);
                        }
                        else
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                            JspostNewClear();
                        }
                    }
                    catch (SoapException ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }
  
        private void JspostInitReturnReceive()
        {
            String recvPeriod = Dw_main.GetItemString(1, "recv_period");
            String memberNo = Dw_main.GetItemString(1, "member_no");
            try
            {
                String xml_head = Dw_main.Describe("Datawindow.Data.Xml");
                String[] resultXML = agencyService.InitReturnReceive(state.SsWsPass, xml_head);
                if (resultXML[1] == "" || resultXML[1] == null)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                    JspostNewClear();
                }
                else
                {
                    Dw_main.Reset();
                    DwUtil.ImportData(resultXML[0], Dw_main, null, FileSaveAsType.Xml);

                    if (resultXML[0] != "")
                    {
                        Dw_detail.Reset();
                        DwUtil.ImportData(resultXML[1], Dw_detail, null, FileSaveAsType.Xml);
                        Dw_detail.SetItemDate(1, "ret_day", state.SsWorkDate);
                        tDw_detail.Eng2ThaiAllRow();
                    }
                }
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
               
            Decimal receiveAmt = Dw_main.GetItemDecimal(1, "receive_amt");
            Decimal outStanding = Dw_main.GetItemDecimal(1, "outstandingbal_begin");
            Decimal recvAmt = Dw_main.GetItemDecimal(1, "recv_amt");
            Decimal addRecvAmt = Dw_main.GetItemDecimal(1, "addrecv_amt");
            Decimal retAmt = Dw_main.GetItemDecimal(1, "ret_all_amt");
            Decimal adjAmt = Dw_main.GetItemDecimal(1, "adj_all_amt");
            Decimal cancelAmt = Dw_main.GetItemDecimal(1, "cancel_all_amt");
            Decimal totalAmt = receiveAmt + outStanding - recvAmt - addRecvAmt + retAmt - adjAmt + cancelAmt;

            if (totalAmt >= 0)
            {
                Dw_main.Reset();
                Dw_detail.Reset();
                Dw_main.InsertRow(0);
                Dw_detail.InsertRow(0);
                LtServerMessage.Text = WebUtil.ErrorMessage("ข้อมูลงวดที่ : " + recvPeriod + " เลขทะเบียนที่ : " + memberNo + " ไม่สามารถทำการคืนเงินลูกหนี้ตัวแทนได้");
            }
            else
            {
                Dw_detail.SetItemDecimal(1, "ret_amt", totalAmt * -1);
                Jspostret_amt();
            }
        }

        private void JspostNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            
            Dw_detail.Reset();
            Dw_detail.InsertRow(0);
        }
        //======================

        #region WebSheet Members

        public void InitJsPostBack()
        {
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postInitReturnReceive = WebUtil.JsPostBack(this, "postInitReturnReceive");
            postret_amt = WebUtil.JsPostBack(this, "postret_amt");
            postInitMember = WebUtil.JsPostBack(this, "postInitMember");

            //=====================
            tDw_detail = new DwThDate(Dw_detail, this);
            tDw_detail.Add("ret_day", "ret_tday");
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                agencyService = wcf.Agency;
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Service ไม่ได้"); }

            if (!IsPostBack)
            {
                JspostNewClear();
            }
            else
            {
                this.RestoreContextDw(Dw_main);
                this.RestoreContextDw(Dw_detail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            else if (eventArg == "postInitReturnReceive")
            {
                JspostInitReturnReceive();
            }
            else if (eventArg == "postret_amt")
            {
                Jspostret_amt();
            }
            else if (eventArg == "postInitMember")
            {
                JspostInitMember();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string year_period = "";
                string member_no = "";
                string ret_tday = "";
                string cause_code = "";
                decimal ret_amt = 0;
                try
                {
                    year_period = Dw_main.GetItemString(1, "recv_period");
                }
                catch { }

                try
                {
                    member_no = Dw_main.GetItemString(1, "member_no");
                }
                catch { }

                try
                {
                    ret_tday = Dw_detail.GetItemString(1, "ret_tday");
                }
                catch { }

                try
                {
                    cause_code = Dw_detail.GetItemString(1, "cause_code");
                }
                catch { }

                try
                {
                    ret_amt = Dw_detail.GetItemDecimal(1, "ret_amt");
                }
                catch { }


                if (year_period == "" || member_no == "" || ret_tday == "" || cause_code == "" || ret_amt == 0)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ : งวด/ทะเบียน/วันที่ทำรายการ/สาเหตุการคืน/ยอดคืน");
                }
                else
                {
                        try
                        {
                            String xml_head = Dw_main.Describe("Datawindow.Data.Xml");
                            Dw_detail.SetItemString(1, "entry_id", state.SsUsername);
                            Dw_detail.SetItemString(1, "machine_id", state.SsClientIp);
                            Dw_detail.SetItemDateTime(1, "system_date", state.SsWorkDate);
                            Dw_detail.SetItemDateTime(1, "adj_time", DateTime.Now);
                            String xml_detail = Dw_detail.Describe("Datawindow.Data.Xml");
                            int result = agencyService.SaveReturnReceive(state.SsWsPass, xml_head, xml_detail);
                            if (result == 1)
                            {
                                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จเรียบร้อยแล้ว");
                                JspostNewClear();
                            }
                        }
                        catch (SoapException ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage(ex.ToString());
                        }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {
            if (Dw_main.RowCount > 1)
            {
                Dw_main.DeleteRow(Dw_main.RowCount);
            }

            if (Dw_detail.RowCount > 1)
            {
                Dw_detail.DeleteRow(Dw_detail.RowCount);
            }

            DwUtil.RetrieveDDDW(Dw_main, "recv_period", "agent.pbl", null);
            DwUtil.RetrieveDDDW(Dw_detail, "cause_code", "agent.pbl", null);
            Dw_main.SaveDataCache();
            Dw_detail.SaveDataCache();
        }

        #endregion
    }
}
