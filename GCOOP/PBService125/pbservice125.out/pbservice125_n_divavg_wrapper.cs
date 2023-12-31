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
	[ServiceContract(Name="n_divavg",Namespace="http://tempurl.org")]
	public class n_divavg : System.IDisposable 
	{
		internal pbservice125.c__n_divavg __nvo__;
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
		public n_divavg()
		{
			
			c__pbservice125.InitAssembly();
			__nvo__ = (pbservice125.c__n_divavg)Sybase.PowerBuilder.WPF.PBSession.CurrentSession.CreateInstance(typeof(pbservice125.c__n_divavg));
			c__pbservice125.RestoreOldSession();
		}
		internal n_divavg(pbservice125.c__n_divavg nvo)
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
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_test(as_test__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_memdet_methpay")]
		public virtual System.Int16 of_save_memdet_methpay(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_det astr_divsrv_det)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_det astr_divsrv_det__temp__ = new c__str_divsrv_det(); astr_divsrv_det.CopyTo(astr_divsrv_det__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_save_memdet_methpay_2_181045906(as_wspass__temp__, ref astr_divsrv_det__temp__);
			astr_divsrv_det.CopyFrom(astr_divsrv_det__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_prc_methpay_opt")]
		public virtual System.Int16 of_prc_methpay_opt(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_proc astr_divsrv_proc)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_proc astr_divsrv_proc__temp__ = new c__str_divsrv_proc(); astr_divsrv_proc.CopyTo(astr_divsrv_proc__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_prc_methpay_opt_2_1679992937(as_wspass__temp__, ref astr_divsrv_proc__temp__);
			astr_divsrv_proc.CopyFrom(astr_divsrv_proc__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_reqchg")]
		public virtual System.Int16 of_init_reqchg(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_req astr_divsrv_req)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_req astr_divsrv_req__temp__ = new c__str_divsrv_req(); astr_divsrv_req.CopyTo(astr_divsrv_req__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_init_reqchg_2_181061149(as_wspass__temp__, ref astr_divsrv_req__temp__);
			astr_divsrv_req.CopyFrom(astr_divsrv_req__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_methpay_ccl")]
		public virtual System.Int16 of_init_methpay_ccl(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_req astr_divsrv_req)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_req astr_divsrv_req__temp__ = new c__str_divsrv_req(); astr_divsrv_req.CopyTo(astr_divsrv_req__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_init_methpay_ccl_2_181061149(as_wspass__temp__, ref astr_divsrv_req__temp__);
			astr_divsrv_req.CopyFrom(astr_divsrv_req__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_slip_grp")]
		public virtual System.Int16 of_init_slip_grp(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_oper astr_divavg_oper)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_oper astr_divavg_oper__temp__ = new c__str_divsrv_oper(); astr_divavg_oper.CopyTo(astr_divavg_oper__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_init_slip_grp_2_1679954507(as_wspass__temp__, ref astr_divavg_oper__temp__);
			astr_divavg_oper.CopyFrom(astr_divavg_oper__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_slip_ccl")]
		public virtual System.Int16 of_init_slip_ccl(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_oper astr_divavg_oper)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_oper astr_divavg_oper__temp__ = new c__str_divsrv_oper(); astr_divavg_oper.CopyTo(astr_divavg_oper__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_init_slip_ccl_2_1679954507(as_wspass__temp__, ref astr_divavg_oper__temp__);
			astr_divavg_oper.CopyFrom(astr_divavg_oper__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_slip")]
		public virtual System.Int16 of_init_slip(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_oper astr_divavg_oper)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_oper astr_divavg_oper__temp__ = new c__str_divsrv_oper(); astr_divavg_oper.CopyTo(astr_divavg_oper__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_init_slip_2_1679954507(as_wspass__temp__, ref astr_divavg_oper__temp__);
			astr_divavg_oper.CopyFrom(astr_divavg_oper__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_detail")]
		public virtual System.Int16 of_init_detail(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_det astr_divsrv_det)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_det astr_divsrv_det__temp__ = new c__str_divsrv_det(); astr_divsrv_det.CopyTo(astr_divsrv_det__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_init_detail_2_181045906(as_wspass__temp__, ref astr_divsrv_det__temp__);
			astr_divsrv_det.CopyFrom(astr_divsrv_det__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_memdet_methpay")]
		public virtual System.Int16 of_init_memdet_methpay(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_det astr_divsrv_det)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_det astr_divsrv_det__temp__ = new c__str_divsrv_det(); astr_divsrv_det.CopyTo(astr_divsrv_det__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_init_memdet_methpay_2_181045906(as_wspass__temp__, ref astr_divsrv_det__temp__);
			astr_divsrv_det.CopyFrom(astr_divsrv_det__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_slip")]
		public virtual System.Int16 of_save_slip(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_oper astr_divavg_oper)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_oper astr_divavg_oper__temp__ = new c__str_divsrv_oper(); astr_divavg_oper.CopyTo(astr_divavg_oper__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_save_slip_2_1679954507(as_wspass__temp__, ref astr_divavg_oper__temp__);
			astr_divavg_oper.CopyFrom(astr_divavg_oper__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_slip_ccl")]
		public virtual System.Int16 of_save_slip_ccl(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_oper astr_divavg_oper)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_oper astr_divavg_oper__temp__ = new c__str_divsrv_oper(); astr_divavg_oper.CopyTo(astr_divavg_oper__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_save_slip_ccl_2_1679954507(as_wspass__temp__, ref astr_divavg_oper__temp__);
			astr_divavg_oper.CopyFrom(astr_divavg_oper__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_slip_grp")]
		public virtual System.Int16 of_save_slip_grp(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_oper astr_divavg_oper)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_oper astr_divavg_oper__temp__ = new c__str_divsrv_oper(); astr_divavg_oper.CopyTo(astr_divavg_oper__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_save_slip_grp_2_1679954507(as_wspass__temp__, ref astr_divavg_oper__temp__);
			astr_divavg_oper.CopyFrom(astr_divavg_oper__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_methpay_ccl")]
		public virtual System.Int16 of_save_methpay_ccl(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_req astr_divsrv_req)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_req astr_divsrv_req__temp__ = new c__str_divsrv_req(); astr_divsrv_req.CopyTo(astr_divsrv_req__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_save_methpay_ccl_2_181061149(as_wspass__temp__, ref astr_divsrv_req__temp__);
			astr_divsrv_req.CopyFrom(astr_divsrv_req__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_reqchg")]
		public virtual System.Int16 of_save_reqchg(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_req astr_divsrv_req)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_req astr_divsrv_req__temp__ = new c__str_divsrv_req(); astr_divsrv_req.CopyTo(astr_divsrv_req__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_save_reqchg_2_181061149(as_wspass__temp__, ref astr_divsrv_req__temp__);
			astr_divsrv_req.CopyFrom(astr_divsrv_req__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_init_methpay")]
		public virtual System.Int16 of_init_methpay(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_req astr_divsrv_req)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_req astr_divsrv_req__temp__ = new c__str_divsrv_req(); astr_divsrv_req.CopyTo(astr_divsrv_req__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_init_methpay_2_181061149(as_wspass__temp__, ref astr_divsrv_req__temp__);
			astr_divsrv_req.CopyFrom(astr_divsrv_req__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_save_methpay")]
		public virtual System.Int16 of_save_methpay(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_divsrv_req astr_divsrv_req)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_divsrv_req astr_divsrv_req__temp__ = new c__str_divsrv_req(); astr_divsrv_req.CopyTo(astr_divsrv_req__temp__);
			__retval__ = ((pbservice125.c__n_divavg)__nvo__).of_save_methpay_2_181061149(as_wspass__temp__, ref astr_divsrv_req__temp__);
			astr_divsrv_req.CopyFrom(astr_divsrv_req__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
	}
} 