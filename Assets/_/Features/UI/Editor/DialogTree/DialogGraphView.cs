using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogGraphView : GraphView
    {
        public readonly Vector2 DefaultNodeSize = new Vector2(200, 150);
        public readonly Vector2 DefaultCommentBlockSize = new Vector2(300, 200);
        public DialogNode EntryPointNode;
        public Blackboard Blackboard = new Blackboard();
        public List<ExposedProperty> ExposedProperties { get; private set; } = new List<ExposedProperty>();
        private NodeSearchWindow _searchWindow;

        public DialogGraphView(DialogGraph editorWindow)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("DialogGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddElement(GetEntryPointNodeInstance());

            AddSearchWindow(editorWindow);
        }


        private void AddSearchWindow(DialogGraph editorWindow)
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            _searchWindow.Configure(editorWindow, this);
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }


        public void ClearBlackBoardAndExposedProperties()
        {
            ExposedProperties.Clear();
            Blackboard.Clear();
        }

        public Group CreateCommentBlock(Rect rect, CommentBlockData commentBlockData = null)
        {
            if(commentBlockData==null)
                commentBlockData = new CommentBlockData();
            var group = new Group
            {
                autoUpdateGeometry = true,
                title = commentBlockData.Title
            };
            AddElement(group);
            group.SetPosition(rect);
            return group;
        }

        public void AddPropertyToBlackBoard(ExposedProperty property, bool loadMode = false)
         {
             var localPropertyName = property.PropertyName;
             var localPropertyValue = property.PropertyValue;
             if (!loadMode)
             {
                 while (ExposedProperties.Any(x => x.PropertyName == localPropertyName))
                     localPropertyName = $"{localPropertyName}(1)";
             }
        
             var item = ExposedProperty.CreateInstance();
             item.PropertyName = localPropertyName;
             item.PropertyValue = localPropertyValue;
             ExposedProperties.Add(item);
        
             var container = new VisualElement();
             var field = new BlackboardField {text = localPropertyName, typeText = "string"};
             container.Add(field);
        
             var propertyValueTextField = new TextField("Value:")
             {
                 value = localPropertyValue
             };
             propertyValueTextField.RegisterValueChangedCallback(evt =>
             {
                 var index = ExposedProperties.FindIndex(x => x.PropertyName == item.PropertyName);
                 ExposedProperties[index].PropertyValue = evt.newValue;
             });
             var sa = new BlackboardRow(field, propertyValueTextField);
             container.Add(sa);
             Blackboard.Add(container);
         }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            var startPortView = startPort;

            ports.ForEach((port) =>
            {
                var portView = port;
                if (startPortView != portView && startPortView.node != portView.node)
                    compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        public void CreateNewDialogueNode(string nodeName, Vector2 position)
        {
            AddElement(CreateNode(null, position));
        }

        public DialogNode CreateNode(Dialog dialogData, Vector2 position)
        {
            var tempDialogueNode = new DialogNode()
            {
                title = "new dialog",
                m_dialogItem = dialogData,
                m_guid = Guid.NewGuid().ToString()
            };
            tempDialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));
            var inputPort = GetPortInstance(tempDialogueNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            tempDialogueNode.inputContainer.Add(inputPort);
            tempDialogueNode.RefreshExpandedState();
            tempDialogueNode.RefreshPorts();
            tempDialogueNode.SetPosition(new Rect(position,
                DefaultNodeSize)); //To-Do: implement screen center instantiation positioning

            var objectField = new ObjectField("Drag Dialog Object here");
            objectField.objectType = typeof(Dialog);
            objectField.RegisterValueChangedCallback(evt =>
            {
                tempDialogueNode.m_dialogItem = evt.newValue as Dialog;
                tempDialogueNode.title = tempDialogueNode.m_dialogItem != null ? tempDialogueNode.m_dialogItem.m_name : "New Dialog";
            });
            objectField.SetValueWithoutNotify(null);
            tempDialogueNode.mainContainer.Add(objectField);

            var button = new Button(() => { AddChoicePort(tempDialogueNode); })
            {
                text = "Add Choice"
            };
            tempDialogueNode.titleButtonContainer.Add(button);
            return tempDialogueNode;
        }


        public void AddChoicePort(DialogNode nodeCache, string overWrittenPortName = "")
        {
            var generatedPort = GetPortInstance(nodeCache, Direction.Output);
            var portLabel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(portLabel);

            var outputPortCount = nodeCache.outputContainer.Query("connector").ToList().Count();
            var outputPortName = string.IsNullOrEmpty(overWrittenPortName)
                ? $"Option {outputPortCount + 1}"
                : overWrittenPortName;


            // var textField = new TextField()
            // {
            //     name = string.Empty,
            //     value = outputPortName
            // };
            // textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
            // generatedPort.contentContainer.Add(new Label("  "));
            // generatedPort.contentContainer.Add(textField);

            Dialog optionDialog = null;
            var objectField = new ObjectField()
            {
                name = outputPortName,
                value = null
            };
            objectField.style.minWidth = 1f;
            objectField.style.width = 100f;
            Debug.Log(objectField.style.width);
            objectField.objectType = typeof(Dialog);
            objectField.RegisterValueChangedCallback(evt => optionDialog = (Dialog)evt.newValue);
            if (optionDialog != null)
            {
                generatedPort.portName = optionDialog.m_message;
            }
            //generatedPort.contentContainer.Add(new Label("  "));
            generatedPort.contentContainer.Add(objectField);
            
            
            var deleteButton = new Button(() => RemovePort(nodeCache, generatedPort))
            {
                text = "X"
            };
            generatedPort.contentContainer.Add(deleteButton);
            generatedPort.portName = outputPortName;
            nodeCache.outputContainer.Add(generatedPort);
            nodeCache.RefreshPorts();
            nodeCache.RefreshExpandedState();
        }

        private void RemovePort(Node node, Port socket)
        {
            var targetEdge = edges.ToList()
                .Where(x => x.output.portName == socket.portName && x.output.node == socket.node);
            if (targetEdge.Any())
            {
                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());
            }

            node.outputContainer.Remove(socket);
            node.RefreshPorts();
            node.RefreshExpandedState();
        }

        private Port GetPortInstance(DialogNode node, Direction nodeDirection,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
        }

        private DialogNode GetEntryPointNodeInstance()
        {
            var nodeCache = new DialogNode()
            {
                title = "START",
                m_guid = Guid.NewGuid().ToString(),
                m_dialogItem = null,
                m_entryPoint = true
            };

            var generatedPort = GetPortInstance(nodeCache, Direction.Output);
            generatedPort.portName = "Next";
            nodeCache.outputContainer.Add(generatedPort);

            nodeCache.capabilities &= ~Capabilities.Movable;
            nodeCache.capabilities &= ~Capabilities.Deletable;

            nodeCache.RefreshExpandedState();
            nodeCache.RefreshPorts();
            nodeCache.SetPosition(new Rect(100, 200, 100, 150));
            return nodeCache;
        }
    }
