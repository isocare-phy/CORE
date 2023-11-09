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
    public partial class w_sheet_ag_receivegroupmem : PageWebSheet, WebSheet
    {
        private AgencyClient agencyService;

        private DwThDate tDw_main;
        protected String postNewClear;
        protected String postInitReceiveGroupMem;
        protected String postrecv_amt;
        protected String postSetBranch;
        protected String postFilterMembCode;
        protected String postRefresh;
        protected String PostsearchMembNo;

        //============================================//
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
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        private void JspostSetBranch()
        {
            String bank_code = Dw_main.GetItemString(1, "expense_bank");
            DataWindowChild D_branch_code = Dw_main.GetChild("expense_branch");
            DwUtil.RetrieveDDDW(Dw_main, "expense_branch", "agent.pbl", null);
            D_branch_code.SetFilter("bank_code = '" + bank_code + "'");
            D_branch_code.Filter();
        }

        private void Jspostrecv_amt()
        {
            int RowCurrent = int.Parse(HdCurrentrow.Value);
            Decimal receive_amt = 0;
            Decimal operate_flag = Dw_detail.GetItemDecimal(RowCurrent, "operate_flag");
            if (operate_flag == 1)
            {
                try
                {
                    receive_amt = Dw_detail.GetItemDecimal(RowCurrent, "receive_amt");
                    Dw_detail.SetItemDecimal(RowCurrent, "recv_amt", receive_amt);
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
        }

        private void JspostInitReceiveGroupMem()
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
                    string agentgrp_code = "";
                    string membgrp_code_begin = "";
                    string membgrp_code_end = "";

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
                        agentgrp_code = Dw_main.GetItemString(1, "agentgrp_code");
                    }
                    catch { }

                    try
                    {
                        membgrp_code_begin = Dw_main.GetItemString(1, "membgrp_code_begin");
                    }
                    catch { }

                    try
                    {
                        membgrp_code_end = Dw_main.GetItemString(1, "membgrp_code_end");
                    }
                    catch { }


                    if (year_period == "" && recv_tday == "" && (agentgrp_code == "" || membgrp_code_begin == "" || membgrp_code_end == ""))
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ");
                    }
                    else
                    {
                        try
                        {
                            String xml_head = Dw_main.Describe("Datawindow.Data.Xml");
                            String resultXML = agencyService.InitReceiveGroupMem(state.SsWsPass, xml_head);
                            if (resultXML != "")
                            {
                                Dw_detail.Reset();
                                Dw_detail.ImportString(resultXML, FileSaveAsType.Xml);
                                Dw_detail.SetFilter("compute_9 > 0");
                                Dw_detail.Filter();
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
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        private void JspostRetreiveDDW()
        {
            DwUtil.RetrieveDDDW(Dw_main, "recv_period", "agent.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "expense_code", "agent.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "expense_bank", "agent.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "tofromacc_id", "agent.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "agentgrp_code", "agent.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "membgrp_code_begin", "agent.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "membgrp_code_end", "agent.pbl", null);
        }

        private void JspostNewClear()
        {
            Dw_main.Enabled = true;
            Dw_main.Reset();
            Dw_main.InsertRow(0);

            Dw_main.SetItemDate(1, "recv_day", state.SsWorkDate);
            tDw_main.Eng2ThaiAllRow();

            Dw_detail.Reset();
            txt_memberno.Text = "";

        }

        private void Refresh()
        {

        }

        private void FilterMembCode()
        {
            String AgentCode;

            try
            {
                AgentCode = HdAgentCode.Value;
            }
            catch
            {
                AgentCode = "";
            }

            DataWindowChild DcMembCodeB = Dw_main.GetChild("membgrp_code_begin");
            DwUtil.RetrieveDDDW(Dw_main, "membgrp_code_begin", "agent.pbl", null);
            DcMembCodeB.SetFilter("agentgrp_code ='" + AgentCode + "'");
            DcMembCodeB.Filter();

            DataWindowChild DcMembCodeE = Dw_main.GetChild("membgrp_code_end");
            DwUtil.RetrieveDDDW(Dw_main, "membgrp_code_end", "agent.pbl", null);
            DcMembCodeE.SetFilter("agentgrp_code ='" + AgentCode + "'");
            DcMembCodeE.Filter();
        }

        private void searchMembNo()
        {
            String membNo;

            try
            {
                membNo = txt_memberno.Text;
            }
            catch
            {
                membNo = "";
            }
            if (membNo != "")
            {
                Dw_detail.SetFilter("member_no='" + membNo + "'");
                Dw_detail.Filter();
            }
            else
            {
                Dw_detail.SetFilter("");
                Dw_detail.Filter();
            }
        }
        //============================================//


        #region WebSheet Members

        public void InitJsPostBack()
        {
            PostsearchMembNo = WebUtil.JsPostBack(this, "PostsearchMembNo");
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postInitReceiveGroupMem = WebUtil.JsPostBack(this, "postInitReceiveGroupMem");
            postrecv_amt = WebUtil.JsPostBack(this, "postrecv_amt");
            postSetBranch = WebUtil.JsPostBack(this, "postSetBranch");
            postFilterMembCode = WebUtil.JsPostBack(this, "postFilterMembCode");
            //=====================
            tDw_main = new DwThDate(Dw_main, this);
            tDw_main.Add("recv_day", "recv_tday");
        }

        public void WebSheetLoadBegin()
        {
            HdSaveRecGroupMem.Value = "false";

            try
            {
                agencyService = wcf.Agency;
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Service ไม่ได้"); }

            if (!IsPostBack)
            {
                JspostNewClear();
                JspostRetreiveDDW();
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
            else if (eventArg == "postInitReceiveGroupMem")
            {
                JspostInitReceiveGroupMem();
            }
            else if (eventArg == "postrecv_amt")
            {
                Jspostrecv_amt();
            }
            else if (eventArg == "postSetBranch")
            {
                JspostSetBranch();
            }
            else if (eventArg == "postFilterMembCode")
            {
                FilterMembCode();
            }
            else if (eventArg == "postRefresh")
            {
                Refresh();
            }
            else if (eventArg == "PostsearchMembNo")
            {
                searchMembNo();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string year_period = "";
                string recv_tday = "";
                string agentgrp_code = "";
                string membgrp_code_begin = "";
                string membgrp_code_end = "";

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
                    agentgrp_code = Dw_main.GetItemString(1, "agentgrp_code");
                }
                catch { }

                try
                {
                    membgrp_code_begin = Dw_main.GetItemString(1, "membgrp_code_begin");
                }
                catch { }

                try
                {
                    membgrp_code_end = Dw_main.GetItemString(1, "membgrp_code_end");
                }
                catch { }


                if (year_period == "" && recv_tday == "" && (agentgrp_code == "" || membgrp_code_begin == "" || membgrp_code_end == ""))
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
                            agencyService.RunSaveRecGroupMemProcess(state.SsWsPass, xml_head, xml_detail, state.SsApplication, state.CurrentPage, state.SsUsername, state.SsClientIp, DateTime.Now, state.SsWorkDate);
                            HdSaveRecGroupMem.Value = "true";
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
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบรายการ กรุณาเลือกรายการ");
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

            Dw_main.SaveDataCache();
            Dw_detail.SaveDataCache();

            if (Dw_detail.RowCount > 0)
            {
                SetDwMasterEnable(1);
            }
            else
            {
                SetDwMasterEnable(0);
            }
        }

        #endregion

        

        protected void B_search_Click(object sender, EventArgs e)
        {
            String member_no = txt_memberno.Text.ToString();
            int RowMemberNo = Dw_detail.FindRow("member_no = '" + member_no + "'", 0, Dw_detail.RowCount);
            if (RowMemberNo > 0)
            {
                Dw_detail.SelectRow(0, false);
                Dw_detail.SelectRow(RowMemberNo, true);
                Dw_detail.SetRow(RowMemberNo);
            }
            else
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลทะเบียน : " + member_no + "กรุณากรอกใหม่");
            }
        }
    }
}

