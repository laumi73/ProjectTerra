using Godot;
using System;

public class Chunk : StaticBody
{
	private SurfaceTool surfaceTool = new SurfaceTool();
	private ArrayMesh mesh = null;
	private MeshInstance meshInstance = null;

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
		this.surfaceTool.Commit(this.mesh);
		this.meshInstance.Mesh = this.mesh;
		
		AddChild(meshInstance, false);
		this.meshInstance.CreateTrimeshCollision();

		//Loop through each block position inside a chunk and draw the blocks
		void drawBlocks()
		{
			for (int x = 0; x < ChunkData.DIMENSION.x; x++)
			{
				for (int y = 0; y < ChunkData.DIMENSION.y; y++)
				{
					for (int z = 0; z < ChunkData.DIMENSION.z; z++)
					{
						drawBlockFaces(new Vector3(x, y, z));
					}
				}
			}
		}

		//Draw the 6 faces of a block
		void drawBlockFaces(Vector3 offset)
		{
			for (int i = 0; i < 6; i++)
			{
				Vector3 vertexA = VoxelData.voxelVertices[VoxelData.voxelTriangles[i, 0]] + offset;
				Vector3 vertexB = VoxelData.voxelVertices[VoxelData.voxelTriangles[i, 1]] + offset;
				Vector3 vertexC = VoxelData.voxelVertices[VoxelData.voxelTriangles[i, 2]] + offset;
				Vector3 vertexD = VoxelData.voxelVertices[VoxelData.voxelTriangles[i, 3]] + offset;
				this.surfaceTool.AddTriangleFan(new Vector3[] {vertexA, vertexB, vertexC});
				this.surfaceTool.AddTriangleFan(new Vector3[] {vertexC, vertexB, vertexD});
			}
		}
	}



	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.updateMesh();
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
