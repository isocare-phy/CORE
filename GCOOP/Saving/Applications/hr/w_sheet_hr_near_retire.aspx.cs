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
using DataLibrary;

namespace Saving.Applications.hr
{
    public partial class w_sheet_hr_near_retire : PageWebSheet , WebSheet 
    {

//        WebState state;
        private DwThDate tdwDetail;
        DwTrans SQLCA;
        //====================
        protected String postSearch;
        protected String postNewClear;
        //====================
        
        private void JspostNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            Dw_detail.Reset();
            lbl_rowcount.Text = "0";
        }
        private void JspostSearch()
        {
            String near_year = null;
            String ls_sql = "", ls_sqlext = "", ls_temp = "";

            ls_sql = Dw_detail.GetSqlSelect();
            try
            {
                near_year = Hd_year.Value;
            }
            catch { near_year = null; }

            if (near_year == null)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกจำนวนปีที่ใกล้เกษียณอายุก่อน");
            }
            else
            {
                if (near_year == "0")
                {
                    ls_sqlext += "and months_between(sysdate,emplbirtdate) > 708 and emplstat <> "+'8'+""; 
                }
                else if (near_year == "1")
                {
                    ls_sqlext += "and months_between(sysdate,emplbirtdate) = 708 and emplstat <> " + '8' + "";                   
                }
                else if (near_year == "2")
                {
                    ls_sqlext += "and months_between(sysdate,emplbirtdate) >= 696 and months_between(sysdate,emplbirtdate) <=707 and emplstat <> " + '8' + "";
                }
                else if (near_year == "3")
                {
                    ls_sqlext += "and months_between(sysdate,emplbirtdate) >= 684 and months_between(sysdate,emplbirtdate) <=695 and emplstat <> " + '8' + "";
                }
                else if (near_year == "4")
                {
                    ls_sqlext += "and months_between(sysdate,emplbirtdate) >= 672 and months_between(sysdate,emplbirtdate) <=683 and emplstat <> " + '8' + "";
                }
                else if (near_year == "5")
                {
                    ls_sqlext += "and months_between(sysdate,emplbirtdate) >= 660 and months_between(sysdate,emplbirtdate) <= 671 and emplstat <> " + '8' + "";
                }

                ls_temp = ls_sql + ls_sqlext;
                hidden_search.Value = ls_temp;
                Dw_detail.SetSqlSelect(hidden_search.Value);
                Dw_detail.Retrieve();
                lbl_rowcount.Text = Convert.ToString(Dw_detail.RowCount);

                tdwDetail.Eng2ThaiAllRow();
                Dw_main.SetItemString(1, "ai_year", Hd_year.Value);

                if (Dw_detail.RowCount < 1)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลเจ้าหน้าที่ใกล้เกษียณ");
                    lbl_rowcount.Text = "0";
                }
            }
           
           
        }
        #region WebSheet Members

        public void InitJsPostBack()
        {
            postSearch = WebUtil.JsPostBack(this, "postSearch");
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");

            tdwDetail = new DwThDate(Dw_detail, this);
            tdwDetail.Add("emplbirtdate", "empbirth_tdate");
            tdwDetail.Add("emplprobdate", "emplprob_tdate");
        }

        public void WebSheetLoadBegin()
        {
            state = new WebState(Session, Request);
            SQLCA = new DwTrans();
            SQLCA.Connect();
            Dw_main.SetTransaction(SQLCA);
            Dw_detail.SetTransaction(SQLCA);


            if (!IsPostBack)
            {
                Dw_main.InsertRow(0);
            }

            if (!hidden_search.Value.Equals(""))
            {

                Dw_detail.Retrieve();
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSearch")
            {
                JspostSearch();
            }
            else if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            Dw_main.SaveDataCache();
            Dw_detail.SaveDataCache();
        }

        #endregion

        
    }
}
