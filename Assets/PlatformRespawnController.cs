using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRespawnController : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToFade = new List<GameObject>();
    [SerializeField] HeatPlatformController heater;
    public IEnumerator Fade(float timeToFade)
    {
        foreach (var item in objectsToFade)
        {
            item.SetActive(false);
        }
        yield return new WaitForSeconds(timeToFade);
        heater.ResetCountOfUses();
        foreach (var item in objectsToFade)
        {
            item.SetActive(true);
        }
    }
}
