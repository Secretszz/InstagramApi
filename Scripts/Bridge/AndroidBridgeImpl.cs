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

namespace Instagram.Api
{
	using UnityEngine;

	/// <summary>
	/// 
	/// </summary>
	internal class AndroidBridgeImpl : IBridge
	{
		private const string UnityPlayerClassName = "com.unity3d.player.UnityPlayer";
		private const string BridgeClassName = "com.ins.bridge.InsApiBridge";
		private const string InitClassName = "com.ins.bridge.IInitListener";
		private const string ShareClassName = "com.ins.bridge.IShareListener";
		private AndroidJavaClass bridge;
		private AndroidJavaObject currentActivity;
		
		void IBridge.Init(IInitListener listener)
		{
			AndroidJavaClass unityPlayer = new AndroidJavaClass(UnityPlayerClassName);
			currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			bridge = new AndroidJavaClass(BridgeClassName);
			bridge.CallStatic("init", currentActivity, new InitCallback(listener));
		}

		bool IBridge.IsInstalled()
		{
			return bridge != null && bridge.CallStatic<bool>("isInstalledInstagram", currentActivity);
		}

		void IBridge.ShareImage(string imagePath, IShareListener listener)
		{
			if (bridge == null)
			{
				listener.OnError(-1, "empty bridge");
			}
			else
			{
				bridge?.CallStatic("shareImage", currentActivity, imagePath, new ShareCallback(listener));
			}
		}

		void IBridge.ShareLink(string linkUrl, IShareListener listener)
		{
			if (bridge == null)
			{
				listener.OnError(-1, "empty bridge");
			}
			else
			{
				bridge?.CallStatic("ShareLink", currentActivity, linkUrl, new ShareCallback(listener));
			}
		}

		void IBridge.ShareVideo(string videoUrl, IShareListener listener)
		{
			if (bridge == null)
			{
				listener.OnError(-1, "empty bridge");
			}
			else
			{
				bridge?.CallStatic("shareVideo", currentActivity, videoUrl, new ShareCallback(listener));
			}
		}
		
		/// <summary>
		/// 初始化回调
		/// </summary>
		private class InitCallback : AndroidJavaProxy
		{
			public InitCallback(IInitListener listener) : base(InitClassName)
			{
				this.listener = listener;
			}

			private IInitListener listener;

			public void onSuccess()
			{
				listener?.OnSuccess();
			}

			public void onError(int errorCode, string errorMessage)
			{
				listener?.OnError(errorCode, errorMessage);
			}
		}

		/// <summary>
		/// 分享回调
		/// </summary>
		private class ShareCallback : AndroidJavaProxy
		{
			public ShareCallback(IShareListener listener) : base(ShareClassName)
			{
				this.listener = listener;
			}

			private IShareListener listener;

			/// <summary>
			/// 分享成功回调
			/// </summary>
			public void onSuccess()
			{
				listener?.OnSuccess();
			}

			/// <summary>
			/// 分享失败回调
			/// </summary>
			/// <param name="errorCode">错误码</param>
			/// <param name="errorMessage">错误信息</param>
			public void onError(int errorCode, string errorMessage)
			{
				listener?.OnError(errorCode, errorMessage);
			}
		}
	}
}

#endif