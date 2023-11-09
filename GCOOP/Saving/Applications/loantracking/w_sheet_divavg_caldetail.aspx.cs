using System;
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
using CommonLibrary;
using Sybase.DataWindow;
using Saving.WsCommon;
using Saving.WsShrlon;
using System.Web.Services.Protocols;
using System.Data.OracleClient;
using DBAccess;

namespace Saving.Applications.loantracking
{
    public partial class w_sheet_divavg_caldetail : PageWebSheet, WebSheet 
    {
        private Shrlon shrlonService;
        private Common commonService;

        private DwThDate tDwMain;
        protected String postInit;
        protected String postNewClear;
        protected String postCalDivDetail;
        protected String postInitMember;
        //======================
        private void JspostInitMember()
        {
            try
            {
                Dw_main.SetItemString(1, "member_no", Hdmem_no.Value);
                String div_year = Dw_main.GetItemString(1, "div_year");
                String member_no = Dw_main.GetItemString(1, "member_no");
                String resultXML = shrlonService.InitDivavgDetail(state.SsWsPass, div_year, member_no);
                if (resultXML != "")
                {
                    Dw_main.Reset();
                    Dw_main.ImportString(resultXML, FileSaveAsType.Xml);
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

        private void JspostCalDivDetail()
        {
            string d_year = "";
            string mber = "";
            try
            {
                d_year = Dw_main.GetItemString(1, "div_year");
            }
            catch { }

            try
            {
                mber = Dw_main.GetItemString(1, "member_no");
            }
            catch { }


            if (d_year == null || mber == null || d_year == "" || mber == "")
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ");
            }
            else
            {
                try
                {
                    String xml_head = Dw_main.Describe("DataWindow.Data.XML");
                    String xml_detail = Dw_detail.Describe("DataWindow.Data.XML");
                    String[] resultXML = shrlonService.CalDivavgDetail(state.SsWsPass, xml_head, xml_detail);
                    if (resultXML[0] != "")
                    {
                        Dw_main.Reset();
                        Dw_main.ImportString(resultXML[0], FileSaveAsType.Xml);

                        if (resultXML[1] != "")
                        {
                            Dw_detail.Reset();
                            Dw_detail.ImportString(resultXML[1], FileSaveAsType.Xml);
                        }
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
        private void JsGetYear()
        {
            int account_year = 0;
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {

                String sql = @"select max(DISTINCT DIV_YEAR) from MBDIVMASTER";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    account_year = int.Parse(dt.GetString("max(DISTINCTDIV_YEAR)"));
                    Dw_main.SetItemString(1, "div_year", account_year.ToString());
                }
                else
                {
                    sqlca.Rollback();
                }
            }
            catch (Exception ex)
            {
                account_year = DateTime.Now.Year;
                account_year = account_year + 543;
                Dw_main.SetItemString(1, "div_year", account_year.ToString());
                //LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        private void JspostNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            Dw_detail.Reset();
            JsGetYear();
        }

        private void JspostInit()
        {
            string d_year = "";
            string mber = "";
            try
            {
                d_year = Dw_main.GetItemString(1, "div_year");
            }
            catch { }

            try
            {
                mber = Dw_main.GetItemString(1, "member_no");
            }
            catch { }


            if (d_year == null || mber == null || d_year == "" || mber == "")
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ");
            }
            else
            {
                try
                {
                    String div_year = Dw_main.GetItemString(1, "div_year");
                    String member_no = Dw_main.GetItemString(1, "member_no");
                    String resultXML = shrlonService.InitDivavgDetail(state.SsWsPass, div_year, member_no);
                    if (resultXML != "")
                    {
                        Dw_main.Reset();
                        Dw_main.ImportString(resultXML, FileSaveAsType.Xml);
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

        #region WebSheet Members

        public void InitJsPostBack()
        {
            postInit = WebUtil.JsPostBack(this, "postInit");
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postCalDivDetail = WebUtil.JsPostBack(this, "postCalDivDetail");
            postInitMember = WebUtil.JsPostBack(this, "postInitMember");


            tDwMain = new DwThDate(Dw_main, this);
            tDwMain.Add("startacc_date", "startacc_tdate");
            tDwMain.Add("endacc_date", "endacc_tdate");
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                this.ConnectSQLCA();
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Database ไม่ได้"); }

            try
            {
                shrlonService = new Shrlon();
                commonService = new Common();

            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Service ไม่ได้"); }

            Dw_main.SetTransaction(sqlca);
            Dw_detail.SetTransaction(sqlca);
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
            if (eventArg == "postInit")
            {
                JspostInit();
            }
            else if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            else if (eventArg == "postCalDivDetail")
            {
                JspostCalDivDetail();
            }
            else if (eventArg == "postInitMember")
            {
                JspostInitMember();
            }
        }

        public void SaveWebSheet()
        {
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
            Dw_main.SaveDataCache();
            Dw_detail.SaveDataCache();
        }

        #endregion
    }
}
