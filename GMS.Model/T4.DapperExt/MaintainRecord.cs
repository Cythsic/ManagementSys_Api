
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2020-12-22 18:12:24
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失
//     作者：Harbour CTS
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dapper.Contrib.Extensions;

namespace GMS.Model
{	
   
   [Table("MaintainRecord")]
    public partial class MaintainRecord
    {

	   
/// <summary>
     	/// 
     	/// </summary>
	
	public int id { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string EventNumber { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string UserName { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string UserID { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string RoomNum { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string EventStatus { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string Record { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public DateTime? StartTime { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public DateTime? OrderTime { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public DateTime? EndTime { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string OrderID { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string ProcessID { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public string Picture { get; set; }

		
/// <summary>
     	/// 
     	/// </summary>
	
	public int? IsStop { get; set; }

		   
    }
}
