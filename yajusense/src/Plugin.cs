﻿using System;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using yajusense.Core;
using yajusense.Core.Config;
using yajusense.Core.Services;
using yajusense.Modules;
using yajusense.UI;
using yajusense.Utils;
using yajusense.VRC;

namespace yajusense;

[BepInPlugin("yajusense", "yajusense", "1.0.0")]
public class Plugin : BasePlugin
{
	public new static ManualLogSource Log;
	public static readonly string ClientDirectory = Path.Combine(Directory.GetCurrentDirectory(), "yajusense");
	private Harmony _harmony;

	public override void Load()
	{
		Log = base.Log;
		Log.LogInfo("Initializing yajusense...");

		FileUtils.EnsureDirectoryExists(ClientDirectory);

		_harmony = new Harmony("yajusense");
		Log.LogInfo("Harmony initialized successfully");

		_harmony.PatchAll();
		Log.LogInfo("Patches applied successfully");

		CursorUnlocker.Init();

		AudioService.Init();

		ConfigManager.Init();

		ModuleManager.Init();

		CoroutineRunner.Initialize(AddComponent<CoroutineRunner>());
		AddComponent<YjMonoBehaviour>();

		Log.LogInfo("yajusense initialized successfully");
	}
}

public class YjMonoBehaviour : MonoBehaviour
{
	public YjMonoBehaviour(IntPtr handle) : base(handle) { }

	private void Update()
	{
		ModuleManager.UpdateModules();
		PlayerTracker.Update();
	}

	private void OnGUI()
	{
		ModuleManager.RenderModules();
		NotificationManager.OnGUI();
	}
}