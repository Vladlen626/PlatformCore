using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace PlatformCore.Editor.Installer
{
	public sealed class PlatformCoreInstallerWindow : EditorWindow
	{
		private static readonly DependencyEntry[] RequiredDependencies =
		{
			new DependencyEntry("Cinemachine", "com.unity.cinemachine", DependencyType.Required),
			new DependencyEntry("TextMesh Pro", "com.unity.textmeshpro", DependencyType.Required),
			new DependencyEntry("Input System", "com.unity.inputsystem", DependencyType.Required),
			new DependencyEntry("Newtonsoft Json", "com.unity.nuget.newtonsoft-json", DependencyType.Required),
			new DependencyEntry("UniTask", "com.cysharp.unitask", DependencyType.Required),
			new DependencyEntry("PrimeTween", "com.kyrylokuzyk.primetween", DependencyType.Required),
		};

		private static readonly DependencyEntry[] OptionalDependencies =
		{
			new DependencyEntry("FishNet", "com.firstgeargames.fishnet", DependencyType.Optional),
		};

		private ListRequest _listRequest;
		private AddRequest _addRequest;
		private string _currentAddPackageId;
		private readonly Queue<DependencyEntry> _queue = new Queue<DependencyEntry>();
		private readonly Dictionary<string, string> _statuses = new Dictionary<string, string>();
		private bool _installing;

		[MenuItem("Len/Installer")]
		private static void OpenWindow()
		{
			var window = GetWindow<PlatformCoreInstallerWindow>("PlatformCore Installer");
			window.minSize = new Vector2(520f, 360f);
			window.RefreshPackages();
		}

		private void OnEnable()
		{
			RefreshPackages();
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Len PlatformCore Installer", EditorStyles.boldLabel);
			EditorGUILayout.Space();

			DrawDependencyBlock("Required", RequiredDependencies);
			EditorGUILayout.Space();
			DrawDependencyBlock("Optional", OptionalDependencies);
			EditorGUILayout.Space();
			DrawPlatformUIFoundationBlock();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Manual / External Setup", EditorStyles.boldLabel);
			EditorGUILayout.HelpBox("FMOD is optional and not auto-installed. Audio service works in no-op mode without FMOD/FMOD_PRESENT.", MessageType.Info);
			EditorGUILayout.Space();

			using (new EditorGUI.DisabledScope(_installing))
			{
				if (GUILayout.Button("Install Required Dependencies"))
				{
					EnqueueMissing(RequiredDependencies);
				}

				if (GUILayout.Button("Install Optional Dependencies"))
				{
					EnqueueMissing(OptionalDependencies);
				}
			}

			if (GUILayout.Button("Validate Setup"))
			{
				RefreshPackages();
			}

			if (_installing)
			{
				EditorGUILayout.HelpBox("Installing dependencies...", MessageType.None);
			}

			HandleRequests();
		}

		private void DrawDependencyBlock(string blockName, IEnumerable<DependencyEntry> entries)
		{
			EditorGUILayout.LabelField(blockName + " Dependencies", EditorStyles.boldLabel);
			foreach (var entry in entries)
			{
				var key = entry.PackageId;
				if (!_statuses.TryGetValue(key, out var status))
				{
					status = "Unknown";
				}

				EditorGUILayout.LabelField($"- {entry.Name}", status);
			}
		}

		private void DrawPlatformUIFoundationBlock()
		{
			EditorGUILayout.LabelField("Platform UI Foundation", EditorStyles.boldLabel);
			var status = PlatformUIFoundationInstaller.GetStatus(out var details, out _, out _);

			var messageType = MessageType.Info;
			switch (status)
			{
				case PlatformUIFoundationInstallStatus.TemplateMissing:
					messageType = MessageType.Error;
					break;
				case PlatformUIFoundationInstallStatus.Missing:
				case PlatformUIFoundationInstallStatus.InstalledUnknownVersion:
				case PlatformUIFoundationInstallStatus.InstalledOutdated:
					messageType = MessageType.Warning;
					break;
				case PlatformUIFoundationInstallStatus.InstalledCurrent:
					messageType = MessageType.Info;
					break;
			}

			EditorGUILayout.HelpBox(details, messageType);

			using (new EditorGUI.DisabledScope(_installing || status == PlatformUIFoundationInstallStatus.TemplateMissing))
			{
				if (GUILayout.Button("Install / Update Platform UI Foundation (Recommended)"))
				{
					PlatformUIFoundationInstaller.InstallOrUpdate(true);
				}

				if (GUILayout.Button("Install Missing Files Only"))
				{
					PlatformUIFoundationInstaller.InstallOrUpdate(false);
				}
			}
		}

		private void RefreshPackages()
		{
			if (_listRequest != null && !_listRequest.IsCompleted)
			{
				return;
			}

			_listRequest = Client.List(true);
		}

		private void EnqueueMissing(IEnumerable<DependencyEntry> dependencies)
		{
			foreach (var dependency in dependencies)
			{
				if (_statuses.TryGetValue(dependency.PackageId, out var status) && status.StartsWith("Installed", StringComparison.Ordinal))
				{
					continue;
				}

				if (_statuses.TryGetValue(dependency.PackageId, out var queuedStatus) && queuedStatus == "Queued")
				{
					continue;
				}

				_queue.Enqueue(dependency);
				_statuses[dependency.PackageId] = "Queued";
			}

			TryInstallNext();
		}

		private void TryInstallNext()
		{
			if (_addRequest != null)
			{
				return;
			}

			if (_queue.Count == 0)
			{
				_installing = false;
				return;
			}

			var entry = _queue.Dequeue();
			_installing = true;
			_statuses[entry.PackageId] = "Installing...";
			_currentAddPackageId = entry.PackageId;
			_addRequest = Client.Add(entry.PackageId);
		}

		private void HandleRequests()
		{
			if (_listRequest != null && _listRequest.IsCompleted)
			{
				if (_listRequest.Status == StatusCode.Success)
				{
					UpdateStatusesFromList(_listRequest.Result);
				}
				else
				{
					Debug.LogWarning($"[PlatformCoreInstaller] Package list failed: {_listRequest.Error?.message}");
				}

				_listRequest = null;
				Repaint();
			}

			if (_addRequest != null && _addRequest.IsCompleted)
			{
				if (_addRequest.Status == StatusCode.Success)
				{
					_statuses[_addRequest.Result.name] = $"Installed ({_addRequest.Result.version})";
				}
				else
				{
					var packageId = string.IsNullOrWhiteSpace(_currentAddPackageId) ? "unknown" : _currentAddPackageId;
					_statuses[packageId] = $"Failed: {_addRequest.Error?.message}";
					Debug.LogWarning($"[PlatformCoreInstaller] Install failed for {packageId}: {_addRequest.Error?.message}");
				}

				_addRequest = null;
				_currentAddPackageId = null;
				RefreshPackages();
				TryInstallNext();
				Repaint();
			}
		}

		private void UpdateStatusesFromList(PackageCollection packages)
		{
			foreach (var entry in RequiredDependencies)
			{
				UpdateStatus(packages, entry);
			}

			foreach (var entry in OptionalDependencies)
			{
				UpdateStatus(packages, entry);
			}
		}

		private void UpdateStatus(PackageCollection packages, DependencyEntry entry)
		{
			foreach (var package in packages)
			{
				if (!string.Equals(package.name, entry.PackageId, StringComparison.Ordinal))
				{
					continue;
				}

				_statuses[entry.PackageId] = $"Installed ({package.version})";
				return;
			}

			_statuses[entry.PackageId] = entry.Type == DependencyType.Required ? "Missing (Required)" : "Missing (Optional)";
		}

		private readonly struct DependencyEntry
		{
			public DependencyEntry(string name, string packageId, DependencyType type)
			{
				Name = name;
				PackageId = packageId;
				Type = type;
			}

			public string Name { get; }
			public string PackageId { get; }
			public DependencyType Type { get; }
		}

		private enum DependencyType
		{
			Required,
			Optional,
		}
	}

	internal enum PlatformUIFoundationInstallStatus
	{
		TemplateMissing,
		Missing,
		InstalledUnknownVersion,
		InstalledOutdated,
		InstalledCurrent,
	}

	internal static class PlatformUIFoundationInstaller
	{
		private const string TemplateAssetsRelativePath = "Templates~/PlatformUIFoundation/Assets";
		private const string TemplateVersionRelativePath = "Templates~/PlatformUIFoundation/ui-foundation.version.txt";
		private const string InstalledVersionFilePath = "Assets/PlatformCore.Generated/InstallState/platform_ui_foundation.version.txt";

		private static readonly string[] RequiredProjectFiles =
		{
			"Assets/Resources/UI/ColorStyleLibrary.asset",
			"Assets/Resources/UI/TextStyleLibrary.asset",
			"Assets/Resources/UI/ElementBackground.prefab",
			"Assets/Resources/UI/UIGlobalNotificationView.prefab",
			"Assets/Resources/UI/UINotificationsView.prefab",
			"Assets/Resources/UI/UINotificationView.prefab",
			"Assets/PlatformCore/UIFoundation/Sprites/BackgroundCubeWhite.png",
			"Assets/PlatformCore/UIFoundation/Fonts/Roboto-Bold SDF.asset",
			"Assets/PlatformCore/UIFoundation/Fonts/Roboto-Bold.ttf",
		};

		public static PlatformUIFoundationInstallStatus GetStatus(
			out string details,
			out string templateVersion,
			out string installedVersion)
		{
			templateVersion = GetTemplateVersion();
			installedVersion = GetInstalledVersion();

			var sourceRoot = GetTemplateAssetsRoot();
			if (string.IsNullOrWhiteSpace(sourceRoot) || !Directory.Exists(sourceRoot))
			{
				details = "Template assets are missing in package.";
				return PlatformUIFoundationInstallStatus.TemplateMissing;
			}

			var allFilesPresent = true;
			for (var i = 0; i < RequiredProjectFiles.Length; i++)
			{
				if (!File.Exists(RequiredProjectFiles[i]))
				{
					allFilesPresent = false;
					break;
				}
			}

			if (!allFilesPresent)
			{
				details = "Foundation assets are not installed.";
				return PlatformUIFoundationInstallStatus.Missing;
			}

			if (string.IsNullOrWhiteSpace(installedVersion))
			{
				details = "Installed, but version marker is missing.";
				return PlatformUIFoundationInstallStatus.InstalledUnknownVersion;
			}

			if (!string.Equals(installedVersion, templateVersion, StringComparison.Ordinal))
			{
				details = $"Outdated: installed {installedVersion}, package {templateVersion}.";
				return PlatformUIFoundationInstallStatus.InstalledOutdated;
			}

			details = $"Installed (version {installedVersion}).";
			return PlatformUIFoundationInstallStatus.InstalledCurrent;
		}

		public static bool InstallOrUpdate(bool overwriteExisting)
		{
			var sourceRoot = GetTemplateAssetsRoot();
			if (string.IsNullOrWhiteSpace(sourceRoot) || !Directory.Exists(sourceRoot))
			{
				Debug.LogError("[PlatformUIFoundationInstaller] Template assets are missing in package.");
				return false;
			}

			try
			{
				var sourceFiles = Directory.GetFiles(sourceRoot, "*", SearchOption.AllDirectories);
				for (var i = 0; i < sourceFiles.Length; i++)
				{
					var sourcePath = sourceFiles[i];
					if (sourcePath.EndsWith(".meta", StringComparison.OrdinalIgnoreCase))
					{
						continue;
					}

					var relativePath = sourcePath.Substring(sourceRoot.Length + 1).Replace('\\', '/');
					var destinationPath = Path.Combine("Assets", relativePath).Replace('\\', '/');

					CopyAssetFileWithMeta(sourcePath, destinationPath, overwriteExisting);
				}

				WriteInstalledVersionMarker(GetTemplateVersion());
				AssetDatabase.Refresh();
				Debug.Log("[PlatformUIFoundationInstaller] Platform UI foundation installed.");
				return true;
			}
			catch (Exception exception)
			{
				Debug.LogError($"[PlatformUIFoundationInstaller] Install failed: {exception}");
				return false;
			}
		}

		private static string GetTemplateAssetsRoot()
		{
			var packageRoot = GetPackageRoot();
			if (string.IsNullOrWhiteSpace(packageRoot))
			{
				return null;
			}

			return Path.Combine(packageRoot, TemplateAssetsRelativePath);
		}

		private static string GetTemplateVersion()
		{
			var packageRoot = GetPackageRoot();
			if (string.IsNullOrWhiteSpace(packageRoot))
			{
				return string.Empty;
			}

			var versionPath = Path.Combine(packageRoot, TemplateVersionRelativePath);
			if (!File.Exists(versionPath))
			{
				return string.Empty;
			}

			return File.ReadAllText(versionPath).Trim();
		}

		private static string GetInstalledVersion()
		{
			if (!File.Exists(InstalledVersionFilePath))
			{
				return string.Empty;
			}

			return File.ReadAllText(InstalledVersionFilePath).Trim();
		}

		private static void WriteInstalledVersionMarker(string version)
		{
			var directory = Path.GetDirectoryName(InstalledVersionFilePath);
			if (!string.IsNullOrWhiteSpace(directory))
			{
				Directory.CreateDirectory(directory);
			}

			File.WriteAllText(InstalledVersionFilePath, string.IsNullOrWhiteSpace(version) ? "unknown" : version);
		}

		private static string GetPackageRoot()
		{
			var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(PlatformUIFoundationInstaller).Assembly);
			return packageInfo?.resolvedPath;
		}

		private static void CopyAssetFileWithMeta(string sourcePath, string destinationPath, bool overwriteExisting)
		{
			var destinationDirectory = Path.GetDirectoryName(destinationPath);
			if (!string.IsNullOrWhiteSpace(destinationDirectory))
			{
				Directory.CreateDirectory(destinationDirectory);
			}

			var destinationExists = File.Exists(destinationPath);
			if (destinationExists && !overwriteExisting)
			{
				var sourceMeta = sourcePath + ".meta";
				var destinationMeta = destinationPath + ".meta";
				if (File.Exists(sourceMeta) && !File.Exists(destinationMeta))
				{
					File.Copy(sourceMeta, destinationMeta, true);
				}

				return;
			}

			File.Copy(sourcePath, destinationPath, true);

			var sourceMetaPath = sourcePath + ".meta";
			if (File.Exists(sourceMetaPath))
			{
				File.Copy(sourceMetaPath, destinationPath + ".meta", true);
			}
		}
	}
}
