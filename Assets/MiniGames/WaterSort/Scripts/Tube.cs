using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YugantLibrary.MiniGame.WaterSort
{
    public class Tube : MonoBehaviour
    {
        WS_LevelController levelController;
        public Transform waterPartContainer;
        [SerializeField] Stack<Color> tubeColorStack;
        [SerializeField] bool[] isOccupied;
        [SerializeField] int tubeIndex = 0;
        Color defaultColor;

        public delegate void AddingColorDelegate();
        AddingColorDelegate addingColorEvent;

        public delegate void RemovingColorDelgate();
        RemovingColorDelgate removingColorEvent;

        private void OnEnable()
        {
            addingColorEvent += OnAddingColor;
            removingColorEvent += OnRemovingColor;
        }

        private void OnDisable()
        {
            addingColorEvent -= OnAddingColor;
            removingColorEvent -= OnRemovingColor;
        }

        public void Awake()
        {
            tubeColorStack = new Stack<Color>(DataHandler.instance.maxColorInTube);
            isOccupied = new bool[DataHandler.instance.maxColorInTube];
            defaultColor = DataHandler.instance.DefaultColor();
        }

        public Color GetTubeTopColor()
        {
           // levelController.AddTubeEvent;
            return tubeColorStack.Peek();
        }

        public int GetTopSlotTubeIndex()
        {
            return tubeIndex;
        }

        public void AddToTubeStack(Color color)
        {
            tubeColorStack.Push(color);
            addingColorEvent?.Invoke();
            waterPartContainer.transform.GetChild(GetTopSlotTubeIndex()).GetComponent<SpriteRenderer>().color = color;
        }

        void OnAddingColor()
        {
            GetTubeIndexToStackCount();
            SetOccupiedData(GetTopSlotTubeIndex(), true);

        }

        void OnRemovingColor()
        {
            SetOccupiedData(GetTopSlotTubeIndex(), false);
            GetTubeIndexToStackCount();
        }

        void GetTubeIndexToStackCount()
        {
            tubeIndex = tubeColorStack.Count - 1;
        }

        public void RemoveTopTubeStack()
        {
            waterPartContainer.transform.GetChild(GetTopSlotTubeIndex()).GetComponent<SpriteRenderer>().color = defaultColor;
            removingColorEvent?.Invoke();
            tubeColorStack.Pop();
        }

        public bool GetOccupiedData()
        {
            return isOccupied[GetTopSlotTubeIndex()];
        }

        public void SetOccupiedData(int index, bool booleanVal)
        {
            isOccupied[index] = booleanVal;
        }


    }
}
