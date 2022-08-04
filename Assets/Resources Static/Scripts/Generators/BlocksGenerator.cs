using UnityEngine;

[CreateAssetMenu]
public class BlocksGenerator : ScriptableObject
{
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private Sprite[] _blockSprites;
    [SerializeField] private Vector2 _startingSpawnPosition;
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private float _offset;

    public Block[,] SpawnBlocks(Transform parent = null)
    {
        var blocks = new Block[_gridSize.x, _gridSize.y];
        var position = _startingSpawnPosition;
        var delta = Vector2.zero;
        for (var i = 0; i < _gridSize.x; i++)
        {
            for (var j = 0; j < _gridSize.y; j++)
            {
                blocks[i, j] = GetNewBlock(position, parent);
                delta = blocks[i, j].GetSize();
                position.y += delta.y;
                position.y += _offset;
            }

            position.x += delta.x;
            position.x += _offset;
            position.y = _startingSpawnPosition.y;
        }

        return blocks;
    }

    public Block GetNewBlock(Vector2 position, Transform parent)
    {
        var newBlock = Instantiate(_blockPrefab, position, _blockPrefab.transform.rotation, parent);
        var newSpriteIndex = Random.Range(0, _blockSprites.Length);
        newBlock.SetSprite(_blockSprites[newSpriteIndex]);
        return newBlock;
    }
}
