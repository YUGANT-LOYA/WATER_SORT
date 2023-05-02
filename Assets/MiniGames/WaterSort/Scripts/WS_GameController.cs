using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YugantLibrary.MiniGame.WaterSort
{
    public class WS_GameController : MonoBehaviour
    {
        public static WS_GameController instance;

        [Header("Level Pick Info")]
        string folderToPickLevel = "Levels";
        string levelPrefix = "Level_";

        public bool isTesting;
        [SerializeField] Transform levelContainer;
        [SerializeField] GameObject currLevel;



        private void Awake()
        {
            CreateSingleton();
        }

        void CreateSingleton()
        {
            if(instance == null)
            {
                instance = this;
            }
            else if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        void Start()
        {

        }

        void Update()
        {

        }

    }
}
