using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public ModelSystem ModelSystem;
    public int[] facePrices;
    public int[] shirtPrices;
    public int[] pantsPrices;
    public int[] shoesPrices;
    public int[] hairPrices;

    public TMP_Text[] PriceLists;
    public TMP_Text totalMoneyText,myMoneyText; // สร้างเชื่อมตัวแปร TextMeshPro ใน Unity Inspector
    private int currentCoins;
    private List<CartItem> cart = new List<CartItem>();

    public GameObject AlertText;
    public GameObject AlertName;
    public GameObject hairMenu;
    public GameObject makeupMenu;
    public GameObject shirtMenu;
    public GameObject pantMenu;
    public GameObject shoeMenu;

    public GameObject UI;
    public GameObject Name;
    public GameObject ScreenRoom;
    public GameObject NameTextUI;
    public TMP_Text NameInput, NameText;
    public string CharactorName;

    public Texture2D textureToSave;

    public AudioSource AudioMain;
    public AudioClip CashSFX, ScreenShootSFX,AlertSFX;
    private void Start()
    {
/*        PlayerPrefs.SetInt("PlayerCoins", 100); // เอาไว้เทส*/
        currentCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
        UpdateCoinsUI(currentCoins);
        UpdateHairPrices();
    }
    public void BuyItem(string itemType, int itemIndex)
    {
        int[] itemPrices = null;

        switch (itemType)
        {
            case "Face":
                itemPrices = facePrices;
                /*ModelSystem.SetFace(itemIndex);*/
                break;
            case "Shirt":
                itemPrices = shirtPrices;
                /*ModelSystem.SetShirt(itemIndex);*/
                break;
            case "Pants":
                itemPrices = pantsPrices;
                /*ModelSystem.SetPants(itemIndex);*/
                break;
            case "Shoes":
                itemPrices = shoesPrices;
                /*ModelSystem.SetShoes(itemIndex);*/
                break;
            case "Hair":
                itemPrices = hairPrices;
                /*ModelSystem.SetHair(itemIndex);*/
                break;
        }

        if (itemPrices != null && itemIndex >= 0 && itemIndex < itemPrices.Length)
        {
            int itemPrice = itemPrices[itemIndex];

            // ตรวจสอบว่าผู้เล่นมีเงินเพียงพอสำหรับการซื้อรายการ
            if (currentCoins >= itemPrice)
            {
                AudioMain.PlayOneShot(CashSFX, 0.7F);

                // หักเงินจากผู้เล่น
                currentCoins -= itemPrice;
                PlayerPrefs.SetInt("PlayerCoins", currentCoins);

                // อัปเดต UI แสดงเงิน
                UpdateCoinsUI(currentCoins);

                // ประกาศการสั่งซื้อ: ใส่รายการลงใน CharacterCustomization
                ModelSystem.ApplyItem(itemType, itemIndex);

                // บันทึกรายการที่เลือกลงในตะกร้า (ถ้าต้องการ)
                cart.Add(new CartItem(itemType, itemIndex, itemPrice));
                foreach (CartItem item in cart)
                {
                    ModelSystem.ApplyItem(item.type, item.index);
                }

                // บันทึกตัวละครที่เปลี่ยนแปลง
                ModelSystem.SaveCharacter();

                // ล้างตะกร้า
                cart.Clear();
                // คำนวณยอดรวมเงินและอัปเดต TextMeshPro
                int total = CalculateTotalPrice();
                totalMoneyText.text = total.ToString();
            }
            else
            {
                AlertText.SetActive(true); // แสดงข้อความแจ้งเตือนหากเงินไม่เพียงพอ
                AudioMain.PlayOneShot(AlertSFX, 0.7F);
            }
        }
    }


    public void Checkout()
    {
        int total = CalculateTotalPrice();

        

        if (currentCoins >= total)
        {
            // ประกาศการสั่งซื้อ: ใส่รายการลงใน CharacterCustomization
            foreach (CartItem item in cart)
            {
                ModelSystem.ApplyItem(item.type, item.index);
            }

            // บันทึกตัวละครที่เปลี่ยนแปลง
            ModelSystem.SaveCharacter();

            // ล้างตะกร้า
            cart.Clear();

            // หักเงินจากผู้เล่น
            currentCoins -= total;
            PlayerPrefs.SetInt("PlayerCoins", currentCoins);

            // อัปเดต UI แสดงเงิน
            UpdateCoinsUI(currentCoins);
        }
        else
        {
            AlertText.SetActive(true);
        }
    }

    public void CloseAlertText()
    {
        AlertText.SetActive(false);
    }

    public void CloseNameAlert()
    {
        AlertName.SetActive(false);
    }

    private int CalculateTotalPrice()
    {
        int total = 0;
        foreach (CartItem item in cart)
        {
            total += item.price;
        }
        return total;
    }

    private void UpdateCoinsUI(int currentCoins)
    {
        myMoneyText.text = currentCoins.ToString();
    }

    public void UpdateFacePrices()
    {
        for (int i = 0; i < facePrices.Length; i++)
        {
            PriceLists[i].text = facePrices[i].ToString();
        }
    }

    public void UpdateShirtPrices()
    {
        for (int i = 0; i < shirtPrices.Length; i++)
        {
            PriceLists[i].text = shirtPrices[i].ToString();
        }
    }

    public void UpdatePantsPrices()
    {
        for (int i = 0; i < pantsPrices.Length; i++)
        {
            PriceLists[i].text = pantsPrices[i].ToString();
        }
    }

    public void UpdateShoesPrices()
    {
        for (int i = 0; i < shoesPrices.Length; i++)
        {
            PriceLists[i].text = shoesPrices[i].ToString();
        }
    }

    public void UpdateHairPrices()
    {
        for (int i = 0; i < hairPrices.Length; i++)
        {
            PriceLists[i].text = hairPrices[i].ToString();
        }
    }

    public void ShowMenu(string menuName)
    {
        hairMenu.SetActive(menuName == "Hair");
        makeupMenu.SetActive(menuName == "Makeup");
        shirtMenu.SetActive(menuName == "Shirt");
        pantMenu.SetActive(menuName == "Pant");
        shoeMenu.SetActive(menuName == "Shoe");

        if (menuName == "Hair")
        {
            UpdateHairPrices();
        }
        else if (menuName == "Makeup")
        {
            UpdateFacePrices();
        }
        else if (menuName == "Shirt")
        {
            UpdateShirtPrices();
        }
        else if (menuName == "Pant")
        {
            UpdatePantsPrices();
        }
        else if (menuName == "Shoe")
        {
            UpdateShoesPrices();
        }
    }

    public void SetName()
    {
        Name.SetActive(true);

    }
    public void GotoScreen()
    {
        CharactorName = NameInput.text;
        Name.SetActive(false);
        NameText.text = CharactorName;

        UI.SetActive(false);
        ScreenRoom.SetActive(true);
        NameTextUI.SetActive(true);
    }

    public void GotoBackShop()
    {

        UI.SetActive(true);
        ScreenRoom.SetActive(false);
        NameTextUI.SetActive(false);
    }

    public void SaveImage()
    {
        // ปิด GameObject ก่อนถ่ายภาพจอ
        ScreenRoom.SetActive(false);
        AudioMain.PlayOneShot(ScreenShootSFX, 0.7F);
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = "Screenshot" + timeStamp + ".png";
        string pathToSave = fileName;
        ScreenCapture.CaptureScreenshot(pathToSave);
        StartCoroutine(ShareScreenshotCoroutine(pathToSave));
        Debug.Log("บันทึกภาพแล้ว: " + pathToSave);
        StartCoroutine(ActivateAfterDelay());
        // เปิด GameObject เมื่อถ่ายภาพเสร็จแล้ว
        /*ScreenRoom.SetActive(true);*/
    }

    private IEnumerator ShareScreenshotCoroutine(string fileName)
    {
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
        yield return new WaitForSeconds(1); // Wait for the screenshot to be saved.
        new NativeShare().AddFile(filePath).Share();
    }

    IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(1.5f); // นับเวลาถอยหลัง 1 วินาที

        // เปิดใช้งาน ScreenRoom หลังจากผ่านไป 1 วินาที
        ScreenRoom.SetActive(true);
    }

}

public class CartItem
{
    public string type;
    public int index;
    public int price;

    public CartItem(string type, int index, int price)
    {
        this.type = type;
        this.index = index;
        this.price = price;
    }
}
