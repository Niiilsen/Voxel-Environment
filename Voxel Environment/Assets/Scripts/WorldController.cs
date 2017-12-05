using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public Vector3 worldSize = new Vector3(10,2,10);

    public GameObject block;

    public IEnumerator BuildWorld()
    {
        for (int z = 0; z < worldSize.z; z++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                for (int x = 0; x < worldSize.x; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    GameObject cube = GameObject.Instantiate(block, pos, Quaternion.identity);
                    cube.name = x + "_" + y + "_" + z;
                }
                yield return null;

            }
        }
    }

    // Use this for initialization
    void Start()
    {
        Utils.seed = Random.Range(0, 999999.0f);
        print(Utils.seed);
        StartCoroutine(BuildWorld());
    }
   
}
