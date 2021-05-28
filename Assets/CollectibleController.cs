using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [SerializeField] GameObject collectibleObject;
    [SerializeField] float secondsToReenable;

    public void DisableCollectible()
    {
        collectibleObject.SetActive(false);
    }

    IEnumerator ReenableCollectible()
    {
        yield return new WaitForSeconds(secondsToReenable);

        collectibleObject.SetActive(true);
    }
}
