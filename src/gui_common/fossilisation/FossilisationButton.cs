﻿using Godot;

public class FossilisationButton : TextureButton
{
    [Export]
    public Texture AlreadyFossilisedTexture = null!;

    public Microbe AttachedMicrobe = null!;

    private bool alreadyFossilised;

    [Signal]
    public delegate void OnFossilisationDialogOpened(FossilisationButton button);

    public bool AlreadyFossilised
    {
        get => alreadyFossilised;
        set
        {
            alreadyFossilised = value;

            if (alreadyFossilised)
                TextureNormal = AlreadyFossilisedTexture;
        }
    }

    public void UpdatePosition()
    {
        RectGlobalPosition = GetViewport().GetCamera().UnprojectPosition(AttachedMicrobe.GlobalTransform.origin);
    }

    private void OnPressed()
    {
        GUICommon.Instance.PlayButtonPressSound();
        EmitSignal(nameof(OnFossilisationDialogOpened), this);
    }
}
