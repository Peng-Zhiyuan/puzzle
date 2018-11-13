package com.oppo.mobaddemo.activity;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

import com.oppo.mobad.api.ad.InterstitialAd;
import com.oppo.mobad.api.listener.IInterstitialAdListener;
import com.oppo.mobaddemo.R;
import com.oppo.mobaddemo.utils.Constants;

public class InterstitialActivity extends Activity implements IInterstitialAdListener {
    private static final String TAG = "InterstitialActivity";
    private InterstitialAd mInterstitialAd;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_interstitial);
        init();
    }

    @Override
    protected void onDestroy() {
        /**
         * 在退出页面时调用destroyAd来释放广告资源
         */
        if (null != mInterstitialAd) {
            mInterstitialAd.destroyAd();
        }
        super.onDestroy();
    }

    private void init() {
        /**
         * 构造 InterstitialAd.
         */
        mInterstitialAd = new InterstitialAd(this, Constants.INTERSTITIAL_POS_ID);
        /**
         * 设置插屏广告行为监听器.
         */
        mInterstitialAd.setAdListener(this);
        /**
         * 调用 loadAd() 方法请求广告.
         */
        mInterstitialAd.loadAd();
    }

    public void onBnClick(View view) {
        switch (view.getId()) {
            case R.id.load_ad_bn:
                mInterstitialAd.loadAd();
                break;
            case R.id.show_ad_bn:
                mInterstitialAd.showAd();
                break;
            case R.id.destroy_ad_bn:
                /**
                 *销毁广告；注意：调用destroyAd方法以后，在调用该InterstitialAd对象的任何方法将没有效果
                 */
                mInterstitialAd.destroyAd();
                break;
        }
    }

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
        mInterstitialAd.showAd();
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
