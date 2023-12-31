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
	[ServiceContract(Name="n_common",Namespace="http://tempurl.org")]
	public class n_common : System.IDisposable 
	{
		internal pbservice125.c__n_common __nvo__;
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
		public n_common()
		{
			
			c__pbservice125.InitAssembly();
			__nvo__ = (pbservice125.c__n_common)Sybase.PowerBuilder.WPF.PBSession.CurrentSession.CreateInstance(typeof(pbservice125.c__n_common));
			c__pbservice125.RestoreOldSession();
		}
		internal n_common(pbservice125.c__n_common nvo)
		{
			__nvo__ = nvo;
		}
		[OperationContract(Name="of_getnextworkday")]
		public virtual System.Int16 of_getnextworkday(string as_wspass, System.DateTime adtm_fromdate, ref System.DateTime adtm_nextworkdate)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_fromdate__temp__;
			adtm_fromdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_fromdate);
			Sybase.PowerBuilder.PBDateTime adtm_nextworkdate__temp__ = adtm_nextworkdate;
			__retval__ = ((pbservice125.c__n_common)__nvo__).of_getnextworkday(as_wspass__temp__, adtm_fromdate__temp__, ref adtm_nextworkdate__temp__);
			adtm_nextworkdate = adtm_nextworkdate__temp__;
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_getnewdocno")]
		public virtual string of_getnewdocno(string as_wspass, string as_coopid, string doccode)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			string __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_coopid__temp__;
			as_coopid__temp__ = new Sybase.PowerBuilder.PBString((string)as_coopid);
			Sybase.PowerBuilder.PBString doccode__temp__;
			doccode__temp__ = new Sybase.PowerBuilder.PBString((string)doccode);
			__retval__ = ((pbservice125.c__n_common)__nvo__).of_getnewdocno(as_wspass__temp__, as_coopid__temp__, doccode__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_isworkingdate")]
		public virtual bool of_isworkingdate(string as_wspass, System.DateTime adtm_checkdate)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			bool __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDateTime adtm_checkdate__temp__;
			adtm_checkdate__temp__ = new Sybase.PowerBuilder.PBDateTime((System.DateTime)adtm_checkdate);
			__retval__ = ((pbservice125.c__n_common)__nvo__).of_isworkingdate(as_wspass__temp__, adtm_checkdate__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_getdddwxml")]
		public virtual string of_getdddwxml(string as_wspass, string as_dddwobj)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			string __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBString as_dddwobj__temp__;
			as_dddwobj__temp__ = new Sybase.PowerBuilder.PBString((string)as_dddwobj);
			__retval__ = ((pbservice125.c__n_common)__nvo__).of_getdddwxml(as_wspass__temp__, as_dddwobj__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_dwexportexcel_etn")]
		public virtual System.Int16 of_dwexportexcel_etn(string as_wspass, Sybase.PowerBuilder.WCFNVO.str_rptexcel astr_rptexcel)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_rptexcel astr_rptexcel__temp__ = new c__str_rptexcel(); astr_rptexcel.CopyTo(astr_rptexcel__temp__);
			__retval__ = ((pbservice125.c__n_common)__nvo__).of_dwexportexcel_etn(as_wspass__temp__, astr_rptexcel__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_dwexportexcel_rpt")]
		public virtual System.Int16 of_dwexportexcel_rpt(string as_wspass, ref Sybase.PowerBuilder.WCFNVO.str_rptexcel astr_rptexcel)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.Int16 __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			c__str_rptexcel astr_rptexcel__temp__ = new c__str_rptexcel(); astr_rptexcel.CopyTo(astr_rptexcel__temp__);
			__retval__ = ((pbservice125.c__n_common)__nvo__).of_dwexportexcel_rpt_2_2005728673(as_wspass__temp__, ref astr_rptexcel__temp__);
			astr_rptexcel.CopyFrom(astr_rptexcel__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
		[OperationContract(Name="of_lastdayofmonth")]
		public virtual System.DateTime of_lastdayofmonth(string as_wspass, System.DateTime ad_source)
		{
			c__pbservice125.InitSession(__nvo__.Session);
			System.DateTime __retval__;
			Sybase.PowerBuilder.PBString as_wspass__temp__;
			as_wspass__temp__ = new Sybase.PowerBuilder.PBString((string)as_wspass);
			Sybase.PowerBuilder.PBDate ad_source__temp__;
			ad_source__temp__ = new Sybase.PowerBuilder.PBDate((System.DateTime)ad_source);
			__retval__ = ((pbservice125.c__n_common)__nvo__).of_lastdayofmonth(as_wspass__temp__, ad_source__temp__);
			c__pbservice125.RestoreOldSession();
			return __retval__;
		}
	}
} 