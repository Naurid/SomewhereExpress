using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    #region Main Methods

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

        Object loadedAsset = AssetDatabase.LoadAssetAtPath($"Assets/Resources/{fileName}.asset", typeof(DialogContainer));

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

    #endregion

    #region Private and protected

    private List<Edge> Edges => _graphView.edges.ToList();
    private List<DialogNode> Nodes => _graphView.nodes.ToList().Cast<DialogNode>().ToList();

    //private List<Group> CommentBlocks =>
    //_graphView.graphElements.ToList().Where(x => x is Group).Cast<Group>().ToList();

    private DialogContainer _dialogueContainer;
    private DialogGraphView _graphView;

    #endregion
}
