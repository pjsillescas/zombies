using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfiguration
{
    private static TranslationService.Language Language = TranslationService.Language.en;
	private static bool GoreMode = true;

    public static void SetLanguage(TranslationService.Language language)
	{
		Language = language;
	}

	public static TranslationService.Language GetLanguage()
	{
		return Language;
	}

	public static void SetGoreMode(bool goreMode)
	{
		GoreMode = goreMode;
	}

	public static bool GetGoreMode()
	{
		return GoreMode;
	}

}
