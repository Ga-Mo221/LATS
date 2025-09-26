using UnityEngine;

public class GetMainCam : MonoBehaviour
{
    void Start()
    {
        setupCamera();
    }

    public void setupCamera()
    {
        GameObject mainCamObj = Camera.main?.gameObject;

        Canvas canvas = transform.GetComponent<Canvas>();
        if (canvas != null && mainCamObj != null)
        {
            Camera cam = mainCamObj.GetComponent<Camera>();
            if (cam != null)
            {
                canvas.worldCamera = cam;
                if (gameObject.CompareTag("Overlay"))
                    canvas.sortingLayerName = "Overlay";
                else if (transform.name == "PosInventory")
                    canvas.sortingLayerName = "UI-";
                else if (transform.name == "UI_PlayerStats")
                    canvas.sortingLayerName = "UI--";
                else
                    canvas.sortingLayerName = "UI";
            }
            else
            {
                Debug.LogWarning("[GetMainCam] mainCamObj không chứa component Camera.");
            }
        }
        else
        {
            Debug.LogWarning($"[{transform.name}][GetMainCam] Không tìm thấy Canvas hoặc mainCamObj null.");
        }
    }
}
