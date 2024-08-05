using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Linq;

public class InteractionManager : MonoBehaviour
{
  public GameObject MainHUD;
  public GameObject InventoryHud;
  public GameObject VictoryHUD;
  public LevelManager LevelManager;


  public PhotonView view;

  public TextMeshProUGUI promptText;

  private GameObject _interactedGameObject;

  private void Start()
  {
    if (promptText == null)
    {
      promptText = GameObject.Find("InteractPromptText")?.GetComponent<TextMeshProUGUI>();
    }
    promptText.gameObject.SetActive(false);
  }

  private void Update()
  {
    if (!view.IsMine)
      return;

    if (Input.GetKeyDown(KeyCode.E))
      Interact();

    if (Input.GetKeyDown(KeyCode.Tab))
      Inventory.instance.ChangeInventoryWindowState();
  }

  public void DestroyScrapOrMaterial()
  {
    Inventory.instance.TakeScrap();
    Inventory.instance.TakeMaterial();

    // todo ahryshenko Add sound/effects/droped item ???
  }

  public void Interact()
  {
    if (_interactedGameObject == null)
      return;

    var scrapWorkbench = _interactedGameObject.GetComponent<ScrapMaterialsWorkbench>();
    if (scrapWorkbench != null && scrapWorkbench.HasScrap && Inventory.instance.CanTakeItem())
    {
      scrapWorkbench.TakeScrap();
      Inventory.instance.AddScrap(scrapWorkbench.Type);

      return;
    }

    var materialWorkbench = _interactedGameObject.GetComponent<MaterialWorkbench>();
    if (materialWorkbench != null)
    {
      if (materialWorkbench.HasMaterial)
      {
        if (Inventory.instance.CanTakeItem())
        {
          materialWorkbench.TakeMaterial();
          Inventory.instance.AddMaterial(materialWorkbench.Recipe.MaterialType);
        }
      }
      else
      {
        if (Inventory.instance.HasScrap() && Inventory.instance.GetScrap() == materialWorkbench.Recipe.ScrapType)
        {
          Inventory.instance.TakeScrap();
          materialWorkbench.AddScrap();
        }
      }

      return;
    }

    var deviceWorkbench = _interactedGameObject.GetComponent<DeviceWorkbench>();
    if (deviceWorkbench != null)
    {
      if (deviceWorkbench.HasDevice)
      {
        if (Inventory.instance.CanTakeItem(deviceWorkbench.Recipe.Device))
        {
          LevelManager.TryAddCoins();
          deviceWorkbench.TakeDevice();
          Inventory.instance.AddDevice(deviceWorkbench.Recipe.Device);
        }
      }
      else if (Inventory.instance.HasMaterial() && deviceWorkbench.MaterialNeeded(Inventory.instance.GetMaterial()))
      {
        deviceWorkbench.AddMaterials(Inventory.instance.TakeMaterial());
      }

      return;
    }

    var randomWorkbench = _interactedGameObject.GetComponent<RandomWorkbench>();
    if (randomWorkbench != null)
    {
      if (randomWorkbench.DeviceType != null && Inventory.instance.CanTakeItem(randomWorkbench.DeviceType))
      {
        var device = randomWorkbench.TakeDevice();
        Inventory.instance.AddDevice(device);
      }
      else if (randomWorkbench.MaterialType != null && Inventory.instance.CanTakeItem())
      {
        var material = randomWorkbench.TakeMaterial();
        Inventory.instance.AddMaterial(material);
      }
      else if (randomWorkbench.ScrapMaterialType != null && Inventory.instance.CanTakeItem())
      {
        var scrap = randomWorkbench.TakeScrap();
        Inventory.instance.AddScrap(scrap);
      }

      return;
    }

    var finishGameWorkbench = _interactedGameObject.GetComponent<FinishGameWorkbench>();
    if (finishGameWorkbench != null)
    {
      if (Inventory.instance._finishDevices.Any(x => finishGameWorkbench.TryGetFinishDevice(x)))
      {
        VictoryHUD.SetActive(true);

        VictoryHUD.GetComponent<FinishHUD>().SetScore(LevelManager.Coins);

        InventoryHud.SetActive(false);
        MainHUD.SetActive(false);
      }

      return;
    }
  }

  public void OnTriggerEnter(Collider other)
  {
    if (!view.IsMine)
      return;

    var workbench = other.gameObject.GetComponent<DeviceWorkbench>();
    if (workbench != null)
    {
      promptText.gameObject.SetActive(true);
      _interactedGameObject = other.gameObject;
      return;
    }

    var materialWorkbench = other.gameObject.GetComponent<MaterialWorkbench>();
    if (materialWorkbench != null)
    {
      promptText.gameObject.SetActive(true);
      _interactedGameObject = other.gameObject;
      return;
    }

    var scrapMaterialsWorkbench = other.gameObject.GetComponent<ScrapMaterialsWorkbench>();
    if (scrapMaterialsWorkbench != null)
    {
      promptText.gameObject.SetActive(true);
      _interactedGameObject = other.gameObject;
      return;
    }

    var randomWorkbench = other.gameObject.GetComponent<RandomWorkbench>();
    if (randomWorkbench != null)
    {
      promptText.gameObject.SetActive(true);
      _interactedGameObject = other.gameObject;
      return;
    }

    var finishGameWorkbench = other.gameObject.GetComponent<FinishGameWorkbench>();
    if (finishGameWorkbench != null)
    {
      promptText.gameObject.SetActive(true);
      _interactedGameObject = other.gameObject;
      return;
    }
  }

  public void OnTriggerExit(Collider other)
  {
    if (!view.IsMine)
      return;

    var workbench = other.gameObject.GetComponent<DeviceWorkbench>();
    if (workbench != null)
    {
      promptText.gameObject.SetActive(false);
      _interactedGameObject = null;
      return;
    }

    var materialWorkbench = other.gameObject.GetComponent<MaterialWorkbench>();
    if (materialWorkbench != null)
    {
      promptText.gameObject.SetActive(false);
      _interactedGameObject = null;
      return;
    }

    var scrapMaterialsWorkbench = other.gameObject.GetComponent<ScrapMaterialsWorkbench>();
    if (scrapMaterialsWorkbench != null)
    {
      promptText.gameObject.SetActive(false);
      _interactedGameObject = null;
      return;
    }

    var randomWorkbench = other.gameObject.GetComponent<RandomWorkbench>();
    if (randomWorkbench != null)
    {
      promptText.gameObject.SetActive(false);
      _interactedGameObject = null;
      return;
    }

    var finishGameWorkbench = other.gameObject.GetComponent<FinishGameWorkbench>();
    if (finishGameWorkbench != null)
    {
      promptText.gameObject.SetActive(false);
      _interactedGameObject = null;
      return;
    }
  }
}