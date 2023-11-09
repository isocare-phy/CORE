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
namespace Saving.Applications.app_assist
{
    public partial class w_sheet_cm_constant_list : PageWebSheet, WebSheet
    {
        protected String jsinsertrow;
        private String pbl = "as_insconstant.pbl";

        public void InitJsPostBack()
        {
            jsinsertrow = WebUtil.JsPostBack(this, "jsinsertrow");
        }

        public void WebSheetLoadBegin()
        {
            if (IsPostBack)
            {
                this.RestoreContextDw(dw_detail);
               
            }
            else
            {
                DwUtil.RetrieveDataWindow(dw_detail, pbl, null, null);

                Hrow.Value = dw_detail.RowCount.ToString();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsinsertrow")
            {
                jsinsertrows();
            }
        }

        public void SaveWebSheet()
        {
            String sql = "select count(insitemtype_code) as insmax from INSUCFITEMPTYPE";
            DataTable dt = WebUtil.Query(sql);
            int row = Convert.ToInt32(dt.Rows[0]["insmax"]);
            int rowCount = dw_detail.RowCount;
            if (row != rowCount)
            {
                try
                {                   
                    int rowFirst = int.Parse(Hrow.Value);
                    int[] rows = new int[rowCount - rowFirst];
                    int ii = 0;
                    for (int i = rowFirst; i < rowCount; i++)
                    {
                        rows[ii] = i + 1;
                        ii++;
                    }
                    DwUtil.InsertDataWindow(dw_detail, pbl, "INSUCFITEMPTYPE", rows);
                    DwUtil.RetrieveDataWindow(dw_detail, pbl, null, null);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
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
                    DwUtil.UpdateDateWindow(dw_detail, pbl, "INSUCFITEMPTYPE");
                    DwUtil.RetrieveDataWindow(dw_detail, pbl, null, null);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            
            }
        }

        public void WebSheetLoadEnd()
        {
            dw_detail.SaveDataCache();
        }
        public void jsinsertrows()
        {
            dw_detail.InsertRow(0);
                      
        }
    }
}