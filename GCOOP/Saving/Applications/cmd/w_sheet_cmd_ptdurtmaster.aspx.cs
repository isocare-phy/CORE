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
using System.Globalization;
using System.Data.OracleClient;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNCommon;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmd_ptdurtmaster : PageWebSheet, WebSheet
    {
        protected String postFindShow;
        protected String jsPostOnInsert;
        protected String jsPostRetrieveSubgrp;

        private DwThDate tDwMain; 
        Sdt dt = new Sdt();
        String pbl = "cmd_ptdurtmaster.pbl";

        public void InitJsPostBack()
        {
            jsPostOnInsert = WebUtil.JsPostBack(this, "jsPostOnInsert");
            postFindShow = WebUtil.JsPostBack(this, "postFindShow");
            jsPostRetrieveSubgrp = WebUtil.JsPostBack(this, "jsPostRetrieveSubgrp");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("buy_date", "buy_tdate");
            tDwMain.Add("devaluestart_date", "devaluestart_tdate");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                tDwMain.Eng2ThaiAllRow();
                DwMain.SetItemDateTime(1, "buy_date", state.SsWorkDate);
                DwMain.SetItemDateTime(1, "devaluestart_date", state.SsWorkDate);
                WebUtil.RetrieveDDDW(DwMain, "durtgrpsub_code", pbl, null);
                DwDetail.InsertRow(0);
                DwRepair.InsertRow(0);
                DwTran.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain, tDwMain);
                this.RestoreContextDw(DwDetail);
                this.RestoreContextDw(DwRepair);
                this.RestoreContextDw(DwTran);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostOnInsert":
                    PostOnInsert();
                    break;
                case "postFindShow":
                    PostShowData();
                    break;
                case "jsPostRetrieveSubgrp":
                    PostRetrieveSubgrp();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            String durt_id = "", durt_regno = "", durt_name = "", durtgrp_code = "", brand_name = "", model_desc = "", durtserial_number = "", dealer_no = "";
            String unit_code = "", lot_id = "", dept_code = "", holder_name = "", durtgrpsub_code = "";
            Decimal devalue_percent = 0,devaluelastcal_year = 0,unit_price = 0;
            DateTime devaluestart_date,buy_date ;
            Sdt ta;
            try
            {
                durt_id = DwMain.GetItemString(1, "durt_id");
                durt_regno = DwMain.GetItemString(1,"durt_regno").Trim();
                durt_name = DwMain.GetItemString(1,"durt_name").Trim();
                durtgrp_code = DwMain.GetItemString(1, "durtgrp_code").Trim();
                durtgrpsub_code = DwMain.GetItemString(1, "durtgrpsub_code").Trim();
                try { brand_name = DwMain.GetItemString(1, "brand_name").Trim(); }
                catch { brand_name = ""; }
                try { model_desc = DwMain.GetItemString(1, "model_desc").Trim(); }
                catch { model_desc = ""; }
                unit_code = DwMain.GetItemString(1,"unit_code").Trim();
                try { lot_id = DwMain.GetItemString(1, "lot_id").Trim(); }
                catch { lot_id = ""; }
                dept_code = DwMain.GetItemString(1,"dept_code").Trim();
                holder_name = DwMain.GetItemString(1,"holder_name").Trim();
                devalue_percent = DwMain.GetItemDecimal(1,"devalue_percent");
                devaluelastcal_year = DwMain.GetItemDecimal(1,"devaluelastcal_year") - 543;
                unit_price = DwMain.GetItemDecimal(1,"unit_price");
                devaluestart_date = DwMain.GetItemDateTime(1,"devaluestart_date");
                buy_date = DwMain.GetItemDateTime(1,"buy_date");
                try { durtserial_number = DwMain.GetItemString(1, "durtserial_number").Trim(); }
                catch { durtserial_number = ""; }
                try { dealer_no = DwMain.GetItemString(1, "dealer_no").Trim(); }
                catch { dealer_no = ""; }
                if (durt_id == "AUTO")
                {
                    n_commonClient com = wcf.NCommon;
                    durt_id = com.of_getnewdocno(state.SsWsPass,state.SsCoopId, "CMDPRODUCT");

                    try
                    {
                        String insert = @"insert into ptdurtmaster
                                (durt_id, durt_regno, durt_name, durtgrp_code,
                                brand_name, model_desc, unit_code, lot_id, dept_code, holder_name,
                                devalue_percent,devaluelastcal_year, unit_price, devaluestart_date,
                                buy_date, durtgrpsub_code, dealer_no, durtserial_number)values
                                ('" + durt_id + "','" + durt_regno + "','" + durt_name + "','" + durtgrp_code + @"',
                                '" + brand_name + "','" + model_desc + "','" + unit_code + @"',
                                '" + lot_id + "','" + dept_code + "','" + holder_name + @"',
                                 " + devalue_percent + "," + devaluelastcal_year + @",
                                " + unit_price + ",to_date('" + devaluestart_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'),
                                to_date('" + buy_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'), '" + durtgrpsub_code + "' ,'" + dealer_no + "','" + durtserial_number + "')";

                        ta = WebUtil.QuerySdt(insert);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }
                    catch { }
                }
                else
                {

                    try
                    {
                        //DwMain.SetItemString(1, "durt_id",durt_id);
                        String upsql = @"update ptdurtmaster
                                set durt_id ='" + durt_id + "',durt_regno ='" + durt_regno + "', durt_name='" + durt_name + "',durtgrp_code= '" + durtgrp_code + @"',
                                brand_name='" +brand_name+"',model_desc='"+model_desc+"', unit_code= '"+ unit_code +@"',
                                lot_id ='"+lot_id+"',dept_code ='"+dept_code+"',holder_name='"+holder_name+@"',
                                devalue_percent = "+devalue_percent+",devaluelastcal_year="+devaluelastcal_year+@",
                                unit_price="+unit_price+", DEVALUESTART_DATE = to_date('" + devaluestart_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'),
				                buy_date = to_date('" + buy_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'), durtgrpsub_code = '"+ durtgrpsub_code + @"',
                                dealer_no ='" + dealer_no + "',durtserial_number ='" + durtserial_number + "' " +
                                " where durt_id = '" + durt_id+"'";
                        ta = WebUtil.QuerySdt(upsql);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }
                    catch (Exception ex)
                    { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถอัพเดทข้อมูลได้ " + ex); }
                }
            }
            catch (Exception ex)
            {

                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }

        public void WebSheetLoadEnd()
        {
            if (DwMain.RowCount > 1)
            {
                DwMain.DeleteRow(DwMain.RowCount);
            }
            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
        }

        private void PostOnInsert()
        {
            DwMain.InsertRow(0);
        }

        private void jsPostFindShow()
        {
            String durt_id = "", durt_regno = "", durt_name = "", durtgrp_code = "", brand_name = "", model_desc = "", unit_code = "", lot_id = "", dept_code = "", holder_name = "", dealer_no="", durtserial_number="";
            String durtgrpsub_code = "";
            Decimal devalue_percent = 0, devaluelastcal_year = 0, unit_price = 0, durt_status = 0;
            DateTime devaluestart_date = new DateTime();
            DateTime buy_date = new DateTime();
            
            Sdt ta;
            Int32 row = 1;
            durt_id = DwMain.GetItemString(row, "durt_id").Trim();
            try
            {
                String se = @"select durt_id, durt_regno,durt_name ,durtgrp_code,brand_name,model_desc,unit_code,unit_price,durt_status,
                          buy_date,lot_id,devalue_percent, devaluestart_date,devaluelastcal_year,dept_code,holder_name, 
                          durtgrpsub_code ,dealer_no,durtserial_number
                          from ptdurtmaster where  trim(durt_id) = '" + durt_id + "'";
                ta = WebUtil.QuerySdt(se);
                if (ta.Next())
                {
                    durt_regno = ta.GetString("durt_regno").Trim();
                    durt_name = ta.GetString("durt_name").Trim();
                    durtgrp_code = ta.GetString("durtgrp_code").Trim();
                    durtgrpsub_code = ta.GetString("durtgrpsub_code").Trim();
                    brand_name = ta.GetString("brand_name").Trim();
                    model_desc = ta.GetString("model_desc").Trim();
                    unit_code = ta.GetString("unit_code").Trim();
                    lot_id = ta.GetString("lot_id").Trim();
                    dept_code = ta.GetString("dept_code").Trim();
                    holder_name = ta.GetString("holder_name").Trim();
                    devalue_percent = ta.GetDecimal("devalue_percent");
                    devaluelastcal_year = ta.GetDecimal("devaluelastcal_year");
                    unit_price = ta.GetDecimal("unit_price");
                    devaluestart_date = ta.GetDate("devaluestart_date");
                    buy_date = ta.GetDate("buy_date");
                    dealer_no = ta.GetString("dealer_no");
                    durtserial_number = ta.GetString("durtserial_number");
                    durt_status = ta.GetDecimal("durt_status");
                }
                DwMain.SetItemString(row, "durt_regno", durt_regno);
                DwMain.SetItemString(row, "durt_name", durt_name);
                DwMain.SetItemString(row, "durtgrp_code", durtgrp_code);
                DwUtil.RetrieveDDDW(DwMain, "durtgrpsub_code", pbl, durtgrp_code);
                DwMain.SetItemDecimal(row, "durt_status", durt_status);
                DwMain.SetItemString(row, "durtgrpsub_code", durtgrpsub_code);
                DwMain.SetItemString(row, "brand_name", brand_name);
                DwMain.SetItemString(row, "model_desc", model_desc);
                DwMain.SetItemString(row, "unit_code", unit_code);
                DwMain.SetItemString(row, "lot_id", lot_id);
                DwMain.SetItemString(row, "dept_code", dept_code);
                DwMain.SetItemString(row, "holder_name", holder_name);
                DwMain.SetItemDecimal(row, "devalue_percent", devalue_percent);
                DwMain.SetItemDecimal(row, "devaluelastcal_year", devaluelastcal_year + 543);
                DwMain.SetItemDecimal(row, "unit_price", unit_price);
                DwMain.SetItemDateTime(row, "devaluestart_date", devaluestart_date);
                DwMain.SetItemDateTime(1, "buy_date", buy_date);
                DwMain.SetItemString(row, "dealer_no", dealer_no);
                DwMain.SetItemString(row, "durtserial_number", durtserial_number);
            }catch{}
        }

        private void PostShowData()
        {
            string durt_id = "", durtgrp_code = "";
            durt_id = DwMain.GetItemString(1, "durt_id").Trim();
            DwUtil.RetrieveDataWindow(DwMain, pbl, null, durt_id);
            try { durtgrp_code = DwMain.GetItemString(1, "durtgrp_code"); }
            catch { durtgrp_code = PostRetrieveGrp(durt_id); }
            DwUtil.RetrieveDataWindow(DwDetail, pbl, null, durt_id);
            DwUtil.RetrieveDataWindow(DwRepair, pbl, null, durt_id);
            DwUtil.RetrieveDataWindow(DwTran, pbl, null, durt_id);
            DwUtil.RetrieveDDDW(DwMain, "durtgrpsub_code", pbl, durtgrp_code);
            DwUtil.RetrieveDDDW(DwDetail, "durtgrpsub_code", pbl, durtgrp_code);
        }

        private void PostRetrieveSubgrp()
        {
            Decimal devalue_percent = 0;
            String durtgrp_code = DwMain.GetItemString(1, "durtgrp_code").Trim();
            String se = "select devalue_percent from ptucfdurtgrpcode where durtgrp_code = '" + durtgrp_code + "'";
            dt = WebUtil.QuerySdt(se);
            if (dt.Next())
            {
                devalue_percent = dt.GetDecimal("devalue_percent");
            }
            DwMain.SetItemDecimal(1, "devalue_percent", devalue_percent);
            DwUtil.RetrieveDDDW(DwMain, "durtgrpsub_code", pbl, durtgrp_code);
        }

        private string PostRetrieveGrp(string durt_id)
        {
            String durtgrp_code = "";
            String se = "select * from ptdurtmaster where durt_id = {0}";
            se = WebUtil.SQLFormat(se, durt_id);
            dt = WebUtil.QuerySdt(se);
            if (dt.Next())
            {
                durtgrp_code = dt.GetString("durtgrp_code").Trim();
            }
            return durtgrp_code;
        }
    }
}