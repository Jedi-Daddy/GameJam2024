using System;
using UnityEngine;

[Serializable]
public class StaticDataItem
{
  [Header("Info")]
  public string Name;
  public string Description;

  public Sprite Icon;
  public ItemType Type;
  public GameObject DropPrefab;

  public FinishDeviceType FinishDeviceType;
  public DeviceType DeviceType;
  public MaterialType MaterialType;
  public ScrapMaterialType ScrapMaterialType;

  public DeviceRecipeSer DeviceRecipe;

  public FinishDeviceRecipe FinishDeviceRecipe;

  [Header("Stacking")]
  public bool CanStack;
  public int MaxStackAmount;
}

[Serializable]
public class FinishDeviceRecipe
{
  public FinishDeviceRecipeItem[] Items;
}

[Serializable]
public class FinishDeviceRecipeItem
{
  public DeviceType DeviceType;
  public int Count;
}

[Serializable]
public class DeviceRecipeSer
{
  public DeviceRecipeItem[] Items;
}

[Serializable]
public class DeviceRecipeItem
{
  public MaterialType MaterialType;
  public int Count;
}