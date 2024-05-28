using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonData_Thai : MonoBehaviour
{
    // Start is called before the first frame update
    public int matchingThaiIndex;
    public Image buttonImage;
    public Sprite originalSprite; // Add this field
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static implicit operator Button(ButtonData_Thai v)
    {
        throw new NotImplementedException();
    }


}
