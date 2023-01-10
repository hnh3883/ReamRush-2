using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // ���� 134
    //136


    // �ν����� â���� �Ұ��ϱ�
    // ��ǥ�� ������ vector2Int�� ���
    [SerializeField] Vector2Int gridSize; // ���Ž�� �� ���� ����

    [Tooltip("World Grid Size - Should match UnityEditor snap settings.")]
    [SerializeField] int unityGridSize = 10; // 3���� -> 2���� ��ǥ�� ��ȯ�� ������ ����
    public int UnityGridSize { get { return unityGridSize; } } // �ٸ� �������� ����� �� �ֵ��� public ����

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();  // Dictionnary ���� <Key��, Value��>
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } } // �ٸ� �������� ����� �� �ֵ��� public ����

    void Awake()
    {
        CreateGrid();// grid�� �����ȿ� �ִ� Ÿ�� ��ǥ���� �߰����ִ� �޼ҵ�
    }


    // grid�� �ִ� �ش� ��ǥ���� �����ϴ��� �ľ��ϰ� ���� �� ���
    // ���ڰ�(��ǥ)�� �־��� ��, grid�ȿ� ���� �ִٸ� �� ���� ��ȯ
    public Node GetNode(Vector2Int coordinates)  // Node�� ã�� �޼ҵ�
    {
        // grid ��ųʸ����� Key���� coordinates �� ���� 
        if (grid.ContainsKey(coordinates))
        {
            // ���� �����϶�   
            return grid[coordinates];
        }

        // Key ���� ���ٸ� �׳� null ���� ��ȯ�϶�
        return null;
    }

    //140
    public void BlocKNode(Vector2Int coordinates) // isPlaceable�� false�� ��带 ���ŷ (��Ȱ��ȭ�� ��Ҵ� �ƿ� ������ ���ϰ���)
    {
        // ���� grid ��ųʸ����� Key���� coordinates ���
        if (grid.ContainsKey(coordinates))  
        {
            // �ش� ��ǥ�� isWalkable�� false�� �����ع���
            grid[coordinates].isWalkable = false;
        }
    }

    public void ResetNodes()  // ��带 �����Ѵ�
    {
        // grid ��ųʸ��� ó���� ���� ��ȯ
        // KeyValuePairsms �˻��� �� �ִ� Ű/���� ��
        foreach (KeyValuePair<Vector2Int, Node> entry in grid)
        {
            entry.Value.connectedTo = null;  // value�� connectedTo�� ���� ���� null�� ����
            entry.Value.isExplored = false;  // value�� isExplored�� ���� ���� false�� ����
            entry.Value.isPath = false;   // value�� isPath�� ���� ���� false�� ����
        }
    }

    //
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)  // 3���� ��ǥ���� 2���� ��ǥ�� �����ϴ� �޼ҵ�
    {
        // ��ǥ���� ���� ���� ����
        Vector2Int coordinates = new Vector2Int();

        // Mathf.RoundToInt�� �ݿø� �Լ�
        // x��ǥ�� x/10 (���� ��ǥ�� 10����)  (10,0,20) ->> (1,2)
        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);

        // y��ǥ�� z/10 (���� ��ǥ�� 10����)
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize);

        // coordinates (��ǥ��)�� ��ȯ
        return coordinates;
    }

    //
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)  // 2���� ��ǥ���� 3���� ��ǥ�� �����ϴ� �޼ҵ�
    {
        // 3���� ��ǥ���� ���� ���� ����
        Vector3 position = new Vector3();

        // 2������ǥ ���� 10�� ���Ͽ� 3���� ��ǥ�� ����
        // (1,2) ->> (10,0,20)
        position.x = coordinates.x * unityGridSize;
        position.z = coordinates.y * unityGridSize;

        // position (3���� ��ǥ��)�� ��ȯ
        return position;
    }

    void CreateGrid() // grid�� �����ȿ� �ִ� Ÿ�� ��ǥ���� �߰����ִ� �޼ҵ�
    {
        for(int x = 0; x <gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                // coordinates(��ǥ����)�� ���� ���� ��ǥ�� �߰� (�ݺ����� ���ؼ� ó�� ���� ������ �߰�)
                Vector2Int coordinates = new Vector2Int(x, y);  

                // grid ��ųʸ�(�迭)�� �ϳ��� �߰� 
                // (Key : ��ǥ�� , Value : NodeŬ������ Node�޼ҵ�)
                grid.Add(coordinates, new Node(coordinates, true));   //�������� //Node�޼ҵ�� ��ǥ���� isWalkable ���� �޾ƿ�
            }
        }
    }
}
