using UnityEngine;

public class Entry : MonoBehaviour
{
    [SerializeField] private BlocksGenerator _blocksGenerator;
    [SerializeField] private Transform _blocksParent;

    private void Start()
    {
        _blocksGenerator.SpawnBlocks(_blocksParent);
    }
}
