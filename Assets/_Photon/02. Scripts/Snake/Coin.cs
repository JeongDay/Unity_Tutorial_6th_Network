using UnityEngine;

public class Coin : MonoBehaviour
{
    public float turnSpeed = 200f;
    
    void Update()
    {
        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
    }
}