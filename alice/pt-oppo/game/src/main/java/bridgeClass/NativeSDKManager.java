package bridgeClass;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.res.Configuration;
import android.net.Uri;
import android.os.Bundle;
import android.telephony.TelephonyManager;
import android.util.Log;
import android.widget.Toast;


import com.baidu.mtjstatsdk.StatSDKService;
import com.baidu.mtjstatsdk.game.BDGameSDK;
import com.edroity.nativebrige.Gate;
import com.nearme.game.sdk.GameCenterSDK;
import com.nearme.game.sdk.callback.GameExitCallback;
import com.nearme.game.sdk.callback.SinglePayCallback;
import com.nearme.game.sdk.common.model.biz.PayInfo;
import com.nearme.platform.opensdk.pay.PayResponse;
import com.oppo.mobad.api.MobAdManager;


import org.json.JSONObject;


import java.util.Random;


import pzy.game.pt.Action;
import pzy.game.pt.GameActivity;
import pzy.game.pt.JSONObjectUtil;
import pzy.game.pt.OppoADManager;
import pzy.game.pt.OppoInterAd;

/**
 * Created by zhiyuan.peng on 2017/5/17.
 */

public class NativeSDKManager {

    static Activity gameActivity;
    static String TAG = "A-SDKManager";

    static String OPPO_APP_SECRET = "e311f1aff9e844cfa8868aa42356994f";
    static String BAIDU_APP_KEY = "2b90c3977a";

    public static void onGameActivityCreate(Activity theGameActivity, Bundle savedInstanceState)
    {
        gameActivity = theGameActivity;
        GameCenterSDK.init(OPPO_APP_SECRET, gameActivity);
        BDGameSDK.initGame(gameActivity, BAIDU_APP_KEY);
        String deviceId = getIMEI(gameActivity);
        BDGameSDK.setAccount(gameActivity,  deviceId, BAIDU_APP_KEY);
        StatSDKService.setAppChannel(gameActivity, "oppo", true, BAIDU_APP_KEY);
        StatSDKService.setDebugOn(true, BAIDU_APP_KEY);
        String version = NativeGameManifestManager.tryGet("version", "1.0");
        StatSDKService.setAppVersionName(version, BAIDU_APP_KEY);
        BDGameSDK.setOn(gameActivity, BDGameSDK.EXCEPTION_LOG ,BAIDU_APP_KEY);

        OppoADManager.onGameActivityCreate((GameActivity) theGameActivity, savedInstanceState);
        OppoADManager.loadInterAd();
    }


    public static String getIMEI(Context context) {
        String imei;
        try {
            TelephonyManager telephonyManager = (TelephonyManager) context.getSystemService(Context.TELEPHONY_SERVICE);
            imei = telephonyManager.getDeviceId();
        } catch (Exception e) {
            imei = "";
        }
        return imei;
    }

    public static void onGameActivityDestroy()
    {

    }

    public static void onGameActivityStart()
    {
        MobAdManager.getInstance().exit(gameActivity);
    }

    public static void onGameActivityStop()
    {

    }

    public static void onGameActivityPause()
    {
        StatSDKService.onPause(gameActivity, BAIDU_APP_KEY);
    }

    public static void onGameActivityResume()
    {
        StatSDKService.onResume(gameActivity, BAIDU_APP_KEY);
    }

    public static void onGameActivityRestart()
    {

    }

    public static void onGameActivityNewIntent(Intent intent)
    {

    }

    public static void onGameActivityResult(int requestCode, int resultCode, Intent data)
    {

    }

    public static void onGameActivityBackPressed()
    {

    }

    public static void onGameActivityConfigurationChanged(Configuration newConfig)
    {

    }

    // call
    public static void Pay(final String callId, final String arg)
    {
        JSONObject jo = JSONObjectUtil.fromString(arg);
        final String orderId = System.currentTimeMillis() + new Random().nextInt(1000) + "";
        String ext = "";
        final int itemId = JSONObjectUtil.getInt(jo, "itemId");
        String productName = JSONObjectUtil.getString(jo, "productName");
        String productDesc = JSONObjectUtil.getString(jo, "productDesc");

        int price_fen = JSONObjectUtil.getInt(jo, "price_fen");
        double price_yuan = JSONObjectUtil.getDouble(jo, "price_yuan");
        int gold = JSONObjectUtil.getInt(jo, "gold");

        // 在玩家发起充值请求时调用(例如 某玩家选择了某充值包，进入充值流程)
        BDGameSDK.onRechargeRequest(orderId, itemId + "" , price_yuan, gold, 0, BAIDU_APP_KEY);

        final SinglePayCallback callback = new SinglePayCallback()
        {

            // add OPPO 支付成功处理逻辑~
            public void onSuccess(String resultMsg)
            {
                Toast.makeText(gameActivity, "支付成功",Toast.LENGTH_SHORT).show();
                // 玩家充值成功后告 知是哪个订单
                BDGameSDK.onRechargeSuccess(orderId, BAIDU_APP_KEY);
                Gate.callReturn(callId, "SUCCESS");
            }

            // add OPPO 支付失败处理逻辑~
            public void onFailure(String resultMsg, int resultCode)
            {
                if (PayResponse.CODE_CANCEL != resultCode)
                {
                    Toast.makeText(gameActivity, "支付失败",Toast.LENGTH_SHORT).show();
                    Gate.callReturn(callId, "FAIL");
                }
                else
                {
                    // 取消支付处理
                    Toast.makeText(gameActivity, "支付取消",Toast.LENGTH_SHORT).show();
                    Gate.callReturn(callId, "CANCEL");
                }
            }

            /*
            bySelectSMSPay 为true表示回调来自于支付渠道列表选择短信支付，false表示没有
            网络等非主动选择短信支付时候的回调
            */
            public void onCallCarrierPay(PayInfo payInfo, boolean bySelectSMSPay)
            {
                // add 运营商支付逻辑~
                // Toast.makeText(DemoActivity.this, "运营商支付",Toast.LENGTH_SHORT).show();
                // NOT USE THIS METHOD
            }
        };




        final PayInfo payInfo = new PayInfo(orderId, ext, price_fen);
        payInfo.setProductDesc(productName);
        payInfo.setProductName(productDesc);

        gameActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {

                GameCenterSDK.getInstance().doSinglePay(gameActivity, payInfo, callback);
            }
        });

    }

    // notify
    public static void Exit(final String arg)
    {
        gameActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                GameCenterSDK.getInstance().onExit(gameActivity, new GameExitCallback()
                {
                    @Override
                    public void exitGame()
                    {
                        gameActivity.finish();
                    }
                });
            }
        });

    }


    public static void GotoMarket(String callId, String arg)
    {
        Uri uri = Uri.parse("market://details?id=" + gameActivity.getPackageName());
        Intent intent = new Intent(Intent.ACTION_VIEW,uri);
        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        gameActivity.startActivity(intent);
        Gate.callReturn(callId, "");
    }

    public static void OnEnterCore(String arg)
    {
        OppoADManager.ShowBanner();
    }

    public static void OnExitCore(String arg)
    {
        OppoADManager.HideBanner();
    }

    public static String IsInterAdLoaded(String arg)
    {
        boolean b = OppoADManager.isInterAdLoaded();
        if(b)
        {
            return "true";
        }
        else
        {
            return "false";
        }
    }

    public static void ShowInterAd(final String callId, String arg)
    {
        OppoADManager.showInterAd(new Action() {
            @Override
            public void onStart() {
                OppoInterAd ad = OppoADManager.getCurrentInterAd();
                boolean clicked = ad.clicked;
                if(clicked)
                {
                    Gate.callReturn(callId, "true");
                }
                else
                {
                    Gate.callReturn(callId, "false");
                }
                OppoADManager.cleanInterAd();
                OppoADManager.loadInterAd();
            }
        });
    }

}
