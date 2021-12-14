namespace Assets.Scripts.AnimationSystem.Editor.Windows.Toolbar
{
    using global::AnimationSystem.Editor.Windows.Views;
    using GraphProcessor;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using Status = UnityEngine.UIElements.DropdownMenuAction.Status;

    public class AnimationGraphToolbarView : ToolbarView
    {
        private ToolbarButtonData showGameObjects;
        private ToolbarButtonData showFloats;
        private ToolbarButtonData showVectors3;

        public AnimationGraphToolbarView(BaseGraphView baseGraphView) : base(baseGraphView)
        {

        }

        protected override void AddButtons()
        {
            AddButton("Center", graphView.ResetPositionAndZoom);

            bool animablesVisible = graphView.GetPinnedElementStatus<GameObjectParametersView>() != Status.Hidden;
            showGameObjects = AddToggle("Animables", animablesVisible, (v) => graphView.ToggleView<GameObjectParametersView>());
            bool floatsVisible = graphView.GetPinnedElementStatus<FloatParametersView>() != Status.Hidden;
            showFloats = AddToggle("Float", animablesVisible, (v) => graphView.ToggleView<FloatParametersView>());
            bool vector3Visible = graphView.GetPinnedElementStatus<Vector3ParametersView>() != Status.Hidden;
            showVectors3 = AddToggle("Vector3", animablesVisible, (v) => graphView.ToggleView<Vector3ParametersView>());

            AddButton("Show In Project", () => EditorGUIUtility.PingObject(graphView.graph), false);
        }

    }
}