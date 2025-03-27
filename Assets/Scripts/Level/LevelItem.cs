using PathCreation;
using PathCreation.Examples;
using UnityEngine;

public class LevelItem : MonoBehaviour
{
    [field: SerializeField] public PathCreator Path { get; private set; }
    [SerializeField] private RoadMeshCreator pathMesh;

    public void Init()
    {
        pathMesh.TriggerUpdate();
    }
}
