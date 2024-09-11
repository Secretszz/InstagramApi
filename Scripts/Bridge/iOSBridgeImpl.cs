// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		iOSBridgeImpl.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/04/28 14:38:41
// *******************************************

#if UNITY_IOS

namespace Bridge.InstagramApi
{
	using Common;
	using System;
	using System.Runtime.InteropServices;
	using UnityEngine;

	/// <summary>
	/// 
	/// </summary>
	internal class iOSBridgeImpl : IBridge
	{
		[DllImport("__Internal")]
		private static extern bool ins_isIntalled();

		[DllImport("__Internal")]
		private static extern bool ins_openApp();

		[DllImport("__Internal")]
		private static extern bool ins_openAppAndCamara();

		[DllImport("__Internal")]
		private static extern bool ins_openAppAndMedia(int mediaId);

		[DllImport("__Internal")]
		private static extern bool ins_openAppAndUser(string username);

		[DllImport("__Internal")]
		private static extern bool ins_openAppAndLocations(int locationId);

		[DllImport("__Internal")]
		private static extern bool ins_openAppAndTags(string tagName);

		[DllImport("__Internal")]
		private static extern void ins_openDocumentSharebyData(IntPtr imageData, int length, U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Cancel onCancel, U3DBridgeCallback_Error onError);

		[DllImport("__Internal")]
		private static extern void ins_openDocumentShare(string imagePath, U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Cancel onCancel, U3DBridgeCallback_Error onError);

		private static class ShareCallback
		{
			/// <summary>
			/// 分享回调监听
			/// </summary>
			public static IBridgeListener _shareListener;

			/// <summary>
			/// 支付成功回调桥接函数
			/// </summary>
			/// <param name="result"></param>
			[AOT.MonoPInvokeCallback(typeof(U3DBridgeCallback_Success))]
			public static void OnSuccess(string result)
			{
				_shareListener?.OnSuccess(result);
			}

			/// <summary>
			/// 支付用户取消回调桥接函数
			/// </summary>
			[AOT.MonoPInvokeCallback(typeof(U3DBridgeCallback_Cancel))]
			public static void OnCancel()
			{
				_shareListener?.OnCancel();
			}

			/// <summary>
			/// 支付错误回调桥接函数
			/// </summary>
			/// <param name="errCode"></param>
			/// <param name="errMsg"></param>
			[AOT.MonoPInvokeCallback(typeof(U3DBridgeCallback_Error))]
			public static void OnError(int errCode, string errMsg)
			{
				_shareListener?.OnError(errCode, errMsg);
			}
		}

		void IBridge.Init(IBridgeListener listener)
		{
			listener.OnSuccess("");
		}

		bool IBridge.IsInstalled()
		{
			return IsInstalled();
		}

		bool IBridge.OpenApp()
		{
			return ins_openApp();
		}

		bool IBridge.OpenAppAndCamara()
		{
			return ins_openAppAndCamara();
		}

		bool IBridge.OpenAppAndMedia(int mediaId)
		{
			return ins_openAppAndMedia(mediaId);
		}

		bool IBridge.OpenAppAndUser(string username)
		{
			return ins_openAppAndUser(username);
		}

		bool IBridge.OpenAppAndLocations(int locationId)
		{
			return ins_openAppAndLocations(locationId);
		}

		bool IBridge.OpenAppAndTags(string tagName)
		{
			return ins_openAppAndTags(tagName);
		}

		void IBridge.ShareImage(string imagePath, IBridgeListener listener)
		{
			ShareCallback._shareListener = listener;
			ins_openDocumentShare(imagePath, ShareCallback.OnSuccess, ShareCallback.OnCancel, ShareCallback.OnError);
		}

		void IBridge.ShareImage(byte[] imageData, IBridgeListener listener)
		{
			try
			{
				ShareCallback._shareListener = listener;
				int length = imageData.Length;
				IntPtr buffer = Marshal.AllocHGlobal(length);
				Marshal.Copy(imageData, 0, buffer, length);
				ins_openDocumentSharebyData(buffer, length, ShareCallback.OnSuccess, ShareCallback.OnCancel, ShareCallback.OnError);
			}
			catch (Exception e)
			{
				Debug.LogError("字节流转指针解析错误：" + e.Message);
				ShareCallback._shareListener?.OnError(-1, e.Message);
				ShareCallback._shareListener = null;
			}
		}

		void IBridge.ShareLink(string linkUrl, IBridgeListener listener)
		{
			ShareCallback._shareListener?.OnError(-1, "Unsupported");
		}

		void IBridge.ShareVideo(string videoUrl, IBridgeListener listener)
		{
			ShareCallback._shareListener?.OnError(-1, "Unsupported");
		}

		private bool IsInstalled()
		{
			return ins_isIntalled();
		}
	}
}

#endif