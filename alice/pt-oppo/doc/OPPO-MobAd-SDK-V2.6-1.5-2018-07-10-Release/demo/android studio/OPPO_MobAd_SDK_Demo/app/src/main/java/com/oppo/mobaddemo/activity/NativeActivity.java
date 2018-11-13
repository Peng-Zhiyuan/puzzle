package com.oppo.mobaddemo.activity;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import com.oppo.mobaddemo.R;


public class NativeActivity extends Activity {


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //
        setContentView(R.layout.activity_native);
    }

    public void onBnClick(View view) {
        switch (view.getId()) {
            case R.id.native_512X512_text_icon_bn:
                startNative512X512Activity();
                break;
            case R.id.native_640X320_text_img_bn:
                startNative640X320Activity();
                break;
            case R.id.native_320X210_text_img_bn:
                startNative320X210Activity();
                break;
            case R.id.native_group_320X210_text_img_bn:
                startNativeGroup320X210Activity();
                break;
        }
    }

    /**
     * 启动素材规格为512X512大小的图文混合的原生广告
     */
    private void startNative512X512Activity() {
        Intent intent = new Intent(this, Native512X512Activity.class);
        startActivity(intent);
    }

    /**
     * 启动素材规格为640X320大小的图文混合的原生广告
     */
    private void startNative640X320Activity() {
        Intent intent = new Intent(this, Native640X320Activity.class);
        startActivity(intent);
    }

    /**
     * 启动素材规格为320X210大小的图文混合的原生广告
     */
    private void startNative320X210Activity() {
        Intent intent = new Intent(this, Native320X210Activity.class);
        startActivity(intent);
    }

    /**
     * 启动素材规格为三张320X210大小的图文混合的原生广告
     */
    private void startNativeGroup320X210Activity() {
        Intent intent = new Intent(this, NativeGroup320X210Activity.class);
        startActivity(intent);
    }

}
