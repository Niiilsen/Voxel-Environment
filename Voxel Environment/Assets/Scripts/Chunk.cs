using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public Material blockMaterial;
    public Block[,,] chunkData;
    public GameObject chunk;

    public enum ChunkStatus { DRAW, DONE, KEEP };
    public ChunkStatus status;

    void BuildChunk()
    {
        chunkData = new Block[World.chunkSize, World.chunkSize, World.chunkSize];

        //create blocks
        for (int z = 0; z < World.chunkSize; z++)
        {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    int worldX = (int)(x + chunk.transform.position.x);
                    int worldY = (int)(y + chunk.transform.position.y);
                    int worldZ = (int)(z + chunk.transform.position.z);

                    if(worldY == 0)
                        chunkData[x, y, z] = new Block(BlockUtils.BlockType.BEDROCK, pos,
                                        chunk.gameObject, this);
                    else if (Utils.fBM3D(worldX, worldY, worldZ, 0.1f, 3) < 0.49f && worldY < 3)
                        chunkData[x, y, z] = new Block(BlockUtils.BlockType.BEDROCK, pos,
                                        chunk.gameObject, this);
                    else if (Utils.fBM3D(worldX, worldY, worldZ, 0.1f, 3) < 0.42f)
                        chunkData[x, y, z] = new Block(BlockUtils.BlockType.AIR, pos,
                                        chunk.gameObject, this);
                    else if (worldY <= Utils.GenerateStoneHeight(worldX, worldZ))
                    {
                        if (Utils.fBM3D(worldX, worldY, worldZ, 0.1f, 2) < 0.39f && worldY < 15)
                            chunkData[x, y, z] = new Block(BlockUtils.BlockType.DIAMOND, pos,
                                            chunk.gameObject, this);
                        else if (Utils.fBM3D(worldX, worldY, worldZ, 0.15f, 3) < 0.39f)
                            chunkData[x, y, z] = new Block(BlockUtils.BlockType.COAL, pos,
                                            chunk.gameObject, this);
                        else
                        chunkData[x, y, z] = new Block(BlockUtils.BlockType.STONE, pos,
                                        chunk.gameObject, this);
                    }
                    else if (worldY < Utils.GenerateHeight(worldX, worldZ))
                        chunkData[x, y, z] = new Block(BlockUtils.BlockType.DIRT, pos,
                                        chunk.gameObject, this);
                    else if (worldY == Utils.GenerateHeight(worldX, worldZ))
                        chunkData[x, y, z] = new Block(BlockUtils.BlockType.GRASS, pos,
                                        chunk.gameObject, this);
                    else
                        chunkData[x, y, z] = new Block(BlockUtils.BlockType.AIR, pos,
                                        chunk.gameObject, this);

                    status = ChunkStatus.DRAW;
                }

            }
        }
    }

    public void DrawChunk()
    {
        //draw blocks
        for (int z = 0; z < World.chunkSize; z++)
        {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    chunkData[x, y, z].Draw();
                }
            }
        }

        CombineQuads();
        MeshCollider collider = chunk.gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        collider.sharedMesh = chunk.transform.GetComponent<MeshFilter>().mesh;
    }

    public Chunk(Vector3 position, Material c)
    {
        chunk = new GameObject(World.BuildChunkName(position));
        chunk.transform.position = position;
        blockMaterial = c;
        BuildChunk();
    }

    public Block GetBlock(int x, int y, int z)
    {
        return chunkData[x, y, z];
    }


    void CombineQuads()
    {
        MeshFilter[] meshFilters = chunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        MeshFilter mf = (MeshFilter)chunk.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();

        mf.mesh.CombineMeshes(combine);

        MeshRenderer renderer = chunk.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = blockMaterial;

        foreach (Transform quad in chunk.transform)
        {
            GameObject.Destroy(quad.gameObject);
        }
    }
}
