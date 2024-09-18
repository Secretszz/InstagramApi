package com.bridge.instagram;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.util.Log;

import androidx.core.content.FileProvider;

import com.bridge.common.listener.IBridgeListener;

import java.io.File;

public class InstagramApi {
    private final static String TAG = InstagramApi.class.getName();
    private final static String packageName = "com.instagram.android";

    private static class Holder{
        public final static InstagramApi INSTANCE = new InstagramApi();
    }

    public static InstagramApi getInstance(){
        return Holder.INSTANCE;
    }

    public boolean isInstalledInstagram(Activity activity){
        try {
            activity.getPackageManager().getApplicationInfo(packageName, 0);
            return true;
        } catch (Exception ex){
            return false;
        }
    }

    public void init(Activity activity, IBridgeListener listener){
        listener.onSuccess("");
    }

    /**
     * 分享图片
     * @param activity 主activity
     * @param imagePath 图片本地地址
     * @param listener 分享回调
     */
    public void shareImage(Activity activity, String imagePath, IBridgeListener listener){
        String type = "image/*";
        createInstagramIntent(activity, type, imagePath, listener);
    }

    /**
     * 分享视频
     * @param activity 主activity
     * @param videoUrl 视频本地地址
     * @param listener 分享回调
     */
    public void shareVideo(Activity activity, String videoUrl, IBridgeListener listener){
        String type = "video/*";
        createInstagramIntent(activity, type, videoUrl, listener);
    }

    /**
     * 分享链接
     * @param activity 主activity
     * @param linkUrl 链接地址
     * @param listener 分享回调
     */
    public void shareLink(Activity activity, String linkUrl, IBridgeListener listener){
        listener.onError(-1, "Unsupported");
    }

    private void createInstagramIntent(Activity activity, String type, String mediaPath, IBridgeListener listener){
        if (!isInstalledInstagram(activity)){
            listener.onError(-1, "Instagram not installed");
            return;
        }
        try {
            // Create the new Intent using the 'Send' action
            Intent share = new Intent(Intent.ACTION_SEND);

            // Set the MIME type
            share.setType(type);

            // Create the URI from the media
            Uri uri;
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.N){
                uri = FileProvider.getUriForFile(activity, activity.getPackageName() + ".provider", new File(mediaPath));
                share.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
            } else {
                uri = Uri.fromFile(new File(mediaPath));
            }

            //Add the URI to the Intent.
            share.putExtra(Intent.EXTRA_STREAM, uri);
            share.setPackage(packageName);
            // Broadcast the Intent.
            activity.startActivity(Intent.createChooser(share, "Share to"));
            listener.onSuccess("");
        } catch (Exception ex){
            Log.e(TAG, "createInstagramIntent: ", ex);
            listener.onError(-1, ex.getMessage());
        }
    }
}
