
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2020-12-12 08:43:16
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失
//     作者：Harbour CTS
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dapper.Contrib.Extensions;

namespace GMS.Model
{	
   
   [Table("DictManage")]
    public partial class DictManage
    {

	   
/// <summary>
     	/// 
     	/// </summary>
	
	[Key]
	
	public int Id { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public int? DictId { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string Title { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string Value { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public int? SortOrder { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public bool? Status { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string Description { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public bool? DelFlag { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public DateTime? CreateTime { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string CreateUser { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public DateTime? ModifyDate { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string ModifyUser { get; set; }

		   
    }
}
