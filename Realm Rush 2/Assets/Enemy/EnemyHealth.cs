using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    
    [SerializeField] int maxHitPoints = 5; // 적군의 full 체력

    [Tooltip("적이 죽을때 maxHitPoints에 추가하라")]
    [SerializeField] int difficultyRamp = 1;  // 점점 적이 강해짐
    int currentHitPoints = 0;   // 적군의 현재 체력

    // 참조타입 : "Enemy" 클래스 (해당 클래스에 접근하기 위함)
    Enemy enemy;

    // OnEnable은 하이어라키에서 활성화 또는 비활성화 될 때 호출
    // 아래의 ProcessHit 메소드에서 적이 죽으면 비활성화가 되도록 코드 작성됨
    void OnEnable()
    {
        // 적군의 현재 체력을 다시 full 체력으로 돌려놓음.
        currentHitPoints = maxHitPoints;
    }

    void Start()
    {
        // 외부 메소드에 접근
        enemy = GetComponent<Enemy>();
    }

    
    void OnParticleCollision() // 파티클에 맞았을때 발동
    {
        // 파티클(화살)에 적이 맞았다면 메소드 실행
        ProcessHit();  
    }

    void ProcessHit()
    {
        // 적군의 현재 체력을 1씩 감소시킴
        currentHitPoints--;

        // 만약 적군의 체력이 0보다 작아진다면
        if(currentHitPoints <= 0)
        {
            gameObject.SetActive(false);  // 이 스크립트가 부착된 오브젝트를 비활성화 시킴 
            maxHitPoints += difficultyRamp;  // 적군의 full 체력을 설정값만큼 증가시킴 (시간이 흐를수록 난이도 상승 목적)
            enemy.RewardGold(); // Enemy 스크립트의 RewardGold 메소드 실행
        }
    }
}
