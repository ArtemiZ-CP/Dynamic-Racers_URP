using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private IStoreController _controller;
    private IExtensionProvider _extensions;
    private string _productId = "your_product_id";

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _controller = controller;
        _extensions = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("IAP Initialization Failed: " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError("IAP Initialization Failed: " + error + " - " + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError("Purchase Failed: " + failureReason);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (string.Equals(args.purchasedProduct.definition.id, _productId, StringComparison.Ordinal))
        {
            Debug.Log("Purchase successful.");
        }
        return PurchaseProcessingResult.Complete;
    }

    public void BuyProduct()
    {
        _controller.InitiatePurchase(_productId);
    }
    
    private void Start()
    {
        // var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        // builder.AddProduct(_productId, ProductType.Consumable);
        // UnityPurchasing.Initialize((IDetailedStoreListener)this, builder);
    }
}