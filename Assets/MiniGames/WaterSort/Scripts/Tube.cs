using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YugantLibrary.MiniGame.WaterSort
{
    public class Tube : MonoBehaviour
    {
        public Transform waterPartContainer;
        [SerializeField] Stack<Color> tubeColorStack;
        [SerializeField] bool[] isOccupied;
        [SerializeField] int tubeIndex = 0;
        Color defaultColor;

        public void Awake()
        {
            tubeColorStack = new Stack<Color>(DataHandler.instance.maxColorInTube);
            isOccupied = new bool[DataHandler.instance.maxColorInTube];
            defaultColor = DataHandler.instance.DefaultColor();
        }

        public Color GetTubeTopColor()
        {
            return tubeColorStack.Peek();
        }

        public int GetTopSlotTubeIndex()
        {
            return tubeIndex;
        }

        public void AddToTubeStack(Color color)
        {
            tubeColorStack.Push(color);
            GetTubeIndexToStackCount();
            SetOccupiedData(GetTopSlotTubeIndex(), true); 
            waterPartContainer.transform.GetChild(GetTopSlotTubeIndex()).GetComponent<SpriteRenderer>().color = color;
            Debug.Log("ADD Tube index After : " + GetTopSlotTubeIndex());
        }

        void GetTubeIndexToStackCount()
        {
            tubeIndex = tubeColorStack.Count - 1;
        }

        public void RemoveTopTubeStack()
        {
            Debug.Log("REMOVE Tube index Before : " + GetTopSlotTubeIndex());
            waterPartContainer.transform.GetChild(GetTopSlotTubeIndex()).GetComponent<SpriteRenderer>().color = defaultColor;
            SetOccupiedData(GetTopSlotTubeIndex(), false);
            tubeColorStack.Pop();
            GetTubeIndexToStackCount();
            Debug.Log("REMOVE Tube index After : " + GetTopSlotTubeIndex());
        }

        public bool GetOccupiedData()
        {
            Debug.Log($"is Occupied {isOccupied[GetTopSlotTubeIndex()]}");
            return isOccupied[GetTopSlotTubeIndex()];
        }

        public void SetOccupiedData(int index, bool booleanVal)
        {
            isOccupied[index] = booleanVal;
        }


    }
}
