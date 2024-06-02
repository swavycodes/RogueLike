using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Actor), typeof(AStar))]
public class Enemy : MonoBehaviour
{
    public Actor Target;
    public bool IsFighting = false;
    private AStar algorithm;

    private void Start()
    {
        GameManager.Get.AddEnemy(GetComponent<Actor>());
        algorithm = GetComponent<AStar>();
    }

    public void MoveAlongPath(Vector3Int targetPosition)
    {
        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        Vector2 direction = algorithm.Compute((Vector2Int)gridPosition, (Vector2Int)targetPosition);
        Action.Move(GetComponent<Actor>(), direction);
    }

    public void RunAI()
    {
        // TODO: If target is null, set target to player (from gameManager)
        if (Target == null)
        {
            Target = GameManager.Get.Player;
        }

        // TODO: convert the position of the target to a gridPosition
        var gridPosition = MapManager.Get.FloorMap.WorldToCell(Target.transform.position);

        // First check if already fighting, because the FieldOfView check costs more cpu
        if (IsFighting || GetComponent<Actor>().FieldOfView.Contains(gridPosition))
        {
            // TODO: If the enemy was not fighting, is should be fighting now
            if (!IsFighting)
            {
                IsFighting = true;
            }

            // TODO: call MoveAlongPath with the gridPosition
            MoveAlongPath(gridPosition);

        }

    }
}
