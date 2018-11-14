
using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

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

        public static string IsInterAdLoaded(string arg)
        {
            return "true";
        }

        static string lastCallId;
        public static void ShowInterAd(string callId, string arg)
        {
            lastCallId = callId;
            if (Advertisement.IsReady())
            {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show(options);
            }
            else
            {
                Debug.Log("[NativeSDKManager] rewardedVideo not ready");
                Gate.CallReturn(callId, "false");
            }
            
        }

        private static void HandleShowResult(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    Debug.Log("The ad was successfully shown.");
                    //
                    // YOUR CODE TO REWARD THE GAMER
                    // Give coins etc.
                    Gate.CallReturn(lastCallId, "true");
                    break;
                case ShowResult.Skipped:
                    Debug.Log("The ad was skipped before reaching the end.");
                    Gate.CallReturn(lastCallId, "false");
                    break;
                case ShowResult.Failed:
                    Debug.LogError("The ad failed to be shown.");
                    Gate.CallReturn(lastCallId, "false");
                    break;
            }
        }
    }
}
