package com.oppo.mobaddemo.activity;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.RelativeLayout;

import com.oppo.mobad.api.ad.BannerAd;
import com.oppo.mobad.api.listener.IBannerAdListener;
import com.oppo.mobaddemo.R;
import com.oppo.mobaddemo.utils.Constants;


public class BannerActivity extends Activity implements IBannerAdListener {
    private static final String TAG = "BannerActivity";
    private RelativeLayout mAdContainer;
    private BannerAd mBannerAd;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_banner);
        init();
    }


    @Override
    protected void onDestroy() {
        if (null != mBannerAd) {
            /**
             * 在退出页面时调用destroyAd来释放广告资源
             */
            mBannerAd.destroyAd();
        }
        super.onDestroy();
    }

    private void init() {
        mAdContainer = (RelativeLayout) findViewById(R.id.ad_container);
        /**
         * 构造 bannerAd
         */
        mBannerAd = new BannerAd(this, Constants.BANNER_POS_ID);
        /**
         * 设置Banner广告行为监听器
         */
        mBannerAd.setAdListener(this);
        /**
         * 获取Banner广告View，将View添加到你的页面上去
         *
         */
        View adView = mBannerAd.getAdView();
        /**
         * mBannerAd.getAdView()返回可能为空，判断后在添加
         */
        if (null != adView) {
            /**
             * 这里addView是可以自己指定Banner广告的放置位置【一般是页面顶部或者底部】
             */
            RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);
            layoutParams.addRule(RelativeLayout.ALIGN_PARENT_BOTTOM);
            mAdContainer.addView(adView, layoutParams);
        }
        /**
         * 调用loadAd()方法请求广告.
         */
        mBannerAd.loadAd();
    }

    public void onBnClick(View view) {
        switch (view.getId()) {
            case R.id.load_ad_bn:
                mBannerAd.loadAd();
                break;
            case R.id.destroy_ad_bn:
                /**
                 *销毁广告；注意：调用destroyAd方法以后，在调用该BannerAd对象的任何方法将没有效果
                 */
                mBannerAd.destroyAd();
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
