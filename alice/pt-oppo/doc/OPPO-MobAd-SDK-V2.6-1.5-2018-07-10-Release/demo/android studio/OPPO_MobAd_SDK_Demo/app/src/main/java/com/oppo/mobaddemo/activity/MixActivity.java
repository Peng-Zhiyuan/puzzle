package com.oppo.mobaddemo.activity;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.RelativeLayout;

import com.oppo.mobad.api.ad.BannerAd;
import com.oppo.mobad.api.ad.InterstitialAd;
import com.oppo.mobad.api.listener.IBannerAdListener;
import com.oppo.mobad.api.listener.IInterstitialAdListener;
import com.oppo.mobaddemo.R;
import com.oppo.mobaddemo.utils.Constants;

public class MixActivity extends Activity implements IInterstitialAdListener {
    private static final String TAG = "MixActivity";
    //
    private RelativeLayout mAdContainer;
    private BannerAd mBannerAd;
    //
    private InterstitialAd mInterstitialAd;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_mix);
        init();
    }


    @Override
    protected void onDestroy() {
        if (null != mBannerAd) {
            mBannerAd.destroyAd();
        }
        if (null != mInterstitialAd) {
            mInterstitialAd.destroyAd();
        }
        super.onDestroy();
    }

    private void init() {
        initBannerAd();
        initInterstitialAd();
    }

    private void initBannerAd() {
        mAdContainer = (RelativeLayout) findViewById(R.id.ad_container);
        //update 2018-03-29
        mBannerAd = new BannerAd(this, Constants.MIX_BANNER_POS_ID);
        //
        mBannerAd.setAdListener(new IBannerAdListener() {
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
        });

        View adView = mBannerAd.getAdView();

        if (null != adView) {
            mAdContainer.addView(adView);
        }

        mBannerAd.loadAd();
    }

    private void initInterstitialAd() {
        //update 2018-03-29
        mInterstitialAd = new InterstitialAd(this, Constants.MIX_INTERSTITIAL_POS_ID);
        //
        mInterstitialAd.setAdListener(this);

        mInterstitialAd.loadAd();
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
