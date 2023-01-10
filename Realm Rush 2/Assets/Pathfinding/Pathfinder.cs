using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    // 강의 137
    // 138

    [SerializeField] Vector2Int startCoordinates;   // 인스펙터 창에서 설정해줌
    public Vector2Int StartCoordinates { get { return startCoordinates; } }  // startCoordinates 값을 리턴하는 메소드

    [SerializeField] Vector2Int destinationCoordinates;  // 인스펙터 창에서 설정해줌
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }  // destinationCoordinates 값을 리턴하는 메소드


    Node startNode;  // 시작노드
    Node destinationNode;  // 도착노드
    Node currentSearchNode;  // 현재 찾고 있는 노드

    Queue<Node> frontier = new Queue<Node>(); // 대기열 (Queue)
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>(); // 이미 도달한 노드인지 구별하기 위함

    // 상하좌우가 들어있는 배열
    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    // 변수 타입 : 클래스
    GridManager gridManager;

    // grid 배열
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    void Awake()
    {
        // gridManager 클래스에 접근하기 위한 변수
        gridManager = FindObjectOfType<GridManager>();

        // 만약 gridManager가 비어있지 않다면
        if (gridManager != null)
        {
            // gridManager에서의 Grid 배열을 넣어라
            // 그냥 grid 그냥 값 받아오는 거
            grid = gridManager.Grid;

            // 시작노드
            // gridManager 클래스의 Grid 딕셔너리
            startNode = grid[startCoordinates];

            // 도착노드
            destinationNode = grid[destinationCoordinates];

        }
    }

    void Start()
    {
        GetNewPath();   // 시작과 동시에 경로탐색
    }

    public List<Node> GetNewPath()  // 시작지점부터 경로탐색
    {
        return GetNewPath(startCoordinates);  // GetNewPath에 인자값 startCoordinates넣어서 시작지점부터 경로 탐색
    }
    public List<Node> GetNewPath(Vector2Int coordinates)  // 특정 좌표값에서 부터 경로탐색  (중간에서 막혔을 때 사용)
    {
        gridManager.ResetNodes();  // 모든 노드를 리셋
        BreadthFirstSearch(coordinates);  // BFS 알고리즘 (너비 우선 탐색 알고리즘)
        return BuildPath(); // path에 최소 경로를 담는 메소드
    }


    // 2번째로 설명하기
    void ExploreNeighbors()  // 주변(이웃노드)을 탐색하는 메소드
    {
        // 이웃노드들을 담아줄 배열 생성
        List<Node> neighbors = new List<Node>();

        // directions 배열을 처음부터 끝까지 순환 (배열안에는 상,하,좌,우가 들어있음)
        foreach(Vector2Int direction in directions)
        {
            // 현재좌표의 주변좌표를 담기위한 변수
            // 만약 현재좌표가 (2,2)이고, 탐색을 오른쪽으로 한칸 이동한다면 (3,2)가 됨
            // foreach문을 통해 상하좌우 전부 탐색하게 됨
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction;

            // 만약 grid배열의 Key 값이 neighborCoords라면 
            if (grid.ContainsKey(neighborCoords))
            {
                // neighbors배열에 grid안의 노드들을 추가
                neighbors.Add(grid[neighborCoords]);

            }
        }

        // 강의 138
        // 우리가 찾은 이웃들을 순환시켜줌
        foreach(Node neighbor in neighbors)
        {
            // 이웃의 좌표가 이미 도달한 좌표가 아니고, 이웃좌표가 isWalkable이 true 라면 
            if(!reached.ContainsKey(neighbor.coordinates)&& neighbor.isWalkable)
            {
                //이웃좌표와 연결된 좌표는 currentSearchNode
                neighbor.connectedTo = currentSearchNode;

                //도달 노드에 이웃좌표를 넣어줌
                reached.Add(neighbor.coordinates, neighbor);

                // Queue에 이웃노드 넣어줌
                frontier.Enqueue(neighbor);
            }
        }
    }


    // 1번째로 설명하기
    void BreadthFirstSearch(Vector2Int coordinates)  // BFS 알고리즘 (너비 우선 탐색 알고리즘)
    {
        // 시작노드와 도착노드는 인스펙터창에서 정할 수 있음

        // 시작노드는 걸어다닐 수 있게함
        startNode.isWalkable = true;
        // 도착노드는 걸어다닐 수 있게함
        destinationNode.isWalkable = true;

        frontier.Clear(); // Queue를 클리어
        reached.Clear();  // 이미 도달한 노드 (지나온 노드) 클리어

        bool isRunning = true; // 루프에서 도는거 막아줌
        frontier.Enqueue(grid[coordinates]);  // grid 노드를 넣어줌  -> (line 60에서 start 노드 넣어줌)
        reached.Add(coordinates, grid[coordinates]);  // 이미 도달한 노드는 reached 배열에 넣어줌 


        // 탐험할 노드가 트리에 남아있고, isRunning이 true라면
        while (frontier.Count > 0 && isRunning)
        {
            // 현재 찾을 노드는 Queue 에서의 첫 번째 노드
            // 하나씩 매칭 되면서 Queue에서 빠지게 됨
            currentSearchNode = frontier.Dequeue();

            // 현재 찾을 노드의 탐험상태는 true
            currentSearchNode.isExplored = true;
            ExploreNeighbors(); // 이웃 탐색 메소드

            // 만약 현재 찾을 노드의 좌표가 도착좌표와 같다면
            if(currentSearchNode.coordinates == destinationCoordinates)
            {
                // 반복문을 끝내라
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath() // path에 최소 경로를 담는 메소드
    {
        // 빈 노드 생성
        List<Node> path = new List<Node>();

        // 현재 노드를 도착노드로 설정
        Node currentNode = destinationNode;

        // path 배열에 현재 노드 입력
        path.Add(currentNode);
        // 현재노드가 경로가 맞다고 설정
        currentNode.isPath = true;


        // 목적지를 찾았으니까 목적지와 connercted 되어 있는 것만 역순으로 다시 찾아가면 경로가 나옴

        // 연결된 노드를 전부 순환
        while(currentNode.connectedTo != null)
        {
            // 현재 노드는 현재 노드와 연결되어 있는 노드이다.
            currentNode = currentNode.connectedTo;

            // 경로에 현재 노드를 추가
            path.Add(currentNode);

            // 현재 노드의 경로를 true라고 설정
            currentNode.isPath = true;
        }
        
        path.Reverse();// 도착점->시작점을 시작점->도착점으로 역순배치
        return path;  // path를 리턴
    }

    public bool WillBlockPath(Vector2Int coordinates)  // 경로가 막혔다면 -> true값을 리턴  (Tile 스크립트에서 실행)
    {
        // grid는 탐색할 경로의 범위
        // 지나갈 좌표가 유효한지 확인
        // 만약 grid 범위 안의 좌표라면
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;
            
            // 만약 경로길이가 1 이하라면 (타워가 길을 막을 경우)
            if(newPath.Count <= 1)
            {
                GetNewPath();  // 경로를 다시 탐색한다. 
                return true;  // true값을 리턴한다
            }
        }

        return false; // false값을 리턴한다
    }


    // 강의 144
    public void NotifyReceivers()  // 다시 탐색한 경로로 적을 이동하게끔 하는 메소드(RecalculatePath)를 호출 (Tile 스크립트의 OnMouseDown 메소드에서도 사용)
    {
        // 브로드캐스트메세지란 게임오브젝트의 모든 컴포넌트에 접근할 수 있음
        // 클래스에 접근하기 위해 새로운 인스턴스를 생성하지 않아도 된다는 장점이 있지만
        // 연산에 있어서 무겁고 느리다

        // EnemyMover의 클래스의 RecalculatePath 메소드를 실행
        // RecalulaterPath(bool resetPath) 인자값에 false를 넣음 -> 시작노드가 아닌 현재 노드에서 다시 경로검색
        // SendMessageOptions.DontRequireReceiver 수신될 정보가 없음을 허용하고 코드가 오류를 보고하지 않도록 함
        BroadcastMessage("RecalculatePath", false ,SendMessageOptions.DontRequireReceiver);
    }
}
