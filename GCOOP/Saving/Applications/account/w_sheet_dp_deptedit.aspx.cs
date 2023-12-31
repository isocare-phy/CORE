﻿using System;
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
using CoreSavingLibrary.WcfNDeposit;
using System.Web.Services.Protocols;
using DataLibrary;

namespace Saving.Applications.account
{
    public partial class w_sheet_dp_deptedit : PageWebSheet, WebSheet
    {
        protected String postAccountNo;
        private n_depositClient depServ;
        protected String testControls;
        private DwThDate thDwMain;
        private DwThDate thDwTab1;
        private DwThDate thDwTab2;
        private String pblFileName = "dp_deptedit.pbl";
        protected String CheckCoop;
        protected String postNew;
        protected String CoopName;
        protected Decimal CrossFlg;
        protected String setCoopname;
        Sdt dt;
        Sta ta;

        private void JsPostAccountNo()
        {
            try
            {

                String dwAccNo = DwMain.GetItemString(1, "deptaccount_no");
                String accNo = depServ.of_analizeaccno(state.SsWsPass, dwAccNo);
                object[] argsDwMain = new object[2] { HfCoopid.Value, accNo };
                try
                {
                    DwUtil.RetrieveDataWindow(DwMain, pblFileName, thDwMain, argsDwMain);
                }
                catch { }
                try
                {
                    DwTab1.Reset();
                    DwUtil.RetrieveDataWindow(DwTab1, pblFileName, thDwTab1, argsDwMain);
                }
                catch { }
                try
                {
                    DwTab2.Reset();
                    DwUtil.RetrieveDataWindow(DwTab2, pblFileName, thDwTab2, argsDwMain);
                }
                catch { }
                try
                {
                    DwTab3.Reset();
                    DwUtil.RetrieveDataWindow(DwTab3, pblFileName, null, argsDwMain);
                }
                catch { }
                try
                {
                    DwTab4.Reset();
                    DwUtil.RetrieveDataWindow(DwTab4, pblFileName, null, argsDwMain);
                }
                catch { }
                if (DwMain.RowCount < 1)
                {
                    throw new Exception("ไม่พบเลขบัญชีดังกล่าว");
                }
                String accFormat = ViewAccountNoFormat(state.SsWsPass, DwMain.GetItemString(1, "deptaccount_no"));
                DwMain.SetItemString(1, "deptaccount_no", accFormat);
                //     DwMain.SetItemDecimal(1, "cross_coopflag", CrossFlg);
                //     DwMain.SetItemString(1, "dwcrosscoop", HfCoopid.Value);
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }
        public String ViewAccountNoFormat(String wsPass, String deptAccountNo)
        {
            dt = ta.Query("select deptcode_mask from DPDEPTCONSTANT");
            if (!dt.Next())
            {
                ta.Close();
                throw new Exception("ไม่สามารถติดต่อกับ Database ได้");
            }
            else
            {
                ta.Close();
            }
            String format = dt.GetString(0).ToUpper();//"X-XX-XXXXXXX";
            char[] fc = format.ToCharArray();
            char[] ac = deptAccountNo.ToCharArray();
            String accNo = "";
            int j = 0;
            for (int i = 0; i < fc.Length; i++)
            {
                if (fc[i] != 'X')
                {
                    accNo += fc[i].ToString();
                }
                else
                {
                    try
                    {
                        accNo += ac[j++];
                    }
                    catch { accNo += ""; }
                }
            }
            return accNo;
        }
        #region WebSheet Members

        public void InitJsPostBack()
        {
            postAccountNo = WebUtil.JsPostBack(this, "postAccountNo");
            CheckCoop = WebUtil.JsPostBack(this, "CheckCoop");
            postNew = WebUtil.JsPostBack(this, "postNew");
            setCoopname = WebUtil.JsPostBack(this, "setCoopname");
            //----------------------------------------------------------------
            thDwMain = new DwThDate(DwMain, this);
            thDwMain.Add("deptopen_date", "deptopen_tdate");
            thDwMain.Add("lastcalint_date", "lastcalint_tdate");
            //----------------------------------------------------------------
            thDwTab1 = new DwThDate(DwTab1, this);
            thDwTab1.Add("entry_date", "entry_tdate");
            thDwTab1.Add("operate_date", "operate_tdate");
            //----------------------------------------------------------------
            thDwTab2 = new DwThDate(DwTab2, this);
            thDwTab2.Add("prnc_date", "prnc_tdate");
            thDwTab2.Add("prncdue_date", "prncdue_tdate");
            thDwTab2.Add("lastcalint_date", "lastcalint_tdate");
            //----------------------------------------------------------------
        }

        public void WebSheetLoadBegin()
        {
            depServ = wcf.NDeposit;
            this.ConnectSQLCA();
            if (IsPostBack)
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwListCoop);
                this.RestoreContextDw(DwTab1);
                this.RestoreContextDw(DwTab2);
                this.RestoreContextDw(DwTab3);
                this.RestoreContextDw(DwTab4);

            }
            else
            {
                DwMain.InsertRow(0);
                DwListCoop.InsertRow(0);
                DwTab1.InsertRow(0);
                DwTab2.InsertRow(0);
                DwTab3.InsertRow(0);
                DwTab4.InsertRow(0);
                HfCoopid.Value = state.SsCoopId;
                ////HdIsPostBack.Value = "false";
                //string coop_control = state.SsCoopControl;
                //DwUtil.RetrieveDDDW(DwMain, "dwcrosscoop", pblFileName, coop_control);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postAccountNo")
            {
                JsPostAccountNo();
            }
            else if (eventArg == "CheckCoop")
            {
                checkCoop();
            }
            else if (eventArg == "postNew")
            {
                JspostNew();
            }
            else if (eventArg == "setCoopname")
            {
                JsSetCoopname();
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwTab1.PageNavigationBarSettings.Visible = (DwTab1.RowCount > 10);
                DwTab2.PageNavigationBarSettings.Visible = (DwTab2.RowCount > 10);
                DwTab3.PageNavigationBarSettings.Visible = (DwTab3.RowCount > 10);
                DwTab4.PageNavigationBarSettings.Visible = (DwTab4.RowCount > 10);
                if (DwMain.RowCount <= 0)
                {
                    DwMain.InsertRow(0);
                }
            }
            catch { }
            DwMain.SaveDataCache();
            DwListCoop.SaveDataCache();
            DwTab1.SaveDataCache();
            DwTab2.SaveDataCache();
            DwTab3.SaveDataCache();
            DwTab4.SaveDataCache();
        }

        #endregion
        private void checkCoop()
        {

            decimal i = 0;
            decimal crossflag = DwListCoop.GetItemDecimal(1, "cross_coopflag");
            if (crossflag == 1)
            {
                try
                {
                    i = DwListCoop.GetItemDecimal(1, "cross_coopflag");
                }
                catch
                { }

                JspostNew();
                DwUtil.RetrieveDDDW(DwListCoop, "dddwcoopname", "cm_constant_config.pbl", state.SsCoopId, state.SsCoopControl);
            }
            else
            {
                try
                {
                    JspostNew();
                    HfCoopid.Value = state.SsCoopId + "";
                }
                catch
                { }
            }

        }

        private void JspostNew()
        {
            DwMain.Reset();
            DwTab1.Reset();
            DwTab2.Reset();
            DwTab3.Reset();
            DwTab4.Reset();

            DwMain.InsertRow(0);
            DwTab1.InsertRow(0);
            DwTab2.InsertRow(0);
            DwTab3.InsertRow(0);
            DwTab4.InsertRow(0);
            HdIsPostBack.Value = "false";
            //string coop_control = state.SsCoopControl;
            //DwUtil.RetrieveDDDW(DwMain, "dwcrosscoop",pblFileName, coop_control);
        }

        private void JsSetCoopname()
        {
            String Coopid = HfCoopid.Value;
            String Coopname;
            DataTable dt = WebUtil.Query("select coop_name from cmcoopmaster where coop_id ='" + Coopid + "'");
            if (dt.Rows.Count > 0)
            {
                Coopname = dt.Rows[0]["coop_name"].ToString();
                DwListCoop.SetItemDecimal(1, "cross_coopflag", 1);
                DwUtil.RetrieveDDDW(DwListCoop, "dddwcoopname", "cm_constant_config.pbl", state.SsCoopId, state.SsCoopControl);
                DwListCoop.SetItemString(1, "dddwcoopname", Coopname);
            }

            JsPostAccountNo();
        }
    }
}