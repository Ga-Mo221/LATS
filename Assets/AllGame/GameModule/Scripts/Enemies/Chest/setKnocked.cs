using UnityEngine;

public class setKnocked : MonoBehaviour
{
    public void setKnockedPlayer()
    {
        if (PlayerManager.Instance == null)
        {
            Debug.LogWarning("setKnockedPlayer: PlayerManager.Instance is null");
            return;
        }

        PlayerManager.Instance.setKnoked(!PlayerManager.Instance.getKnocked());
    }
}
