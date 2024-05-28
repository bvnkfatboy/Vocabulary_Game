using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuyButton : MonoBehaviour
{
    public ShopSystem shopSystem; // เชื่อม ShopSystem ให้กับปุ่ม

    public string itemType; // ระบุประเภทของไอเทม (Face, Shirt, Pants, Shoes, Hair)
    public int itemIndex; // ระบุดัชนีของไอเทมในรายการ

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickBuy);
    }

    private void OnClickBuy()
    {
/*        print(itemType);
        print(itemIndex);*/
        shopSystem.BuyItem(itemType, itemIndex);
    }
}
