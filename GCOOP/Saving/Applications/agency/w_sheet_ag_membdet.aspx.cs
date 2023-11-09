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
using System.IO;


namespace Saving.Applications.agency
{
    public partial class w_sheet_ag_membdet : PageWebSheet,WebSheet
    {
        private AgencyClient AgencyService;
        private DwThDate tDw_detail;
        protected String postNewClear;
        protected String postInitMembdet_detail;
        protected String postInitMembdet;
        protected String postSearchGroupCode;
        protected String postSelectRow;
        //==============
        private void JspostSelectRow()
        {
            int rowcurrent = 0;
            try 
            {
                 rowcurrent = int.Parse(HdRow.Value.Trim());
            }
            catch { rowcurrent = 1; }
            
            Dw_list.SelectRow(0, false);
            Dw_list.SelectRow(rowcurrent, true);
            Dw_list.SetRow(rowcurrent);
            JspostInitMembdet_detail();
        }

        private void JspostCountMember()
        {
            try 
            {
                String GroupCode = HdGroup.Value.Trim();
                Sta ta = new Sta(sqlca.ConnectionString);
                //select count(a.member_no) from mbmembmaster a where a.membgroup_code = '0001' and a.member_status = 1;
                String sql = @"select count(a.member_no) from mbmembmaster a where a.membgroup_code = '" + GroupCode + "' and a.member_status = 1";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    int countmember = int.Parse(dt.GetString("count(a.member_no)"));
                    lbl_count.Text = countmember.ToString();
                    ta.Close();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบรหัสสังกัด : " + GroupCode);
                    Dw_search.Reset();
                    Dw_search.InsertRow(0);
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        private void JspostSearchGroupCode()
        {
            String Choice = null;
            String GroupCode = null;
            String GroupName = null;
            Choice = Dw_main.GetItemString(1, "choice");
            if (Choice == "1")
            {
                GroupCode = Dw_search.GetItemString(1, "membgroup_code");

                Sta ta = new Sta(sqlca.ConnectionString);
                String sql = @"select membgroup_desc from mbucfmembgroup where membgroup_code = '" + GroupCode + "'";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    GroupName = dt.GetString("membgroup_desc");
                    Dw_search.SetItemString(1, "membgroup_desc", GroupName);
                    ta.Close();
                    JspostInitMembdet_detail();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบรหัสสังกัด : " + GroupCode);
                    JspostNewClear();
                    Dw_search.Reset();
                    Dw_search.InsertRow(0);

                }
            }
            else
            {
                GroupCode = Dw_search.GetItemString(1, "agentgrp_code");
                Sta ta = new Sta(sqlca.ConnectionString);
                String sql = @"select agentgrp_desc from agucfgroupagent where agentgrp_code = '" + GroupCode + "'";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    GroupName = dt.GetString("agentgrp_desc");
                    Dw_search.SetItemString(1, "agentgrp_desc", GroupName);
                    ta.Close();
                    JspostInitMembdet_detail();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบรหัสสังกัดตัวแทน : " + GroupCode);
                    Dw_search.Reset();
                    Dw_search.InsertRow(0);
                }
            }
        }

        private void JspostInitMembdet_detail()
        {
          
            String choice = null;
            try 
            {
                choice = Dw_main.GetItemString(1, "choice");
            }
            catch { choice = null; }

            if (choice != null)
            {
                String group = HdGroup.Value.Trim();
                if (choice == "1")
                {
                    try 
                    {
                        str_agent astr_agent = new str_agent();
                        astr_agent.xml_head = Dw_main.Describe("DataWindow.Data.XML");
                        astr_agent.xml_detail = Dw_detail.Describe("DataWindow.Data.XML");
                        astr_agent.membgroup_code = group;
                        int result = AgencyService.InitMembdet_detail(state.SsWsPass, ref astr_agent);
                        if (result == 1)
                        {
                            String xml_detail = astr_agent.xml_detail;
                            Dw_detail.Reset();
                            Dw_detail.ImportString(xml_detail, FileSaveAsType.Xml);
                            tDw_detail.Eng2ThaiAllRow();
                            JspostCountMember();
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
                else
                {
                    try 
                    {
                        str_agent astr_agent = new str_agent();
                        astr_agent.xml_head = Dw_main.Describe("DataWindow.Data.XML");
                        astr_agent.xml_detail = Dw_detail.Describe("DataWindow.Data.XML");
                        astr_agent.agentgrp_code = group;
                        int result = AgencyService.InitMembdet_detail(state.SsWsPass, ref astr_agent);
                        if (result == 1)
                        {
                            String xml_detail = astr_agent.xml_detail;
                            Dw_detail.Reset();
                            Dw_detail.ImportString(xml_detail, FileSaveAsType.Xml);
                            tDw_detail.Eng2ThaiAllRow();
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
                    catch(Exception ex) 
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                    }
                }
            }
        }
        
        private void ChangeDataWindow()
        {
            switch (Dw_main.GetItemString(1, "choice").ToString())
            {
                case "1":
                    Dw_list.DataWindowObject = "d_agentsrv_membdetgrp_list";
                    Dw_search.DataWindowObject = "d_agentsrv_search_grp";
                    break;
                case "2":
                    Dw_list.DataWindowObject = "d_agentsrv_membdetaggrp_list";
                    Dw_search.DataWindowObject = "d_agentsrv_search_aggrp";
                    break;
            }
            Dw_list.Reset();
            Dw_search.Reset();
        }

        private void JspostNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            Dw_list.Reset();
            Dw_detail.Reset();
            Dw_detail.InsertRow(0);
            Dw_search.Reset();
            lbl_count.Text = "0";
        }

        //InitMembdet
        private void JspostInitMembdet()
        {
            try
            {
                ChangeDataWindow();

                str_agent astr_agent = new str_agent();
                astr_agent.xml_head = Dw_main.Describe("DataWindow.Data.XML");
                astr_agent.xml_list = Dw_list.Describe("DataWindow.Data.XML");
                int result = AgencyService.InitMembdet(state.SsWsPass, ref astr_agent);
                if (result == 1)
                {
                    String xml_list = astr_agent.xml_list;
                    Dw_list.Reset();
                    Dw_list.ImportString(xml_list, FileSaveAsType.Xml);
                    Dw_detail.Reset();
                    Dw_detail.InsertRow(0);
                    Dw_search.InsertRow(0);
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
                // LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + ex);
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาทำรายการค้นหาใหม่");
                JspostNewClear();
            }
        }

        //=============
        #region WebSheet Members

        public void InitJsPostBack()
        {
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postInitMembdet_detail = WebUtil.JsPostBack(this, "postInitMembdet_detail");
            postInitMembdet = WebUtil.JsPostBack(this, "postInitMembdet");
            postSearchGroupCode = WebUtil.JsPostBack(this, "postSearchGroupCode");
            postSelectRow = WebUtil.JsPostBack(this, "postSelectRow");
            //==================
            tDw_detail  = new DwThDate(Dw_detail, this);
            tDw_detail.Add("agent_date", "agent_tdate");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            try
            {
                AgencyService = wcf.Agency;
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Service ไม่ได้"); }

            if (!IsPostBack)
            {
                JspostNewClear();
                DwUtil.RetrieveDDDW(Dw_detail, "membgroup_code", "egat_ag_regagmemb.pbl", null);
                DwUtil.RetrieveDDDW(Dw_detail, "agentgrp_code", "egat_ag_regagmemb.pbl", null);
            }
            else
            {
                this.RestoreContextDw(Dw_main);
                this.RestoreContextDw(Dw_list);
                this.RestoreContextDw(Dw_detail);
                this.RestoreContextDw(Dw_search);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            else if (eventArg == "postInitMembdet_detail")
            {
                Dw_search.Reset();
                Dw_search.InsertRow(0);
                JspostInitMembdet_detail();
            }
            else if (eventArg == "postInitMembdet")
            {
                JspostInitMembdet();
            }
            else if (eventArg == "postSearchGroupCode")
            {
                JspostSearchGroupCode();
            }
            else if (eventArg == "postSelectRow")
            {
                JspostSelectRow();
            }
        }

        public void SaveWebSheet()
        {
           // No Save
        }

        public void WebSheetLoadEnd()
        {
            Dw_main.SaveDataCache();
            Dw_list.SaveDataCache();
            Dw_detail.SaveDataCache();
            Dw_search.SaveDataCache();
            if (Dw_list.RowCount > 0)
            {
                Dw_main.Enabled = false;

            }
            else
            {
                Dw_main.Enabled = true;
            }
        }

        #endregion
    }
}
