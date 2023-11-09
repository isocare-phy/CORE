using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;
using System.Globalization;

namespace Saving.Applications.cmd.ws_gen_ptdurtmasttemp_ctrl
{
    public partial class ws_gen_ptdurtmasttemp : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public String CkPostPtmast { get; set; }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "CkPostPtmast")
            {
                PostPtmast();
            }
        }

        public void InitJsPostBack()
        {
            
        }

        public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadBegin()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            
        }

        private void PostPtmast()
        {
            decimal durtitem_amt = 0, devalue_percent = 0, unit_price = 0, devaluebal_amt = 0, devl_amt = 0;
            string durt_id = "", durt_regno = "", durt_name = "", durtgrp_code = "", durtgrpsub_code = "";
            string model_desc = "", unit_code = "", dept_code = "", holder_name = "", branch_code = "";
            DateTime devaluestart_date, buy_date;
            try
            {
                int j = 0;
                string sqlsetemp = @"select * from ptdurtcaldevaluetemp order by convert(durt_id,UNSIGNED INTEGER)";
                Sdt ta = WebUtil.QuerySdt(sqlsetemp);
                while (ta.Next())
                {
                    j++;
                    durt_id = Convert.ToString(j);
                    durt_regno = "";
                    durt_name = ta.GetString("durt_name");
                    durtgrp_code = "000";
                    durtgrpsub_code = "000";
                    model_desc = "";
                    unit_code = "000";
                    dept_code = "000";
                    buy_date = ta.GetDate("buy_date");
                    devaluestart_date = new DateTime(2017, 12, 31);
                    durtitem_amt = ta.GetDecimal("durtitem_amt");
                    devalue_percent = ta.GetDecimal("devl_percent");
                    unit_price = ta.GetDecimal("unit_price");
                    devaluebal_amt = ta.GetDecimal("devl_bfamt");
                    devl_amt = ta.GetDecimal("devl_amt");
                    if (unit_price == 0)
                    {
                        unit_price = devl_amt;
                    }
                    if (durtitem_amt > 1)
                    {
                        decimal tempdevaluebal_amt = 0, tempdurtitem_amt = 0, tempunitdeval_amt = 0;
                        tempunitdeval_amt = devaluebal_amt;
                        tempdurtitem_amt = durtitem_amt;
                        for (int i = 0; i < durtitem_amt; i++)
                        {
                            tempdevaluebal_amt = Math.Ceiling(tempunitdeval_amt / tempdurtitem_amt);
                            tempunitdeval_amt -= tempdevaluebal_amt;
                            tempdurtitem_amt--;

                            string sqlin = @"insert into ptdurtmaster 
                            (durt_id , durt_regno , durt_name , durtgrp_code , model_desc , unit_code , 
                                devalue_percent , devaluestart_date , devaluelastcal_year , lot_id , dept_code , 
                                holder_name , unit_price , buy_date , durt_status , devaluebal_amt , durtgrpsub_code, 
                                branch_code , entry_id , brand_name ) values
                            ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19})";
                            sqlin = WebUtil.SQLFormat(sqlin, durt_id, durt_regno, durt_name, durtgrp_code, model_desc, unit_code,
                                devalue_percent, devaluestart_date, "2017", "CNV", dept_code, holder_name, unit_price, buy_date, 1, tempdevaluebal_amt,
                                durtgrpsub_code, "001", "CNV", "");
                            Sdt dt = WebUtil.QuerySdt(sqlin);
                            j++;
                            durt_id = Convert.ToString(j);
                        }
                    }
                    else
                    {
                        string sqlin = @"insert into ptdurtmaster 
                            (durt_id , durt_regno , durt_name , durtgrp_code , model_desc , unit_code , 
                                devalue_percent , devaluestart_date , devaluelastcal_year , lot_id , dept_code , 
                                holder_name , unit_price , buy_date , durt_status , devaluebal_amt , durtgrpsub_code, 
                                branch_code , entry_id , brand_name ) values
                            ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19})";
                        sqlin = WebUtil.SQLFormat(sqlin, durt_id, durt_regno, durt_name, durtgrp_code, model_desc, unit_code,
                            devalue_percent, devaluestart_date, "2017", "CNV", dept_code, holder_name, unit_price, buy_date, 1, devaluebal_amt,
                            durtgrpsub_code, "001", "CNV", "");
                        Sdt dt = WebUtil.QuerySdt(sqlin);
                    }
                }
            }
            catch (Exception ex) 
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

    }
}