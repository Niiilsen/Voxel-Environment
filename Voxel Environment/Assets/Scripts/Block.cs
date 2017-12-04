using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block{

    BlockUtils.BlockType blockType;
    public bool isSolid;
    Chunk owner;
    GameObject parent;
    Vector3 position;

    public Block(BlockUtils.BlockType blockType, Vector3 position, GameObject parent, Chunk owner)
    {
        this.blockType = blockType;
        this.position = position;
        this.parent = parent;
        this.owner = owner;
        isSolid = BlockUtils.IsBlockTypeSolid(this.blockType);
    }

    void CreateQuad(BlockUtils.BlockSide side)
    {

        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh" + side.ToString();

        Quad quadBase = BlockUtils.ConstructQuad(blockType, side);
        mesh.vertices = quadBase.vertices;
        mesh.normals = quadBase.normals;
        mesh.uv = quadBase.uvs;
        mesh.triangles = quadBase.triangles;

        mesh.RecalculateBounds();


        GameObject quad = new GameObject("Quad");
        quad.transform.position = position;
        quad.transform.parent = parent.transform;

        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
    }

    int ConvertBlockIndexToLocal(int i)
    {
        if (i == -1)
            i = World.chunkSize - 1;
        else if (i == World.chunkSize)
            i = 0;
        return i;
    }

    private bool HasSolidNeighbour(int x, int y, int z)
    {
        Chunk nChunk;
        //Adjust
        if (x < 0 || x >= World.chunkSize ||
            y < 0 || y >= World.chunkSize ||
            z < 0 || z >= World.chunkSize)
        {
            Vector3 neighbourChunkPos = this.parent.transform.position +
                new Vector3((x - (int)position.x) * World.chunkSize,
                            (y - (int)position.y) * World.chunkSize,
                            (z - (int)position.z) * World.chunkSize);
            string nName = World.BuildChunkName(neighbourChunkPos);

            x = ConvertBlockIndexToLocal(x);
            y = ConvertBlockIndexToLocal(y);
            z = ConvertBlockIndexToLocal(z);

            if (World.chunks.TryGetValue(nName, out nChunk))
            {
            }
            else
                return false;
        }
        else {
            nChunk = owner;
        }

        try
        {
            return nChunk.GetBlock(x, y, z).isSolid;
        }
        catch (System.IndexOutOfRangeException ex){}

        return false;
    }

	// Use this for initialization
	public void Draw () {
        if (blockType == BlockUtils.BlockType.AIR)
            return;

        if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z + 1))
        {
            CreateQuad(BlockUtils.BlockSide.FRONT);
        }
        if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z - 1))
        { 
            CreateQuad(BlockUtils.BlockSide.BACK); 
        }
        if (!HasSolidNeighbour((int)position.x, (int)position.y + 1, (int)position.z))
        {
            CreateQuad(BlockUtils.BlockSide.TOP); 
        }
        if (!HasSolidNeighbour((int)position.x, (int)position.y - 1, (int)position.z))
        {
            CreateQuad(BlockUtils.BlockSide.BOTTOM); 
        }
        if (!HasSolidNeighbour((int)position.x - 1, (int)position.y, (int)position.z))
        {
            CreateQuad(BlockUtils.BlockSide.LEFT); 
        }
        if (!HasSolidNeighbour((int)position.x + 1, (int)position.y, (int)position.z))
        {
            CreateQuad(BlockUtils.BlockSide.RIGHT); 
        }
    }
	
}
