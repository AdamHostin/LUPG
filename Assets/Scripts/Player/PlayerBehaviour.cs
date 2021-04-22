using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float Health = 100;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
   

    public void GroupHeal(int addHealth)
    {
        Health += addHealth;
    }
    // Pouzil som enumerator kvoli setreniu vykonu. Je tam case break lebo predpokladam ze tam v buducnosti budu dalsie podmienky spojene z zivotom
    private IEnumerator HealthChack(float waitTime)
    {
        switch (Health)
        {
            case 1:
                Debug.Log("VyhralSi");
                break;
          
        }
        yield return new WaitForSeconds(waitTime);
    }
}
