using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceWorkbench : MonoBehaviour
{
  public DeviceRecipe Recipe;

  public int WorkSpeed;
  public bool HasDevice;

  public GameObject GeneratedMaterial;

  private Dictionary<MaterialType, int> _materials;

  private void Awake()
  {
    _materials = new Dictionary<MaterialType, int>();
  }

  public void AddMaterials(MaterialType material)
  {
    if (_materials.ContainsKey(material))
      _materials[material]++;
    else
      _materials.Add(material, 1);

    SoundSystem.Instance.PlayOneShot("AddMaterials");

    if (!MaterialsEnough())
      return;

    StartCoroutine(nameof(ProduceItem));
  }

  public void TakeDevice()
  {
    HasDevice = false;
    GeneratedMaterial.SetActive(false);
    SoundSystem.Instance.PlayOneShot("TakeDevice");
  }

  public bool MaterialNeeded(MaterialType type)
  {
    var material = Recipe.Materials.FirstOrDefault(x => x.Type == type);
    return material != null && (!_materials.ContainsKey(type) || _materials[type] < material.Count);
  }

  private IEnumerator ProduceItem()
  {
    yield return new WaitForSeconds(WorkSpeed);

    _materials.Clear();

    GeneratedMaterial.SetActive(true);
    HasDevice = true;

    SoundSystem.Instance.PlayOneShot("DeviceCreated");
  }

  private bool MaterialsEnough()
  { 
    foreach (var material in Recipe.Materials)
    {
      if (!_materials.TryGetValue(material.Type, out var count) || count < material.Count)
        return false;
    }

    return true;
  }
}
