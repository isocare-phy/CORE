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
using CoreSavingLibrary.WcfNCommon;
using Sybase.DataWindow;
using DataLibrary;
using System.Web.Services.Protocols;
using CoreSavingLibrary.WcfNFinance;

namespace Saving.Applications.app_finance
{
    public partial class w_sheet_cancelslip : PageWebSheet, WebSheet
    {
        protected n_financeClient fin;
        protected String postRetreiveSlip;
        protected String postProtect;
        private DwThDate tDwHead;


        #region WebSheet Members

        public void InitJsPostBack()
        {
            postRetreiveSlip = WebUtil.JsPostBack(this, "postRetreiveSlip");
            postProtect = WebUtil.JsPostBack(this, "postProtect");
            tDwHead = new DwThDate(DwHead);
            tDwHead.Add("adtm_date", "adtm_tdate");
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                fin = wcf.NFinance;

                if (!IsPostBack)
                {
                    DwHead.Reset();
                    DwList.Reset();

                    DwHead.InsertRow(0);
                    DwHead.SetItemDateTime(1, "adtm_date", state.SsWorkDate);
                    tDwHead.Eng2ThaiAllRow();

                    DwUtil.RetrieveDDDW(DwHead, "cash_type", "cancelslip.pbl", null);
                    DwHead.Modify("cash_type.protect=1");
                    RetreiveSlip();
                }
                else
                {
                    this.RestoreContextDw(DwHead);
                    this.RestoreContextDw(DwList);
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

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetreiveSlip")
            {
                RetreiveSlip();
            }
            else if (eventArg == "postProtect")
            {
                Protect();
            }
        }
        
        public void SaveWebSheet()
        {
            try
            {
                int re;
                String mainXml = "", listXml = "";

                mainXml = DwHead.Describe("Datawindow.Data.Xml");
                listXml = DwList.Describe("Datawindow.Data.Xml");

                re = fin.of_postcancelslip(state.SsWsPass, state.SsCoopId, state.SsUsername, mainXml, listXml);

                if (re > 0)
                {
                    RetreiveSlip();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
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

        public void WebSheetLoadEnd()
        {
            DwHead.SaveDataCache();
            DwList.SaveDataCache();
        }

        #endregion


        protected void Protect()
        {
            Decimal fixCashType = DwHead.GetItemDecimal(1, "fixcash_type");
            if (fixCashType == 1)
            {
                DwHead.Modify("cash_type.protect=0");
            }
            else if (fixCashType == 0)
            {
                DwHead.Modify("cash_type.protect=1");
            }
        }

        protected void RetreiveSlip()
        {
            Int32 result;
            String mainXml = "", listXml = "";
            mainXml = DwHead.Describe("Datawindow.Data.Xml");

            try
            {
                result = fin.of_retrieve_cancleslip(state.SsWsPass, state.SsCoopId, mainXml, ref listXml);                
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
                listXml = "";
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                listXml = "";
            }

            if (listXml == "")
            {
                DwList.Reset();
            }
            else
            {
                DwList.Reset();
                //DwList.ImportString(listXml, FileSaveAsType.Xml);
                DwUtil.ImportData(listXml, DwList, null, FileSaveAsType.Xml);
                DwList.Sort();
            }
        }
    }
}
