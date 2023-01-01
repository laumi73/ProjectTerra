using Godot;
using System;

public class PlayerController : Spatial
{
    public float movementSpeed = 0.5f;
    public float movementTime = 5f;
    public float rotationAmount = 0.05f;

    public Vector3 newPosition;
    public Basis newRotation;

    public Transform defaultPosition = new Transform() {
            origin = new Vector3(10f, 90f, 50f),
            basis = new Basis(Vector3.Left, Mathf.Deg2Rad(45f))
        };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Transform = defaultPosition;
        this.newPosition = Translation;
        this.newRotation = Transform.basis;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        this.handleInput(delta);
        GD.Print(this.RotationDegrees);
    }

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
            this.newPosition += (Vector3.Forward * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_move_backward"))
        {
            this.newPosition -= (Vector3.Forward * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_move_left"))
        {
            this.newPosition += (Vector3.Left * this.movementSpeed);
        }
        if (Input.IsActionPressed("player_move_right"))
        {
            this.newPosition -= (Vector3.Left * this.movementSpeed);
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
            basis = new Basis(orthonormalizedTransform.basis.Quat().Slerp(newRotation.Orthonormalized().Quat(), delta * 4))
        };
        Transform = Transform.Orthonormalized();
    }
}
