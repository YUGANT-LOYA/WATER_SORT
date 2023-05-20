using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

namespace YugantLibrary.MiniGame.WaterSort
{
    public class WS_LevelController : MonoBehaviour
    {
        [BoxGroup("DIFFICULTY OF LEVEL")]
        public bool generateLevelRandomly = true;

        [BoxGroup("DIFFICULTY OF LEVEL")]
        [Range(3, 16)]
        public int totalTubeCount;
        [BoxGroup("DIFFICULTY OF LEVEL")]
        public bool customizeEmptyTubes;
        [BoxGroup("DIFFICULTY OF LEVEL")]
        [ShowIf("customizeEmptyTubes")]
        [Range(1, 4)]
        public int emptyTubeCount = 2;
        [BoxGroup("DIFFICULTY OF LEVEL")]
        public DataHandler.DIFFICULTY diffcultyOfCurrLevel = DataHandler.DIFFICULTY.NONE;
        [BoxGroup("DIFFICULTY OF LEVEL")]
        public int totalMovesToFinishLevel = 5;
        [BoxGroup("DIFFICULTY OF LEVEL")]
        public float levelTimer = 0f;

        [Header("References")]
        public GameObject tube;
        public Transform tubeContainerHolder;
        int totalTubeContainers;
        [SerializeField] List<Color> colorsUsedInTubes;
        int colorAssignIndex = 0;

        [SerializeField] Tube tube1, tube2;
        Color defaultColor;

        public List<Tube> emptyTubesList = new List<Tube>();
        public List<Tube> filledTubesList = new List<Tube>();

        private void Awake()
        {
            if (diffcultyOfCurrLevel == DataHandler.DIFFICULTY.NONE)
            {
                diffcultyOfCurrLevel = DataHandler.instance.GetCurrentDifficulty();
            }
            defaultColor = DataHandler.instance.DefaultColor();

            if (generateLevelRandomly)
            {
                Init();
                SetTubeContainerPosition();
                AssignColorsToEachTube();
                UseMoveToRandomizeColorsInTube();
            }
        }

        private void Start()
        {
           
        }

        #region Creating Tubes at RunTime with Given Data
        void Init()
        {
            if (totalTubeCount < DataHandler.instance.maxTubeInRow)
            {
                totalTubeContainers = 1;
            }
            else
            {
                totalTubeContainers = totalTubeCount / DataHandler.instance.maxTubeInRow;

                if (totalTubeCount % DataHandler.instance.maxTubeInRow > 0)
                {
                    totalTubeContainers++;
                }
            }

            bool isLastContainerHaveLessTubes = totalTubeCount % DataHandler.instance.maxTubeInRow > 0 ? true : false;

            for (int i = 1; i <= totalTubeContainers; i++)
            {

                GameObject tubeContainer = new GameObject($"TubeContainer_{i}");
                tubeContainer.transform.SetParent(tubeContainerHolder.transform);

                int totalTubes = DataHandler.instance.maxTubeInRow;

                if (isLastContainerHaveLessTubes && i == totalTubeContainers)
                {
                    totalTubes = totalTubeCount % DataHandler.instance.maxTubeInRow;
                }


                for (int j = 0; j < totalTubes; j++)
                {
                    GameObject tubeObj = Instantiate(tube, tubeContainer.transform);
                    tubeObj.name = $"Tube_{i}_{j}";
                }

                SetTubePositions(tubeContainer.transform, totalTubes);
            }
        }
        void SetTubeContainerPosition()
        {
            float temp = DataHandler.instance.tubeContainerGap / 2;
            if (totalTubeContainers % 2 == 0)
            {
                for (int i = 0; i < totalTubeContainers; i++)
                {
                    Vector3 tubeContainerPos = tubeContainerHolder.transform.position;
                    if (i % 2 != 0)
                    {
                        tubeContainerHolder.transform.GetChild(i).position = new Vector3(tubeContainerPos.x, tubeContainerPos.y - temp);
                        temp += DataHandler.instance.tubeContainerGap;
                    }
                    else
                    {
                        tubeContainerHolder.transform.GetChild(i).position = new Vector3(tubeContainerPos.x, tubeContainerPos.y + temp);

                    }
                }
            }
            else
            {
                float tubeHeight = tube.transform.localScale.y;
                temp = DataHandler.instance.tubeContainerGap;

                for (int i = 1; i < totalTubeContainers; i++)
                {
                    Vector3 tubeContainerPos = tubeContainerHolder.transform.position;
                    if (i % 2 != 0)
                    {
                        tubeContainerHolder.transform.GetChild(i).position = new Vector3(tubeContainerPos.x, tubeContainerPos.y + temp - tubeHeight);
                    }
                    else
                    {
                        tubeContainerHolder.transform.GetChild(i).position = new Vector3(tubeContainerPos.x, tubeContainerPos.y - temp + tubeHeight);
                        temp += DataHandler.instance.tubeContainerGap;
                    }
                }
            }
        }
        void SetTubePositions(Transform tubeContainer, int tubesInContainer)
        {
            int tempCounter = 1;
            Vector3 containerPos = tubeContainer.transform.position;

            if (tubesInContainer % 2 != 0)
            {
                for (int i = 1; i < tubeContainer.childCount; i++)
                {
                    if (i % 2 != 0)
                    {
                        tubeContainer.GetChild(i).transform.position = new Vector3(containerPos.x - (tempCounter * DataHandler.instance.tubeGap) - tube.transform.localScale.x / 2, containerPos.y);
                    }
                    else
                    {
                        tubeContainer.GetChild(i).transform.position = new Vector3(containerPos.x + (tempCounter * DataHandler.instance.tubeGap) + tube.transform.localScale.x / 2, containerPos.y);
                        tempCounter++;
                    }
                }
            }
            else
            {
                float temp = DataHandler.instance.tubeGap / 2;
                for (int i = 0; i < tubeContainer.childCount; i++)
                {
                    if (i % 2 != 0)
                    {
                        tubeContainer.GetChild(i).transform.position = new Vector3(containerPos.x - temp, containerPos.y);
                        temp += DataHandler.instance.tubeGap;
                    }
                    else
                    {
                        tubeContainer.GetChild(i).transform.position = new Vector3(containerPos.x + temp, containerPos.y);
                    }
                }
            }

        }
        #endregion


        #region Assigning Whole Tube with Randomly Selected Color
        void AssignColorsToEachTube()
        {
            int colorCount = totalTubeCount - emptyTubeCount;
            colorsUsedInTubes = DataHandler.instance.GetRandomColor(colorCount);
            int tubeFilledCount = colorCount;
            int[] colorAssigned = new int[colorCount];

            for (int i = 0; i < tubeContainerHolder.childCount; i++)
            {
                Transform tubeContainer = tubeContainerHolder.GetChild(i);

                for (int j = 0; j < tubeContainer.childCount; j++)
                {
                    Tube tubeScript = tubeContainer.GetChild(j).GetComponent<Tube>();

                    if (tubeFilledCount > 0)
                    {
                        do
                        {
                            colorAssignIndex = SelectRandomColorForTube(colorAssigned.ToList());
                        }
                        while (colorAssignIndex == -1);

                        colorAssigned[colorAssignIndex]++;

                        for (int k = 0; k < DataHandler.instance.maxColorInTube; k++)
                        {
                            SpriteRenderer spriteRenderer = tubeScript.waterPartContainer.GetChild(k).GetComponent<SpriteRenderer>();
                            spriteRenderer.color = colorsUsedInTubes[colorAssignIndex];

                            tubeScript.AddToTubeStack(spriteRenderer.color);
                        }

                        filledTubesList.Add(tubeScript);
                        tubeFilledCount--;
                    }
                    else
                    {
                        emptyTubesList.Add(tubeScript);
                    }
                }
            }
        }
        int SelectRandomColorForTube(List<int> list)
        {
            colorAssignIndex = Random.Range(0, list.Count);
            int val = list[colorAssignIndex] > 0 ? -1 : colorAssignIndex;
            return val;
        }
        #endregion


        void UseMoveToRandomizeColorsInTube()
        {
            SelectTube1();
            SelectTube2();

            for (int i = 0; i < totalMovesToFinishLevel; i++)
            {
                if (emptyTubesList.Count > 0)
                {
                    Debug.Log("Mixing Color Count : " + i);
                    SelectTube1();

                    if (tube2.GetTopSlotTubeIndex() == DataHandler.instance.maxColorInTube - 1)
                    {
                        SelectTube2();
                    }

                    Debug.Log("Tube 1 : " + tube1.name);
                    Debug.Log("Tube 2 : " + tube2.name);

                    MixColor(tube1, tube2);
                }
            }

            //MoveEmptyTubesAtEnd();
        }

        void SelectTube1()
        {
            tube1 = GetFilledTube();

            while (!tube1.GetOccupiedData() || tube1 == tube2)
            {
                tube1 = GetFilledTube();
            }
        }

        void SelectTube2()
        {
            tube2 = GetEmptyTube();

            while (tube1 == tube2)
            {
                tube2 = GetEmptyTube();
            }
        }

        void MixColor(Tube tubeScript1, Tube tubeScript2)
        {
            tubeScript2.AddToTubeStack(tubeScript1.GetTubeTopColor());
            tubeScript1.RemoveTopTubeStack();

            emptyTubesList.Add(tubeScript1);

            if (tubeScript1.GetTopSlotTubeIndex() == -1)
            {
                filledTubesList.Remove(tubeScript1);
                emptyTubesList.Add(tubeScript1);
            }
            
            if (tubeScript2.GetTopSlotTubeIndex() == DataHandler.instance.maxColorInTube - 1)
            {
                filledTubesList.Add(tubeScript2);
                emptyTubesList.Remove(tubeScript2);
            }

        }
        void MoveEmptyTubesAtEnd()
        {

        }
        Tube GetFilledTube()
        {
            int index = Random.Range(0, filledTubesList.Count);
            Tube tubeScript = filledTubesList[index];
            return tubeScript;
        }

        Tube GetEmptyTube()
        {
            if (tube2 == null || tube2.GetTopSlotTubeIndex() == DataHandler.instance.maxColorInTube - 1)
            {
                Debug.Log("Tube 2 Changing");
                int index = Random.Range(0, emptyTubesList.Count);
                Tube tubeScript = emptyTubesList[index];
                return tubeScript;
            }
            else
            {
                Debug.Log("Tube 2 Remains Unchanged");
                return tube2;
            }

        }

        #region Randomly Assigning Colors To Each Part of Tube
        void AssignColorsToTubeElements()
        {
            int colorCount = totalTubeCount - emptyTubeCount;
            colorsUsedInTubes = DataHandler.instance.GetRandomColor(colorCount);
            int tubeFilledCount = colorCount;
            int[] colorAssigned = new int[colorCount];

            for (int i = 0; i < tubeContainerHolder.childCount; i++)
            {
                Transform tubeContainer = tubeContainerHolder.GetChild(i);

                for (int j = 0; j < tubeContainer.childCount; j++)
                {
                    Tube tubeScript = tubeContainer.GetChild(j).GetComponent<Tube>();

                    if (tubeFilledCount > 0)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            SpriteRenderer spriteRenderer = tubeScript.waterPartContainer.GetChild(k).GetComponent<SpriteRenderer>();

                            do
                            {
                                colorAssignIndex = SelectRandomIndex(colorAssigned.ToList());
                            }
                            while (colorAssignIndex == -1);

                            colorAssigned[colorAssignIndex]++;
                            spriteRenderer.color = colorsUsedInTubes[colorAssignIndex];
                        }
                        tubeFilledCount--;
                    }
                }
            }
        }

        int SelectRandomIndex(List<int> list)
        {
            colorAssignIndex = Random.Range(0, list.Count);
            int val = list[colorAssignIndex] >= 4 ? -1 : colorAssignIndex;
            return val;
        }

        #endregion
    }
}

