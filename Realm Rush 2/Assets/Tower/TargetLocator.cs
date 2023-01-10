using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon; // 타워의 헤드부분 (인스펙터에서 헤드 부분만 따로 지정해줌)
    [SerializeField] ParticleSystem projectileParticles;  // 파티클 시스템으로 지정 (화살)
    [SerializeField] float range = 15 ;  //화살의 사거리
    Transform target; // 적

    void Update()
    {
        FindClosestTarget(); // 제일 가까운 적을 찾는 메소드
        AimWeapon();    // 타워 방향을 결정하고, 화살까지 발사하는 메소드
    }

    // 강의 124
    void FindClosestTarget() // 가장 가까운 타겟을 찾는 메소드
    {
        // 참조타입 : Enemy 클래스
        // Enemy 클래스에 접근
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        // 가장 가까운 타겟 (일단 비워둠)
        Transform closestTarget = null;

        // 임의로 무한대의 큰 값으로 세팅 
        float maxDistance = Mathf.Infinity;


        // enemies 배열에서 처음부터 끝까지 반복
        foreach(Enemy enemy in enemies)
        {
            // targetDistance = 자신의 위치와 적의 위치의 차이
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            // 만약 타겟거리가 maxDistance보다 작다면
            if (targetDistance < maxDistance)
            {
                // closestTarget에 enemies 배열의 enemy 위치값을 대입한다.
                closestTarget = enemy.transform;
                // maxDistance에 타겟과의 거리를 대입
                maxDistance = targetDistance;

                // 더 가까운 적이 나타날때마다 maxDistance가 더 작아지고
                // 계속해서 반복하면 가장 가까운 적을 판단가능
            }
        }

        // 타겟은 가장 가까운 타겟으로 정한다
        target = closestTarget;
    }

    void AimWeapon() // 타워의 방향을 판단하는 메소드
    {
        // 타겟과의 거리 = 자신의 위치와 타겟의 위치의 차이
        float targetDistance = Vector3.Distance(transform.position, target.position);
        
        //타워의 헤드부분은 타겟을 향한다 (위의 메소드에서 target = closetTarget으로 지정)
        weapon.LookAt(target);

        // 타겟과의 거리가 사정거리보다 작다면
        if(targetDistance < range)
        {
            Attack(true); // 공격을 해라
        }

        // 그렇지 않다면
        else
        {
            Attack(false); // 공격을 하지마라
        }
    }

    void Attack(bool isActive) // 화살(파티클)을 날리는 메소드
    {
        // 모듈접근법 (파티클 모듈에 접근)
        var emissionModule = projectileParticles.emission;

        // 인자값이 true면 파티클이 동작, false면 동작하지 않음
        emissionModule.enabled = isActive;
    }
}
