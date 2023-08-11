using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogContainer : ScriptableObject
{
    public List<NodeLinkData> m_nodeLinks = new();
    public List<DialogNodeData> m_dialogNodeData = new();
    public List<ExposedProperty> m_exposedProperties = new List<ExposedProperty>();
    public List<CommentBlockData> m_commentBlockData = new List<CommentBlockData>();
}

