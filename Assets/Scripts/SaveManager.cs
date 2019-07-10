using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MaximovInk
{
    public class SaveManager : MonoBehaviour
    {
        private SavedNode[] nodes;
        private Node[] instanceNodes; 
        public InputField fileName;

        public Transform loadButtonsParent;
        public Button buttonPrefab;
        
        public void Save()
        {
            nodes = new SavedNode[ MainManager.instance.nodesParent.childCount];

            for (var i = 0; i < nodes.Length; i++)
            {
                var node = MainManager.instance.nodesParent.GetChild(i).GetComponent<Node>();
                nodes[i] = new SavedNode
                {
                    xPosition = node.transform.position.x,
                    yPosition = node.transform.position.y,
                    id = node.instanceId,
                    ConntectedNodes = new SavedNode.ConnectedNode[node.InPoints.Length],
                    value1 = (node as EditableNode)?.value1 ?? -1,
                    value2 = (node as Label)?.textMesh.text ?? string.Empty
                };
                for (var j = 0; j < nodes[i].ConntectedNodes.Length; j++)
                {

                        nodes[i].ConntectedNodes[j] = new SavedNode.ConnectedNode
                        {
                            nodeId = node.InPoints[j].Input != null ?
                                node.InPoints[j].Input.node.transform.GetSiblingIndex() :
                                -1,
                            pointId = node.InPoints[j].Input != null ? node.InPoints[j].Input.id : -1
                        };
                    
                }

            }

            if (!Directory.Exists(Application.dataPath + "/blueprints/"))
            {
                Directory.CreateDirectory(Application.dataPath + "/blueprints");
            }

            string path = Application.dataPath + "/blueprints/" + fileName.text + ".json";
            /*if (!File.Exists(path))
            {
                File.Create(path);
            }*/

            var json = Extenshions.JsonHelper.ToJson(nodes);
            
            File.WriteAllText(path,json);

        }

        public void LoadSchemes()
        {
            if (!Directory.Exists(Application.dataPath + "/blueprints/"))
            {
                Directory.CreateDirectory(Application.dataPath + "/blueprints");
            }

            
            
            var files = Directory.EnumerateFiles(Application.dataPath+"/blueprints", "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".json") ).ToList();

            for (int i = 0; i < loadButtonsParent.childCount; i++)
            {
                Destroy(loadButtonsParent.GetChild(i).gameObject);
            }

            for (int i = 0; i < files.Count; i++)
            {
                var b = Instantiate(buttonPrefab, loadButtonsParent);
                int bi = i;
                b.GetComponentInChildren<Text>().text = Path.GetFileName(files[i]).Split('.')[0];
                b.onClick.AddListener(() => { LoadFile(files[bi]); });
            }
        }

        public void LoadFile(string path)
        {
            fileName.text = Path.GetFileName(path).Split('.')[0];
            string json = File.ReadAllText(path);
            nodes = Extenshions.JsonHelper.FromJson<SavedNode>(json);

            MainManager.instance.Clear();
            instanceNodes = new Node[nodes.Length];
            for (int i = 0; i < nodes.Length; i++)
            {
                MainManager.instance.SpawnNode(nodes[i].id,out instanceNodes[i]);
                instanceNodes[i].transform.position = new Vector3(nodes[i].xPosition,nodes[i].yPosition);
                if (instanceNodes[i] is EditableNode)
                {
                    ((EditableNode) instanceNodes[i]).value1 = nodes[i].value1;
                }

                if (instanceNodes[i] is Label)
                {
                    ((Label) instanceNodes[i]).textMesh.text = nodes[i].value2;
                }
            }

            for (int i = 0; i < nodes.Length; i++)
            {
                for (int j = 0; j < nodes[i].ConntectedNodes.Length; j++)
                {
                    if (nodes[i].ConntectedNodes[j].nodeId != -1 && nodes[i].ConntectedNodes[j].pointId != -1)
                    {                        
                        instanceNodes[i].InPoints[j].Input = instanceNodes[nodes[i].ConntectedNodes[j].nodeId]
                            .OutPoints[nodes[i].ConntectedNodes[j].pointId];
                        instanceNodes[i].InPoints[j].Input.Outs.Add(instanceNodes[i].InPoints[j]);
                        instanceNodes[i].InPoints[j].UpdateLine();
                    }
                }
                
            }
        }
    }
    
    [Serializable]
    public class SavedNode
    {
        public int id;
        
        public float xPosition, yPosition;
        public float value1;
        public string value2;

        public ConnectedNode[] ConntectedNodes;
        
        [Serializable]
        public class ConnectedNode
        {
            public int nodeId;
            public int pointId;
        }
    }
    
    
}