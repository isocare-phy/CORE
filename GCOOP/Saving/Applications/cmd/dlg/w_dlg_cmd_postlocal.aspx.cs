using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Sybase.DataWindow;
using DataLibrary;
using CoreSavingLibrary.WcfNCommon;

namespace Saving.Applications.cmd.dlg
{
    public partial class w_dlg_cmd_postlocal : PageWebDialog, WebDialog
    {
        protected String Refresh;
        protected String newClear;
        protected String postRetrieveList;
        protected String postRetrieveMain;
        private DwThDate tDwMain;
        protected String jsPostblank;
        String create_id;
        public void InitJsPostBack()
        {
            newClear = WebUtil.JsPostBack(this, "newClear");
            Refresh = WebUtil.JsPostBack(this, "Refresh");
            postRetrieveList = WebUtil.JsPostBack(this, "postRetrieveList");
            postRetrieveMain = WebUtil.JsPostBack(this, "postRetrieveMain");
            jsPostblank = WebUtil.JsPostBack(this, "jsPostblank");
            tDwMain = new DwThDate(Dw_main, this);
            tDwMain.Add("move_day", "move_tday");
            tDwMain.Add("entry_date", "entry_tdate");
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                try
                {
                    //Dw_main.Modify("b_search.visible = 0");
                    create_id = Request.QueryString["create_id"].ToString().Trim();
                    Hfcreate_id.Value = create_id;
                    RetrieveList();
                    Dw_main.SetItemString(1, "lot_id", "010001");
                    Dw_main.SetItemString(1, "lotend", "010001"); 
                    //Dw_Local.InsertRow(0);
                    //Dw_main.InsertRow(0);
                    //Dw_main.SetItemString(1,"entry_id", state.SsUsername);
                    //Dw_main.SetItemDateTime(1,"move_day", state.SsWorkDate);
                    //Dw_main.SetItemDateTime(1,"entry_date", state.SsWorkDate);
                    //Dw_main.SetItemString(1,"branch_id", state.SsCoopId);
                    tDwMain.Eng2ThaiAllRow();
                }
                catch { }
            }
            else
            {
                this.RestoreContextDw(Dw_Local);
                this.RestoreContextDw(Dw_main);
            }

            //DwUtil.RetrieveDDDW(Dw_main, "branch_id", "cmd_local.pbl", null);
            if (!IsPostBack)
            {
                //Dw_main.SetItemString(1, "branch_id","01");
            }
            DwUtil.RetrieveDDDW(Dw_main, "lot_id", "cmd_local.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "lotend", "cmd_local.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "locate_stock", "cmd_local.pbl", null);
            tDwMain.Eng2ThaiAllRow();
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "Refresh":
                    postRefresh();
                    break;
                case "postRetrieveList":
                    RetrieveList();
                    break;
                case "postRetrieveMain":
                    RetrieveMain();
                    break;
                case "newClear":
                    NewClear();
                    break;
                case "jsPostblank":
                    ChangeDate2TH();
                    break;
            }
        }

        public void WebDialogLoadEnd()
        {
            Dw_main.SaveDataCache();
            Dw_Local.SaveDataCache();
        }

        #region Function

        private void NewClear()
        {
            try
            {
                Dw_main.Reset();
                Dw_Local.Reset();
                Dw_main.InsertRow(0);
                Dw_main.SetItemDateTime(1, "move_day", state.SsWorkDate);
                Dw_main.SetItemDateTime(1, "entry_date", state.SsWorkDate);
                Dw_main.SetItemString(1, "branch_id", "01");
                Dw_main.SetItemString(1, "entry_id", state.SsUsername);
                DwUtil.RetrieveDDDW(Dw_main, "locate_stock", "cmd_local.pbl", null);
                tDwMain.Eng2ThaiAllRow();
                //  DwUtil.RetrieveDDDW(Dw_main, "branch_id", "cmd_local.pbl", null);
                //   DwUtil.RetrieveDDDW(Dw_main, "lot_id", "cmd_local.pbl", null);
                //   DwUtil.RetrieveDDDW(Dw_main, "lotend", "cmd_local.pbl", null);
                //   DwUtil.RetrieveDDDW(Dw_main, "locate_stock", "cmd_local.pbl", null);
            }
            catch { }
        }

        private void postRefresh()
        {
            string product_id = HfproductId.Value;
            DataWindowChild child = Dw_main.GetChild("lot_id");
            child.SetTransaction(sqlca);
            child.Retrieve();
            child.SetFilter("product_id='" + product_id + "'");
            child.Filter();
            DataWindowChild childend = Dw_main.GetChild("lotend");
            childend.SetTransaction(sqlca);
            childend.Retrieve();
            childend.SetFilter("product_id='" + product_id + "'");
            childend.Filter();
            DwUtil.RetrieveDDDW(Dw_main, "unit_count", "cmd_local.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "locate_stock", "cmd_local.pbl", null);
        }

        private void RetrieveList()
        {
            String create_id = Hfcreate_id.Value;
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();
            String product_id = "", item_list = "", entry_id="";
            string sql = @" SELECT  *                       
                            FROM PTNMLPRODUCT
                            WHERE ( CREATE_ID='" + create_id + "' ) ";
            dt = ta.Query(sql);
            if (dt.Next())
            {
                product_id = dt.GetString("PRODUCT_ID");
                item_list = dt.GetString("PRODUCT_NAME");
            }
            Dw_Local.SetTransaction(sqlca);
            Dw_Local.Retrieve(product_id);
            if (Dw_Local.RowCount < 1)
            {
                Dw_main.Reset();
                Dw_main.InsertRow(0);
                Dw_main.SetItemString(1, "create_id", create_id);
                Dw_main.SetItemString(1, "item_list", item_list);
                Dw_main.SetItemString(1, "entry_id", state.SsUsername);
                Dw_main.SetItemDateTime(1, "entry_date", state.SsWorkDate);
                Dw_main.SetItemDateTime(1, "move_day", state.SsWorkDate);
                Dw_main.SetItemString(1, "product_id",product_id);
                if (Dw_main.RowCount > 1)
                {
                    Dw_main.DeleteRow(Dw_main.RowCount);
                }
                
            }
        }

        private void RetrieveMain()
        {
            String product_id = HfproductId.Value;
            String lot_id = HLotid.Value;
            Dw_main.SetTransaction(sqlca);
            Dw_main.Reset();
            Dw_main.Retrieve(product_id, lot_id);
            tDwMain.Eng2ThaiAllRow();
            //Dw_main.SetItemDateTime(1, "move_day", state.SsWorkDate);
            //Dw_main.SetItemDateTime(1, "entry_date", state.SsWorkDate);
            DwUtil.RetrieveDDDW(Dw_main, "locate_stock", "cmd_local.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "locate_stock", "cmd_local.pbl", null);

            // postRefresh();
        }

        public void ChangeDate2TH()
        {
            //Hc ThDate

            String date;
            int day, month, year;
            DateTime dt;

            date = Dw_main.GetItemString(1, "move_tday");
            day = Convert.ToInt16(date.Substring(0, 2));
            month = Convert.ToInt16(date.Substring(2, 2));
            year = Convert.ToInt16(date.Substring(4, 4)) - 543;
            dt = new DateTime(year, month, day);
            Dw_main.SetItemDateTime(1, "move_day", dt);

            date = Dw_main.GetItemString(1, "entry_tdate");
            day = Convert.ToInt16(date.Substring(0, 2));
            month = Convert.ToInt16(date.Substring(2, 2));
            year = Convert.ToInt16(date.Substring(4, 4)) - 543;
            dt = new DateTime(year, month, day);
            Dw_main.SetItemDateTime(1, "entry_date", dt);
        }

        #endregion

        protected void BtnSav_Click(object sender, EventArgs e)
        {

            String sqlStr;
            String product_id, lot_id, lotend, branch_id, locate_stock, user_name, item_list, unit_count, entry_id;
            DateTime move_day, entry_date;
            Decimal seq_no;
            Double count;
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();
            try
            {
                try { seq_no = Dw_main.GetItemDecimal(1, "seq_no"); }
                catch { seq_no = 0; }
                product_id = Dw_main.GetItemString(1, "product_id");
                lot_id = Dw_main.GetItemString(1, "lot_id");
                lotend = Dw_main.GetItemString(1, "lotend");
                branch_id = Dw_main.GetItemString(1, "branch_id");
                locate_stock = Dw_main.GetItemString(1, "locate_stock");
                try { user_name = Dw_main.GetItemString(1, "username"); }
                catch { user_name = locate_stock; }
                move_day = Dw_main.GetItemDateTime(1, "move_day");
                item_list = Dw_main.GetItemString(1, "item_list");
                try { unit_count = Dw_main.GetItemString(1, "unit_name"); }
                catch { unit_count = ""; }
                entry_id = Dw_main.GetItemString(1, "entry_id");
                entry_date = Dw_main.GetItemDateTime(1, "entry_date");

                sqlStr = "SELECT count(*) FROM PTUCFLOCALDEA WHERE seq_no = " + seq_no + " ";
                dt = ta.Query(sqlStr);
                dt.Next();
                count = dt.GetDouble("count(*)");

                if (count >= 1)
                {
                    try
                    {
                        sqlStr = @"UPDATE PTUCFLOCALDEA SET product_id ='" + product_id + @"',
                                                    lot_id ='" + lot_id + @"',
                                                    branch_id ='" + branch_id + @"',
                                                    locate_stock ='" + locate_stock + @"',
                                                    username ='" + user_name + @"',
                                                    move_day = to_date('" + move_day.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),
                                                    item_list ='" + item_list + @"',
                                                    unit_count ='" + unit_count + @"',
                                                    entry_id ='" + entry_id + @"',
                                                    entry_date = to_date('" + entry_date.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy')
                              WHERE seq_no = " + seq_no + "";
                        ta.Exe(sqlStr);
                        LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขเรียบร้อย"); NewClear();
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }
                }
                else if (count == 0)
                {
                    try
                    {
                        //วน for เก็บข้อมูลสถานที่เก้ฐของแต่ล่ะlot
                        int lot_start = Convert.ToInt32(WebUtil.Right(lot_id, 3));
                        int lot_end = Convert.ToInt32(WebUtil.Right(lotend, 3));

                        branch_id = Dw_main.GetItemString(1, "branch_id");
                        for (int i = lot_start; i <= lot_end; i++)
                        {
                            n_commonClient comm = wcf.NCommon;
                            Int32 maxdoc = Convert.ToInt32(comm.of_getnewdocno(state.SsWsPass,state.SsCoopId, "CMDSEQLOCNO"));
                            Dw_main.SetItemDouble(1, "seq_no", maxdoc);
                            Dw_main.SetTransaction(sqlca);
                            String lot = WebUtil.Left(lot_id, 2);
                            String lot_set = lot + i.ToString("000");
                            sqlStr = @"INSERT INTO PTUCFLOCALDEA (  seq_no,                 product_id,                 lot_id,                                                                   branch_id,  
                                                              locate_stock,                   username,               move_day,                                                                   item_list,  
                                                                unit_count,                   entry_id,             entry_date)                         
                                      VALUES(
                                                            " + maxdoc + ",       '" + product_id + "',      '" + lot_set + "',                                                        '" + branch_id + @"',
                                                    '" + locate_stock + "',        '" + user_name + "', to_date('" + move_day.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),  '" + item_list + @"',
                                                      '" + unit_count + "',         '" + entry_id + "', to_date('" + entry_date.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'))";

                            ta.Exe(sqlStr);
                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย"); NewClear();
                        }
                        //Dw_main.Reset();
                        //Dw_Local.Reset();
                        //Dw_main.Retrieve(product_id);
                        //Dw_Local.Retrieve(product_id);


                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}