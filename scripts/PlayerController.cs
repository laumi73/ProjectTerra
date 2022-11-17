using Godot;
using System;

public class PlayerController: Spatial
{
    public float movementSpeed = 0.5f;
    public float movementTime = 5f;

    public Vector3 newPosition;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.newPosition = Translation;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        this.handleInput(delta);
    }

    private void handleInput(float delta) {
        if (Input.IsActionPressed("camera_move_up"))
        {
            this.newPosition -= (Transform.basis.z * this.movementSpeed);
        }
        if (Input.IsActionPressed("camera_move_down"))
        {
            this.newPosition += (Transform.basis.z * this.movementSpeed);
        }
        if (Input.IsActionPressed("camera_move_left"))
        {
            this.newPosition -= (Transform.basis.x * this.movementSpeed);
        }
        if (Input.IsActionPressed("camera_move_right"))
        {
            this.newPosition += (Transform.basis.x * this.movementSpeed);
        }

        Translation = Translation.LinearInterpolate(newPosition, delta * this.movementTime);
    }
}
