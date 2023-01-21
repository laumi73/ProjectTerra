using Godot;

public static class VoxelData {
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
        {6, 7, 2, 3}, //Top
        {4, 5, 0, 1}, //Bottom
        {3, 7, 0, 4}, //Left
        {6, 2, 5, 1}, //Right
        {7, 6, 4, 5}, //Front
        {2, 3, 1, 0}  //Back
    };
}