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

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_deptcode_ctrl
{
    public partial class w_sheet_cmd_ucf_deptcode : PageWebSheet, WebSheet
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
                string dept_desc = dsList.DATA[row].dept_desc;
                try
                {
                    string dept_code = dsList.DATA[row].dept_code;
                    String del = @"delete ptucfdeptcode where dept_code = {0}";
                    del = WebUtil.SQLFormat(del, dept_code);
                    ta = WebUtil.QuerySdt(del);

                    dsList.RetrieveList();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + dept_desc + " สำเร็จ");
                }
                catch (Exception ex)
                { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }


            }
            if (eventArg == Postedit)
            {
                Hdadd_status.Value = "1";     
                int fc_row = dsList.GetRowFocus();
                string dept_code = dsList.DATA[fc_row].dept_code;
                dsMain.retrieve(dept_code);
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
                    string dept_code = dsList.DATA[row_fc].dept_code;
                    string dept_desc = dsMain.DATA[0].dept_desc;
                    string dept_abb = dsMain.DATA[0].dept_abb;
                    string sqlEdit = @"update ptucfdeptcode set  dept_desc={1},dept_abb={2}
                                    where  dept_code={0}";
                    sqlEdit = WebUtil.SQLFormat(sqlEdit, dept_code, dept_desc, dept_abb);
                    exe.SQL.Add(sqlEdit);


                    exe.Execute();
                    exe.SQL.Clear();

                    LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลแผนก  " + dept_desc + " สำเร็จ");
                    Hdadd_status.Value = "2";
                    dsMain.ResetRow();
                    dsList.RetrieveList();

                }
                else if (add_status == "2")
                {
                    string dept_code = "";
                    string dept_desc = dsMain.DATA[0].dept_desc;
                    string dept_abb = dsMain.DATA[0].dept_abb;
                    
                    try
                    {
                        String se = @"select max(dept_code)as dept_code from ptucfdeptcode";
                        ta = WebUtil.QuerySdt(se);
                        if (ta.Next())
                        {
                            dept_code = ta.GetString("dept_code");
                        }
                        if (dept_code == null || dept_code == "")
                        {
                            dept_code = "0";
                        }
                        dept_code = Convert.ToString(Convert.ToDecimal(dept_code) + 1);

                        while (dept_code.Length < 3)
                        {
                            dept_code = "0" + dept_code;
                        }
                    }
                    catch { }
                    try
                    {
                        String insert = @"insert into ptucfdeptcode (dept_code,dept_desc, dept_abb)
                                values( {0}, {1}, {2})";
                        insert = WebUtil.SQLFormat(insert, dept_code, dept_desc, dept_abb);
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