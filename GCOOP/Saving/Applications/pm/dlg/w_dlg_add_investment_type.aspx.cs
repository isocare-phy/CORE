using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNAdmin;
using CoreSavingLibrary.WcfNPm;
using DataLibrary;

namespace Saving.Applications.pm.dlg
{
    public partial class w_dlg_add_investment_type : PageWebDialog, WebDialog
    {
        protected String JsSaveInvestType;
        protected String jsBtDel;

        public void InitJsPostBack()
        {
            JsSaveInvestType = WebUtil.JsPostBack(this, "JsSaveInvestType");
            jsBtDel = WebUtil.JsPostBack(this, "jsBtDel");
        }

        public void WebDialogLoadBegin()
        {
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
            if (eventArg == "JsSaveInvestType")
            {
                SaveInvestType();
            }
            if (eventArg == "jsBtDel")
            {
                JSBtDelType();
            }
        }

        public void WebDialogLoadEnd()
        {
            dw_main.SaveDataCache();
        }
        public void SaveInvestType()
        {
            n_pmClient svPm = wcf.NPm;
            try
            {
                for (int i = 1; i <= dw_main.RowCount; i++)
                {
                    dw_main.SetItemString(i, "coop_id", state.SsCoopId);
                }
                String xml_main = dw_main.Describe("DataWindow.Data.XML");
                int result = svPm.of_set_investtype(state.SsWsPass, xml_main);
                if (result == 1)
                {
                    HdResult.Value = "True";
                    LtServerMessage.Text = WebUtil.CompleteMessage("จัดทำข้อมูลสำเร็จ");
                    try
                    {
                        DwUtil.RetrieveDataWindow(dw_main, "pm_investment.pbl", null, null);
                        string group_id = dw_main.GetItemString(Convert.ToInt32(HdRow.Value), "investtype_code");
                        HdValue.Value = group_id;
                    }
                    catch
                    {
                        HdResult.Value = "False";
                        LtServerMessage.Text = WebUtil.WarningMessage("บันทึกสำเร็จ กรุณาเลือกแถว");
                    }
                }
                else
                {
                    HdResult.Value = "False";
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกข้อมูลไม่สำเร็จ");
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            //DwUtil.RetrieveDataWindow(dw_main, "pm_investment.pbl", null, null);
        }
        private void JSBtDelType()
        {
            n_pmClient svPm = wcf.NPm;
            int rownum = Convert.ToInt32(HdMainRowDel.Value);
            try
            {
                string coop_id = dw_main.GetItemString(rownum, "coop_id");

                try
                {
                    string investtype_code = dw_main.GetItemString(rownum, "investtype_code");
                    decimal old_new = dw_main.GetItemDecimal(rownum, "old_new");
                    if (old_new != 1)
                    {
                        string sql = "DELETE PMUCFINVEST_TYPE where investtype_code = " + investtype_code;
                        WebUtil.Query(sql);
                        dw_main.DeleteRow(rownum);
                        LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบแถวสำเร็จ");
                    }
                    else
                    {
                        String xml_main = dw_main.Describe("DataWindow.Data.XML");
                        int result = svPm.of_del_investtype(state.SsWsPass, coop_id, investtype_code);
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
            catch
            {
                dw_main.DeleteRow(rownum);
            }
            DwUtil.RetrieveDataWindow(dw_main, "pm_investment.pbl", null, null);
            HdMainRowDel.Value = "";
        }
    }
}