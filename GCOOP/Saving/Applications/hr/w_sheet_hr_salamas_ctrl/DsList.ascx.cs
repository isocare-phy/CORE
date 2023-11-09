using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.hr.w_sheet_hr_salamas_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRNPAYROLLDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRNPAYROLL;

            this.EventClicked = "OnDsListClicked";
            this.EventItemChanged = "OnDsListItemChanged";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
        }

        public void retrieve(String year, String emplcode, int month, int pay_type)
        {
            int year_pay = Convert.ToInt16(year) - 543;
            int month_pay = month + 1;
            int pay_type1 = pay_type + 1;
            string sql = @"     
                                  
                         SELECT HRNPAYROLL_SALARY.PAYLIST,   
                                HRNPAYROLL.PAY_YEAR,   
                                HRNPAYROLL.PAY_MONTH,   
                                HRNPAYROLL.SEQ_PAY,   
                                HRNPAYROLL.EMPLID,   
                                HRNPAYROLL.INCOMETYPE_ID,   
                                HRNPAYROLL.PAYROLL_AMT,   
                                HRNUCFINCOME_TYPE.INCOMETPYE_NAM,   
                                HRNPAYROLL_SALARY.SEQ_SORT,   
                                HRNPAYROLL.TRAN_DATE,   
                                HRNPAYROLL.TRAN_ACC,
                                HRNPAYROLL.ENTRY_DATE
                                FROM HRNPAYROLL_SALARY,   
                                HRNPAYROLL,   
                                HRNUCFINCOME_TYPE,
                                hrnmlemplfilemas  
                        WHERE ( HRNPAYROLL.SEQ_PAY = HRNPAYROLL_SALARY.SEQ_PAY ) and 
                                ( hrnmlemplfilemas.emplid = HRNPAYROLL.emplid ) and
                                    ( HRNPAYROLL.SEQ_PAY in('R01','R04','R11','R02','R09','P01','P12','P15','P18','P17','P16','N01')) and
                                ( hrnucfincome_type.incometype_id(+) = hrnpayroll.incometype_id) AND  
                                ( hrnmlemplfilemas.emplid = '" + emplcode + @"' ) AND 
              
                                HRNPAYROLL.PAY_YEAR = '" + year + @"' AND  
                                HRNPAYROLL.PAY_MONTH = '" + month_pay + @"'   
                       ORDER BY    HRNPAYROLL_SALARY.SEQ_SORT ASC

 ";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}