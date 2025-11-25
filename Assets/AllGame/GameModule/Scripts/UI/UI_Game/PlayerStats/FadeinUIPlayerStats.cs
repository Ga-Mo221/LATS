using System.Collections;
using UnityEngine;

public class FadeinUIPlayerStats : MonoBehaviour
{
    [SerializeField] float duration = 0.25f;
    private RectTransform tr;
    private float currentScale;
    void Start()
    {
        tr = GetComponent<RectTransform>();
        if (tr != null)
            currentScale = tr.localScale.x;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float scal = Mathf.Lerp(currentScale, 0, time/ duration);
            tr.localScale = new Vector3(scal, scal, 1);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
