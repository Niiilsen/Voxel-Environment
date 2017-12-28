using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World : MonoBehaviour
{


    public GameObject player;
    public Material textureAtlas;

    public static int columnHeight = 6;
    public static int chunkSize = 16;
    public static int worldSize = 4;
    public static int radius = 4;
   
    public static Dictionary<string, Chunk> chunks;

    bool firstbuild = true;
    bool building = false;

    public Slider loadingAmount;
    public Camera cam;
    public Button playButton;

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
        building = true;
        int posX = (int)Mathf.Floor(player.transform.position.x / chunkSize);
        int posZ = (int)Mathf.Floor(player.transform.position.z / chunkSize);

        float totalChunks = (Mathf.Pow(radius * 2 + 1, 2) * columnHeight) * 2;
        int processCount = 0;

        for (int z = -radius; z <= radius; z++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = 0; y < columnHeight; y++)
                {
                    Vector3 chunkPosition = new Vector3((x + posX) * chunkSize, y * chunkSize, (z + posZ) * chunkSize);

                    Chunk c;
                    string name = BuildChunkName(chunkPosition);
                    if (chunks.TryGetValue(name, out c))
                    {
                        c.status = Chunk.ChunkStatus.KEEP;
                        break;
                    }
                    else
                    {
                        c = new Chunk(chunkPosition, textureAtlas);
                        c.chunk.transform.parent = this.transform;
                        chunks.Add(c.chunk.name, c);
                    }

                    if (firstbuild)
                    {
                        processCount++;
                        loadingAmount.value = processCount / totalChunks * 100;
                    }

                    yield return null;
                }
            }
        }

        foreach (KeyValuePair<string, Chunk> c in chunks)
        {
            if (c.Value.status == Chunk.ChunkStatus.DRAW)
            {
                c.Value.DrawChunk();
                c.Value.status = Chunk.ChunkStatus.KEEP;
            }

            c.Value.status = Chunk.ChunkStatus.DONE;
            if (firstbuild)
            {
                processCount++;
                loadingAmount.value = processCount / totalChunks * 100;
            }


            yield return null;
        }

        if (firstbuild)
        {
            player.SetActive(true);
            loadingAmount.gameObject.SetActive(false);
            cam.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            firstbuild = false;
        }

        building = false;

    }

    public void StartBuild()
    {
        StartCoroutine(BuildWorld());
    }


    private void Start()
    {
        player.SetActive(false);
        Utils.seed = Random.Range(0.0f, 999999.0f);
        chunks = new Dictionary<string, Chunk>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (!building && !firstbuild)
        {
            StartCoroutine(BuildWorld());
        }
    }

}
