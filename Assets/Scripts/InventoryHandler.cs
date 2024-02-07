using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        public string itemName;
        public string type;
        public Sprite sprite;
    }

    public List<GameObject> itemImageSlots;
    public Text pointsText;
    public Canvas inventoryCanvas;
    public Image redBorder;
    public Image greenBorder;
    private Item[] inventory = new Item[10];
    private int inventoryCount = 0;
    private int selectedSlotIndex = -1;
    private int points = 0;
    private bool isRedFlashing = false;
    private bool isGreenFlashing = false;

    void Start()
    {
        UpdatePointsUI();
        redBorder.enabled = false;
        greenBorder.enabled = false;
    }

    void Update()
    {
        CheckInput();
        HighlightSelectedSlot();

        if (isRedFlashing)
        {
            StartCoroutine(FlashRedBorder());
        }
        else
        {
            redBorder.enabled = false;
        }

        if (isGreenFlashing)
        {
            StartCoroutine(FlashGreenBorder());
        }
        else
        {
            greenBorder.enabled = false;
        }
    }

    void CheckInput()
    {
        int selectedSlot = -1;

        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                selectedSlot = i - 1;
                break;
            }
        }

        if (selectedSlot >= 0 && selectedSlot < itemImageSlots.Count)
        {
            selectedSlotIndex = selectedSlot;
        }
    }

    void HighlightSelectedSlot()
    {
        for (int i = 0; i < itemImageSlots.Count; i++)
        {
            Image image = itemImageSlots[i].GetComponent<Image>();

            if (i == selectedSlotIndex)
            {
                image.color = Color.green;
            }
            else
            {
                image.color = Color.white;
            }
        }
    }


    public void AddItem(Item newItem)
    {
        for(int i = 0; i< 10; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = newItem;
                inventoryCount++;

                DisplayItemSprite(newItem, i);
                break;
            }
        }
        

        //PrintInventory();
    }

    private void PrintInventory()
    {
        UnityEngine.Debug.Log("--------------------Inventory: ----------------------");
        foreach (Item item in inventory)
        {
            UnityEngine.Debug.Log($"Item: {item.itemName}, Type: {item.type}");
        }
    }

    private void DisplayItemSprite(Item item, int index)
    {
        GameObject imageSlot = itemImageSlots[index];

        if (imageSlot != null)
        {
            GameObject newImageObject = new GameObject("ItemImage");
            newImageObject.transform.SetParent(imageSlot.transform, false);

            Image image = newImageObject.AddComponent<Image>();
            image.sprite = item.sprite;
            image.preserveAspect = true;
            image.rectTransform.sizeDelta = new Vector2(70f, 70f); 
        }
        else
        {
            UnityEngine.Debug.LogWarning("No available Image slots in the UI.");
        }
    }

    private GameObject FindImageSlot()
    {
        if (inventoryCount < 10)
        {
            return itemImageSlots[inventoryCount-1];
        }
        else
        {
            return null;
        }
    }

    public void HandleThrowAway(string tag)
    {

        if (selectedSlotIndex >= 0 && selectedSlotIndex < inventory.Length && inventory[selectedSlotIndex] != null)
        {
            UnityEngine.Debug.Log(tag);
            UnityEngine.Debug.Log(inventory[selectedSlotIndex]?.type);

            if (inventory[selectedSlotIndex]?.type == tag)
            {
                updatePoints(10) ;
                StartCoroutine(StartGreenBorderFlash());
            }
            else
            {
                updatePoints(-10);
                StartCoroutine(StartRedBorderFlash());
            }
            
            Transform selectedSlotTransform = itemImageSlots[selectedSlotIndex].transform;
            Transform childImageTransform = selectedSlotTransform.Find("ItemImage");
            if (childImageTransform != null)
            {
                Destroy(childImageTransform.gameObject);
                inventory[selectedSlotIndex] = null;
            }
            UnityEngine.Debug.Log("Item thrown away");
        }
    }

    private IEnumerator StartRedBorderFlash()
    {
        isRedFlashing = true;
        yield return new WaitForSeconds(0.5f); // Adjust the duration of the flashing effect
        isRedFlashing = false;
    }

    private IEnumerator FlashRedBorder()
    {
        UpdateRedBorder(true);
        yield return new WaitForSeconds(0.1f);
        UpdateRedBorder(false);
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator StartGreenBorderFlash()
    {
        isGreenFlashing = true;
        yield return new WaitForSeconds(0.5f); // Adjust the duration of the flashing effect
        isGreenFlashing = false;
    }

    private IEnumerator FlashGreenBorder()
    {
        UpdateGreenBorder(true);
        yield return new WaitForSeconds(0.1f);
        UpdateGreenBorder(false);
        yield return new WaitForSeconds(0.1f);
    }

    private void UpdatePointsUI()
    {
        if (pointsText != null)
        {
            UnityEngine.Debug.Log(points);
            int displayPoints = Math.Abs(points);
            UnityEngine.Debug.Log(displayPoints);
            // Check if points are negative and format the text accordingly
            string pointsDisplayText = (points < 0) ? $"Points: -{displayPoints}" : $"Points: {displayPoints}";

            pointsText.text = pointsDisplayText;
        }
    }

    private void updatePoints(int amount)
    {
        points += amount;
        UpdatePointsUI();
    }

    private void UpdateRedBorder(bool enable)
    {
        redBorder.enabled = enable;
    }
    private void UpdateGreenBorder(bool enable)
    {
        greenBorder.enabled = enable;
    }
}