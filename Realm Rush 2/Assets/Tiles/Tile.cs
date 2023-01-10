using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerprefab;
    [SerializeField] bool isplaceable;


    // get 메소드는 변수만 반환
    // 읽기 전용 메소드가 되어버림  (쓰기전용은 set)
    // isplaceable의 값을 반환
    public bool Isplaceable { get { return isplaceable; } }

    GridManager gridManager; // 변수타입 : 클래스 (클래스에 접근하기 위함) 
    Pathfinder pathfinder;
    Vector2Int coordinates = new Vector2Int();  // 좌표값을 받기 위해 2차원으로 변수 선언

    void Awake()
    {
        // Gridmanager 클래스에 접근
        gridManager = FindObjectOfType<GridManager>();

        // Pathfinder 클래스에 접근
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void Start()
    {
        //gridManager가 빈 값이 아니라면
        if(gridManager != null)
        {
            // coordinates에 좌표값을 넣어라
            // gridManager의 GetCoordinatesFromPosition 메소드는 3차원 값을 넣으면 2차원 값을 반환하게 설계됨
            // (x,y,z) -> (x,z)
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);


            // 만약 설치할 수 없는 곳이라면 
            if (!isplaceable)
            {
                // GridManager의 BlockNode 메소드을 발동하라
                // BlockNode에 좌표값을 넣으면 isWalkable이 false가 됨
                gridManager.BlocKNode(coordinates);
            }
        }        
    }

    void OnMouseDown()  // 마우스를 눌렀을때
    {
        // 만약 isWaㅣkable 이면서 && 경로가 막히지 않았다면
        if (gridManager.GetNode(coordinates).isWalkable && !pathfinder.WillBlockPath(coordinates))
        {
            
            // true를 반환 (CreateTower에서 인스턴스화 진행)
            bool isSuccussful = towerprefab.CreateTower(towerprefab, transform.position);


            // isSuccussful가 true라면 
            if (isSuccussful)
            {
                // GridManager의 BlockNode 메소드을 발동하라
                // BlockNode에 좌표값을 넣으면 isWalkable이 false가 됨
                gridManager.BlocKNode(coordinates);


                pathfinder.NotifyReceivers(); // // 경로 재검색 메소드 호출
            }            
        }
    }
}
