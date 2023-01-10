using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  // 스크립트 직렬화
public class Node
{
    // 강의 134
    public Vector2Int coordinates;  // 좌표
    public bool isWalkable;  // 적이 걸어갈 수 있는 타일인지 판별용
    public bool isExplored;  // 탐험할 노드
    public bool isPath;  // 경로 노드
    public Node connectedTo; // 연결되어 있는지 판단


    // 생성자 (생성자의 이름은 클래스이름과 동일)
    // 반환형은 없고, 매개변수를 가짐
    // 객체를 생성할 때 호출 되는 메소드
    public Node(Vector2Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;  // 다른 곳에서 받아온 인자값을 여기에 담아줌
        this.isWalkable = isWalkable;    // 다른 곳에서 받아온 인자값을 여기에 담아줌
    }
}
