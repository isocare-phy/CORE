using System.ServiceModel; 
using System.Runtime.Serialization; 
using System.Net.Security; 
using System.ServiceModel.Web; 
using System.ServiceModel.Activation; 
using System.Transactions; 
using Sybase.PowerBuilder.WCFNVO; 
namespace pbservice125
{
	[System.Diagnostics.DebuggerStepThrough]
	[ServiceContract(Name="n_account",Namespace="http://tempurl.org")]
	public class n_account : System.IDisposable 
	{
		internal pbservice125.c__n_account __nvo__;
		private bool ____disposed____ = false;
		public void Dispose()
		{
			if (____disposed____)
				return;
			____disposed____ = true;
			c__pbservice125.InitSession(__nvo__.Session);
			Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(__nvo__);
			c__pbservice125.RestoreOldSession();
		}
		public n_account()
		{
			
			c__pbservice125.InitAssembly();
			__nvo__ = (pbservice125.c__n_account)Sybase.PowerBuilder.WPF.PBSession.CurrentSession.CreateInstance(typeof(pbservice125.c__n_account));
			c__pbservice125.RestoreOldSession();
		}
		internal n_account(pbservice125.c__n_account nvo)
		{
			__nvo__ = nvo;
		}
		[OperationContract(Name="of_candel_year")]
		public virtual System.Int16 of_candel_year(string as_wspass, System.Int16 ai_year)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_candel_year(as_wspass__temp__, ai_year__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_vcproc")]
		public virtual System.Int16 of_vcproc(string as_wspass, System.DateTime adtm_sysdate, string as_sysgencode, string as_coopid, string as_userid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_sysdate__temp__;
			adtm_sysdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_sysdate);
			Sybase.PowerBuilder.PBString as_sysgencode__temp__;
			as_sysgencode__temp__ = new Sybase.PowerBuilder.PBString((string)as_sysgencode);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_userid__temp__;
			as_userid__temp__ = new Sybase.PowerBuilder.PBString((string)as_userid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_vcproc(as_wspass__temp__, adtm_sysdate__temp__, as_sysgencode__temp__, as_coopid__temp__, as_userid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_vcproc_tks")]
		public virtual System.Int16 of_vcproc_tks(string as_wspass, System.DateTime adtm_sysdate, string as_sysgencode, string as_coopid, string as_userid, string as_type_xml, ref string as_voucherno)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_sysdate__temp__;
			adtm_sysdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_sysdate);
			Sybase.PowerBuilder.PBString as_sysgencode__temp__;
			as_sysgencode__temp__ = new Sybase.PowerBuilder.PBString((string)as_sysgencode);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_userid__temp__;
			as_userid__temp__ = new Sybase.PowerBuilder.PBString((string)as_userid);
			Sybase.PowerBuilder.PBString as_type_xml__temp__;
			as_type_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_type_xml);
			Sybase.PowerBuilder.PBString as_voucherno__temp__ = as_voucherno;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_vcproc_tks(as_wspass__temp__, adtm_sysdate__temp__, as_sysgencode__temp__, as_coopid__temp__, as_userid__temp__, as_type_xml__temp__, ref as_voucherno__temp__);
			as_voucherno = as_voucherno__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_vcmastdet")]
		public virtual System.Int16 of_init_vcmastdet(string as_wspass, string as_vcno, ref string as_vcmas_xml, ref string as_vcdet_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_vcno__temp__;
			as_vcno__temp__ = new Sybase.PowerBuilder.PBString((string)as_vcno);
			Sybase.PowerBuilder.PBString as_vcmas_xml__temp__ = as_vcmas_xml;
			Sybase.PowerBuilder.PBString as_vcdet_xml__temp__ = as_vcdet_xml;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_init_vcmastdet(as_wspass__temp__, as_vcno__temp__, ref as_vcmas_xml__temp__, ref as_vcdet_xml__temp__);
			as_vcmas_xml = as_vcmas_xml__temp__;
			as_vcdet_xml = as_vcdet_xml__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_vclistday")]
		public virtual System.Int16 of_init_vclistday(string as_wspass, System.DateTime adtm_date, ref string as_vclist_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_date__temp__;
			adtm_date__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_date);
			Sybase.PowerBuilder.PBString as_vclist_xml__temp__ = as_vclist_xml;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_init_vclistday(as_wspass__temp__, adtm_date__temp__, ref as_vclist_xml__temp__);
			as_vclist_xml = as_vclist_xml__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_vcpost_to_gl")]
		public virtual System.Int16 of_save_vcpost_to_gl(string as_wspass, string as_postlist_xml, string as_entry_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_postlist_xml__temp__;
			as_postlist_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_postlist_xml);
			Sybase.PowerBuilder.PBString as_entry_id__temp__;
			as_entry_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_entry_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_save_vcpost_to_gl(as_wspass__temp__, as_postlist_xml__temp__, as_entry_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_vcpost_to_gl")]
		public virtual System.Int16 of_init_vcpost_to_gl(string as_wspass, System.DateTime adtm_begin, System.DateTime adtm_end, System.Int16 ai_poststatus, ref string as_vclist_xml, string as_coop_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_begin__temp__;
			adtm_begin__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_begin);
			Sybase.PowerBuilder.PBDateTime adtm_end__temp__;
			adtm_end__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_end);
			Sybase.PowerBuilder.PBInt ai_poststatus__temp__;
			ai_poststatus__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_poststatus);
			Sybase.PowerBuilder.PBString as_vclist_xml__temp__ = as_vclist_xml;
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_init_vcpost_to_gl(as_wspass__temp__, adtm_begin__temp__, adtm_end__temp__, ai_poststatus__temp__, ref as_vclist_xml__temp__, as_coop_id__temp__);
			as_vclist_xml = as_vclist_xml__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_cancel_post_to_gl")]
		public virtual System.Int16 of_cancel_post_to_gl(string as_wspass, string as_postlist_xml, string as_entry_id, string as_coop_id, System.DateTime adtm_date)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_postlist_xml__temp__;
			as_postlist_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_postlist_xml);
			Sybase.PowerBuilder.PBString as_entry_id__temp__;
			as_entry_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_entry_id);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			Sybase.PowerBuilder.PBDateTime adtm_date__temp__;
			adtm_date__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_date);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_cancel_post_to_gl(as_wspass__temp__, as_postlist_xml__temp__, as_entry_id__temp__, as_coop_id__temp__, adtm_date__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_vclistcancel")]
		public virtual System.Int16 of_init_vclistcancel(string as_wspass, System.DateTime adtm_date, string as_coopid, ref string as_vclist_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_date__temp__;
			adtm_date__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_date);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_vclist_xml__temp__ = as_vclist_xml;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_init_vclistcancel(as_wspass__temp__, adtm_date__temp__, as_coopid__temp__, ref as_vclist_xml__temp__);
			as_vclist_xml = as_vclist_xml__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_vclistcancel")]
		public virtual System.Int16 of_save_vclistcancel(string as_wspass, System.DateTime adtm_vc_date, string as_voucher, string as_coopid, string as_entryid, System.DateTime adtm_cancel_date)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_vc_date__temp__;
			adtm_vc_date__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_vc_date);
			Sybase.PowerBuilder.PBString as_voucher__temp__;
			as_voucher__temp__ = new Sybase.PowerBuilder.PBString((string)as_voucher);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_entryid__temp__;
			as_entryid__temp__ = new Sybase.PowerBuilder.PBString((string)as_entryid);
			Sybase.PowerBuilder.PBDateTime adtm_cancel_date__temp__;
			adtm_cancel_date__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_cancel_date);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_save_vclistcancel(as_wspass__temp__, adtm_vc_date__temp__, as_voucher__temp__, as_coopid__temp__, as_entryid__temp__, adtm_cancel_date__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_gen_data_paytrnbank")]
		public virtual System.Int16 of_gen_data_paytrnbank(string as_wspass, string as_head_xml, string as_share_xml, string as_loan_xml, string as_etc_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_head_xml__temp__;
			as_head_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_head_xml);
			Sybase.PowerBuilder.PBString as_share_xml__temp__;
			as_share_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_share_xml);
			Sybase.PowerBuilder.PBString as_loan_xml__temp__;
			as_loan_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_loan_xml);
			Sybase.PowerBuilder.PBString as_etc_xml__temp__;
			as_etc_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_etc_xml);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_gen_data_paytrnbank(as_wspass__temp__, as_head_xml__temp__, as_share_xml__temp__, as_loan_xml__temp__, as_etc_xml__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_get_cash_bg_fw")]
		public virtual System.Int16 of_get_cash_bg_fw(string as_wspass, System.DateTime adtm_startdate, string as_coop_id, ref decimal adc_begin, ref decimal adc_forward)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_startdate__temp__;
			adtm_startdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_startdate);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			Sybase.PowerBuilder.PBDecimal adc_begin__temp__ = adc_begin;
			Sybase.PowerBuilder.PBDecimal adc_forward__temp__ = adc_forward;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_get_cash_bg_fw(as_wspass__temp__, adtm_startdate__temp__, as_coop_id__temp__, ref adc_begin__temp__, ref adc_forward__temp__);
			adc_begin = adc_begin__temp__;
			adc_forward = adc_forward__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_savevoucher")]
		public virtual System.Int16 of_savevoucher(string as_wspass, string as_vcmas_xml, string as_vcdet_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_vcmas_xml__temp__;
			as_vcmas_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_vcmas_xml);
			Sybase.PowerBuilder.PBString as_vcdet_xml__temp__;
			as_vcdet_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_vcdet_xml);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_savevoucher(as_wspass__temp__, as_vcmas_xml__temp__, as_vcdet_xml__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_vclistcancel_by_vcno")]
		public virtual System.Int16 of_init_vclistcancel_by_vcno(string as_wspass, string vc_no, string as_coopid, ref string as_vclist_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString vc_no__temp__;
			vc_no__temp__ = new Sybase.PowerBuilder.PBString((string)vc_no);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_vclist_xml__temp__ = as_vclist_xml;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_init_vclistcancel_by_vcno(as_wspass__temp__, vc_no__temp__, as_coopid__temp__, ref as_vclist_xml__temp__);
			as_vclist_xml = as_vclist_xml__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_close_month_clear_vcno")]
		public virtual System.Int16 of_close_month_clear_vcno(string as_wspass, System.Int16 ai_year, System.Int16 ai_period)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBInt ai_period__temp__;
			ai_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_period);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_close_month_clear_vcno(as_wspass__temp__, ai_year__temp__, ai_period__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_set_default_accountid")]
		public virtual System.Int16 of_set_default_accountid(string as_wspass, string as_coop_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_set_default_accountid(as_wspass__temp__, as_coop_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_close_month")]
		public virtual System.Int16 of_close_month(string as_wspass, System.Int16 ai_year, System.Int16 ai_period)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBInt ai_period__temp__;
			ai_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_period);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_close_month(as_wspass__temp__, ai_year__temp__, ai_period__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_cancel_closemonth_clear_vcno")]
		public virtual System.Int16 of_cancel_closemonth_clear_vcno(string as_wspass, System.Int16 ai_year, System.Int16 ai_period)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBInt ai_period__temp__;
			ai_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_period);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_cancel_closemonth_clear_vcno(as_wspass__temp__, ai_year__temp__, ai_period__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_cancel_closemonth")]
		public virtual System.Int16 of_cancel_closemonth(string as_wspass, System.Int16 ai_year, System.Int16 ai_period)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBInt ai_period__temp__;
			ai_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_period);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_cancel_closemonth(as_wspass__temp__, ai_year__temp__, ai_period__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_close_year")]
		public virtual System.Int16 of_close_year(string as_wspass, System.Int16 ai_year)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_close_year(as_wspass__temp__, ai_year__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_cancel_closeyear")]
		public virtual System.Int16 of_cancel_closeyear(string as_wspass, System.Int16 ai_year)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_cancel_closeyear(as_wspass__temp__, ai_year__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_consstant_bs")]
		public virtual System.Int16 of_init_consstant_bs(string as_wspass, string as_data_desc, ref string as_buzz_display_xml, ref string as_usr_type_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_data_desc__temp__;
			as_data_desc__temp__ = new Sybase.PowerBuilder.PBString((string)as_data_desc);
			Sybase.PowerBuilder.PBString as_buzz_display_xml__temp__ = as_buzz_display_xml;
			Sybase.PowerBuilder.PBString as_usr_type_xml__temp__ = as_usr_type_xml;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_init_consstant_bs(as_wspass__temp__, as_data_desc__temp__, ref as_buzz_display_xml__temp__, ref as_usr_type_xml__temp__);
			as_buzz_display_xml = as_buzz_display_xml__temp__;
			as_usr_type_xml = as_usr_type_xml__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_constant_bs")]
		public virtual System.Int16 of_save_constant_bs(string as_wspass, string as_main_xml, string as_detail_xml, string as_money_code, System.Int16 an_sheet_seq)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_main_xml__temp__;
			as_main_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_main_xml);
			Sybase.PowerBuilder.PBString as_detail_xml__temp__;
			as_detail_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_detail_xml);
			Sybase.PowerBuilder.PBString as_money_code__temp__;
			as_money_code__temp__ = new Sybase.PowerBuilder.PBString((string)as_money_code);
			Sybase.PowerBuilder.PBInt an_sheet_seq__temp__;
			an_sheet_seq__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)an_sheet_seq);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_save_constant_bs(as_wspass__temp__, as_main_xml__temp__, as_detail_xml__temp__, as_money_code__temp__, an_sheet_seq__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_formular_fc")]
		public virtual System.Int16 of_init_formular_fc(string as_wspass, string as_money_sheetcode, System.Int16 an_data_group, ref string as_formular_fc_xml, ref string as_formular_fc_choose_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_money_sheetcode__temp__;
			as_money_sheetcode__temp__ = new Sybase.PowerBuilder.PBString((string)as_money_sheetcode);
			Sybase.PowerBuilder.PBInt an_data_group__temp__;
			an_data_group__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)an_data_group);
			Sybase.PowerBuilder.PBString as_formular_fc_xml__temp__ = as_formular_fc_xml;
			Sybase.PowerBuilder.PBString as_formular_fc_choose_xml__temp__ = as_formular_fc_choose_xml;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_init_formular_fc(as_wspass__temp__, as_money_sheetcode__temp__, an_data_group__temp__, ref as_formular_fc_xml__temp__, ref as_formular_fc_choose_xml__temp__);
			as_formular_fc_xml = as_formular_fc_xml__temp__;
			as_formular_fc_choose_xml = as_formular_fc_choose_xml__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_formula_fc")]
		public virtual System.Int16 of_save_formula_fc(string as_wspass, string as_main_xml, string as_money_code, string as_data_group)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_main_xml__temp__;
			as_main_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_main_xml);
			Sybase.PowerBuilder.PBString as_money_code__temp__;
			as_money_code__temp__ = new Sybase.PowerBuilder.PBString((string)as_money_code);
			Sybase.PowerBuilder.PBString as_data_group__temp__;
			as_data_group__temp__ = new Sybase.PowerBuilder.PBString((string)as_data_group);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_save_formula_fc(as_wspass__temp__, as_main_xml__temp__, as_money_code__temp__, as_data_group__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_formula_sm")]
		public virtual System.Int16 of_save_formula_sm(string as_wspass, string as_main_xml, string as_money_code, string as_data_group)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_main_xml__temp__;
			as_main_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_main_xml);
			Sybase.PowerBuilder.PBString as_money_code__temp__;
			as_money_code__temp__ = new Sybase.PowerBuilder.PBString((string)as_money_code);
			Sybase.PowerBuilder.PBString as_data_group__temp__;
			as_data_group__temp__ = new Sybase.PowerBuilder.PBString((string)as_data_group);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_save_formula_sm(as_wspass__temp__, as_main_xml__temp__, as_money_code__temp__, as_data_group__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_formula_sm")]
		public virtual System.Int16 of_init_formula_sm(string as_wspass, string as_money_sheetcode, System.Int16 an_data_group, ref string as_formular_sm_xml, ref string as_formular_sm_choose_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_money_sheetcode__temp__;
			as_money_sheetcode__temp__ = new Sybase.PowerBuilder.PBString((string)as_money_sheetcode);
			Sybase.PowerBuilder.PBInt an_data_group__temp__;
			an_data_group__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)an_data_group);
			Sybase.PowerBuilder.PBString as_formular_sm_xml__temp__ = as_formular_sm_xml;
			Sybase.PowerBuilder.PBString as_formular_sm_choose_xml__temp__ = as_formular_sm_choose_xml;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_init_formula_sm(as_wspass__temp__, as_money_sheetcode__temp__, an_data_group__temp__, ref as_formular_sm_xml__temp__, ref as_formular_sm_choose_xml__temp__);
			as_formular_sm_xml = as_formular_sm_xml__temp__;
			as_formular_sm_choose_xml = as_formular_sm_choose_xml__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_gen_cashflow_sheet")]
		public virtual System.Int16 of_gen_cashflow_sheet(string as_wspass, string as_acc_bs_head_xml, string as_moneysheet_code, string as_coop_id, ref string as_acc_bs_ret_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_acc_bs_head_xml__temp__;
			as_acc_bs_head_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_acc_bs_head_xml);
			Sybase.PowerBuilder.PBString as_moneysheet_code__temp__;
			as_moneysheet_code__temp__ = new Sybase.PowerBuilder.PBString((string)as_moneysheet_code);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			Sybase.PowerBuilder.PBString as_acc_bs_ret_xml__temp__ = as_acc_bs_ret_xml;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_gen_cashflow_sheet(as_wspass__temp__, as_acc_bs_head_xml__temp__, as_moneysheet_code__temp__, as_coop_id__temp__, ref as_acc_bs_ret_xml__temp__);
			as_acc_bs_ret_xml = as_acc_bs_ret_xml__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_delete_accountid")]
		public virtual System.Int16 of_delete_accountid(string as_wspass, string as_accountid, string as_coop_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_accountid__temp__;
			as_accountid__temp__ = new Sybase.PowerBuilder.PBString((string)as_accountid);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_delete_accountid(as_wspass__temp__, as_accountid__temp__, as_coop_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_add_newaccount_id")]
		public virtual System.Int16 of_add_newaccount_id(string as_wspass, string as_account_iddetail_xml, System.Int16 ai_update_add)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_account_iddetail_xml__temp__;
			as_account_iddetail_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_account_iddetail_xml);
			Sybase.PowerBuilder.PBInt ai_update_add__temp__;
			ai_update_add__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_update_add);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_add_newaccount_id(as_wspass__temp__, as_account_iddetail_xml__temp__, ai_update_add__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_candel_period")]
		public virtual System.Int16 of_candel_period(string as_wspass, System.Int16 ai_year, System.Int16 ai_period)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBInt ai_period__temp__;
			ai_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_period);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_candel_period(as_wspass__temp__, ai_year__temp__, ai_period__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_add_accperiod")]
		public virtual System.Int16 of_add_accperiod(string as_wspass, System.Int16 ai_year, string as_accperiod_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBString as_accperiod_xml__temp__;
			as_accperiod_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_accperiod_xml);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_add_accperiod(as_wspass__temp__, ai_year__temp__, as_accperiod_xml__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_add_first_sumleger_period")]
		public virtual System.Int16 of_add_first_sumleger_period(string as_wspass, System.Int16 ai_year, string as_coop_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_add_first_sumleger_period(as_wspass__temp__, ai_year__temp__, as_coop_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_set_budget")]
		public virtual System.Int16 of_set_budget(string as_wspass, string as_xml, string as_coopid, System.Int16 an_year, string as_budget_type)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_xml__temp__;
			as_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_xml);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBInt an_year__temp__;
			an_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)an_year);
			Sybase.PowerBuilder.PBString as_budget_type__temp__;
			as_budget_type__temp__ = new Sybase.PowerBuilder.PBString((string)as_budget_type);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_set_budget(as_wspass__temp__, as_xml__temp__, as_coopid__temp__, an_year__temp__, as_budget_type__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_process_budget")]
		public virtual System.Int16 of_process_budget(string as_wspass, string as_coopid, System.Int16 ai_year, System.Int16 ai_period)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBInt ai_period__temp__;
			ai_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_period);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_process_budget(as_wspass__temp__, as_coopid__temp__, ai_year__temp__, ai_period__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_add_accyear")]
		public virtual System.Int16 of_add_accyear(string as_wspass, string as_year_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_year_xml__temp__;
			as_year_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_year_xml);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_add_accyear(as_wspass__temp__, as_year_xml__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_cal_dp")]
		public virtual System.Int16 of_cal_dp(string as_wspass, System.DateTime adtm_caltodate, string as_coop)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_caltodate__temp__;
			adtm_caltodate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_caltodate);
			Sybase.PowerBuilder.PBString as_coop__temp__;
			as_coop__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_cal_dp(as_wspass__temp__, adtm_caltodate__temp__, as_coop__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_gen_balance_sheet")]
		public virtual System.Int16 of_gen_balance_sheet(string as_wspass, string as_acc_bs_head_xml, string as_moneysheet_code, string as_coop_id, ref string as_acc_bs_ret_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_acc_bs_head_xml__temp__;
			as_acc_bs_head_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_acc_bs_head_xml);
			Sybase.PowerBuilder.PBString as_moneysheet_code__temp__;
			as_moneysheet_code__temp__ = new Sybase.PowerBuilder.PBString((string)as_moneysheet_code);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			Sybase.PowerBuilder.PBString as_acc_bs_ret_xml__temp__ = as_acc_bs_ret_xml;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_gen_balance_sheet(as_wspass__temp__, as_acc_bs_head_xml__temp__, as_moneysheet_code__temp__, as_coop_id__temp__, ref as_acc_bs_ret_xml__temp__);
			as_acc_bs_ret_xml = as_acc_bs_ret_xml__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_gen_cashpaper_activities")]
		public virtual string of_gen_cashpaper_activities(string as_wspass, System.DateTime adtm_start, System.DateTime adtm_end, System.Int16 ai_sum_period, string as_coop_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			string __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_start__temp__;
			adtm_start__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_start);
			Sybase.PowerBuilder.PBDateTime adtm_end__temp__;
			adtm_end__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_end);
			Sybase.PowerBuilder.PBInt ai_sum_period__temp__;
			ai_sum_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_sum_period);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_gen_cashpaper_activities(as_wspass__temp__, adtm_start__temp__, adtm_end__temp__, ai_sum_period__temp__, as_coop_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_gen_cashpaper_drcr")]
		public virtual string of_gen_cashpaper_drcr(string as_wspass, System.DateTime adtm_start, System.DateTime adtm_end, System.Int16 ai_sum_period, string as_coop_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			string __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_start__temp__;
			adtm_start__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_start);
			Sybase.PowerBuilder.PBDateTime adtm_end__temp__;
			adtm_end__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_end);
			Sybase.PowerBuilder.PBInt ai_sum_period__temp__;
			ai_sum_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_sum_period);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_gen_cashpaper_drcr(as_wspass__temp__, adtm_start__temp__, adtm_end__temp__, ai_sum_period__temp__, as_coop_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_gen_day_journalreport")]
		public virtual string of_gen_day_journalreport(string as_wspass, System.DateTime adtm_start_date, System.DateTime adtm_end_date, System.Int16 ai_type_group, string as_coop_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			string __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_start_date__temp__;
			adtm_start_date__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_start_date);
			Sybase.PowerBuilder.PBDateTime adtm_end_date__temp__;
			adtm_end_date__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_end_date);
			Sybase.PowerBuilder.PBInt ai_type_group__temp__;
			ai_type_group__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_type_group);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_gen_day_journalreport(as_wspass__temp__, adtm_start_date__temp__, adtm_end_date__temp__, ai_type_group__temp__, as_coop_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_gen_ledger_report")]
		public virtual string of_gen_ledger_report(string as_wspass, System.DateTime adtm_start, System.DateTime adtm_end, string as_account_id1, string as_account_id2, string as_coop_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			string __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_start__temp__;
			adtm_start__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_start);
			Sybase.PowerBuilder.PBDateTime adtm_end__temp__;
			adtm_end__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_end);
			Sybase.PowerBuilder.PBString as_account_id1__temp__;
			as_account_id1__temp__ = new Sybase.PowerBuilder.PBString((string)as_account_id1);
			Sybase.PowerBuilder.PBString as_account_id2__temp__;
			as_account_id2__temp__ = new Sybase.PowerBuilder.PBString((string)as_account_id2);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_gen_ledger_report(as_wspass__temp__, adtm_start__temp__, adtm_end__temp__, as_account_id1__temp__, as_account_id2__temp__, as_coop_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_gen_trial_bs")]
		public virtual System.Int16 of_gen_trial_bs(string as_wspass, string as_choose_report_xml, string as_coop_id, ref string as_trial_report_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_choose_report_xml__temp__;
			as_choose_report_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_choose_report_xml);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			Sybase.PowerBuilder.PBString as_trial_report_xml__temp__ = as_trial_report_xml;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_gen_trial_bs(as_wspass__temp__, as_choose_report_xml__temp__, as_coop_id__temp__, ref as_trial_report_xml__temp__);
			as_trial_report_xml = as_trial_report_xml__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_get_cashpaper_detail")]
		public virtual string of_get_cashpaper_detail(string as_wspass, System.Int16 ai_sheet, string as_type_activity, string as_coop_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			string __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_sheet__temp__;
			ai_sheet__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_sheet);
			Sybase.PowerBuilder.PBString as_type_activity__temp__;
			as_type_activity__temp__ = new Sybase.PowerBuilder.PBString((string)as_type_activity);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_get_cashpaper_detail(as_wspass__temp__, ai_sheet__temp__, as_type_activity__temp__, as_coop_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_get_year_period")]
		public virtual System.Int16 of_get_year_period(string as_wspass, System.DateTime adtm_date, string as_coop_id, ref System.Int16 ai_year, ref System.Int16 ai_period)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_date__temp__;
			adtm_date__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_date);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			Sybase.PowerBuilder.PBInt ai_year__temp__ = ai_year;
			Sybase.PowerBuilder.PBInt ai_period__temp__ = ai_period;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_get_year_period(as_wspass__temp__, adtm_date__temp__, as_coop_id__temp__, ref ai_year__temp__, ref ai_period__temp__);
			ai_year = ai_year__temp__;
			ai_period = ai_period__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_account_splitactivity")]
		public virtual string of_init_account_splitactivity(string as_wspass, System.Int16 ai_year, System.Int16 ai_period)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			string __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBInt ai_period__temp__;
			ai_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_period);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_init_account_splitactivity(as_wspass__temp__, ai_year__temp__, ai_period__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_contuseprofit")]
		public virtual string of_init_contuseprofit(string as_wspass, System.Int16 ai_year, System.Int16 ai_period)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			string __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBInt ai_period__temp__;
			ai_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_period);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_init_contuseprofit(as_wspass__temp__, ai_year__temp__, ai_period__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_vcset_noncash")]
		public virtual string of_init_vcset_noncash(string as_wspass, System.DateTime adtm_datebegin, System.DateTime adtm_dateend, string as_account_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			string __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_datebegin__temp__;
			adtm_datebegin__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_datebegin);
			Sybase.PowerBuilder.PBDateTime adtm_dateend__temp__;
			adtm_dateend__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_dateend);
			Sybase.PowerBuilder.PBString as_account_id__temp__;
			as_account_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_account_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_init_vcset_noncash(as_wspass__temp__, adtm_datebegin__temp__, adtm_dateend__temp__, as_account_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_account_splitactivity")]
		public virtual System.Int16 of_save_account_splitactivity(string as_wspass, string as_splitactivity_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_splitactivity_xml__temp__;
			as_splitactivity_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_splitactivity_xml);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_save_account_splitactivity(as_wspass__temp__, as_splitactivity_xml__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_asset")]
		public virtual System.Int16 of_save_asset(string as_wspass, string as_xml, string as_ratexml, string as_coop)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_xml__temp__;
			as_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_xml);
			Sybase.PowerBuilder.PBString as_ratexml__temp__;
			as_ratexml__temp__ = new Sybase.PowerBuilder.PBString((string)as_ratexml);
			Sybase.PowerBuilder.PBString as_coop__temp__;
			as_coop__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_save_asset(as_wspass__temp__, as_xml__temp__, as_ratexml__temp__, as_coop__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_contuseprofit")]
		public virtual System.Int16 of_save_contuseprofit(string as_wspass, string as_conuse_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_conuse_xml__temp__;
			as_conuse_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_conuse_xml);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_save_contuseprofit(as_wspass__temp__, as_conuse_xml__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_vcset_noncash")]
		public virtual System.Int16 of_save_vcset_noncash(string as_wspass, string as_cash_detail_xml, System.DateTime adtm_date_begin, System.DateTime adtm_date_end, string as_account_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_cash_detail_xml__temp__;
			as_cash_detail_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_cash_detail_xml);
			Sybase.PowerBuilder.PBDateTime adtm_date_begin__temp__;
			adtm_date_begin__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_date_begin);
			Sybase.PowerBuilder.PBDateTime adtm_date_end__temp__;
			adtm_date_end__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_date_end);
			Sybase.PowerBuilder.PBString as_account_id__temp__;
			as_account_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_account_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_save_vcset_noncash(as_wspass__temp__, as_cash_detail_xml__temp__, adtm_date_begin__temp__, adtm_date_end__temp__, as_account_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_update_cashpaper_detail")]
		public virtual System.Int16 of_update_cashpaper_detail(string as_wspass, string as_cash_detail_xml, System.Int16 ai_sheet, string as_type_activity)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_cash_detail_xml__temp__;
			as_cash_detail_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_cash_detail_xml);
			Sybase.PowerBuilder.PBInt ai_sheet__temp__;
			ai_sheet__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_sheet);
			Sybase.PowerBuilder.PBString as_type_activity__temp__;
			as_type_activity__temp__ = new Sybase.PowerBuilder.PBString((string)as_type_activity);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_update_cashpaper_detail(as_wspass__temp__, as_cash_detail_xml__temp__, ai_sheet__temp__, as_type_activity__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_process_pension")]
		public virtual System.Int16 of_process_pension(string as_wspass, string as_coopid, System.Int16 ai_year)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_process_pension(as_wspass__temp__, as_coopid__temp__, ai_year__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_vcproc_nan")]
		public virtual System.Int16 of_vcproc_nan(string as_wspass, System.DateTime adtm_sysdate, string as_sysgencode, string as_coopid, string as_userid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_sysdate__temp__;
			adtm_sysdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_sysdate);
			Sybase.PowerBuilder.PBString as_sysgencode__temp__;
			as_sysgencode__temp__ = new Sybase.PowerBuilder.PBString((string)as_sysgencode);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_userid__temp__;
			as_userid__temp__ = new Sybase.PowerBuilder.PBString((string)as_userid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_vcproc_nan(as_wspass__temp__, adtm_sysdate__temp__, as_sysgencode__temp__, as_coopid__temp__, as_userid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_vcproc_pea")]
		public virtual System.Int16 of_vcproc_pea(string as_wspass, System.DateTime adtm_sysdate, string as_sysgencode, string as_coopid, string as_userid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_sysdate__temp__;
			adtm_sysdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_sysdate);
			Sybase.PowerBuilder.PBString as_sysgencode__temp__;
			as_sysgencode__temp__ = new Sybase.PowerBuilder.PBString((string)as_sysgencode);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_userid__temp__;
			as_userid__temp__ = new Sybase.PowerBuilder.PBString((string)as_userid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_vcproc_pea(as_wspass__temp__, adtm_sysdate__temp__, as_sysgencode__temp__, as_coopid__temp__, as_userid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_vcproc_mhs")]
		public virtual System.Int16 of_vcproc_mhs(string as_wspass, System.DateTime adtm_sysdate, string as_sysgencode, string as_coopid, string as_userid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_sysdate__temp__;
			adtm_sysdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_sysdate);
			Sybase.PowerBuilder.PBString as_sysgencode__temp__;
			as_sysgencode__temp__ = new Sybase.PowerBuilder.PBString((string)as_sysgencode);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_userid__temp__;
			as_userid__temp__ = new Sybase.PowerBuilder.PBString((string)as_userid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_vcproc_mhs(as_wspass__temp__, adtm_sysdate__temp__, as_sysgencode__temp__, as_coopid__temp__, as_userid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_process_member_balance")]
		public virtual System.Int16 of_process_member_balance(string as_wspass, System.Int16 ai_year, System.Int16 ai_period, string as_coopid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBInt ai_period__temp__;
			ai_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_period);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_process_member_balance(as_wspass__temp__, ai_year__temp__, ai_period__temp__, as_coopid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_gen_trial_bs_coop")]
		public virtual System.Int16 of_gen_trial_bs_coop(string as_wspass, string as_choose_report_xml, string as_coop_id, ref string as_trial_report_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_choose_report_xml__temp__;
			as_choose_report_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_choose_report_xml);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			Sybase.PowerBuilder.PBString as_trial_report_xml__temp__ = as_trial_report_xml;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_gen_trial_bs_coop(as_wspass__temp__, as_choose_report_xml__temp__, as_coop_id__temp__, ref as_trial_report_xml__temp__);
			as_trial_report_xml = as_trial_report_xml__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_vcmastdet_nan")]
		public virtual System.Int16 of_init_vcmastdet_nan(string as_wspass, string as_vcno, ref string as_vcmas_xml, ref string as_vcdet_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_vcno__temp__;
			as_vcno__temp__ = new Sybase.PowerBuilder.PBString((string)as_vcno);
			Sybase.PowerBuilder.PBString as_vcmas_xml__temp__ = as_vcmas_xml;
			Sybase.PowerBuilder.PBString as_vcdet_xml__temp__ = as_vcdet_xml;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_init_vcmastdet_nan(as_wspass__temp__, as_vcno__temp__, ref as_vcmas_xml__temp__, ref as_vcdet_xml__temp__);
			as_vcmas_xml = as_vcmas_xml__temp__;
			as_vcdet_xml = as_vcdet_xml__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_gen_trial_bs2")]
		public virtual System.Int16 of_gen_trial_bs2(string as_wspass, System.DateTime adtm_start_date, System.DateTime adtm_end_date, string as_check_flag, string as_coop_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_start_date__temp__;
			adtm_start_date__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_start_date);
			Sybase.PowerBuilder.PBDateTime adtm_end_date__temp__;
			adtm_end_date__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_end_date);
			Sybase.PowerBuilder.PBString as_check_flag__temp__;
			as_check_flag__temp__ = new Sybase.PowerBuilder.PBString((string)as_check_flag);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_gen_trial_bs2(as_wspass__temp__, adtm_start_date__temp__, adtm_end_date__temp__, as_check_flag__temp__, as_coop_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_gen_ledger_report_day")]
		public virtual string of_gen_ledger_report_day(string as_wspass, System.DateTime adtm_start, System.DateTime adtm_end, string as_account_id1, string as_account_id2, string as_coop_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			string __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_start__temp__;
			adtm_start__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_start);
			Sybase.PowerBuilder.PBDateTime adtm_end__temp__;
			adtm_end__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_end);
			Sybase.PowerBuilder.PBString as_account_id1__temp__;
			as_account_id1__temp__ = new Sybase.PowerBuilder.PBString((string)as_account_id1);
			Sybase.PowerBuilder.PBString as_account_id2__temp__;
			as_account_id2__temp__ = new Sybase.PowerBuilder.PBString((string)as_account_id2);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_gen_ledger_report_day(as_wspass__temp__, adtm_start__temp__, adtm_end__temp__, as_account_id1__temp__, as_account_id2__temp__, as_coop_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_get_balance_begin_day")]
		public virtual decimal of_get_balance_begin_day(string as_wspass, string as_account_id, System.DateTime adtm_date_find, string as_coop_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			decimal __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_account_id__temp__;
			as_account_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_account_id);
			Sybase.PowerBuilder.PBDateTime adtm_date_find__temp__;
			adtm_date_find__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_date_find);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_get_balance_begin_day(as_wspass__temp__, as_account_id__temp__, adtm_date_find__temp__, as_coop_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_get_balance_begin")]
		public virtual decimal of_get_balance_begin(string as_wspass, string as_account_id, System.Int16 ai_account_year, System.Int16 ai_period, string as_coop_id)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			decimal __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_account_id__temp__;
			as_account_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_account_id);
			Sybase.PowerBuilder.PBInt ai_account_year__temp__;
			ai_account_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_account_year);
			Sybase.PowerBuilder.PBInt ai_period__temp__;
			ai_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_period);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_get_balance_begin(as_wspass__temp__, as_account_id__temp__, ai_account_year__temp__, ai_period__temp__, as_coop_id__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_vcproc_exat")]
		public virtual System.Int16 of_vcproc_exat(string as_wspass, System.DateTime adtm_sysdate, string as_sysgencode, string as_coopid, string as_userid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_sysdate__temp__;
			adtm_sysdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_sysdate);
			Sybase.PowerBuilder.PBString as_sysgencode__temp__;
			as_sysgencode__temp__ = new Sybase.PowerBuilder.PBString((string)as_sysgencode);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_userid__temp__;
			as_userid__temp__ = new Sybase.PowerBuilder.PBString((string)as_userid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_vcproc_exat(as_wspass__temp__, adtm_sysdate__temp__, as_sysgencode__temp__, as_coopid__temp__, as_userid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_get_cash_bg_fw_first_period")]
		public virtual System.Int16 of_get_cash_bg_fw_first_period(string as_wspass, System.DateTime adtm_startdate, string as_coop_id, ref decimal adc_begin, ref decimal adc_forward)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_startdate__temp__;
			adtm_startdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_startdate);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			Sybase.PowerBuilder.PBDecimal adc_begin__temp__ = adc_begin;
			Sybase.PowerBuilder.PBDecimal adc_forward__temp__ = adc_forward;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_get_cash_bg_fw_first_period(as_wspass__temp__, adtm_startdate__temp__, as_coop_id__temp__, ref adc_begin__temp__, ref adc_forward__temp__);
			adc_begin = adc_begin__temp__;
			adc_forward = adc_forward__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_vcproc_ry")]
		public virtual System.Int16 of_vcproc_ry(string as_wspass, System.DateTime adtm_sysdate, string as_sysgencode, string as_coopid, string as_userid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_sysdate__temp__;
			adtm_sysdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_sysdate);
			Sybase.PowerBuilder.PBString as_sysgencode__temp__;
			as_sysgencode__temp__ = new Sybase.PowerBuilder.PBString((string)as_sysgencode);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_userid__temp__;
			as_userid__temp__ = new Sybase.PowerBuilder.PBString((string)as_userid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_vcproc_ry(as_wspass__temp__, adtm_sysdate__temp__, as_sysgencode__temp__, as_coopid__temp__, as_userid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_gen_balance_sheet_excel")]
		public virtual System.Int16 of_gen_balance_sheet_excel(string as_wspass, string as_moneysheet_code, System.Int16 ai_year_1, System.Int16 ai_year_2, System.Int16 ai_month_1, System.Int16 ai_month_2, System.Int16 ai_show_all, System.Int16 ai_data_1, System.Int16 ai_data_2, System.Int16 ai_compere_b1_b3, System.Int16 ai_show_remark, string as_coop_id, System.Int16 ai_total_show, System.Int16 ai_percent_status)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_moneysheet_code__temp__;
			as_moneysheet_code__temp__ = new Sybase.PowerBuilder.PBString((string)as_moneysheet_code);
			Sybase.PowerBuilder.PBInt ai_year_1__temp__;
			ai_year_1__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year_1);
			Sybase.PowerBuilder.PBInt ai_year_2__temp__;
			ai_year_2__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year_2);
			Sybase.PowerBuilder.PBInt ai_month_1__temp__;
			ai_month_1__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_month_1);
			Sybase.PowerBuilder.PBInt ai_month_2__temp__;
			ai_month_2__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_month_2);
			Sybase.PowerBuilder.PBInt ai_show_all__temp__;
			ai_show_all__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_show_all);
			Sybase.PowerBuilder.PBInt ai_data_1__temp__;
			ai_data_1__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_data_1);
			Sybase.PowerBuilder.PBInt ai_data_2__temp__;
			ai_data_2__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_data_2);
			Sybase.PowerBuilder.PBInt ai_compere_b1_b3__temp__;
			ai_compere_b1_b3__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_compere_b1_b3);
			Sybase.PowerBuilder.PBInt ai_show_remark__temp__;
			ai_show_remark__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_show_remark);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			Sybase.PowerBuilder.PBInt ai_total_show__temp__;
			ai_total_show__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_total_show);
			Sybase.PowerBuilder.PBInt ai_percent_status__temp__;
			ai_percent_status__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_percent_status);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_gen_balance_sheet_excel(as_wspass__temp__, as_moneysheet_code__temp__, ai_year_1__temp__, ai_year_2__temp__, ai_month_1__temp__, ai_month_2__temp__, ai_show_all__temp__, ai_data_1__temp__, ai_data_2__temp__, ai_compere_b1_b3__temp__, ai_show_remark__temp__, as_coop_id__temp__, ai_total_show__temp__, ai_percent_status__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_vcproc_lap")]
		public virtual System.Int16 of_vcproc_lap(string as_wspass, System.DateTime adtm_sysdate, string as_sysgencode, string as_coopid, string as_userid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_sysdate__temp__;
			adtm_sysdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_sysdate);
			Sybase.PowerBuilder.PBString as_sysgencode__temp__;
			as_sysgencode__temp__ = new Sybase.PowerBuilder.PBString((string)as_sysgencode);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_userid__temp__;
			as_userid__temp__ = new Sybase.PowerBuilder.PBString((string)as_userid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_vcproc_lap(as_wspass__temp__, adtm_sysdate__temp__, as_sysgencode__temp__, as_coopid__temp__, as_userid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_add_newemployee")]
		public virtual System.Int16 of_add_newemployee(string as_wspass, string as_employee_detail_xml, System.Int16 ai_update_add)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_employee_detail_xml__temp__;
			as_employee_detail_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_employee_detail_xml);
			Sybase.PowerBuilder.PBInt ai_update_add__temp__;
			ai_update_add__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_update_add);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_add_newemployee(as_wspass__temp__, as_employee_detail_xml__temp__, ai_update_add__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_close_depreciation")]
		public virtual System.Int16 of_close_depreciation(string as_wspass, System.Int16 ai_year, System.DateTime adtm_caltodate, string as_coopid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBDateTime adtm_caltodate__temp__;
			adtm_caltodate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_caltodate);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_close_depreciation(as_wspass__temp__, ai_year__temp__, adtm_caltodate__temp__, as_coopid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_set_employee_salary")]
		public virtual System.Int16 of_set_employee_salary(string as_wspass, string as_xml, string as_coopid, System.Int16 an_year)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_xml__temp__;
			as_xml__temp__ = new Sybase.PowerBuilder.PBString((string)as_xml);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBInt an_year__temp__;
			an_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)an_year);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_set_employee_salary(as_wspass__temp__, as_xml__temp__, as_coopid__temp__, an_year__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_vcproc_msk")]
		public virtual System.Int16 of_vcproc_msk(string as_wspass, System.DateTime adtm_sysdate, string as_sysgencode, string as_coopid, string as_userid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_sysdate__temp__;
			adtm_sysdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_sysdate);
			Sybase.PowerBuilder.PBString as_sysgencode__temp__;
			as_sysgencode__temp__ = new Sybase.PowerBuilder.PBString((string)as_sysgencode);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_userid__temp__;
			as_userid__temp__ = new Sybase.PowerBuilder.PBString((string)as_userid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_vcproc_msk(as_wspass__temp__, adtm_sysdate__temp__, as_sysgencode__temp__, as_coopid__temp__, as_userid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_get_bg_fw")]
		public virtual System.Int16 of_get_bg_fw(string as_wspass, System.DateTime adtm_startdate, string as_coop_id, string as_accountid, ref decimal adc_begin, ref decimal adc_forward)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_startdate__temp__;
			adtm_startdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_startdate);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			Sybase.PowerBuilder.PBString as_accountid__temp__;
			as_accountid__temp__ = new Sybase.PowerBuilder.PBString((string)as_accountid);
			Sybase.PowerBuilder.PBDecimal adc_begin__temp__ = adc_begin;
			Sybase.PowerBuilder.PBDecimal adc_forward__temp__ = adc_forward;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_get_bg_fw(as_wspass__temp__, adtm_startdate__temp__, as_coop_id__temp__, as_accountid__temp__, ref adc_begin__temp__, ref adc_forward__temp__);
			adc_begin = adc_begin__temp__;
			adc_forward = adc_forward__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_vcproc_rbt")]
		public virtual System.Int16 of_vcproc_rbt(string as_wspass, System.DateTime adtm_sysdate, string as_sysgencode, string as_coopid, string as_userid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_sysdate__temp__;
			adtm_sysdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_sysdate);
			Sybase.PowerBuilder.PBString as_sysgencode__temp__;
			as_sysgencode__temp__ = new Sybase.PowerBuilder.PBString((string)as_sysgencode);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_userid__temp__;
			as_userid__temp__ = new Sybase.PowerBuilder.PBString((string)as_userid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_vcproc_rbt(as_wspass__temp__, adtm_sysdate__temp__, as_sysgencode__temp__, as_coopid__temp__, as_userid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_get_period_date")]
		public virtual System.Int16 of_get_period_date(string as_wspass, System.Int16 ai_year, System.Int16 ai_period, string as_coop_id, ref System.DateTime adtm_begin_date, ref System.DateTime adtm_end_date)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBInt ai_period__temp__;
			ai_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_period);
			Sybase.PowerBuilder.PBString as_coop_id__temp__;
			as_coop_id__temp__ = new Sybase.PowerBuilder.PBString((string)as_coop_id);
			Sybase.PowerBuilder.PBDateTime adtm_begin_date__temp__ = adtm_begin_date;
			Sybase.PowerBuilder.PBDateTime adtm_end_date__temp__ = adtm_end_date;
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_get_period_date(as_wspass__temp__, ai_year__temp__, ai_period__temp__, as_coop_id__temp__, ref adtm_begin_date__temp__, ref adtm_end_date__temp__);
			adtm_begin_date = adtm_begin_date__temp__;
			adtm_end_date = adtm_end_date__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_vcproc_hat")]
		public virtual System.Int16 of_vcproc_hat(string as_wspass, System.DateTime adtm_sysdate, string as_sysgencode, string as_coopid, string as_userid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_sysdate__temp__;
			adtm_sysdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_sysdate);
			Sybase.PowerBuilder.PBString as_sysgencode__temp__;
			as_sysgencode__temp__ = new Sybase.PowerBuilder.PBString((string)as_sysgencode);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_userid__temp__;
			as_userid__temp__ = new Sybase.PowerBuilder.PBString((string)as_userid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_vcproc_hat(as_wspass__temp__, adtm_sysdate__temp__, as_sysgencode__temp__, as_coopid__temp__, as_userid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_gen_data_asset_liquidity")]
		public virtual System.Int16 of_gen_data_asset_liquidity(string as_wspass, System.Int16 ai_year, System.Int16 ai_period, string as_coopid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBInt ai_year__temp__;
			ai_year__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_year);
			Sybase.PowerBuilder.PBInt ai_period__temp__;
			ai_period__temp__ = new Sybase.PowerBuilder.PBInt((System.Int16)ai_period);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_gen_data_asset_liquidity(as_wspass__temp__, ai_year__temp__, ai_period__temp__, as_coopid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_vcproc_stk")]
		public virtual System.Int16 of_vcproc_stk(string as_wspass, System.DateTime adtm_sysdate, string as_sysgencode, string as_coopid, string as_userid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_sysdate__temp__;
			adtm_sysdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_sysdate);
			Sybase.PowerBuilder.PBString as_sysgencode__temp__;
			as_sysgencode__temp__ = new Sybase.PowerBuilder.PBString((string)as_sysgencode);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_userid__temp__;
			as_userid__temp__ = new Sybase.PowerBuilder.PBString((string)as_userid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_vcproc_stk(as_wspass__temp__, adtm_sysdate__temp__, as_sysgencode__temp__, as_coopid__temp__, as_userid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_vcproc_phy")]
		public virtual System.Int16 of_vcproc_phy(string as_wspass, System.DateTime adtm_sysdate, string as_sysgencode, string as_coopid, string as_userid)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_sysdate__temp__;
			adtm_sysdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_sysdate);
			Sybase.PowerBuilder.PBString as_sysgencode__temp__;
			as_sysgencode__temp__ = new Sybase.PowerBuilder.PBString((string)as_sysgencode);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString as_userid__temp__;
			as_userid__temp__ = new Sybase.PowerBuilder.PBString((string)as_userid);
			__retval__ = ((pbservice125.c__n_account)__nvo__).of_vcproc_phy(as_wspass__temp__, adtm_sysdate__temp__, as_sysgencode__temp__, as_coopid__temp__, as_userid__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
	}
} 