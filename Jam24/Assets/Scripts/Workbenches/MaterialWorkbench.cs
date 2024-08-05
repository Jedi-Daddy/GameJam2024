using System.Collections;
using UnityEngine;

public class MaterialWorkbench : MonoBehaviour
{
  public MaterialRecipe Recipe;

  public int WorkSpeed;
  public bool HasMaterial;

  public GameObject GeneratedMaterial;

  public void AddScrap() =>
    StartCoroutine(nameof(ProduceItem));

  public void TakeMaterial()
  {
    HasMaterial = false;
    GeneratedMaterial.SetActive(false);
    SoundSystem.Instance.PlayOneShot("TakeMaterial");
  }

  private IEnumerator ProduceItem()
  {
    yield return new WaitForSeconds(WorkSpeed);

    GeneratedMaterial.SetActive(true);
    HasMaterial = true;

    SoundSystem.Instance.PlayOneShot("MaterialCreated");
  }
}
