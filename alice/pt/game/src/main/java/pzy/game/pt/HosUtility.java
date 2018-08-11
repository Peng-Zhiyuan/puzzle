package pzy.game.pt;

import android.app.Activity;
import android.content.ClipData;
import android.content.ClipboardManager;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.media.AudioFormat;
import android.media.AudioRecord;
import android.media.MediaRecorder;
import android.os.Build;
import android.util.Log;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;

import java.util.zip.ZipEntry;
import java.util.zip.ZipFile;

/**
 * Created by edrotiy_mac1 on 2017/5/30.
 */

public class HosUtility {

    public static String TAG = "HosUtility";

    public static String dySignRet;

    public static Activity gameActivity;

    // c# call this
    //public static void dySign(String key) {
    //    dySignRet = Cpp.dynamicSign(key, gameActivity);
    //}

    public static void StartBattertyReceiver(Activity gameActivity) {
        //注册广播接受者java代码
        IntentFilter intentFilter = new IntentFilter(Intent.ACTION_BATTERY_CHANGED);
        //创建广播接受者对象
        BatteryReceiver batteryReceiver = new BatteryReceiver();
        //注册receiver
        gameActivity.registerReceiver(batteryReceiver, intentFilter);
    }

    private static void verifyDex(Activity activity) {
        //获取String.xml中的value
        // Long dexCrc = Long.parseLong(this.getString(R.string.crc_value));
        String crc;
        String apkPath = activity.getPackageCodePath();
        try {
            ZipFile zipFile = new ZipFile(apkPath);
            ZipEntry dexEntry = zipFile.getEntry("classes.dex");
            //计算classes.dex的 crc
            long dexEntryCrc = dexEntry.getCrc();
            Log.d("DEX", dexEntryCrc + "");
            //对比
            crc = Long.toString(dexEntryCrc);
        } catch (Exception e) {
            e.printStackTrace();
        }

    }

    public static void hideBar(Activity activity, boolean hasFocus) {
        if (hasFocus && Build.VERSION.SDK_INT >= 19) {
            Window window = activity.getWindow();
            WindowManager.LayoutParams params = window.getAttributes();
            params.systemUiVisibility = View.SYSTEM_UI_FLAG_LAYOUT_STABLE
                    | View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
                    | View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
                    | View.SYSTEM_UI_FLAG_HIDE_NAVIGATION
                    | View.SYSTEM_UI_FLAG_FULLSCREEN
                    | View.SYSTEM_UI_FLAG_IMMERSIVE_STICKY;
            window.setAttributes(params);
        }

    }

    public static boolean isVoicePermissionReturn;
    public static void isVoicePermission() {

        try {
            AudioRecord record = new AudioRecord(MediaRecorder.AudioSource.MIC, 22050, AudioFormat.CHANNEL_IN_MONO,
                    AudioFormat.ENCODING_PCM_16BIT, AudioRecord.getMinBufferSize(22050, AudioFormat.CHANNEL_IN_MONO,
                    AudioFormat.ENCODING_PCM_16BIT));
            record.startRecording();
            int recordingState = record.getRecordingState();
            if (recordingState == AudioRecord.RECORDSTATE_STOPPED) {
                isVoicePermissionReturn = false;
                return;
            }
            record.release();
            isVoicePermissionReturn = true;
            return;
        } catch (Exception e) {
            isVoicePermissionReturn = false;
            return;
        }

    }

    public static void copyTextToClipboard(final String text) {

        GameActivity.instance.runOnUiThread(new Runnable() {

            @Override
            public void run() {

                ClipboardManager clipboardManager = (ClipboardManager) GameActivity.instance.getSystemService(Context.CLIPBOARD_SERVICE);
                ClipData clipData = ClipData.newPlainText("hos", text);
                clipboardManager.setPrimaryClip(clipData);
            }
        });
    }
}
