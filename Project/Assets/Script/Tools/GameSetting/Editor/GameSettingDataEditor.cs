﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
public class TestEditor
{
	#if UNITY_EDITOR
	[MenuItem("Assets/Export GameSettingData Object")]
	public static void Execute()
	{
		GameSettingData  data = ScriptableObject.CreateInstance<GameSettingData>();//创建Test的一个实例
	

		//创建资源文件,这时会在监视面板中看到并且可以直接编辑数据啦！！！！
		AssetDatabase.CreateAsset(data , "Assets/Resources/Data/GameSetting.asset" );
		AssetDatabase.Refresh();
		//我们还可以创建资源包
		/*Object o = AssetDatabase.LoadAssetAtPath("Assets/Resources/Data/GameSetting.asset", typeof(GameSettingData));
		BuildPipeline.BuildAssetBundle(o, null, "Assets/Resources/Data/GameSetting.unity3d");
		AssetDatabase.Refresh();*/
		//到此我们已经创建了名为“TestAsset”的资源文件了,需要用到的时候有三种方法：
		//1.编辑器环境下使用的话可以用Test o = AssetDatabase.LoadAssetAtPath(path , typeof(Test));将此资源文件“反序列化"为一个类对象,从而可以进行后续操作
		//2.运行时使用可以将该资源文件放到Resources文件夹下，并通过Test o = Resourecs.Load("TestAsset",typeof(Test)) as Test; 也可以通过WWW来 加载，详细请参考Unity3DAPI
		//我们还可以将其打包为asset,BuildPipeline.BuildAssetBundle(test, null, "TestAsset.assetbundle");若如此则只能通过WWW来加载
	}
	#endif
}