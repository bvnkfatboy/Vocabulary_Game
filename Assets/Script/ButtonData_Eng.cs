using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonData_Eng : MonoBehaviour
{
    public int matchingEnglishIndex;
    public Image buttonImage;
    public Sprite originalSprite; // Add this field
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static implicit operator Button(ButtonData_Eng v)
    {
        throw new NotImplementedException();
    }
}
