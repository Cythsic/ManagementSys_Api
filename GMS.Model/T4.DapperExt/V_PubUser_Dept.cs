//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2019-08-09 15:16:08
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失
//     作者：Harbour CTS
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dapper.Contrib.Extensions;

namespace GMS.Model
{	
   
   [Table("V_PubUser_Dept")]
    public partial class V_PubUser_Dept
    {

	   /// <summary>
     	/// 
     	/// </summary>
		public int Id { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string UserCode { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string UserName { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string RealName { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string UserPwd { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public bool Sex { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string IdentityNo { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public DateTime? Birthday { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string DeptCode { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public bool ManagerFlag { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string Tel { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string EMail { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string QQ { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string Remark { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public bool StopFlag { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string Crid { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public DateTime? Crdt { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string Lmid { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public DateTime? Lmdt { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public DateTime? LoginDate { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string ProvinceCode { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string CityCode { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string RegionCode { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string UserAddress { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string Wxcode { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string HeadUrl { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string DeptName { get; set; }

		   
    }
}

