package pzy.game.pt;

import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;

import bridgeClass.NativeGameManifestManager;
import bridgeClass.NativeSDKManager;


/**
 * Created by zhiyuan.peng on 2017/5/6.
 */

public class GameActivity extends UnityPlayerActivity {

    public static GameActivity instance;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        instance = this;
        NativeGameManifestManager.onGameActivitCreate(this);
        PermissionManager.onCreate(this);
        NativeSDKManager.onGameActivityCreate(this, savedInstanceState);
        HosUtility.gameActivity = this;
        HosUtility.hideBar(this, true);
        HosUtility.StartBattertyReceiver(this);

    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        NativeSDKManager.onGameActivityDestroy();

    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        NativeSDKManager.onGameActivityResult(requestCode, resultCode, data);
    }

    @Override
    public void onWindowFocusChanged(boolean hasFocus) {
        super.onWindowFocusChanged(hasFocus);
        HosUtility.hideBar(this, hasFocus);
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        NativeSDKManager.onGameActivityBackPressed();
    }

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        NativeSDKManager.onGameActivityConfigurationChanged(newConfig);
    }

    @Override
    protected void onStop() {
        super.onStop();
        NativeSDKManager.onGameActivityStop();
    }

    @Override
    protected void onStart() {
        super.onStart();
        NativeSDKManager.onGameActivityStart();
    }

    @Override
    protected void onPause() {
        super.onPause();
        NativeSDKManager.onGameActivityPause();
    }

    @Override
    protected void onResume() {
        super.onResume();
        NativeSDKManager.onGameActivityResume();
    }

    //@Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        //super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        PermissionManager.onRequestPermissionsResult(requestCode,permissions, grantResults);
    }

    @Override
    protected void onRestart() {
        super.onRestart();
        NativeSDKManager.onGameActivityRestart();
    }

    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        NativeSDKManager.onGameActivityNewIntent(intent);
    }
}
