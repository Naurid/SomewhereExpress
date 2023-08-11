using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

// public class GraphSaveUtility 
// {
//     private DialogGraphView _targetGraphView;
//     private DialogContainer _containerCache;
//
//     private List<Edge> m_edges => _targetGraphView.edges.ToList();
//     private List<DialogNode> m_nodes => _targetGraphView.nodes.ToList().Cast<DialogNode>().ToList();//is there a way to write this in a simpler way?????????????????
//
//     public static GraphSaveUtility GetInstance(DialogGraphView targetGraphView)
//     {
//         return new GraphSaveUtility
//         {
//             _targetGraphView = targetGraphView
//         };
//     }
//
//     public void SaveGraph(string fileName)
//     {
//         if (!m_edges.Any()) return;
//
//         var dialogContainer = ScriptableObject.CreateInstance<DialogContainer>();
//          
//         var connectedPorts = m_edges.Where(x => x.input.node != null).ToArray();
//
//         for (var i = 0; i < connectedPorts.Length; i++)
//         {
//             var outputNode = (DialogNode)connectedPorts[i].output.node;
//             var inputNode = (DialogNode)connectedPorts[i].input.node;
//             
//             dialogContainer.m_nodeLinks.Add(new NodeLinkData
//             {
//                m_baseNodeGuid = outputNode.m_guid,
//                m_portName = connectedPorts[i].output.portName,
//                m_targetNodeGuid = inputNode.m_guid
//             });
//         }
//
//         foreach (var dialogNode in m_nodes.Where(node => !node.m_entryPoint))
//         {
//             dialogContainer.m_dialogNodeData.Add(new DialogNodeData
//             {
//                 m_guid = dialogNode.m_guid,
//                 m_dialogData = dialogNode.m_dialogItem,
//                 m_position = dialogNode.GetPosition().position
//             });
//         }
//
//         if (!AssetDatabase.IsValidFolder("Assets/Resources"))
//         {
//             AssetDatabase.CreateFolder("Assets", "Resources");
//         }
//         
//         AssetDatabase.CreateAsset(dialogContainer,$"Assets/Resources/{fileName}.asset");
//         AssetDatabase.SaveAssets();
//     }
//
//     public void LoadGraph(string fileName)
//     {
//         _containerCache = Resources.Load<DialogContainer>(fileName);
//
//         if (_containerCache == null)
//         {
//             EditorUtility.DisplayDialog("File not found", "Target Dialog File does not exist", "OK");
//             return;
//         }
//
//         ClearGraph();
//         CreateNodes();
//         ConnectNodes();
//     }
//
//     private void ConnectNodes()
//     {
//         for (int i = 0; i < m_nodes.Count; i++)
//         {
//             var connections = _containerCache.m_nodeLinks.Where(x => x.m_baseNodeGuid == m_nodes[i].m_guid).ToList();
//             Debug.Log(connections.Count);
//
//             for (int j = 0; j < connections.Count; j++)
//             {
//                 var targetNodeGuid = connections[j].m_targetNodeGuid;
//
//                 var targetNode = m_nodes.First(x => x.m_guid == targetNodeGuid);
//                 
//                 LinkNode(m_nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode?.inputContainer[0]);
//                 targetNode.SetPosition(new Rect(_containerCache.m_dialogNodeData.First(x => x.m_guid == targetNodeGuid).m_position, _targetGraphView.defaultSize));
//             }
//         }
//     }
//
//     private void LinkNode(Port output, Port input)
//     {
//         var tempEdge = new Edge
//         {
//             output = output,
//             input = input
//         };
//         
//         tempEdge?.input.Connect(tempEdge);
//         tempEdge?.output.Connect(tempEdge);
//         _targetGraphView.Add(tempEdge);
//     }
//
//     private void CreateNodes()
//     {
//         foreach (var nodeData in _containerCache.m_dialogNodeData)
//         {
//             DialogNode currentNode;
//             if (nodeData.m_dialogData != null)
//             {
//                 currentNode = _targetGraphView.CreateDialogueNode(nodeData.m_dialogData.m_name);
//             }
//             currentNode = _targetGraphView.CreateDialogueNode("");
//             currentNode.m_guid = nodeData.m_guid;
//             
//             _targetGraphView.Add(currentNode);
//
//             var nodePorts = _containerCache.m_nodeLinks.Where(x => x.m_baseNodeGuid == nodeData.m_guid).ToList();
//             nodePorts.ForEach(x => _targetGraphView.AddChoicePort(currentNode, x.m_portName));
//         }
//     }
//
//     private void ClearGraph()
//     {
//         m_nodes.Find(x => x.m_entryPoint).m_guid = _containerCache.m_nodeLinks[0].m_baseNodeGuid;
//
//         foreach (var node in m_nodes)
//         {
//             if (node.m_entryPoint) continue;
//             
//             m_edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));
//             
//             _targetGraphView.RemoveElement(node);
//         }
//     }
// }

public class GraphSaveUtility
    {
        private List<Edge> Edges => _graphView.edges.ToList();
        private List<DialogNode> Nodes => _graphView.nodes.ToList().Cast<DialogNode>().ToList();

        //private List<Group> CommentBlocks =>
            //_graphView.graphElements.ToList().Where(x => x is Group).Cast<Group>().ToList();

        private DialogContainer _dialogueContainer;
        private DialogGraphView _graphView;

        public static GraphSaveUtility GetInstance(DialogGraphView graphView)
        {
            return new GraphSaveUtility
            {
                _graphView = graphView
            };
        }

        public void SaveGraph(string fileName)
        {
            var dialogueContainerObject = ScriptableObject.CreateInstance<DialogContainer>();
            if (!SaveNodes(fileName, dialogueContainerObject)) return;
            //SaveExposedProperties(dialogueContainerObject);
            //SaveCommentBlocks(dialogueContainerObject);

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            UnityEngine.Object loadedAsset = AssetDatabase.LoadAssetAtPath($"Assets/Resources/{fileName}.asset", typeof(DialogContainer));

            if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset)) 
			{
                AssetDatabase.CreateAsset(dialogueContainerObject, $"Assets/Resources/{fileName}.asset");
            }
            else 
			{
                DialogContainer container = loadedAsset as DialogContainer;
                container.m_nodeLinks = dialogueContainerObject.m_nodeLinks;
                container.m_dialogNodeData = dialogueContainerObject.m_dialogNodeData;
                //container.ExposedProperties = dialogueContainerObject.ExposedProperties;
                //container.CommentBlockData = dialogueContainerObject.CommentBlockData;
                EditorUtility.SetDirty(container);
            }

            AssetDatabase.SaveAssets();
        }

        private bool SaveNodes(string fileName, DialogContainer dialogueContainerObject)
        {
            if (!Edges.Any()) return false;
            var connectedSockets = Edges.Where(x => x.input.node != null).ToArray();
            for (var i = 0; i < connectedSockets.Count(); i++)
            {
                var outputNode = (connectedSockets[i].output.node as DialogNode);
                var inputNode = (connectedSockets[i].input.node as DialogNode);
                dialogueContainerObject.m_nodeLinks.Add(new NodeLinkData
                {
                    m_baseNodeGuid = outputNode.m_guid,
                    m_portName = connectedSockets[i].output.portName,
                    m_targetNodeGuid = inputNode.m_guid
                });
            }

            foreach (var node in Nodes.Where(node => !node.m_entryPoint))
            {
                dialogueContainerObject.m_dialogNodeData.Add(new DialogNodeData
                {
                    m_guid = node.m_guid,
                    m_dialogData = node.m_dialogItem,
                    m_position = node.GetPosition().position
                });
            }

            return true;
        }

        // private void SaveExposedProperties(DialogueContainer dialogueContainer)
        // {
        //     dialogueContainer.ExposedProperties.Clear();
        //     dialogueContainer.ExposedProperties.AddRange(_graphView.ExposedProperties);
        // }

        // private void SaveCommentBlocks(DialogueContainer dialogueContainer)
        // {
        //     foreach (var block in CommentBlocks)
        //     {
        //         var nodes = block.containedElements.Where(x => x is DialogueNode).Cast<DialogueNode>().Select(x => x.GUID)
        //             .ToList();
        //
        //         dialogueContainer.CommentBlockData.Add(new CommentBlockData
        //         {
        //             ChildNodes = nodes,
        //             Title = block.title,
        //             Position = block.GetPosition().position
        //         });
        //     }
        // }

        public void LoadGraph(string fileName)
        {
            _dialogueContainer = Resources.Load<DialogContainer>(fileName);
            if (_dialogueContainer == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Target Narrative Data does not exist!", "OK");
                return;
            }

            ClearGraph();
            GenerateDialogueNodes();
            ConnectDialogueNodes();
            //AddExposedProperties();
            //GenerateCommentBlocks();
        }

        /// <summary>
        /// Set Entry point GUID then Get All Nodes, remove all and their edges. Leave only the entrypoint node. (Remove its edge too)
        /// </summary>
        private void ClearGraph()
        {
            Nodes.Find(x => x.m_entryPoint).m_guid = _dialogueContainer.m_nodeLinks[0].m_baseNodeGuid;
            foreach (var perNode in Nodes)
            {
                if (perNode.m_entryPoint) continue;
                Edges.Where(x => x.input.node == perNode).ToList()
                    .ForEach(edge => _graphView.RemoveElement(edge));
                _graphView.RemoveElement(perNode);
            }
        }

        /// <summary>
        /// Create All serialized nodes and assign their guid and dialogue text to them
        /// </summary>
        private void GenerateDialogueNodes()
        {
            foreach (var perNode in _dialogueContainer.m_dialogNodeData)
            {
                var tempNode = _graphView.CreateNode(perNode.m_dialogData, Vector2.zero);
                tempNode.m_guid = perNode.m_guid;
                _graphView.AddElement(tempNode);

                var nodePorts = _dialogueContainer.m_nodeLinks.Where(x => x.m_baseNodeGuid == perNode.m_guid).ToList();
                nodePorts.ForEach(x => _graphView.AddChoicePort(tempNode, x.m_portName));
            }
        }

        private void ConnectDialogueNodes()
        {
            for (var i = 0; i < Nodes.Count; i++)
            {
                var k = i; //Prevent access to modified closure
                var connections = _dialogueContainer.m_nodeLinks.Where(x => x.m_baseNodeGuid == Nodes[k].m_guid).ToList();
                for (var j = 0; j < connections.Count(); j++)
                {
                    var targetNodeGUID = connections[j].m_targetNodeGuid;
                    var targetNode = Nodes.First(x => x.m_guid == targetNodeGUID);
                    LinkNodesTogether(Nodes[i].outputContainer[j].Q<Port>(), (Port) targetNode.inputContainer[0]);

                    targetNode.SetPosition(new Rect(
                        _dialogueContainer.m_dialogNodeData.First(x => x.m_guid == targetNodeGUID).m_position,
                        _graphView.DefaultNodeSize));
                }
            }
        }

        private void LinkNodesTogether(Port outputSocket, Port inputSocket)
        {
            var tempEdge = new Edge()
            {
                output = outputSocket,
                input = inputSocket
            };
            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            _graphView.Add(tempEdge);
        }

        // private void AddExposedProperties()
        // {
        //     _graphView.ClearBlackBoardAndExposedProperties();
        //     foreach (var exposedProperty in _dialogueContainer.ExposedProperties)
        //     {
        //         _graphView.AddPropertyToBlackBoard(exposedProperty);
        //     }
        // }

        // private void GenerateCommentBlocks()
        // {
        //     foreach (var commentBlock in CommentBlocks)
        //     {
        //         _graphView.RemoveElement(commentBlock);
        //     }
        //
        //     foreach (var commentBlockData in _dialogueContainer.CommentBlockData)
        //     {
        //        var block = _graphView.CreateCommentBlock(new Rect(commentBlockData.Position, _graphView.DefaultCommentBlockSize),
        //             commentBlockData);
        //        block.AddElements(Nodes.Where(x=>commentBlockData.ChildNodes.Contains(x.GUID)));
        //     }
        // }
    }
