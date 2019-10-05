using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelGenerator levelGen;

    // Start is called before the first frame update
    void Start()
    {
        levelGen.GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
