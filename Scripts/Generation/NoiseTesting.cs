using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NoiseTesting : MonoBehaviour {

    public BlocksManager BlocksFunctions;

    [Header("Landscape Settings")]
    public GameObject Block;
    public float HeightScale = 60.0F;
    public float WidthScale = 2.0F;
    public float Offset = -560.0F;
    public float BiomeOffset = 6.4F;
    public float BiomeOffset1 = 40.0F;
    public float BiomeScale = 3.0F;

    [Header("Buildings Settings")]
    public List<GameObject> BuildingsPrefabs;
    [Range(1, 10)] public int SpaceBetweenBuildings;
    float offsetPrev;
    int OqupiedBlocksCount;
    private List<GameObject> blocks = new List<GameObject>();
    
    private void Start() {
      GenerateLandscape(500);
      ChangeLandscape();
      offsetPrev = Offset;
    }

    private void Update() {
        //ChangeLandscape();
    }

    void GenerateLandscape(int Width){
        for(int i = 0; i < Width; i++){
            GameObject block = ObjectPooler.Instance.SpawnFromPool("Earth", new Vector2(0.64f * (float)i, CalculateHeight(i) * 0.64f));
            blocks.Add(block);
        }
    }

    void ChangeLandscape(){
        Color color = Color.gray;
        for(int j = 0; j < blocks.Count; j++){
            int y = CalculateHeight(j, Offset, WidthScale, HeightScale);
            int biome = CalculateHeight(j, BiomeOffset, BiomeScale, BiomeOffset1);
            blocks[j].transform.position = new Vector2(0.64f * (float)j, y * 0.64f);

            if(OqupiedBlocksCount > 0){
                OqupiedBlocksCount--;
            }

            // Biomes
            if(biome <= 10){
                // Standart Forest
                color = Color.magenta;
                if(j % 4 == 0){
                    BlocksFunctions.MakeTree("Oak Tree", new Vector2(0.64f * (float)j, (y + 1) * 0.64f));
                }

            } else if(biome <= 20){
                // After-forest
                if(Random.Range(0, 4) == 2 && OqupiedBlocksCount == 0){
                    BlocksFunctions.CreateBlock("Grass", new Vector2(0.64f * (float)j, (y + 1) * 0.64f));
                }
                color = Color.yellow;

            } else if(biome <= 30){
                // City
                color = Color.blue;
                if(OqupiedBlocksCount == 0 && GetPlainBlocksCount(j) >= 10){
                    GameObject Building = Instantiate(BuildingsPrefabs[Random.Range(0, BuildingsPrefabs.Count)], new Vector2(0.64f * (float)j, (y + 1) * 0.64f), Quaternion.identity);
                    BuildingsManager BuildingInteractions = Building.GetComponent<BuildingsManager>();
                    BuildingInteractions.ChangeBlocks();
                    OqupiedBlocksCount = BuildingInteractions.Width + SpaceBetweenBuildings;
                }

            } else {
                // Roads
                color = Color.green;
                if(Random.Range(0, 5) == 2 && OqupiedBlocksCount == 0){
                    BlocksFunctions.CreateBlock("Torch", new Vector2(0.64f * (float)j, (y + 1) * 0.64f));
                }
            }

            blocks[j].GetComponent<SpriteRenderer>().color = color;
        }
    }

    public int GetPlainBlocksCount(int StartBlockJ){
        int Result = 1;
        int StartHeight = CalculateHeight(StartBlockJ, Offset, WidthScale, HeightScale);
        int CurrentHeight = StartHeight;
        if(HeightScale > 2f || HeightScale < 2f){
            while(CurrentHeight == StartHeight){
                Result++;
                StartBlockJ++;
                CurrentHeight = CalculateHeight(StartBlockJ, Offset, WidthScale, HeightScale);
            }
            return Result;
        } else{
            return -1;
        }
        
    }

    public int CalculateHeight(int i, float seed = 1, float scale = 2, float Height = 10){
        return (int)(Height * Mathf.PerlinNoise((float)i / 100f * scale + seed, seed));
    }
}