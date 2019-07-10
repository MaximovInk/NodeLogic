using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MaximovInk
{
    public class MainManager : MonoBehaviour
    {
        public Line line;
        
        public static MainManager instance;

        private Node selectedNode;
        
        private Point start, end;

        private Vector3 lastMousePos;

        public float camMoveSpeed = 0.1f;
        public float camScaleSpeed = 0.1f;

        private Vector2 moveOffset;
        
        public Transform cam;

        public float NodeSnap = 0.5f;
        public float PointsSnap = 0.3f;

        public Node[] nodes;

        public GameObject panelObj;
        public Text nodeName;
        public GameObject slider;
        public GameObject labelField;

        public Transform nodesParent;

        public void OnValueChange(string v)
        {
            (selectedNode as EditableNode).value1 = float.Parse(v, CultureInfo.GetCultureInfo("en-US"));
        }

        public void OnLabelEndEdit(string l)
        {
            (selectedNode as Label).textMesh.text = l;
        }

        public void Clear()
        {
            for (int i = 0; i < nodesParent.childCount; i++)
            {
                Destroy(nodesParent.GetChild(i).gameObject);
            }
        }

        public void SpawnNode(int index)
        {
            var node = Instantiate(nodes[index] ,nodesParent);
            node.instanceId = index;
            node.transform.position = new Vector3(cam.position.x,cam.position.y,0);
        }
        public void SpawnNode(int index,out Node get)
        {
            var node = Instantiate(nodes[index] ,nodesParent);
            node.instanceId = index;
            node.transform.position = new Vector3(cam.position.x,cam.position.y,0);
            get = node;
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(instance);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(instance);
                line.width = 0.05f;

            }
        }

        private void Update()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            { 
                Vector3 mousePosWorld =  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                Vector3 mousePos = Input.mousePosition;

                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    if (selectedNode != null)
                    {
                        selectedNode.RemoveNode();
                        panelObj.SetActive(false);
                    }
                }


                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (selectedNode != null)
                    {
                        var value1 = 0.0f;
                        var value2 = string.Empty;
                        var duplicateValue1 = selectedNode is EditableNode;
                        var duplicateValue2 = selectedNode is Label;

                        if (duplicateValue1)
                        {
                            value1 = (selectedNode as EditableNode).value1;
                        }

                        if (duplicateValue2)
                        {
                            value2 = (selectedNode as Label).textMesh.text;
                        }

                        SpawnNode(selectedNode.instanceId, out selectedNode);

                        if (duplicateValue1)
                        {
                            (selectedNode as EditableNode).value1 = value1;
                        }
                        if (duplicateValue2)
                        {
                            (selectedNode as Label).textMesh.text = value2;
                        }
                    }
                }



                if (Input.GetMouseButton(2))
                {

                    cam.transform.position -= (mousePos - lastMousePos) * camMoveSpeed;
                }

                if (Input.GetMouseButton(1))
                {


                    Camera.main.orthographicSize += (mousePos.x - lastMousePos.x) * camScaleSpeed;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    selectedNode = null;
                    panelObj.SetActive(false);
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                        Vector2.zero);
                    if (hit)
                    {

                        var node = (hit.collider.GetComponent<Node>());
                        var point = hit.collider.GetComponent<Point>();
                        if (node != null)
                        {
                            selectedNode = node;
                            moveOffset = selectedNode.transform.position - mousePosWorld;
                            panelObj.SetActive(true);
                            nodeName.text = selectedNode.name;
                            slider.SetActive(selectedNode is EditableNode);
                            labelField.SetActive(selectedNode is Label);
                            if (slider.activeSelf)
                            {
                                slider.GetComponent<InputField>().text =
                                    (selectedNode as EditableNode).value1.ToString("0.00000",CultureInfo.GetCultureInfo("en-US"));
                            }

                            if (labelField.activeSelf)
                            {
                                labelField.GetComponent<InputField>().text = (selectedNode as Label).textMesh.text;
                            }
                        }

                        else if (point != null)
                        {
                            start = point;
                            line.start = start.transform.position;
                            line.end = mousePosWorld;
                            line.gameObject.SetActive(true);
                        }
                    }



                }

                if (Input.GetMouseButton(0))
                {

                    if (selectedNode != null)
                    {
                        selectedNode.transform.position = mousePosWorld + (Vector3) moveOffset;
                        selectedNode.PositionChanged();
                    }

                    if (start != null)
                    {
                        line.start = start.transform.position;
                        line.end = mousePosWorld;
                    }

                }

                if (Input.GetMouseButtonUp(0))
                {
                    /*if (selectedNode != null)
                    {
                        //selectedNode.PositionChanged();
                        selectedNode = null;
                    }*/


                    //lastPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                    line.gameObject.SetActive(false);

                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                        Vector2.zero);
                    if (hit)
                    {
                        var point = hit.collider.GetComponent<Point>();
                        if (point != null)
                        {
                            end = point;
                        }
                    }

                    if (start != null && end != null)
                    {
                        if (start is PointOut && end is PointIn)
                        {
                            ApplyConntection(end as PointIn, start as PointOut);
                        }
                        else if (start is PointIn && end is PointOut)
                        {
                            ApplyConntection(start as PointIn, end as PointOut);
                        }
                    }


                }

                lastMousePos = mousePos;
            }

            

            
        }


        private void ApplyConntection(PointIn a,PointOut b)
        {
            start = null;
            end = null;
            
            if (a.Input != null)
            {
                a.Input.Outs.Remove(a);
            }
            if (a.Input == b)
            {
                a.Input = null;
            }
            else
            {
                a.Input = b;
                b.Outs.Add(a);
            }
            
            
            
            a.UpdateLine();
            a.OnCircuitChanged();
            b.OnCircuitChanged();
            

        }
    }
}