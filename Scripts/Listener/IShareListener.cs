
// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		IShareListener.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/04/28 14:43:29
// *******************************************

namespace Instagram.Api
{
	/// <summary>
	/// 
	/// </summary>
	public interface IShareListener
	{
		void OnSuccess();

		void OnError(int errCode, string errMsg);
	}
}