using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeUI : MonoBehaviour
{
  public Image MainItemImage;
  public Image[] MaterialsImage;

  public StaticDataItem StaticMaterialData;

  public void Set(FinishDeviceType device)
  {
    var staticMaterialData = StaticDataManager.Instance.StaticMaterialItems.FirstOrDefault(x => x.Type == ItemType.FinishDevice && x.FinishDeviceType == device);

    MainItemImage.sprite = staticMaterialData.Icon;

    foreach (var item in MaterialsImage)
      item.gameObject.SetActive(false);
    
    for (var i = 0; i < staticMaterialData.FinishDeviceRecipe.Items.Length; i++)
    {
      var item = staticMaterialData.FinishDeviceRecipe.Items[i];
      var deviceItem = StaticDataManager.Instance.StaticMaterialItems.FirstOrDefault(x => x.Type == ItemType.Device && x.DeviceType == item.DeviceType);

      MaterialsImage[i].gameObject.SetActive(true);
      MaterialsImage[i].sprite = deviceItem.Icon;
    }
  }

  public void Set(DeviceType device)
  {
    var staticMaterialData = StaticDataManager.Instance.StaticMaterialItems.FirstOrDefault(x => x.Type == ItemType.Device && x.DeviceType == device);

    MainItemImage.sprite = staticMaterialData.Icon;

    foreach (var item in MaterialsImage)
      item.gameObject.SetActive(false);

    for (var i = 0; i < staticMaterialData.DeviceRecipe.Items.Length; i++)
    {
      var item = staticMaterialData.DeviceRecipe.Items[i];
      var deviceItem = StaticDataManager.Instance.StaticMaterialItems.FirstOrDefault(x => x.Type == ItemType.Material && x.MaterialType == item.MaterialType);

      MaterialsImage[i].gameObject.SetActive(true);
      MaterialsImage[i].sprite = deviceItem.Icon;
    }
  }
}