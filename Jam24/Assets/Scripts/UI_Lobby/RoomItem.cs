using TMPro;
using System;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
  public TextMeshProUGUI Name;

  public Action<string> OnJoinClick;

  public void Join() =>
    OnJoinClick?.Invoke(Name.text);
}