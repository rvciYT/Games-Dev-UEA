using UnityEngine;

public class TownHouseDestroyed : MonoBehaviour
{
    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.TownHouseGameOver();
        }
    }
}
