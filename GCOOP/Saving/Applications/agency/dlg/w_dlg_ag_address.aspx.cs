using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNAgency;
using DataLibrary;
namespace Saving.Applications.agency.dlg
{
    public partial class w_dlg_ag_address : PageWebDialog,WebDialog 
    {

       // private AgencyClient AgencyService;
        public String pbl = "egat_ag_regagmemb.pbl";
        //========================
        public String postSetDistrict;
        public String postSetpostcode;

        //=============================
        private void JspostSetpostcode()
        {
            String postcode = null;
            String district_code = Dw_main.GetItemString(1, "district_code");
            String province_code = Dw_main.GetItemString(1, "province_code");
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {

                String sql = @"select postcode from mbucfdistrict where district_code = '" + district_code + "' and province_code = '" + province_code + "'";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    postcode = dt.GetString("postcode");
                    Dw_main.SetItemString(1, "postcode", postcode.Trim());
                }
                else
                {
                    sqlca.Rollback();
                }
            }
            catch (Exception ex)
            {
                Dw_main.SetItemString(1, "postcode", "");
            }
        }

        private void JspostSetDistrict()
        {
            String province_code = Dw_main.GetItemString(1, "province_code");
            DwUtil.RetrieveDDDW(Dw_main, "district_code", pbl, null);
            DataWindowChild dc = Dw_main.GetChild("district_code");
            dc.SetFilter("province_code ='" + province_code + "'");
            dc.Filter();
        }

        private void JsNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            JspostSetDistrict();
        }
       

        #region WebDialog Members

        public void InitJsPostBack()
        {
            postSetDistrict = WebUtil.JsPostBack(this, "postSetDistrict");
            postSetpostcode = WebUtil.JsPostBack(this, "postSetpostcode");
            //==================


            //==================
        }

        public void WebDialogLoadBegin()
        {
          
            this.ConnectSQLCA();

            DwUtil.RetrieveDDDW(Dw_main, "district_code", pbl, null);
            DwUtil.RetrieveDDDW(Dw_main, "province_code", pbl, null);


            if (!IsPostBack)
            {
                String memb_addr = null;
                String mooban = null;
                String soi = null;
                String road = null;
                String tambol = null;
                String district_code = null;
                String province_code = null;
                String postcode = null;

                try { memb_addr = Request["memb_addr"].Trim(); }
                catch { memb_addr = null; }

                try { mooban = Request["mooban"].Trim(); }
                catch { mooban = null; }

                try { soi = Request["soi"].Trim(); }
                catch { soi = null; }

                try { road = Request["road"].Trim(); }
                catch { road = null; }

                try { tambol = Request["tambol"].Trim(); }
                catch { tambol = null; }

                try { district_code = Request["district_code"].Trim(); }
                catch { district_code = null; }

                try { province_code = Request["province_code"].Trim(); }
                catch { province_code = null; }

                try { postcode = Request["postcode"].Trim(); }
                catch { postcode = null; }

                JsNewClear();

                if (memb_addr == "null" || memb_addr == "")
                {
                    Dw_main.SetItemString(1, "memb_addr", "");
                }
                else
                {
                    Dw_main.SetItemString(1, "memb_addr", memb_addr);
                }

                if (mooban == "null" || mooban == "")
                {
                    Dw_main.SetItemString(1, "mooban", "");
                }
                else
                {
                    Dw_main.SetItemString(1, "mooban", mooban);
                }

                if (soi == "null" || soi == "")
                {
                    Dw_main.SetItemString(1, "soi", "");
                }
                else
                {
                    Dw_main.SetItemString(1, "soi", soi);
                }

                if (road == "null"  || road == "")
                {
                    Dw_main.SetItemString(1, "road", "");
                }
                else
                {
                    Dw_main.SetItemString(1, "road", road);
                }

                if (tambol == "null" || tambol == "")
                { Dw_main.SetItemString(1, "tambol", ""); }
                else
                {
                    // หาชื่อตำบล
                    Sta ta = new Sta(sqlca.ConnectionString);
                    try
                    {
                        String tambol_desc = null;
                        String district_full = province_code + district_code;
                        String tambol_full = district_full + tambol;
                        String sql = @"select tambol_desc from mbucftambol where district_code = '" + district_full + "' and tambol_code = '" + tambol_full + "'";
                        Sdt dt = ta.Query(sql);
                        if (dt.Next())
                        {
                            tambol_desc = dt.GetString("tambol_desc").Trim();
                            tambol = tambol_desc;
                        }
                        else
                        {
                            sqlca.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                       // LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                    }

                    Dw_main.SetItemString(1, "tambol", tambol); 
                }

                if (district_code == "null" || district_code == "")
                { Dw_main.SetItemString(1, "district_code", ""); }
                else
                {
                    String district_full = province_code + district_code;
                    Dw_main.SetItemString(1, "district_code", district_full);
                }

                if (province_code == "null" || province_code == "")
                { Dw_main.SetItemString(1, "province_code", ""); }
                else
                {
                    Dw_main.SetItemString(1, "province_code", province_code); 
                }

                if (postcode == "null" || postcode == "")
                { Dw_main.SetItemString(1, "postcode", ""); }
                else
                {
                    Dw_main.SetItemString(1, "postcode", postcode);
                }
            }
            else
            {
                this.RestoreContextDw(Dw_main);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSetDistrict")
            {
                JspostSetDistrict();
            }
            else if (eventArg == "postSetpostcode")
            {
                JspostSetpostcode();
            }
        }

        public void WebDialogLoadEnd()
        {
            if (Dw_main.RowCount > 1)
            {
                Dw_main.DeleteRow(Dw_main.RowCount);
            }
            Dw_main.SaveDataCache();
        }

        #endregion
    }
}