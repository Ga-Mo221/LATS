using UnityEngine;

public class loadintro : MonoBehaviour
{
    [SerializeField] private Intro _intro;

    public void addLoop()
    {
        _intro.addLoop();
    }
}
