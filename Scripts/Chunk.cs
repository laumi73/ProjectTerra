using Godot;
using System;

public class Chunk : StaticBody
{
    private SurfaceTool surfaceTool = new SurfaceTool();
    private Mesh mesh = null;
    private MeshInstance meshInstance = null;

    public void updateMesh() {
        //Unload mesh if there's an existing one
        if (this.meshInstance != null) {
            this.meshInstance.CallDeferred("queue_free");
            this.meshInstance = null;
        }

        
    }

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
