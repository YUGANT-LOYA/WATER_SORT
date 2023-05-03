using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YugantLibrary.MiniGame.WaterSort
{
    public class WS_UI_Handler : MonoBehaviour
    {
        public Button prevButton,nextButton;

        private void Awake()
        {
            Update_Prev_Next_Button();
        }

        public void PreviousLevel()
        {
            int id = WS_GameController.instance.currLevelId;

            if (id > 1)
            {
                WS_GameController.instance.currLevelId--;
                Update_Prev_Next_Button();
                WS_GameController.instance.CreateLevel();
               
            }

            
        }

        public void NextLevel()
        {
            int id = WS_GameController.instance.currLevelId;

            if(id < WS_GameController.instance.GetTotalLevels())
            {
                WS_GameController.instance.currLevelId++;
                Update_Prev_Next_Button();
                WS_GameController.instance.CreateLevel();
                
            }
        }

        void Update_Prev_Next_Button()
        {
            int id = WS_GameController.instance.currLevelId;

            if (id <= 1)
            {
                prevButton.interactable = false;
                nextButton.interactable = true;
            }
            else if(id > 1 && id < WS_GameController.instance.GetTotalLevels())
            {
                prevButton.interactable = true;
                nextButton.interactable = true;
            }
            else
            {
                prevButton.interactable = true;
                nextButton.interactable = false;
               
            }
        }
    }
}
