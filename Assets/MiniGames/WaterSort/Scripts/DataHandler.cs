using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YugantLibrary.MyCustomAttributes;

namespace YugantLibrary.MiniGame.WaterSort
{
    public class DataHandler : MonoBehaviour
    {
        public static DataHandler instance;

        public enum DIFFICULTY
        {
            NONE,
            VERY_EASY,
            EASY,
            MODERATE,
            CHALLENGING,
            EXPERT
        }

        public enum GAME_MODE
        {
            CLASSIC,
            SURVIVAL
        }


        [CustomReadOnly][SerializeField] DIFFICULTY currDifficulty = DIFFICULTY.VERY_EASY;
        [SerializeField] GAME_MODE gameMode = GAME_MODE.CLASSIC;

        public int moveToCompleteCurrLevel
        {
            get  {  return moveToCompleteCurrLevel;     }
            set  {  moveToCompleteCurrLevel = value;    }
        }

        [CustomReadOnly] [SerializeField] float camSize = 7f;

        [Header("Main Content Frame Info")]
        [CustomReadOnly] [SerializeField] float maxHorizontalTubePlacement = 5f;
        [CustomReadOnly] [SerializeField] float maxVerticalTubePlacement = 8f;

        [Header("Tube Related Info")]
        public int maxTubeInRow = 4;
        public float tubeGap = 1.5f;
        [CustomReadOnly] public int maxColorInTube = 4;
        [SerializeField] List<Color> setOfColorsInTube;

        [Header("Tube Container Info")]
        public float tubeContainerGap = 2.75f;

        private void Awake()
        {
            CreateSingleton();
        }
        void CreateSingleton()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        public float GetCamSize()
        {
            return camSize;
        }
        public float GetHorizontalFrame()
        {
            return maxHorizontalTubePlacement;
        }
        public float GetVerticalFrame()
        {
            return maxVerticalTubePlacement;
        }
        public Vector2 GetTubeHolderEndPos()
        {
            return new Vector2(maxHorizontalTubePlacement / 2, maxVerticalTubePlacement / 2);
        }
        public List<Color> TotalColorsForTubes()
        {
            return new List<Color>(setOfColorsInTube);
        }
        public List<Color> GetRandomColor(int num)
        {
            List<Color> colorsList = new List<Color>();
            List<Color> totalColorList = TotalColorsForTubes();

            for (int i = 0; i < num; i++)
            {
                int randomIndex = Random.Range(0, totalColorList.Count);
                colorsList.Add(totalColorList[randomIndex]);
                totalColorList.RemoveAt(randomIndex);
            }

            return colorsList;
        }
        public DIFFICULTY GetCurrentDifficulty()
        {
            return currDifficulty;
        }
        public GAME_MODE GetGameMode()
        {
            return gameMode;
        }



        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Camera cam = Camera.main;
            float camOffset = 0.5f;
            Gizmos.DrawWireCube(cam.transform.position, new Vector3(camSize - camOffset, camSize * 2));

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(cam.transform.position, new Vector3(maxHorizontalTubePlacement, maxVerticalTubePlacement));
        }
    }
}