// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		ManifestProcessor.cs
//
// Author Name:		Bridge
//
// Create Time:		2023/12/04 19:13:02
// *******************************************

#if UNITY_ANDROID
namespace Bridge.FacebookApi
{
    using System.Xml.Linq;
    using System.IO;
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using Editor;

    internal static class ManifestProcessor
    {
        [PostProcessBuild(10001)]
        public static void OnPostprocessBuild(BuildTarget target, string projectPath)
        {
            CopyNativeCode(projectPath);
            Common.ManifestProcessor.QueriesElements.Add(new XElement("package", new XAttribute(Common.ManifestProcessor.ns + "name", "com.instagram.android")));
        }

        private static void CopyNativeCode(string projectPath)
        {
            var sourcePath = ThirdSDKPackageManager.GetUnityPackagePath(ThirdSDKPackageManager.InstagramApiPackageName);
            if (string.IsNullOrEmpty(sourcePath))
            {
                // 这个不是通过ump下载的包，查找工程内部文件夹
                sourcePath = "Assets/ThirdSDK/InstagramApi";
            }

            sourcePath += "/Plugins/Android/instagram";
            Debug.Log("remotePackagePath===" + sourcePath);
            string targetPath = Path.Combine(projectPath, Common.ManifestProcessor.NATIVE_CODE_DIR, "instagram");
            Debug.Log("targetPath===" + targetPath);
            FileTool.DirectoryCopy(sourcePath, targetPath);
        }
    }
}
#endif
