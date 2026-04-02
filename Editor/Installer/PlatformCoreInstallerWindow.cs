using System;
using System.Collections.Generic;
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
		};

		private static readonly DependencyEntry[] OptionalDependencies =
		{
			new DependencyEntry("FishNet", "com.firstgeargames.fishnet", DependencyType.Optional),
		};

		private ListRequest _listRequest;
		private AddRequest _addRequest;
		private readonly Queue<DependencyEntry> _queue = new Queue<DependencyEntry>();
		private readonly Dictionary<string, string> _statuses = new Dictionary<string, string>();
		private bool _installing;

		[MenuItem("Tools/Len/PlatformCore/Installer")]
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

			EditorGUILayout.LabelField("Manual / External Setup", EditorStyles.boldLabel);
			EditorGUILayout.HelpBox("FMOD and PrimeTween are not auto-installed. Configure them manually if your project uses these integrations.", MessageType.Info);
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

				EditorGUILayout.LabelField($"• {entry.Name}", status);
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
					var packageId = _addRequest.PackageIdOrName;
					_statuses[packageId] = $"Failed: {_addRequest.Error?.message}";
					Debug.LogWarning($"[PlatformCoreInstaller] Install failed for {packageId}: {_addRequest.Error?.message}");
				}

				_addRequest = null;
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
}
