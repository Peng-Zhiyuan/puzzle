package com.oppo.mobaddemo.activity;

import android.app.Activity;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.view.View;
import android.widget.Toast;

import com.androidquery.AQuery;
import com.oppo.mobad.api.ad.NativeAd;
import com.oppo.mobad.api.listener.INativeAdListener;
import com.oppo.mobad.api.params.INativeAdData;
import com.oppo.mobad.api.params.INativeAdFile;
import com.oppo.mobad.api.params.NativeAdError;
import com.oppo.mobaddemo.R;
import com.oppo.mobaddemo.utils.Constants;

import java.util.List;

public class Native512X512Activity extends Activity implements INativeAdListener {

    private static final String TAG = "Native512X512Activity";

    private NativeAd mNativeAd;
    /**
     * 原生广告数据对象。
     */
    private INativeAdData mINativeAdData;
    //
    private AQuery mAQuery;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_native_text_icon_512_512);
        initView();
        initData();
    }

    private void initView() {
        mAQuery = new AQuery(this);
        findViewById(R.id.native_ad_container).setVisibility(View.GONE);
        mAQuery.id(R.id.load_native_ad_bn).clicked(this, "loadAd");
        mAQuery.id(R.id.show_native_ad_bn).clicked(this, "showAd").enabled(false);
    }

    public void loadAd() {
        if (null != mNativeAd) {
            /**
             *调用loadAd方法加载原生广告。
             */
            mNativeAd.loadAd();
        }
    }

    public void showAd() {
        /**
         *在展示原生广告前调用isAdValid判断当前广告是否有效，否则即使展示了广告，也是无效曝光、点击，不计费的
         *注意：每个INativeAdData对象只有一次有效曝光、一次有效点击；多次曝光，多次点击都只扣一次费。
         */
        if (null != mINativeAdData && mINativeAdData.isAdValid()) {
            findViewById(R.id.native_ad_container).setVisibility(View.VISIBLE);
            /**
             *展示推广应用的ICON，大小为512X512。
             */
            if (null != mINativeAdData.getIconFiles() && mINativeAdData.getIconFiles().size() > 0) {
                INativeAdFile iNativeAdFile = (INativeAdFile) mINativeAdData.getIconFiles().get(0);
                mAQuery.id(R.id.icon_iv).image(iNativeAdFile.getUrl(), false, true);
            }
            /**
             * 判断是否需要展示“广告”Logo标签
             */
            if (null != mINativeAdData.getLogoFile()) {
                mAQuery.id(R.id.logo_iv).image(mINativeAdData.getLogoFile().getUrl(), false, true);
            }
            mAQuery.id(R.id.title_tv).text(null != mINativeAdData.getTitle() ? mINativeAdData.getTitle() : "");
            mAQuery.id(R.id.desc_tv).text(null != mINativeAdData.getDesc() ? mINativeAdData.getDesc() : "");
            mAQuery.id(R.id.action_bn).text(null != mINativeAdData.getClickBnText() ? mINativeAdData.getClickBnText() : "").clicked(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    /**
                     *原生广告被点击时必须调用onAdClick方法通知SDK进行点击统计；
                     * 注意：onAdClick方法必须在onAdShow方法之后再调用才有效，否则是无效点击。
                     */
                    mINativeAdData.onAdClick(v);
                }
            });
            /**
             * 原生广告曝光时必须调用onAdShow方法通知SDK进行曝光统计，否则就没有曝光数据。
             */
            mINativeAdData.onAdShow(findViewById(R.id.native_ad_container));
        }
    }

    private void initData() {
        /**
         *构造NativeAd对象。
         */
        mNativeAd = new NativeAd(this, Constants.NATIVE_512X512_TEXT_ICON_POS_ID, this);
    }

    @Override
    protected void onDestroy() {
        if (null != mNativeAd) {
            /**
             *銷毀NativeAd对象，释放资源。
             */
            mNativeAd.destroyAd();
        }
        super.onDestroy();
    }

    /**
     * 原生广告加载成功，在onAdSuccess回调广告数据
     *
     * @param iNativeAdDataList
     */
    @Override
    public void onAdSuccess(List iNativeAdDataList) {
        if (null != iNativeAdDataList && iNativeAdDataList.size() > 0) {
            mINativeAdData = (INativeAdData) iNativeAdDataList.get(0);
            mAQuery.id(R.id.show_native_ad_bn).enabled(true);
            Toast.makeText(Native512X512Activity.this, "加载原生广告成功", Toast.LENGTH_SHORT).show();
        }
    }

    @Override
    public void onAdFailed(NativeAdError nativeAdError) {
        Toast.makeText(Native512X512Activity.this, "加载原生广告失败,错误码：" + nativeAdError.toString(), Toast.LENGTH_LONG).show();
    }

    @Override
    public void onAdError(NativeAdError nativeAdError, INativeAdData iNativeAdData) {
        Toast.makeText(Native512X512Activity.this, "调用原生广告统计方法出错,错误码：" + nativeAdError.toString(), Toast.LENGTH_LONG).show();
    }
}
