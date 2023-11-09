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

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_branchcode_ctrl
{
    public partial class w_sheet_cmd_ucf_branchcode : PageWebSheet, WebSheet
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
            if (eventArg == Postedit)
            {
                Hdadd_status.Value = "1";
                int fc_row = dsList.GetRowFocus();
                string branch_code = dsList.DATA[fc_row].branch_code;
                dsMain.retrieve(branch_code);
                // PostSetData();

            }
            if (eventArg == Postdel)
            {

               // String branch_code = "";
                int row = dsList.GetRowFocus();
                string branch_desc = dsList.DATA[row].branch_desc;
                try
                {
                    string branchcode = dsList.DATA[row].branch_code;
                    String del = @"delete ptucfbranchcode where branch_code = {0}";
                    del = WebUtil.SQLFormat(del, branchcode);
                    ta = WebUtil.QuerySdt(del);

                    dsList.RetrieveList();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + branch_desc + " สำเร็จ");
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
                  string branch_code = dsList.DATA[row_fc].branch_code;
                  string branch_desc = dsMain.DATA[0].branch_desc;

                  string sqlEdit = @"update ptucfbranchcode set  branch_desc={1}
                                    where  branch_code={0}";
                  sqlEdit = WebUtil.SQLFormat(sqlEdit, branch_code, branch_desc);
                  exe.SQL.Add(sqlEdit);


                  exe.Execute();
                  exe.SQL.Clear();

                  LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลรายการ  " + branch_desc + " สำเร็จ");
                  Hdadd_status.Value = "2";
                  dsMain.ResetRow();
                  dsList.RetrieveList();

              }
              else if (add_status == "2")
              {


                  string branch_code = "";
                  string branch_desc = dsMain.DATA[0].branch_desc;

                  try
                  {
                      String se = @"select max(branch_code)as branch_code from ptucfbranchcode";
                      ta = WebUtil.QuerySdt(se);
                      if (ta.Next())
                      {
                          branch_code = ta.GetString("branch_code");
                      }
                      if (branch_code == null || branch_code == "")
                      {
                          branch_code = "0";
                      }
                      branch_code = Convert.ToString(Convert.ToDecimal(branch_code) + 1);

                      while (branch_code.Length < 3)
                      {
                          branch_code = "0" + branch_code;
                      }
                  }
                  catch { }
                  try
                  {
                      String insert = @"insert into ptucfbranchcode (branch_code,branch_desc)
                                values( {0}, {1})";
                      insert = WebUtil.SQLFormat(insert, branch_code, branch_desc);
                      ta = WebUtil.QuerySdt(insert);
                      LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกรายการ " + branch_desc + " สำเร็จ");
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
        
    }
}