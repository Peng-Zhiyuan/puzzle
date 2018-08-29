package com.oppo.mobaddemo.activity;

import android.Manifest;
import android.app.Activity;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.support.v4.app.ActivityCompat;
import android.view.View;
import android.widget.Toast;

import com.oppo.mobad.api.MobAdManager;
import com.oppo.mobaddemo.R;


public class MainActivity extends Activity {

    private static final String TAG = "MainActivity";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    /**
     * 判断应用是否已经获得SDK运行必须的READ_PHONE_STATE、WRITE_EXTERNAL_STORAGE两个权限。
     *
     * @return
     */
    private boolean hasNecessaryPMSGranted() {
        if (PackageManager.PERMISSION_GRANTED == ActivityCompat.checkSelfPermission(this, Manifest.permission.READ_PHONE_STATE)) {
            if (PackageManager.PERMISSION_GRANTED == ActivityCompat.checkSelfPermission(this, Manifest.permission.WRITE_EXTERNAL_STORAGE)) {
                return true;
            }
        }
        return false;
    }

    public void onBnClick(View view) {
        switch (view.getId()) {
            case R.id.banner_bn:
                startBannerActivity();
                break;
            case R.id.interstitial_bn:
                startInterstitialActivity();
                break;
            case R.id.mix_bn:
                startMixActivity();
                break;
            case R.id.native_bn:
                startNativeActivity();
                break;
            //add 2018-04-09、启动原生模板广告
            case R.id.native_templet_bn:
                startNativeTempletActivity();
                break;
            //add 2018-04-15、启动激励视频广告
            case R.id.reward_video_bn:
                startRewardVideoActivity();
                break;
        }
    }


    private void startBannerActivity() {
        if (hasNecessaryPMSGranted()) {
            Intent intent = new Intent(this, BannerActivity.class);
            startActivity(intent);
        } else {
            Toast.makeText(this, "没有 READ_PHONE_STATE 或 WRITE_EXTERNAL_STORAGE 权限，SDK无法正常运行!!!", Toast.LENGTH_SHORT).show();
        }
    }

    private void startInterstitialActivity() {
        if (hasNecessaryPMSGranted()) {
            Intent intent = new Intent(this, InterstitialActivity.class);
            startActivity(intent);
        } else {
            Toast.makeText(this, "没有 READ_PHONE_STATE 或 WRITE_EXTERNAL_STORAGE 权限，SDK无法正常运行!!!", Toast.LENGTH_SHORT).show();
        }
    }

    private void startMixActivity() {
        if (hasNecessaryPMSGranted()) {
            Intent intent = new Intent(this, MixActivity.class);
            startActivity(intent);
        } else {
            Toast.makeText(this, "没有 READ_PHONE_STATE 或 WRITE_EXTERNAL_STORAGE 权限，SDK无法正常运行!!!", Toast.LENGTH_SHORT).show();
        }
    }

    private void startNativeActivity() {
        if (hasNecessaryPMSGranted()) {
            Intent intent = new Intent(this, NativeActivity.class);
            startActivity(intent);
        } else {
            Toast.makeText(this, "没有 READ_PHONE_STATE 或 WRITE_EXTERNAL_STORAGE 权限，SDK无法正常运行!!!", Toast.LENGTH_SHORT).show();
        }
    }

    //add 2018-04-09
    private void startNativeTempletActivity() {
        if (hasNecessaryPMSGranted()) {
            Intent intent = new Intent(this, NativeTempletActivity.class);
            startActivity(intent);
        } else {
            Toast.makeText(this, "没有 READ_PHONE_STATE 或 WRITE_EXTERNAL_STORAGE 权限，SDK无法正常运行!!!", Toast.LENGTH_SHORT).show();
        }
    }

    //add 2018-04-15
    private void startRewardVideoActivity() {
        if (hasNecessaryPMSGranted()) {
            Intent intent = new Intent(this, RewardVideoActivity.class);
            startActivity(intent);
        } else {
            Toast.makeText(this, "没有 READ_PHONE_STATE 或 WRITE_EXTERNAL_STORAGE 权限，SDK无法正常运行!!!", Toast.LENGTH_SHORT).show();
        }
    }

    @Override
    public void onBackPressed() {
        /**
         * 在你的应用程序进程退出时，调用该方法释放SDK 资源
         * */
        MobAdManager.getInstance().exit(this);
        //
        super.onBackPressed();
        try {
            /**
             * 退出应用进程
             */
            android.os.Process.killProcess(android.os.Process.myPid());
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

}
