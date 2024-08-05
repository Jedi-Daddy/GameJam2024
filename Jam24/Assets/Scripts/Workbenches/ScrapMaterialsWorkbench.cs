using System.Collections;
using UnityEngine;

public class ScrapMaterialsWorkbench : MonoBehaviour
{
  public int WorkSpeed;
  public bool HasScrap;

  public ScrapMaterialType Type;
  public GameObject GeneratedScrap;

  private Coroutine _produceItem;

  private void Awake() =>
    _produceItem = StartCoroutine(nameof(ProduceItem));

  public void TakeScrap()
  {
    HasScrap = false;
    GeneratedScrap.SetActive(false);

    SoundSystem.Instance.PlayOneShot("TakeScrap");
  }

  private IEnumerator ProduceItem()
  {
    while (true)
    {
      if (!HasScrap)
      {
        yield return new WaitForSeconds(WorkSpeed);

        HasScrap = true;
        GeneratedScrap.SetActive(true);

        SoundSystem.Instance.PlayOneShot("ScrapSpawned");
      }
      else
      {
        yield return new WaitForSeconds(1);
      }
    }
  }
}
