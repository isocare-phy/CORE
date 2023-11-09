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

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_durtgrpsubcode_ctrl
{
    public partial class w_sheet_cmd_ucf_durtgrpsubcode : PageWebSheet, WebSheet
    {
        Sdt ta = new Sdt();

        [JsPostBack]
        public String Postedit { get; set; }
        [JsPostBack]
        public String Postcode { get; set; }
        [JsPostBack]
        public String Postdel { get; set; }

        [JsPostBack]
        public String PostInsertRow { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                Hdadd_status.Value = "2";
                dsMain.Ddgroupcode();
                //dsMain.retrieve();
               // dsList.DATA[0].add_status = "2";
            //dsList.RetrieveList();

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == Postedit)
            {
                Hdadd_status.Value = "1";
                int fc_row = dsList.GetRowFocus();
                string durtgrp_code = dsList.DATA[fc_row].durtgrp_code;
                string durtgrpsub_code = dsList.DATA[fc_row].durtgrpsub_code;
                dsMain.retrieve(durtgrp_code, durtgrpsub_code);

            }
            if (eventArg == Postdel)
            {


                int row = dsList.GetRowFocus();
                string durtgrpsub_desc = dsList.DATA[row].durtgrpsub_desc;
                try
                {
                    string durtgrpsub_code = dsList.DATA[row].durtgrpsub_code;
                    string durtgrp_code = dsList.DATA[row].durtgrp_code;
                    String del = @"delete ptucfdurtgrpsubcode where durtgrpsub_code = {0} and durtgrp_code={1}";
                    del = WebUtil.SQLFormat(del, durtgrpsub_code, durtgrp_code);
                    ta = WebUtil.QuerySdt(del);

                    dsList.RetrieveList(dsMain.DATA[0].durtgrp_code);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + durtgrpsub_desc + " สำเร็จ");
                }
                catch (Exception ex)
                { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }


            }

            if (eventArg == Postcode)
            {
                dsMain.RetrieveMain(dsMain.DATA[0].durtgrp_code);
             dsList.RetrieveList(dsMain.DATA[0].durtgrp_code);
             dsMain.DATA[0].durtgrpsub_desc = "";


            }
            
        }

        public void SaveWebSheet()
        {

            ExecuteDataSource exe = new ExecuteDataSource(this);

            try
            {
                String durtgrp_code = "", durtgrpsub_code = "", durtgrpsub_desc = "", durtgrpsub_abb = "";               
                string add_status = Hdadd_status.Value;
                if (add_status == "1")
                {
                    int row_fc = dsList.GetRowFocus();
                     durtgrp_code = dsList.DATA[row_fc].durtgrp_code;
                     durtgrpsub_code = dsMain.DATA[0].durtgrpsub_code;
                     durtgrpsub_desc = dsMain.DATA[0].durtgrpsub_desc;
                     durtgrpsub_abb = dsMain.DATA[0].durtgrpsub_abb;
                    decimal devalue_percent = dsMain.DATA[0].devalue_percent;
                    string sqlEdit = @"update ptucfdurtgrpsubcode set durtgrpsub_desc = {0}, durtgrpsub_abb = {1}, 
                    devalue_percent = {2} where durtgrp_code = {3} and durtgrpsub_code = {4}";
                    sqlEdit = WebUtil.SQLFormat(sqlEdit, durtgrpsub_desc, durtgrpsub_abb, devalue_percent, durtgrp_code, durtgrpsub_code);
                    exe.SQL.Add(sqlEdit);


                    exe.Execute();
                    exe.SQL.Clear();

                    LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลรายการ  " + durtgrpsub_desc + " สำเร็จ");
                    Hdadd_status.Value = "2";
                    dsMain.DATA[0].durtgrpsub_desc = "";
                    dsMain.DATA[0].durtgrpsub_abb = "";
                    dsMain.RetrieveMain(dsMain.DATA[0].durtgrp_code);
                    //dsMain.ResetRow();
                    dsList.RetrieveList(durtgrp_code);

                }
                else if (add_status == "2")
                {

                   // string durtgrpsub_code = "";
                     durtgrpsub_desc = dsMain.DATA[0].durtgrpsub_desc;
                    durtgrp_code = dsMain.DATA[0].durtgrp_code;
                     durtgrpsub_abb = dsMain.DATA[0].durtgrpsub_abb;
                    decimal devalue_percent = dsMain.DATA[0].devalue_percent;

                    try
                    {
                        String se = @"select max(durtgrpsub_code)as durtgrpsub_code from ptucfdurtgrpsubcode where durtgrp_code = {0}";
                        se = WebUtil.SQLFormat(se, durtgrp_code);
                        ta = WebUtil.QuerySdt(se);
                        if (ta.Next())
                        {
                            durtgrpsub_code = ta.GetString("durtgrpsub_code");
                        }
                        if (durtgrpsub_code == null || durtgrpsub_code == "")
                        {
                            durtgrpsub_code = "0";
                        }
                        durtgrpsub_code = Convert.ToString(Convert.ToDecimal(durtgrpsub_code) + 1);

                        while (durtgrpsub_code.Length < 3)
                        {
                            durtgrpsub_code = "0" + durtgrpsub_code;
                        }
                    }
                    catch { }
                    try
                    {
                        String insert = @"insert into ptucfdurtgrpsubcode
                            (durtgrp_code, durtgrpsub_code, durtgrpsub_desc, devalue_percent, durtgrpsub_abb)
                            values( {0}, {1}, {2}, {3}, {4})";
                        insert = WebUtil.SQLFormat(insert, durtgrp_code, durtgrpsub_code, durtgrpsub_desc, devalue_percent, durtgrpsub_abb);
                        ta = WebUtil.QuerySdt(insert);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                        dsMain.DATA[0].durtgrpsub_desc = "";
                        //dsMain.ResetRow();
                        dsMain.DATA[0].durtgrpsub_desc = "";
                        dsMain.DATA[0].durtgrpsub_abb="";
                        dsMain.RetrieveMain(dsMain.DATA[0].durtgrp_code);
                        dsList.RetrieveList(durtgrp_code);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            
        }

        //#region ListFunction

        //private void PostSetData()
        //{
          
        //    string add_status="1";
        //}

        //#endregion



    }

}