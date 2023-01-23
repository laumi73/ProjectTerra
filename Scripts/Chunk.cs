using Godot;
using System;

public class Chunk : StaticBody
{
    // Mesh fields
    private SurfaceTool surfaceTool;
    private ArrayMesh mesh;
    private MeshInstance meshInstance;
    private SpatialMaterial testMaterial;

    // Chunk fields
    bool [,,,] voxelMap;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Variable initializations
        // Mesh related variables
        this.surfaceTool = new SurfaceTool();
        this.mesh = null;
        this.meshInstance = null;
        this.testMaterial = new SpatialMaterial();
        this.testMaterial.AlbedoTexture = GD.Load<Texture>("res://resources/textures/blocks/ReferenceTexture.png");

        // Chunk related variables

        this.updateMesh();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void updateMesh()
    {
        //Unload mesh if there's an existing one
        if (this.meshInstance != null)
        {
            this.meshInstance.CallDeferred("queue_free");
            this.meshInstance = null;
        }

        this.mesh = new ArrayMesh();
        this.meshInstance = new MeshInstance();


        //Draw all of the blocks inside a chunk
        this.surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
        drawBlocks();

        this.surfaceTool.GenerateNormals(false);
        this.surfaceTool.SetMaterial(this.testMaterial);
        this.surfaceTool.Commit(this.mesh);
        this.meshInstance.Mesh = this.mesh;
        
        AddChild(meshInstance, false);
        this.meshInstance.CreateTrimeshCollision();

        //Loop through each block position inside a chunk and draw the blocks
        void drawBlocks()
        {
            for (int y = 0; y < ChunkData.ChunkHeight; y++)
            {
                for (int x = 0; x < ChunkData.ChunkWidth; x++)
                {
                    for (int z = 0; z < ChunkData.ChunkWidth; z++)
                    {
                        drawBlockFaces(new Vector3(x, y, z));
                    }
                }
            }
        }

        //Draw the 6 faces of a block
        void drawBlockFaces(Vector3 offset)
        {
            Vector2[] uv1 = new Vector2[]{VoxelData.voxelUVs[0], VoxelData.voxelUVs[1], VoxelData.voxelUVs[3]};
            Vector2[] uv2 = new Vector2[]{VoxelData.voxelUVs[3], VoxelData.voxelUVs[1], VoxelData.voxelUVs[2]};
            for (int i = 0; i < 6; i++)
            {
                Vector3 vertexA = VoxelData.voxelVertices[VoxelData.voxelTriangles[i, 0]] + offset;
                Vector3 vertexB = VoxelData.voxelVertices[VoxelData.voxelTriangles[i, 1]] + offset;
                Vector3 vertexC = VoxelData.voxelVertices[VoxelData.voxelTriangles[i, 2]] + offset;
                Vector3 vertexD = VoxelData.voxelVertices[VoxelData.voxelTriangles[i, 3]] + offset;

                this.surfaceTool.AddTriangleFan(new Vector3[] {vertexA, vertexB, vertexC}, uv1);
                this.surfaceTool.AddTriangleFan(new Vector3[] {vertexC, vertexB, vertexD}, uv2);
            }
        }
    }
}
