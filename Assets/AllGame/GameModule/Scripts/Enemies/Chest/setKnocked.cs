using UnityEngine;

public class setKnocked : MonoBehaviour
{
    
    public static setKnocked Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform viewTransform;

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
