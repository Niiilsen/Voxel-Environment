using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Material blockMaterial;
    public Block[,,] chunkData;

    IEnumerator BuildChunk(int sizeX, int sizeY, int sizeZ)
    {
        chunkData = new Block[sizeX, sizeY, sizeZ];

        //create blocks
        for (int z = 0; z < sizeZ; z++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    if (Random.Range(0, 100) < 50)
                        chunkData[x, y, z] = new Block(BlockUtils.BlockType.DIRT, pos,
                                        this.gameObject, blockMaterial);
                    else
                        chunkData[x, y, z] = new Block(BlockUtils.BlockType.GRASS, pos,
                                        this.gameObject, blockMaterial);
                }
                yield return null;
            }
        }

        //draw blocks
        for (int z = 0; z < sizeZ; z++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    chunkData[x, y, z].Draw();

                }
            }
        }
    }


    // Use this for initialization
    void Start()
    {
        StartCoroutine(BuildChunk(5, 5, 5));
    }
}
