
// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		IBridge.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/04/28 14:22:50
// *******************************************

namespace Bridge.InstagramApi
{
	using Common;
	
	/// <summary>
	/// 
	/// </summary>
	internal interface IBridge
	{
		/// <summary>
		/// 初始化
		/// </summary>
		void Init(IInitListener listener);

		/// <summary>
		/// 是否下载了ins
		/// </summary>
		/// <returns></returns>
		bool IsInstalled();

#if UNITY_IOS

		/// <summary>
		/// 启动 Instagram 应用。
		/// </summary>
		bool OpenApp();

		/// <summary>
		/// 启动 Instagram 应用并打开相机视图；若是没有相机的设备，则打开照片库。
		/// </summary>
		bool OpenAppAndCamara();

		/// <summary>
		/// 启动 Instagram 应用并加载与指定编号值相符的帖子。
		/// </summary>
		/// <param name="mediaId">帖子编号值</param>
		/// <returns></returns>
		bool OpenAppAndMedia(int mediaId);

		/// <summary>
		/// 启动 Instagram 应用并加载与指定帐号值相符的 Instagram 用户。
		/// </summary>
		/// <param name="username">用户帐号值</param>
		/// <returns></returns>
		bool OpenAppAndUser(string username);

		/// <summary>
		/// 启动 Instagram 应用并加载与指定编号值相符的位置动态。
		/// </summary>
		/// <param name="locationId">位置动态编号值</param>
		/// <returns></returns>
		bool OpenAppAndLocations(int locationId);

		/// <summary>
		/// 启动 Instagram 应用并加载与指定名称值相符的话题标签页面。
		/// </summary>
		/// <param name="tagName">话题标签页面名称</param>
		/// <returns></returns>
		bool OpenAppAndTags(string tagName);

		/// <summary>
		/// 分享图片
		/// </summary>
		/// <param name="imageData">图片数据</param>
		/// <param name="listener">拉起分享窗口事件</param>
		void ShareImage(byte[] imageData, IShareListener listener);

#endif

		/// <summary>
		/// 分享图片
		/// </summary>
		/// <param name="imagePath">图片本地地址</param>
		/// <param name="listener">拉起分享窗口事件</param>
		void ShareImage(string imagePath, IShareListener listener);

		/// <summary>
		/// 分享链接
		/// </summary>
		/// <param name="linkUrl">链接地址</param>
		/// <param name="listener">拉起分享窗口事件</param>
		void ShareLink(string linkUrl, IShareListener listener);

		/// <summary>
		/// 分享视频
		/// </summary>
		/// <param name="videoUrl">视频地址</param>
		/// <param name="listener">拉起分享窗口事件</param>
		void ShareVideo(string videoUrl, IShareListener listener);
	}
}
