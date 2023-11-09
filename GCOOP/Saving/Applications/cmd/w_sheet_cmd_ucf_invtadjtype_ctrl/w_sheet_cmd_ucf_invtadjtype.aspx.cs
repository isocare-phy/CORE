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

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_invtadjtype_ctrl
{
    public partial class w_sheet_cmd_ucf_invtadjtype : PageWebSheet, WebSheet
    {
        Sdt ta = new Sdt();

        [JsPostBack]
        public String Postdel { get; set; }
        [JsPostBack]
        public String Postdelete { get; set; }
        [JsPostBack]
        public String Postedit { get; set; }
        [JsPostBack]
        public String Posteditlist { get; set; }

        [JsPostBack]
        public String PostInsertRow { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
            dsList2.InitDsList2(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
         //dsMain.retrieve();
   
                //dsMain.DATA[0].sign_flag = "1";
                Hdadd_status.Value = "2";
            dsList.RetrieveList();
            dsList2.RetrieveList2();
            
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == Postedit)
            {
                Hdadd_status.Value = "1";
                int fc_row = dsList.GetRowFocus();
                string adjtype_code = dsList.DATA[fc_row].adjtype_code;
                dsMain.retrieve(adjtype_code);
                // PostSetData();

            }
            if (eventArg == Posteditlist)
            {
                Hdadd_status.Value = "3";
                int fc_row = dsList2.GetRowFocus();
                string adjtype_code = dsList2.DATA[fc_row].adjtype_code;
                dsMain.retrieve(adjtype_code);
                // PostSetData();

            }
            if (eventArg == Postdel)
            {

                // String branch_code = "";
               
                int row = dsList.GetRowFocus();
               
                string adjtype_desc = dsList.DATA[row].adjtype_desc;
                string sign_flag = dsList.DATA[row].sign_flag;
                try
                {
                    string adjtypedesc = dsList.DATA[row].adjtype_desc;
                    string adjtype_code = dsList.DATA[row].adjtype_code;
                    String del = @"delete ptucfinvtadjtype where adjtype_code = {0}";
                    del = WebUtil.SQLFormat(del, adjtype_code);
                    ta = WebUtil.QuerySdt(del);

                    dsList.RetrieveList();
                    dsList2.RetrieveList2();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + adjtypedesc + " สำเร็จ");
                }
                catch (Exception ex)
                { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            }

            if (eventArg == Postdelete)
            {

                // String branch_code = "";
                
                int row = dsList2.GetRowFocus();

                string adjtype_desc = dsList2.DATA[row].adjtype_desc;
                string sign_flag = dsList2.DATA[row].sign_flag;
                try
                {
                    string adjtypedesc = dsList2.DATA[row].adjtype_desc;
                    string adjtype_code = dsList2.DATA[row].adjtype_code;
                    String del = @"delete ptucfinvtadjtype where adjtype_code = {0}";
                    del = WebUtil.SQLFormat(del, adjtype_code);
                    ta = WebUtil.QuerySdt(del);

                    dsList.RetrieveList();
                    dsList2.RetrieveList2();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + adjtypedesc + " สำเร็จ");
                }
                catch (Exception ex)
                { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
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
                    string adjtype_code = dsList.DATA[row_fc].adjtype_code;
                    string adjtype_desc = dsMain.DATA[0].adjtype_desc;
                    string sign_flag = dsMain.DATA[0].sign_flag;

                    string sqlEdit = @"update ptucfinvtadjtype set  adjtype_desc={1},sign_flag={2}
                                    where  adjtype_code={0}";
                    sqlEdit = WebUtil.SQLFormat(sqlEdit, adjtype_code, adjtype_desc, sign_flag);
                    exe.SQL.Add(sqlEdit);


                    exe.Execute();
                    exe.SQL.Clear();

                    LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลรายการ  " + adjtype_desc + " สำเร็จ");
                    Hdadd_status.Value = "2";
                    dsMain.ResetRow();
                    dsList.RetrieveList();
                    dsList2.RetrieveList2();

                }
                else if (add_status == "3")
                {
                    int row_fc = dsList2.GetRowFocus();
                    string adjtype_code = dsList2.DATA[row_fc].adjtype_code;
                    string adjtype_desc = dsMain.DATA[0].adjtype_desc;
                    string sign_flag = dsMain.DATA[0].sign_flag;

                    string sqlEdit = @"update ptucfinvtadjtype set  adjtype_desc={1},sign_flag={2}
                                    where  adjtype_code={0}";
                    sqlEdit = WebUtil.SQLFormat(sqlEdit, adjtype_code, adjtype_desc, sign_flag);
                    exe.SQL.Add(sqlEdit);


                    exe.Execute();
                    exe.SQL.Clear();

                    LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลรายการ  " + adjtype_desc + " สำเร็จ");
                    Hdadd_status.Value = "2";
                    dsMain.ResetRow();
                    dsList.RetrieveList();
                    dsList2.RetrieveList2();

                }
                else if (add_status == "2")
                {

                    string adjtype_code = "";
                    string adjtype_desc = dsMain.DATA[0].adjtype_desc;
                    string sign_flag = dsMain.DATA[0].sign_flag;

                    try
                    {
                        String se = @"select max(adjtype_code)as adjtype_code from ptucfinvtadjtype";
                        ta = WebUtil.QuerySdt(se);
                        if (ta.Next())
                        {
                            adjtype_code = ta.GetString("adjtype_code");
                        }
                        if (adjtype_code == null || adjtype_code == "")
                        {
                            adjtype_code = "0";
                        }
                        adjtype_code = Convert.ToString(Convert.ToDecimal(adjtype_code) + 1);

                        while (adjtype_code.Length < 3)
                        {
                            adjtype_code = "0" + adjtype_code;
                        }
                    }
                    catch { }
                    try
                    {
                        String insert = @"insert into ptucfinvtadjtype (adjtype_code,adjtype_desc,sign_flag)
                                                    values( {0}, {1},{2})";
                        insert = WebUtil.SQLFormat(insert, adjtype_code, adjtype_desc, sign_flag);
                        ta = WebUtil.QuerySdt(insert);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกรายการ " + adjtype_desc + " สำเร็จ");
                        dsMain.ResetRow();
                        dsList.RetrieveList();
                        dsList2.RetrieveList2();
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
        
    }
}