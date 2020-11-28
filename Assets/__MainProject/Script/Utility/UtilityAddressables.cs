using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class UtilityAddressables
{
    public static void LoadAssetByStringAddressAsync<T>(string address, Action<AsyncOperationHandle<T>> callback)
   
    {
        UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(address).Completed += callback;
    }

    public static void LoadAssetByAssetReference<T>(AssetReference assetReference, Action<AsyncOperationHandle<T>> callback)
 
    {
    
        assetReference.LoadAssetAsync<T>().Completed += callback;
    }


}
