namespace AnimationSystem.Graph.Animations.Creation
{
    using AnimationSystem.Graph.Animations.Creation.ParameterTypes;
    using AnimationSystem.Logic.Animation;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [System.Serializable]
    public class AnimationGraphCreator 
    {

        #region Animation Objects
        [BoxGroup("Animables"), SerializeField]
        public List<AnimableObject> AnimableObjects;

        [BoxGroup("Parameters"), SerializeField]
        public ParametersContainer ParametersContainer;

        [BoxGroup("Graph"), SerializeField]
        public AnimationGraph SampleGraph;
        #endregion


#if UNITY_EDITOR
        [BoxGroup("Graph"), Button("CreateGraph")]
        public void CreateGraph()
        {
            if (SampleGraph == null)
            {
                Debug.LogError("Create empty Animation Graph and assign it to SampleGraph");
                return;
            }

            if(SampleGraph.nodes.Find(n => n.GetType() == typeof(AnimationStartScript)) == null)
            {
                var startNode = BaseNode.CreateFromType<AnimationStartScript>(new Vector2(-200, 0));
                SampleGraph.AddNode(startNode);
            }


            if (UpdateGraph())
            {
                EditorUtility.SetDirty(SampleGraph);
                AssetDatabase.SaveAssetIfDirty(SampleGraph);

                ShowGraph();
            }

        }

        [BoxGroup("Graph"), Button("Update Graph")]
        public bool UpdateGraph()
        {
            if (SampleGraph == null)
            {
                Debug.LogError("Create empty Animation Graph and assign it to SampleGraph");
                return false;
            }
            for (int i = 0; i < AnimableObjects.Count; i++)
            {
                if(AnimableObjects[i].ObjectToAnimate == null)
                {
                    Debug.LogError($"Add Object to {AnimableObjects[i].GraphParameterName} animable and match graph again");
                    return false;
                }
                var parameterGuid = "";
                if (SampleGraph.GetExposedParameter(AnimableObjects[i].GraphParameterName) != null)
                {
                    parameterGuid = SampleGraph.GetExposedParameter(AnimableObjects[i].GraphParameterName).guid;
                }
                else
                {
                    var parameterName = AnimableObjects[i].GraphParameterName != "" ? AnimableObjects[i].GraphParameterName : AnimableObjects[i].ObjectToAnimate.name;
                    parameterGuid = SampleGraph.AddExposedParameter(parameterName, typeof(GameObjectParameter), AnimableObjects[i].ObjectToAnimate);
                    AnimableObjects[i].GraphParameterName = parameterName;
                    EditorUtility.SetDirty(SampleGraph);
                    AssetDatabase.SaveAssetIfDirty(SampleGraph);
                }
                if(AnimableObjects[i].ObjectAnimator == null)
                {
                    AnimableObjects[i].GenerateAnimator();
                }

                var animables = AnimableObjects[i].ObjectAnimator.Animables;
                for (int j = 0; j < animables.Count; j++)
                {
                    var matchedNode = SampleGraph.nodes.Find(n => n.GUID == animables[j].AssignedNodeGUID);
                    if (matchedNode != null)
                    {
                        Debug.Log($"Node {matchedNode} already exist");
                    }
                    else
                    {
                        var vect = new Vector2(j * 430, -i * 300);
                        var goParameter = BaseNode.CreateFromType<ParameterNode>(vect - new Vector2(80, -70));
                        goParameter.accessor = ParameterAccessor.Get;
                        goParameter.parameterGUID = parameterGuid;
                        SampleGraph.AddNode(goParameter);
                        animables[j].CreateNode(SampleGraph, vect, goParameter);
                    }
                }
                EditorUtility.SetDirty(SampleGraph);
                AssetDatabase.SaveAssetIfDirty(SampleGraph);
            }
            Undo.undoRedoPerformed?.Invoke();
            return true;
        }

        [Button("Create Parameters")]
        public void CreateParams()
        {
            CreateParameters<FloatParameterData, float, FloatParameter>(ParametersContainer.FloatParameterDatas);
            CreateParameters<Vector3ParameterData, Vector3, Vector3Parameter>(ParametersContainer.Vector3ParameterDatas);
        }

        public bool CreateParameters<T, U, W>(List<T> parameters) where T : BaseParameterData<U, W> where W : ExposedParameter 
        {
            if (SampleGraph == null)
            {
                Debug.LogError("Create empty Animation Graph and assign it to SampleGraph");
                return false;
            }
            for(int i = 0; i < parameters.Count; i++)
            {
                if(parameters[i].ParameterName == "")
                {
                    Debug.LogError($"Empty parameter name on index {i}. Fix it and try again");
                    return false;
                }
                var parameterGuid = "";
                if (SampleGraph.GetExposedParameter(parameters[i].ParameterName) != null)
                {
                    parameterGuid = SampleGraph.GetExposedParameter(parameters[i].ParameterName).guid;
                }
                else
                {
                    var parameterName = parameters[i].ParameterName;
                    parameterGuid = SampleGraph.AddExposedParameter(parameterName, typeof(W), parameters[i].ParameterValue);
                    EditorUtility.SetDirty(SampleGraph);
                    AssetDatabase.SaveAssetIfDirty(SampleGraph);
                }
            }
            return true;
        }


        [BoxGroup("Graph"), Button("Show Graph")]
        public void ShowGraph()
        {
            if (SampleGraph == null)
            {
                Debug.LogError("Create empty Animation Graph and assign it to SampleGraph");
                return;
            }
            Undo.undoRedoPerformed?.Invoke();
            EditorWindow.GetWindow<AnimationSystem.Editor.Windows.ExposedPropertiesGraphWindow>().InitializeGraph(SampleGraph as BaseGraph);
        }
#endif
        public bool FillParameters()
        {
            if (SampleGraph == null)
            {
                Debug.LogError("Create empty Animation Graph and assign it to SampleGraph");
                return false;
            }
            var animableParameters = SampleGraph.exposedParameters.FindAll(p => p.value.GetType() == typeof(GameObject));
            if (AnimableObjects.Count < animableParameters.Count)
            {
                Debug.LogError($"MISSING OBJECTS! GRAPH NEED {animableParameters.Count} ANIMABLE OBJECTS");
                return false;
            }
            var goNodes = SampleGraph.GetParameterNodesOfType<GameObject>();
            foreach (var paramNode in goNodes)
            {
                var attachedEdges = paramNode.outputPorts[0].GetEdges();
                foreach (var att in attachedEdges)
                {
                    var no = att.inputNode as AnimationNode;
                    if(att.inputPort.fieldName == "animableGo")
                    {
                        no.SetAnimableObject(AnimableObjects.Find(a => a.GraphParameterName == paramNode.parameter.name).ObjectToAnimate);
                    }
                    else
                    {
                        no.SetOptionalGOs(new GameObject[] { AnimableObjects.Find(a => a.GraphParameterName == paramNode.parameter.name).ObjectToAnimate });
                    }
                }
            }

            return true;
        }

        [BoxGroup("Graph"), Button("Match objects with Graph")]
        public bool CheckIfObjectsMatchingGraphParameters()
        {
            if (SampleGraph == null)
            {
                Debug.LogError("Create empty Animation Graph and assign it to SampleGraph");
                return false;
            }

            var goNodes = SampleGraph.GetParameterNodesOfType<GameObject>();

            for (int i = 0; i < goNodes.Count; i++)
            {
                var matched = AnimableObjects.Find(a => a.GraphParameterName == goNodes[i].name);
                if (matched == null)
                {
                    AnimableObjects.Add(new AnimableObject() { GraphParameterName = goNodes[i].name });
                    Debug.LogWarning($"Add Object to {goNodes[i].name} animable and match graph again");
                }
                else
                {
                    if (matched.ObjectToAnimate == null)
                    {
                        Debug.LogError($"Add Object to {goNodes[i].name} animable and match graph again");
                        continue;
                    }
                    if (matched.ObjectAnimator == null)
                    {
                        matched.GenerateAnimator();
                    }
                    var nodesAttachedTo = GetNodesConnectedTo(goNodes[i].name);
                    foreach (var node in nodesAttachedTo)
                    {
                        matched.AddComponentWithType(node.GetNeededType());
                    }
                }
            }
            return true;
        }

        public List<AnimationNode> GetNodesConnectedTo(string parameterNode)
        {
            var animNodes = SampleGraph.GraphAnimationNodes;
            var connectedTo = animNodes.FindAll(n => n.GetAssignedParameter().parameter.name == parameterNode);
            return connectedTo;
        }
    }
}