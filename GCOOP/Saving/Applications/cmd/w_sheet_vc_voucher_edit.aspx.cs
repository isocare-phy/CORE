using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
//using Saving.CmConfig;
using DataLibrary;

namespace Saving.Applications.account
{
    public partial class w_sheet_vc_voucher_edit1 : System.Web.UI.Page
    {
        //เป็นส่วนที่ อินเฮอร์ริทมาจาก  cmconfig เรียกใช้ DwTrans เพราะจะได้เปลี่ยน base ที่เดียว ไม่ต้องมากำหนด base หลายรอบ
        private DwTrans SQLCA;

        protected void Page_Load(object sender, EventArgs e)
        {
         
            SQLCA = new DwTrans();
            SQLCA.Connect();
           
            dw_list.SetTransaction(SQLCA);
            dw_detail.SetTransaction(SQLCA);
            dw_main.SetTransaction(SQLCA);

            if (!hidden_search.Value.Equals(""))
            {
                dw_list.SetSqlSelect(hidden_search.Value);
                dw_list.Retrieve();
            }

            
            try
            {
                txt_vcdate.Text = "01/06/2553";
                txt_vcdate_TextChanged(null, null);
            }
            catch { }

            try
            {
                if (Request["voucher_no"] != null && Request["voucher_no"].Trim() != "")
                {
                    dw_main.Retrieve(Request["voucher_no"].Trim());
                    dw_detail.Retrieve(Request["voucher_no"].Trim());
                }
            }
            catch { }

            try
            {
                
            }
            catch { }

        }

        protected void Page_LoadComplete()
        {
            SQLCA.Disconnect();
        }

        protected void txt_vcdate_TextChanged(object sender, EventArgs e)
        {
            CultureInfo th = new CultureInfo("th-TH");
            DateTime dt;
            DateTime dtt;

            try
            {
                //สร้างตัวแปร DateTime ขึ้นมาชื่อ dt เพื่อนำมารับค่าจาก txt_vcdate.text
                dt = DateTime.ParseExact(txt_vcdate.Text.Trim(), "dd/MM/yyyy", th);
                dtt = DateTime.ParseExact(txt_vcdate.Text.Trim(), "dd/MM/yyyy", th);
                //สั่งให้ dw_list retrieve ค่าโดยส่งค่าวันที่ กับค่า branch_id ให้กับ dw_list
                dw_list.Retrieve(dt, "001");
                txt_vcdate.Text = dt.ToString("dd/MM/yyyy");
                hidden_search.Value = txt_vcdate.Text;
               
            }
            catch
            { }
        }

       

     
    }
}
