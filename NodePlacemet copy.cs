using UnityEngine;

public class NodePlacemet : MonoBehaviour
{

    public GameObject nodePrefab;
    public AIFight ai;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) //Right-click
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            GameObject node = Instantiate(nodePrefab, worldPos, Quaternion.identity);
            ai.AddNode(node.transform);
        }

    }
}
