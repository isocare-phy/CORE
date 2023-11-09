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

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_invtlitemcode_ctrl
{
    public partial class w_sheet_cmd_ucf_invtlitemcode : PageWebSheet, WebSheet
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
                dsMain.DATA[0].sign_flag = "1";
            dsList.RetrieveList();
            
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == Postdel)
            {

               // String branch_code = "";
                int row = dsList.GetRowFocus();
                string item_des = dsList.DATA[row].item_des;
                dsMain.DATA[0].sign_flag = "1";
                try
                {
                    string item_code = dsList.DATA[row].item_code;
                    String del = @"delete ptucfinvtlitemcode where item_code = {0}";
                    del = WebUtil.SQLFormat(del, item_code);
                    ta = WebUtil.QuerySdt(del);

                    dsList.RetrieveList();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + item_des + " สำเร็จ");
                    
                }
                catch (Exception ex)
                { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }


            }
            if (eventArg == Postedit)
            {
                Hdadd_status.Value = "1";
                dsMain.DATA[0].sign_flag = "1";
                int fc_row = dsList.GetRowFocus();
                string item_code = dsList.DATA[fc_row].item_code;
                dsMain.retrieve(item_code);
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
                  string item_code = dsList.DATA[row_fc].item_code;
                  string item_des = dsMain.DATA[0].item_des;
                  string sign_flag = dsMain.DATA[0].sign_flag;

                  string sqlEdit = @"update ptucfinvtlitemcode set  item_des={1} ,sign_flag={2}
                                    where  item_code={0}";
                  sqlEdit = WebUtil.SQLFormat(sqlEdit, item_code, item_des, sign_flag);
                  exe.SQL.Add(sqlEdit);


                  exe.Execute();
                  exe.SQL.Clear();

                  LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลรายการ  " + item_des + " สำเร็จ");
                  Hdadd_status.Value = "2";
                 
                  dsMain.ResetRow();
                  dsList.RetrieveList();

              }
              else if (add_status == "2")
              {

                  string item_code = "";
                  string item_des = dsMain.DATA[0].item_des;
                  string sign_flag = dsMain.DATA[0].sign_flag;
                 

                  try
                  {
                      String se = @"select max(item_code)as item_code from ptucfinvtlitemcode";
                      ta = WebUtil.QuerySdt(se);
                      if (ta.Next())
                      {
                          item_code = ta.GetString("item_code");
                      }
                      if (item_code == null || item_code == "")
                      {
                          item_code = "0";
                      }
                      item_code = Convert.ToString(Convert.ToDecimal(item_code) + 1);

                      while (item_code.Length < 3)
                      {
                          item_code = "0" + item_code;
                      }
                  }
                  catch { }
                  try
                  {
                      String insert = @"insert into ptucfinvtlitemcode (item_code,item_des,sign_flag)
                                                    values( {0}, {1},{2})";
                      insert = WebUtil.SQLFormat(insert, item_code, item_des, sign_flag);
                      ta = WebUtil.QuerySdt(insert);
                      LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกรายการ " + item_des + " สำเร็จ");
             
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