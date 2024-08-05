using UnityEngine;

public class StaticDataManager : MonoBehaviour
{
  public static StaticDataManager Instance;

  public StaticDataItem[] StaticMaterialItems;

  private void Awake()
  {
    Instance = this;
    DontDestroyOnLoad(gameObject);
  }
}
