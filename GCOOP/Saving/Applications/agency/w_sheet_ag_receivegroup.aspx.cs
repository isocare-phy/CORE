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
    public partial class w_sheet_ag_receivegroup : PageWebSheet,WebSheet 
    {
        private AgencyClient  agencyService;
        private CommonClient commonService;

        private DwThDate tDw_main; 
        protected String postNewClear;
        protected String postInitReceiveGroup;
        protected String postrecv_amt;
        protected String postSetBranch;
    
        //======================
    
        public void SetDwMasterEnable(int protect)
        {
            try
            {
                if (protect == 1)
                {
                    Dw_main.Enabled = false;
                }
                else
                {
                    Dw_main.Enabled = true;
                }
                int RowAll = int.Parse(Dw_main.Describe("Datawindow.Column.Count"));
                for (int li_index = 1; li_index <= RowAll; li_index++)
                {
                    Dw_main.Modify("#" + li_index.ToString() + ".protect= " + protect.ToString());
                }
            }
            catch (Exception ex)
            {
                //  LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        private void JspostSetBranch()
        {
            int RowCurrent = int.Parse(HdCurrentrow.Value);
            String BranchId = HdBranchId.Value;
            Dw_detail.SetItemString(RowCurrent, "expense_branch", BranchId);
        }

        private void Jspostrecv_amt()
        {
            int RowCurrent = int.Parse(HdCurrentrow.Value);
            Decimal stmt_amt = 0;
            Decimal operate_flag = Dw_detail.GetItemDecimal(RowCurrent, "operate_flag");
            if (operate_flag == 1)
            {
                try
                {
                    stmt_amt = Dw_detail.GetItemDecimal(RowCurrent, "stmt_amt");
                    Dw_detail.SetItemDecimal(RowCurrent, "recv_amt", stmt_amt);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else 
            {
                try
                {
                    Dw_detail.SetItemDecimal(RowCurrent, "recv_amt", 0);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            //JspostRetreiveDDW();
        }

        private void JspostInitReceiveGroup()
        {
            try
            {
                if (Dw_detail.RowCount > 0)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากดปุ่ม New เพื่อเคลียร์ข้อมูลก่อน");
                }
                else 
                {
                    string year_period = "";
                    string recv_tday = "";
                    string sgroup = "";
                    string egroup = "";
                    try
                    {
                        year_period = Dw_main.GetItemString(1, "recv_period");
                    }
                    catch { }

                    try
                    {
                        recv_tday = Dw_main.GetItemString(1, "recv_tday");
                    }
                    catch { }

                    try
                    {
                        sgroup = Dw_main.GetItemString(1, "sagentgrp_code");
                    }
                    catch { }

                    try
                    {
                        egroup = Dw_main.GetItemString(1, "eagentgrp_code");
                    }
                    catch { }


                    if (year_period == "" || recv_tday == "" || sgroup == "" || egroup == "")
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ");
                    }
                    else
                    {

                        try
                        {
                            String xml_head = Dw_main.Describe("Datawindow.Data.Xml");
                            String resultXML = agencyService.InitReceiveGroup(state.SsWsPass, xml_head);
                            if (resultXML != "")
                            {
                                Dw_detail.Reset();
                                Dw_detail.ImportString(resultXML, FileSaveAsType.Xml);
                                DwUtil.RetrieveDDDW(Dw_detail, "expense_code", "agent.pbl", null);
                                DwUtil.RetrieveDDDW(Dw_detail, "expense_bank", "agent.pbl", null);
                                DwUtil.RetrieveDDDW(Dw_detail, "expense_branch", "agent.pbl", null);
                                DwUtil.RetrieveDDDW(Dw_detail, "tofromacc_id", "agent.pbl", null);
                            }
                            else
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                                //JspostNewClear();
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
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        private void JspostRetreiveDDW()
        {
            DwUtil.RetrieveDDDW(Dw_detail, "expense_code", "agent.pbl", null);
            DwUtil.RetrieveDDDW(Dw_detail, "expense_bank", "agent.pbl", null);
            //DwUtil.RetrieveDDDW(Dw_detail, "expense_branch", "agent.pbl", null);
            DwUtil.RetrieveDDDW(Dw_detail, "tofromacc_id", "agent.pbl", null);
        }

        private void JspostNewClear()
        {
            Dw_main.Enabled = true;
            Dw_main.Reset();
            Dw_main.InsertRow(0);

            Dw_main.SetItemDate(1, "recv_day", state.SsWorkDate);
            tDw_main.Eng2ThaiAllRow();

            Dw_detail.Reset();
            CheckAll.Checked = false;
          
        }
        //======================
        #region WebSheet Members

        public void InitJsPostBack()
        {
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postInitReceiveGroup = WebUtil.JsPostBack(this, "postInitReceiveGroup");
            postrecv_amt = WebUtil.JsPostBack(this, "postrecv_amt");
            postSetBranch = WebUtil.JsPostBack(this, "postSetBranch");
            
            //=====================
            tDw_main = new DwThDate(Dw_main , this);
            tDw_main.Add("recv_day", "recv_tday");

        }

        public void WebSheetLoadBegin()
        {
            HdSaveRecGroup.Value = "false";

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
            else if (eventArg == "postInitReceiveGroup")
            {
                JspostInitReceiveGroup();
            }
            else if (eventArg == "postrecv_amt")
            {
                Jspostrecv_amt();
            }
            else if (eventArg == "postSetBranch")
            {
                JspostSetBranch();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string year_period = "";
                string recv_tday = "";
                string sgroup = "";
                string egroup = "";
                string expense_code = "";
                string expense_bank = "";
                string expense_branch = "";
                string tofromacc_id = "";
                try
                {
                    year_period = Dw_main.GetItemString(1, "recv_period");
                }
                catch { }

                try
                {
                    recv_tday = Dw_main.GetItemString(1, "recv_tday");
                }
                catch { }

                try
                {
                    sgroup = Dw_main.GetItemString(1, "sagentgrp_code");
                }
                catch { }

                try
                {
                    egroup = Dw_main.GetItemString(1, "eagentgrp_code");
                }
                catch { }


                if (year_period == "" || recv_tday == "" || sgroup == "" || egroup == "")
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ");
                }
                else
                {
                    int RowCheck = Dw_detail.FindRow("operate_flag = 1", 0, Dw_detail.RowCount);
                    if (RowCheck > 0)
                    {
                        try
                        {
                            String xml_head = Dw_main.Describe("Datawindow.Data.Xml");
                            String xml_detail = Dw_detail.Describe("Datawindow.Data.Xml");
                            agencyService.RunSaveRecGroupProcess(state.SsWsPass, xml_head, xml_detail, state.SsApplication, state.CurrentPage);
                            HdSaveRecGroup.Value = "true";
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
                    else 
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีรายการที่เลือก กรุณาเลือกรายการ");
                    }
                    
                }
            }
            catch(Exception ex)
            {
            
            }
        }
    
        public void WebSheetLoadEnd()
        {
            if (Dw_main.RowCount > 1)
            {
                Dw_main.DeleteRow(Dw_main.RowCount);
            }
            if (Dw_detail.RowCount > 0)
            {
                SetDwMasterEnable(1);
            }
            else 
            {
                SetDwMasterEnable(0);
            }
            //recv_period
            DwUtil.RetrieveDDDW(Dw_main, "recv_period", "agent.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "sagentgrp_code", "agent.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "eagentgrp_code", "agent.pbl", null);
            Dw_main.SaveDataCache();
            Dw_detail.SaveDataCache();

        }

        #endregion

        protected void CheckAll_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckAll.Checked)
            {
                if (Dw_detail.RowCount > 0)
                {
                    for (int i = 1; i <= Dw_detail.RowCount; i++)
                    {
                        Decimal recv_amt;
                        recv_amt = Dw_detail.GetItemDecimal(i, "stmt_amt");
                        Dw_detail.SetItemDecimal(i, "operate_flag", 1);
                        Dw_detail.SetItemDecimal(i, "recv_amt", recv_amt);
                    }
                }
                else 
                {
                    CheckAll.Checked = false;
                }
                
            }
            else
            {
                for (int i = 1; i <= Dw_detail.RowCount; i++)
                {
                    Dw_detail.SetItemDecimal(i, "operate_flag", 0);
                    Dw_detail.SetItemDecimal(i, "recv_amt", 0);
                    Dw_detail.SetItemString(i, "expense_code", null);
                    Dw_detail.SetItemString(i, "expense_bank", null);
                    Dw_detail.SetItemString(i, "expense_branch", null);
                    Dw_detail.SetItemString(i, "tofromacc_id", null);
                }
            }
        }

        
    }
}
