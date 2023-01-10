using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인스펙터 창에서 추가할 수 있는 속성들을 코드로 추가
// EnemyMover 스크립트만 추가해도 자동으로 Enemy 스크립트가 추가됨.
[RequireComponent(typeof(Enemy))] 

public class EnemyMover : MonoBehaviour
{    
    // 적군의 이동 속도를 선언하고, 범위는 0~5 사이로 설정
    [SerializeField] [Range(0f, 5f)]float speed = 1f;   // 레인지 강의 116 10분대에 나옴

    // 이동 경로를 담아줄 배열을 선언함
    List<Node> path = new List<Node>();

    // 참조타입 : "Enemy" 클래스 (해당 클래스에 접근하기 위함)
    Enemy enemy;
    // 참조타입 : "GridManager" 클래스 (해당 클래스에 접근하기 위함)
    GridManager gridManager;
    // 참조타입 : "Pathfinder" 클래스 (해당 클래스에 접근하기 위함)
    Pathfinder pathfinder;

    //void start는 한 번 밖에 실행안되서 OnEnable 메소드 사용
    // OnEnable은 하이어라키에서 활성화 또는 비활성화 될 때 호출
    // EnemyHealth 클래스의 ProcessHit 메소드에서 적이 죽으면 비활성화가 되도록 코드 작성됨 
    void OnEnable()   
    {
        ReturnToStart();   // start 지점에서 enemy 시작(출발)
        RecalulaterPath(true);  // 경로를 다시 계산할 메소드
    }
     void Awake()
    {
        enemy = GetComponent<Enemy>();   // 외부 메소드에 접근
        gridManager = FindObjectOfType<GridManager>();  // 외부 메소드에 접근
        pathfinder = FindObjectOfType<Pathfinder>();  // 외부 메소드에 접근
    }


    void RecalulaterPath(bool resetPath) // 다시 탐색한 경로로 적을 이동하게끔 하는 메소드
    {
        // 2차원 변수 생성
        // 
        Vector2Int coordinates = new Vector2Int();

        // resetPath이  true라면
        if (resetPath)
        {
            // 변수에 시작위치를 담아주고
            coordinates = pathfinder.StartCoordinates;  
        }
        else  // 아니라면
        {
            // 변수에 현재 위치를 담아준다
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();  // 모든 코루틴 멈추기
        path.Clear();  //경로를 초기화 한다
        path = pathfinder.GetNewPath(coordinates); // path에 새로운 경로를 추가한다 (coordinates 좌표부터 주변 탐색)
        StartCoroutine(FollowPath()); // FollowPath 코루틴을 실행

    }

    void ReturnToStart() // 적을 시작위치로 되돌리기 위한 메소드
    {
        // 이 스크립트가 붙은 오브젝트(적)의 위치는 pathfinder 클래스에 선언되어 있는 StartCoordinates (시작위치)
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }

    void FinishPath() // 적이 끝까지 도착했을 때 실행할 메소드
    {
        // 적을 도착지까지 못막으면 penalty를 부여
        enemy.StealGold();

        // 오브젝트(적)를 비활성화
        gameObject.SetActive(false);
    } 
    
    IEnumerator FollowPath()  // 자연스러운 움직임을 나타내는 함수
    {
        // path 배열을 처음부터 마지막까지 반복하겠다. (waypoint는 인자값)
        for(int i = 1; i < path.Count; i++)
        {
            // 강의 116 7:30초

            // 시작위치는 현재 오브젝트(적)의 위치
            Vector3 startPosition = transform.position;

            // 종료위치는 경로의 제일 마지막 위치
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);

            // 자연스런 움직임을 위한 변수
            float travelPercent = 0f;

            // 종료위치를 바라보게함 (회전을 나타내는 코드)
            transform.LookAt(endPosition);

            while(travelPercent < 1f) // 종료지점 
            {
                // 한 프레임에 걸린 시간을 더해줌 (speed 변수로 속도 조절 가능)
                travelPercent += Time.deltaTime * speed;

                // 선형보간법(Lerp)을 이용해서 자연스러운 움직임을 표현함
                // Vector3.Lerp(위치1, 위치2, 0~1 사이값) 
                // travelPercent가 1이 되는 순간 위치1에서 위치2로 이동
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);

                // yield return은 잠시 통제권을 넘겨주는 것
                // WaitForEndOfFrame은 프레임의 끝까지 기다린 다음 다시 코루틴을 시작하는 코드
                yield return new WaitForEndOfFrame(); 
                // 강의113 4:40초 대
            }

        }

        FinishPath(); // 적이 끝까지 도착했을 때 페널티를 먹이고, 비활성화를 시켜라
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            speed += 1;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            speed -= 1;
        }
    }
}
