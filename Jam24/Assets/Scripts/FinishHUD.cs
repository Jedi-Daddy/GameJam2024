using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class FinishHUD : MonoBehaviour
{
  public TextMeshProUGUI ScoreText;

  public void SetScore(int score)
  {
    ScoreText.text = $"Score: {score}";
  }

  public void ReturnToMenu()
  {
    StopAllCoroutines();
    PhotonNetwork.LeaveRoom();
    SceneManager.LoadScene("MainUI");
  }
}