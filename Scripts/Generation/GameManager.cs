using System.Collections; // импротируем все необходимые текстурки
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<GameObject> BuildingsPrefabs;

    [Header("World Configuration")]

    [Range(30f, 500f)]
    public int Width = 30; //ширина мира

    [Range(30f, 500f)]
    public int Height = 30; // высота мира


    [Range(30f, 500f)]
    public int CavesLevel = 30; 
    [Range(0,100)]
	public int RandomFillPercent = 50;

    public bool GenerateTrees = true;
    public bool GenerateFlora = true;

	private int[,] map;


    [Header("Overlay")]
    public GameObject Overlay;

    [HideInInspector]
    public float Cell_size = 0.64f; // сторона тестурки кубика (в нашем случае 64х64 - 0.64 )
    [HideInInspector]
    public int count_mobs; // количество создаваемых мобов

    private Vector2 size = new Vector2(1f, 1f); // размер блоков в юнити
    private Vector2 position = new Vector2(0, 0); // позиция самого первого блока
    private float delta = 0f; // коэффициент изменения поверхности по координате y

    [Header("Type World")]
    [System.NonSerialized]
    public string type; // тип мира
    [System.NonSerialized]
    public bool flat = false;

    [System.NonSerialized]
    
    private List<Vector2> ores_mass = new List<Vector2>(); // СПИСОК хранящий местаположения всех руд

    [Header("Managers")]
    public SpawnMob script_mobs; // скрипт спавна мобов
    public BlocksManager Block_Manager;
    public DestroyAndPlaceBlocks block_lomanie;

    string seed;
	bool useRandomSeed = true;
    int BlockInWidth = 0;
    
    [Header("Loading")]
    public UnityEngine.UI.Slider Loader;
    public GameObject loadingScreen;
    float progress = 0f;
    List<string> typesworld = new List<string> () {"City", "Standart"};
    int FlatBlocksCounter = 0;
    int BiomeBlockCounter = 0;
    bool BuildingIsPlacedOnFlatTerrain = false;

    public void Update(){
        if (loadingScreen != null){ 
            //Loader.value = progress;
            if (progress >= 100){
            loadingScreen.GetComponent<Menu>().Hero.GetComponent<HeroMovement>().Spawn(new Vector2(Width * 0.32f, 5f));
            loadingScreen.GetComponent<Menu>().Hero.SetActive(true); // включаем героя
            GenerationMobs();
            Destroy(loadingScreen);
            Overlay.SetActive(true);
            }
        } 
    }


    public void Generate() {
        StartCoroutine(OnGeneratingRoutine());

        if(CavesLevel <= 0){
            CavesLevel = Height;}

        if(Height < 0){
            Height = - Height;}

        if(Height == 0){
            Height = 50;}
        
    }
    

    void BedrockLine(GameObject block, int width, int y){
        for(int k = 0; k < width; k++){
                Block_Manager.CreateBlock("Stone", new Vector2((k + 1) * Cell_size, y * Cell_size));
            }
    }

    void CavesLevelGeneration(Vector2 Delta, GameObject[] SpriteList, int width, int height){
        GenerateMap(width, height);
        if (map != null) {
			for(int i = 0; i < width; i++){
                for(int j = 0; j < height; j++){
                        int value = map[i, j];
                        Vector2 block_pos = new Vector2((Delta.x + i) * Cell_size, j * Cell_size + -Delta.y * Cell_size + -height * Cell_size);
                        if (value == 1){
                            if (Random.Range(0, 10) == 1){
                                Block_Manager.CreateBlock("Stone", block_pos);
                            } else {
                                Block_Manager.CreateBlock("Stone", block_pos);
                            }
                        }
                        if (value == 0){
                            Block_Manager.CreateBlockBackground(Block_Manager.PrefabStones[0], block_pos);
                        }
                        if(j != 0 && map[i, j - 1] == 1 && value == 0 && Random.Range(1, 101) < 10){
                                Block_Manager.CreateBlock(new List<string>(){"Purple Crystal", "Red Crystal", "Diamonds"}[Random.Range(0, 3)], block_pos);
                            }
                    }
                }
			}
		}


    void GenerateBorders(Vector2 position, string name) {  // Генерация невидимых стенок. Передаем только позицию, и название
        GameObject block = new GameObject(name); // создаем объект стенок
        BoxCollider2D collider = block.AddComponent<BoxCollider2D>(); // добавляем хитбокс для блока
        collider.size = new Vector2(Cell_size, 99999);// указываем размеры хитбокса. 99999 - высота, чтобы игрок не вылетел за край карты
        block.transform.position = position; // указывем позицию
    }

    private IEnumerator OnGeneratingRoutine() {
        yield return null;
        float percent = 80f / Width;

        GenerateBorders(new Vector2(0, 0), "Border-left");
        GenerateBorders(new Vector2((Width + 1) * Cell_size, 0), "Border-right");
        for(int i = 0; i < Width; i++) {
            progress += percent;

            // Сдвигаем коор-ду X, чтобы начать строить следующий столбец.
            position.x += Cell_size;

            if(Random.Range(0, 10) == 1 && !flat){
                FlatBlocksCounter = Random.Range(10, 20);
            }
            if (FlatBlocksCounter > 0){
                flat = true;
                FlatBlocksCounter -= 1;
            } else {
                flat = false;
                BuildingIsPlacedOnFlatTerrain = false;
            }
            
            switch (flat){ // смотрим, плоский ли у нас мир
            case false: delta += Random.Range(-1, 2) * Cell_size; break; // не плоский - сдвигаем блок вверх или вниз
            case true: delta = delta; break; // плоский - блоки на одном уровне, не меняем переменную
            }

            float y = 0f;
            if (Random.Range(0, 10) == 1 && BiomeBlockCounter <= 0) {
                type = typesworld[Random.Range(0, typesworld.Count)];
                BiomeBlockCounter = Random.Range(20, 40);
            } else if(BiomeBlockCounter > 0){
                BiomeBlockCounter --;
            }

            for(int j = 0; j < Height; j++){ // начинаем сторить мир по высоте
                y = position.y-(Cell_size * j + delta); // получаем текущую позицию у. Из старой позиции (поскольку мы идем вниз) мы вычитаем то, на какой высоте находися для генерации (j) и изменение по высотк
                float bord = -(CavesLevel - 1) * Cell_size; // получаем координаты для коренной породы
            
                // Если блок находится выше максимальной глубины.
                if(y > bord){

                    // Разные виды миров.
                    switch (type){
                        case "City": // генерация для пустыни

                            if (j == 0){ // если у нас самый первый блок, и сработал щанс на генерацию флоры
                                switch(Random.Range(0,3)){
                                    case 1:
                                        if(flat && !BuildingIsPlacedOnFlatTerrain && FlatBlocksCounter > 8) {
                                            GameObject Building = Instantiate(BuildingsPrefabs[Random.Range(0, BuildingsPrefabs.Count)], new Vector2(position.x, y), Quaternion.identity);
                                            Building.GetComponent<BuildingsManager>().ChangeBlocks();
                                            BuildingIsPlacedOnFlatTerrain = true;
                                            }
                                        break;
                                    case 11:
                                        if(GenerateTrees) Block_Manager.MakeTree("Fir", new Vector2(position.x, y));
                                        break;
                                    case 13:
                                        if (GenerateFlora) Block_Manager.CreateBlock("Torch", new Vector2(position.x, y));
                                        break;
                                        }
                                }
                            else if(j == 1){ // если у нас идет высота для генерации песка
                                Block_Manager.CreateBlock("Earth", new Vector2(position.x, y));} // генеруем песок

                            else if(2 <= j && j < Height / 2) {// если нужная высота для генерации в данном случае коблы, занимающая 1/4 мира
                                Block_Manager.CreateBlock("Earth", new Vector2(position.x, y));} // генерируем коблы
                            else {
                               Block_Manager.CreateBlock("Stone", new Vector2(position.x, y));}  
                            break; // необходимый break для указания следущих типов. Удаленние приведет к ошибке в юнити

                        case "SnowWorld": // генерация для снежного мира
                            
                            if(j == 1){ // елсли нужная высота для генерации снега
                                Block_Manager.CreateBlock("Snow", new Vector2(position.x, y));} // генерируем снег

                            else if(2 <= j && j < Height / 4){ // если нужная высота для генерации земли, занимающая 1/4 мира
                                Block_Manager.CreateBlock("Earth", new Vector2(position.x, y));}

                            else if(Height / 4 <= j){ // если мы находимя от 1/4 высоты до половины высоты - генерируем камень
                                Block_Manager.CreateBlock("Stone", new Vector2(position.x, y));}
                            else {
                               Block_Manager.CreateBlock("Stone", new Vector2(position.x, y));}  
                            break;  // необходимый break для указания следущих типов. Удаленние приведет к ошибке в юнити
                        
                        default: // генерация для стандартного мира

                            if (j == 0 && Random.Range(0,6) < 2){ // если у нас идет самый первый блок, и сработал шанс на генерацию флоры
                                    int i_flora = Random.Range(0,2);
                                    if (i_flora == 0){
                                        GameObject obj = Block_Manager.flora[Random.Range(0, Block_Manager.flora.Length)];
                                        switch (obj.name){
                                            default:
                                                if(GenerateFlora) {
                                                    Block_Manager.CreateBlock("Torch", new Vector2(position.x, y - 0.64f));
                                                    Block_Manager.CreateBlockBackground(Block_Manager.PrefabGrass, new Vector2(position.x, y));
                                                }
                                                break;
                                            
                                            case "Torch":
                                                if(GenerateFlora) {
                                                    Block_Manager.CreateBlock("Torch", new Vector2(position.x, y));
                                                    Block_Manager.CreateBlockBackground(Block_Manager.PrefabGrass, new Vector2(position.x, y));
                                                }
                                                break;
                                        }
                                        } 
                                    else if(i_flora == 1) {
                                        if(GenerateTrees) Block_Manager.MakeTree("Oak", new Vector2(position.x, y));
                                    }
                            }
                            else if(j == 0){ // если нужная высота для генерации травы
                                if(GenerateFlora) Block_Manager.CreateBlock("Grass", new Vector2(position.x, y));} //генерируем траву

                            if(j == 1){ // если нужная высота для генерации травы
                                if(GenerateFlora){
                                     Block_Manager.CreateBlock("Roots", new Vector2(position.x, y));
                                } else {
                                    Block_Manager.CreateBlock("Earth", new Vector2(position.x, y));
                                }
                                     } //генерируем траву

                            if(2 <= j && j < Height / 2){ // если нужная высота для генерации земли, занимающая 1/4 мира
                                Block_Manager.CreateBlock("Earth", new Vector2(position.x, y));} // генерируем землю
                            if(Height / 2 <= j ){
                                Block_Manager.CreateBlock("Stone", new Vector2(position.x, y));} 
                            break;  // необходимый break для указания следущих типов. Удаленние приведет к ошибке в юнити
                    }
                }
                else { // если мы прошли все необходимые нам высоты
                    break; // завершаем цикл
                    }
                }

        while(y > -(CavesLevel) * Cell_size) {
            Block_Manager.CreateBlock("Stone", new Vector2(position.x, y));
            y -= Cell_size;
        }

        yield return new WaitForEndOfFrame(); // возвращаем именения в нашей сцене
    }

        
        CavesLevelGeneration(new Vector2(1, CavesLevel - 1), Block_Manager.PrefabStones, Width, Height);

        int bedrock_delta = 0;
        for(int i = 0; i < 8; i++){
            BedrockLine(Block_Manager.PrefabStones[Random.Range(0, Block_Manager.PrefabStones.Length)], Width, -Height * 2 + bedrock_delta);
            bedrock_delta--;
        }

        progress = 100f;

    }

    private void GenerationMobs() { // функция генерации мобов
        //switch(type) { // проверка на тип мира. Необходимо для изменения мобов
            //case "SnowWorld": // если у нас снежный мир
                script_mobs.SpawnMobs(Block_Manager.SnowMan, Random.Range(1, 5), this.Width); // генерируем мобов, от 1 до 4, до нашей ширины мира
                //break; // необходимый break для указания следущих типов. Удаленние приведет к ошибке в юнити
        //}
        
    }

    void GenerateMap(int width, int height) {
		map = new int[width, height];
		RandomFillMap(width, height);

		for (int i = 0; i < 5; i ++) {
			SmoothMap(width, height);
		}
	}


	void RandomFillMap(int width, int height) {
		if (useRandomSeed) {
			seed = Time.time.ToString();
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (x == 0 || x == width-1 || y == 0 || y == height -1) {
					map[x,y] = 1;
				}
				else {
					map[x,y] = (pseudoRandom.Next(0,100) < RandomFillPercent)? 1: 0;
				}
			}
		}
	}

	void SmoothMap(int width, int height) {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neighbourWallTiles = GetSurroundingWallCount(x, y, width, height);

				if (neighbourWallTiles > 4)
					map[x,y] = 1;
				else if (neighbourWallTiles < 4)
					map[x,y] = 0;

			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY, int width, int height) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map[neighbourX,neighbourY];
					}
				}
				else {
					wallCount ++;
				}
			}
		}

		return wallCount;
	}

    
}
