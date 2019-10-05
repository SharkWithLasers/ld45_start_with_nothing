using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private FloatReference bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);

        if (transform.position.x > 5f)
        {
            Destroy(gameObject);
        }
    }

}
