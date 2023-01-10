using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]// �׻� ������� ���� (������ ���� ����Ǿ Ÿ�� ��ǥ �ű�� ���̾��Ű â�� �ٷ� ����)
[RequireComponent(typeof(TextMeshPro))] // �ν����� â���� �߰��� �� �ִ� �Ӽ����� �ڵ�� �߰�

public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;  // �⺻ Ÿ���� �Ͼ��
    [SerializeField] Color blockedColor = Color.gray;   // �����ִ� Ÿ���� ȸ��
    [SerializeField] Color exploredColor = Color.yellow;    // Ž�� Ÿ���� �����
    [SerializeField] Color pathdColor = new Color(1f, 0.5f, 0f);  // ��� Ÿ���� ��Ȳ��

    TextMeshPro label;  // �� Ÿ������ �ڽ��� ��ǥ�� ��Ÿ���� ���� �ؽ�ƮŸ�� ����
    Vector2Int coordinates = new Vector2Int();   // ��ǥ���� ����ֱ� ���� Vector2Int������ ���� ����  (x,y)�� ������ ��
    GridManager gridManager;  // ����Ÿ�� : Ŭ���� (�ش� Ŭ������ �����ϱ� ����)

    Tile waypoint;


    void Awake()
    {
        // GridManager�� �����ϱ� ���� �޼ҵ�
        gridManager = FindObjectOfType<GridManager>();

        // text�� ���� ��Ŵ (Ÿ�Ͽ� ��ǥ�� �������� ��)
        label = GetComponent<TextMeshPro>();

        // ó�� ������ ���� text�� ������ �ʰ���
        label.enabled = false;

        // �����Ҷ� ��ǥ���� �����ֱ� ���� Awake���� �� �� ����
        DisplayCoordinates();


        waypoint = GetComponentInParent<Tile>();
    }

    void Update()
    {
        // ���� ������ �ʴٸ� ������ �����϶�
        // ȯ�������Ҷ��� �Ʒ� �ڵ尡 �ʿ��ϱ� ���� (���ӽ����� ���� �ʿ�X)
        if (!Application.isPlaying)
        {
            DisplayCoordinates(); // �� Ÿ�� �ؽ�Ʈ�� ��ǥ���� �־��ִ� �޼ҵ�
            UpdateObjectName(); // ���̾��Ű â���� �̸��� �����ϴ� �޼ҵ�
            label.enabled = true;  // Ÿ�� text���� ������
        }

        SetLabelColor(); // ���� �����ϴ� �޼ҵ� 
        ToggleLabels();  // Ű������ CŰ�� �ؽ�Ʈ�� Ȱ��ȭ/��Ȱ��ȭ �ϴ� �޼ҵ�
    }

    void SetLabelColor()   // ���� �����ϴ� �޼ҵ�
    {
        //gridManager ���� ���ٸ� �Ʒ� �ڵ� �������� �ʰ� �׳� �ٷ� ����
        if (gridManager == null) { return; }

        // �ӽ� ��忡 ���� 
        // ���ڰ�(��ǥ)�� grid �ȿ� �ִٸ� ��ǥ���� �״�� ��ȯ
        // gridManager Ŭ������ GetNode �޼ҵ�� ���ڰ��� �ִٸ�
        // ��ȯ�ϰ� ���ٸ� �ƹ��͵� ��ȯ���� �ʴ´�.
        Node node = gridManager.GetNode(coordinates);

        //node ���� ���ٸ� �Ʒ� �ڵ� �������� �ʰ� �׳� �ٷ� ����
        if (node == null) { return; }


        // ��ǥ�� �ؽ�Ʈ ���� ���� (x,z) 
        // ���� �� �ִ� Ÿ���� �ƴ϶�� ������(ȸ��)
        if (!node.isWalkable)
        {
            label.color = blockedColor;
        }

        // �ش� Ÿ���� ��� Ÿ���̶�� ��Ȳ��
        else if (node.isPath)
        {
            label.color = pathdColor;
        }

        // �ش� Ÿ���� Ž�� Ÿ���̶�� �����
        else if (node.isExplored)
        {
            label.color = exploredColor;
        }

        // ���������� ����Ʈ(�Ͼ��)��
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

    void ToggleLabels()  // Ű����� ��ǥ �ؽ�Ʈ�� Ȱ��ȭ/��Ȱ��ȭ �� �� �ִ� �޼ҵ�
    {
        // ���� Ű���� CŰ�� �����ٸ�
        if (Input.GetKeyDown(KeyCode.C))
        {
            //label ������ TextMeshPro Ÿ������ ����Ǿ� ����
            // �ؽ�Ʈ�� Ȱ��ȭ ���Ѷ� (�ؽ�Ʈ -> �� Ÿ������ ���� ��ǥ��)
            label.enabled = !label.IsActive();
        }
    }

    void DisplayCoordinates()  // �� Ÿ�� �ؽ�Ʈ�� ��ǥ���� �־��ִ� �޼ҵ�
    {
        if (gridManager == null) { return; }

        // RoundToInt�� �ݿø� �Լ�
        // vector3 Ÿ�԰� vector2 Ÿ���� ��Ī�� �ȵǱ� ������ ������ ��ȯ�Ͽ� �־���
        // snap���� �����ִ� ������ 10������ �����̵��� snap�� �����ؼ� 1������ �������ֱ� ����
        // ����Ƽ ���� ��ǥ�� (10,10) �̶�� tile ���� ��ǥ�� (1,1)�� �ǵ��� ��
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x/ gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z/ gridManager.UnityGridSize);

        // label ������ TextMeshPro Ÿ������ ����Ǿ� ����
        // �ؽ�Ʈ(�� Ÿ���� ��ǥ��)�� " x,y " 
        label.text = coordinates.x + "," + coordinates.y;
    }

    void UpdateObjectName()  // ���̾��Ű â���� �̸��� �����ϴ� �޼ҵ�
    {
        // ���̾��Ű â������ �̸��� ��ǥ������ ���� (string ���·� ��ȯ�Ͽ� �޾ƿ�)
        transform.parent.name = coordinates.ToString();
    }
}
