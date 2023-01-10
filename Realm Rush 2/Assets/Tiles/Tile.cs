using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerprefab;
    [SerializeField] bool isplaceable;


    // get �޼ҵ�� ������ ��ȯ
    // �б� ���� �޼ҵ尡 �Ǿ����  (���������� set)
    // isplaceable�� ���� ��ȯ
    public bool Isplaceable { get { return isplaceable; } }

    GridManager gridManager; // ����Ÿ�� : Ŭ���� (Ŭ������ �����ϱ� ����) 
    Pathfinder pathfinder;
    Vector2Int coordinates = new Vector2Int();  // ��ǥ���� �ޱ� ���� 2�������� ���� ����

    void Awake()
    {
        // Gridmanager Ŭ������ ����
        gridManager = FindObjectOfType<GridManager>();

        // Pathfinder Ŭ������ ����
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void Start()
    {
        //gridManager�� �� ���� �ƴ϶��
        if(gridManager != null)
        {
            // coordinates�� ��ǥ���� �־��
            // gridManager�� GetCoordinatesFromPosition �޼ҵ�� 3���� ���� ������ 2���� ���� ��ȯ�ϰ� �����
            // (x,y,z) -> (x,z)
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);


            // ���� ��ġ�� �� ���� ���̶�� 
            if (!isplaceable)
            {
                // GridManager�� BlockNode �޼ҵ��� �ߵ��϶�
                // BlockNode�� ��ǥ���� ������ isWalkable�� false�� ��
                gridManager.BlocKNode(coordinates);
            }
        }        
    }

    void OnMouseDown()  // ���콺�� ��������
    {
        // ���� isWa��kable �̸鼭 && ��ΰ� ������ �ʾҴٸ�
        if (gridManager.GetNode(coordinates).isWalkable && !pathfinder.WillBlockPath(coordinates))
        {
            
            // true�� ��ȯ (CreateTower���� �ν��Ͻ�ȭ ����)
            bool isSuccussful = towerprefab.CreateTower(towerprefab, transform.position);


            // isSuccussful�� true��� 
            if (isSuccussful)
            {
                // GridManager�� BlockNode �޼ҵ��� �ߵ��϶�
                // BlockNode�� ��ǥ���� ������ isWalkable�� false�� ��
                gridManager.BlocKNode(coordinates);


                pathfinder.NotifyReceivers(); // // ��� ��˻� �޼ҵ� ȣ��
            }            
        }
    }
}
