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

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_takereasoncode_ctrl
{
    public partial class w_sheet_cmd_ucf_takereasoncode : PageWebSheet, WebSheet
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
                string takereason_code = dsList.DATA[fc_row].takereason_code;
                dsMain.retrieve(takereason_code);
                // PostSetData();

            }
            if (eventArg == Postdel)
            {

                
                int row = dsList.GetRowFocus();
                string takereason_desc = dsList.DATA[row].takereason_desc;
                try
                {
                    string takereason_code = dsList.DATA[row].takereason_code;
                    String del = @"delete ptucftakereasoncode where takereason_code = {0}";
                    del = WebUtil.SQLFormat(del, takereason_code);
                    ta = WebUtil.QuerySdt(del);

                    dsList.RetrieveList();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + takereason_desc + " สำเร็จ");
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
                  string takereason_code = dsList.DATA[row_fc].takereason_code;
                  string takereason_desc = dsMain.DATA[0].takereason_desc;

                  string sqlEdit = @"update ptucftakereasoncode set  takereason_desc={1}
                                    where  takereason_code={0}";
                  sqlEdit = WebUtil.SQLFormat(sqlEdit, takereason_code, takereason_desc);
                  exe.SQL.Add(sqlEdit);


                  exe.Execute();
                  exe.SQL.Clear();

                  LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลรายการ  " + takereason_desc + " สำเร็จ");
                  Hdadd_status.Value = "2";
                  dsMain.ResetRow();
                  dsList.RetrieveList();

              }
              else if (add_status == "2")
              {
                  string takereason_code = "";
                  string takereason_desc = dsMain.DATA[0].takereason_desc;

                  try
                  {
                      String se = @"select max(takereason_code)as takereason_code from ptucftakereasoncode";
                      ta = WebUtil.QuerySdt(se);
                      if (ta.Next())
                      {
                          takereason_code = ta.GetString("takereason_code");
                      }
                      if (takereason_code == null || takereason_code == "")
                      {
                          takereason_code = "0";
                      }
                      takereason_code = Convert.ToString(Convert.ToDecimal(takereason_code) + 1);

                      while (takereason_code.Length < 2)
                      {
                          takereason_code = "0" + takereason_code;
                      }
                  }
                  catch { }
                  try
                  {
                      String insert = @"insert into ptucftakereasoncode (takereason_code,takereason_desc)
                                values( {0}, {1})";
                      insert = WebUtil.SQLFormat(insert, takereason_code, takereason_desc);
                      ta = WebUtil.QuerySdt(insert);
                      LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกรายการ " + takereason_desc + " สำเร็จ");
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