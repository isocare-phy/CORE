using System.ServiceModel; 
using System.Runtime.Serialization; 
namespace Sybase.PowerBuilder.WCFNVO
{
	[DataContract]
	public struct str_yrcfbal_proc
	{
		internal str_yrcfbal_proc(c__str_yrcfbal_proc __x__)
		{
			CopyFrom(out this, __x__);
		}
		internal void CopyFrom(c__str_yrcfbal_proc __x__)
		{
			CopyFrom(out this, __x__);
		}
		[DataMember] 
		public string xml_option;
		internal void CopyTo(c__str_yrcfbal_proc __x__)
		{
			__x__.xml_option = xml_option;
		}
		internal static void CopyFrom(out str_yrcfbal_proc __this__, c__str_yrcfbal_proc __x__)
		{
			__this__.xml_option = __x__.xml_option;
		}
		public static explicit operator object[](str_yrcfbal_proc __this__)
		{
			return new object[] {
				__this__.xml_option
			};
		}
	}
} 