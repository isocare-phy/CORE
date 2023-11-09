using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using System.Data;

namespace Saving.Applications.hr.w_sheet_hr_tax_ctrl
{
    public partial class w_sheet_hr_tax : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostProvinceId { get; set; }
        [JsPostBack]
        public string PostDeleteRow { get; set; }
        [JsPostBack]
        public string PostTaxcal { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDs(this);
            dsTaxcal.InitDs(this);
            dsDiscount.InitDs(this);
            dsTax.InitDsList(this);
            dsTaxsum.InitDs(this);
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

            if (eventArg == PostProvinceId)
            {

                String year_pay = year.Text;
                int mount_pay = 1;
                string emplcode = dsMain.DATA[0].EMPLCODE;
                //LtServerMessage.Text = WebUtil.ErrorMessage(mount+" "+year_pay);
                dsTaxcal.RetreiveEmplid(emplcode, year_pay, mount_pay);
                dsDiscount.RetreiveEmplid(emplcode, year_pay, mount_pay);
                dsDiscount.DATA[0].A04 = 30000;
                dsTax.retrieve(year_pay);
                dsTaxcal.DATA[0].A02 = dsDiscount.DATA[0].A02;
                //setSocise();
                dsTaxcal.DATA[0].SALARY = dsTaxcal.DATA[0].SALARY * 12;
                setTotall(year_pay);


            }
            else if (eventArg == PostDeleteRow)
            {
                //int rowDel = dsDetail.GetRowFocus();
                //dsDetail.DeleteRow(rowDel);
            }
            else if (eventArg == PostTaxcal)
            {
                String year_pay = year.Text;
                setTotall(year_pay);

            }
        }
        private void setSalary40()
        {
            Decimal totalsalary40 = dsTaxcal.DATA[0].COMPUTE_1 * Convert.ToDecimal(0.4);

            if (totalsalary40 > 60000)
            {
                dsDiscount.DATA[0].A03 = 60000;
            }
            else dsDiscount.DATA[0].A03 = totalsalary40;


        }

        private void setSocise()
        {
            Decimal P12, P15, soscise_amt = 0, provident_amt = 0;
            String sqlStrP12_set = @"select setpayroll_amt from hrnpayroll_set where emplid = '" + getEmplid(dsMain.DATA[0].EMPLCODE.Trim()) + "' and seq_pay ='P12'";
            DataTable dt_P12_set = WebUtil.Query(sqlStrP12_set);
            try
            {
                soscise_amt = Convert.ToDecimal(dt_P12_set.Rows[0]["setpayroll_amt"]) * 12;
            }
            catch
            {
                String sqlStrP12 = @"select socisepercent from HRNMLEMPLINFODEA where emplid = '" + getEmplid(dsMain.DATA[0].EMPLCODE.Trim()) + "' ";
                DataTable dt_P12 = WebUtil.Query(sqlStrP12);
                try
                {
                    P12 = Convert.ToDecimal(dt_P12.Rows[0]["socisepercent"]);
                }
                catch { P12 = 0; }
                if (dsTaxcal.DATA[0].COMPUTE_1 >= 15000) { soscise_amt = (15000 * P12) * 12; }
                else
                {

                    soscise_amt = (dsTaxcal.DATA[0].COMPUTE_1 * P12) * 12;
                }
            }
            String sqlStrP15_set = @"select setpayroll_amt from hrnpayroll_set where emplid = '" + getEmplid(dsMain.DATA[0].EMPLCODE.Trim()) + "' and seq_pay ='P15'";
            DataTable dt_P15_set = WebUtil.Query(sqlStrP15_set);
            try
            {
                provident_amt = Convert.ToDecimal(dt_P15_set.Rows[0]["setpayroll_amt"]) * 12;
            }
            catch
            {
                String sqlStrP15 = @"select emplprovident from HRNMLEMPLINFODEA where emplid = '" + getEmplid(dsMain.DATA[0].EMPLCODE.Trim()) + "' ";
                DataTable dt_P15 = WebUtil.Query(sqlStrP15);
                try
                {

                    P15 = Convert.ToDecimal(dt_P15.Rows[0]["emplprovident"]);
                }
                catch { P15 = 0; }

                provident_amt = (dsTaxcal.DATA[0].COMPUTE_1 * P15) * 12;
            }
            dsDiscount.DATA[0].A13 = soscise_amt;
            dsDiscount.DATA[0].A14 = provident_amt;
        }
        private void setTotall(String year)
        {

            dsTaxcal.DATA[0].COMPUTE_1 = dsTaxcal.DATA[0].A02 + dsTaxcal.DATA[0].SALARY + dsTaxcal.DATA[0].BONUS;
            setSalary40();

            dsDiscount.DATA[0].COMPUTE_1 = dsDiscount.DATA[0].A13 + dsDiscount.DATA[0].A14 + dsDiscount.DATA[0].A03 + dsDiscount.DATA[0].A04 + dsDiscount.DATA[0].A05 + dsDiscount.DATA[0].A06 + dsDiscount.DATA[0].A07 + dsDiscount.DATA[0].A08 + dsDiscount.DATA[0].A09 + dsDiscount.DATA[0].A10 + dsDiscount.DATA[0].A11 + dsDiscount.DATA[0].A12 + dsDiscount.DATA[0].A21 + dsDiscount.DATA[0].A22 + dsDiscount.DATA[0].A23;
            dsDiscount.DATA[0].COMPUTE_2 = dsTaxcal.DATA[0].COMPUTE_1 - dsDiscount.DATA[0].COMPUTE_1;

            GetCalTax(year);
            setTaxsum();
        }
        public void Totalall()
        {
            Decimal totalrecive = dsTaxcal.DATA[0].COMPUTE_1;
            Decimal totaldiscount = dsDiscount.DATA[0].COMPUTE_1;
            Decimal totalall = totalrecive - totaldiscount;
            dsDiscount.DATA[0].COMPUTE_2 = totalall;
        }
        private void setTaxsum()
        {
            Decimal taxsum = dsTax.DATA[1].Taxcall + dsTax.DATA[2].Taxcall + dsTax.DATA[3].Taxcall + dsTax.DATA[4].Taxcall + dsTax.DATA[5].Taxcall + dsTax.DATA[6].Taxcall + dsTax.DATA[7].Taxcall;
            Decimal taxmonth = taxsum / 12;
            dsTaxsum.DATA[0].TAXSUM = taxsum;
            dsTaxsum.DATA[0].TAXMONTH = taxmonth;
        }
        private void GetCalTax(String year)
        {

            ///////////////////ตารางภาษี/////////////////////////////
            String sqlStr;
            //Sta ta = new Sta(sqlca.ConnectionString);

            sqlStr = @" SELECT HRNUCFTAX_RATE.MIN_RATE,   
                                             HRNUCFTAX_RATE.MAX_RATE,   
                                             HRNUCFTAX_RATE.TAXPER,   
                                             HRNUCFTAX_RATE.GOSS_RATE  
                                        FROM HRNUCFTAX_RATE  
                                         where taxyear='" + year + @"'
                                    ORDER BY HRNUCFTAX_RATE.TAXPER ASC";
            DataTable dt = WebUtil.Query(sqlStr);


            int row_rate = dt.Rows.Count;
            Decimal[] min_rate = new Decimal[row_rate];
            Decimal[] max_rate1 = new Decimal[row_rate];
            Decimal[] taxper1 = new Decimal[row_rate];
            Decimal[] goss_rate1 = new Decimal[row_rate];
            // ArrayList tax_rate = new ArrayList();
            Decimal totalall = dsDiscount.DATA[0].COMPUTE_2;
            for (int t = 0; t < row_rate; t++)
            {
                try
                {


                    min_rate[t] = Convert.ToDecimal(dt.Rows[t]["MIN_RATE"]);
                    max_rate1[t] = Convert.ToDecimal(dt.Rows[t]["MAX_RATE"]);
                    taxper1[t] = Convert.ToDecimal(dt.Rows[t]["TAXPER"]);
                    goss_rate1[t] = Convert.ToDecimal(dt.Rows[t]["GOSS_RATE"]);

                    //if (totalall >= min_rate && totalall <= max_rate1)
                    //{
                    //    decimal taxyear = (totalall - max_rate1 + 1) * (taxper1 / 100);
                    //    dsTax.DATA[t].Taxcall = taxyear;
                    //    break;
                    //}
                    //else
                    //{

                    //}


                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }

            }
            if (totalall >= min_rate[0] && totalall <= max_rate1[0])
            {
                dsTax.DATA[1].Taxcall = 0;
                dsTax.DATA[2].Taxcall = 0;
                dsTax.DATA[3].Taxcall = 0;
                dsTax.DATA[4].Taxcall = 0;
                dsTax.DATA[5].Taxcall = 0;
                dsTax.DATA[6].Taxcall = 0;
                dsTax.DATA[7].Taxcall = 0;
            }
            else if (totalall >= min_rate[1])
            {
                Decimal taxyear_amt = totalall - goss_rate1[0];
                if (taxyear_amt > 150000)
                {
                    Decimal taxyear1 = 150000 * (taxper1[1] / 100);
                    dsTax.DATA[1].Taxcall = taxyear1;
                    Decimal taxyear_amt1 = taxyear_amt - goss_rate1[1];
                    if (taxyear_amt1 > 200000)
                    {
                        Decimal taxyear2 = 200000 * (taxper1[2] / 100);
                        dsTax.DATA[2].Taxcall = taxyear2;
                        Decimal taxyear_amt2 = taxyear_amt1 - goss_rate1[2];
                        if (taxyear_amt2 > 250000)
                        {
                            Decimal taxyear3 = 250000 * (taxper1[3] / 100);
                            dsTax.DATA[3].Taxcall = taxyear3;
                            Decimal taxyear_amt3 = taxyear_amt2 - goss_rate1[3];
                            if (taxyear_amt3 > 250000)
                            {
                                Decimal taxyear4 = 250000 * (taxper1[4] / 100);
                                dsTax.DATA[4].Taxcall = taxyear4;
                                Decimal taxyear_amt4 = taxyear_amt3 - goss_rate1[4];
                                if (taxyear_amt4 > 1000000)
                                {
                                    Decimal taxyear5 = 1000000 * (taxper1[5] / 100);
                                    dsTax.DATA[5].Taxcall = taxyear5;
                                    Decimal taxyear_amt5 = taxyear_amt4 - goss_rate1[5];
                                    if (taxyear_amt5 > 2000000)
                                    {

                                        Decimal taxyear6 = 2000000 * (taxper1[6] / 100);
                                        dsTax.DATA[6].Taxcall = taxyear6;
                                        Decimal taxyear_amt6 = taxyear_amt5 - goss_rate1[6];
                                        if (taxyear_amt6 > 0)
                                        {
                                            Decimal taxyear7 = taxyear_amt6 * (taxper1[7] / 100);
                                            dsTax.DATA[7].Taxcall = taxyear7;

                                        }
                                    }
                                    else
                                    {

                                        Decimal taxyear6 = taxyear_amt5 * (taxper1[6] / 100);
                                        dsTax.DATA[6].Taxcall = taxyear6;
                                        dsTax.DATA[7].Taxcall = 0;
                                    }
                                }
                                else
                                {

                                    Decimal taxyear5 = taxyear_amt4 * (taxper1[5] / 100);
                                    dsTax.DATA[5].Taxcall = taxyear5;
                                    dsTax.DATA[6].Taxcall = 0;
                                    dsTax.DATA[7].Taxcall = 0;
                                }
                            }
                            else
                            {
                                Decimal taxyear4 = taxyear_amt3 * (taxper1[4] / 100);
                                dsTax.DATA[4].Taxcall = taxyear3;
                                dsTax.DATA[5].Taxcall = 0;
                                dsTax.DATA[6].Taxcall = 0;
                                dsTax.DATA[7].Taxcall = 0;
                            }
                        }
                        else
                        {
                            Decimal taxyear3 = taxyear_amt2 * (taxper1[3] / 100);
                            dsTax.DATA[3].Taxcall = taxyear3;
                            dsTax.DATA[4].Taxcall = 0;
                            dsTax.DATA[5].Taxcall = 0;
                            dsTax.DATA[6].Taxcall = 0;
                            dsTax.DATA[7].Taxcall = 0;
                        }
                    }
                    else
                    {
                        Decimal taxyear2 = taxyear_amt1 * (taxper1[2] / 100);
                        dsTax.DATA[2].Taxcall = taxyear2;
                        dsTax.DATA[3].Taxcall = 0;
                        dsTax.DATA[4].Taxcall = 0;
                        dsTax.DATA[5].Taxcall = 0;
                        dsTax.DATA[6].Taxcall = 0;
                        dsTax.DATA[7].Taxcall = 0;
                    }
                }
                else
                {
                    Decimal taxyear1 = taxyear_amt * (taxper1[1] / 100);
                    dsTax.DATA[1].Taxcall = taxyear1;
                    dsTax.DATA[2].Taxcall = 0;
                    dsTax.DATA[3].Taxcall = 0;
                    dsTax.DATA[4].Taxcall = 0;
                    dsTax.DATA[5].Taxcall = 0;
                    dsTax.DATA[6].Taxcall = 0;
                    dsTax.DATA[7].Taxcall = 0;
                }


            }
        }
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
                String year_pay = year.Text;
                int year_pay1 = Convert.ToInt16(year_pay) - 543;
                int mount_pay = 1;
                string EMPLCODE = dsMain.DATA[0].EMPLCODE;
                String EMPLID = getEmplid(EMPLCODE);
                String selectsql = "select * from hrnpayroll_taxset where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "'";
                DataTable dt = WebUtil.Query(selectsql);


                int row_select = dt.Rows.Count;
                if (row_select > 0)
                {
                    String sqlupdate = "update hrnpayroll_taxset set taxlist1_amt = '" + dsTaxcal.DATA[0].SALARY + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A01'";
                    String sqlupdate2 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsTaxcal.DATA[0].A02 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A02'";
                    String sqlupdate3 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A03 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A03'";
                    String sqlupdate4 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A04 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A04'";
                    String sqlupdate5 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A05 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A05'";
                    String sqlupdate6 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A06 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A06'";
                    String sqlupdate7 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A07 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A07'";
                    String sqlupdate8 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A08 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A08'";
                    String sqlupdate9 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A09 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A09'";
                    String sqlupdate10 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A10 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A10'";
                    String sqlupdate11 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A11 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A11'";
                    String sqlupdate12 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A12 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A12'";
                    String sqlupdate13 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A13 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A13'";
                    String sqlupdate14 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A14 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A14'";
                    String sqlupdate15 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsTaxsum.DATA[0].TAXSUM + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A15'";
                    String sqlupdate16 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsTaxsum.DATA[0].TAXMONTH + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A16'";
                    String sqlupdate21 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A21 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A21'";
                    String sqlupdate22 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A22 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A22'";
                    String sqlupdate23 = "update hrnpayroll_taxset set taxlist1_amt = '" + dsDiscount.DATA[0].A23 + "' where pay_year = '" + year_pay1 + "' and pay_month = '" + mount_pay + "' and emplid ='" + EMPLID + "' and seq_tax = 'A23'";
                    exed.SQL.Add(sqlupdate);
                    exed.SQL.Add(sqlupdate2);
                    exed.SQL.Add(sqlupdate3);
                    exed.SQL.Add(sqlupdate4);
                    exed.SQL.Add(sqlupdate5);
                    exed.SQL.Add(sqlupdate6);
                    exed.SQL.Add(sqlupdate7);
                    exed.SQL.Add(sqlupdate8);
                    exed.SQL.Add(sqlupdate9);
                    exed.SQL.Add(sqlupdate10);
                    exed.SQL.Add(sqlupdate11);
                    exed.SQL.Add(sqlupdate12);
                    exed.SQL.Add(sqlupdate13);
                    exed.SQL.Add(sqlupdate14);
                    exed.SQL.Add(sqlupdate15);
                    exed.SQL.Add(sqlupdate16);
                    exed.SQL.Add(sqlupdate21);
                    exed.SQL.Add(sqlupdate22);
                    exed.SQL.Add(sqlupdate23);

                }
                else
                {
                    String selectsqltaxlist = "select taxlist from hrnpayroll_taxcfg where seq_tax  in ('A01','A02','A03','A04','A05' ,'A06' , 'A07' , 'A08' , 'A09','A10','A11','A12','A13','A14','A15','A16','A21','A22','A23')";
                    DataTable dtt = WebUtil.Query(selectsqltaxlist);


                    int row_selecttax = dtt.Rows.Count;

                    String insertsql = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values('" + year_pay1 + "','" + mount_pay + "','A01','" + EMPLID + "','" + dtt.Rows[0]["taxlist"] + "','" + dsTaxcal.DATA[0].SALARY + @"','0')  ";



                    String insertsql2 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                ('" + year_pay1 + "','" + mount_pay + "','A02','" + EMPLID + "','" + dtt.Rows[1]["taxlist"] + "','" + dsTaxcal.DATA[0].A02 + @"','0')";

                    String insertsql3 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                 ('" + year_pay1 + "','" + mount_pay + "','A03','" + EMPLID + "','" + dtt.Rows[2]["taxlist"] + "','" + dsDiscount.DATA[0].A03 + @"','0')";

                    String insertsql4 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                ('" + year_pay1 + "','" + mount_pay + "','A04','" + EMPLID + "','" + dtt.Rows[3]["taxlist"] + "','" + dsDiscount.DATA[0].A04 + @"','0')";
                    String insertsql5 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                  ('" + year_pay1 + "','" + mount_pay + "','A05','" + EMPLID + "','" + dtt.Rows[4]["taxlist"] + "','" + dsDiscount.DATA[0].A05 + @"','0')";
                    String insertsql6 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                 ('" + year_pay1 + "','" + mount_pay + "','A06','" + EMPLID + "','" + dtt.Rows[5]["taxlist"] + "','" + dsDiscount.DATA[0].A06 + @"','0')";
                    String insertsql7 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                 ('" + year_pay1 + "','" + mount_pay + "','A07','" + EMPLID + "','" + dtt.Rows[6]["taxlist"] + "','" + dsDiscount.DATA[0].A07 + @"','0')";
                    String insertsql8 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                 ('" + year_pay1 + "','" + mount_pay + "','A08','" + EMPLID + "','" + dtt.Rows[7]["taxlist"] + "','" + dsDiscount.DATA[0].A08 + @"','0')";
                    String insertsql9 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                  ('" + year_pay1 + "','" + mount_pay + "','A09','" + EMPLID + "','" + dtt.Rows[8]["taxlist"] + "','" + dsDiscount.DATA[0].A09 + @"','0')";
                    String insertsql10 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                ('" + year_pay1 + "','" + mount_pay + "','A10','" + EMPLID + "','" + dtt.Rows[9]["taxlist"] + "','" + dsDiscount.DATA[0].A10 + @"','0')";
                    String insertsql11 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                 ('" + year_pay1 + "','" + mount_pay + "','A11','" + EMPLID + "','" + dtt.Rows[10]["taxlist"] + "','" + dsDiscount.DATA[0].A11 + @"','0')";
                    String insertsql12 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                 ('" + year_pay1 + "','" + mount_pay + "','A12','" + EMPLID + "','" + dtt.Rows[11]["taxlist"] + "','" + dsDiscount.DATA[0].A12 + @"','0')";
                    String insertsql13 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                 ('" + year_pay1 + "','" + mount_pay + "','A13','" + EMPLID + "','" + dtt.Rows[12]["taxlist"] + "','" + dsDiscount.DATA[0].A13 + @"','0')";
                    String insertsql14 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                ('" + year_pay1 + "','" + mount_pay + "','A14','" + EMPLID + "','" + dtt.Rows[13]["taxlist"] + "','" + dsDiscount.DATA[0].A14 + @"','0')";

                    String insertsql15 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                ('" + year_pay1 + "','" + mount_pay + "','A15','" + EMPLID + "','" + dtt.Rows[14]["taxlist"] + "','" + dsTaxsum.DATA[0].TAXSUM + @"','0')";

                    String insertsql16 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                ('" + year_pay1 + "','" + mount_pay + "','A16','" + EMPLID + "','" + dtt.Rows[15]["taxlist"] + "','" + dsTaxsum.DATA[0].TAXMONTH + @"','0')";

                    String insertsql21 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                ('" + year_pay1 + "','" + mount_pay + "','A21','" + EMPLID + "','" + dtt.Rows[16]["taxlist"] + "','" + dsDiscount.DATA[0].A21 + @"','0')";

                    String insertsql22 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                ('" + year_pay1 + "','" + mount_pay + "','A22','" + EMPLID + "','" + dtt.Rows[17]["taxlist"] + "','" + dsDiscount.DATA[0].A22 + @"','0')";

                    String insertsql23 = @"insert into hrnpayroll_taxset(pay_year,pay_month,seq_tax,emplid,taxlist,taxlist1_amt,taxlist2_amt)
                                         values
                                                ('" + year_pay1 + "','" + mount_pay + "','A23','" + EMPLID + "','" + dtt.Rows[18]["taxlist"] + "','" + dsDiscount.DATA[0].A23 + @"','0')";

                    exed.SQL.Add(insertsql);
                    exed.SQL.Add(insertsql2);
                    exed.SQL.Add(insertsql3);
                    exed.SQL.Add(insertsql4);
                    exed.SQL.Add(insertsql5);
                    exed.SQL.Add(insertsql6);

                    exed.SQL.Add(insertsql7);
                    exed.SQL.Add(insertsql8);

                    exed.SQL.Add(insertsql9);
                    exed.SQL.Add(insertsql10);
                    exed.SQL.Add(insertsql11);
                    exed.SQL.Add(insertsql12);
                    exed.SQL.Add(insertsql13);
                    exed.SQL.Add(insertsql14);
                    exed.SQL.Add(insertsql15);
                    exed.SQL.Add(insertsql16);
                    exed.SQL.Add(insertsql21);
                    exed.SQL.Add(insertsql22);
                    exed.SQL.Add(insertsql23);

                }
                int result = exed.Execute();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
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