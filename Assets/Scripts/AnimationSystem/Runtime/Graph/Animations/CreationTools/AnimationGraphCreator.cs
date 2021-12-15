namespace AnimationSystem.Graph.Animations.Creation
{
    using AnimationSystem.Graph.Animations.Creation.ParameterTypes;
    using AnimationSystem.Logic.Animation;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
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

        /// <summary>
        /// Creates animables and parameters using data from animation runner, then shows created graph.
        /// Note that SampleGraph need to be filled with empty animation graph.
        /// </summary>
        [BoxGroup("Graph Control"), Button("CreateGraph")]
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
        
        /// <summary>
        /// Updates current oppened graph. Creates graph parameters based on animation runner parameters if they are not existing. 
        /// If AnimationRunner has defined animations in Animables - create nodes with given anim type with attached object nodes.
        /// </summary>
        /// <returns>TRUE if Graph updated correctly. False if updating failed - check logs.</returns>
        [BoxGroup("Graph Control"), Button("Update Graph")]
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
            if (!CreateParams())
                return false;
            
            Undo.undoRedoPerformed?.Invoke();
            return true;
        }

        /// <summary>
        /// Creates Parameters in graph based on params in AnimationRunner
        /// </summary>
        /// <returns></returns>
        public bool CreateParams()
        {
            if (!CreateParameters<FloatParameterData, float, FloatParameter>(ParametersContainer.FloatParameterDatas))
                return false;
            if (!CreateParameters<Vector3ParameterData, Vector3, Vector3Parameter>(ParametersContainer.Vector3ParameterDatas))
                return false;
            return true;
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
                if (SampleGraph.GetExposedParameter(parameters[i].ParameterName) == null)
                {
                    var parameterName = parameters[i].ParameterName;
                    SampleGraph.AddExposedParameter(parameterName, typeof(W), parameters[i].ParameterValue);
                    EditorUtility.SetDirty(SampleGraph);
                    AssetDatabase.SaveAssetIfDirty(SampleGraph);
                }
            }
            return true;
        }


        /// <summary>
        /// Opens current attached Graph if its not null.
        /// </summary>
        [BoxGroup("Graph Control"), Button("Show Graph")]
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

        /// <summary>
        /// Checks if Animation Runner has all needed Animables and Paramaters required by Graph
        /// </summary>
        /// <returns>TRUE if has all parameters, FALSE if doesn't have parameters or parameters got empty values.</returns>
        [BoxGroup("Objects To Graph Match"), Button("Check if object contain graph parameters")]
        public bool CheckIfObjectsMatchingGraphParameters()
        {
            if (SampleGraph == null)
            {
                Debug.LogError("Create empty Animation Graph and assign it to SampleGraph");
                return false;
            }

            var goNodes = SampleGraph.GetParametersOfType<GameObjectParameter>();

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

            if (!CheckParametersWithType<System.Single, FloatParameter, FloatParameterData>(ref ParametersContainer.FloatParameterDatas))
                return false;
            if (!CheckParametersWithType<Vector3, Vector3Parameter, Vector3ParameterData>(ref ParametersContainer.Vector3ParameterDatas))
                return false;
            return true;
        }

        /// <summary>
        /// Cheks if Runner has all needed Parameters with given Type.
        /// </summary>
        /// <typeparam name="T">Input value type</typeparam>
        /// <typeparam name="U">Parameter Type - inherits from ExposedParameter</typeparam>
        /// <typeparam name="W">Parameter Data Model - inherits from BaseParameterData</typeparam>
        /// <param name="paramsInObject">Parameters list to check and fill</param>
        /// <returns>true if fill</returns>
        public bool CheckParametersWithType<T, U, W>(ref List<W> paramsInObject) where W : BaseParameterData<T, U> where U : ExposedParameter
        {
            var paramNodes = SampleGraph.GetParametersOfType<U>();

            for (int i = 0; i < paramNodes.Count; i++)
            {
                var matched = paramsInObject.Find(a => a.ParameterName == paramNodes[i].name);
                if (matched == null)
                {
                    var newParam = (W)Activator.CreateInstance(typeof(W), new object[] { paramNodes[i].name });
                    paramsInObject.Add(newParam);
                }
            }
            return true;
        }

#endif
        /// <summary>
        /// Fills nodes values in animation graph.
        /// </summary>
        /// <returns>TRUE if values are filled correctly, FALSE if got an error - Check logs</returns>
        public bool FillParameters()
        {
            FillAnimationParameters();

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

        /// <summary>
        /// Filling parameters values in Animation Graph based on values from AnimtationRunner
        /// </summary>
        private void FillAnimationParameters()
        {
            FillParametersOfType<float, FloatParameter, FloatParameterData>(ref ParametersContainer.FloatParameterDatas);
            FillParametersOfType<Vector3, Vector3Parameter, Vector3ParameterData>(ref ParametersContainer.Vector3ParameterDatas);
        }

        /// <summary>
        /// Fills parameters of given type T
        /// </summary>
        /// <typeparam name="T">Input value type</typeparam>
        /// <typeparam name="U">Parameter Type - inherits from ExposedParameter</typeparam>
        /// <typeparam name="W">Parameter Data Model - inherits from BaseParameterData</typeparam>
        /// <param name="paramsInObject"></param>
        private void FillParametersOfType<T, U, W>(ref List<W> paramsInObject) where W : BaseParameterData<T, U> where U : ExposedParameter
        {
            var parameters = SampleGraph.GetParametersOfType<U>();
            foreach(var parameter in parameters)
            {
                parameter.value = paramsInObject.Find(p => p.ParameterName == parameter.name).ParameterValue;
                SampleGraph.NotifyExposedParameterValueChanged(parameter);
            }
        }

        /// <summary>
        /// Gets connected nodes to node with <paramref name="parameterNode"/>
        /// </summary>
        /// <param name="parameterNode">Name of the node</param>
        /// <returns>Connected nodes</returns>
        public List<AnimationNode> GetNodesConnectedTo(string parameterNode)
        {
            var animNodes = SampleGraph.GraphAnimationNodes;
            var connectedTo = animNodes.FindAll(n => n.GetAssignedParameter().parameter.name == parameterNode);
            return connectedTo;
        }

        #region Remote Control
        public void SetParameter<T>(string parameterName, T value) 
        {
            
        }

        #endregion
    }
}