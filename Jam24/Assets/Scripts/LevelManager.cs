using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LevelManager : MonoBehaviour
{
  public TimeSpan PlaytimeInMinutes;

  public PhotonView view;

  public int Coins;
  public int LevelDurationInSeconds;

  public TMP_Text TimerText;
  public TMP_Text CoinsText;

  public FinishDeviceType MainDevice;

  public GameObject[] RecipeUis;

  public GameObject MainHUD;
  public GameObject Inventory;
  public GameObject DefeateHUD;

  private List<RecipeUI> _recipeUis;

  private void Awake()
  {
    SoundSystem.Instance.Play("Level");

    PlaytimeInMinutes = TimeSpan.FromSeconds(LevelDurationInSeconds);

    _recipeUis = new List<RecipeUI>();

    foreach (var it in RecipeUis)
      _recipeUis.Add(it.GetComponent<RecipeUI>());
  }

  private void Start()
  {
    StartCoroutine(nameof(TimerProc));
  }

  private IEnumerator TimerProc()
  {
    TimerText.text = PlaytimeInMinutes.ToString(@"mm\:ss");
    while (PlaytimeInMinutes.TotalSeconds > 0)
    {
      yield return new WaitForSeconds(1);

      PlaytimeInMinutes = PlaytimeInMinutes.Subtract(TimeSpan.FromSeconds(1));
      TimerText.text = PlaytimeInMinutes.ToString(@"mm\:ss");
    }

    DefeateHUD.SetActive(true);

    DefeateHUD.GetComponent<FinishHUD>().SetScore(Coins);

    Inventory.SetActive(false);
    MainHUD.SetActive(false);

    yield break;
  }

  public void TryAddCoins()
  {
    var time = LevelDurationInSeconds - PlaytimeInMinutes.TotalSeconds;

    var coins = 0;
    if (time <= 60)
      coins = 90;
    else if (time <= 90)
      coins = 60;
    else if (time <= 120)
      coins = 30;


    Coins += coins;
    CoinsText.text = Coins.ToString();

    // todo add coroutine for animation
  }

  public void SetGoalId(int id)
  {
    if (!view.IsMine)
      return;

    MainDevice = (FinishDeviceType) id + 1;

    var index = 0;
    _recipeUis[index++].Set(MainDevice);

    var staticMaterialData = StaticDataManager.Instance.StaticMaterialItems.FirstOrDefault(x => x.Type == ItemType.FinishDevice && x.FinishDeviceType == MainDevice);

    for (var i = 0; i < staticMaterialData.FinishDeviceRecipe.Items.Length; i++)
    {
      var item = staticMaterialData.FinishDeviceRecipe.Items[i];
      _recipeUis[index++].Set(item.DeviceType);
    }
  }
}