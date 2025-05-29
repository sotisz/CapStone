using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    public enum BlockType{ GROUND, WALL}
    
    public BlockType blockType;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = new GameObject();
        BlockInfo info = target.GetComponentInParent<BlockInfo>();
        if (info != null)
        {
            //info.blockType == BlockType.GROUND
        }
    }
}
