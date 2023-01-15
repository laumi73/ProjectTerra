using Godot;
using System;

public class PlayerController : Spatial
{
    // Parameters
    private float movementSpeed = 0.5f;
    private float movementTime = 5f;
    private float rotationAmount = 0.02f;
    private float rotationSpeed = 3f;
    private Vector3 zoomAmount;

    // Objects
    private Transform cameraTransform;


    private Vector3 newPosition;
    private Basis newRotation;
    private Vector3 newZoom;

    public Transform defaultPosition = new Transform() {
            origin = new Vector3(10f, 90f, 50f),
            basis = new Basis(Vector3.Left, Mathf.Deg2Rad(45f))
        };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Transform = defaultPosition;
        this.newPosition = this.Translation;
        this.newRotation = this.Transform.basis;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        this.handleInput(delta);
    }

    // Modify transform upon buttonpress/mouseclick
    private void handleInput(float delta)
    {
        if (Input.IsActionPressed("player_move_up"))
        {
            this.newPosition += (Vector3.Up * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_move_down"))
        {
            this.newPosition += (Vector3.Down * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_move_forward"))
        {
            this.newPosition -= ((Transform.basis.z * new Vector3(1f, 0f, 1f)).Normalized() * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_move_backward"))
        {
            this.newPosition += ((Transform.basis.z * new Vector3(1f, 0f, 1f)).Normalized() * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_move_left"))
        {
            this.newPosition -= (Transform.basis.x * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_move_right"))
        {
            this.newPosition += (Transform.basis.x * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_rotate_left"))
        {
            this.newRotation = this.newRotation.Rotated(Vector3.Up, rotationAmount);
        }
        if (Input.IsActionPressed("player_rotate_right"))
        {
            this.newRotation = this.newRotation.Rotated(Vector3.Up, -rotationAmount);
        }

        Translation = Translation.LinearInterpolate(newPosition, delta * this.movementTime);
        Transform orthonormalizedTransform = this.Transform.Orthonormalized();

        this.Transform = new Transform
        {
            origin = orthonormalizedTransform.origin,
            basis = new Basis(orthonormalizedTransform.basis.Quat().Slerp(newRotation.Orthonormalized().Quat(), delta * rotationSpeed))
        };
    }
}
