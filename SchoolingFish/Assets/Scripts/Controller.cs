using UnityEngine;

public class Controller : MonoBehaviour
{
    public float MoveSpeed = 10f;

    internal float default_MoveSpeed = 10f;

    public CharacterController MyController;

    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        default_MoveSpeed = MoveSpeed;
    }

    void Update()
    {
        velocity.x = Input.GetAxis("Horizontal") * MoveSpeed;
        velocity.z = Input.GetAxis("Vertical") * MoveSpeed;
        velocity.y = Input.GetAxis("Height") * MoveSpeed;

        velocity = transform.TransformDirection(velocity);
        MyController.Move(velocity * Time.deltaTime);
    }
}