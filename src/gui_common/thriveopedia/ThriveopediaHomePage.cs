﻿using Godot;

public class ThriveopediaHomePage : ThriveopediaPage
{
    public override string PageName => "Home";
    public override string TranslatedPageName => TranslationServer.Translate("HOME_PAGE");

    public override void UpdateCurrentWorldDetails()
    {
    }

    public override void OnNavigationPanelSizeChanged(bool collapsed)
    {
    }
}
