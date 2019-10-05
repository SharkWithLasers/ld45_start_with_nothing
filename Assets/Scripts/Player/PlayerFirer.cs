using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PlayerFirer : MonoBehaviour
{
    [SerializeField] private IntReference numBullets;

    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private FloatReference fireRate;

    [SerializeField] private BoolReference inDebug;

    // Update is called once per frame
    void Update()
    {
        if (inDebug && Input.GetKeyDown(KeyCode.P))
        {
            numBullets.Value += 10;
        }

        if (Input.GetButtonDown("Fire1") && numBullets > 0)
        {
            FireBullet();
            numBullets.Value -= 1;
        }
    }


    void FireBullet()
    {
        var bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }
}
