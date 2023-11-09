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

namespace Saving.Applications.cmd.ws_cmd_dealer_ctrl
{
    public partial class ws_cmd_dealer : PageWebSheet, WebSheet
    {
        Sdt ta = new Sdt();

        [JsPostBack]
        public String Postdel { get; set; }

        [JsPostBack]
        public String PostInsertRow { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
        }

        public void WebSheetLoadBegin()
        {
            //if (!IsPostBack)
            //{
            //    dsMain.retrieve();
            //}
        }

        public void CheckJsPostBack(string eventArg)
        {
            
        }

        public void SaveWebSheet()
        {
            String dealer_no = "", dealer_name = "";
            String dealer_addr = "", dealer_taxid = "";
            decimal dealer_phone;
            decimal count = 0;
            try
            {
                 dealer_no = WebUtil.MemberNoFormat(dsMain.DATA[0].DEALER_NO.Trim());
                 dsMain.DATA[0].DEALER_NO = dealer_no;
                 dealer_name = dsMain.DATA[0].DEALER_NAME.Trim();
                 dealer_addr = dsMain.DATA[0].DEALER_ADDR.Trim();
                 dealer_taxid = dsMain.DATA[0].DEALER_TAXID.Trim();
                 dealer_phone = dsMain.DATA[0].DEALER_PHONE;

                string sql = sql = @"
                                    SELECT count(*) as result
                                    FROM         
                                        PTUCFDEALER 
                                    where PTUCFDEALER.DEALER_NO = '" + dealer_no + "' ";
                sql = WebUtil.SQLFormat(sql);
                ta = WebUtil.QuerySdt(sql);
                if (ta.Next())
                    count = ta.GetDecimal("count");
                if (count == 0)
                {
                    sql = @"insert into PTUCFDEALER 
                                    ( dealer_no, 
		                                dealer_name, 
		                                dealer_addr, 
		                                dealer_taxid, 
		                                dealer_phone )
                                    values({0},{1},{2},{3},{4},{5},{6},{7}) ";
                    sql = WebUtil.SQLFormat(sql, dealer_no, dealer_name, dealer_addr
                        , dealer_taxid, dealer_phone );
                    WebUtil.QuerySdt(sql);
                }
                else if (count == 1)
                {
                    //update
                    sql = @"update PTUCFDEALER 
                                    set 
		                                dealer_name = {1}, 
		                                dealer_addr = {2}, 
		                                dealer_taxid = {3}, 
		                                dealer_phone = {4} 
                                    where dealer_no = {0} ";
                    sql = WebUtil.SQLFormat(sql, dealer_no, dealer_name, dealer_addr
                        , dealer_taxid, dealer_phone );
                    WebUtil.QuerySdt(sql);
                }


                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
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