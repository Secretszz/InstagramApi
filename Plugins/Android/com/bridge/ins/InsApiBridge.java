package com.ins.bridge;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.util.Log;

import androidx.core.content.FileProvider;

import java.io.File;

public class InsApiBridge {
    private final static String TAG = InstagramApiManager.class.getName();
    private final static String packageName = "com.instagram.android";

    public static boolean isInstalledInstagram(Activity activity){
        try {
            activity.getPackageManager().getApplicationInfo(packageName, 0);
            return true;
        } catch (Exception var2) {
            return false;
        }
    }
    
    public static void init(Activity activity, IInitListener listener){
        listener.onSuccess();
    }

    /**
     * 分享图片
     * @param imagePath 图片本地地址
     */
    public static void shareImage(Activity activity, final String imagePath, final IShareListener listener){
        String type = "image/*";
        createInstagramIntent(activity, type, imagePath, listener);
    }

    /**
     * 分享视频
     * @param videoUrl 视频本地地址
     */
    public static void shareVideo(Activity activity, String videoUrl, final IShareListener listener) {
        String type = "video/*";
        createInstagramIntent(activity, type, videoUrl, listener);
    }

    /**
     * 分享视频
     * @param linkUrl 链接地址
     */
    public static void shareLink(Activity activity, String linkUrl, final IShareListener listener) {
		listener.onError(-1, "unsupported");
    }

    private static void createInstagramIntent(Activity activity, String type, String mediaPath, final IShareListener listener){
        if (!isInstalledInstagram(activity)){
            listener.onError(-1, "Instagram not installed");
            return;
        }
        try {
            // Create the new Intent using the 'Send' action.
            Intent share = new Intent(Intent.ACTION_SEND);

            // Set the MIME type
            share.setType(type);

            // Create the URI from the media
            Uri uri;
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.N) {//android 7.0以上
                //第二个参数："app的包名.fileProvider"
                uri = FileProvider.getUriForFile(activity, activity.getPackageName() + ".provider", new File(mediaPath));
                share.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
            } else {
                uri = Uri.fromFile(new File(mediaPath));
            }

            // Add the URI to the Intent.
            share.putExtra(Intent.EXTRA_STREAM, uri);
            share.setPackage(packageName);
            // Broadcast the Intent.
            activity.startActivity(Intent.createChooser(share, "Share to"));
            listener.onSuccess();
        } catch (Exception ex){
            Log.e(TAG, "createInstagramIntent: ", ex);
            listener.onError(-1, ex.getMessage());
        }
    }
}
