using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Pooling;
public class BreakPoint : MonoBehaviour
{   
    [SerializeField] SoundManager sounds;
    public List<GameObject> blocks = new List<GameObject>() { };
    public ItemDrop item_m;
    public HeroParameters hero;
    public GameObject DefaultBG;
    public DestroyAreaScript Area;
    public List<Material> BreakMaterial = new List<Material>() { };
    public Material Standart;

    void SetBreakMask(int hp, int MaxHp, List<Material> Materials, GameObject Block)
    {   
        int id = (Materials.Count - 1) - (int)(((float)hp / (float)MaxHp) * (float)(Materials.Count));
        Block.GetComponent<SpriteRenderer>().material = Materials[id];
    }
    public void CreateBlockBackground(GameObject block, Vector2 position, bool ChangeColor = true)
    {
        GameObject backblock = ObjectPooler.Instance.SpawnFromPool("DefaultBackground", position);
        SpriteRenderer renderer = backblock.GetComponent<SpriteRenderer>();
        SpriteRenderer OriginalBlock = block.GetComponent<SpriteRenderer>();
        renderer.sprite = OriginalBlock.sprite;
        if(ChangeColor){
            renderer.color = Color.gray;
        } else{
            renderer.color = OriginalBlock.color;
        }
        backblock.name = block.name + "_BG";
    }

    void Update()
    {
        blocks = Area.blocks;
        if (transform.name == "BreakPoint1") return;
        if (blocks.Count > 0)
        {
            for (int i = 0; i < blocks.Count; i++)
            {

                float left = blocks[i].transform.position.x;
                float right = blocks[i].transform.position.x + 0.64f;

                if (transform.position.x > left && transform.position.x < right)
                {
                    float up = blocks[i].transform.position.y;
                    float down = blocks[i].transform.position.y - 0.64f;
                    if (transform.position.y > down && transform.position.y < up)
                    {
                        GameObject block = blocks[i];
                        Block BlockScript = block.GetComponent<Block>();
                        switch (block.tag)
                        {
                            case "block":
                                if (BlockScript.hp - 1 <= 0)
                                {   
                                    block.GetComponent<SpriteRenderer>().material = Standart;
                                    if(BlockScript.PlaceBackground) {
                                        CreateBlockBackground(block, block.transform.position, BlockScript.ChangeBackgroundColor);
                                    }
                                    BlockScript.hp = BlockScript.MaxHp;
                                    item_m.GenerateItemDrop(block, transform.position);
                                    blocks.RemoveAt(i);
                                }
                                else
                                {   
                                    if(BlockScript.hp == BlockScript.MaxHp){
                                        sounds.PlayHit(block.name);
                                    }
                                    if(!BlockScript.IsBreaking){
                                        BlockScript.hp -= 1;
                                        SetBreakMask(BlockScript.hp, BlockScript.MaxHp, BreakMaterial, block);
                                        BlockScript.BreakBlock(0.1f, Standart);
                                    }
                              }
                                break;

                            case "bg_block":
                                if (Input.GetKey(KeyCode.LeftShift))
                                {
                                    blocks.RemoveAt(i);
                                    Destroy(block);
                                }
                                break;

                        }
                        gameObject.SetActive(false);

                    }
                    else gameObject.SetActive(false);
                }
                else gameObject.SetActive(false);


            }

        }
    }

}
