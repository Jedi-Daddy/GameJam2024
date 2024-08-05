using System.Collections;
using UnityEngine;

public class Packager : MonoBehaviour
{
  public DeviceType Item;
  public int WorkSpeed;

  public int GeneratedMaterialsCount;
  public GameObject GeneratedMaterial;

  private int _devicesCount;
  private Coroutine _produceItem;

  private void Awake() =>
    _produceItem = StartCoroutine(nameof(ProduceItem));

  private void Update()
  {
    if (GeneratedMaterialsCount > 0 && !GeneratedMaterial.activeInHierarchy)
      GeneratedMaterial.SetActive(true);
  }

  public void AddMaterials() =>
    _devicesCount++;

  private IEnumerator ProduceItem()
  {
    while (true)
    {
      if (_devicesCount > 0)
      {
        yield return new WaitForSeconds(WorkSpeed);

        GeneratedMaterialsCount++;
        _devicesCount--;

        SoundSystem.Instance.PlayOneShot("DevicePacked");
      }
      else
      {
        yield return new WaitForSeconds(1);
      }
    }
  }
}
