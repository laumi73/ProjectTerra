using Godot;

public static class VoxelData {
    public static readonly Vector3[] voxelVertices = new Vector3[8] {
        new Vector3(0f, 0f, 0f), //0
        new Vector3(1f, 0f, 0f), //1
        new Vector3(1f, 1f, 0f), //2
        new Vector3(0f, 0f, 1f), //3
        new Vector3(0f, 0f, 1f), //4
        new Vector3(1f, 0f, 1f), //5
        new Vector3(1f, 1f, 1f), //6
        new Vector3(0f, 1f, 1f)  //7
    };

    public static readonly int[,] voxelTriangles = new int[6, 4] {
        {3, 7, 2, 6}, //Top
        {5, 6, 4, 7}, //Bottom
        {4, 7, 0, 3}, //Left
        {1, 2, 5, 6}, //Right
        {5, 6, 4, 7}, //Front
        {0, 3, 1, 2}  //Back
    };
}