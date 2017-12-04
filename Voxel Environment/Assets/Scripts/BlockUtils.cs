using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUtils {

    const int atlasSize = 16;
    const float tileSize = 0.0625f; //     1.0f/atlasSize

    //all possible vertices 
    static Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
    static Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
    static Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
    static Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
    static Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
    static Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
    static Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
    static Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

    public enum Face
    {
        STONE = 241, REFINED_STONE = 242, DIRT = 243, GRASS_SIDE = 244, WOODEN_PLANKS = 245, SLOB = 246,
        PRESSURE_PLATE = 247, BRICK = 248, TNT_SIDE = 249, TNT_TOP = 250, TNT_BOT = 251, SPIDERWEB = 252, FLOWER_RED = 253, FLOWER_YELLOW = 254, SAPLING = 256, GRASS = 99
    }

    public enum BlockType
    {
        STONE, DIRT, GRASS, WOODEN_PLANKS, BRICK, TNT, AIR
    }

    public enum BlockSide { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };

    static float GetXOffsetFromIndex(int index)
    {
        int multiplier = index % atlasSize;
        return tileSize * multiplier;
    }

    static float GetYOffsetFromIndex(int index)
    {
        float multiplier = Mathf.Floor(index / atlasSize);
        return tileSize * multiplier;
    }

    //Generate UV coordinates for texturing
    public static Vector2[] GenerateUVCoordinates(BlockType type, BlockSide side)
    {
        int index = GetIndexOfUV(type, side);
        float xOffset = GetXOffsetFromIndex(index);
        float yOffset = GetYOffsetFromIndex(index);
        Vector2 offset = new Vector2(xOffset, yOffset);

       // Debug.Log("Side: " + side.ToString() + "   Index: " + index + "     Offset: " + offset);

        Vector2[] uvs = new Vector2[4];
        uvs[0] = new Vector2(tileSize, tileSize) + offset; //uv11
        uvs[1] = new Vector2(0, tileSize) + offset;  //uv01
        uvs[2] = new Vector2(0, 0) + offset;  //uv00
        uvs[3] = new Vector2(tileSize, 0) + offset;  //uv10

        return uvs;
    }

    //Return the appropriate texture index for the type of block and its sides
    static int GetIndexOfUV(BlockType type, BlockSide side)
    {
        int index = 0;

        switch (type)
        {
            case BlockType.GRASS:
                if (side == BlockSide.TOP)
                    index = (int)Face.GRASS;
                else if (side == BlockSide.BOTTOM)
                    index = (int)Face.DIRT;
                else
                    index = (int)Face.GRASS_SIDE;
                break;
            case BlockType.DIRT:
                index = (int)Face.DIRT;
                break;
            case BlockType.STONE:
                index = (int)Face.STONE;
                break;
        }
        
        return index - 1;
    }    
    
    //Returns true if the block is solid
    public static bool IsBlockTypeSolid(BlockType blockType)
    {
        if (blockType == BlockType.AIR)
        {
            return false;
        }
        return true;
    }

    public static Quad ConstructQuad(BlockType type, BlockSide side)
    {
        Quad quad = new Quad();
        switch (side)
        {
            case BlockSide.BOTTOM:
                quad.vertices = new Vector3[] { p0, p1, p2, p3 };
                quad.normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                break;
            case BlockSide.TOP:
                quad.vertices = new Vector3[] { p7, p6, p5, p4 };
                quad.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                break;
            case BlockSide.LEFT:
                quad.vertices = new Vector3[] { p7, p4, p0, p3 };
                quad.normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                break;
            case BlockSide.RIGHT:
                quad.vertices = new Vector3[] { p5, p6, p2, p1 };
                quad.normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                break;
            case BlockSide.FRONT:
                quad.vertices = new Vector3[] { p4, p5, p1, p0 };
                quad.normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                break;
            case BlockSide.BACK:
                quad.vertices = new Vector3[] { p6, p7, p3, p2 };
                quad.normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                break;
        }

        quad.triangles = new int[] { 3, 1, 0, 3, 2, 1 };
        quad.uvs = GenerateUVCoordinates(type, side);

        return quad;

    }
}
