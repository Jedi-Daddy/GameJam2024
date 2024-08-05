using UnityEngine;

public class FinishGameWorkbench : MonoBehaviour
{
  public FinishDeviceType Recipe;

  public bool TryGetFinishDevice(FinishDeviceType type)
  {
    if (Recipe != type)
      return false;

    return true;
  }
}
