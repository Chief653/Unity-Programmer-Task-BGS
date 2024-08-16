using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5f;
    public float anticipationDistance = 2f;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if(PlayerController.instance.isDead)
            return;

        Vector3 targetPosition = player.position + offset;

        Vector3 anticipationOffset = new Vector3(
            anticipationDistance * Input.GetAxisRaw("Horizontal"), 
            anticipationDistance * Input.GetAxisRaw("Vertical"),
            0);

        targetPosition += anticipationOffset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
