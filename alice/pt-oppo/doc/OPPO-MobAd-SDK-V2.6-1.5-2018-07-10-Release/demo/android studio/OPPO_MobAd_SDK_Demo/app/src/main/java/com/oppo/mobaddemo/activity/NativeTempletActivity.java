package com.oppo.mobaddemo.activity;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import com.oppo.mobaddemo.R;
import com.oppo.mobaddemo.utils.Constants;

/**
 * Created by 80059209 on 2018-04-09.
 */

public class NativeTempletActivity extends Activity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //
        setContentView(R.layout.activity_native_templet);
    }

    public void onBnClick(View view) {
        switch (view.getId()) {
            case R.id.native_temple_640X320_normal_bn:
            case R.id.native_temple_320X210_normal_bn:
            case R.id.native_temple_group_320X210_normal_bn:
                startNativeNormalActivity(getPosId(view.getId()));
                break;
            case R.id.native_temple_640X320_recycler_bn:
            case R.id.native_temple_320X210_recycler_bn:
            case R.id.native_temple_group_320X210_recycler_bn:
                startNativeRecyclerViewActivity(getPosId(view.getId()));
                break;
        }
    }

    /**
     * 启动普通的原生模板广告
     *
     * @param posId
     */
    private void startNativeNormalActivity(String posId) {
        Intent intent = new Intent(this, NativeTempletNormalActivity.class);
        intent.putExtra(NativeTempletNormalActivity.EXTRA_KEY_POS_ID, posId);
        startActivity(intent);
    }

    /**
     * 启动内嵌在List里面的原生模板广告
     *
     * @param posId
     */
    private void startNativeRecyclerViewActivity(String posId) {
        Intent intent = new Intent(this, NativeTempletRecyclerViewActivity.class);
        intent.putExtra(NativeTempletRecyclerViewActivity.EXTRA_KEY_POS_ID, posId);
        startActivity(intent);
    }

    private String getPosId(int viewId) {
        String posId = "";
        switch (viewId) {
            case R.id.native_temple_640X320_normal_bn:
            case R.id.native_temple_640X320_recycler_bn:
                posId = Constants.NATIVE_TEMPLET_640X320_POS_ID;
                break;
            case R.id.native_temple_320X210_normal_bn:
            case R.id.native_temple_320X210_recycler_bn:
                posId = Constants.NATIVE_TEMPLET_320X210_POS_ID;
                break;
            case R.id.native_temple_group_320X210_normal_bn:
            case R.id.native_temple_group_320X210_recycler_bn:
                posId = Constants.NATIVE_TEMPLET_GROUP_320X210_POS_ID;
                break;
        }
        return posId;
    }
}
