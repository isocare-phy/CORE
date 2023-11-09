using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using Sybase.DataWindow;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_ucf_loanobjective : PageWebSheet, WebSheet
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
                    int ll_row = DwMain.InsertRow(0);
                    DwMain.SetItemString(ll_row, "coop_id", state.SsCoopControl);
                    break;

            }
        }

        public void SaveWebSheet()
        {
            try
            {
                SaveInfo();
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, null);
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
            string sqldel = "", LOANOBJECTIVE_CODE="";
            try
            {
                LOANOBJECTIVE_CODE = DwMain.GetItemString(r, "LOANOBJECTIVE_CODE");
                sqldel = "delete from LCUCFLOANOBJECTIVE where coop_id ='" + state.SsCoopId + "' and LOANOBJECTIVE_CODE='" + LOANOBJECTIVE_CODE + "'";
                Sdt del = WebUtil.QuerySdt(sqldel);
                DwMain.DeleteRow(r);
                LtServerMessage.Text=WebUtil.CompleteMessage("ลบแถวสำเร็จ");
            }
            catch(Exception ex) {LtServerMessage.Text=WebUtil.ErrorMessage(ex); }
            
        }
        public void SaveInfo()
        {
            string sqlupdate = "", sqlinsert = "";
            string sqlcount = "", loanobjective_desc = "", loanobjective_code="";
            int count = 0;
            int r = DwMain.RowCount;
            sqlcount = "select count(loanobjective_code) as countrow from LCUCFLOANOBJECTIVE where coop_id ='" + state.SsCoopId + "'";
            Sdt c = WebUtil.QuerySdt(sqlcount);
            if (c.Next())
            {
                count = Convert.ToInt32(c.GetDecimal("countrow"));
            }
            //update
            for (int i = 1; i < count + 1; i++)
            {
                loanobjective_code = DwMain.GetItemString(i, "loanobjective_code");
                loanobjective_desc = DwMain.GetItemString(i, "loanobjective_desc");
                sqlupdate = "update LCUCFLOANOBJECTIVE set loanobjective_desc='" + loanobjective_desc + "' where loanobjective_code='" + loanobjective_code + "' and coop_id ='" + state.SsCoopControl + "'";
                Sdt up = WebUtil.QuerySdt(sqlupdate);
            }
            //insert
            for (int i = count+1; i < r + 1; i++)
            {
                loanobjective_code = DwMain.GetItemString(i, "loanobjective_code");
                loanobjective_desc = DwMain.GetItemString(i, "loanobjective_desc");
                sqlinsert = @"  INSERT INTO LCUCFLOANOBJECTIVE  
                ( coop_id,LOANOBJECTIVE_CODE,LOANOBJECTIVE_DESC )  
                VALUES ( '"+state.SsCoopControl+"','"+loanobjective_code+"','"+loanobjective_desc+"')";
                Sdt up = WebUtil.QuerySdt(sqlinsert);
            }
        }
    }
}