using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;

namespace Saving.Applications.trading
{
    public partial class w_sheet_td_opr_iv_ajax : PageWebSheet, WebSheet
    {

        public void InitJsPostBack()
        {
            this.IgnoreReadable = true;


        }

        public void WebSheetLoadBegin()
        {

        }

        public void CheckJsPostBack(string eventArg)
        {

        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {

        }

        [AjaxPostBack]
        public string hello()
        {
            return "Hello Wolrd";
        }

        [AjaxPostBack]
        public string ajaxinitDetail()
        {
            string xml_string = "";
            try
            {

                string product_no = Request["product_no"];
                string store_id = Request["store_id"];

                string sql = @"SELECT tdproductmaster.COOP_ID,   
                                    tdstockmaster.store_id,
                                     tdproductmaster.PRODUCT_NO,   
                                     tdproductmaster.UNIT_CODE,   
                                    tdproductmaster.product_desc,
tdstockmaster.tax_rate,
tdstockmaster.taxtype_code
                            FROM tdproductmaster,tdstockmaster
                            where tdproductmaster.coop_id = tdstockmaster.coop_id 
                                and  tdproductmaster.product_no = tdstockmaster.product_no 


and  tdproductmaster.COOP_ID = '" + state.SsCoopId + @"'  and
                             tdstockmaster.STORE_ID = '" + store_id + @"' and
                             tdproductmaster.PRODUCT_NO = '" + product_no + "' ";
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.GetRowCount() > 0)
                {
                    xml_string = WebUtil.datatabletoXML(dt);
                }
                else
                {
                    xml_string = "";
                }
            }
            catch//Exception ex)
            {
                xml_string = "";
                //LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            return xml_string;
        }

        [AjaxPostBack]
        public string ajaxinitItemQty()
        {
            string xml = "";
            try
            {
                // int row = Convert.ToInt16(HdRowDwDetail.Value);
                string debt_no = Request["debt_no"];
                string price_level = Request["price_level"];
                string slip_tdate = Request["slip_tdate"];
                string[] arr = slip_tdate.Split('/');
                slip_tdate = arr[0] + "/" + arr[1] + "/" + Convert.ToString(Convert.ToDecimal(arr[2]) - 543);
                DateTime slip_date = Convert.ToDateTime(slip_tdate);  //GetItemDateTime(1, "slip_date");
                string product_no = Request["product_no"];
                decimal item_qty = Convert.ToDecimal(Request["item_qty"]);

                String Sql = @"select * from tdproductpricelist 
                where product_no = '" + product_no + "'" +
                " and price_lelvel = '" + price_level + "'" +
                " and (fromqty <= " + item_qty + " and toqty >= " + item_qty + ")" +
                " and trunc(effective_date) <= to_date('" + slip_date.ToString("ddMMyyyy") + "', 'ddmmyyyy')" +
                " and (trunc(expire_date) >= to_date('" + slip_date.ToString("ddMMyyyy") + "', 'ddmmyyyy') or expire_date is null)";
                Sdt dt = WebUtil.QuerySdt(Sql);
                if (dt.GetRowCount() > 0)
                {
                    xml = WebUtil.datatabletoXML(dt);
                }
                else { xml = ""; }

            }
            catch //Exception ex)
            {
                xml = "";
                //LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            return xml;
        }

        [AjaxPostBack]
        public string ajaxinitDebt_no()
        {
            string xml = "";
            try
            {
                string coop_id = Request["coop_id"];
                string debt_no = Request["debt_no"];
                string sql = @" select mbucfprename.prename_desc||tddebtmaster.debt_name||' '||mbucfprename.suffname_desc  as debtnm,   
		                    debt_contact as debt_contact,   
                            payment_type as paymentby,   
                            phone as contact_phone,   
                            fax as contact_fax,   
                            credit_term as credit_term,   
                            lead_time as deliday,   
                            price_level as price_level,   
                            debt_addr||' '||' '||district_desc||' '||province_desc  as debt_addr ,
		                    value_desc as  debt_type
                            from tddebtmaster,   
                                 mbucfprovince,   
                                 mbucfdistrict, 
                                 mbucfprename,  
		                        cmucfdropdownlist
                           WHERE ( tddebtmaster.debt_province = mbucfprovince.province_code (+)) and  
                                 ( tddebtmaster.debt_province = mbucfdistrict.province_code (+)) and  
                                 ( tddebtmaster.debt_district = mbucfdistrict.district_code (+))  and  
                                 ( tddebtmaster.prename_code = mbucfprename.prename_code (+)) and   
                                 ( tddebtmaster.debtfromtype_code = cmucfdropdownlist.value_code (+)) and   
                                 ( cmucfdropdownlist.field_code = 'CUSTTYPE') and   
                                 ( tddebtmaster.coop_id = '" + coop_id + @"') and 
                                 ( tddebtmaster.debt_no = '" + debt_no + @"') ";

                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.GetRowCount() > 0)
                {
                    xml = WebUtil.datatabletoXML(dt);
                }
                else { xml = ""; }
            }
            catch
            {
                xml = "";
            }
            return xml;
        }
    }
}