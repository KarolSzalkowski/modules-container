namespace AnimationSystem.Graph.Animations.Creation
{
    using AnimationSystem.ViewManager.Animation;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [System.Serializable]
    public class AnimationGraphCreator 
    {
        [BoxGroup("Animables"), SerializeField]
        public List<AnimableObject> AnimableObjects;

        [BoxGroup("Graph"), SerializeField]
        public AnimationGraph SampleGraph;

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
                if (SampleGraph.GetExposedParameter(AnimableObjects[i].ObjectToAnimate.name) != null)
                {
                    parameterGuid = SampleGraph.GetExposedParameter(AnimableObjects[i].ObjectToAnimate.name).guid;
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

        [BoxGroup("Graph"), Button("Show Graph")]
        public void ShowGraph()
        {
            if (SampleGraph == null)
            {
                Debug.LogError("Create empty Animation Graph and assign it to SampleGraph");
                return;
            }
            Undo.undoRedoPerformed?.Invoke();
            EditorWindow.GetWindow<ExposedPropertiesGraphWindow>().InitializeGraph(SampleGraph as BaseGraph);
        }

        public bool FillParameters()
        {
            if (SampleGraph == null)
            {
                Debug.LogError("Create empty Animation Graph and assign it to SampleGraph");
                return false;
            }

            if (AnimableObjects.Count < SampleGraph.exposedParameters.Count)
            {
                Debug.LogError($"MISSING OBJECTS! GRAPH NEED {SampleGraph.exposedParameters.Count} ANIMABLE OBJECTS");
                return false;
            }
            var parameterNodes = SampleGraph.nodes.FindAll(n => n.GetType() == typeof(ParameterNode));
            foreach (var node in parameterNodes)
            {
                var paramNode = node as ParameterNode;
                var attachedNodes = node.GetOutputNodes();
                foreach (var att in attachedNodes)
                {
                    var no = att as AnimationNode;

                    no.SetAnimableObject(AnimableObjects.Find(a => a.GraphParameterName == paramNode.parameter.name).ObjectToAnimate);
                }
            }
            //for (int i = 0; i < AnimableObjects.Count; i++)
            //{
            //    AnimableObjects[i].SetObjectToAnimables();

            //    var parameter = SampleGraph.GetExposedParameter(AnimableObjects[i].GraphParameterName);
            //    if (parameter == null)
            //    {
            //        Debug.LogError($"There is no matching parameter with name: {AnimableObjects[i].GraphParameterName}");
            //        return false;
            //    }



            //    parameter.value = AnimableObjects[i].ObjectToAnimate;
            //    SampleGraph.NotifyExposedParameterChanged(parameter);
            //}
            return true;
        }

        //public bool InjectObjectsToNodes()
        //{
        //    foreach(var node in SampleGraph.GraphAnimationNodes)
        //    {
        //        node.
        //    }
        //}

        [BoxGroup("Graph"), Button("Match objects with Graph")]
        public bool CheckIfObjectsMatchingGraphParameters()
        {
            if (SampleGraph == null)
            {
                Debug.LogError("Create empty Animation Graph and assign it to SampleGraph");
                return false;
            }
            for (int i = 0; i < SampleGraph.exposedParameters.Count; i++)
            {
                var matched = AnimableObjects.Find(a => a.GraphParameterName == SampleGraph.exposedParameters[i].name);
                if (matched == null)
                {
                    AnimableObjects.Add(new AnimableObject() { GraphParameterName = SampleGraph.exposedParameters[i].name });
                    Debug.LogWarning($"Add Object to {SampleGraph.exposedParameters[i].name} animable and match graph again");
                }
                else
                {
                    if (matched.ObjectToAnimate == null)
                    {
                        Debug.LogError($"Add Object to {SampleGraph.exposedParameters[i].name} animable and match graph again");
                        continue;
                    }
                    if (matched.ObjectAnimator == null)
                    {
                        matched.GenerateAnimator();
                    }
                    var nodesAttachedTo = GetNodesConnectedTo(SampleGraph.exposedParameters[i].name);
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