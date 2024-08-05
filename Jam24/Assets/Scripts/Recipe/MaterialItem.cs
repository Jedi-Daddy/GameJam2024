using System;

[Serializable]
public class MaterialItem
{
  public MaterialType Type;
  public int Count = 1;

  public MaterialItem() { }

  public MaterialItem(MaterialType type, int count) =>
    (Type, Count) = (type, count);
}