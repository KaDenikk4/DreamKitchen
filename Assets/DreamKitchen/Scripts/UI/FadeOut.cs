using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Image imageFadeOut;
    void Start()
    {
        imageFadeOut.canvasRenderer.SetAlpha(1.0f);
        cFadeOut();
    }

    private void Update()
    {
        if (imageFadeOut.canvasRenderer.GetAlpha() == 0)
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void cFadeOut()
    {
        imageFadeOut.CrossFadeAlpha(0, 2, false);
    }
}
