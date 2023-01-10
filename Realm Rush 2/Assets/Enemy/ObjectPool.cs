using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // 복사할 프리팹들을 담아줄 게임오브젝트 타입 변수 선언
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] [Range(0, 50)] int poolSize = 5;  // 풀링 크기 설정 (복사할 오브젝트 수)    범위 : 0~50
    [SerializeField] [Range(0.1f,30f)] float spawnTimer = 1f;  // 풀링 간격설정 (1초에 하나씩 풀링)  범위 :0.1~ 30

    // 배열 선언
    GameObject[] pool;

    void Awake()
    {
        // start 구문보다 빨리 시작
        PopulatePool();  // 풀링하는 메소드 (처음 시작할 때 인스턴스화만 해줌)
    }

    void Start()
    {
        // 비활성화된 하이어라키들을 1초간격으로 활성화 시키는 코루틴 함수
        StartCoroutine(SpawnEnemy());
    }

    void PopulatePool() // 풀링하는 메소드
    {
        // 풀의 크기는 poolSize로 지정
        pool = new GameObject[poolSize];

        // 풀 배열의 길이만큼 반복
        for (int i = 0; i < pool.Length; i++)
        {
            // i번째 pool을 인스턴스화 해라
            pool[i] = Instantiate(enemyPrefab, transform);

            // 인스턴스만 시켜놓고 비활성화 상태로 둠
            pool[i].SetActive(false);
        }
    }

    void EnableObjectInPool()
    {
        // 풀 배열의 길이만큼 반복
        for(int i = 0; i < pool.Length; i++)
        {
            // 만약 i번째 pool이 비활성화 되어있다면
            if(pool[i].activeInHierarchy == false)
            {
                // i번째 pool을 활성화 시켜라
                // 끝까지 도착하면 EnemyMover 스크립트에서 다시 비활성화 시킴
                pool[i].SetActive(true);
                return;
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        // 코루틴을 반복하기 위함
        while (true)
        {
            // 비활성화된 하이어라키들을 1초간격으로 활성화
            EnableObjectInPool();

            // yield return은 잠시 통제권을 넘겨주는 것
            // WaitForSeconds()는 인자값만큼 시간이 지나면 다시 코루틴을 시작하는 코드 
            yield return new WaitForSeconds(spawnTimer);
        }
    }

}
