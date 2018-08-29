package com.oppo.mobaddemo.listener;

import android.util.Log;

import com.oppo.mobad.api.ad.InterstitialAd;
import com.oppo.mobad.api.listener.IInterstitialAdListener;

import java.lang.ref.WeakReference;

/**
 * Created by 80059209 on 2017-11-17.
 */

public class InterstitialAdListener implements IInterstitialAdListener {

    private WeakReference<InterstitialAd> mAdWeakReference;

    public InterstitialAdListener(InterstitialAd interstitialAd) {
        mAdWeakReference = new WeakReference<InterstitialAd>(interstitialAd);
    }

    private static final String TAG = "InterstitialAdListener";

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
        /**
         *  调用sowAd方法展示插屏广告
         *  注意：只有请求广告回调onAdReady以后，调用loadAd方法才会展示广告，如果是onAdFailed，则即使调用了showAd，也不会展示广告
         *
         */
        if (null != mAdWeakReference.get()) {
            mAdWeakReference.get().showAd();
        }
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
