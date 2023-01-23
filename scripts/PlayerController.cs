using Godot;
using System;

//TODO: set default position to the first block from the sky at origin, add a bit of minimum height OR last saved positions
//TODO: make movement speed dependent on zoom amount
//TODO: add some sort of indicator of where the playerController is centered at when moving
//TODO: switch to using quaternion
//TODO: make camera rotate 180 degree on default; currently it's facing -z direction
//TODO: add more comments to clarify rotation steps

public class PlayerController : Spatial
{
    // Parameters
    private float movementSpeed = 0.5f;
    private float movementTime = 4f;
    private float rotationAmount = 0.02f;
    private float mouseRotationAmount = 0.003f;
    private float rotationTime = 4f;
    private float zoomAmount = 5f;
    private float maxZoomAmount = 100f;
    private float zoomTime = 1f;
    private float maxYRotation = Mathf.Deg2Rad(85f);

    // Node
    private Camera playerCamera; // Camera node attached to PlayerController

    // Fields
    private Vector3 newPosition;
    private Basis newRotation;
    private Vector3 newZoom;

    private Vector2 initialCursorPosition;
    private Vector2 newCursorLocation;

    private Transform defaultControllerPosition = new Transform()
    {
        origin = new Vector3(10f, 90f, 50f),
        basis = new Basis(Vector3.Left, 0f)
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
        this.handleMouseInput(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }

    // Modify newPosition and newRotation upon mouse input
    private void handleMouseInput(float delta)
    {
        if (Input.IsActionJustReleased("player_mouse_zoom_in"))
        { // Zoom in
            this.newZoom -= (new Vector3(0f, 0f, this.zoomAmount));
            if (this.newZoom.z < 0)
            {
                this.newZoom = Vector3.Zero;
            }
        }
        if (Input.IsActionJustReleased("player_mouse_zoom_out"))
        { // Zoom out
            this.newZoom += (new Vector3(0f, 0f, this.zoomAmount));
            if (this.newZoom.z > this.maxZoomAmount)
            {
                this.newZoom = new Vector3(0f, 0f, this.maxZoomAmount);
            }
        }
        if (Input.IsActionJustPressed("player_mouse_rotate"))
        {
            this.initialCursorPosition = this.GetViewport().GetMousePosition();
        }
        if (Input.IsActionPressed("player_mouse_rotate"))
        {
            this.newCursorLocation = this.GetViewport().GetMousePosition();
            float currentYAngle = this.newRotation.GetEuler().x;
            float rotationValue = (this.initialCursorPosition - this.newCursorLocation).y * this.mouseRotationAmount;
            if (rotationValue > 0 && ((currentYAngle + rotationAmount) < this.maxYRotation))
            {
                this.newRotation = this.newRotation.Rotated(this.newRotation.Column0, rotationValue);
                this.newRotation = this.newRotation.Rotated(Vector3.Up, (this.initialCursorPosition - this.newCursorLocation).x * this.mouseRotationAmount);
                this.initialCursorPosition = this.newCursorLocation;
            }
            else if (rotationValue < 0 && (currentYAngle - rotationAmount > -this.maxYRotation))
            {
                this.newRotation = this.newRotation.Rotated(this.newRotation.Column0, rotationValue);
                this.newRotation = this.newRotation.Rotated(Vector3.Up, (this.initialCursorPosition - this.newCursorLocation).x * this.mouseRotationAmount);
                this.initialCursorPosition = this.newCursorLocation;
            }
        }
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
        if (Input.IsActionPressed("player_rotate_up")) // Rotate up
        {
            if (!((this.newRotation.GetEuler().x + this.rotationAmount) > this.maxYRotation))
            {
                this.newRotation = this.newRotation.Rotated(this.newRotation.Column0, this.rotationAmount);
            }
        }
        if (Input.IsActionPressed("player_rotate_down")) // Rotate down
        {
            if (!((this.newRotation.GetEuler().x - this.rotationAmount) < -this.maxYRotation))
            {
                this.newRotation = this.newRotation.Rotated(this.newRotation.Column0, -this.rotationAmount);
            }
        }
        if (Input.IsActionPressed("player_rotate_left")) // Rotate left
        {
            this.newRotation = this.newRotation.Rotated(Vector3.Up, this.rotationAmount);
        }
        if (Input.IsActionPressed("player_rotate_right")) // Rotate right
        {
            this.newRotation = this.newRotation.Rotated(Vector3.Up, -this.rotationAmount);
        }
        if (Input.IsActionPressed("player_zoom_in")) // Zoom in
        {
            this.newZoom -= (new Vector3(0f, 0f, this.zoomAmount));
            if (this.newZoom.z < 0)
            {
                this.newZoom = Vector3.Zero;
            }
        }
        if (Input.IsActionPressed("player_zoom_out")) // Zoom out
        {
            this.newZoom += (new Vector3(0f, 0f, this.zoomAmount));
            if (this.newZoom.z > this.maxZoomAmount)
            {
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
