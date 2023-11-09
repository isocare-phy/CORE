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

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_cutreasoncode_ctrl
{
    public partial class w_sheet_cmd_ucf_cutreasoncode : PageWebSheet, WebSheet
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
                string cutreason_code = dsList.DATA[fc_row].cutreason_code;
                dsMain.retrieve(cutreason_code);
                // PostSetData();

            }
            if (eventArg == Postdel)
            {

                
                int row = dsList.GetRowFocus();
                string cutreason_desc = dsList.DATA[row].cutreason_desc;
                try
                {
                    string cutreason_code = dsList.DATA[row].cutreason_code;
                    String del = @"delete ptucfcutreasoncode where cutreason_code = {0}";
                    del = WebUtil.SQLFormat(del, cutreason_code);
                    ta = WebUtil.QuerySdt(del);

                    dsList.RetrieveList();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + cutreason_desc + " สำเร็จ");
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
                  string cutreason_code = dsList.DATA[row_fc].cutreason_code;
                  string cutreason_desc = dsMain.DATA[0].cutreason_desc;

                  string sqlEdit = @"update ptucfcutreasoncode set  cutreason_desc={1}
                                    where  cutreason_code={0}";
                  sqlEdit = WebUtil.SQLFormat(sqlEdit, cutreason_code, cutreason_desc);
                  exe.SQL.Add(sqlEdit);


                  exe.Execute();
                  exe.SQL.Clear();

                  LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลรายการ  " + cutreason_desc + " สำเร็จ");
                  Hdadd_status.Value = "2";
                  dsMain.ResetRow();
                  dsList.RetrieveList();

              }
              else if (add_status == "2")
              {

                  string cutreason_code = "";
                  string cutreason_desc = dsMain.DATA[0].cutreason_desc;

                  try
                  {
                      String se = @"select max(cutreason_code)as cutreason_code from ptucfcutreasoncode";
                      ta = WebUtil.QuerySdt(se);
                      if (ta.Next())
                      {
                          cutreason_code = ta.GetString("cutreason_code");
                      }
                      if (cutreason_code == null || cutreason_code == "")
                      {
                          cutreason_code = "0";
                      }
                      cutreason_code = Convert.ToString(Convert.ToDecimal(cutreason_code) + 1);

                      while (cutreason_code.Length < 2)
                      {
                          cutreason_code = "0" + cutreason_code;
                      }
                  }
                  catch { }
                  try
                  {
                      String insert = @"insert into ptucfcutreasoncode (cutreason_code,cutreason_desc)
                                values( {0}, {1})";
                      insert = WebUtil.SQLFormat(insert, cutreason_code, cutreason_desc);
                      ta = WebUtil.QuerySdt(insert);
                      LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกรายการ " + cutreason_desc + " สำเร็จ");
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