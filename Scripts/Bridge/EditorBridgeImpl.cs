// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		EditorBridgeImpl.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/04/28 14:46:39
// *******************************************

namespace Instagram.Api
{
	/// <summary>
	/// 
	/// </summary>
	public class EditorBridgeImpl : IBridge
	{
		void IBridge.Init(IInitListener listener)
		{
			listener.OnSuccess();
		}

		bool IBridge.IsInstalled()
		{
			return false;
		}

#if UNITY_IOS

		bool IBridge.OpenApp()
		{
			return false;
		}

		bool IBridge.OpenAppAndCamara()
		{
			return false;
		}

		bool IBridge.OpenAppAndMedia(int mediaId)
		{
			return false;
		}

		bool IBridge.OpenAppAndUser(string username)
		{
			return false;
		}

		bool IBridge.OpenAppAndLocations(int locationId)
		{
			return false;
		}

		bool IBridge.OpenAppAndTags(string tagName)
		{
			return false;
		}

		void IBridge.ShareImage(byte[] imageData, IShareListener listener)
		{
			listener.OnSuccess();
		}

#endif

		void IBridge.ShareImage(string imagePath, IShareListener listener)
		{
			listener.OnSuccess();
		}

		void IBridge.ShareLink(string linkUrl, IShareListener listener)
		{
			listener.OnSuccess();
		}

		void IBridge.ShareVideo(string videoUrl, IShareListener listener)
		{
			listener.OnSuccess();
		}
	}
}