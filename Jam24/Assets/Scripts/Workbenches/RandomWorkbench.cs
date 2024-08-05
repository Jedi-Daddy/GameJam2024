using System;
using System.Collections;
using UnityEngine;

public class RandomWorkbench : MonoBehaviour
{
  public int WorkSpeed;
  public bool HasAnyItem;

  public ScrapMaterialType? ScrapMaterialType;
  public MaterialType? MaterialType;
  public DeviceType? DeviceType;

  public GameObject GeneratedItem;

  private System.Random _random;

  private Coroutine _produceItem;

  private void Awake()
  {
    _random = new System.Random();
    _produceItem = StartCoroutine(nameof(ProduceItem));
  }

  public ScrapMaterialType TakeScrap()
  {
    HasAnyItem = false;
    GeneratedItem.SetActive(false);
    var scrap = ScrapMaterialType.Value;
    ScrapMaterialType = null;

    SoundSystem.Instance.PlayOneShot("TakeScrap");

    return scrap;
  }

  public MaterialType TakeMaterial()
  {
    HasAnyItem = false;
    GeneratedItem.SetActive(false);
    var material = MaterialType.Value;
    MaterialType = null;

    SoundSystem.Instance.PlayOneShot("TakeScrap");

    return material;
  }

  public DeviceType TakeDevice()
  {
    HasAnyItem = false;
    GeneratedItem.SetActive(false);
    var device = DeviceType.Value;
    DeviceType = null;

    SoundSystem.Instance.PlayOneShot("TakeScrap");

    return device;
  }

  private IEnumerator ProduceItem()
  {
    while (true)
    {
      if (!HasAnyItem)
      {
        yield return new WaitForSeconds(WorkSpeed);

        var randomItemType = (ItemType) _random.Next(1, 4);
        switch (randomItemType)
        {
          case ItemType.Scrap:
            var randomItem = _random.Next(1, 5);
            ScrapMaterialType = (ScrapMaterialType)randomItem;
            break;

          case ItemType.Device:
            randomItem = _random.Next(1, 4);
            MaterialType = (MaterialType)randomItem;
            break;

          case ItemType.Material:
            randomItem = _random.Next(1, 5);
            DeviceType = (DeviceType)randomItem;
            break;
        }

        HasAnyItem = true;
        GeneratedItem.SetActive(true);

        SoundSystem.Instance.PlayOneShot("RandomItemSpawned");
      }
      else
      {
        yield return new WaitForSeconds(1);
      }
    }
  }
}
