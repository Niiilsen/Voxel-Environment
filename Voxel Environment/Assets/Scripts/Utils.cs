using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

    static int maxHeight = 34;
    static float smooth = 0.01f;
    static int octaves = 4;
    static float persistence = 0.5f;

    public static int GenerateHeight(float x, float z)
    {
        float height = Map(0, maxHeight, 0, 1, fractalBrownianMotion(x * smooth, z * smooth, octaves, persistence));
        return (int)height;
    }

    public static int GenerateStoneHeight(float x, float z)
    {
        float height = Map(0, maxHeight - 10, 0, 1, fractalBrownianMotion(x * smooth*2, z * smooth*2, octaves+1, persistence));
        return (int)height;
    }

    static float Map(float newmin, float newmax, float originmin, float originmax, float value)
    {
        return Mathf.Lerp(newmin, newmax, Mathf.InverseLerp(originmin, originmax, value));
    }

    static float fractalBrownianMotion(float x, float z, int oct, float pers)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;

        for (int i = 0; i < oct; i++)
        {
            total += Mathf.PerlinNoise(x * frequency, z * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= pers;
            frequency *= 2;
        }

        return total / maxValue;
    }



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
