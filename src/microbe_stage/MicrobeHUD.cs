using System;
using System.Collections.Generic;
using System.Text;
using Godot;

/// <summary>
///   Manages the microbe HUD display
/// </summary>
public class MicrobeHUD : Node
{
    [Export]
    public NodePath AnimationPlayerPath;
    [Export]
    public NodePath LeftPanelsPath;
    [Export]
    public NodePath MouseHoverPanelPath;
    [Export]
    public NodePath HoveredItemsContainerPath;
    [Export]
    public NodePath MenuPath;
    [Export]
    public NodePath PauseButtonPath;
    [Export]
    public NodePath ResumeButtonPath;
    [Export]
    public NodePath AtpLabelPath;
    [Export]
    public NodePath HpLabelPath;
    [Export]
    public NodePath PopulationLabelPath;
    [Export]
    public NodePath PatchLabelPath;
    [Export]
    public NodePath EditorButtonPath;
    [Export]
    public NodePath HelpScreenPath;

    [Export]
    public NodePath EnvironmentPanelPath;
    [Export]
    public NodePath OxygenBarPath;
    [Export]
    public NodePath Co2BarPath;
    [Export]
    public NodePath NitrogenBarPath;
    [Export]
    public NodePath TemperatureBarPath;
    [Export]
    public NodePath SunlightBarPath;
    [Export]
    public NodePath PressureBarPath;
    [Export]
    public NodePath EnvironmentPanelBarContainerPath;

    [Export]
    public NodePath CompoundsPanelPath;
    [Export]
    public NodePath GlucoseBarPath;
    [Export]
    public NodePath AmmoniaBarPath;
    [Export]
    public NodePath PhosphateBarPath;
    [Export]
    public NodePath HydrogenSulfideBarPath;
    [Export]
    public NodePath IronBarPath;
    [Export]
    public NodePath CompoundsPanelBarContainerPath;

    [Export]
    public NodePath AgentsPanelPath;
    [Export]
    public NodePath OxytoxyBarPath;
    [Export]
    public NodePath AgentsPanelBarContainerPath;

    [Export]
    public NodePath AtpBarPath;
    [Export]
    public NodePath HealthBarPath;
    [Export]
    public NodePath AmmoniaReproductionBarPath;
    [Export]
    public NodePath PhosphateReproductionBarPath;

    [Export]
    public PackedScene ExtinctionBoxScene;
    [Export]
    public PackedScene WinBoxScene;
    [Export]
    public AudioStream MicrobePickupOrganelleSound;
    [Export]
    public Texture AmmoniaBW;
    [Export]
    public Texture PhosphatesBW;
    [Export]
    public Texture AmmoniaInv;
    [Export]
    public Texture PhosphatesInv;

    private AnimationPlayer animationPlayer;
    private PanelContainer mouseHoverPanel;
    private VBoxContainer hoveredItems;
    private NinePatchRect environmentPanel;
    private GridContainer environmentPanelBarContainer;
    private NinePatchRect compoundsPanel;

    private ProgressBar oxygenBar;
    private ProgressBar co2Bar;
    private ProgressBar nitrogenBar;
    private ProgressBar temperatureBar;
    private ProgressBar sunlightBar;
    private ProgressBar pressureBar;

    private GridContainer compoundsPanelBarContainer;
    private ProgressBar glucoseBar;
    private ProgressBar ammoniaBar;
    private ProgressBar phosphateBar;
    private ProgressBar hydrogenSulfideBar;
    private ProgressBar ironBar;

    private Control agentsPanel;
    private ProgressBar oxytoxyBar;

    private TextureProgress atpBar;
    private TextureProgress healthBar;
    private TextureProgress ammoniaReproductionBar;
    private TextureProgress phosphateReproductionBar;

    private VBoxContainer leftPanels;
    private Control menu;
    private TextureButton pauseButton;
    private TextureButton resumeButton;
    private Label atpLabel;
    private Label hpLabel;
    private Label populationLabel;
    private Label patchLabel;
    private TextureButton editorButton;
    private Node extinctionBox;
    private Node winBox;
    private Control helpScreen;

    private Godot.Collections.Array compoundBars;

    /// <summary>
    ///   Access to the stage to retrieve information for display as
    ///   well as call some player initiated actions.
    /// </summary>
    private MicrobeStage stage;

    /// <summary>
    ///   Show mouse coordinates data in the mouse
    ///   hover box, useful during develop.
    /// </summary>
    private bool showMouseCoordinates = false;

    /// <summary>
    ///   For toggling paused with the pause button.
    /// </summary>
    private bool paused = false;

    // Checks
    private bool environmentCompressed = false;
    private bool compundCompressed = false;
    private bool leftPanelsActive = false;

    public override void _Ready()
    {
        compoundBars = GetTree().GetNodesInGroup("CompoundBar");

        mouseHoverPanel = GetNode<PanelContainer>(MouseHoverPanelPath);
        pauseButton = GetNode<TextureButton>(PauseButtonPath);
        resumeButton = GetNode<TextureButton>(ResumeButtonPath);
        leftPanels = GetNode<VBoxContainer>(LeftPanelsPath);
        agentsPanel = GetNode<Control>(AgentsPanelPath);

        environmentPanel = GetNode<NinePatchRect>(EnvironmentPanelPath);
        environmentPanelBarContainer = GetNode<GridContainer>(EnvironmentPanelBarContainerPath);
        oxygenBar = GetNode<ProgressBar>(OxygenBarPath);
        co2Bar = GetNode<ProgressBar>(Co2BarPath);
        nitrogenBar = GetNode<ProgressBar>(NitrogenBarPath);
        temperatureBar = GetNode<ProgressBar>(TemperatureBarPath);
        sunlightBar = GetNode<ProgressBar>(SunlightBarPath);
        pressureBar = GetNode<ProgressBar>(PressureBarPath);

        compoundsPanel = GetNode<NinePatchRect>(CompoundsPanelPath);
        compoundsPanelBarContainer = GetNode<GridContainer>(CompoundsPanelBarContainerPath);
        glucoseBar = GetNode<ProgressBar>(GlucoseBarPath);
        ammoniaBar = GetNode<ProgressBar>(AmmoniaBarPath);
        phosphateBar = GetNode<ProgressBar>(PhosphateBarPath);
        hydrogenSulfideBar = GetNode<ProgressBar>(HydrogenSulfideBarPath);
        ironBar = GetNode<ProgressBar>(IronBarPath);

        oxytoxyBar = GetNode<ProgressBar>(OxytoxyBarPath);
        atpBar = GetNode<TextureProgress>(AtpBarPath);
        healthBar = GetNode<TextureProgress>(HealthBarPath);
        ammoniaReproductionBar = GetNode<TextureProgress>(AmmoniaReproductionBarPath);
        phosphateReproductionBar = GetNode<TextureProgress>(PhosphateReproductionBarPath);

        atpLabel = GetNode<Label>(AtpLabelPath);
        hpLabel = GetNode<Label>(HpLabelPath);
        menu = GetNode<Control>(MenuPath);
        animationPlayer = GetNode<AnimationPlayer>(AnimationPlayerPath);
        hoveredItems = GetNode<VBoxContainer>(HoveredItemsContainerPath);
        populationLabel = GetNode<Label>(PopulationLabelPath);
        patchLabel = GetNode<Label>(PatchLabelPath);
        editorButton = GetNode<TextureButton>(EditorButtonPath);
        helpScreen = GetNode<Control>(HelpScreenPath);

        OnEnterStageTransition();
    }

    public void OnEnterStageTransition()
    {
        // Fade out for that smooth satisfying transition
        TransitionManager.Instance.AddScreenFade(Fade.FadeType.FadeOut, 0.5f);
        TransitionManager.Instance.StartTransitions(null, string.Empty);
    }

    public override void _Process(float delta)
    {
        if (stage == null)
            return;

        if (stage.Player != null)
        {
            UpdateNeededBars();
            UpdateCompoundBars();
            UpdateReproductionProgress();
            UpdateATP();
            UpdateHealth();
        }

        if (stage.Camera != null)
        {
            UpdateHoverInfo();
        }

        UpdatePopulation();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            OpenMicrobeStageMenuPressed();
        }
    }

    public void Init(MicrobeStage stage)
    {
        this.stage = stage;
    }

    public void ResizeEnvironmentPanel(string mode)
    {
        var bars = environmentPanelBarContainer.GetChildren();

        if (mode == "compress" && !environmentCompressed)
        {
            environmentCompressed = true;

            GUICommon.Instance.TweenUIProperty(
                environmentPanel, "rect_min_size", environmentPanel.RectMinSize, new Vector2(195, 170), 0.3f);
            environmentPanelBarContainer.Columns = 2;
            environmentPanelBarContainer.AddConstantOverride("vseparation", 20);
            environmentPanelBarContainer.AddConstantOverride("hseparation", 17);

            foreach (ProgressBar bar in bars)
            {
                GUICommon.Instance.TweenUIProperty(
                    bar, "rect_min_size", new Vector2(95, 25), new Vector2(73, 25), 0.3f);
                bar.GetNode<Label>("Label").Hide();
                bar.GetNode<Label>("Value").Align = Label.AlignEnum.Center;
            }
        }

        if (mode == "expand" && environmentCompressed)
        {
            environmentCompressed = false;

            GUICommon.Instance.TweenUIProperty(
                environmentPanel, "rect_min_size", environmentPanel.RectMinSize, new Vector2(195, 224), 0.3f);
            environmentPanelBarContainer.Columns = 1;
            environmentPanelBarContainer.AddConstantOverride("vseparation", 4);
            environmentPanelBarContainer.AddConstantOverride("hseparation", 0);

            foreach (ProgressBar bar in bars)
            {
                GUICommon.Instance.TweenUIProperty(
                    bar, "rect_min_size", bar.RectMinSize, new Vector2(162, 25), 0.3f);
                bar.GetNode<Label>("Label").Show();
                bar.GetNode<Label>("Value").Align = Label.AlignEnum.Left;
            }
        }
    }

    public void ResizeCompoundPanel(string mode)
    {
        var bars = compoundsPanelBarContainer.GetChildren();

        if (mode == "compress" && !compundCompressed)
        {
            compundCompressed = true;
            compoundsPanelBarContainer.AddConstantOverride("vseparation", 20);
            compoundsPanelBarContainer.AddConstantOverride("hseparation", 14);

            if (bars.Count < 4)
            {
                compoundsPanelBarContainer.Columns = 2;
            }
            else
            {
                compoundsPanelBarContainer.Columns = 3;
            }

            foreach (ProgressBar bar in bars)
            {
                GUICommon.Instance.TweenUIProperty(
                    bar, "rect_min_size", new Vector2(90, 25), new Vector2(64, 25), 0.3f);
                bar.GetNode<Label>("Label").Hide();
            }
        }

        if (mode == "expand" && compundCompressed)
        {
            compundCompressed = false;
            compoundsPanelBarContainer.Columns = 1;
            compoundsPanelBarContainer.AddConstantOverride("vseparation", 5);
            compoundsPanelBarContainer.AddConstantOverride("hseparation", 0);

            foreach (ProgressBar bar in bars)
            {
                GUICommon.Instance.TweenUIProperty(
                    bar, "rect_min_size", bar.RectMinSize, new Vector2(220, 25), 0.3f);
                bar.GetNode<Label>("Label").Show();
            }
        }
    }

    /// <summary>
    ///   Enables the editor button.
    /// </summary>
    public void ShowReproductionDialog()
    {
        if (editorButton.Disabled)
        {
            GUICommon.Instance.PlayCustomSound(MicrobePickupOrganelleSound);

            editorButton.Disabled = false;
            editorButton.GetNode<TextureRect>("Highlight").Show();
            editorButton.GetNode<TextureProgress>("ReproductionBar/PhosphateReproductionBar").TintProgress =
                new Color(1, 1, 1, 1);
            editorButton.GetNode<TextureProgress>("ReproductionBar/AmmoniaReproductionBar").TintProgress =
                new Color(1, 1, 1, 1);
            editorButton.GetNode<TextureRect>("ReproductionBar/PhosphateIcon").Texture = PhosphatesBW;
            editorButton.GetNode<TextureRect>("ReproductionBar/AmmoniaIcon").Texture = AmmoniaBW;
            editorButton.GetNode<AnimationPlayer>("AnimationPlayer").Play("EditorButtonFlash");
        }
    }

    /// <summary>
    ///   Disables the editor button.
    /// </summary>
    public void HideReproductionDialog()
    {
        if (!editorButton.Disabled)
        {
            editorButton.Disabled = true;
            editorButton.GetNode<TextureRect>("Highlight").Hide();
            editorButton.GetNode<Control>("ReproductionBar").Show();
            editorButton.GetNode<TextureProgress>("ReproductionBar/PhosphateReproductionBar").TintProgress =
                new Color(0.69f, 0.42f, 1, 1);
            editorButton.GetNode<TextureProgress>("ReproductionBar/AmmoniaReproductionBar").TintProgress =
                new Color(1, 0.62f, 0.12f, 1);
            editorButton.GetNode<TextureRect>("ReproductionBar/PhosphateIcon").Texture = PhosphatesInv;
            editorButton.GetNode<TextureRect>("ReproductionBar/AmmoniaIcon").Texture = AmmoniaInv;
            editorButton.GetNode<AnimationPlayer>("AnimationPlayer").Stop();
        }
    }

    public void OnSuicide()
    {
        if (stage.Player != null)
        {
            stage.Player.Damage(9999.0f, "suicide");
        }
    }

    public void UpdatePatchInfo(string patchName)
    {
        patchLabel.Text = "Patch: " + patchName;
    }

    public void EditorButtonPressed()
    {
        GD.Print("Move to editor pressed");

        TransitionManager.Instance.AddScreenFade(Fade.FadeType.FadeIn, 0.3f, false);
        TransitionManager.Instance.StartTransitions(stage, nameof(MicrobeStage.MoveToEditor));
    }

    public void ShowExtinctionBox()
    {
        if (extinctionBox == null)
        {
            extinctionBox = ExtinctionBoxScene.Instance();
            AddChild(extinctionBox);
        }
    }

    public void ToggleWinBox()
    {
        if (winBox == null)
        {
            winBox = WinBoxScene.Instance();
            AddChild(winBox);

            winBox.GetNode<Timer>("Timer").Connect("timeout", this, nameof(ToggleWinBox));
        }
        else
        {
            winBox.QueueFree();
        }
    }

    public void ToggleHelpScreen()
    {
        GUICommon.Instance.PlayButtonPressSound();

        if (!helpScreen.Visible)
        {
            helpScreen.Show();
            menu.Hide();
        }
        else
        {
            helpScreen.Hide();
            menu.Show();
        }
    }

    /// <summary>
    ///   Updates the GUI bars to show only needed compounds
    /// </summary>
    public void UpdateNeededBars()
    {
        if (stage.Player == null)
            return;

        var compounds = stage.Player.Compounds;

        if (!compounds.HasAnyBeenSetUseful())
            return;

        if (compounds.IsUseful("oxytoxy"))
        {
            agentsPanel.Show();
        }
        else
        {
            agentsPanel.Hide();
        }

        foreach (ProgressBar bar in compoundBars)
        {
            var compound = SimulationParameters.Instance.GetCompound(bar.Name);

            if (compounds.IsUseful(compound))
            {
                bar.Show();
            }
            else
            {
                bar.Hide();
            }
        }

        // Resize the compound panel dynamically
        var compoundsPanelVBoxContainer = compoundsPanel.GetNode<VBoxContainer>("VBoxContainer");

        compoundsPanelVBoxContainer.RectSize = new Vector2(compoundsPanelVBoxContainer.RectMinSize.x, 0);

        var targetSize = compoundsPanel.RectMinSize.LinearInterpolate(
            new Vector2(compoundsPanel.RectMinSize.x, compoundsPanelVBoxContainer.RectSize.y), 0.15f);

        compoundsPanel.RectMinSize = targetSize;
    }

    public void UpdateEnvironmentalBars(Biome biome)
    {
        var oxygen = biome.Compounds["oxygen"].Dissolved * 100;
        var co2 = biome.Compounds["carbondioxide"].Dissolved * 100;
        var nitrogen = biome.Compounds["nitrogen"].Dissolved * 100;
        var sunlight = biome.Compounds["sunlight"].Dissolved * 100;
        var temperature = biome.AverageTemperature;

        oxygenBar.MaxValue = 100;
        oxygenBar.Value = oxygen;
        oxygenBar.GetNode<Label>("Value").Text = oxygen + "%";

        co2Bar.MaxValue = 100;
        co2Bar.Value = co2;
        co2Bar.GetNode<Label>("Value").Text = co2 + "%";

        nitrogenBar.MaxValue = 100;
        nitrogenBar.Value = nitrogen;
        nitrogenBar.GetNode<Label>("Value").Text = nitrogen + "%";

        sunlightBar.MaxValue = 100;
        sunlightBar.Value = sunlight;
        sunlightBar.GetNode<Label>("Value").Text = sunlight + "%";

        temperatureBar.GetNode<Label>("Value").Text = temperature + " °C";

        // TODO: pressure?
    }

    /// <summary>
    ///   Updates the mouse hover box with stuff.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     This creates and removes GUI elements every frame.
    ///     Supposedly that's quite expensive, but I think that's
    ///     how the old JS code do it anyway.
    ///   </para>
    /// </remarks>
    private void UpdateHoverInfo()
    {
        foreach (Node children in hoveredItems.GetChildren())
        {
            hoveredItems.RemoveChild(children);

            // Using QueueFree leaves a gap at
            // the bottom of the panel
            children.Free();
        }

        if (mouseHoverPanel.RectSize != new Vector2(270, 130))
            mouseHoverPanel.RectSize = new Vector2(270, 130);

        if (mouseHoverPanel.MarginLeft != -280)
            mouseHoverPanel.MarginLeft = -280;
        if (mouseHoverPanel.MarginRight != -10)
            mouseHoverPanel.MarginRight = -10;

        var compounds = stage.Clouds.GetAllAvailableAt(stage.Camera.CursorWorldPos);

        var builder = new StringBuilder(string.Empty, 250);

        if (showMouseCoordinates)
        {
            builder.AppendFormat("Stuff at {0:F1}, {1:F1}:\n",
                stage.Camera.CursorWorldPos.x, stage.Camera.CursorWorldPos.z);
        }

        var mousePosLabel = hoveredItems.GetParent().GetNode<Label>("MousePos");

        if (compounds.Count == 0)
        {
            builder.Append("Nothing to eat here");
        }
        else
        {
            builder.Append("At cursor:");

            bool first = true;

            // Create for each compound the information in GUI
            foreach (var entry in compounds)
            {
                if (first)
                {
                    var compoundsLabel = new Label();
                    compoundsLabel.RectMinSize = new Vector2(238, 35);
                    compoundsLabel.Valign = Label.VAlign.Center;
                    hoveredItems.AddChild(compoundsLabel);
                    compoundsLabel.Text = "Compounds: ";
                }

                first = false;

                var hBox = new HBoxContainer();
                var compoundText = new Label();

                var readableName = SimulationParameters.Instance.GetCompound(entry.Key).Name;
                var compoundIcon = GUICommon.Instance.CreateCompoundIcon(readableName, 25, 25);

                var compoundsText = new StringBuilder(readableName, 150);
                compoundsText.AppendFormat(": {0:F1}", entry.Value);

                compoundText.Text = compoundsText.ToString();

                hBox.AddChild(compoundIcon);
                hBox.AddChild(compoundText);
                hoveredItems.AddChild(hBox);
            }
        }

        var aiMicrobes = GetTree().GetNodesInGroup(Constants.AI_GROUP);

        // Show the hovered over microbe's species
        foreach (Microbe entry in aiMicrobes)
        {
            var distance = (entry.Translation - stage.Camera.CursorWorldPos).Length();

            // Find only cells that have the mouse
            // position within their membrane
            if (distance > entry.Radius)
                continue;

            var microbeText = new Label();
            microbeText.RectMinSize = new Vector2(238, 40);
            microbeText.Valign = Label.VAlign.Center;
            hoveredItems.AddChild(microbeText);

            microbeText.Text = "Cell of species " + entry.Species.FormattedName;
        }

        mousePosLabel.Text = builder.ToString();
    }

    /// <summary>
    ///   Updates the compound bars with the correct values.
    /// </summary>
    private void UpdateCompoundBars()
    {
        var compounds = stage.Player.Compounds;

        glucoseBar.MaxValue = compounds.Capacity;
        glucoseBar.Value = compounds.GetCompoundAmount("glucose");
        glucoseBar.GetNode<Label>("Value").Text = glucoseBar.Value + " / " + glucoseBar.MaxValue;

        ammoniaBar.MaxValue = compounds.Capacity;
        ammoniaBar.Value = compounds.GetCompoundAmount("ammonia");
        ammoniaBar.GetNode<Label>("Value").Text = ammoniaBar.Value + " / " + ammoniaBar.MaxValue;

        phosphateBar.MaxValue = compounds.Capacity;
        phosphateBar.Value = compounds.GetCompoundAmount("phosphates");
        phosphateBar.GetNode<Label>("Value").Text = phosphateBar.Value + " / " + phosphateBar.MaxValue;

        hydrogenSulfideBar.MaxValue = compounds.Capacity;
        hydrogenSulfideBar.Value = compounds.GetCompoundAmount("hydrogensulfide");
        hydrogenSulfideBar.GetNode<Label>("Value").Text = hydrogenSulfideBar.Value + " / " +
            hydrogenSulfideBar.MaxValue;

        ironBar.MaxValue = compounds.Capacity;
        ironBar.Value = compounds.GetCompoundAmount("iron");
        ironBar.GetNode<Label>("Value").Text = ironBar.Value + " / " + ironBar.MaxValue;

        oxytoxyBar.MaxValue = compounds.Capacity;
        oxytoxyBar.Value = compounds.GetCompoundAmount("oxytoxy");
        oxytoxyBar.GetNode<Label>("Value").Text = oxytoxyBar.Value + " / " + oxytoxyBar.MaxValue;
    }

    private void UpdateReproductionProgress()
    {
        // Get player reproduction progress
        float totalProgress = stage.Player.CalculateReproductionProgress(
            out Dictionary<string, float> gatheredCompounds, out Dictionary<string, float> totalNeededCompounds);

        float fractionOfAmmonia = 0;
        float fractionOfPhosphates = 0;

        try
        {
            fractionOfAmmonia = gatheredCompounds["ammonia"] / totalNeededCompounds["ammonia"];
        }
        catch (Exception e)
        {
            GD.PrintErr("can't get reproduction ammonia progress: ", e);
        }

        try
        {
            fractionOfPhosphates = gatheredCompounds["phosphates"] / totalNeededCompounds["phosphates"];
        }
        catch (Exception e)
        {
            GD.PrintErr("can't get reproduction phosphates progress: ", e);
        }

        ammoniaReproductionBar.Value = fractionOfAmmonia * ammoniaReproductionBar.MaxValue;
        phosphateReproductionBar.Value = fractionOfPhosphates * phosphateReproductionBar.MaxValue;
    }

    private void UpdateATP()
    {
        var atp = stage.Player.Compounds.GetCompoundAmount("atp");
        var capacity = stage.Player.Compounds.Capacity;

        GUICommon.Instance.TweenBarValue(atpBar, atp, capacity);
        atpLabel.Text = Mathf.Round(atp) + " / " + capacity;

        // Hide the progress bar when the atp is less than 1.5
        if (atpBar.Value < 1.5)
        {
            atpBar.TintProgress = new Color(0, 0, 0);
        }
        else
        {
            atpBar.TintProgress = new Color(1, 1, 1);
        }
    }

    private void UpdateHealth()
    {
        var hp = stage.Player.Hitpoints;
        var maxHP = stage.Player.MaxHitpoints;

        GUICommon.Instance.TweenBarValue(healthBar, hp, maxHP);
        hpLabel.Text = Mathf.RoundToInt(hp) + " / " + maxHP;
    }

    private void UpdatePopulation()
    {
        populationLabel.Text = stage.GameWorld.PlayerSpecies.Population.ToString();
    }

    /// <summary>
    ///   Received for button that opens the menu inside the Microbe Stage.
    /// </summary>
    private void OpenMicrobeStageMenuPressed()
    {
        if (menu.Visible)
        {
            menu.Hide();

            if (!paused)
                GetTree().Paused = false;
        }
        else
        {
            menu.Show();
            GetTree().Paused = true;
        }

        GUICommon.Instance.PlayButtonPressSound();
    }

    private void PauseButtonPressed()
    {
        GUICommon.Instance.PlayButtonPressSound();

        paused = !paused;
        if (paused)
        {
            pauseButton.Hide();
            resumeButton.Show();
            pauseButton.Pressed = false;

            // Pause the game
            GetTree().Paused = true;
        }
        else
        {
            pauseButton.Show();
            resumeButton.Hide();
            resumeButton.Pressed = false;

            // Unpause the game
            GetTree().Paused = false;
        }
    }

    private void CompoundButtonPressed()
    {
        GUICommon.Instance.PlayButtonPressSound();

        if (!leftPanelsActive)
        {
            leftPanelsActive = true;
            animationPlayer.Play("HideLeftPanels");
        }
        else
        {
            leftPanelsActive = false;
            animationPlayer.Play("ShowLeftPanels");
        }
    }

    private void ReturnToMenuPressed()
    {
        // Unpause the game as well as close the pause menu
        OpenMicrobeStageMenuPressed();

        TransitionManager.Instance.AddScreenFade(Fade.FadeType.FadeIn, 0.3f, false);
        TransitionManager.Instance.StartTransitions(stage, nameof(MicrobeStage.ReturnToMenu));
    }

    /// <summary>
    ///   Receiver for exiting game from microbe stage.
    /// </summary>
    private void ExitPressed()
    {
        GUICommon.Instance.PlayButtonPressSound();
        GetTree().Quit();
    }
}