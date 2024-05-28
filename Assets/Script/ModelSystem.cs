using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelSystem : MonoBehaviour
{
    public List<Sprite> hairSprites;
    public List<Sprite> faceSprites;
    public List<Sprite> shirtSprites;
    public List<Sprite> pantsSprites;
    public List<Sprite> shoesSprites;

    public Image hairImage;
    public Image faceImage;
    public Image shirtImage;
    public Image pantsImage;
    public Image shoesImage;

    private int hairIndex;
    private int faceIndex;
    private int shirtIndex;
    private int pantsIndex;
    private int shoesIndex;

    public void Start()
    {
        LoadCharacter();
    }
    // ใช้เมธอดนี้เพื่อกำหนดรูปภาพผมของตัวละคร
    public void SetHair(int hairIndex)
    {
        if (hairIndex >= 0 && hairIndex < hairSprites.Count)
        {
            this.hairIndex = hairIndex;
            hairImage.sprite = hairSprites[hairIndex];
        }
    }

    // ใช้เมธอดนี้เพื่อกำหนดรูปภาพหน้าของตัวละคร
    public void SetFace(int faceIndex)
    {
        if (faceIndex >= 0 && faceIndex < faceSprites.Count)
        {
            this.faceIndex = faceIndex;
            faceImage.sprite = faceSprites[faceIndex];
        }
    }

    // ใช้เมธอดนี้เพื่อกำหนดรูปภาพเสื้อของตัวละคร
    public void SetShirt(int shirtIndex)
    {
        if (shirtIndex >= 0 && shirtIndex < shirtSprites.Count)
        {
            this.shirtIndex = shirtIndex;
            shirtImage.sprite = shirtSprites[shirtIndex];
        }
    }

    // ใช้เมธอดนี้เพื่อกำหนดรูปภาพกางเกงของตัวละคร
    public void SetPants(int pantsIndex)
    {
        if (pantsIndex >= 0 && pantsIndex < pantsSprites.Count)
        {
            this.pantsIndex = pantsIndex;
            pantsImage.sprite = pantsSprites[pantsIndex];
        }
    }

    // ใช้เมธอดนี้เพื่อกำหนดรูปภาพรองเท้าของตัวละคร
    public void SetShoes(int shoesIndex)
    {
        if (shoesIndex >= 0 && shoesIndex < shoesSprites.Count)
        {
            this.shoesIndex = shoesIndex;
            shoesImage.sprite = shoesSprites[shoesIndex];
        }
    }


    public void ApplyItem(string type, int index)
    {
        // ทำการปรับแต่งตัวละครตามชนิดและดัชนีที่รับมา
        if (type == "Face")
        {
            SetFace(index);
        }
        else if (type == "Shirt")
        {
            SetShirt(index);
        }
        else if (type == "Pants")
        {
            SetPants(index);
        }
        else if (type == "Shoes")
        {
            SetShoes(index);
        }
        else if (type == "Hair")
        {
            SetHair(index);

        }
    }


    // ใช้เมธอดนี้เพื่อบันทึกชุดของผู้เล่น
    public void SaveCharacter()
    {
        PlayerPrefs.SetInt("HairIndex", hairIndex);
        PlayerPrefs.SetInt("FaceIndex", faceIndex);
        PlayerPrefs.SetInt("ShirtIndex", shirtIndex);
        PlayerPrefs.SetInt("PantsIndex", pantsIndex);
        PlayerPrefs.SetInt("ShoesIndex", shoesIndex);
    }

    // ใช้เมธอดนี้เพื่อโหลดชุดของผู้เล่น
    public void LoadCharacter()
    {
        hairIndex = PlayerPrefs.GetInt("HairIndex", 0);
        faceIndex = PlayerPrefs.GetInt("FaceIndex", 0);
        shirtIndex = PlayerPrefs.GetInt("ShirtIndex", 0);
        pantsIndex = PlayerPrefs.GetInt("PantsIndex", 0);
        shoesIndex = PlayerPrefs.GetInt("ShoesIndex", 0);

        SetHair(hairIndex);
        SetFace(faceIndex);
        SetShirt(shirtIndex);
        SetPants(pantsIndex);
        SetShoes(shoesIndex);
    }

}
