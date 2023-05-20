using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

namespace YugantLibrary.MiniGame.WaterSort
{
    public class Tube : MonoBehaviour
    {
        public Transform waterPartContainer;
        [SerializeField] Vector2 pourPos = new Vector2(1.15f, 1.5f);
        [SerializeField] float totalTransferTime = 1.5f;
        [SerializeField] Stack<Color> tubeColorStack;
        [SerializeField] bool[] isOccupied;
        [SerializeField] int tubeIndex = 0, defaultSortingLayer;

        [SerializeField] float raiseTubeVal = 0.5f;
        Vector2 defaultPos;


        public delegate void TubeOperationHandlerDelegate();
        public TubeOperationHandlerDelegate addingColorEvent, removingColorEvent, tubeSelectedEvent, tubeUnSelectedEvent;

        public delegate void TubeTransferColorDelegate(Tube tube_1, Tube tube_2);
        public TubeTransferColorDelegate tubeTransferColorEvent;

        private void OnEnable()
        {
            addingColorEvent += OnAddingColor;
            removingColorEvent += OnRemovingColor;
            tubeSelectedEvent += TubeSelected;
            tubeUnSelectedEvent += TubeUnSelected;
            tubeTransferColorEvent += TransferColor;
        }

        private void OnDisable()
        {
            addingColorEvent -= OnAddingColor;
            removingColorEvent -= OnRemovingColor;
            tubeSelectedEvent -= TubeSelected;
            tubeUnSelectedEvent -= TubeUnSelected;
            tubeTransferColorEvent -= TransferColor;
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
            waterPartContainer.transform.GetChild(GetTopSlotTubeIndex()).GetComponent<SpriteRenderer>().color = DataHandler.instance.DefaultColor();
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

            if (tube1.defaultPos.y <= tube2.defaultPos.y)
            {
                if (tube1.defaultPos.x < tube2.defaultPos.x)
                {
                    Debug.Log("Tube 1 is at Left");
                    Vector2 pos = new Vector2(tube2.transform.localPosition.x - pourPos.x, tube2.transform.localPosition.y + pourPos.y);
                    tube1.gameObject.transform.DOMove(pos, pouringTime).OnComplete(() =>
                    {
                        tube1.transform.DORotate(new Vector3(0f, 0f, -80f), pouringTime * 2).OnComplete(() =>
                        {
                            tube1.transform.DORotate(new Vector3(0f, 0f, 0f), pouringTime).OnComplete(() =>
                            {
                                tube1.transform.DOMove(tube1.defaultPos, pouringTime).OnComplete(() =>
                                {
                                    tube1.GetComponent<SortingGroup>().sortingOrder = defaultSortingLayer;
                                });
                            });

                        });
                    });

                }
                else
                {
                    Debug.Log("Tube 1 is at Right");
                    Vector2 pos = new Vector2(tube2.transform.localPosition.x + pourPos.x, tube2.transform.localPosition.y + pourPos.y);
                    tube1.gameObject.transform.DOMove(pos, pouringTime).OnComplete(() =>
                    {
                        tube1.transform.DORotate(new Vector3(0f, 0f, 80f), pouringTime * 2).OnComplete(() =>
                        {
                            tube1.transform.DORotate(new Vector3(0f, 0f, 0f), pouringTime).OnComplete(() =>
                            {
                                tube1.transform.DOMove(tube1.defaultPos, pouringTime).OnComplete(()=> 
                                { 
                                 tube1.GetComponent<SortingGroup>().sortingOrder = defaultSortingLayer;
                                });
                            });
                        });
                    });
                }
            }
            else
            {
                if (tube1.defaultPos.x < tube2.defaultPos.x)
                {
                    Debug.Log("Tube 1 is at Left");
                    Vector2 pos = new Vector2(tube2.transform.localPosition.x - pourPos.x, tube2.transform.localPosition.y - pourPos.y);
                    tube1.gameObject.transform.DOMove(pos, pouringTime).OnComplete(() =>
                    {
                        tube1.transform.DORotate(new Vector3(0f, 0f, -80f), pouringTime * 2).OnComplete(() =>
                        {
                            tube1.transform.DORotate(new Vector3(0f, 0f, 0f), pouringTime).OnComplete(() =>
                            {
                                tube1.transform.DOMove(tube1.defaultPos, pouringTime);
                            });

                        });
                    });
                }
                else
                {
                    Debug.Log("Tube 1 is at Right");
                    Vector2 pos = new Vector2(tube2.transform.localPosition.x + pourPos.x, tube2.transform.localPosition.y - pourPos.y);
                    tube1.gameObject.transform.DOMove(pos, pouringTime).OnComplete(() =>
                    {
                        tube1.transform.DORotate(new Vector3(0f, 0f, 80f), pouringTime * 2).OnComplete(() =>
                        {
                            tube1.transform.DORotate(new Vector3(0f, 0f, 0f), pouringTime).OnComplete(() =>
                            {
                                tube1.transform.DOMove(tube1.defaultPos, pouringTime);
                            });

                        });
                    });
                }
            }
        }
    }
}
