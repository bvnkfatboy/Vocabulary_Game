using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject hairMenu;
    public GameObject makeupMenu;
    public GameObject shirtMenu;
    public GameObject pantMenu;
    public GameObject shoeMenu;

    public void ShowMenu(string menuName)
    {
        hairMenu.SetActive(menuName == "Hair");
        makeupMenu.SetActive(menuName == "Makeup");
        shirtMenu.SetActive(menuName == "Shirt");
        pantMenu.SetActive(menuName == "Pant");
        shoeMenu.SetActive(menuName == "Shoe");
    }
}
