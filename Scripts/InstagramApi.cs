// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		InstagramApi.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/04/28 14:45:51
// *******************************************

namespace Bridge.InstagramApi
{
	using Common;

	/// <summary>
	/// 
	/// </summary>
	public static class InstagramApi
	{
		private static IBridge _bridge;

		/// <summary>
		/// SDK桥接文件
		/// </summary>
		private static IBridge bridgeImpl
		{
			get
			{
				if (_bridge == null)
				{
#if UNITY_IOS && !UNITY_EDITOR
					_bridge = new iOSBridgeImpl();
#elif UNITY_ANDROID && !UNITY_EDITOR
					_bridge = new AndroidBridgeImpl();
#else
					_bridge = new EditorBridgeImpl();
#endif
				}

				return _bridge;
			}
		}
		
		public static void Init(IBridgeListener listener)
		{
			bridgeImpl.Init(listener);
		}

		public static bool IsInstalled()
		{
			return bridgeImpl.IsInstalled();
		}

#if UNITY_IOS

		public static bool OpenApp()
		{
			return bridgeImpl.OpenApp();
		}

		public static bool OpenAppAndCamara()
		{
			return bridgeImpl.OpenAppAndCamara();
		}

		public static bool OpenAppAndMedia(int mediaId)
		{
			return bridgeImpl.OpenAppAndMedia(mediaId);
		}

		public static bool OpenAppAndUser(string username)
		{
			return bridgeImpl.OpenAppAndUser(username);
		}

		public static bool OpenAppAndLocations(int locationId)
		{
			return bridgeImpl.OpenAppAndLocations(locationId);
		}

		public static bool OpenAppAndTags(string tagName)
		{
			return bridgeImpl.OpenAppAndTags(tagName);
		}

		public static void ShareImage(byte[] imageData, IBridgeListener listener)
		{
			bridgeImpl.ShareImage(imageData, listener);
		}

#endif

		public static void ShareImage(string imagePath, IBridgeListener listener)
		{
			bridgeImpl.ShareImage(imagePath, listener);
		}

		public static void ShareLink(string linkUrl, IBridgeListener listener)
		{
			bridgeImpl.ShareLink(linkUrl, listener);
		}

		public static void ShareVideo(string videoUrl, IBridgeListener listener)
		{
			bridgeImpl.ShareVideo(videoUrl, listener);
		}
	}
}