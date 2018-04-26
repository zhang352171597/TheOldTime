using UnityEngine;
using System.Collections;

public delegate void voidDelegateDirection(Vector3 dir);
public delegate void DelegateOfVoid();
public delegate void DelegateOfVoidWithBool(bool b);
public delegate void DelegateOfUpdate(float dt);
public delegate void DelegateOfFloat(float f);
public delegate void DelegateOfString(string s);
public delegate void DelegateOfReleaseState(enSkillReleaseState d);


public class Common
{
    public string Layer_Ground = "ground";
}

public class GamePath
{
	public const string UIPrefabs = "Prefabs/UI/";
    public const string SkillPrefabs = "Prefabs/SkillPrefab/";
    public const string ActorPrefabs = "Prefabs/Actor/";
    public const string MapPrefabs = "Prefabs/Map/";
}

public class GamePrefab
{
    public const string DropItem = "Prefabs/Drop/DropItemDefault";
    public const string WorldCameraModule = "Prefabs/CameraModule";
    public const string HintItem = "Prefabs/UI/Tools/HintItem";
    public const string ItemBoxUI = "Prefabs/UI/Tools/ItemBoxUI";
}

public class GameColor
{
    public static string TextColor_Green = "[00FF16FF]";
    public static string TextColor_Red = "[FF0000]";
    public static string TextColor_Orange = "[FF6E00FF]";
    public static string TextColor_YellowWhite = "[F2D3A3FF]";
    public static string TextColor_Normal = "[-]";
    public static Color Color_Grey = new Color(0, 1, 1);
    public static Color Color_White = new Color(1, 1, 1);
}
