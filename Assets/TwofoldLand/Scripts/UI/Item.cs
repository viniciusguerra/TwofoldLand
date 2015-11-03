using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemData itemData;

    public ItemData ItemData
    {
        get
        {
            return itemData;
        }
    }

    public string ItemAddress
    {
        get
        {
            return itemAddressText.text;
        }
        set
        {
            itemAddressText.text = value;
        }
    }

    public string ItemName
    {
        get
        {
            return LanguageManager.Instance.GetTextValue(itemData.itemNameKey);
        }
    }

    public string ItemDescription
    {
        get
        {
            return LanguageManager.Instance.GetTextValue(itemData.itemDescriptionKey);
        }
    }

    public bool Consumable
    {
        get
        {
            return itemData.consumable;
        }
    }

    public string CostToClone
    {
        get
        {
            return itemData.itemCostToClone;
        }
    }

    [Header("UI Elements")]
    [SerializeField]
    private Text itemNameText;
    [SerializeField]
    private Text itemAddressText;
    [SerializeField]
    private Text itemDescriptionText;
    [SerializeField]
    private Text itemCostText;

    public GameObject itemCostArea;
    public GameObject consumableItemArea;

    public void Initialize(ItemData itemData, string address)
    {
        this.itemData = itemData;

        ItemAddress = address;

        itemNameText.text = LanguageManager.Instance.GetTextValue(itemData.itemNameKey);
        itemDescriptionText.text = LanguageManager.Instance.GetTextValue(itemData.itemDescriptionKey);
        itemCostText.text = itemData.itemCostToClone;

        itemCostArea.SetActive(Consumable ? false : true);
        consumableItemArea.SetActive(Consumable ? true : false);
    }

    public bool Use(int amount)
    {
        if (Consumable)
        {
            if (amount > 1)
            {
                HUD.Instance.log.ShowMessage("Consumable Item. Only one Instance can be used.");

                return false;
            }
            else
            {
                HUD.Instance.log.ShowMessage(ItemName + " consumed");

                Destroy(gameObject);

                return true;
            }
        }
        else
        {
            int cost = int.Parse(CostToClone) * amount;

            if (Player.Instance.Aura > cost)
            {
                Player.Instance.SpendAura(cost);

                Destroy(gameObject);
                return true;
            }
            else
                return false;
        }
    }

    public static Item GetItemFromCommandParameter(string parameter, out int amount)
    {
        string[] parameterSplit = parameter.Split(new char[] { '[', ']' }, System.StringSplitOptions.RemoveEmptyEntries);

        Item item = HUD.Instance.storage.GetItemFromAddress(parameterSplit[0]);

        amount = (item != null && parameterSplit.Length > 1) ? int.Parse(parameterSplit[1]) : 0;

        return item;
    }
}
