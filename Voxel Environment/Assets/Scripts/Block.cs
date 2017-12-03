using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block{

    BlockUtils.BlockType blockType;
    public bool isSolid;
    GameObject parent;
    Vector3 position;
    Material material;
    

    public Block(BlockUtils.BlockType blockType, Vector3 position, GameObject parent, Material material)
    {
        this.blockType = blockType;
        this.position = position;
        this.parent = parent;
        this.material = material;
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

        MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = material;
    }

	// Use this for initialization
	public void Draw () {
        CreateQuad(BlockUtils.BlockSide.BACK);
        CreateQuad(BlockUtils.BlockSide.FRONT);
        CreateQuad(BlockUtils.BlockSide.TOP);
        CreateQuad(BlockUtils.BlockSide.LEFT);
        CreateQuad(BlockUtils.BlockSide.RIGHT);
        CreateQuad(BlockUtils.BlockSide.BOTTOM);
    }
	
}
