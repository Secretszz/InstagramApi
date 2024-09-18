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
		private const string ManagerClassName = "com.bridge.instagram.InstagramApi";
		private AndroidJavaObject api;
		private AndroidJavaObject currentActivity;
		
		void IBridge.Init(IBridgeListener listener)
		{
			AndroidJavaClass unityPlayer = new AndroidJavaClass(UnityPlayerClassName);
			currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass jc = new AndroidJavaClass(ManagerClassName);
			api = jc.CallStatic<AndroidJavaObject>("getInstance");
			api.Call("init", currentActivity, new BridgeCallback(listener));
		}

		bool IBridge.IsInstalled()
		{
			return api != null && api.Call<bool>("isInstalledInstagram", currentActivity);
		}

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

		void IBridge.ShareImage(byte[] imageData, IBridgeListener listener)
		{
			listener.OnError(-1, "Only available on iOS");
		}

		void IBridge.ShareImage(string imagePath, IBridgeListener listener)
		{
			if (api == null)
			{
				listener.OnError(-1, "empty bridge");
			}
			else
			{
				api?.Call("shareImage", currentActivity, imagePath, new BridgeCallback(listener));
			}
		}

		void IBridge.ShareLink(string linkUrl, IBridgeListener listener)
		{
			if (api == null)
			{
				listener.OnError(-1, "empty bridge");
			}
			else
			{
				api?.Call("ShareLink", currentActivity, linkUrl, new BridgeCallback(listener));
			}
		}

		void IBridge.ShareVideo(string videoUrl, IBridgeListener listener)
		{
			if (api == null)
			{
				listener.OnError(-1, "empty bridge");
			}
			else
			{
				api?.Call("shareVideo", currentActivity, videoUrl, new BridgeCallback(listener));
			}
		}
	}
}

#endif