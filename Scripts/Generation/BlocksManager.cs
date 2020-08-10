using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Pooling;

public class BlocksManager : MonoBehaviour
{   
    [Header("Gameplay Settings")]
    public bool GlowUp = true;
    public float Cell_Size = 0.64f;

    [Header("Materials")]
    public List<Material> BreakingBlockMaterials;
    public GameObject DefaultBG;
    public Material OutlineMat;
    public Material Default;

    [Header("Repeated Blocks Prefabs")]
    public GameObject[] PrefabStones;
    public GameObject[] PrefabCobbles;
    public GameObject[] crystals;
    public GameObject[] flora;
    public GameObject[] Ores;

    [Header("Prefab blocks")]
    public GameObject PrefabDirt;
    public GameObject PrefabRoots;
    public GameObject PrefabGrass;
    public GameObject PrefabBedrock;
    public GameObject PrefabSand;
    public GameObject PrefabSnow;
    public GameObject PrefabOak;
    public GameObject PrefabOakLeaves;
    public GameObject PrefabFir;
    public GameObject PrefabFirLeaves;
    
    [Header("Mobs Prefabs")]
    public GameObject SnowMan;
    [HideInInspector]
    public DestroyAndPlaceBlocks Destroy_Manager;

    [HideInInspector]

    public string[] trees = new string[2] {"Oak", "Fir"};

    public GameObject CreateBlock(string block, Vector2 position)
    { 
        return ObjectPooler.Instance.SpawnFromPool(block, position);
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

    public void GeneratationVertical(string BlockName, Vector2 _Position, int Height, Transform Parent)
    {   
        Vector2 Position = new Vector2(_Position.x, _Position.y);
        for (int i = 0; i <= Height; i++)
        {
            CreateBlock(BlockName, new Vector2(Position.x, Position.y)).transform.parent = Parent;
            Position.y += 0.64f;
        }

    }

    public void GeneratationHorizontal(string block, Vector2 position, int count, bool is_differnce, Transform Parent)
    {
        if (!is_differnce)
        {
            for (int i = 0; i <= count; i++)
            {
                CreateBlock(block, new Vector2(position.x, position.y)).transform.parent = Parent;
                position.x += 0.64f;
            }
        }
        else
        {
            Vector2 start_position = position;
            for (int i = 0; i <= count / 2; i++)
            {
                CreateBlock(block, new Vector2(position.x, position.y)).transform.parent = Parent;
                position.x += 0.64f;
            }
            position = start_position;
            for (int i = 0; i <= count / 2; i++)
            {
                CreateBlock(block, new Vector2(position.x, position.y)).transform.parent = Parent;
                position.x -= 0.64f;

            }
        }
    }

    public void GenerateCube(string block, Vector2 position, Vector2 Size, Transform Parent)
    {
        for (int i = 0; i < Size.y; i++)
        {   
            GeneratationHorizontal(block, new Vector2(position.x, position.y), (int)Size.x, true, Parent);
            position.y += Cell_Size;
        }
    }
    public GameObject MakeTree(string type_tree, Vector2 position)
    {   
        GameObject tree = new GameObject(type_tree);
        if (type_tree == "random"){
            type_tree = trees[Random.Range(0,2)];
        }
        switch (type_tree)
        {
            default:
                int TrunkHeight = Random.Range(2, 5);
                int LeavesHeight = Random.Range(3, 6);
                int LeavesWidth = Random.Range(2, 4);
                GeneratationVertical("Oak", position, TrunkHeight, tree.transform);
                GenerateCube("OakLeaves", new Vector2(position.x, position.y + (Cell_Size * (TrunkHeight + 1))), new Vector2(LeavesWidth, LeavesHeight), tree.transform);
                return tree;
            case "Fir":
                int height_stebel_fir = Random.Range(2, 5);
                GeneratationVertical("Fir", position, height_stebel_fir, tree.transform);
                GeneratationHorizontal("FirLeaves", new Vector2(position.x, position.y + (Cell_Size * (height_stebel_fir + 1))), 5, true, tree.transform);
                GeneratationHorizontal("FirLeaves", new Vector2(position.x, position.y + (Cell_Size * (height_stebel_fir + 2))), 3, true, tree.transform);
                GeneratationHorizontal("FirLeaves", new Vector2(position.x, position.y + (Cell_Size * (height_stebel_fir + 3))), 1, true, tree.transform);
                CreateBlock("Fir", new Vector2(position.x, position.y + (Cell_Size * (height_stebel_fir + 4)))).transform.parent = tree.transform;
                GeneratationHorizontal("FirLeaves", new Vector2(position.x, position.y + (Cell_Size * (height_stebel_fir + 5))), 3, true, tree.transform);
                GeneratationHorizontal("FirLeaves", new Vector2(position.x, position.y + (Cell_Size * (height_stebel_fir + 6))), 1, true, tree.transform);
                return tree;
        }
    }
    public void ping()
    {
        Debug.Log("PING PING PING");
    }
}
