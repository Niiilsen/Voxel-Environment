using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public Material textureAtlas;
    public static int columnHeight = 4;
    public static int chunkSize = 16;
    public static int worldSize = 5;
    public static Dictionary<string, Chunk> chunks;

    public static string BuildChunkName(Vector3 pos)
    {
        return (int)pos.x + "_" + (int)pos.y + "_" + (int)pos.z;
    }

    IEnumerator BuildChunkColumn()
    {
        for (int i = 0; i < columnHeight; i++)
        {
            Vector3 chunkPosition = new Vector3(this.transform.position.x,
                i * chunkSize,
                this.transform.position.z);

            Chunk chunk = new Chunk(chunkPosition, textureAtlas);
            chunk.chunk.transform.parent = this.transform;
            chunks.Add(chunk.chunk.name, chunk);
        }

        foreach (KeyValuePair<string, Chunk> c in chunks)
        {
            c.Value.DrawChunk();
        }
            yield return null;
    }

    IEnumerator BuildWorld()
    {
        for (int z = 0; z < worldSize; z++)
        {
            for (int x = 0; x < worldSize; x++)
            {
                for (int y = 0; y < columnHeight; y++)
                {
                    Vector3 chunkPosition = new Vector3(x * chunkSize, y * chunkSize, z * chunkSize);
                    Chunk c = new Chunk(chunkPosition, textureAtlas);
                    c.chunk.transform.parent = this.transform;
                    chunks.Add(c.chunk.name, c);
                }
            }
        }

        foreach (KeyValuePair<string, Chunk> c in chunks)
        {
            c.Value.DrawChunk();
        }
        yield return null;
    }

    private void Start()
    {
        chunks = new Dictionary<string, Chunk>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        StartCoroutine(BuildWorld());
    }

}
