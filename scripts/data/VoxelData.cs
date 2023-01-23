using Godot;

public static class VoxelData
{
    public static readonly Vector3[] voxelVertices = new Vector3[8] {
        new Vector3(0f, 0f, 0f), //0
        new Vector3(1f, 0f, 0f), //1
        new Vector3(1f, 1f, 0f), //2
        new Vector3(0f, 1f, 0f), //3
        new Vector3(0f, 0f, 1f), //4
        new Vector3(1f, 0f, 1f), //5
        new Vector3(1f, 1f, 1f), //6
        new Vector3(0f, 1f, 1f)  //7
    };

    public static readonly int[,] voxelTriangles = new int[6, 4] {
        {3, 2, 7, 6}, //Top
        {1, 0, 5, 4}, //Bottom
        {4, 0, 7, 3}, //Left
        {1, 5, 2, 6}, //Right
        {5, 4, 6, 7}, //Front
        {0, 1, 3, 2}  //Back
    };

    public static readonly Vector2[] voxelUVs = new Vector2[4] {
        new Vector2(0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0f),
        new Vector2(0f, 0f)
    };
}