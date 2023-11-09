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

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_unitcode_ctrl
{
    public partial class w_sheet_cmd_ucf_unitcode : PageWebSheet, WebSheet
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
            //    dsMain.retrieve();
            dsList.RetrieveList();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == Postdel)
            {

                
                int row = dsList.GetRowFocus();
                string unit_desc = dsList.DATA[row].unit_desc;
                try
                {
                    string unit_code = dsList.DATA[row].unit_code;
                    String del = @"delete ptucfunitcode where unit_code = {0}";
                    del = WebUtil.SQLFormat(del, unit_code);
                    ta = WebUtil.QuerySdt(del);

                    dsList.RetrieveList();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + unit_desc + " สำเร็จ");
                }
                catch (Exception ex)
                { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }


            }
            if (eventArg == Postedit)
            {
                Hdadd_status.Value = "1";
                int fc_row = dsList.GetRowFocus();
                string unit_code = dsList.DATA[fc_row].unit_code;
                dsMain.retrieve(unit_code);
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
                    string unit_code = dsList.DATA[row_fc].unit_code;
                    string unit_desc = dsMain.DATA[0].unit_desc;
                    string sqlEdit = @"update ptucfunitcode set unit_desc={1}
                                    where  unit_code={0}";
                    sqlEdit = WebUtil.SQLFormat(sqlEdit, unit_code, unit_desc);
                    exe.SQL.Add(sqlEdit);


                    exe.Execute();
                    exe.SQL.Clear();

                    LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลรายการ  " + unit_desc + " สำเร็จ");
                    Hdadd_status.Value = "2";
                    dsMain.ResetRow();
                    dsList.RetrieveList();

                }
                else if (add_status == "2")
                {
                    string unit_code = "";
                    string unit_desc = dsMain.DATA[0].unit_desc;

                    try
                    {
                        String se = @"select max(unit_code)as unit_code from ptucfunitcode";
                        ta = WebUtil.QuerySdt(se);
                        if (ta.Next())
                        {
                            unit_code = ta.GetString("unit_code");
                        }
                        if (unit_code == null || unit_code == "")
                        {
                            unit_code = "0";
                        }
                        unit_code = Convert.ToString(Convert.ToDecimal(unit_code) + 1);

                        while (unit_code.Length < 3)
                        {
                            unit_code = "0" + unit_code;
                        }
                    }
                    catch { }
                    try
                    {
                        String insert = @"insert into ptucfunitcode (unit_code,unit_desc)
                                values( {0}, {1})";
                        insert = WebUtil.SQLFormat(insert, unit_code, unit_desc);
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
    }
}