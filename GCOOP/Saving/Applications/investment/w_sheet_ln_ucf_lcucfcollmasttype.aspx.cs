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
    public partial class w_sheet_ln_ucf_lcucfcollmasttype : PageWebSheet, WebSheet
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
            string sqldel = "", collmasttype_code = "";
            
            try
            {
                collmasttype_code = DwMain.GetItemString(r, "collmasttype_code");
                if (collmasttype_code != "" && collmasttype_code != null)
                {
                    sqldel = "delete from LCUCFCOLLMASTTYPE where collmasttype_code='" + collmasttype_code + "'";
                    Sdt del = WebUtil.QuerySdt(sqldel);
                    DwMain.DeleteRow(r);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบแถวสำเร็จ");
                }
                else {
                    DwMain.DeleteRow(r);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบแถวสำเร็จ");
                }
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
            string sqlcount = "", collmasttype_desc = "", collmasttype_code = "";
            int count = 0;
            int r = DwMain.RowCount;
            collmasttype_code = DwMain.GetItemString(r, "collmasttype_code");
            sqlcount = "select count(collmasttype_code) as countrow from LCUCFCOLLMASTTYPE";
            Sdt c = WebUtil.QuerySdt(sqlcount);
            while (c.Next())
            {
                count = Convert.ToInt32(c.GetDecimal("countrow"));
            }
            //update
            for (int i = 1; i < count + 1; i++)
            {
                collmasttype_desc = DwMain.GetItemString(i, "collmasttype_desc");
                sqlupdate = "update LCUCFCOLLMASTTYPE set collmasttype_desc='" + collmasttype_desc + "' where collmasttype_code ='" + collmasttype_code + "'";
                Sdt up = WebUtil.QuerySdt(sqlupdate);
            }
            //insert
            for (int i = count+1; i < r + 1; i++)
            {
                collmasttype_code = DwMain.GetItemString(i, "collmasttype_code");
                collmasttype_desc = DwMain.GetItemString(i, "collmasttype_desc");
                sqlinsert = @"  INSERT INTO LCUCFCOLLMASTTYPE  
                ( collmasttype_code,collmasttype_desc )  
                VALUES ( '" + collmasttype_code + "','" + collmasttype_desc + "')";
                Sdt up = WebUtil.QuerySdt(sqlinsert);
            }
        }
    }
}