using System.ServiceModel; 
using System.Runtime.Serialization; 
namespace Sybase.PowerBuilder.WCFNVO
{
	[DataContract]
	public struct str_slipadjust
	{
		internal str_slipadjust(c__str_slipadjust __x__)
		{
			CopyFrom(out this, __x__);
		}
		internal void CopyFrom(c__str_slipadjust __x__)
		{
			CopyFrom(out this, __x__);
		}
		[DataMember] 
		public string member_no;
		[DataMember] 
		public string memcoop_id;
		[DataMember] 
		public string adjtype_code;
		[DataMember] 
		public string ref_slipno;
		[DataMember] 
		public string ref_slipcoopid;
		[DataMember] 
		public System.DateTime adjslip_date;
		[DataMember] 
		public System.DateTime operate_date;
		[DataMember] 
		public string xml_sliphead;
		[DataMember] 
		public string xml_slipdet;
		[DataMember] 
		public string entry_id;
		[DataMember] 
		public string coop_id;
		internal void CopyTo(c__str_slipadjust __x__)
		{
			__x__.member_no = member_no;
			__x__.memcoop_id = memcoop_id;
			__x__.adjtype_code = adjtype_code;
			__x__.ref_slipno = ref_slipno;
			__x__.ref_slipcoopid = ref_slipcoopid;
			__x__.adjslip_date = adjslip_date;
			__x__.operate_date = operate_date;
			__x__.xml_sliphead = xml_sliphead;
			__x__.xml_slipdet = xml_slipdet;
			__x__.entry_id = entry_id;
			__x__.coop_id = coop_id;
		}
		internal static void CopyFrom(out str_slipadjust __this__, c__str_slipadjust __x__)
		{
			__this__.member_no = __x__.member_no;
			__this__.memcoop_id = __x__.memcoop_id;
			__this__.adjtype_code = __x__.adjtype_code;
			__this__.ref_slipno = __x__.ref_slipno;
			__this__.ref_slipcoopid = __x__.ref_slipcoopid;
			__this__.adjslip_date = __x__.adjslip_date;
			__this__.operate_date = __x__.operate_date;
			__this__.xml_sliphead = __x__.xml_sliphead;
			__this__.xml_slipdet = __x__.xml_slipdet;
			__this__.entry_id = __x__.entry_id;
			__this__.coop_id = __x__.coop_id;
		}
		public static explicit operator object[](str_slipadjust __this__)
		{
			return new object[] {
				__this__.member_no
				,__this__.memcoop_id
				,__this__.adjtype_code
				,__this__.ref_slipno
				,__this__.ref_slipcoopid
				,__this__.adjslip_date
				,__this__.operate_date
				,__this__.xml_sliphead
				,__this__.xml_slipdet
				,__this__.entry_id
				,__this__.coop_id
			};
		}
	}
} 