using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
  public Image icon;
  public GameObject SelectedMask;
  public GameObject CountBGImage;
  public TextMeshProUGUI CountText;

  public int index;
  public bool equipped;

  private bool _selected;
  private bool _isInteractable;

  public StaticDataItem StaticMaterialData;

  public void Set(FinishDeviceType device)
  {
    StaticMaterialData = StaticDataManager.Instance.StaticMaterialItems.FirstOrDefault(x => x.Type == ItemType.FinishDevice && x.FinishDeviceType == device);

    _isInteractable = true;
    icon.gameObject.SetActive(true);
    icon.sprite = StaticMaterialData.Icon;

    CountBGImage.SetActive(false);
    CountText.text = string.Empty;
  }

  public void Set(DeviceType device)
  {
    StaticMaterialData = StaticDataManager.Instance.StaticMaterialItems.FirstOrDefault(x => x.Type == ItemType.Device && x.DeviceType == device);

    _isInteractable = true;
    icon.gameObject.SetActive(true);
    icon.sprite = StaticMaterialData.Icon;

    CountBGImage.SetActive(false);
    CountText.text = string.Empty;
  }

  public void Set(MaterialType material)
  {
    StaticMaterialData = StaticDataManager.Instance.StaticMaterialItems.FirstOrDefault(x => x.Type == ItemType.Material && x.MaterialType == material);

    _isInteractable = true;
    icon.gameObject.SetActive(true);
    icon.sprite = StaticMaterialData.Icon;

    CountBGImage.SetActive(false);
    CountText.text = string.Empty;
  }

  public void Set(ScrapMaterialType scrap)
  {
    StaticMaterialData = StaticDataManager.Instance.StaticMaterialItems.FirstOrDefault(x => x.Type == ItemType.Scrap    && x.ScrapMaterialType == scrap);

    _isInteractable = true;
    icon.gameObject.SetActive(true);
    icon.sprite = StaticMaterialData.Icon;

    CountBGImage.SetActive(false);
    CountText.text = string.Empty;
  }

  // clears the item slot
  public void Clear()
  {
    icon.gameObject.SetActive(false);
    SelectedMask.SetActive(false);
    CountText.text = string.Empty;
    _isInteractable = false;
  }

  // called when we click on the slot
  public void OnButtonClick()
  {
    if (!_isInteractable)
      return;

    if (_selected)
    {
      Inventory.instance.DeselectItem(index);
      SelectedMask.SetActive(false);
    }
    else
    {
      Inventory.instance.SelectItem(StaticMaterialData, index);
      SelectedMask.SetActive(true);
    }

    _selected = !_selected;
  }
}