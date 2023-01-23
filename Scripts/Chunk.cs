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
    bool[,,] blockMap;


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
        this.blockMap = new bool[ChunkData.ChunkWidth, ChunkData.ChunkHeight, ChunkData.ChunkWidth];

        // Functions
        this.updateMesh();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    private void updateMesh()
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
        addBlocks();
        GD.Print(getBlockDataByPosition(new Vector3(5, 5, 5)));
        drawBlocks();

        this.surfaceTool.GenerateNormals(false);
        this.surfaceTool.SetMaterial(this.testMaterial);
        this.surfaceTool.Commit(this.mesh);
        this.meshInstance.Mesh = this.mesh;

        this.AddChild(meshInstance, false);
        this.meshInstance.CreateTrimeshCollision();

        //Loop through each block position inside a chunk and add assign the block at that position to blockMap
        void addBlocks()
        {
            for (int y = 0; y < ChunkData.ChunkHeight; y++)
            {
                for (int x = 0; x < ChunkData.ChunkWidth; x++)
                {
                    for (int z = 0; z < ChunkData.ChunkWidth; z++)
                    {
                        // Add the block to blockMap
                        this.blockMap[x, y, z] = true;
                    }
                }
            }
        }


        //Loop through each block position inside a chunk and draw the blocks
        void drawBlocks()
        {
            for (int y = 0; y < ChunkData.ChunkHeight; y++)
            {
                for (int x = 0; x < ChunkData.ChunkWidth; x++)
                {
                    for (int z = 0; z < ChunkData.ChunkWidth; z++)
                    {
                        // Draw the block
                        drawBlockFaces(new Vector3(x, y, z));
                    }
                }
            }
        }

        //Draw the 6 faces of a block
        void drawBlockFaces(Vector3 blockPosition)
        {
            Vector2[] uv1 = new Vector2[] { BlockData.blockUVs[0], BlockData.blockUVs[1], BlockData.blockUVs[3] };
            Vector2[] uv2 = new Vector2[] { BlockData.blockUVs[3], BlockData.blockUVs[1], BlockData.blockUVs[2] };
            for (int i = 0; i < 6; i++)
            {
                if (!this.getBlockDataByPosition(blockPosition + BlockData.faceOffset[i]))
                {
                    Vector3 vertexA = BlockData.blockVertices[BlockData.blockTriangles[i, 0]] + blockPosition;
                    Vector3 vertexB = BlockData.blockVertices[BlockData.blockTriangles[i, 1]] + blockPosition;
                    Vector3 vertexC = BlockData.blockVertices[BlockData.blockTriangles[i, 2]] + blockPosition;
                    Vector3 vertexD = BlockData.blockVertices[BlockData.blockTriangles[i, 3]] + blockPosition;

                    this.surfaceTool.AddTriangleFan(new Vector3[] { vertexA, vertexB, vertexC }, uv1);
                    this.surfaceTool.AddTriangleFan(new Vector3[] { vertexC, vertexB, vertexD }, uv2);
                }
            }
        }
    }

    private bool getBlockDataByPosition(Vector3 position)
    {
        //Check if it's out of bound
        if (!isOutOfBound(position))
        {
            return blockMap[Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z)];
        }
        else
        {
            return false;
        }
    }

    private bool isOutOfBound(Vector3 position)
    {
        if (
            (position.x >= 0 && position.x < ChunkData.ChunkWidth) &&
            (position.y >= 0 && position.y < ChunkData.ChunkHeight) &&
            (position.z >= 0 && position.z < ChunkData.ChunkWidth)
        )
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
