using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public float speed = 5;
    public Vector2 offset = new Vector2(10, 5);

    void Update()
    {
        Vector3    targetPosition = Player.inst.transform.position - Player.inst.Forward() * offset.x + Vector3.up * offset.y;
        Quaternion targetRotation = Quaternion.LookRotation(Player.inst.transform.position - transform.position);

        transform.position = Vector3   .Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }
}
