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
    bool isActive = true;

    Collider2D platformColl;
    Collider2D platformTrigger;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        platformColl = GetComponentsInChildren<BoxCollider2D>()[1];
        platformTrigger = GetComponent<BoxCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Player") && isActive)
        {
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(timeBeforeFading);

        while (sprite.color.a > opacityOffset)
        {
            float newAlpha = Mathf.Lerp(sprite.color.a, 0, fadeSpeed * Time.deltaTime);
            newAlpha = Mathf.Clamp(newAlpha, 0, 1);
            sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b,newAlpha);

            if (sprite.color.a < minAlphaToCollide)
            {
                platformColl.enabled = false;
                isActive = false;
            }

            yield return changeFrequency;
        }

        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);

        Debug.Log("Before Fade window");

        yield return new WaitForSeconds(secondsToBeInvisible);

        Debug.Log("After Fade window");

        while (sprite.color.a <  1 - opacityOffset)
        {
            float newAlpha = Mathf.Lerp(sprite.color.a, 1, fadeSpeed * Time.deltaTime);
            newAlpha = Mathf.Clamp(newAlpha, 0, 1);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, newAlpha);

            if (sprite.color.a < minAlphaToCollide)
            {
                platformColl.enabled = true;
            }

            yield return changeFrequency;
        }
        Debug.Log("Should work");
        isActive = true;
    }
    
}
