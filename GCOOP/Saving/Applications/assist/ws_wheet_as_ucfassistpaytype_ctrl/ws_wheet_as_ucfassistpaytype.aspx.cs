using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.assist.ws_wheet_as_ucfassistpaytype_ctrl
{
    public partial class ws_wheet_as_ucfassistpaytype : PageWebSheet, WebSheet
    {
       
        [JsPostBack]
        public string PostNewRow { get; set; }
        [JsPostBack]
        public string PostDelRow { get; set; }

        public void InitJsPostBack()
        {
            dsList.InitDs(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsList.Retrieve();//show data first
                dsList.RetriveGroup();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostNewRow)
            {
                dsList.InsertAtRow(0);
                dsList.SetItem(0, dsList.DATA.COOP_IDColumn, state.SsCoopControl);//set value to primary key

                int li_chkasscode = 0;
                string ls_asscode;
                for (int li_row = 1; li_row < dsList.RowCount; li_row++)
                {
                    if (li_chkasscode < Convert.ToInt32(dsList.DATA[li_row].ASSISTPAY_CODE))
                    {
                        li_chkasscode = Convert.ToInt32(dsList.DATA[li_row].ASSISTPAY_CODE);
                    }
                }
                li_chkasscode = li_chkasscode + 1;
                ls_asscode = li_chkasscode.ToString();
                if (ls_asscode.Length == 1) { ls_asscode = '0' + ls_asscode; }
                dsList.DATA[0].ASSISTPAY_CODE = ls_asscode;
                dsList.RetriveGroup();
            }
            else if (eventArg == PostDelRow)
            {
                String ls_chktype = "";
                int row = dsList.GetRowFocus();
                String ls_type = dsList.DATA[row].ASSISTPAY_CODE;

                //chk เงื่อนไขการจ่าย
                string sql = @"select assisttype_pay from ASSUCFASSISTTYPEDET where assisttype_pay={0} and coop_id={1} ";
                sql = WebUtil.SQLFormat(sql, ls_type, state.SsCoopControl);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('การจ่ายสวัสดิการประเภทนี้มีการกำหนดเงื่อนไขการจ่ายแล้วไม่สามารถลบได้');", true); return;
                }
                else
                {
                   try
                    {
                        ls_chktype = @"delete from ASSUCFASSISTPAYTYPE where coop_id = {0} and assistpay_code={1} ";
                        ls_chktype = WebUtil.SQLFormat(ls_chktype, state.SsCoopId, ls_type);
                        WebUtil.ExeSQL(ls_chktype);
                        dsList.Retrieve();
                        dsList.RetriveGroup();
                        LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลสำเร็จ");
                    }
                   catch
                   {
                       LtServerMessage.Text = WebUtil.ErrorMessage("ลบข้อมูลไม่สำเสร็จ");
                   }
                }   
            }
        }

        public void SaveWebSheet()
        {
            string sqlStr, ls_assiscode, ls_assisdesc, ls_chkassiscode = "",ls_assisgroup;
            int li_row;

            try
            {
                for (li_row = 0; li_row < dsList.RowCount; li_row++)
                {
                    if (dsList.DATA[li_row].ASSISTPAY_CODE.ToString() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณากรอกรหัสประเภทการจ่ายสวัสดิการ');", true); return;
                    }
                    else if (dsList.DATA[li_row].ASSISTPAY_DESC.ToString() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณากรอกคำอธิบายารจ่ายสวัสดิการ');", true); return;
                    }
                    else if (ls_chkassiscode.IndexOf(dsList.DATA[li_row].ASSISTPAY_CODE.ToString()) > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('เลขประเภทการจ่ายสวัสดิการซ้ำกัน กรุณาตรวจสอบ');", true); return;
                    }
                    ls_chkassiscode = ls_chkassiscode + ", " + dsList.DATA[li_row].ASSISTPAY_CODE.ToString();

                }
                for (li_row = 0; li_row < dsList.RowCount; li_row++)
                {                    
                    ls_assiscode  = dsList.DATA[li_row].ASSISTPAY_CODE.ToString();
                    ls_assisdesc  = dsList.DATA[li_row].ASSISTPAY_DESC.ToString();
                    ls_assisgroup = dsList.DATA[li_row].ASSISTTYPE_GROUP;
                    //chk ประเภทสวัสดิการ
                    string sql = @"select assistpay_code from ASSUCFASSISTPAYTYPE where assistpay_code={0} and coop_id={1}";
                    sql = WebUtil.SQLFormat(sql, ls_assiscode, state.SsCoopControl);
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        sqlStr = @"update ASSUCFASSISTPAYTYPE set 
                                assistpay_desc={0}, ASSISTTYPE_GROUP={3} where assistpay_code={1} and coop_id={2}
                                ";
                        sqlStr = WebUtil.SQLFormat(sqlStr, ls_assisdesc, ls_assiscode, state.SsCoopId,ls_assisgroup);
                        WebUtil.ExeSQL(sqlStr);
                    }
                    else
                    {
                        sqlStr = @"insert into ASSUCFASSISTPAYTYPE 
                            (assistpay_code, assistpay_desc, coop_id,ASSISTTYPE_GROUP)
                            values
                            ({0}, {1}, {2}, {3})";
                        sqlStr = WebUtil.SQLFormat(sqlStr, ls_assiscode, ls_assisdesc, state.SsCoopId, ls_assisgroup);
                        WebUtil.ExeSQL(sqlStr);
                    }
                }
                dsList.Retrieve();
                dsList.RetriveGroup();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเสร็จ");
            }
        }

        public void WebSheetLoadEnd()
        {

        }
    }
}