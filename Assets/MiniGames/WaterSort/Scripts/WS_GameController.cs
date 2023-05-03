using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using YugantLibrary.MyCustomAttributes;

namespace YugantLibrary.MiniGame.WaterSort
{
    public class WS_GameController : MonoBehaviour
    {
        public static WS_GameController instance;

        [Header("MainInfo")]
        [CustomReadOnly] [SerializeField] string folderToPickLevel = "WaterSort_Levels";
        [CustomReadOnly] [SerializeField] string levelPrefix = "Level_";
        [SerializeField] int totalLevels = 5;

        [Header("References")]
        [SerializeField] Transform levelContainer;
        [SerializeField] GameObject currLevel;


        public bool isTesting;

        public int currLevelId = 1;



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

        private void Start()
        {
            if (!isTesting)
            {
                CreateLevel();
            }
        }

        public void CreateLevel()
        {
            ClearLevelContainer();
            GameObject level = Instantiate(Resources.Load<GameObject>($"{folderToPickLevel}/{levelPrefix}{currLevelId}"), levelContainer);
            currLevel = level;
        }

        void ClearLevelContainer()
        {
            foreach (Transform trans in levelContainer.transform)
            {
                Destroy(trans.gameObject);
            }
        }

        public int GetTotalLevels()
        {
            return totalLevels;
        }
    }
}
