using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Inventory : MonoBehaviour
{
  // singleton
  public static Inventory instance;

  public int MaxDevicesItems = 2;

  public ItemSlotUI[] uiSlots;

  public GameObject inventoryWindow;

  [Header("Selected Item")]
  public TextMeshProUGUI selectedItemName;
  public TextMeshProUGUI selectedItemDescription;


  public Transform dropPosition;

  public TextMeshProUGUI selectedItemStatNames;
  public TextMeshProUGUI selectedItemStatValues;

  public GameObject useButton;
  public GameObject dropButton;

  [Header("Events")]
  public UnityEvent onOpenInventory;
  public UnityEvent onCloseInventory;


  // Selected items indexes
  public List<int> SelectedItemIndexes;

  private MaterialType? _material;
  private ScrapMaterialType? _scrap;
  private List<DeviceType> _devices;
  public List<FinishDeviceType> _finishDevices;

  private bool _isOpened;

  private void Awake()
  {
    instance = this;
    _devices = new List<DeviceType>();
    _finishDevices = new List<FinishDeviceType>();
  }

  private void Start()
  {
    inventoryWindow.SetActive(false);

    // initialize the slots
    for (var x = 0; x < uiSlots.Length; x++)
    {
      uiSlots[x].index = x;
      uiSlots[x].Clear();
    }
  }

  public void ChangeInventoryWindowState()
  {
    Debug.LogError($"Scrap {_scrap}");
    Debug.LogError($"Material {_material}");

    if (_isOpened)
      CloseInventoryWindow();
    else
      OpenInventoryWindow();
  }

  public bool CanTakeItem(DeviceType? deviceType = null) =>
    deviceType.HasValue ? _devices.Count < MaxDevicesItems : _scrap == null && _material == null;

  public bool HasScrap() =>
    _scrap != null;

  public ScrapMaterialType GetScrap() =>
    _scrap.Value;

  public bool HasMaterial() =>
    _material != null;

  public MaterialType GetMaterial() =>
    _material.Value;

  public void AddMaterial(MaterialType material) =>
    _material = material;

  public MaterialType TakeMaterial()
  {
    var material = _material;
    _material = null;

    return material.Value;
  }

  public void AddScrap(ScrapMaterialType scrap) =>
    _scrap = scrap;

  public ScrapMaterialType TakeScrap()
  {
    var scrap = _scrap;
    _scrap = null;

    return scrap.Value;
  }

  public void AddDevice(DeviceType device) =>
    _devices.Add(device);

  public void TakeDevice(DeviceType device) =>
    _devices.Remove(device);

  public void SelectItem(StaticDataItem data, int index)
  {
    SelectedItemIndexes.Add(index);
    selectedItemName.text = data.Name;
    selectedItemDescription.text = data.Description;
  }

  public void DeselectItem(int index) =>
    SelectedItemIndexes.Remove(index);

  public void TryMerge()
  {
    if (SelectedItemIndexes.Count < 2)
      return;

    var items = new Dictionary<DeviceType, int>();
    foreach(var index in SelectedItemIndexes)
    {
      var slot = uiSlots[index];
      if (items.ContainsKey(slot.StaticMaterialData.DeviceType))
        items[slot.StaticMaterialData.DeviceType]++;
      else
        items.Add(slot.StaticMaterialData.DeviceType, 1);
    }

    var item = StaticDataManager.Instance.StaticMaterialItems.FirstOrDefault(x => x.Type == ItemType.FinishDevice && x.FinishDeviceRecipe.Items.All(y => items.ContainsKey(y.DeviceType) && y.Count == items[y.DeviceType]));
    if (item == null)
    {
      Debug.LogWarning($"Can't find materials for merge");
      return;
    }

    _finishDevices.Add(item.FinishDeviceType);
    foreach(var device in item.FinishDeviceRecipe.Items)
    {
      for (var i = 0; i < device.Count; i++)
      {
        _devices.Remove(device.DeviceType);
      }
    }

    SelectedItemIndexes.Clear();

    for (var x = 0; x < uiSlots.Length; x++)
      uiSlots[x].Clear();

    var index1 = 0;

    foreach (var item1 in _finishDevices)
    {
      uiSlots[index1].Set(item1);
      index1++;
    }

    foreach (var item1 in _devices)
    {
      uiSlots[index1].Set(item1);
      index1++;
    }

    if (_material.HasValue)
    {
      uiSlots[index1].Set(_material.Value);
      index1++;
    }

    if (_scrap.HasValue)
    {
      uiSlots[index1].Set(_scrap.Value);
    }

  }

  private void OpenInventoryWindow()
  {
    _isOpened = true;

    inventoryWindow.SetActive(true);

    var index = 0;

    foreach(var item in _finishDevices)
    {
      uiSlots[index].Set(item);
      index++;
    }

    foreach(var item in _devices)
    {
      uiSlots[index].Set(item);
      index++;
    }

    if (_material.HasValue)
    {
      uiSlots[index].Set(_material.Value);
      index++;
    }

    if (_scrap.HasValue)
    {
      uiSlots[index].Set(_scrap.Value);
    }
  }

  private void CloseInventoryWindow()
  {
    _isOpened = false;
    SelectedItemIndexes.Clear();
    inventoryWindow.SetActive(false);

    for (var x = 0; x < uiSlots.Length; x++)
      uiSlots[x].Clear();
  }
}