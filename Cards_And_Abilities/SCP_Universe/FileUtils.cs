using System.Reflection;
using BepInEx;
using static SCP_Universe.SCP_Plugin;

namespace SCP_Universe;

public static class FileUtils
{
	private static readonly string ScpPluginsDir = GetDir();

	private static readonly string[] FilesToSearch = Directory.GetFiles(
		ScpPluginsDir, "*", SearchOption.AllDirectories
	);

	private static string GetDir()
	{
		return EnableHotReload
			? Path.Combine(Paths.BepInExRootPath, "plugins/SCP_Universe")
			: Assembly.GetExecutingAssembly().Location.Replace("SCP_Universe.dll", "");
	}

	public static byte[] ReadFileAsBytes(string file)
	{
		return File.ReadAllBytes(FindFileInPluginDir(file));
	}

	public static string FindFileInPluginDir(string file)
	{
		Log.LogDebug($"Looking for file [{file}]");
		try
		{
			return FilesToSearch.Single(str => Path.GetFileName(str) == file);
		}
		catch (InvalidOperationException e)
		{
			Log.LogError($"Unable to find file [{Path.GetFileName(file)}] in directory [{ScpPluginsDir}] ! " +
			             $"Are you sure you have this file in the same directory as the .dll?");
			throw;
		}
	}
}
