using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ν����� â���� �߰��� �� �ִ� �Ӽ����� �ڵ�� �߰�
// EnemyMover ��ũ��Ʈ�� �߰��ص� �ڵ����� Enemy ��ũ��Ʈ�� �߰���.
[RequireComponent(typeof(Enemy))] 

public class EnemyMover : MonoBehaviour
{    
    // ������ �̵� �ӵ��� �����ϰ�, ������ 0~5 ���̷� ����
    [SerializeField] [Range(0f, 5f)]float speed = 1f;   // ������ ���� 116 10�д뿡 ����

    // �̵� ��θ� ����� �迭�� ������
    List<Node> path = new List<Node>();

    // ����Ÿ�� : "Enemy" Ŭ���� (�ش� Ŭ������ �����ϱ� ����)
    Enemy enemy;
    // ����Ÿ�� : "GridManager" Ŭ���� (�ش� Ŭ������ �����ϱ� ����)
    GridManager gridManager;
    // ����Ÿ�� : "Pathfinder" Ŭ���� (�ش� Ŭ������ �����ϱ� ����)
    Pathfinder pathfinder;

    //void start�� �� �� �ۿ� ����ȵǼ� OnEnable �޼ҵ� ���
    // OnEnable�� ���̾��Ű���� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ �� �� ȣ��
    // EnemyHealth Ŭ������ ProcessHit �޼ҵ忡�� ���� ������ ��Ȱ��ȭ�� �ǵ��� �ڵ� �ۼ��� 
    void OnEnable()   
    {
        ReturnToStart();   // start �������� enemy ����(���)
        RecalulaterPath(true);  // ��θ� �ٽ� ����� �޼ҵ�
    }
     void Awake()
    {
        enemy = GetComponent<Enemy>();   // �ܺ� �޼ҵ忡 ����
        gridManager = FindObjectOfType<GridManager>();  // �ܺ� �޼ҵ忡 ����
        pathfinder = FindObjectOfType<Pathfinder>();  // �ܺ� �޼ҵ忡 ����
    }


    void RecalulaterPath(bool resetPath) // �ٽ� Ž���� ��η� ���� �̵��ϰԲ� �ϴ� �޼ҵ�
    {
        // 2���� ���� ����
        // 
        Vector2Int coordinates = new Vector2Int();

        // resetPath��  true���
        if (resetPath)
        {
            // ������ ������ġ�� ����ְ�
            coordinates = pathfinder.StartCoordinates;  
        }
        else  // �ƴ϶��
        {
            // ������ ���� ��ġ�� ����ش�
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();  // ��� �ڷ�ƾ ���߱�
        path.Clear();  //��θ� �ʱ�ȭ �Ѵ�
        path = pathfinder.GetNewPath(coordinates); // path�� ���ο� ��θ� �߰��Ѵ� (coordinates ��ǥ���� �ֺ� Ž��)
        StartCoroutine(FollowPath()); // FollowPath �ڷ�ƾ�� ����

    }

    void ReturnToStart() // ���� ������ġ�� �ǵ����� ���� �޼ҵ�
    {
        // �� ��ũ��Ʈ�� ���� ������Ʈ(��)�� ��ġ�� pathfinder Ŭ������ ����Ǿ� �ִ� StartCoordinates (������ġ)
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }

    void FinishPath() // ���� ������ �������� �� ������ �޼ҵ�
    {
        // ���� ���������� �������� penalty�� �ο�
        enemy.StealGold();

        // ������Ʈ(��)�� ��Ȱ��ȭ
        gameObject.SetActive(false);
    } 
    
    IEnumerator FollowPath()  // �ڿ������� �������� ��Ÿ���� �Լ�
    {
        // path �迭�� ó������ ���������� �ݺ��ϰڴ�. (waypoint�� ���ڰ�)
        for(int i = 1; i < path.Count; i++)
        {
            // ���� 116 7:30��

            // ������ġ�� ���� ������Ʈ(��)�� ��ġ
            Vector3 startPosition = transform.position;

            // ������ġ�� ����� ���� ������ ��ġ
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);

            // �ڿ����� �������� ���� ����
            float travelPercent = 0f;

            // ������ġ�� �ٶ󺸰��� (ȸ���� ��Ÿ���� �ڵ�)
            transform.LookAt(endPosition);

            while(travelPercent < 1f) // �������� 
            {
                // �� �����ӿ� �ɸ� �ð��� ������ (speed ������ �ӵ� ���� ����)
                travelPercent += Time.deltaTime * speed;

                // ����������(Lerp)�� �̿��ؼ� �ڿ������� �������� ǥ����
                // Vector3.Lerp(��ġ1, ��ġ2, 0~1 ���̰�) 
                // travelPercent�� 1�� �Ǵ� ���� ��ġ1���� ��ġ2�� �̵�
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);

                // yield return�� ��� �������� �Ѱ��ִ� ��
                // WaitForEndOfFrame�� �������� ������ ��ٸ� ���� �ٽ� �ڷ�ƾ�� �����ϴ� �ڵ�
                yield return new WaitForEndOfFrame(); 
                // ����113 4:40�� ��
            }

        }

        FinishPath(); // ���� ������ �������� �� ���Ƽ�� ���̰�, ��Ȱ��ȭ�� ���Ѷ�
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
