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
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_sum_balance_carried_forward : PageWebSheet, WebSheet
    {

        protected String postAddDistance;
        protected String postEditDistance;
        protected String postSaveEdit;
        protected String postRefresh;
        protected String postShowItemMain;



        public void InitJsPostBack()
        {
            postAddDistance = WebUtil.JsPostBack(this, "postAddDistance");
            postEditDistance = WebUtil.JsPostBack(this, "postEditDistance");
            postSaveEdit = WebUtil.JsPostBack(this, "postSaveEdit");
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postShowItemMain = WebUtil.JsPostBack(this, "postShowItemMain");


        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            DwMain.SetTransaction(sqlca);
            DwAdd.SetTransaction(sqlca);

            if (!IsPostBack)
            {
             
                DwMain.InsertRow(0);
                DwAdd.InsertRow(0);

            }
            else
            {
                try
                {

                    DwMain.RestoreContext();
                    DwAdd.RestoreContext();
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }

            DwAdd.SetTransaction(sqlca);
            DwAdd.Retrieve("bug10", 1, 2010);


            //DwMain.InsertRow(0);
            //DwUtil.RetrieveDDDW(DwMain, "as_asstype", "assis.pbl", null);//เป็นเรียกข้อมูลที่อยู่อีก DW อีกตัวมาแสดง

            //object[] arg = new object[3];
            //arg[0] = "bug10";
            //arg[1] = 1;
            //arg[2] = 2010;
            //DwUtil.RetrieveDataWindow(DwAdd, "assis.pbl", null, arg);//เป็นการเรียกข้อมูลออกมาแสดงในตัวมันเอง 

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postAddDistance")
            {
                AddDistance();
            }
            else if (eventArg == "postEditDistance")
            {
                EditDistance();
            }
            else if (eventArg == "postSaveEdit")
            {
                SaveEdit();
            }

            else if (eventArg == "postShowItemMain")
            {
                ShowItem();
            }
            else if (eventArg == "postRefresh")
            {
                Refresh();
            }
        }

        public void SaveWebSheet()
        {
            String sqlStr , budget_envcode = "" ;
            Decimal budget_month = 0, budget_year = 0; 
            Decimal budget_bf = 0, budget_use = 0, budget_bal = 0;
            budget_bf = DwAdd.GetItemDecimal(1, "budget_bf");
            budget_use = DwAdd.GetItemDecimal(1, "budget_use");
            budget_bal = DwAdd.GetItemDecimal(1, "budget_bal");

            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();
            try
            {
            //คำสั่ง insert update 
            
                DwUtil.UpdateDataWindow(DwAdd,"assis.pbl","asnbudget"); 

             sqlStr = @"UPDATE  asnbudget
                               SET     budget_bf     = '" + budget_bf + @"'
                                       budget_use    = '" + budget_use + @"'
                                       budget_bal    = '" + budget_bal + @"'
                               WHERE   (budget_month = '" + budget_month + @"')and
                                       (budget_year  = '" + budget_year + @"' ) ";

                   ta.Exe(sqlStr);
            }
                
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขเรียบร้อย");
            }



            DwMain.Reset();
            DwMain.InsertRow(1);
            
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwAdd.SaveDataCache();
        }


        public void SetApproveTDateAllRow()
        {


        }

        private void ShowItem()
        {
            try
            {
                //เป็นตัวลูกที่ทำเป็น Dpdown assistype_desc ใน DwMain

                String sqlStr, as_envcodetype = "";

                Decimal budget_month = 0, budget_year = 0;
                Decimal as_year = DwMain.GetItemDecimal(1, "as_year");
                Decimal as_month = DwMain.GetItemDecimal(1, "as_month");
                        as_envcodetype = DwMain.GetItemString(1, "as_envcodetype");
                Decimal budget_bf = 0, budget_use = 0, budget_bal = 0;
                        budget_bf = DwAdd.GetItemDecimal(1,"budget_bf");
                        budget_use = DwAdd.GetItemDecimal(1, "budget_use");
                        budget_bal = DwAdd.GetItemDecimal(1, "budget_bal");
                
                DwAdd.SetTransaction(sqlca);
                DwAdd.Retrieve(as_envcodetype, as_month, as_year);

              //DwMain.InsertRow(0); //ทดลองเพิ่ม Row
              //DwAdd.InsertRow(0); //ทดลองเพิ่ม Row
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
              DwMain.Reset();
            //DwAdd.Reset();
              DwMain.InsertRow(1);
            //DwAdd.InsertRow(1);



        }


        private void AddDistance()
        {

        }

        private void EditDistance()
        {


        }

        private void SaveEdit()
        {


            String sqlStr, as_envcodetype = "";
            as_envcodetype = DwAdd.GetItemString(1, "as_envcodetype");
            Decimal budget_month = 0, budget_year = 0, budget_bf = 0, budget_use = 0, budget_bal = 0, compute_1 = 0;
            Decimal as_month = DwAdd.GetItemDecimal(1, "as_month");
            Decimal as_year = DwAdd.GetItemDecimal(1, "as_year");

            Sta ta = new Sta(sqlca.ConnectionString);

            sqlStr = @" SELECT *
                          FROM asnbudget  
                                   WHERE 
                                        ( asnbudget.budget_envcode =" + as_envcodetype + @" ) AND  
                                        ( asnbudget.budget_month =" + as_month + @" ) AND  
                                        ( asnbudget.budget_year =" + as_year + @" ) ";

            Sdt dt_insert = ta.Query(sqlStr);
            int row_insert = dt_insert.GetRowCount();
            for (int r = 0; r < row_insert; r++)
            {

                sqlStr = @" UPDATE ASBUDGET SET  BUDGET_YEAR = " + as_year + ", BUDGET_MONTH =' " + as_month + @"' ,
                                                             BUDGET_ENVCODE =' " + as_envcodetype + @" ', BUDGET_BF = " + budget_bf + @", 
                                                             BUDGET_USE =' " + budget_use + "' , BUDGET_BAL =' " + budget_bal + @" ' 

                                        WHERE ( ASBUDGET.BUDGET_YEAR =" + as_year + ") AND ( ASBUDGET.BUDGET_MONTH = " + as_month + @") AND  
                                                ( ASBUDGET.BUDGET_BF ='" + budget_bf + "') AND ( ASBUDGET.BUDGET_USE = '" + budget_use + @"') AND  
                                                ( ASBUDGET.BUDGET_BAL = '" + budget_bal + @"')";

                Sdt dt_haverow2 = ta.Query(sqlStr);

                if (dt_haverow2.GetRowCount() > 0)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการบันทึกข้อมูลแล้ว");
                }
            }

            ta.Exe(sqlStr);

            //DwMain.Reset();
            //DwAdd.Reset();
            //DwMain.InsertRow(1);
            //DwAdd.InsertRow(1);

        }

        private void Refresh()
        {

        }

    }
}