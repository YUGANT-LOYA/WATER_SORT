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

        public void SetTopSlotTubeIndex(int index)
        {
            tubeIndex = index;
        }

        public void AddToTubeStack(Color color)
        {
            Debug.Log("ADD Tube index Before : " + GetTopSlotTubeIndex());
            tubeColorStack.Push(color);
            SetOccupiedData(GetTopSlotTubeIndex(), true);
            waterPartContainer.transform.GetChild(GetTopSlotTubeIndex()).GetComponent<SpriteRenderer>().color = color;
            SetTubeIndexToStackCount();
            Debug.Log("ADD Tube index After : " + GetTopSlotTubeIndex());
        }

        void SetTubeIndexToStackCount()
        {
            tubeIndex = tubeColorStack.Count - 1;
        }

        public void RemoveTopTubeStack()
        {
            Debug.Log("REMOVE Tube index Before : " + GetTopSlotTubeIndex());
            waterPartContainer.transform.GetChild(GetTopSlotTubeIndex()).GetComponent<SpriteRenderer>().color = defaultColor;
            SetOccupiedData(GetTopSlotTubeIndex(), false);
            tubeColorStack.Pop();
            SetTubeIndexToStackCount();
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
