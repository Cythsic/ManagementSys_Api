//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2019-08-12 11:19:26
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失
//     作者：Harbour CTS
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dapper.Contrib.Extensions;

namespace GMS.Model
{	
   
   [Table("V_PubFunction_Parent")]
    public partial class V_PubFunction_Parent
    {

	   /// <summary>
     	/// 
     	/// </summary>
		public string FunctionCode { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string FunctionEnglish { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string FunctionChina { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string FunctionDescrip { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string ParentCode { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public bool MenuFlag { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public bool? StopFlag { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string URLString { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public DateTime? editdate { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string editor { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public int? sortidx { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string target { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string MenuIcon { get; set; }

		/// <summary>
     	/// 
     	/// </summary>
		public string parentName { get; set; }

		   
    }
}

