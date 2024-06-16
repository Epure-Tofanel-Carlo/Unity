using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpener : MonoBehaviour
{
    public Canvas shop;
    public GameObject buttoninfo;
    public GameObject hud;
    public StoreBuyManager buymanager;
    public GameObject sellmanager;
    void Start()
    {
        hud.SetActive(true);
        buttoninfo.SetActive(false);
        shop.enabled = false;
    }
    void Update()
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.E))
        {
            buymanager.createShopItems();
            sellmanager.GetComponentInChildren<StoreSellMananger>().createShopItems();
            sellmanager.SetActive(false);
            hud.SetActive(false);
            shop.enabled = true;
            buttoninfo.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("A intrat");
        buttoninfo.SetActive(true);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hud.SetActive(true);
        // Debug.Log("A iesit");
        buttoninfo.SetActive(false);
    }
}
