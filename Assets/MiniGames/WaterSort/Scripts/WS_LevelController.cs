using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


namespace YugantLibrary.MiniGame.WaterSort
{
    public class WS_LevelController : MonoBehaviour
    {
        [Header("Tube Info")]
        public GameObject tube;

        [Range(3, 16)]
        public int totalTubeCount;

        public bool customizeEmptyTubes;

        [ShowIf("customizeEmptyTubes")]
        [Range(0, 4)]
        public int emptyTubeCount;

        [Header("Tube Placement Info")]
        public Transform tubeContainerHolder;
        int totalTubeContainers;

        private void Awake()
        {
            Init();
        }

        void Start()
        {

        }

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

            Debug.Log("Total Tube Container : " + totalTubeContainers);

            bool isLastContainerHaveLessTubes = totalTubeCount % DataHandler.instance.maxTubeInRow > 0 ? true : false;

            for (int i = 1; i <= totalTubeContainers; i++)
            {

                GameObject tubeContainer = new GameObject($"TubeContainer_{i}");
                tubeContainer.transform.SetParent(tubeContainerHolder.transform);

                int totalTubes = DataHandler.instance.maxTubeInRow;

                if (isLastContainerHaveLessTubes && i == totalTubeContainers)
                {
                    totalTubes = totalTubeCount % DataHandler.instance.maxTubeInRow;
                    Debug.Log("TOTAL TUBES : " + totalTubes);
                }


                for (int j = 0; j < totalTubes; j++)
                {
                    GameObject tubeObj = Instantiate(tube, tubeContainer.transform);
                    tubeObj.name = $"Tube_{j}";
                }

                SetTubePositions(tubeContainer.transform, totalTubes);
            }

            SetTubeContainerPosition();
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
                        Debug.Log($"Tube Pos {i} :  {tubeContainer.GetChild(i).transform.position}");
                        temp += DataHandler.instance.tubeGap;
                    }
                    else
                    {
                        tubeContainer.GetChild(i).transform.position = new Vector3(containerPos.x + temp, containerPos.y);
                        Debug.Log($"Tube Pos {i} :  {tubeContainer.GetChild(i).transform.position}");

                    }
                }
            }

        }

        void SetTubeContainerPosition()
        {
            float temp = DataHandler.instance.tubeContainerGap/2;
            if (totalTubeContainers % 2 == 0)
            {
                for (int i = 0; i < totalTubeContainers; i++)
                {
                    Vector3 tubeContainerPos = tubeContainerHolder.transform.position;
                    if (i % 2 != 0)
                    {
                        tubeContainerHolder.transform.GetChild(i).position = new Vector3(tubeContainerPos.x, tubeContainerPos.y -  temp); 
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
    }
}
