
using UnityEngine;
using System.Collections;

namespace BridgeClasses
{
    public class NativeSDKManager
    {
        // public static void Pay(string args)
        // {
        //     Debug.Log("onTestNotify");
        // }

        public static void Pay(string id, string args)
        {
            //Debug.Log("up stream call SUCCESS!");
            //NativeBridge.UpstreamCallReturn(id, "result");
            Debug.Log("[NativeSDKManager] simulate ret SUCCESS");
            Gate.CallReturn(id, "SUCCESS");
        }
    }
}
