using Godot;
using System;

public class PlayerController : Spatial
{
    // Parameters
    private float movementSpeed = 0.5f;
    private float movementTime = 4f;
    private float rotationAmount = 0.02f;
    private float rotationTime = 4f;
    private float zoomAmount = 5f;
    private float maxZoomAmount = 100f;
    private float zoomTime = 1f;

    // Node
    private Camera playerCamera; // Camera node attached to PlayerController


    private Vector3 newPosition;
    private Basis newRotation;
    private Vector3 newZoom;

    private Transform defaultControllerPosition = new Transform()
    {
        origin = new Vector3(10f, 90f, 50f),
        basis = new Basis(Vector3.Left, Mathf.Deg2Rad(45f))
    };

     private Transform defaultCameraTransform = new Transform()
    {
        origin = new Vector3(0f, 0f, 15f),
        basis = new Basis(Vector3.Left, 0f)
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Get other nodes
        this.playerCamera = this.GetNode<Camera>("PlayerCamera");

        // Set initial values
        this.ProcessPriority = Int32.MinValue; // Ensures that camera changes is done last
        
        this.Transform = this.defaultControllerPosition;
        this.playerCamera.Transform = this.defaultCameraTransform;
        
        this.newPosition = this.Translation;
        this.newRotation = this.Transform.basis;
        this.newZoom = this.playerCamera.Translation;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        this.handleInput(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }

    // Modify transform upon keyboard press 
    private void handleInput(float delta)
    {
        if (Input.IsActionPressed("player_move_up")) // Move up
        {
            this.newPosition += (Vector3.Up * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_move_down")) // Move down
        {
            this.newPosition += (Vector3.Down * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_move_forward")) // Move forward
        {
            this.newPosition -= ((this.Transform.basis.z * new Vector3(1f, 0f, 1f)).Normalized() * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_move_backward")) // Move backward
        {
            this.newPosition += ((this.Transform.basis.z * new Vector3(1f, 0f, 1f)).Normalized() * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_move_left")) // Move left
        {
            this.newPosition -= (this.Transform.basis.x * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_move_right")) // Mvoe right
        {
            this.newPosition += (this.Transform.basis.x * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_rotate_left")) // Rotate left
        {
            this.newRotation = this.newRotation.Rotated(Vector3.Up, rotationAmount);
        }
        if (Input.IsActionPressed("player_rotate_right")) // Rotate right
        {
            this.newRotation = this.newRotation.Rotated(Vector3.Up, -rotationAmount);
        }
        if (Input.IsActionPressed("player_zoom_in")) // Zoom in
        {
            this.newZoom -= (new Vector3(0f, 0f, this.zoomAmount));
            if (this.newZoom.z < 0) {
                this.newZoom = Vector3.Zero;
            }
        }
        if (Input.IsActionPressed("player_zoom_out")) // Zoom out
        {
            this.newZoom += (new Vector3(0f, 0f, this.zoomAmount));
            if (this.newZoom.z > this.maxZoomAmount) {
                this.newZoom = new Vector3(0f, 0f, this.maxZoomAmount);
            }
        }

        // Interpolations
        this.playerCamera.Translation = this.playerCamera.Translation.LinearInterpolate(this.newZoom, delta * zoomTime);
        this.Translation = this.Translation.LinearInterpolate(newPosition, delta * this.movementTime);
        Transform orthonormalizedTransform = this.Transform.Orthonormalized();

        this.Transform = new Transform
        {
            origin = orthonormalizedTransform.origin,
            basis = new Basis(orthonormalizedTransform.basis.Quat().Slerp(this.newRotation.Orthonormalized().Quat(), delta * this.rotationTime))
        };
    }
}
