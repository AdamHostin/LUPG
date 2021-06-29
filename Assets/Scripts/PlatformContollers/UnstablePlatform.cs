using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstablePlatform : MonoBehaviour
{
    [SerializeField] float timeBeforeFading;
    [SerializeField] float secondsToBeInvisible;
    [SerializeField] float fadeSpeed;
    [SerializeField] int changeFrequency;
    [SerializeField] float opacityOffset;
    [SerializeField] float minAlphaToCollide;
    [SerializeField] GameObject[] objectsToFade;
    

    Collider2D platformColl;
    Collider2D platformTrigger;
    SpriteRenderer[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        platformColl = GetComponentsInChildren<BoxCollider2D>()[1];
        platformTrigger = GetComponent<BoxCollider2D>();
        sprites = GetComponentsInChildren<SpriteRenderer>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(timeBeforeFading);

        while (sprites[0].color.a > opacityOffset)
        {
            float newAlpha = Mathf.Lerp(sprites[0].color.a, 0, fadeSpeed * Time.deltaTime);
            newAlpha = Mathf.Clamp(newAlpha, 0, 1);
            foreach (var sprite  in sprites) sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b,newAlpha);

            if (sprites[0].color.a < minAlphaToCollide)
            {
                platformColl.enabled = false;
                platformTrigger.enabled = false;  
            }

            yield return changeFrequency;
        }

        foreach (var sprite in sprites) sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
        DisableObjects();

        yield return new WaitForSeconds(secondsToBeInvisible);


        while (sprites[0].color.a <  1 - opacityOffset)
        {
            float newAlpha = Mathf.Lerp(sprites[0].color.a, 1, fadeSpeed * Time.deltaTime);
            newAlpha = Mathf.Clamp(newAlpha, 0, 1);
            foreach (var sprite in sprites) sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, newAlpha);

            if (sprites[0].color.a < minAlphaToCollide)
            {
                platformColl.enabled = true;

            }

            yield return changeFrequency;
        }
        foreach (var sprite in sprites) sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        platformTrigger.enabled = true;
        
    }
    

    void DisableObjects()
    {
        foreach (var childObject in objectsToFade)
        {
            childObject.SetActive(false);
        }
    }

    void EnableObjects()
    {
        foreach (var childObject in objectsToFade)
        {
            childObject.SetActive(true);
        }
    }
}
