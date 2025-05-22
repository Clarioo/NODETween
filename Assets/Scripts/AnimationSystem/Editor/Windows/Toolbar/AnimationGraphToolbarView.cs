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
        protected ToolbarButtonData showGameObjects;
        protected ToolbarButtonData showFloats;
        protected ToolbarButtonData showVectors3;
        protected ToolbarButtonData showVectors2;
        protected ToolbarButtonData showStrings;
        protected ToolbarButtonData showColors;

        public AnimationGraphToolbarView(BaseGraphView baseGraphView) : base(baseGraphView)
        {

        }

        protected override void AddButtons()
        {
            AddButton("Center", graphView.ResetPositionAndZoom);

            bool animablesVisible = graphView.GetPinnedElementStatus<GameObjectParametersView>() != Status.Hidden;
            showGameObjects = AddToggle("Animables", animablesVisible, (v) => graphView.ToggleView<GameObjectParametersView>());
            bool floatsVisible = graphView.GetPinnedElementStatus<FloatParametersView>() != Status.Hidden;
            showFloats = AddToggle("Float", floatsVisible, (v) => graphView.ToggleView<FloatParametersView>());
            bool vector3Visible = graphView.GetPinnedElementStatus<Vector3ParametersView>() != Status.Hidden;
            showVectors3 = AddToggle("Vector3", vector3Visible, (v) => graphView.ToggleView<Vector3ParametersView>());
            bool vector2Visible = graphView.GetPinnedElementStatus<Vector2ParametersView>() != Status.Hidden;
            showVectors2 = AddToggle("Vector2", vector2Visible, (v) => graphView.ToggleView<Vector2ParametersView>());
            bool stringVisible = graphView.GetPinnedElementStatus<StringParametersView>() != Status.Hidden;
            showStrings = AddToggle("String", stringVisible, (v) => graphView.ToggleView<StringParametersView>());
            bool colorVisible = graphView.GetPinnedElementStatus<ColorParametersView>() != Status.Hidden;
            showColors = AddToggle("Color", colorVisible, (v) => graphView.ToggleView<ColorParametersView>());

            AddButton("Show In Project", () => EditorGUIUtility.PingObject(graphView.graph), false);
        }

    }
}