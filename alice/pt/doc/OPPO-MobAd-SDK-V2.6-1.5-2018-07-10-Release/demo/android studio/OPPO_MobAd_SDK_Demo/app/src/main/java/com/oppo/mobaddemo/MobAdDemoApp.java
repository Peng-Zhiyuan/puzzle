package com.oppo.mobaddemo;

import android.app.Application;

import com.oppo.mobad.api.InitParams;
import com.oppo.mobad.api.MobAdManager;
import com.oppo.mobaddemo.utils.Constants;
import com.squareup.leakcanary.LeakCanary;

/**
 * Created by 80059209 on 2017-03-10.
 */

public class MobAdDemoApp extends Application {
    @Override
    public void onCreate() {
        super.onCreate();
        /**
         * 应用不需要加入这行代码
         */
        initLeakcanary();
        /**
         * 应用必须加入这行代码，初始化广告SDK
         */
        initSdk();
    }

    /**
     * LeakCanary是一款内存泄露检测工具；接入应用不需要加入这行代码，
     * 这个只是Demo检测内训泄露问题的一个工具
     */
    private void initLeakcanary() {
        if (LeakCanary.isInAnalyzerProcess(this)) {
            // This process is dedicated to LeakCanary for heap analysis.
            // You should not init your app in this process.
            return;
        }
        LeakCanary.install(this);
        // Normal app init code...
    }

    private void initSdk() {
        InitParams initParams = new InitParams.Builder()
                .setDebug(true)//true打开SDK日志，当应用发布Release版本时，必须注释掉这行代码的调用，或者设为false
                .build();
        /**
         * 调用这行代码初始化广告SDK
         */
        MobAdManager.getInstance().init(this, Constants.APP_ID, initParams);
    }
}
