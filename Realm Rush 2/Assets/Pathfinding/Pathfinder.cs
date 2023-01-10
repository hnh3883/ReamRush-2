using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    // ���� 137
    // 138

    [SerializeField] Vector2Int startCoordinates;   // �ν����� â���� ��������
    public Vector2Int StartCoordinates { get { return startCoordinates; } }  // startCoordinates ���� �����ϴ� �޼ҵ�

    [SerializeField] Vector2Int destinationCoordinates;  // �ν����� â���� ��������
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }  // destinationCoordinates ���� �����ϴ� �޼ҵ�


    Node startNode;  // ���۳��
    Node destinationNode;  // �������
    Node currentSearchNode;  // ���� ã�� �ִ� ���

    Queue<Node> frontier = new Queue<Node>(); // ��⿭ (Queue)
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>(); // �̹� ������ ������� �����ϱ� ����

    // �����¿찡 ����ִ� �迭
    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    // ���� Ÿ�� : Ŭ����
    GridManager gridManager;

    // grid �迭
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    void Awake()
    {
        // gridManager Ŭ������ �����ϱ� ���� ����
        gridManager = FindObjectOfType<GridManager>();

        // ���� gridManager�� ������� �ʴٸ�
        if (gridManager != null)
        {
            // gridManager������ Grid �迭�� �־��
            // �׳� grid �׳� �� �޾ƿ��� ��
            grid = gridManager.Grid;

            // ���۳��
            // gridManager Ŭ������ Grid ��ųʸ�
            startNode = grid[startCoordinates];

            // �������
            destinationNode = grid[destinationCoordinates];

        }
    }

    void Start()
    {
        GetNewPath();   // ���۰� ���ÿ� ���Ž��
    }

    public List<Node> GetNewPath()  // ������������ ���Ž��
    {
        return GetNewPath(startCoordinates);  // GetNewPath�� ���ڰ� startCoordinates�־ ������������ ��� Ž��
    }
    public List<Node> GetNewPath(Vector2Int coordinates)  // Ư�� ��ǥ������ ���� ���Ž��  (�߰����� ������ �� ���)
    {
        gridManager.ResetNodes();  // ��� ��带 ����
        BreadthFirstSearch(coordinates);  // BFS �˰��� (�ʺ� �켱 Ž�� �˰���)
        return BuildPath(); // path�� �ּ� ��θ� ��� �޼ҵ�
    }


    // 2��°�� �����ϱ�
    void ExploreNeighbors()  // �ֺ�(�̿����)�� Ž���ϴ� �޼ҵ�
    {
        // �̿������� ����� �迭 ����
        List<Node> neighbors = new List<Node>();

        // directions �迭�� ó������ ������ ��ȯ (�迭�ȿ��� ��,��,��,�찡 �������)
        foreach(Vector2Int direction in directions)
        {
            // ������ǥ�� �ֺ���ǥ�� ������� ����
            // ���� ������ǥ�� (2,2)�̰�, Ž���� ���������� ��ĭ �̵��Ѵٸ� (3,2)�� ��
            // foreach���� ���� �����¿� ���� Ž���ϰ� ��
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction;

            // ���� grid�迭�� Key ���� neighborCoords��� 
            if (grid.ContainsKey(neighborCoords))
            {
                // neighbors�迭�� grid���� ������ �߰�
                neighbors.Add(grid[neighborCoords]);

            }
        }

        // ���� 138
        // �츮�� ã�� �̿����� ��ȯ������
        foreach(Node neighbor in neighbors)
        {
            // �̿��� ��ǥ�� �̹� ������ ��ǥ�� �ƴϰ�, �̿���ǥ�� isWalkable�� true ��� 
            if(!reached.ContainsKey(neighbor.coordinates)&& neighbor.isWalkable)
            {
                //�̿���ǥ�� ����� ��ǥ�� currentSearchNode
                neighbor.connectedTo = currentSearchNode;

                //���� ��忡 �̿���ǥ�� �־���
                reached.Add(neighbor.coordinates, neighbor);

                // Queue�� �̿���� �־���
                frontier.Enqueue(neighbor);
            }
        }
    }


    // 1��°�� �����ϱ�
    void BreadthFirstSearch(Vector2Int coordinates)  // BFS �˰��� (�ʺ� �켱 Ž�� �˰���)
    {
        // ���۳��� �������� �ν�����â���� ���� �� ����

        // ���۳��� �ɾ�ٴ� �� �ְ���
        startNode.isWalkable = true;
        // �������� �ɾ�ٴ� �� �ְ���
        destinationNode.isWalkable = true;

        frontier.Clear(); // Queue�� Ŭ����
        reached.Clear();  // �̹� ������ ��� (������ ���) Ŭ����

        bool isRunning = true; // �������� ���°� ������
        frontier.Enqueue(grid[coordinates]);  // grid ��带 �־���  -> (line 60���� start ��� �־���)
        reached.Add(coordinates, grid[coordinates]);  // �̹� ������ ���� reached �迭�� �־��� 


        // Ž���� ��尡 Ʈ���� �����ְ�, isRunning�� true���
        while (frontier.Count > 0 && isRunning)
        {
            // ���� ã�� ���� Queue ������ ù ��° ���
            // �ϳ��� ��Ī �Ǹ鼭 Queue���� ������ ��
            currentSearchNode = frontier.Dequeue();

            // ���� ã�� ����� Ž����´� true
            currentSearchNode.isExplored = true;
            ExploreNeighbors(); // �̿� Ž�� �޼ҵ�

            // ���� ���� ã�� ����� ��ǥ�� ������ǥ�� ���ٸ�
            if(currentSearchNode.coordinates == destinationCoordinates)
            {
                // �ݺ����� ������
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath() // path�� �ּ� ��θ� ��� �޼ҵ�
    {
        // �� ��� ����
        List<Node> path = new List<Node>();

        // ���� ��带 �������� ����
        Node currentNode = destinationNode;

        // path �迭�� ���� ��� �Է�
        path.Add(currentNode);
        // �����尡 ��ΰ� �´ٰ� ����
        currentNode.isPath = true;


        // �������� ã�����ϱ� �������� connercted �Ǿ� �ִ� �͸� �������� �ٽ� ã�ư��� ��ΰ� ����

        // ����� ��带 ���� ��ȯ
        while(currentNode.connectedTo != null)
        {
            // ���� ���� ���� ���� ����Ǿ� �ִ� ����̴�.
            currentNode = currentNode.connectedTo;

            // ��ο� ���� ��带 �߰�
            path.Add(currentNode);

            // ���� ����� ��θ� true��� ����
            currentNode.isPath = true;
        }
        
        path.Reverse();// ������->�������� ������->���������� ������ġ
        return path;  // path�� ����
    }

    public bool WillBlockPath(Vector2Int coordinates)  // ��ΰ� �����ٸ� -> true���� ����  (Tile ��ũ��Ʈ���� ����)
    {
        // grid�� Ž���� ����� ����
        // ������ ��ǥ�� ��ȿ���� Ȯ��
        // ���� grid ���� ���� ��ǥ���
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;
            
            // ���� ��α��̰� 1 ���϶�� (Ÿ���� ���� ���� ���)
            if(newPath.Count <= 1)
            {
                GetNewPath();  // ��θ� �ٽ� Ž���Ѵ�. 
                return true;  // true���� �����Ѵ�
            }
        }

        return false; // false���� �����Ѵ�
    }


    // ���� 144
    public void NotifyReceivers()  // �ٽ� Ž���� ��η� ���� �̵��ϰԲ� �ϴ� �޼ҵ�(RecalculatePath)�� ȣ�� (Tile ��ũ��Ʈ�� OnMouseDown �޼ҵ忡���� ���)
    {
        // ��ε�ĳ��Ʈ�޼����� ���ӿ�����Ʈ�� ��� ������Ʈ�� ������ �� ����
        // Ŭ������ �����ϱ� ���� ���ο� �ν��Ͻ��� �������� �ʾƵ� �ȴٴ� ������ ������
        // ���꿡 �־ ���̰� ������

        // EnemyMover�� Ŭ������ RecalculatePath �޼ҵ带 ����
        // RecalulaterPath(bool resetPath) ���ڰ��� false�� ���� -> ���۳�尡 �ƴ� ���� ��忡�� �ٽ� ��ΰ˻�
        // SendMessageOptions.DontRequireReceiver ���ŵ� ������ ������ ����ϰ� �ڵ尡 ������ �������� �ʵ��� ��
        BroadcastMessage("RecalculatePath", false ,SendMessageOptions.DontRequireReceiver);
    }
}
