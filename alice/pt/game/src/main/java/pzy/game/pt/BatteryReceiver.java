package pzy.game.pt;

/**
 * Created by edrotiy_mac1 on 2017/5/30.
 */

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

/**
 * 广播接受者
 */
class BatteryReceiver extends BroadcastReceiver
{
    @Override
    public void onReceive(Context context, Intent intent) {

        //判断它是否是为电量变化的Broadcast Action
//        if(Intent.ACTION_BATTERY_CHANGED.equals(intent.getAction())){
//            //获取当前电量
//            int level = intent.getIntExtra("level", 0);
//            //电量的总刻度
//            int scale = intent.getIntExtra("scale", 100);
//            JSONObject jo = new JSONObject();
//            JSONObjectUtil.put(jo, "@name", "BatteryChanged");
//            JSONObjectUtil.put(jo, "level", level);
//            JSONObjectUtil.put(jo, "scale", scale);
//            String jsonString = jo.toString();
//            edroity.unity.csharpchannel.Proxy.enqueueMessage(jsonString);
//        }
    }
}