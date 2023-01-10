using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]// 항상 재생모드로 실행 (편집할 때도 실행되어서 타일 좌표 옮기면 하이어라키 창에 바로 적용)
[RequireComponent(typeof(TextMeshPro))] // 인스펙터 창에서 추가할 수 있는 속성들을 코드로 추가

public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;  // 기본 타일은 하얀색
    [SerializeField] Color blockedColor = Color.gray;   // 막혀있는 타일은 회색
    [SerializeField] Color exploredColor = Color.yellow;    // 탐험 타일은 노란색
    [SerializeField] Color pathdColor = new Color(1f, 0.5f, 0f);  // 경로 타일은 주황색

    TextMeshPro label;  // 각 타일위에 자신의 좌표를 나타내기 위한 텍스트타입 변수
    Vector2Int coordinates = new Vector2Int();   // 좌표값을 담아주기 위해 Vector2Int형으로 변수 선언  (x,y)만 있으면 됨
    GridManager gridManager;  // 변수타입 : 클래스 (해당 클래스에 참조하기 위함)

    Tile waypoint;


    void Awake()
    {
        // GridManager를 참조하기 위한 메소드
        gridManager = FindObjectOfType<GridManager>();

        // text에 연결 시킴 (타일에 좌표값 나오도록 함)
        label = GetComponent<TextMeshPro>();

        // 처음 시작할 때는 text를 보이지 않게함
        label.enabled = false;

        // 시작할때 좌표값을 보여주기 위해 Awake에서 한 번 실행
        DisplayCoordinates();


        waypoint = GetComponentInParent<Tile>();
    }

    void Update()
    {
        // 실행 중이지 않다면 다음을 실행하라
        // 환경조성할때만 아래 코드가 필요하기 때문 (게임실행할 때는 필요X)
        if (!Application.isPlaying)
        {
            DisplayCoordinates(); // 각 타일 텍스트에 좌표값을 넣어주는 메소드
            UpdateObjectName(); // 하이어라키 창에서 이름을 변경하는 메소드
            label.enabled = true;  // 타일 text값을 보여줌
        }

        SetLabelColor(); // 색을 세팅하는 메소드 
        ToggleLabels();  // 키보드의 C키로 텍스트를 활성화/비활성화 하느 메소드
    }

    void SetLabelColor()   // 색을 세팅하는 메소드
    {
        //gridManager 값이 없다면 아래 코드 실행하지 않고 그냥 바로 리턴
        if (gridManager == null) { return; }

        // 임시 노드에 저장 
        // 인자값(좌표)가 grid 안에 있다면 좌표값을 그대로 반환
        // gridManager 클래스의 GetNode 메소드는 인자값이 있다면
        // 반환하고 없다면 아무것도 반환하지 않는다.
        Node node = gridManager.GetNode(coordinates);

        //node 값이 없다면 아래 코드 실행하지 않고 그냥 바로 리턴
        if (node == null) { return; }


        // 좌표의 텍스트 색깔 설정 (x,z) 
        // 걸을 수 있는 타일이 아니라면 검정색(회색)
        if (!node.isWalkable)
        {
            label.color = blockedColor;
        }

        // 해당 타일이 경로 타일이라면 주황색
        else if (node.isPath)
        {
            label.color = pathdColor;
        }

        // 해당 타일이 탐험 타일이라면 노란색
        else if (node.isExplored)
        {
            label.color = exploredColor;
        }

        // 나머지들은 디폴트(하얀색)색
        else
        {
            label.color = defaultColor;
        }

/*        if (waypoint.Isplaceable)
        {
            label.color = defaultColor;
            
        }
        else
        {
            Debug.Log(coordinates + "ddd" + waypoint.Isplaceable);
            label.color = blockedColor;
        }*/
    }

    void ToggleLabels()  // 키보드로 좌표 텍스트를 활성화/비활성화 할 수 있는 메소드
    {
        // 만약 키보드 C키를 눌렀다면
        if (Input.GetKeyDown(KeyCode.C))
        {
            //label 변수는 TextMeshPro 타입으로 선언되어 있음
            // 텍스트를 활성화 시켜라 (텍스트 -> 각 타일위에 적힌 좌표값)
            label.enabled = !label.IsActive();
        }
    }

    void DisplayCoordinates()  // 각 타일 텍스트에 좌표값을 넣어주는 메소드
    {
        if (gridManager == null) { return; }

        // RoundToInt은 반올림 함수
        // vector3 타입과 vector2 타입이 매칭이 안되기 때문에 정수로 변환하여 넣어줌
        // snap으로 나눠주는 이유는 10단위씩 움직이도록 snap을 설정해서 1단위로 변경해주기 위함
        // 유니티 상의 좌표는 (10,10) 이라면 tile 상의 좌표는 (1,1)이 되도록 함
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x/ gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z/ gridManager.UnityGridSize);

        // label 변수는 TextMeshPro 타입으로 선언되어 있음
        // 텍스트(각 타일의 좌표값)는 " x,y " 
        label.text = coordinates.x + "," + coordinates.y;
    }

    void UpdateObjectName()  // 하이어라키 창에서 이름을 변경하는 메소드
    {
        // 하이어라키 창에서의 이름을 좌표값으로 변경 (string 형태로 변환하여 받아옴)
        transform.parent.name = coordinates.ToString();
    }
}
