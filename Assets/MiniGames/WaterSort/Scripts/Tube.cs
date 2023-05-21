using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

namespace YugantLibrary.MiniGame.WaterSort
{
    public class Tube : MonoBehaviour
    {
        [SerializeField] WS_LevelController levelController;
        public Transform waterPartContainer;
        [SerializeField] Transform leftPourTrans, rightPourTrans;
        [SerializeField] float totalTransferTime = 1.5f;
        [SerializeField] Stack<Color> tubeColorStack;
        [SerializeField] bool[] isOccupied;
        [SerializeField] int tubeIndex = 0;
        int defaultSortingLayer;

        [SerializeField] float raiseTubeVal = 0.5f;
        Vector2 defaultPos;


        public delegate void TubeOperationHandlerDelegate();
        public TubeOperationHandlerDelegate addingColorEvent, removingColorEvent, tubeSelectedEvent, tubeUnSelectedEvent;

        public delegate void TubeTransferColorDelegate(Tube tube_1, Tube tube_2);
        public TubeTransferColorDelegate tubeTransferColorEvent;

        public delegate void SwitchTubeColorDelegate(Tube tube_1, Tube tube_2, Vector3 finalPos, float pourTime);
        public SwitchTubeColorDelegate switchColorEvent;

        private void OnEnable()
        {
            addingColorEvent += OnAddingColor;
            removingColorEvent += OnRemovingColor;
            tubeSelectedEvent += TubeSelected;
            tubeUnSelectedEvent += TubeUnSelected;
            tubeTransferColorEvent += TransferColor;
            switchColorEvent += SwitchingColor;
        }

        private void OnDisable()
        {
            addingColorEvent -= OnAddingColor;
            removingColorEvent -= OnRemovingColor;
            tubeSelectedEvent -= TubeSelected;
            tubeUnSelectedEvent -= TubeUnSelected;
            tubeTransferColorEvent -= TransferColor;
            switchColorEvent -= SwitchingColor;
        }

        public void Awake()
        {
            tubeColorStack = new Stack<Color>(DataHandler.instance.maxColorInTube);
            isOccupied = new bool[DataHandler.instance.maxColorInTube];
            defaultSortingLayer = GetComponent<SpriteRenderer>().sortingOrder;
        }

        private void Start()
        {
            defaultPos = this.transform.position;
        }

        public void SetLevelControllerScript(WS_LevelController levelScript)
        {
            levelController = levelScript;
        }

        public Color GetTubeTopColor()
        {
            return tubeColorStack.Peek();
        }

        public int GetStackCount()
        {
            return tubeColorStack.Count;
        }

        public void AddToTubeStack(Color color)
        {
            tubeColorStack.Push(color);
            addingColorEvent?.Invoke();
            waterPartContainer.transform.GetChild(GetStackCount() - 1).GetComponent<SpriteRenderer>().color = color;
        }
        public void RemoveTopTubeStack()
        {
            waterPartContainer.transform.GetChild(GetStackCount() - 1).GetComponent<SpriteRenderer>().color = DataHandler.instance.DefaultColor();
            removingColorEvent?.Invoke();
            tubeColorStack.Pop();
        }

        void OnAddingColor()
        {
            SetTopOccupiedData(true);
        }

        void OnRemovingColor()
        {
            SetTopOccupiedData(false);
        }

        public bool GetOccupiedData()
        {
            return isOccupied[GetStackCount() - 1];
        }

        public void SetTopOccupiedData(bool booleanVal)
        {
            isOccupied[GetStackCount() - 1] = booleanVal;
        }

        void TubeSelected()
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + raiseTubeVal, this.transform.position.z);
        }

        void TubeUnSelected()
        {
            this.transform.position = defaultPos;
        }

        void TransferColor(Tube tube1, Tube tube2)
        {
            float pouringTime = totalTransferTime / 4;
            //Vector2 currPos = tube2.transform.localPosition;

            if (tube1.defaultPos.x <= tube2.defaultPos.x)
            {
                switchColorEvent?.Invoke(tube1, tube2, tube2.leftPourTrans.position, pouringTime);
            }
            else
            {
                switchColorEvent?.Invoke(tube1, tube2, tube2.rightPourTrans.position, pouringTime);
            }
        }

        void SwitchingColor(Tube tube_1, Tube tube_2, Vector3 finalPos, float pourTime)
        {
            tube_1.gameObject.transform.DOMove(finalPos, pourTime).OnComplete(() =>
            {
                tube_1.transform.DORotate(new Vector3(0f, 0f, -80f), (pourTime * 2)).OnComplete(() =>
                {
                    tube_1.transform.DORotate(new Vector3(0f, 0f, 0f), pourTime).OnComplete(() =>
                    {
                        tube_1.transform.DOMove(tube_1.defaultPos, pourTime).OnComplete(() =>
                        {
                            tube_1.GetComponent<SortingGroup>().sortingOrder = defaultSortingLayer;
                            ChangeColor(tube_1, tube_2);
                        });
                    });

                });
            });
        }

        void ChangeColor(Tube tube1, Tube tube2)
        {
            tube2.AddToTubeStack(tube1.GetTubeTopColor());
            tube1.RemoveTopTubeStack();
        }
    }
}
