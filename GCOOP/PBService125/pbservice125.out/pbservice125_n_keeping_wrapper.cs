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
	[ServiceContract(Name="n_keeping",Namespace="http://tempurl.org")]
	public class n_keeping : System.IDisposable 
	{
		internal pbservice125.c__n_keeping __nvo__;
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
		public n_keeping()
		{
			
			c__pbservice125.InitAssembly();
			__nvo__ = (pbservice125.c__n_keeping)Sybase.PowerBuilder.WPF.PBSession.CurrentSession.CreateInstance(typeof(pbservice125.c__n_keeping));
			c__pbservice125.RestoreOldSession();
		}
		internal n_keeping(pbservice125.c__n_keeping nvo)
		{
			__nvo__ = nvo;
		}
		[OperationContract(Name="of_test")]
		public virtual string of_test(string as_test)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			string __retval__;
			Sybase.PowerBuilder.PBString as_test__temp__;
			as_test__temp__ = new Sybase.PowerBuilder.PBString((string)as_test);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_test(as_test__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_adjust_monthly")]
		public virtual System.Int16 of_save_adjust_monthly(string as_wspass, Sybase.PowerBuilder.WCFNVO.str_keep_adjust astr_keep_adjust)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_keep_adjust astr_keep_adjust__temp__ = new c__str_keep_adjust(); astr_keep_adjust.CopyTo(astr_keep_adjust__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_save_adjust_monthly(as_wspass__temp__, astr_keep_adjust__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_adjust_monthly")]
		public virtual System.Int16 of_init_adjust_monthly(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_keep_adjust astr_keep_adjust)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_keep_adjust astr_keep_adjust__temp__ = new c__str_keep_adjust(); astr_keep_adjust.CopyTo(astr_keep_adjust__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_init_adjust_monthly_2_1858027769(as_wspass__temp__, ref astr_keep_adjust__temp__);
			astr_keep_adjust.CopyFrom(astr_keep_adjust__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_adjmth_ccl")]
		public virtual System.Int16 of_save_adjmth_ccl(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_keep_adjust astr_keep_adjust)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_keep_adjust astr_keep_adjust__temp__ = new c__str_keep_adjust(); astr_keep_adjust.CopyTo(astr_keep_adjust__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_save_adjmth_ccl_2_1858027769(as_wspass__temp__, ref astr_keep_adjust__temp__);
			astr_keep_adjust.CopyFrom(astr_keep_adjust__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_kpmast_detail")]
		public virtual System.Int16 of_init_kpmast_detail(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_keep_detail astr_keep_detail)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_keep_detail astr_keep_detail__temp__ = new c__str_keep_detail(); astr_keep_detail.CopyTo(astr_keep_detail__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_init_kpmast_detail_2_1739098417(as_wspass__temp__, ref astr_keep_detail__temp__);
			astr_keep_detail.CopyFrom(astr_keep_detail__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_adjust_monthly_prc_option")]
		public virtual System.Int16 of_init_adjust_monthly_prc_option(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_keep_adjust astr_keep_adjust)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_keep_adjust astr_keep_adjust__temp__ = new c__str_keep_adjust(); astr_keep_adjust.CopyTo(astr_keep_adjust__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_init_adjust_monthly_prc_option_2_1858027769(as_wspass__temp__, ref astr_keep_adjust__temp__);
			astr_keep_adjust.CopyFrom(astr_keep_adjust__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_kptp_ccl")]
		public virtual System.Int16 of_save_kptp_ccl(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_keep_adjust astr_keep_adjust)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_keep_adjust astr_keep_adjust__temp__ = new c__str_keep_adjust(); astr_keep_adjust.CopyTo(astr_keep_adjust__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_save_kptp_ccl_2_1858027769(as_wspass__temp__, ref astr_keep_adjust__temp__);
			astr_keep_adjust.CopyFrom(astr_keep_adjust__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_slcls_year_lon_opt")]
		public virtual System.Int16 of_slcls_year_lon_opt(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_slcls_proc astr_slcls_proc)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_slcls_proc astr_slcls_proc__temp__ = new c__str_slcls_proc(); astr_slcls_proc.CopyTo(astr_slcls_proc__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_slcls_year_lon_opt_2_31616204(as_wspass__temp__, ref astr_slcls_proc__temp__);
			astr_slcls_proc.CopyFrom(astr_slcls_proc__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_slcls_year_lon_opt")]
		public virtual System.Int16 of_init_slcls_year_lon_opt(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_slcls_proc astr_slcls_proc)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_slcls_proc astr_slcls_proc__temp__ = new c__str_slcls_proc(); astr_slcls_proc.CopyTo(astr_slcls_proc__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_init_slcls_year_lon_opt_2_31616204(as_wspass__temp__, ref astr_slcls_proc__temp__);
			astr_slcls_proc.CopyFrom(astr_slcls_proc__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_req_expense")]
		public virtual System.Int16 of_save_req_expense(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_keep_request astr_keep_request)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_keep_request astr_keep_request__temp__ = new c__str_keep_request(); astr_keep_request.CopyTo(astr_keep_request__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_save_req_expense_2_657833563(as_wspass__temp__, ref astr_keep_request__temp__);
			astr_keep_request.CopyFrom(astr_keep_request__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_rcv_post")]
		public virtual System.Int16 of_init_rcv_post(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_keep_proc astr_keep_proc)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_keep_proc astr_keep_proc__temp__ = new c__str_keep_proc(); astr_keep_proc.CopyTo(astr_keep_proc__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_init_rcv_post_2_707119728(as_wspass__temp__, ref astr_keep_proc__temp__);
			astr_keep_proc.CopyFrom(astr_keep_proc__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_req_expense")]
		public virtual System.Int16 of_init_req_expense(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_keep_request astr_keep_request)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_keep_request astr_keep_request__temp__ = new c__str_keep_request(); astr_keep_request.CopyTo(astr_keep_request__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_init_req_expense_2_657833563(as_wspass__temp__, ref astr_keep_request__temp__);
			astr_keep_request.CopyFrom(astr_keep_request__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_kptp_ccl")]
		public virtual System.Int16 of_init_kptp_ccl(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_keep_adjust astr_keep_adjust)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_keep_adjust astr_keep_adjust__temp__ = new c__str_keep_adjust(); astr_keep_adjust.CopyTo(astr_keep_adjust__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_init_kptp_ccl_2_1858027769(as_wspass__temp__, ref astr_keep_adjust__temp__);
			astr_keep_adjust.CopyFrom(astr_keep_adjust__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_adjmth_ccl")]
		public virtual System.Int16 of_init_adjmth_ccl(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_keep_adjust astr_keep_adjust)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_keep_adjust astr_keep_adjust__temp__ = new c__str_keep_adjust(); astr_keep_adjust.CopyTo(astr_keep_adjust__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_init_adjmth_ccl_2_1858027769(as_wspass__temp__, ref astr_keep_adjust__temp__);
			astr_keep_adjust.CopyFrom(astr_keep_adjust__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_mr_opr_proc")]
		public virtual System.Int16 of_init_mr_opr_proc(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_money_return_xml astr_mr_xml)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_money_return_xml astr_mr_xml__temp__ = new c__str_money_return_xml(); astr_mr_xml.CopyTo(astr_mr_xml__temp__);
			__retval__ = ((pbservice125.c__n_keeping)__nvo__).of_init_mr_opr_proc_2_1452393809(as_wspass__temp__, ref astr_mr_xml__temp__);
			astr_mr_xml.CopyFrom(astr_mr_xml__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
	}
} 