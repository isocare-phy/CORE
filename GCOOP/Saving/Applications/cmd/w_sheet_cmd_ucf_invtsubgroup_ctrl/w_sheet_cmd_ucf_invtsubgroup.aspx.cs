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

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_invtsubgroup_ctrl
{
    public partial class w_sheet_cmd_ucf_invtsubgroup : PageWebSheet, WebSheet
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
                string invtsubgrp_code = dsList.DATA[fc_row].invtsubgrp_code;
                string invtgrp_code = dsList.DATA[fc_row].invtgrp_code;
                dsMain.retrieve(invtgrp_code,invtsubgrp_code);
               // PostSetData();
               
            }
            if (eventArg == Postdel)
            {


                int row = dsList.GetRowFocus();
                string invtsubgrp_desc = dsList.DATA[row].invtsubgrp_desc;
                try
                {
                    string invtsubgrp_code = dsList.DATA[row].invtsubgrp_code;
                    String del = @"delete ptucfinvtsubgroupcode where invtsubgrp_code = {0}";
                    del = WebUtil.SQLFormat(del, invtsubgrp_code);
                    ta = WebUtil.QuerySdt(del);

                    dsList.RetrieveList(dsMain.DATA[0].invtgrp_code);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + invtsubgrp_desc + " สำเร็จ");
                }
                catch (Exception ex)
                { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }


            }

            if (eventArg == Postcode)
            {
               
               
                dsList.RetrieveList(dsMain.DATA[0].invtgrp_code);
                dsMain.DATA[0].invtsubgrp_desc = "";
                
               
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
                    string invtgrp_code = dsList.DATA[row_fc].invtgrp_code;
                    string invtsubgrp_desc = dsMain.DATA[0].invtsubgrp_desc;
                    string invtsubgrp_code = dsMain.DATA[0].invtsubgrp_code;
                    string sqlEdit = @"update ptucfinvtsubgroupcode set  invtsubgrp_desc={1}
where invtsubgrp_code={0} and  invtgrp_code={2}";
                    sqlEdit = WebUtil.SQLFormat(sqlEdit, invtsubgrp_code, invtsubgrp_desc, invtgrp_code);
                    exe.SQL.Add(sqlEdit);


                    exe.Execute();
                    exe.SQL.Clear();

                    LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลรายการ  " + invtsubgrp_desc + " สำเร็จ");
                    Hdadd_status.Value = "2";
                    //dsMain.ResetRow();
                    dsMain.DATA[0].invtsubgrp_desc = "";
                    dsList.RetrieveList(invtgrp_code);

                }
                else if (add_status == "2")
                {
                    
                    string invtsubgrp_code = "";
                    string invtsubgrp_desc = dsMain.DATA[0].invtsubgrp_desc;
                    string invtgrp_code = dsMain.DATA[0].invtgrp_code;
                  
                    
                    try
                    {
                        String se = @"select max(invtsubgrp_code)as invtsubgrp_code from ptucfinvtsubgroupcode
where  ptucfinvtsubgroupcode.invtgrp_code ={0}";
                        se = WebUtil.SQLFormat(se, invtgrp_code);
                        ta = WebUtil.QuerySdt(se);
                        if (ta.Next())
                        {
                            invtsubgrp_code = ta.GetString("invtsubgrp_code");
                        }
                        if (invtsubgrp_code == null || invtsubgrp_code == "")
                        {
                            invtsubgrp_code = "0";
                        }
                        invtsubgrp_code = Convert.ToString(Convert.ToDecimal(invtsubgrp_code) + 1);

                        while (invtsubgrp_code.Length < 3)
                        {
                            invtsubgrp_code = "0" + invtsubgrp_code;
                        }
                    }
                    catch { }
                    try
                    {
                        String insert = @"insert into ptucfinvtsubgroupcode (invtgrp_code,invtsubgrp_desc, invtsubgrp_code)
                                values( {0}, {1}, {2})";
                        insert = WebUtil.SQLFormat(insert, invtgrp_code, invtsubgrp_desc, invtsubgrp_code);
                        ta = WebUtil.QuerySdt(insert);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                        //dsMain.ResetRow();
                        dsMain.DATA[0].invtsubgrp_desc="";
                        dsList.RetrieveList(invtgrp_code);
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