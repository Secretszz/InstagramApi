package com.ins.bridge;

/**
 * 分享监听
 */
public interface InsShareListener {
    /**
     * 分享成功
     */
    void onSuccess();

    /**
     * 分享失败
     * @param errCode 错误码
     * @param errMsg 错误信息
     */
    void onError(int errCode, String errMsg);
}
