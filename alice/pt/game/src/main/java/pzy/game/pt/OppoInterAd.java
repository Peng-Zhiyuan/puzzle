package pzy.game.pt;

import android.util.Log;

import com.oppo.mobad.api.ad.InterstitialAd;
import com.oppo.mobad.api.listener.IInterstitialAdListener;

import pzy.game.pt.oppo_ad.Constants;

public class OppoInterAd
{
    GameActivity gameActivity;
    public boolean isLoaded;
    public boolean loadFailed;
    public boolean clicked;
    Action onClose;

    public OppoInterAd(GameActivity gameActivity)
    {
        this.gameActivity = gameActivity;
    }

    static InterstitialAd rawAd = null;
    public void load(final Action onLoadFail)
    {
        gameActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                rawAd = new InterstitialAd(gameActivity, Constants.INTERSTITIAL_POS_ID);
                /**
                 * 设置插屏广告行为监听器.
                 */
                rawAd.setAdListener(new IInterstitialAdListener() {
                    @Override
                    public void onAdReady() {
                        isLoaded = true;
                    }

                    @Override
                    public void onAdClose() {
                        if(onClose != null)
                        {
                            onClose.onStart();
                        }
                    }

                    @Override
                    public void onAdShow() {

                    }

                    @Override
                    public void onAdFailed(String s) {
                        loadFailed = true;
                        onLoadFail.onStart();
                    }

                    @Override
                    public void onAdClick() {
                        clicked = true;
                    }
                });
                /**
                 * 调用 loadAd() 方法请求广告.
                 */
                rawAd.loadAd();
            }
        });

    }

    public void show(Action onClose)
    {
        this.onClose = onClose;
        gameActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                rawAd.showAd();
            }
        });
    }
}
