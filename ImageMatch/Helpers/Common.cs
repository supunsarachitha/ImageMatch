using System;
using System.IO;
using System.Reflection;
using Plugin.SimpleAudioPlayer;

namespace ImageMatch.Helpers
{
	public class Common
	{
		public static string WinToneURL = "https://files.freemusicarchive.org/storage-freemusicarchive-org/music/no_curator/Yung_Kartz/July_2019/Yung_Kartz_-_02_-_Levels.mp3";

		public static string FailToneURL = "";

		public static int MaxFailAttemptCount = 5;

		public static int GridColumnCount =0;

		public static int GridRowCount = 0;

		public static string GamePageBackGroundImageURL = "https://www.iconfinder.com/icons/3305207/download/png/128";

		public static ISimpleAudioPlayer AudioPlayer= Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;

		public static string RemoteImageUrl = "https://api.jsonbin.io/b/6269cfef25069545a32a216f";



		public static void PlaySound(string name)
		{
			var stream = GetStreamFromFile(name);

			Common.AudioPlayer.Load(stream);
			Common.AudioPlayer.Play();
		}

		public static Stream GetStreamFromFile(string filename)
		{
			var assembly = typeof(App).GetTypeInfo().Assembly;
			var stream = assembly.GetManifestResourceStream("ImageMatch.Audio." + filename);
			return stream;
		}

	}
}

