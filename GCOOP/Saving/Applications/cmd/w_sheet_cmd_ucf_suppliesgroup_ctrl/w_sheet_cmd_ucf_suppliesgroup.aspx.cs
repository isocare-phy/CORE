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

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_suppliesgroup_ctrl
{
    public partial class w_sheet_cmd_ucf_suppliesgroup : PageWebSheet, WebSheet
    {
        Sdt ta = new Sdt();

        [JsPostBack]
        public String Postdel { get; set; }
        [JsPostBack]
        public String Postedit { get; set; }

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
            dsList.RetrieveList();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == Postdel)
            {

                //String InvtgrpCode = "";
                int row = dsList.GetRowFocus();
                string invtgrp_desc = dsList.DATA[row].invtgrp_desc;
                try
                {
                    string invtgrp_code = dsList.DATA[row].invtgrp_code;
                    String del = @"delete ptucfinvtgroupcode where invtgrp_code = {0}";
                    del = WebUtil.SQLFormat(del, invtgrp_code);
                    ta = WebUtil.QuerySdt(del);

                    dsList.RetrieveList();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + invtgrp_desc + " สำเร็จ");
                }
                catch (Exception ex)
                { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }


            }

            if (eventArg == Postedit)
            {
                Hdadd_status.Value = "1";
                int fc_row = dsList.GetRowFocus();
                string invtgrp_code = dsList.DATA[fc_row].invtgrp_code;
                dsMain.retrieve(invtgrp_code);
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
                    string invtgrp_code = dsList.DATA[row_fc].invtgrp_code;
                    string invtgrp_desc = dsMain.DATA[0].invtgrp_desc;

                    string sqlEdit = @"update ptucfinvtgroupcode set  invtgrp_desc={1}
                                    where  invtgrp_code={0}";
                    sqlEdit = WebUtil.SQLFormat(sqlEdit, invtgrp_code, invtgrp_desc);
                    exe.SQL.Add(sqlEdit);


                    exe.Execute();
                    exe.SQL.Clear();

                    LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลหมวดพัสดุ  " + invtgrp_desc + " สำเร็จ");
                    Hdadd_status.Value = "2";
                    dsMain.ResetRow();
                    dsList.RetrieveList();

                }
                else if (add_status == "2")
                {
                    string invtgrp_code = "";
                    string invtgrp_desc = dsMain.DATA[0].invtgrp_desc;
                    

                    try
                    {
                        String se = @"select max(invtgrp_code)as invtgrp_code from ptucfinvtgroupcode";
                        ta = WebUtil.QuerySdt(se);
                        if (ta.Next())
                        {
                            invtgrp_code = ta.GetString("invtgrp_code");
                        }
                        if (invtgrp_code == null || invtgrp_code == "")
                        {
                            invtgrp_code = "0";
                        }
                        invtgrp_code = Convert.ToString(Convert.ToDecimal(invtgrp_code) + 1);

                        while (invtgrp_code.Length < 3)
                        {
                            invtgrp_code = "0" + invtgrp_code;
                        }
                    }
                    catch { }
                    try
                    {
                        String insert = @"insert into ptucfinvtgroupcode (invtgrp_code,invtgrp_desc)
                                values( {0}, {1})";
                        insert = WebUtil.SQLFormat(insert, invtgrp_code, invtgrp_desc);
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
            //dsMain.Save


            //    DwMain.SaveDataCache();
            //DwDetail.SaveDataCache();
           
        }
        //private void PostOnDelete()
        //{
        //    String InvtgrpCode = "";
        //    int row =dsList.GetRowFocus();
        //    //Int32 row = Convert.ToInt32(HdR.Value);
        //    try
        //    {
        //        string invtgrp_code = dsList.DATA[row].invtgrp_code;
        //       // InvtgrpCode = dsList.SetItem(row, "invtgrp_code");
        //        String del = @"delete ptucfinvtgroupcode where invtgrp_code = '" + InvtgrpCode + "'";
        //        ta = WebUtil.QuerySdt(del);
        //        dsList.RetrieveList();
        //        LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + InvtgrpCode + " สำเร็จ");
        //    }
        //    catch (Exception ex)
        //    { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        //}
    }
}