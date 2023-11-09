﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.mbshr.ws_mbshr_adt_mbmoney_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.MBMEMBMONEYTR_LISTDataTable DATA { get; set; }
        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.MBMEMBMONEYTR_LIST;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
            this.Button.Add("b_del");
        }
        public void RetrieveList(string memeber_no)
        {
            String sql = @"  
                  SELECT MBMEMBMONEYTR.MEMBER_NO,   
                         MBMEMBMONEYTR.TRTYPE_CODE,   
                         MBMEMBMONEYTR.MONEYTYPE_CODE,   
                         MBMEMBMONEYTR.BANK_CODE,   
                         MBMEMBMONEYTR.BANK_BRANCH,   
                         MBMEMBMONEYTR.BANK_ACCID,   
                         MBMEMBMONEYTR.COOP_ID,   
                         MBUCFMONEYTRTYPE.TRTYPE_DESC,   
                         MBUCFMONEYTRTYPE.SIGN_FLAG,   
                         CMUCFBANKBRANCH.BRANCH_NAME  
                    FROM MBMEMBMONEYTR,   
                         MBUCFMONEYTRTYPE,   
                         CMUCFBANKBRANCH  
                   WHERE ( mbmembmoneytr.bank_code = cmucfbankbranch.bank_code (+)) and  
                         ( mbmembmoneytr.bank_branch = cmucfbankbranch.branch_id (+)) and  
                         (trim(MBMEMBMONEYTR.TRTYPE_CODE) = trim(MBUCFMONEYTRTYPE.TRTYPE_CODE)) and  
                         ( ( MBMEMBMONEYTR.MEMBER_NO = {0} ) AND  
                         ( MBMEMBMONEYTR.COOP_ID = {1} ) ) AND MBMEMBMONEYTR.TRTYPE_CODE NOT IN('KEEP1','DVAV1')";
            sql = WebUtil.SQLFormat(sql, memeber_no, state.SsCoopId);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
            this.DdMoneytrtype();
            this.DdMoneytype();
            this.DdBank();
        }
        public void DdMoneytrtype()
        {
            string sql = @"
               SELECT TRTYPE_CODE,   
                      TRTYPE_DESC,
                      TRTYPE_CODE||' - '||TRTYPE_DESC as display,
                      SIGN_FLAG  ,
                      1 as sorter
                 FROM MBUCFMONEYTRTYPE  where TRTYPE_CODE not in('KEEP1','DVAV1')
                 union 
                 select '00','--เลือกประเภทรายการ--','',0,0 from dual order by sorter,TRTYPE_CODE";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "trtype_code", "display", "TRTYPE_CODE");
        }
        public void DdMoneytype()
        {
            string sql = @"
                SELECT MONEYTYPE_CODE,   
                       MONEYTYPE_DESC,   
                       MONEYTYPE_CODE||' - '||MONEYTYPE_DESC as display,
                       SORT_ORDER  ,
                       1 as sorter
                  FROM CMUCFMONEYTYPE
                    union 
                    select '00','--เลือกรายการ--','',0,0 from dual order by sorter,moneytype_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "moneytype_code", "display", "MONEYTYPE_CODE");
        }
        public void DdBank()
        {
            string sql = @"
                  SELECT BANK_CODE,   
                         BANK_CODE || ' - ' || BANK_DESC as BANK_DESC,   
                         EDIT_FORMAT  ,
                         1 as sorter
                    FROM CMUCFBANK   
                    union 
                    select '00','--เลือกธนาคาร--','',0 from dual order by sorter,bank_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "bank_code", "BANK_DESC", "BANK_CODE");
        }
    }
}