// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		AndroidBridgeImpl.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/04/28 14:25:23
// *******************************************

#if UNITY_ANDROID

namespace Bridge.InstagramApi
{
	using Common;
	using UnityEngine;

	/// <summary>
	/// 
	/// </summary>
	internal class AndroidBridgeImpl : IBridge
	{
		private const string UnityPlayerClassName = "com.unity3d.player.UnityPlayer";
		private const string BridgeClassName = "com.bridge.ins.InstagramApi";
		private AndroidJavaClass bridge;
		private AndroidJavaObject currentActivity;
		
		void IBridge.Init(IBridgeListener listener)
		{
			AndroidJavaClass unityPlayer = new AndroidJavaClass(UnityPlayerClassName);
			currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			bridge = new AndroidJavaClass(BridgeClassName);
			bridge.CallStatic("init", currentActivity, new BridgeCallback(listener));
		}

		bool IBridge.IsInstalled()
		{
			return bridge != null && bridge.CallStatic<bool>("isInstalledInstagram", currentActivity);
		}

		void IBridge.ShareImage(string imagePath, IBridgeListener listener)
		{
			if (bridge == null)
			{
				listener.OnError(-1, "empty bridge");
			}
			else
			{
				bridge?.CallStatic("shareImage", currentActivity, imagePath, new BridgeCallback(listener));
			}
		}

		void IBridge.ShareLink(string linkUrl, IBridgeListener listener)
		{
			if (bridge == null)
			{
				listener.OnError(-1, "empty bridge");
			}
			else
			{
				bridge?.CallStatic("ShareLink", currentActivity, linkUrl, new BridgeCallback(listener));
			}
		}

		void IBridge.ShareVideo(string videoUrl, IBridgeListener listener)
		{
			if (bridge == null)
			{
				listener.OnError(-1, "empty bridge");
			}
			else
			{
				bridge?.CallStatic("shareVideo", currentActivity, videoUrl, new BridgeCallback(listener));
			}
		}
	}
}

#endif