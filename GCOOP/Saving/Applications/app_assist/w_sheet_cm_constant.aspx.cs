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
using DBAccess;
namespace Saving.Applications.app_assist
{

    public partial class w_sheet_cm_constant : PageWebSheet, WebSheet
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
                this.RestoreContextDw(dw_status);               
               
            }
            else
            {
                DwUtil.RetrieveDataWindow(dw_status, "as_insconstant.pbl", null, null);             
                
                HdStatusRow.Value = dw_status.RowCount.ToString();
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
            String sql = "select count(insmemb_type) as insmax from INSUCFMEMBTYPE";
            DataTable dt = WebUtil.Query(sql);
            int rowdb = Convert.ToInt32(dt.Rows[0]["insmax"]);
            int rowCount = dw_status.RowCount;
            if (rowdb != rowCount)
            {
                try
                {
                    int rowFirst = int.Parse(HdStatusRow.Value);
                    int[] rows = new int[rowCount - rowFirst];
                    int ii = 0;
                    for (int i = rowFirst; i < rowCount; i++)
                    {
                        rows[ii] = i + 1;
                        ii++;
                    }
                    DwUtil.InsertDataWindow(dw_status, pbl, "INSUCFMEMBTYPE", rows);
                    DwUtil.RetrieveDataWindow(dw_status, pbl, null, null);
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
                    DwUtil.UpdateDateWindow(dw_status, pbl, "INSUCFMEMBTYPE");
                    DwUtil.RetrieveDataWindow(dw_status, pbl, null, null);
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
            dw_status.SaveDataCache();
            
           
        }

        public void jsinsertrows()
        {
            
            dw_status.InsertRow(0);
            String sql = "select max(insmemb_type) as insmax from INSUCFMEMBTYPE";
            DataTable dt = WebUtil.Query(sql);            
            int row = Convert.ToInt32(dt.Rows[0]["insmax"]) + 1;
            int rowNow = dw_status.RowCount;
            dw_status.SetItemDecimal(rowNow, "insmemb_type", row);
        }

       

    }
}