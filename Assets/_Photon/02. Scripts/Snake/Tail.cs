using System;
using UnityEngine;

public class Tail : MonoBehaviour
{
    private SnakeController mySnake;
    
    private void OnTriggerEnter(Collider other)
    {
        // 꼬리에 닿은 대상이 Snake일 때   그 대상의 꼬리를 가져오는 기능
        SnakeController otherSnake = other.GetComponent<SnakeController>();
        if (otherSnake != null && mySnake != null)
        {
            if (otherSnake != mySnake)
            {
                otherSnake.enabled = false;
                Debug.Log($"{otherSnake}를 잡았습니다.");
            }
        }
    }

    public void SetSnake(SnakeController snake)
    {
        mySnake = snake;
    }
}