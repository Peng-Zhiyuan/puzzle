package com.oppo.mobaddemo.activity;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.os.Bundle;
import android.text.method.ScrollingMovementMethod;
import android.util.Log;
import android.view.View;
import android.widget.TextView;

import com.oppo.mobad.api.ad.RewardVideoAd;
import com.oppo.mobad.api.listener.IRewardVideoAdListener;
import com.oppo.mobad.api.params.RewardVideoAdParams;
import com.oppo.mobaddemo.utils.Constants;
import com.oppo.mobaddemo.R;

/**
 * Created by 80059209 on 2018-04-15.
 */

public class RewardVideoActivity extends Activity implements IRewardVideoAdListener {
    private static final String TAG = "RewardVideoActivity";
    private TextView mStatusTv;
    private RewardVideoAd mRewardVideoAd;
    private AlertDialog mAlertDialog;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_reward_video);
        initView();
        initData();
    }

    @Override
    protected void onDestroy() {
        destroyVideo();
        super.onDestroy();
    }

    private void initView() {
        mStatusTv = (TextView) findViewById(R.id.status_tv);
        mStatusTv.setMovementMethod(new ScrollingMovementMethod());
    }

    private void initData() {
        /**
         * 构造激励视频广告对象
         */
        mRewardVideoAd = new RewardVideoAd(this, Constants.REWARD_VIDEO_POS_ID, this);
        printStatusMsg("初始化视频广告.");
        /**
         *当请求视频广告成功时，用来展示视频播放入口的Dialog
         */
        mAlertDialog = new AlertDialog.Builder(this)
                .setCancelable(false)
                .setTitle("获取奖励")
                .setMessage("观看视频、重新获取闯关机会？")
                .setPositiveButton("确定", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        playVideo();
                        dialog.dismiss();
                    }
                })
                .setNegativeButton("取消", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        dialog.dismiss();
                    }
                })
                .create();
    }

    public void onBnClick(View view) {
        switch (view.getId()) {
            case R.id.req_video_ad:
                loadVideo();
                break;
        }
    }

    private void loadVideo() {
        /**
         * 调用loadAd方法请求激励视频广告;通过RewardVideoAdParams.setFetchTimeout方法可以设置请求
         * 视频广告最大超时时间，单位毫秒；
         */
        RewardVideoAdParams rewardVideoAdParams=new RewardVideoAdParams.Builder()
                .setFetchTimeout(3000)
                .build();
        mRewardVideoAd.loadAd(rewardVideoAdParams);
        printStatusMsg("请求加载视频广告.");
    }

    private void playVideo() {
        /**
         * 调用showAd方法播放激励视频广告
         */
        mRewardVideoAd.showAd();
        printStatusMsg("播放视频广告.");
    }

    private void destroyVideo() {
        /**
         * 销毁激励视频广告
         */
        mRewardVideoAd.destroyAd();
        printStatusMsg("释放视频广告资源.");
    }

    private void printStatusMsg(String txt) {
        if (null != txt) {
            Log.d(TAG, txt);
            mStatusTv.setText(mStatusTv.getText() + "\n" + txt);
        }
    }

    @Override
    public void onAdSuccess() {
        /**
         *请求视频广告成功、展示播放视频的入口Dialog
         */
        mAlertDialog.show();
        printStatusMsg("请求视频广告成功.");
    }

    @Override
    public void onAdFailed(String msg) {
        /**
         * 请求广告失败、不展示播放视频的入口Dialog
         */
        printStatusMsg("请求视频广告失败. msg=" + msg);
    }

    @Override
    public void onAdClick(long currentPosition) {
        printStatusMsg("视频广告被点击，当前播放进度 = " + currentPosition + " 秒");
    }

    @Override
    public void onVideoPlayStart() {
        printStatusMsg("视频开始播放.");
    }

    @Override
    public void onVideoPlayComplete() {
        /**
         * TODO 用户完成激励视频观看，给予用户奖励。
         */
        printStatusMsg("视频广告播放完成,给用户发放奖励.");
    }

    @Override
    public void onVideoPlayError(String msg) {
        printStatusMsg("视频播放错误，错误信息=" + msg);
    }

    @Override
    public void onVideoPlayClose(long currentPosition) {
        printStatusMsg("视频广告被关闭，当前播放进度 = " + currentPosition + " 秒");
    }

    @Override
    public void onLandingPageOpen() {
        printStatusMsg("视频广告落地页打开.");
    }

    @Override
    public void onLandingPageClose() {
        printStatusMsg("视频广告落地页关闭.");
    }
}
