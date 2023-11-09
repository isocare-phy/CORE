using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CoreSavingLibrary.WcfNCommon;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using CoreSavingLibrary.WcfNFinance;
using System.Globalization;

namespace Saving.Applications.app_finance
{
    public partial class w_dlg_closeday : PageWebSheet, WebSheet
    {
        private n_financeClient fin;
        private DwThDate tDwMain;

        #region WebSheet Members

        void WebSheet.InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain);
            tDwMain.Add("operate_date", "operate_tdate");
            tDwMain.Add("entry_date", "entry_tdate");
            tDwMain.Add("close_date", "close_tdate");
        }

        void WebSheet.WebSheetLoadBegin()
        {
            fin = wcf.NFinance;

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                Int32 gph;
                String closeday_xml = "", chqwait_xml = "";

                try
                {
                    try
                    {
                        gph = fin.of_init_close_day(state.SsWsPass, state.SsCoopId, state.SsUsername, state.SsWorkDate, state.SsApplication, ref closeday_xml, ref chqwait_xml);
                    }
                    catch(Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.WarningMessage(ex);
                        
                    }

                    DwMain.Reset();
                    DwMain.ImportString(closeday_xml, FileSaveAsType.Xml);

                    if (chqwait_xml != "")
                    {
                            //DwChqlist.ImportString(chqwait_xml, FileSaveAsType.Xml);
                            DwUtil.ImportData(chqwait_xml, DwChqlist, tDwMain, FileSaveAsType.Xml);
                    }

                    DwMain.SetItemDateTime(1, "operate_date", state.SsWorkDate);
                    tDwMain.Eng2ThaiAllRow();

                    DwMain.SetItemDateTime(1, "close_date", state.SsWorkDate);
                    tDwMain.Eng2ThaiAllRow();

                    //DataWindowChild dc = DwMain.GetChild("coopbranch_id");
                    //DwMain.SetItemString(1, "coopbranch_id", state.SsCoopId);
                    //dc.ImportString(fin.GetChildBranch(state.SsWsPass), FileSaveAsType.Xml);
                    Decimal cash_foward = DwMain.GetItemDecimal(1, "cash_foward");
                    if (cash_foward < 0)
                    {
                        throw new Exception("ยอดเงินสดคงเหลือติดลบ กรุณาตรวจสอบ");
                    }
                }
                catch (Exception ex)
                {
                    CultureInfo ThaiCulture = new CultureInfo("th-TH");
                    //LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                    LtServerMessage.Text = WebUtil.WarningMessage("ระบบได้ทำการปิดสิ้นวันที่" + "   " + state.SsWorkDate.ToString("dd/MM/yyyy", ThaiCulture) + "   " + "เรียบร้อยแล้ว");
                }
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwChqlist);
            }

            DwMain.Modify("t_entry_time.text = '" + DateTime.Now.ToString("hh:mm:ss") + "'");
            DwMain.Modify("t_coopname.text='" + state.SsCoopName + "'");

            DwMain.SetItemString(1, "entry_date", Convert.ToString(state.SsWorkDate));
            tDwMain.Eng2ThaiAllRow();
        }

        void WebSheet.CheckJsPostBack(string eventArg)
        {

        }

        void WebSheet.SaveWebSheet()
        {
            try
            {
                String XmlMain = DwMain.Describe("DataWindow.Data.XML");
                String XmlChqlist = DwChqlist.Describe("DataWindow.Data.XML");
                Decimal cash_foward = DwMain.GetItemDecimal(1, "cash_foward");
                if (cash_foward >= 0)
                {
                    int re = fin.of_close_day(state.SsWsPass, state.SsApplication, XmlMain, XmlChqlist);
                    if (re == 1)
                    {
                        try
                        {
                    //        String Sql = "UPDATE AMAPPSTATUS SET closeday_status =1 , closeday_id = '" + state.SsUsername + "' where application = 'app_finance'";
                    //        DataTable dt = WebUtil.Query(Sql);
                            LtServerMessage.Text = WebUtil.CompleteMessage("ปิดงานประจำวันเรียบร้อย");
                        }
                        catch
                        {
                    //        LtServerMessage.Text = WebUtil.WarningMessage("ระบบการเงิน ปิดสรุปวันเเล้ว เเต่ไม่สามารถ ปิดระบบที่ AppStatus กรุณาปิดที่ระบบ Admin");
                        }
                    }

                    
                }
                else
                {
                    throw new Exception("ไม่สามารถบันทึกได้เนื่องจากยอดเงินสดคงเหลือติดลบ กรุณาตรวจสอบ");
                }
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

        void WebSheet.WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwChqlist.SaveDataCache();
        }

        #endregion
    }
}
