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
    public partial class w_dlg_add_investment_group : PageWebDialog, WebDialog
    {
        protected String JsSaveInvestGroup;
        protected String jsBtDel;

        public void InitJsPostBack()
        {
            JsSaveInvestGroup = WebUtil.JsPostBack(this, "JsSaveInvestGroup");
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
            if (eventArg == "JsSaveInvestGroup")
            {
                SaveInvestGroup();
            }
            if (eventArg == "jsBtDel")
            {
                JSBtDelInv();
            }
        }

        public void WebDialogLoadEnd()
        {
            dw_main.SaveDataCache();
        }
        public void SaveInvestGroup()
        {
            n_pmClient svPm = wcf.NPm;
            try
            {
                for (int i = 1; i <= dw_main.RowCount; i++)
                {
                    dw_main.SetItemString(i, "coop_id", state.SsCoopId);
                }
                String xml_main = dw_main.Describe("DataWindow.Data.XML");
                int result = svPm.of_set_investgroup(state.SsWsPass, xml_main);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกข้อมูลไม่สำเร็จ");
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            DwUtil.RetrieveDataWindow(dw_main, "pm_investment.pbl", null, null);
        }
        private void JSBtDelInv()
        {
            n_pmClient svPm = wcf.NPm;
            int rownum = Convert.ToInt32(HdMainRowDel.Value);
            try
            {
                string coop_id = dw_main.GetItemString(rownum, "coop_id");

                try
                {
                    string group_code = dw_main.GetItemString(rownum, "group_code");
                    decimal old_new = dw_main.GetItemDecimal(rownum, "old_new");
                    if (old_new != 1)
                    {
                        string sql = "DELETE PMUCFINVEST_GROUP where group_code = " + group_code;
                        WebUtil.Query(sql);
                        dw_main.DeleteRow(rownum);
                        LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบแถวสำเร็จ");
                    }
                    else
                    {
                        String xml_main = dw_main.Describe("DataWindow.Data.XML");
                        int result = svPm.of_del_investgroup(state.SsWsPass, coop_id, group_code);
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