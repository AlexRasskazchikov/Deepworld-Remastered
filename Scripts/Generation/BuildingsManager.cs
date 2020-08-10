using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingsManager : MonoBehaviour
{   
    //[SerializeField] BlocksManager BlocksMngr;

    [SerializeField] int _ObjectWidth;
    [SerializeField] int _ObjectHeight;
    bool AlreadyChangedBlocks = false;

    [System.Serializable]
    public struct BlockChangeStruct{
        public GameObject OldBlocks;
        public List<GameObject> NewBlocks;
    }

    [SerializeField] List<BlockChangeStruct> BlocksChanges;
    Dictionary<string, List<GameObject>> BlocksLists = new Dictionary<string, List<GameObject>>();

    public int Width {
        get {return _ObjectWidth;}
    }

    public int Height {
        get {return _ObjectHeight;}
    }

    public void CreateBlockBackground(GameObject block, Vector2 position)
    {
        GameObject backblock = ObjectPooler.Instance.SpawnFromPool("DefaultBackground", position);
        backblock.GetComponent<SpriteRenderer>().sprite = block.GetComponent<SpriteRenderer>().sprite;
        backblock.name = block.name + "_BG";
    }
    public void ChangeBlocks(){
        foreach(BlockChangeStruct BlockStruct in BlocksChanges){
                GameObject NewBlock = BlockStruct.NewBlocks[Random.Range(0, BlockStruct.NewBlocks.Count)];
                foreach(Transform BlockInList in BlockStruct.OldBlocks.transform.GetComponentsInChildren<Transform>().ToList()){
                    if(BlockInList.gameObject.transform.tag == "block"){
                        GameObject ChangedBlock = Instantiate(NewBlock, BlockInList.position, Quaternion.identity);
                        ChangedBlock.transform.parent = transform;
                        Destroy(BlockInList.gameObject);
                    }
                    else if(BlockInList.gameObject.transform.tag == "bg_block"){
                        CreateBlockBackground(NewBlock, BlockInList.position);
                        Destroy(BlockInList.gameObject);
                    }
            }
        }
    }
}
