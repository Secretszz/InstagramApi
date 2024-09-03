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
		private static extern void ins_openDocumentSharebyData(IntPtr imageData, int length, OnFinishShare onFinishShare);

		[DllImport("__Internal")]
		private static extern void ins_openDocumentShare(string imagePath, OnFinishShare onFinishShare);

		/// <summary>
		/// iOS桥接分享回调事件
		/// </summary>
		private delegate void OnFinishShare(int errCode, string errMsg);

		/// <summary>
		/// Unity分享回调事件
		/// </summary>
		private static IShareListener _shareListener;

		/// <summary>
		/// iOS桥接分享回调事件
		/// </summary>
		/// <param name="errCode"></param>
		/// <param name="errMsg"></param>
		[AOT.MonoPInvokeCallback(typeof(OnFinishShare))]
		private static void ShareCallback(int errCode, string errMsg)
		{
			if (errCode == 0)
			{
				_shareListener?.OnSuccess();
			}
			else
			{
				_shareListener?.OnError(errCode, errMsg);
			}
		}

		void IBridge.Init(IInitListener listener)
		{
			listener.OnSuccess();
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

		void IBridge.ShareImage(string imagePath, IShareListener listener)
		{
			_shareListener = listener;
			ins_openDocumentShare(imagePath, ShareCallback);
		}

		void IBridge.ShareImage(byte[] imageData, IShareListener listener)
		{
			try
			{
				_shareListener = listener;
				int length = imageData.Length;
				IntPtr buffer = Marshal.AllocHGlobal(length);
				Marshal.Copy(imageData, 0, buffer, length);
				ins_openDocumentSharebyData(buffer, length, ShareCallback);
			}
			catch (Exception e)
			{
				Debug.LogError("字节流转指针解析错误：" + e.Message);
				_shareListener?.OnError(-1, e.Message);
				_shareListener = null;
			}
		}

		void IBridge.ShareLink(string linkUrl, IShareListener listener)
		{
			_shareListener?.OnError(-1, "Unsupported");
		}

		void IBridge.ShareVideo(string videoUrl, IShareListener listener)
		{
			_shareListener?.OnError(-1, "Unsupported");
		}

		private bool IsInstalled()
		{
			return ins_isIntalled();
		}
	}
}

#endif