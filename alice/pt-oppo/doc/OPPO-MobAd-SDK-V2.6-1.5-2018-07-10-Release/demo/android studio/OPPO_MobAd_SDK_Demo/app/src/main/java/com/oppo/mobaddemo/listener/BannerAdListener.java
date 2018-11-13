package com.oppo.mobaddemo.listener;

import android.util.Log;

import com.oppo.mobad.api.listener.IBannerAdListener;

/**
 * Created by 80059209 on 2017-11-17.
 */

public class BannerAdListener implements IBannerAdListener {

    private static final String TAG="BannerAdListener";

    @Override
    public void onAdShow() {
        /**
         *广告展示
         */
        Log.d(TAG, "onAdShow");
    }

    @Override
    public void onAdFailed(String errMsg) {
        /**
         *请求广告失败
         */
        Log.d(TAG, "onAdFailed:errMsg=" + (null != errMsg ? errMsg : ""));
    }

    @Override
    public void onAdReady() {

        /**
         *请求广告成功
         */
        Log.d(TAG, "onAdReady");
    }

    @Override
    public void onAdClick() {
        /**
         *广告被点击
         */
        Log.d(TAG, "onAdClick");
    }

    @Override
    public void onAdClose() {
        /**
         *广告被关闭
         */
        Log.d(TAG, "onAdClose");
    }
}
