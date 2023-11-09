using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.shrlon.ws_sl_slip_pay_mth_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.SLSLIPPAYINDataTable DATA { get; set; }
        public string IsShow = "visible";

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            css2.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.SLSLIPPAYIN;           
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.Register();
            this.Button.Add("b_memsearch");

            this.Button.Add("b_ref");
        }

        public void DdSliptype()
        {
            string sql = @" 
              	     SELECT   trim(SLUCFSLIPTYPE.SLIPTYPE_CODE) AS SLIPTYPE_CODE,
                    SLUCFSLIPTYPE.SLIPTYPE_DESC,
                    SLUCFSLIPTYPE.SLIPTYPE_SORT,
                    SLUCFSLIPTYPE.SLIPTYPE_CODE || ' - ' || SLUCFSLIPTYPE.SLIPTYPE_DESC as SLTYPE_DISPLAY,
                    1 as sorter 
                FROM SLUCFSLIPTYPE  
                WHERE slucfsliptype.slipmanual_flag = 1  
                union
                select '','',0,'', 0 from dual order by sorter, SLIPTYPE_SORT 
            ";

            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "SLIPTYPE_CODE", "SLTYPE_DISPLAY", "SLIPTYPE_CODE");
        }

        public void DdMoneyType()
        {
            string sql = @" 
                   SELECT MONEYTYPE_CODE,   
                          MONEYTYPE_CODE || ' - '|| MONEYTYPE_DESC as MONEYTYPE_DISPLAY,1 as sorter
                   FROM CMUCFMONEYTYPE where MONEYTYPE_STATUS = 'DAY'
                   union
                   select '','', 0 from dual order by sorter, MONEYTYPE_CODE
            ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "MONEYTYPE_CODE", "MONEYTYPE_DISPLAY", "MONEYTYPE_CODE");
        }

        public void DdFromAccId(string sliptype_code, string moneytype_code)
        {
            string sql = @"                         
                SELECT 
                        CMUCFTOFROMACCID.MONEYTYPE_CODE, 
			             CMUCFTOFROMACCID.ACCOUNT_ID,    
			             CMUCFTOFROMACCID.ACCOUNT_ID ||'-'||ACCMASTER.ACCOUNT_NAME AS fromacc_display,
                  		 ACCMASTER.ACCOUNT_NAME  ,  
                         1 as sorter
                 FROM ACCMASTER,   
        		      CMUCFTOFROMACCID  
                 WHERE 
		              ( ACCMASTER.COOP_ID = CMUCFTOFROMACCID.COOP_ID )  
                 and  (CMUCFTOFROMACCID.ACCOUNT_ID =ACCMASTER.ACCOUNT_ID)
                 and (CMUCFTOFROMACCID.SLIPTYPE_CODE = '" + sliptype_code + "') and (CMUCFTOFROMACCID.MONEYTYPE_CODE = '" + moneytype_code + @"')
                union select '','','','',0 from dual order by sorter
            ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "tofrom_accid", "fromacc_display", "account_id");
        }

        public void DdTofromAccBlank()
        {
            string sql = "select '' as fromacc_display,'' as account_id from dual";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "tofrom_accid", "fromacc_display", "account_id");
        }

        public void RetrieveMain(string payinslip_no)
        {
            String sql = @"SELECT SLSLIPPAYIN.MEMBGROUP_CODE,   
                         SLSLIPPAYIN.COOP_ID,  
                         SLSLIPPAYIN.DOCUMENT_NO,  
                         SLSLIPPAYIN.PAYINSLIP_NO,   
                         SLSLIPPAYIN.MEMBER_NO,   
                         SLSLIPPAYIN.SLIP_DATE,   
                         SLSLIPPAYIN.OPERATE_DATE,   
                         SLSLIPPAYIN.SLIPTYPE_CODE,   
                         MBMEMBMASTER.MEMB_NAME,   
                         MBMEMBMASTER.MEMB_SURNAME,   
                         MBUCFPRENAME.PRENAME_DESC,   
                         MBUCFMEMBGROUP.MEMBGROUP_DESC,   
                         SLSLIPPAYIN.SLIP_AMT,   
                         SLSLIPPAYIN.MONEYTYPE_CODE,   
                         SLSLIPPAYIN.ENTRY_ID,   
                         SLSLIPPAYIN.ACCID_FLAG,   
                         SLSLIPPAYIN.TOFROM_ACCID,   
                         SLSLIPPAYIN.SLIP_STATUS  
                    FROM SLSLIPPAYIN,   
                         MBMEMBMASTER,   
                         MBUCFMEMBGROUP,   
                         SLUCFSLIPTYPE,   
                         CMUCFMONEYTYPE,   
                         MBUCFPRENAME  
                   WHERE ( SLSLIPPAYIN.COOP_ID = MBMEMBMASTER.COOP_ID ) and  
                         ( SLSLIPPAYIN.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  
                         ( SLSLIPPAYIN.MEMBGROUP_CODE = MBUCFMEMBGROUP.MEMBGROUP_CODE (+)) and  
                         ( SLSLIPPAYIN.SLIPTYPE_CODE = SLUCFSLIPTYPE.SLIPTYPE_CODE ) and  
                         ( SLSLIPPAYIN.MONEYTYPE_CODE = CMUCFMONEYTYPE.MONEYTYPE_CODE ) and  
                         ( MBUCFPRENAME.PRENAME_CODE = MBMEMBMASTER.PRENAME_CODE ) and  
                         ( (SLSLIPPAYIN.COOP_ID={0} )and 
                           (SLSLIPPAYIN.PAYINSLIP_NO={1}) )";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, payinslip_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);                                 
        }

        public void RetrieveMember(string member_no)
        {
            String sql = @"select MBMEMBMASTER.MEMBER_NO,
                                  MBMEMBMASTER.MEMB_NAME,   
                                  MBMEMBMASTER.MEMB_SURNAME,   
                                  MBUCFPRENAME.PRENAME_DESC,   
                                  MBUCFMEMBGROUP.MEMBGROUP_DESC, 
		                          MBUCFMEMBGROUP.MEMBGROUP_CODE
                            from MBMEMBMASTER,   
                                 MBUCFMEMBGROUP,        
                                 MBUCFPRENAME  
                            where ( MBMEMBMASTER.MEMBER_NO = {1} ) and
		                          ( MBMEMBMASTER.COOP_ID = {0} ) and  
                                  ( MBMEMBMASTER.MEMBGROUP_CODE = MBUCFMEMBGROUP.MEMBGROUP_CODE (+)) and   
                                  ( MBUCFPRENAME.PRENAME_CODE = MBMEMBMASTER.PRENAME_CODE ) ";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, member_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void RetrievePeriod(string member_no)
        {
            String sql = @"select kpreceive.coop_id,
		                       kpreceive.recv_period,
							kpreceive.member_no
                        from ( select kptempreceive.coop_id,
						                kptempreceive.recv_period,
									   kptempreceive.member_no
                                   from kptempreceive, kptempreceivedet
                                   where kptempreceive.kpslip_no = kptempreceivedet.kpslip_no
                                   and kptempreceive.coop_id = kptempreceivedet.coop_id
                                   and kptempreceive.recv_period = (select max(kptempreceive.recv_period) from kptempreceive )
                                union
                                select kpmastreceive.coop_id,
						              kpmastreceive.recv_period,
									kpmastreceive.member_no
                                 from kpmastreceive, kpmastreceivedet
                                 where kpmastreceive.kpslip_no = kpmastreceivedet.kpslip_no
                                 and kpmastreceive.coop_id = kpmastreceivedet.coop_id
                                 and (kpmastreceive.keeping_status <> 1 or  kpmastreceivedet.keepitem_status <> 1)
                        ) kpreceive

                        where kpreceive.member_no = {1}  and kpreceive.coop_id = {0}
                         group by 
		                        kpreceive.coop_id,
		                        kpreceive.recv_period,
							 kpreceive.member_no 
                        order by kpreceive.recv_period";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl ,member_no);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "recv_period", "recv_period", "recv_period");
        }

        public void RetrieveReceipt(string member_no, string recv_period)
        {
            String sql = @"select kpreceive.coop_id,
		                        kpreceive.recv_period,
		                        kpreceive.receipt_no,
		                        kpreceive.member_no,
		                        kpreceive.operate_date,
		                        kpreceive.receive_amt,
                                kpreceive.sorter
                        from ( select kptempreceive.coop_id,
						              kptempreceive.recv_period,
		                              kptempreceive.receipt_no,
		                              kptempreceive.member_no,
		                              kptempreceive.operate_date,
		                              kptempreceive.receive_amt,
                                      2 as sorter
                                      from kptempreceive, kptempreceivedet
                                      where kptempreceive.kpslip_no = kptempreceivedet.kpslip_no
                                      and kptempreceive.coop_id = kptempreceivedet.coop_id
                                      and (kptempreceive.keeping_status <> 1 or  kptempreceivedet.keepitem_status <> 1)
                                      and kptempreceive.recv_period = (select max(kptempreceive.recv_period) from kptempreceive )
                                union
                                select kpmastreceive.coop_id,
						               kpmastreceive.recv_period,
		                               kpmastreceive.receipt_no,
		                               kpmastreceive.member_no,
		                               kpmastreceive.operate_date,
		                               kpmastreceive.receive_amt,
                                       1 as sorter
                                       from kpmastreceive, kpmastreceivedet
                                       where kpmastreceive.kpslip_no = kpmastreceivedet.kpslip_no
                                       and kpmastreceive.coop_id = kpmastreceivedet.coop_id
                                       and (kpmastreceive.keeping_status <> 1 or  kpmastreceivedet.keepitem_status <> 1)
                        ) kpreceive
                        where kpreceive.member_no = {1}  and kpreceive.coop_id = {0} and kpreceive.recv_period = {2}
                         group by 
		                        kpreceive.coop_id,
		                        kpreceive.recv_period,
		                        kpreceive.receipt_no,
		                        kpreceive.member_no,
		                        kpreceive.operate_date,
		                        kpreceive.receive_amt,
                                kpreceive.sorter
                            union 
                             select '' as coop_id,
						              'ALL' as recv_period,
									'' as receipt_no,
									'' as member_no,
									sysdate as operate_date,
									0 as receive_amt,
									0 as sorter
                                 from dual 
                        order by sorter,receipt_no,member_no";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, member_no, recv_period);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "receipt_no", "receipt_no", "receipt_no");
        }
    }
}