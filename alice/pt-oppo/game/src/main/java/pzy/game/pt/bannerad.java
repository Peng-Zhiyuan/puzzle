package pzy.game.pt;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewGroup.LayoutParams;
import android.widget.RelativeLayout;

import com.oppo.mobad.api.ad.BannerAd;
import com.oppo.mobad.api.listener.IBannerAdListener;

import pzy.game.pt.oppo_ad.Constants;

public class bannerad implements IBannerAdListener{
	private static final String TAG = "BannerActivity";
    private RelativeLayout mAdContainer;
    private BannerAd mBannerAd;

private Activity act;
public bannerad(Activity ac){

	    act=ac;

        act.runOnUiThread(new Runnable(){

			@Override
			public void run() {
				// TODO Auto-generated method stub
		        init();
			}});
    
        
    }


 public void onDestroy() {
        if (null != mBannerAd) {
            /**
             * ���˳�ҳ��ʱ����destroyAd���ͷŹ����Դ
             */
            mBannerAd.destroyAd();
        }
        
    }

    private void init() {
     //   mAdContainer = (RelativeLayout) findViewById(R.id.ad_container);
    	   mAdContainer =new RelativeLayout(act);
        /**
         * ���� bannerAd
         */
        mBannerAd = new BannerAd(act, Constants.BANNER_POS_ID);
        /**
         * ����Banner�����Ϊ������
         */
        mBannerAd.setAdListener(this);
        
        /**
         * ��ȡBanner���View����View��ӵ����ҳ����ȥ
         *
         */
        View adView = mBannerAd.getAdView();
        /**
         * mBannerAd.getAdView()���ؿ���Ϊ�գ��жϺ������
         */
        if (null != adView) {
            /**
             * ����addView�ǿ����Լ�ָ��Banner���ķ���λ�á�һ����ҳ�涥�����ߵײ���
             */
            RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.WRAP_CONTENT);
            layoutParams.addRule(RelativeLayout.ALIGN_PARENT_BOTTOM);
            mAdContainer.addView(adView, layoutParams);
        }
        /**
         * ����loadAd()����������.
         */
        mBannerAd.loadAd();
		act.addContentView( mAdContainer,  new LayoutParams(LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT)); 
    }



    @Override
    public void onAdShow() {
        /**
         *���չʾ
         */
   //     Log.d(TAG, "onAdShow");
    }

    @Override
    public void onAdFailed(String errMsg) {
        /**
         *������ʧ��
         */
    //    Log.d(TAG, "onAdFailed:errMsg=" + (null != errMsg ? errMsg : ""));
    }

    @Override
    public void onAdReady() {
    	 
        /**
         *������ɹ�
         */
     //   Log.d(TAG, "onAdReady");
    }

    @Override
    public void onAdClick() {
        /**
         *��汻���
         */
    //    Log.d(TAG, "onAdClick");
    }

    @Override
    public void onAdClose() {
        /**
         *��汻�ر�
         */
       // Log.d(TAG, "onAdClose");
    }

}
