using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class chapterFader : MonoBehaviour
{
    public Image blackImage;
    [SerializeField] private float alpha;

    public void Start()
    {
        StartCoroutine(FadeIn());
        
    }

    public void FadeTo(string _chapterName)
    {
        
        StartCoroutine(FadeOut(_chapterName));
    }

    IEnumerator FadeIn()
    {
        alpha = 1;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }

    }

    IEnumerator FadeOut(string chapterName)
    {
        alpha = 0;

        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        SceneManager.LoadScene(chapterName);
    }



}
