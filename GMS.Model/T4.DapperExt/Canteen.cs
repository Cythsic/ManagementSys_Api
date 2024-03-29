
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2020-12-22 10:54:44
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失
//     作者：Harbour CTS
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dapper.Contrib.Extensions;

namespace GMS.Model
{	
   
   [Table("Canteen")]
    public partial class Canteen
    {

	   
/// <summary>
     	/// 自增主键
     	/// </summary>
	
	[Key]
	
	public int Id { get; set; }

		
/// <summary>
     	/// 菜名
     	/// </summary>
	
	public string Name { get; set; }

		
/// <summary>
     	/// 价格
     	/// </summary>
	
	public string Price { get; set; }

		
/// <summary>
     	/// 类型 0:正常 1:套餐 2:面食主料 3:面食配料
     	/// </summary>
	
	public string Type { get; set; }

		
/// <summary>
     	/// 星期几
     	/// </summary>
	
	public string Week { get; set; }

		
/// <summary>
     	/// 区域名称
     	/// </summary>
	
	public string AreaName { get; set; }

		
/// <summary>
     	/// 图片地址
     	/// </summary>
	
	public string ImgUrl { get; set; }

		
/// <summary>
     	/// 排序
     	/// </summary>
	
	public int? SortOrder { get; set; }

		
/// <summary>
     	/// 创建时间
     	/// </summary>
	
	public DateTime? CreateTime { get; set; }

		
/// <summary>
     	/// 最后修改时间
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

		
/// <summary>
     	/// 停用标识符
     	/// </summary>
	
	public bool? IsStop { get; set; }

		   
    }
}

