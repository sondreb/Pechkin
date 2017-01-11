using Pechkin;
using Pechkin.Synchronized;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PechkinTests
{
	public class HtmlToPdfTests
	{
		[Fact(DisplayName = "Embedded images")]
		public void GeneratePdfFromEmbeddedHtml()
		{
			var html = GetResourceString("PechkinTests.Resources.embedded.html");
			Assert.NotNull(html);

			SimplePechkin pechin = new SimplePechkin(new GlobalConfig());
			byte[] result = pechin.Convert(html);

			//File.WriteAllBytes(@"C:\temp\embedded.pdf", result);

			Assert.NotNull(result);

			pechin.Dispose();
		}

		[Fact(DisplayName = "Embedded images (synchronized)")]
		public void GeneratePdfFromEmbeddedHtmlSync()
		{
			var html = GetResourceString("PechkinTests.Resources.embedded.html");
			Assert.NotNull(html);

			var globalConfig = new GlobalConfig();
			var objectConfig = new ObjectConfig();

			objectConfig.SetPrintBackground(true);

			SynchronizedPechkin pechin = new SynchronizedPechkin(globalConfig);
			byte[] result = pechin.Convert(objectConfig, html);

			//File.WriteAllBytes(@"C:\temp\embedded.pdf", result);

			Assert.NotNull(result);

			SynchronizedPechkin.ClearBeforeExit();
		}

		public static string GetResourceString(string name)
		{
			if (name == null)
				return null;

			Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
			if (s == null)
				return null;

			return new StreamReader(s).ReadToEnd();
		}
	}
}
