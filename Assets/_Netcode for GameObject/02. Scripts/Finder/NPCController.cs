using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;

    public float wanderRadius = 50f;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        anim.SetFloat("MotionSpeed", 1); // 애니메이션 재생 속도 <- 사용자 입력으로 동작하는 상태가 아니라서 수동으로 입력

        StartCoroutine(MovingRoutine());
    }

    IEnumerator MovingRoutine()
    {
        while (true)
        {
            SetRandomDestination();
            int randomMove = Random.Range(0, 2); // 0 : Walk, 1 : Run
            agent.speed = randomMove == 0 ? 2f : 6f; // 이동 속도 적용
            
            anim.SetFloat("Speed", agent.speed); // 애니메이션 적용
            yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

            anim.SetFloat("Speed", 0f);
            float waitTime = Random.Range(3f, 5f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SetRandomDestination()
    {
        var randomDir = Random.insideUnitSphere * wanderRadius;
        randomDir += transform.position; // 현재 위치에서 랜덤 방향 설정

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, wanderRadius, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }
}