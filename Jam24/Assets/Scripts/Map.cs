using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
  public GameObject[] WorkbenchSpawnPoints;
  public GameObject WorkbenchPrefab;
  public int WorkbenchCount;

  public GameObject[] MaterialsSpawnPoints;
  public GameObject MaterialsPrefab;
  public int MaterialsCount;

  public GameObject[] UserSpawnPoints;
  public GameObject UserPrefab;

  private int _connectedUsersCount;

  private void Awake()
  {
    var workbenchSpawnPointIndexes = GetRandomIndexes(WorkbenchCount);
    var workItemsSpawnPointIndexes = GetRandomIndexes(MaterialsCount);

    SpawnItems(WorkbenchSpawnPoints, workbenchSpawnPointIndexes, WorkbenchPrefab);
    SpawnItems(MaterialsSpawnPoints, workItemsSpawnPointIndexes, MaterialsPrefab);
    SpawnItems(UserSpawnPoints, new List<int> { _connectedUsersCount }, UserPrefab);
  }

  private List<int> GetRandomIndexes(int maxItems)
  {
    var workbenchSpawnPointIndexes = new List<int>();
    var random = new System.Random();

    while(workbenchSpawnPointIndexes.Count < maxItems)
    {
      var index = random.Next(0, maxItems);

      if (workbenchSpawnPointIndexes.Contains(index))
        continue;

      workbenchSpawnPointIndexes.Add(index);
    }

    return workbenchSpawnPointIndexes;
  }

  private void SpawnItems(GameObject[] spawnPoints, List<int> spawnPointIndexes, GameObject prefab)
  {
    foreach(var index in spawnPointIndexes)
      Instantiate(prefab, spawnPoints[index].transform);
  }
}
