using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  // ��ũ��Ʈ ����ȭ
public class Node
{
    // ���� 134
    public Vector2Int coordinates;  // ��ǥ
    public bool isWalkable;  // ���� �ɾ �� �ִ� Ÿ������ �Ǻ���
    public bool isExplored;  // Ž���� ���
    public bool isPath;  // ��� ���
    public Node connectedTo; // ����Ǿ� �ִ��� �Ǵ�


    // ������ (�������� �̸��� Ŭ�����̸��� ����)
    // ��ȯ���� ����, �Ű������� ����
    // ��ü�� ������ �� ȣ�� �Ǵ� �޼ҵ�
    public Node(Vector2Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;  // �ٸ� ������ �޾ƿ� ���ڰ��� ���⿡ �����
        this.isWalkable = isWalkable;    // �ٸ� ������ �޾ƿ� ���ڰ��� ���⿡ �����
    }
}
