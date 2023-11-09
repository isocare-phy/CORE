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
using CoreSavingLibrary.WcfCommon;
using CoreSavingLibrary.WcfReport;
using CoreSavingLibrary.WcfTrading;
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.trading
{
    public partial class w_sheet_td_info_store : PageWebSheet, WebSheet
    {
        private string pbl = "sheet_td_info_store.pbl";
        protected String postInsertRow;
        protected String postDeleteRow;
        int InsertRow = 0; //จำนวนแถวที่เพิ่ม
        int DataRow = 0; //จำนวนข้อมูลที่มีอยู่เดิม

        public void InitJsPostBack()
        {
            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            if (!IsPostBack)
            {
                DwMain.Retrieve();
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postInsertRow")
            {
                JspostInsertRow();
            }
            else if (eventArg == "postDeleteRow")
            {
                JspostDeleteRow();
            }
        }
        
        public void SaveWebSheet()
        {
            SaveInfo();
        }

        public void WebSheetLoadEnd()
        {
            try
            {
          //      DwUtil.RetrieveDDDW(DwMain, "store_type", pbl, null);
            }
            catch { }
            DwMain.SaveDataCache();
        }

        private void JspostInsertRow()
        {
            int row = DwMain.InsertRow(0);            
            DwMain.SetItemDecimal(row, "active_flag", 1);
            DwMain.SetItemDecimal(row, "default_flag", 1);
        }

        private void JspostDeleteRow()
        {
            Int16 RowDetail = Convert.ToInt16(HdRow.Value);
            try
            {
                string sqldelete = @"DELETE FROM tdstore WHERE store_id = '" + DwMain.GetItemString(RowDetail, "store_id") + "' and coop_id = '" + state.SsCoopId + "'";
                Sdt delete = WebUtil.QuerySdt(sqldelete);
                DwMain.DeleteRow(RowDetail);
                LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเสร็จเรียบร้อยแล้ว");
            }
            catch
            {
                try
                {
                    DwMain.GetItemString(RowDetail, "store_id");
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถลบข้อมูลได้");
                }
                catch
                {
                    DwMain.DeleteRow(RowDetail);
                }
            }
        }

        private void SaveInfo()
        {
            bool flag = true;
            string erroe_code = "";
            string store_id = "";
            string store_desc = "";
            string store_type = "";
            string store_addr = "";
            string store_district = "";
            string store_province = "";
            string post_code = "";
            Decimal active_flag = 0;
            Decimal default_flag = 0;
           

            InsertRow = DwMain.RowCount;
            string sqlcount = @"SELECT * FROM tdstore";
            Sdt count = WebUtil.QuerySdt(sqlcount);
            DataRow = count.GetRowCount();
            try
            {
                for (int j = 1; j <= DataRow; j++)
                {
                    try
                    {
                        store_id = DwMain.GetItemString(j, "store_id");
                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลรหัสคลังสินค้า");
                        return;
                    }
                    try
                    {
                        store_desc = DwMain.GetItemString(j, "store_desc");
                    }
                    catch
                    {
                        store_desc = "";
                    }
                    try
                    {
                        store_type = DwMain.GetItemString(j, "store_type");
                    }
                    catch
                    {
                        store_type = "";
                    }
                    try
                    {
                        store_addr = DwMain.GetItemString(j, "store_addr");
                    }
                    catch
                    {
                        store_addr = "";
                    }
                    try
                    {
                        store_district = DwMain.GetItemString(j, "store_district");
                    }
                    catch
                    {
                        store_district = "";
                    }
                    try
                    {
                        store_province = DwMain.GetItemString(j, "store_province");
                    }
                    catch
                    {
                        store_province = "";
                    }
                    try
                    {
                        post_code = DwMain.GetItemString(j, "post_code");
                    }
                    catch
                    {
                        post_code = "";
                    }

                    try
                    {
                        active_flag = DwMain.GetItemDecimal(j, "active_flag");
                    }
                    catch
                    {
                        active_flag = 0;
                    }
                    try
                    {
                        default_flag = DwMain.GetItemDecimal(j, "default_flag");
                    }
                    catch
                    {
                        default_flag = 0;
                    }

                    string sqlupdate = @"UPDATE tdstore SET 
                    store_desc = '" + store_desc + @"',
                    store_type = '" + store_type + @"',
                    store_addr = '" + store_addr + @"',
                    store_district = '" + store_district + @"',
                    store_province = '" + store_province + @"',
                    post_code = '" + post_code + @"',
                    active_flag = " + active_flag + @",  
                    default_flag = " + default_flag + @"
                    where  store_id = '" + store_id + @"' and  
                    coop_id = '" + state.SsCoopId + "'";
                    Sdt update = WebUtil.QuerySdt(sqlupdate);              
                }

                for (int i = DataRow + 1; i <= InsertRow; i++)
                {
                    try
                    {
                        try
                        {
                            store_id = DwMain.GetItemString(i, "store_id");
                        }
                        catch
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลรหัสคลังสินค้า");
                            return;
                        }
                        try
                        {
                            store_desc = DwMain.GetItemString(i, "store_desc");
                        }
                        catch
                        {
                            store_desc = "";
                        }
                        try
                        {
                            store_type = DwMain.GetItemString(i, "store_type");
                        }
                        catch
                        {
                            store_type = "";
                        }
                        try
                        {
                            store_addr = DwMain.GetItemString(i, "store_addr");
                        }
                        catch
                        {
                            store_addr = "";
                        }
                        try
                        {
                            store_district = DwMain.GetItemString(i, "store_district");
                        }
                        catch
                        {
                            store_district = "";
                        }
                        try
                        {
                            store_province = DwMain.GetItemString(i, "store_province");
                        }
                        catch
                        {
                            store_province = "";
                        }
                        try
                        {
                            post_code = DwMain.GetItemString(i, "post_code");
                        }
                        catch
                        {
                            post_code = "";
                        }

                        try
                        {
                            active_flag = DwMain.GetItemDecimal(i, "active_flag");
                        }
                        catch
                        {
                            active_flag = 0;
                        }
                        try
                        {
                            default_flag = DwMain.GetItemDecimal(i, "default_flag");
                        }
                        catch
                        {
                            default_flag = 0;
                        }

                        string sqlinsert = @"INSERT INTO tdstore (store_id,store_desc,store_type,store_addr,store_district,store_province,post_code,active_flag,default_flag,coop_id) VALUES 
                        ('" + store_id + @"',
                        '" + store_desc + @"',
                        '" + store_type + @"',
                        '" + store_addr + @"',
                        '" + store_district + @"',
                        '" + store_province + @"',
                        '" + post_code + @"',
                        " + active_flag + @",
                        " + default_flag + @",
                        '" + state.SsCoopId + "')";                                     
                        Sdt insert = WebUtil.QuerySdt(sqlinsert);
                    }
                    catch
                    {
                        if (!flag)
                        {
                            erroe_code += ", ";
                        }
                        erroe_code += store_id;
                        flag = false;
                    }
                }
                if (flag)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จเรียบร้อยแล้ว");
                }
                else
                {
                    DwMain.Reset();
                    DwMain.Retrieve();
                    LtServerMessage.Text = WebUtil.ErrorMessage("รหัสคลังสินค้า " + erroe_code + " มีอยู่แล้ว");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.ToString());
            }
        }

    }
}