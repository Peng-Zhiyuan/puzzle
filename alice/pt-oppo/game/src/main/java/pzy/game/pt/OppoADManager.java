package pzy.game.pt;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewGroup.LayoutParams;
import android.widget.RelativeLayout;

import com.oppo.mobad.api.ad.BannerAd;
import com.oppo.mobad.api.ad.InterstitialAd;
import com.oppo.mobad.api.listener.IBannerAdListener;
import com.oppo.mobad.api.listener.IInterstitialAdListener;

import pzy.game.pt.oppo_ad.Constants;

public class OppoADManager
{
    static GameActivity gameActivity;
    static String TAG = "OppoADManager";
    public static void onGameActivityCreate(GameActivity theGameActivity, Bundle savedInstanceState)
    {
        gameActivity = theGameActivity;
        CreateLayoutFromOppoAd();
    }


    static RelativeLayout adContainuer = null;
    private static void CreateLayoutFromOppoAd()
    {
        gameActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                RelativeLayout layout = new RelativeLayout(gameActivity);

                gameActivity.addContentView(layout, new LayoutParams(LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT));

                //gameActivity.mUnityPlayer.addView(layout);
                adContainuer = layout;
            }
        });

    }

    public static void HideBanner()
    {
        Log.d(TAG, "HideBanner");
        gameActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if(mBannerAd != null)
                {
                    mBannerAd.destroyAd();
                    mBannerAd = null;
                }
            }
        });

    }

    static BannerAd mBannerAd = null;
    public static void ShowBanner()
    {
        Log.d(TAG, "ShowBanner");
        //bannerad ad = new bannerad(gameActivity);
        gameActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if(mBannerAd == null)
                {
                    //RelativeLayout mAdContainer = gameActivity.mUnityPlayer;
                    /**
                     * 构造 bannerAd
                     */
                    mBannerAd = new BannerAd(gameActivity, Constants.BANNER_POS_ID);
                    /**
                     * 设置Banner广告行为监听器
                     */
                    //mBannerAd.setAdListener(this);
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
                        adContainuer.addView(adView, layoutParams);
                        Log.d(TAG, "banner view added");
                    }
                    /**
                     * 调用loadAd()方法请求广告.
                     */
                    mBannerAd.loadAd();
                    Log.d(TAG, "complete");

                }
            }
        });

    }

    public static OppoInterAd interAd;
    public static void loadInterAd()
    {
        if(interAd == null)
        {
            interAd = new OppoInterAd(gameActivity);
            interAd.load(new Action() {
                @Override
                public void onStart() {
                    interAd = null;
                    loadInterAd();
                }
            });
        }
    }

    public static void showInterAd(Action onClose)
    {
        if(interAd != null)
        {
            if(interAd.isLoaded)
            {
                interAd.show(onClose);
            }
        }
    }

    public static boolean isInterAdLoaded()
    {
        if(interAd != null)
        {
            return interAd.isLoaded;
        }
        return false;
    }

    public static OppoInterAd getCurrentInterAd()
    {
        return interAd;
    }

    public static void cleanInterAd()
    {
        interAd = null;
    }



}
