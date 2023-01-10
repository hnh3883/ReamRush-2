using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // 강의 134
    //136


    // 인스펙터 창에서 소개하기
    // 좌표값 때문에 vector2Int형 사용
    [SerializeField] Vector2Int gridSize; // 경로탐색 시 범위 설정

    [Tooltip("World Grid Size - Should match UnityEditor snap settings.")]
    [SerializeField] int unityGridSize = 10; // 3차원 -> 2차원 좌표로 변환시 나눠줄 변수
    public int UnityGridSize { get { return unityGridSize; } } // 다른 곳에서도 사용할 수 있도록 public 선언

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();  // Dictionnary 선언 <Key값, Value값>
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } } // 다른 곳에서도 사용할 수 있도록 public 선언

    void Awake()
    {
        CreateGrid();// grid의 범위안에 있는 타일 좌표들을 추가해주는 메소드
    }


    // grid에 있는 해당 좌표들이 존재하는지 파악하고 싶을 때 사용
    // 인자값(좌표)를 넣었을 때, grid안에 값이 있다면 그 값을 반환
    public Node GetNode(Vector2Int coordinates)  // Node를 찾는 메소드
    {
        // grid 딕셔너리에서 Key값이 coordinates 일 때만 
        if (grid.ContainsKey(coordinates))
        {
            // 값을 리턴하라   
            return grid[coordinates];
        }

        // Key 값이 없다면 그냥 null 값을 반환하라
        return null;
    }

    //140
    public void BlocKNode(Vector2Int coordinates) // isPlaceable이 false인 노드를 블로킹 (비활성화된 장소는 아예 가지도 못하게함)
    {
        // 만약 grid 딕셔너리에서 Key값이 coordinates 라면
        if (grid.ContainsKey(coordinates))  
        {
            // 해당 좌표의 isWalkable을 false로 변경해버림
            grid[coordinates].isWalkable = false;
        }
    }

    public void ResetNodes()  // 노드를 리셋한다
    {
        // grid 딕셔너리의 처음과 끝을 순환
        // KeyValuePairsms 검색할 수 있는 키/값의 쌍
        foreach (KeyValuePair<Vector2Int, Node> entry in grid)
        {
            entry.Value.connectedTo = null;  // value가 connectedTo인 것을 전부 null로 변경
            entry.Value.isExplored = false;  // value가 isExplored인 것을 전부 false로 변경
            entry.Value.isPath = false;   // value가 isPath인 것을 전부 false로 변경
        }
    }

    //
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)  // 3차원 좌표값을 2차원 좌표로 변경하는 메소드
    {
        // 좌표값을 받을 변수 생성
        Vector2Int coordinates = new Vector2Int();

        // Mathf.RoundToInt는 반올림 함수
        // x좌표는 x/10 (실제 좌표가 10단위)  (10,0,20) ->> (1,2)
        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);

        // y좌표는 z/10 (실제 좌표가 10단위)
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize);

        // coordinates (좌표값)을 반환
        return coordinates;
    }

    //
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)  // 2차원 좌표값을 3차원 좌표로 변경하는 메소드
    {
        // 3차원 좌표값을 받을 변수 생성
        Vector3 position = new Vector3();

        // 2차원좌표 값에 10배 곱하여 3차원 좌표로 변겅
        // (1,2) ->> (10,0,20)
        position.x = coordinates.x * unityGridSize;
        position.z = coordinates.y * unityGridSize;

        // position (3차원 좌표값)을 반환
        return position;
    }

    void CreateGrid() // grid의 범위안에 있는 타일 좌표들을 추가해주는 메소드
    {
        for(int x = 0; x <gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                // coordinates(좌표변수)에 범위 안의 좌표를 추가 (반복문을 통해서 처음 부터 끝까지 추가)
                Vector2Int coordinates = new Vector2Int(x, y);  

                // grid 딕셔너리(배열)에 하나씩 추가 
                // (Key : 좌표값 , Value : Node클래스의 Node메소드)
                grid.Add(coordinates, new Node(coordinates, true));   //지역변수 //Node메소드는 좌표값과 isWalkable 값을 받아옴
            }
        }
    }
}
