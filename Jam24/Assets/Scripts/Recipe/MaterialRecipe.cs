using System;

[Serializable]
public class MaterialRecipe
{
  public MaterialType MaterialType;
  public ScrapMaterialType ScrapType;

  public MaterialRecipe() { }

  public MaterialRecipe(MaterialType materialType, ScrapMaterialType scrapType) =>
    (MaterialType, ScrapType) = (materialType, scrapType);
}