using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YugantLibrary.MiniGame.WaterSort
{
    public class WS_LevelController : MonoBehaviour
    {
        [Header("Tube Info")]
        public GameObject tube;
        public int totalTubeCount,emptyTubeCount;

        [Header("Tube Placement Info")]
        public Transform tubeContainer;
        public float horizontalOffset, verticalOffset;

        private void Awake()
        {
            Init();
        }

        void Start()
        {

        }

        void Init()
        {
            for(int i = 0; i < totalTubeCount; i++)
            {
                GameObject tubeObj = Instantiate(tube, tubeContainer);
                tubeObj.name = $"Tube{i}";
                Debug.Log("Tube Name : " + tubeObj.name);

                SetTubePositions(tubeObj);

            }
        }

        void SetTubePositions(GameObject tube)
        {

        }
    }
}
