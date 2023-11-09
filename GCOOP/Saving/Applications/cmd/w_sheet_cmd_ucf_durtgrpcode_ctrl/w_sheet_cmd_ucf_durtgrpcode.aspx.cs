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

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_durtgrpcode_ctrl
{
    public partial class w_sheet_cmd_ucf_durtgrpcode : PageWebSheet, WebSheet
    {
        Sdt ta = new Sdt();

        [JsPostBack]
        public String Postedit { get; set; }
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
                //dsMain.retrieve();
               // dsList.DATA[0].add_status = "2";
            dsList.RetrieveList();

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == Postdel)
            {

                
                int row = dsList.GetRowFocus();
                string durtgrp_desc = dsList.DATA[row].durtgrp_desc;
                try
                {
                    string durtgrp_code = dsList.DATA[row].durtgrp_code;
                    String del = @"delete ptucfdurtgrpcode where durtgrp_code = {0}";
                    del = WebUtil.SQLFormat(del, durtgrp_code);
                    ta = WebUtil.QuerySdt(del);

                    dsList.RetrieveList();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + durtgrp_desc + " สำเร็จ");
                }
                catch (Exception ex)
                { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }


            }
            if (eventArg == Postedit)
            {
                Hdadd_status.Value = "1";     
                int fc_row = dsList.GetRowFocus();
                string durtgrp_code = dsList.DATA[fc_row].durtgrp_code;
                dsMain.retrieve(durtgrp_code);
               // PostSetData();
               
            }
            
        }

        public void SaveWebSheet()
        {
            
                ExecuteDataSource exe = new ExecuteDataSource(this);
               
            try
            {
                string add_status = Hdadd_status.Value;
                if (add_status == "1")
                {
                    int row_fc = dsList.GetRowFocus();
                    string durtgrp_code = dsList.DATA[row_fc].durtgrp_code;
                    string durtgrp_desc = dsMain.DATA[0].durtgrp_desc;
                    decimal devalue_percent = dsMain.DATA[0].devalue_percent;
                    string durtgrp_abb = dsMain.DATA[0].durtgrp_abb;
                    string sqlEdit = @"update ptucfdurtgrpcode set  durtgrp_desc={1},devalue_percent={2},durtgrp_abb={3}
                                    where  durtgrp_code={0}";
                    sqlEdit = WebUtil.SQLFormat(sqlEdit, durtgrp_code, durtgrp_desc, devalue_percent, durtgrp_abb);
                    exe.SQL.Add(sqlEdit);


                    exe.Execute();
                    exe.SQL.Clear();

                    LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลรายการ  " + durtgrp_desc + " สำเร็จ");
                    Hdadd_status.Value = "2";
                    dsMain.ResetRow();
                    dsList.RetrieveList();

                }
                else if (add_status == "2")
                {
                    string durtgrp_code = "";
                    string durtgrp_desc = dsMain.DATA[0].durtgrp_desc;
                    decimal devalue_percent = dsMain.DATA[0].devalue_percent;
                    string durtgrp_abb = dsMain.DATA[0].durtgrp_abb;
                    
                    try
                    {
                        String se = @"select max(durtgrp_code)as durtgrp_code from ptucfdurtgrpcode";
                        ta = WebUtil.QuerySdt(se);
                        if (ta.Next())
                        {
                            durtgrp_code = ta.GetString("durtgrp_code");
                        }
                        if (durtgrp_code == null || durtgrp_code == "")
                        {
                            durtgrp_code = "0";
                        }
                        durtgrp_code = Convert.ToString(Convert.ToDecimal(durtgrp_code) + 1);

                        while (durtgrp_code.Length < 3)
                        {
                            durtgrp_code = "0" + durtgrp_code;
                        }
                    }
                    catch { }
                    try
                    {
                        String insert = @"insert into ptucfdurtgrpcode (durtgrp_code,durtgrp_desc, devalue_percent,durtgrp_abb)
                                values( {0}, {1}, {2},{3})";
                        insert = WebUtil.SQLFormat(insert, durtgrp_code, durtgrp_desc, devalue_percent, durtgrp_abb);
                        ta = WebUtil.QuerySdt(insert);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                        dsMain.ResetRow();
                        dsList.RetrieveList();
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