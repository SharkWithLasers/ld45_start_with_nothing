using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//not actually fuel depleted text. more like... traumatizeThenDie
public class FuelDepletedText : MonoBehaviour
{
    private TraumatibleCamera _traumatible;

    // Start is called before the first frame update
    void Start()
    {
        // turns outs this doesn't only apply to cameras
        _traumatible = GetComponent<TraumatibleCamera>();

        StartCoroutine(TraumatizeThenDie());
    }

    IEnumerator TraumatizeThenDie()
    {
        _traumatible.Traumatize();

        yield return new WaitForSeconds(2f);

        Destroy(this.gameObject);
    }
}
