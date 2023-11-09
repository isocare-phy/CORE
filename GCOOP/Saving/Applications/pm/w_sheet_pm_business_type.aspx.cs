using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNPm;
using DataLibrary;

namespace Saving.Applications.pm
{
    public partial class w_sheet_pm_business_type : PageWebSheet, WebSheet
    {
        protected String jsBtDel;
        public void InitJsPostBack()
        {
            jsBtDel = WebUtil.JsPostBack(this, "jsBtDel");
        }

        public void WebSheetLoadBegin()
        {
            //dw_main.InsertRow(0);
            if (!IsPostBack)
            {
              
                DwUtil.RetrieveDataWindow(dw_main, "pm_investment.pbl", null, null);
            }
            else
            {
                this.RestoreContextDw(dw_main);
            }
        }
        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsBtDel":
                    JSBtDel();
                    break;
            }
        }
        private void JSBtDel()
        {
            n_pmClient svPm = wcf.NPm;
            int rownum = Convert.ToInt32(HdMainRowDel.Value);
            try
            {
                string coop_id = dw_main.GetItemString(rownum, "coop_id");

                try
                {
                    string industry_code = dw_main.GetItemString(rownum, "industry_code");
                    decimal old_new = dw_main.GetItemDecimal(rownum, "old_new");
                    if (old_new != 1)
                    {
                        string sql = "DELETE PMUCFINDUSTRY_GROUP where industry_code = " + industry_code;
                        WebUtil.Query(sql);
                        dw_main.DeleteRow(rownum);
                        LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบแถวสำเร็จ");
                    }
                    else
                    {

                        String xml_main = dw_main.Describe("DataWindow.Data.XML");
                        int result = svPm.of_del_industry(state.SsWsPass, coop_id, industry_code);
                        if (result == 1)
                        {
                            LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบสำเร็จ");
                        }
                        else
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ทำการลบไม่สำเร็จ");
                        }
                    }

                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            }
            catch {
                dw_main.DeleteRow(rownum);
             }
            DwUtil.RetrieveDataWindow(dw_main, "pm_investment.pbl", null, null);
           
            HdMainRowDel.Value = "";
           
        }
        public void SaveWebSheet()
        {
            n_pmClient svPm = wcf.NPm;
            try
            {
                for (int i = 1; i <= dw_main.RowCount; i++)
                {
                    dw_main.SetItemString(i, "coop_id", state.SsCoopId);
                }
                String xml_main = dw_main.Describe("DataWindow.Data.XML");
                int result = svPm.of_set_industry(state.SsWsPass, xml_main);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("จัดทำข้อมูลสำเร็จ");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("จัดทำข้อมูลไม่สำเร็จ");
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            DwUtil.RetrieveDataWindow(dw_main, "pm_investment.pbl", null, null);

        }

        public void WebSheetLoadEnd()
        {
            
            dw_main.SaveDataCache();
        }
    }
}