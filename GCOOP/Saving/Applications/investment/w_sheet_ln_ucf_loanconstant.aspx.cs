using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNCommon;
using DataLibrary;
using Sybase.DataWindow;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_ucf_loanconstant : PageWebSheet, WebSheet
    {
        protected String jsPostDel;
        protected String jsPostInsert;
        public String pbl = "ln_ucf_all.pbl";
        public void InitJsPostBack()
        {
            jsPostDel = WebUtil.JsPostBack(this, "jsPostDel");
            jsPostInsert = WebUtil.JsPostBack(this, "jsPostInsert");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, null);
                if (DwMain.RowCount == 0)
                {
                    DwMain.InsertRow(0);
                }
            }
            else
            {

                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostDel":
                    JsPostDel();
                    break;
                case "jsPostInsert":
                    DwMain.InsertRow(0);
                    DwMain.SetItemString(DwMain.RowCount, "coop_id", state.SsCoopId);
                    break;

            }
        }

        public void SaveWebSheet()
        {
            try
            {
                SaveInfo();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จเนื่องจาก : "+ex); }

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }
        public void JsPostDel()
        {
            int r = Convert.ToInt32(HdRow.Value);
            string sqldel = "";
            Decimal formatyear_type = 0;
            try
            {
                formatyear_type = DwMain.GetItemDecimal(r, "formatyear_type");
                sqldel = "delete from lccfloanconstant where coop_id='" + state.SsCoopControl + "'";
                    Sdt del = WebUtil.QuerySdt(sqldel);
                    DwMain.DeleteRow(r);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบแถวสำเร็จ");
            }
            catch
            {
                DwMain.DeleteRow(r);
                LtServerMessage.Text = WebUtil.CompleteMessage("ลบแถวสำเร็จ");
            }
        }
        public void SaveInfo()
        {
            string sqlupdate = "", sqlinsert = "";
            string sqlcount = "", loanpermgrp_desc = "", loanpermgrp_code = "";
            Decimal formatyear_type = 0;
            int count = 0;
            int r = DwMain.RowCount;
            formatyear_type = DwMain.GetItemDecimal(r, "formatyear_type");
            sqlcount = "select count(formatyear_type) as countrow from LCCFLOANCONSTANT where coop_id ='" + state.SsCoopId + "'";
            Sdt c = WebUtil.QuerySdt(sqlcount);
            while (c.Next())
            {
                count = Convert.ToInt32(c.GetDecimal("countrow"));
            }
            //update
            for (int i = 1; i < count + 1; i++)
            {
                Decimal MAXALLLOAN_AMOUNT = DwMain.GetItemDecimal(i, "maxallloan_amount");
                Decimal DAYINYEAR = DwMain.GetItemDecimal(i, "dayinyear");
                Decimal FORMATYEAR_TYPE = DwMain.GetItemDecimal(i, "formatyear_type");
                Decimal FIXPAYCAL_TYPE = DwMain.GetItemDecimal(i, "fixpaycal_type");
                Decimal RDINTSATANG_TYPE = DwMain.GetItemDecimal(i, "rdintsatang_type");
                String RDINTSATANG_TABCODE = DwMain.GetItemString(i, "rdintsatang_tabcode");
                Decimal RDINTSATANGSUM_TYPE = DwMain.GetItemDecimal(i, "rdintsatangsum_type");
                Decimal RDINTDEC_TYPE = DwMain.GetItemDecimal(i, "rdintdec_type");
                Decimal RDINTDEC_DIGIT = DwMain.GetItemDecimal(i, "rdintdec_digit");
                Decimal INTDATEVIEW_TYPE = DwMain.GetItemDecimal(i, "intdateview_type");

                sqlupdate = @" UPDATE LCCFLOANCONSTANT SET  MAXALLLOAN_AMOUNT = '" + MAXALLLOAN_AMOUNT + "',  DAYINYEAR = '" + DAYINYEAR + "',   FORMATYEAR_TYPE = '" + FORMATYEAR_TYPE + "', FIXPAYCAL_TYPE = '" + FIXPAYCAL_TYPE + "',   RDINTSATANG_TYPE ='" + RDINTSATANG_TYPE + "',   RDINTSATANG_TABCODE = '" + RDINTSATANG_TABCODE + "',   RDINTSATANGSUM_TYPE = '" + RDINTSATANGSUM_TYPE + "',   RDINTDEC_TYPE = '" + RDINTDEC_TYPE + "',    RDINTDEC_DIGIT = '" + RDINTDEC_DIGIT + "',INTDATEVIEW_TYPE ='"+INTDATEVIEW_TYPE+"' WHERE coop_id='"+state.SsCoopControl+"' ";
                Sdt up = WebUtil.QuerySdt(sqlupdate);
            }
            //insert
            for (int i = count+1; i < r + 1; i++)
            {
                Decimal MAXALLLOAN_AMOUNT = DwMain.GetItemDecimal(i, "maxallloan_amount");
                Decimal DAYINYEAR = DwMain.GetItemDecimal(i, "dayinyear");
                Decimal FORMATYEAR_TYPE = DwMain.GetItemDecimal(i, "formatyear_type");
                Decimal FIXPAYCAL_TYPE = DwMain.GetItemDecimal(i, "fixpaycal_type");
                Decimal RDINTSATANG_TYPE = DwMain.GetItemDecimal(i, "rdintsatang_type");
                String RDINTSATANG_TABCODE = DwMain.GetItemString(i, "rdintsatang_tabcode");
                Decimal RDINTSATANGSUM_TYPE = DwMain.GetItemDecimal(i, "rdintsatangsum_type");
                Decimal RDINTDEC_TYPE = DwMain.GetItemDecimal(i, "rdintdec_type");
                Decimal RDINTDEC_DIGIT = DwMain.GetItemDecimal(i, "rdintdec_digit");
                Decimal INTDATEVIEW_TYPE = DwMain.GetItemDecimal(i, "intdateview_type");
                sqlinsert = @"  INSERT INTO LCCFLOANCONSTANT  
                ( coop_id,MAXALLLOAN_AMOUNT,DAYINYEAR,FORMATYEAR_TYPE,FIXPAYCAL_TYPE,RDINTSATANG_TYPE,RDINTSATANG_TABCODE,RDINTSATANGSUM_TYPE,RDINTDEC_TYPE,RDINTDEC_DIGIT,INTDATEVIEW_TYPE)
                VALUES ( '" + state.SsCoopControl + "','" + MAXALLLOAN_AMOUNT + "','" + DAYINYEAR + "','" + FORMATYEAR_TYPE + "','" + FIXPAYCAL_TYPE + "','" + RDINTSATANG_TYPE + "','" + RDINTSATANG_TABCODE + "','" + RDINTSATANGSUM_TYPE + "','" + RDINTDEC_TYPE + "','" + RDINTDEC_DIGIT + "','" + INTDATEVIEW_TYPE + "')";
                Sdt up = WebUtil.QuerySdt(sqlinsert);
            }
        }
    }
}