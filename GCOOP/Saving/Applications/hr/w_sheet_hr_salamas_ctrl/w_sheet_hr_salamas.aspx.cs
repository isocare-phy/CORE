using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using DataLibrary;
using System.Drawing;

namespace Saving.Applications.hr.w_sheet_hr_salamas_ctrl
{
    public partial class w_sheet_hr_salamas : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostTaxcal { get; set; } //สำสั่ง JS postback
        [JsPostBack]
        public string PostRetreive { get; set; } //สำสั่ง JS postback
        [JsPostBack]
        public string PostTrandp { get; set; } //สำสั่ง JS postback

        public void InitJsPostBack()
        {
            dsList.InitDsList(this);
            dsMain.InitDs(this);
            dsTran.InitDs(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.DdApplication();

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostTaxcal)
            {
                GetProcess();
                JsPostRetreive();
            }
            else if (eventArg == PostRetreive)
            {
                JsPostRetreive();
            }
            else if (eventArg == PostTrandp)
            {
                GetTranfer();

            }
        }
        private void setColorDsList()
        {
            Color myRgbColor = new Color();

            myRgbColor = Color.FromArgb(3, 2, 5);
            Color myRgbfontColor = new Color();

            myRgbfontColor = Color.FromArgb(0, 255, 0);
            dsList.FindTextBox(4, "PAYLIST").BackColor = myRgbColor;
            dsList.FindTextBox(10, "PAYLIST").BackColor = myRgbColor;
            dsList.FindTextBox(11, "PAYLIST").BackColor = myRgbColor;
            dsList.FindTextBox(4, "PAYLIST").ForeColor = myRgbfontColor;
            dsList.FindTextBox(10, "PAYLIST").ForeColor = myRgbfontColor;
            dsList.FindTextBox(11, "PAYLIST").ForeColor = myRgbfontColor;

            dsList.FindTextBox(4, "PAYROLL_AMT").BackColor = myRgbColor;
            dsList.FindTextBox(10, "PAYROLL_AMT").BackColor = myRgbColor;
            dsList.FindTextBox(11, "PAYROLL_AMT").BackColor = myRgbColor;
            dsList.FindTextBox(4, "PAYROLL_AMT").ForeColor = myRgbfontColor;
            dsList.FindTextBox(10, "PAYROLL_AMT").ForeColor = myRgbfontColor;
            dsList.FindTextBox(11, "PAYROLL_AMT").ForeColor = myRgbfontColor;
        }
        private void GetTranfer()
        {
            try
            {
                String year_pay = year.Text;
                int year_pay1 = Convert.ToInt16(year_pay);
                int mount_pay = month.SelectedIndex + 1;
                int pay_type1 = pay_type.SelectedIndex + 1;
                String pay_type2 = "0" + pay_type1;
                DateTime pay_date = dsTran.DATA[0].TRAN_DATE;
                string acc_tran = dsTran.DATA[0].TRAN_ACC;


                String sqlCheck = "select * from dpdepttran where coop_id = '" + state.SsCoopId + "' and memcoop_id = '" + state.SsCoopId + "' and system_code = 'DHR' and tran_year = '" + year_pay1 + "' and tran_date  =  to_date('" + pay_date.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy') ";
                Sdt dtt = WebUtil.QuerySdt(sqlCheck);
                int r = dtt.Rows.Count;

                if (r > 0)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เดือนนี้มีการโอนเข้าบัญชีเงินฝากเรียบร้อยแล้ว");
                }
                else
                {
                    ExecuteDataSource exed = new ExecuteDataSource(this);
                    try
                    {
                        string sqlStr = @"select    hrnpayroll.emplid,hrnpayroll.seq_pay,   
                                                            hrnpayroll.payroll_amt,   
                                                            hrnpayroll.tran_acc,
									                        hrnpayroll.tran_date,
                                                            dpdeptmaster.member_no,
                                                            hrnpayroll.pay_year
                                                from hrnpayroll ,dpdeptmaster,hrnmlemplinfodea
                                                where   hrnpayroll.seq_pay='N01'  and  hrnpayroll.tran_acc = dpdeptmaster.deptaccount_no
                                                        and hrnmlemplinfodea.emplid =  hrnpayroll.emplid
                                                        and pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + @"'and hrnmlemplinfodea.paymeth <> '2'
                                                        and  hrnpayroll.emplid = '" + dsList.DATA[0].EMPLID + "'";
                        Sdt dt = WebUtil.QuerySdt(sqlStr);
                        if (dt.Next())
                        {
                            Decimal PAYROLL_AMT = dt.GetDecimal("PAYROLL_AMT");
                            String member_no = dt.GetString("member_no");
                            String Acc_tran = dt.GetString("TRAN_ACC");
                            DateTime paydate = dt.GetDate("TRAN_DATE");
                            String sqlInsertdp = @"insert into dpdepttran 
                                                    values('" + state.SsCoopId + @"'    ,'" + Acc_tran + @"'    ,'" + state.SsCoopId + @"'  ,'" + member_no + @"'   ,'DHR'
                                                          ,'" + dt.GetInt32("pay_year") + @"'   , to_date('" + pay_date.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy')     ,'1'    ,null   ,null
                                                          ,null         ,null       ,'" + PAYROLL_AMT + @"'     ,null       ,null
                                                          ,null         ,'1'        ,null        ,null          ,null       ,'001'
                                                          ,'0'          ,null       ,null        ,null          ,null)";
                            exed.SQL.Add(sqlInsertdp);
                            exed.Execute();
                            LtServerMessage.Text = WebUtil.CompleteMessage("โอนเข้าเงินฝากเรียบร้อยแล้ว");

                        }
                    }
                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex + " โอนเข้าเงินฝากไม่สำเร็จ"); }

                }

            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex + " โอนเข้าเงินฝากไม่สำเร็จ"); }
        }
        private void GetProcess()
        {
            try
            {
                String year_pay = year.Text;
                int year_pay1 = Convert.ToInt16(year_pay) - 543;
                int mount_pay = month.SelectedIndex + 1;
                DateTime pay_date1 = DateTime.ParseExact(pay_date.Text, "dd/MM/yyyy", new CultureInfo("th-TH"), DateTimeStyles.None);
                string emplid1 = getEmplid(dsMain.DATA[0].EMPLCODE);
                int pay_typey = pay_type.SelectedIndex + 1;
                String pay_type1 = "0" + pay_typey;
                ExecuteDataSource exed = new ExecuteDataSource(this);

                String sqlStr = @"DELETE FROM HRNPAYROLL
                                WHERE
                                ( HRNPAYROLL.PAY_YEAR = '" + year_pay + @"' )
                                 AND HRNPAYROLL.emplid = ( select emplid from hrnmlemplfilemas where emplid='" + emplid1 + "' and  empltypeid = '" + pay_type1 + @"' )
                                 and ( HRNPAYROLL.PAY_MONTH = '" + mount_pay + "' )";

                exed.SQL.Add(sqlStr);
                int result = exed.Execute();
                exed.SQL.Clear();
                sqlStr = @" SELECT HRNPAYROLL_SALARY.PAYLIST,   
                            HRNPAYROLL_SET.SET_NO,   
                            HRNPAYROLL_SALARY.SEQ_PAY,   
                            HRNPAYROLL_SET.EMPLID,   
                            HRNPAYROLL_SET.SETPAYROLL_AMT,   
                            HRNPAYROLL_SALARY.SEQ_SORT,   
                            HRNPAYROLL_SALARY.FLAG_SHOW_SET     
                            FROM HRNPAYROLL_SALARY, hrnmlemplfilemas,  
                            HRNPAYROLL_SET  
                            WHERE (HRNPAYROLL_SET.SEQ_PAY =  HRNPAYROLL_SALARY.SEQ_PAY) 
                             AND  HRNPAYROLL_SET.emplid=hrnmlemplfilemas.emplid and
                                    hrnmlemplfilemas.empltypeid = '" + pay_type1 + @"' and hrnmlemplfilemas.emplstat = '1'
                            and HRNPAYROLL_SET.emplid= '" + emplid1 + @"' 
                            ORDER BY HRNPAYROLL_SET.EMPLID ASC,   
                            HRNPAYROLL_SALARY.SEQ_SORT ASC";

                Sdt dt_1 = WebUtil.QuerySdt(sqlStr);

                string bankacct = "";//เลขบัญชี

                string sqlStrbankacct = @"select bankacct from hrnmlemplinfodea  ,hrnmlemplfilemas where hrnmlemplinfodea.emplid= hrnmlemplfilemas.emplid
                                  and hrnmlemplfilemas.emplid = '" + emplid1 + "' ";
                DataTable dt_bankacct = WebUtil.Query(sqlStrbankacct);
                try
                {
                    bankacct = dt_bankacct.Rows[0]["bankacct"].ToString();
                }
                catch { bankacct = "xxxxxxx"; }

                while (dt_1.Next())
                {
                    decimal setpayroll_amt = dt_1.GetDecimal("setpayroll_amt");
                    string emplid = dt_1.GetString("emplid");
                    string seq_pay = dt_1.GetString("seq_pay");
                    sqlStr = @"  INSERT INTO HRNPAYROLL  
                                                                ( PAY_YEAR,   
                                                                PAY_MONTH,   
                                                                SEQ_PAY,   
                                                                EMPLID,   
                                                                INCOMETYPE_ID,   
                                                                PAYROLL_AMT,   
                                                                ENTRY_ID,   
                                                                ENTRY_DATE,TRAN_DATE,TRAN_ACC )  
                                                        VALUES ( " + year_pay + @",   
                                                                " + mount_pay + @",   
                                                                '" + seq_pay + @"',   
                                                                '" + emplid + @"',   
                                                                null,   
                                                                " + setpayroll_amt + @",   
                                                                '" + state.SsUsername + @"',
                                                                to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),to_date('" + pay_date1.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),'" + bankacct + @"'  )";
                    exed.SQL.Add(sqlStr);
                    int resultl = exed.Execute();
                    exed.SQL.Clear();
                }

                //*********update หักชำระหนี้ P18****************
                try
                {
                    string sqlinsertR18 = @"  update hrnpayroll  
                                          set payroll_amt = (select  sum(( kptempreceivedet.item_payment )* (kpucfkeepitemtype.sign_flag ) ) as money 
                                                from kptempreceive,   
                                                    kptempreceivedet,   
                                                    kpucfkeepitemtype ,
                                                    hrnmlemplfilemas
                                                where ( kptempreceive.member_no = kptempreceivedet.member_no ) and  
                                                        ( kptempreceive.recv_period = kptempreceivedet.recv_period ) and  
                                                        ( kptempreceivedet.keepitemtype_code = kpucfkeepitemtype.keepitemtype_code ) and  
                                                        ( kptempreceive.coop_id = kptempreceivedet.coop_id ) and  
                                                        ( kptempreceive.coop_id = kpucfkeepitemtype.coop_id ) and
                                                    ( hrnmlemplfilemas.member_no = kptempreceive.member_no) and
                                                    hrnmlemplfilemas.emplid = {2} ) 
                                          where ( hrnpayroll.pay_year ={0} ) and  
                                                ( hrnpayroll.pay_month =  {1}  ) and  
                                                ( hrnpayroll.seq_pay ='P18'  ) and  
                                                ( hrnpayroll.emplid ={2})";
                    sqlinsertR18 = WebUtil.SQLFormat(sqlinsertR18, year_pay, mount_pay, emplid1);
                    WebUtil.Query(sqlinsertR18);
                }


                catch { }

                decimal sum_R09 = 0, sum_P16 = 0;

                //*********sum รายรับทั้งหมด ไว้ที่ R09****************
                try
                {
                    sqlStr = @"SELECT SUM(  HRNPAYROLL.PAYROLL_AMT  )AS PAYROLL_AMT
                               FROM HRNPAYROLL  
                               WHERE ( HRNPAYROLL.PAY_YEAR =" + year_pay + @"  ) AND  
                                    ( HRNPAYROLL.PAY_MONTH = " + mount_pay + @" ) AND  
                                    ( HRNPAYROLL.SEQ_PAY  in('R01','R02','R04','R11')) AND  
                                    ( HRNPAYROLL.EMPLID ='" + emplid1 + "'  ) ";
                    DataTable dt_4 = WebUtil.Query(sqlStr);
                    try
                    {
                        sum_R09 = Convert.ToDecimal(dt_4.Rows[0]["PAYROLL_AMT"]);
                    }
                    catch { sum_R09 = 0; }

                    string sqlupdateR09 = @"  update hrnpayroll  
                                          set payroll_amt = {3} 
                                          where ( hrnpayroll.pay_year ={0} ) and  
                                                ( hrnpayroll.pay_month =  {1}  ) and  
                                                ( hrnpayroll.seq_pay ='R09'  ) and  
                                                ( hrnpayroll.emplid ={2})";
                    sqlupdateR09 = WebUtil.SQLFormat(sqlupdateR09, year_pay, mount_pay, emplid1, sum_R09);
                    WebUtil.Query(sqlupdateR09);
                }
                catch { }

                //*********sum รายจ่ายทั้งหมด ไว้ที่ P16****************
                try
                {
                    sqlStr = @"  SELECT SUM(  HRNPAYROLL.PAYROLL_AMT  )AS PAYROLL_AMT
                                FROM HRNPAYROLL  
                                WHERE ( HRNPAYROLL.PAY_YEAR =" + year_pay + @"  ) AND  
                                    ( HRNPAYROLL.PAY_MONTH = " + mount_pay + @" ) AND  
                                    ( HRNPAYROLL.SEQ_PAY in('P01','P12','P15','P18','P17')) AND  
                                    ( HRNPAYROLL.EMPLID ='" + emplid1 + "'  ) ";
                    DataTable dt_5 = WebUtil.Query(sqlStr);
                    try
                    {
                        sum_P16 = Convert.ToDecimal(dt_5.Rows[0]["PAYROLL_AMT"]);
                    }
                    catch { sum_P16 = 0; }

                    string sqlupdateP16 = @"  update hrnpayroll  
                                          set payroll_amt = {3} 
                                          where ( hrnpayroll.pay_year ={0} ) and  
                                                ( hrnpayroll.pay_month =  {1}  ) and  
                                                ( hrnpayroll.seq_pay ='P16'  ) and  
                                                ( hrnpayroll.emplid ={2})";
                    sqlupdateP16 = WebUtil.SQLFormat(sqlupdateP16, year_pay, mount_pay, emplid1, sum_P16);
                    WebUtil.Query(sqlupdateP16);
                }
                catch { }
                //*********sum รายรับทั้งหมด-รายจ่ายทั้งหมด ไว้ที่  N01 รายได้สุทธิ ****************
                try
                {
                    decimal p_amt = sum_R09 - sum_P16;
                    string sqlupdateP16 = @"  update hrnpayroll  
                                          set payroll_amt = {3} 
                                          where ( hrnpayroll.pay_year ={0} ) and  
                                                ( hrnpayroll.pay_month =  {1}  ) and  
                                                ( hrnpayroll.seq_pay ='N01'  ) and  
                                                ( hrnpayroll.emplid ={2})";
                    sqlupdateP16 = WebUtil.SQLFormat(sqlupdateP16, year_pay, mount_pay, emplid1, p_amt);
                    WebUtil.Query(sqlupdateP16);
                }
                catch { }

                LtServerMessage.Text = WebUtil.CompleteMessage("ประมวลผลเสร็จเรียบร้อยแล้ว");
            }//end else have row

            //}
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }



        }

        //        private Decimal GetCallTaxMonth(String emplid)
        //        {
        //            String sqlStr;
        //            Sta ta = new Sta(sqlca.ConnectionString);
        //            Sdt dt = new Sdt();
        //            Decimal year_pay = (dw_process.GetItemDecimal(1, "year_pay") - 543);
        //            Decimal month_pay = dw_process.GetItemDecimal(1, "month_pay");
        //            Decimal taxmonth;
        //            sqlStr = @"   SELECT HRNPAYROLL_TAXSET.SEQ_TAX,   
        //                                             HRNPAYROLL_TAXSET.TAXLIST1_AMT  
        //                                        FROM HRNPAYROLL_TAXSET  
        //                                       WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR = " + year_pay + @"  ) AND  
        //                                             ( HRNPAYROLL_TAXSET.PAY_MONTH =" + month_pay + @" ) AND  
        //                                             ( HRNPAYROLL_TAXSET.SEQ_TAX = 'A16' ) AND  
        //                                             ( HRNPAYROLL_TAXSET.EMPLID ='" + emplid + "')   ";
        //            dt = ta.Query(sqlStr);
        //            dt.Next();
        //            taxmonth = Convert.ToDecimal(dt.Rows[0]["TAXLIST1_AMT"]);
        //            ta.Close();
        //            return taxmonth;
        //        }
        private String getEmplid(String emplcode)
        {
            String selectempid = "select emplid from hrnmlemplfilemas where emplcode = '" + emplcode + "'";
            DataTable dtemplid = WebUtil.Query(selectempid);
            String EMPLID = dtemplid.Rows[0]["emplid"].ToString().Trim();
            return EMPLID;
        }



        public void SaveWebSheet()
        {
            try
            {
                ExecuteDataSource exed = new ExecuteDataSource(this);
                Decimal tax_new = 0;
                Decimal totalrecive = 0;
                Decimal P12 = 0;
                Decimal P15 = 0;
                Decimal P18 = 0;
                Decimal other = 0;
                for (int i = 0; i < dsList.RowCount; i++)
                {
                    //Decimal tax_new = 0;
                    //Decimal totalrecive;
                    //Decimal P12 = 0;
                    //Decimal P15 = 0;
                    //Decimal P18 = 0;
                    //Decimal other = 0;
                    string seq_pay = dsList.DATA[i].SEQ_PAY;
                    Decimal payroll_amt = dsList.DATA[i].PAYROLL_AMT;
                    String year_pay = year.Text;
                    string emplid = getEmplid(dsMain.DATA[0].EMPLCODE);
                    int mount_pay = month.SelectedIndex + 1;

                    Decimal totalpay = tax_new + P12 + P15 + P18 + other;
                    Decimal totalsuti = totalrecive - totalpay;
                    switch (seq_pay)
                    {
                        case "R09":
                            totalrecive += payroll_amt;
                            break;
                        case "P18":
                            P18 += payroll_amt;
                            break;
                        case "P17":
                            other += payroll_amt;
                            break;
                        case "P01":
                            String sqlupdateP01 = "update hrnpayroll set payroll_amt = '" + payroll_amt + "' where pay_year ='" + year_pay + @"'
                                         and seq_pay = '" + seq_pay + "' and emplid = '" + emplid + "' and pay_month = '" + mount_pay + "'";
                            exed.SQL.Add(sqlupdateP01);
                            exed.Execute();
                            tax_new += payroll_amt;
                            break;
                        case "P12":
                            String sqlupdateP12 = "update hrnpayroll set payroll_amt = '" + payroll_amt + "' where pay_year ='" + year_pay + @"'
                                         and seq_pay = '" + seq_pay + "' and emplid = '" + emplid + "' and pay_month = '" + mount_pay + "'";
                            exed.SQL.Add(sqlupdateP12);
                            exed.Execute();
                            P12 += payroll_amt;
                            break;
                        case "P15":
                            String sqlupdateP15 = "update hrnpayroll set payroll_amt = '" + payroll_amt + "' where pay_year ='" + year_pay + @"'
                                         and seq_pay = '" + seq_pay + "' and emplid = '" + emplid + "' and pay_month = '" + mount_pay + "'";
                            exed.SQL.Add(sqlupdateP15);
                            exed.Execute();
                            P15 += payroll_amt;
                            break;
                        case "P16":
                            String sqlupdateP16 = "update hrnpayroll set payroll_amt = '" + totalpay + "' where pay_year ='" + year_pay + @"'
                                         and seq_pay = '" + seq_pay + "' and emplid = '" + emplid + "' and pay_month = '" + mount_pay + "'";
                            exed.SQL.Add(sqlupdateP16);
                            exed.Execute();
                            dsList.DATA[i].PAYROLL_AMT = totalpay;
                            break;
                        case "N01":
                            String sqlupdateN01 = "update hrnpayroll set payroll_amt = '" + totalsuti + "' where pay_year ='" + year_pay + @"'
                                         and seq_pay = '" + seq_pay + "' and emplid = '" + emplid + "' and pay_month = '" + mount_pay + "'";
                            exed.SQL.Add(sqlupdateN01);
                            exed.Execute();
                            dsList.DATA[i].PAYROLL_AMT = totalsuti;
                            break;
                    }

                }


                //                Decimal tax_new = dsList.DATA[5].PAYROLL_AMT;
                //                Decimal totalrecive = dsList.DATA[4].PAYROLL_AMT;
                //                Decimal P12 = dsList.DATA[6].PAYROLL_AMT;
                //                Decimal P15 = dsList.DATA[7].PAYROLL_AMT;
                //                Decimal P18 = dsList.DATA[8].PAYROLL_AMT;
                //                Decimal other = dsList.DATA[9].PAYROLL_AMT;
                //                Decimal totalpay = tax_new + P12 + P15 + P18 + other;
                //                Decimal totalsuti = totalrecive - totalpay;
                //                String year_pay = year.Text;
                //                string emplid = getEmplid(dsMain.DATA[0].EMPLCODE);
                //                int mount_pay = month.SelectedIndex + 1;
                //                String sqlupdatetax = "update hrnpayroll set payroll_amt = '" + tax_new + "' where pay_year ='" + year_pay + @"'
                //                                         and seq_pay = 'P01' and emplid = '" + emplid + "' and pay_month = '" + mount_pay + "'";
                //                String sqlupdateP12 = "update hrnpayroll set payroll_amt = '" + P12 + "' where pay_year ='" + year_pay + @"'
                //                                         and seq_pay = 'P12' and emplid = '" + emplid + "' and pay_month = '" + mount_pay + "'";
                //                String sqlupdateP15 = "update hrnpayroll set payroll_amt = '" + P15 + "' where pay_year ='" + year_pay + @"'
                //                                         and seq_pay = 'P15' and emplid = '" + emplid + "' and pay_month = '" + mount_pay + "'";

                //                String sqlupdatetotal = "update hrnpayroll set payroll_amt = '" + totalpay + "' where pay_year ='" + year_pay + @"'
                //                                         and seq_pay = 'P16' and emplid = '" + emplid + "' and pay_month = '" + mount_pay + "'";
                //                String sqlupdatetotalsuti = "update hrnpayroll set payroll_amt = '" + totalsuti + "' where pay_year ='" + year_pay + @"'
                //                                         and seq_pay = 'N01' and emplid = '" + emplid + "' and pay_month = '" + mount_pay + "'";
                //                exed.SQL.Add(sqlupdatetax);
                //                exed.SQL.Add(sqlupdateP12);
                //                exed.SQL.Add(sqlupdateP15);
                //                exed.SQL.Add(sqlupdatetotal);
                //                exed.SQL.Add(sqlupdatetotalsuti);

                //                int result = exed.Execute();

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                //dsList.DATA[10].PAYROLL_AMT = totalpay;
                //dsList.DATA[11].PAYROLL_AMT = totalsuti;

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }

        }

        public void WebSheetLoadEnd()
        {
        }

        private void JsPostRetreive()
        {
            String year_pay = year.Text;
            int mount_pay = month.SelectedIndex;
            string emplcode = getEmplid(dsMain.DATA[0].EMPLCODE);
            int pay_type1 = pay_type.SelectedIndex;
            dsList.retrieve(year_pay, emplcode, mount_pay, pay_type1);
            dsTran.Retrivetran(year_pay, emplcode, mount_pay);
            setColorDsList();
        }

    }

}