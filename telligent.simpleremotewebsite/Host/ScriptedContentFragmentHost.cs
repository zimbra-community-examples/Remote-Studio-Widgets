using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telligent.SimpleRemoteWebSite
{
    public class ScriptedContentFragmentHost : Telligent.Evolution.Extensibility.UI.Version1.RemoteScriptedContentFragmentHost
    {
		private static readonly Guid InstanceId = new Guid("665c41a9ee324fde9b3c44b3425c5d48");

		// The name of the user within Evolution to execute widgets as
		string _serviceUserName = "USERNAME";

		// The user's REST API key for this application
		string _serviceApiKey = "APIKEY";

		// The root URL of Evolution (with trailing forward slash):
		string _evolutionUrl = "URL";

		// this implementation supports only a single host implementation, this static method provides easy access to it
		public static Telligent.Evolution.Extensibility.UI.Version1.RemoteScriptedContentFragmentHost GetInstance()
		{
			return Telligent.Evolution.Extensibility.UI.Version1.RemoteScriptedContentFragmentHost.Get(InstanceId);
		}

		#region RemoteScriptedCOntentFragmentHost Implementation

		public override Guid Id
		{
			get { return InstanceId; }
		}

        public override string CallbackUrl
        {
            get 
			{ 
				// return the location of the Remote Widget Studio handler
				return GetCurrentHttpContext().Response.ApplyAppPathModifier("~/rsw.ashx"); 
			}
        }

        public override string EvolutionRootUrl
        {
            get { return _evolutionUrl; }
        }

		public override void ApplyAuthenticationToHostRequest(System.Net.HttpWebRequest request, bool forAccessingUser)
		{
			// add authentication information for requests back to Evolution (this uses a REST API Key only)

			var token = String.Format("{0}:{1}", _serviceApiKey, _serviceUserName);
			var tokenBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(token));
			request.Headers.Add("Rest-User-Token", tokenBase64);
		}

        public override IEnumerable<Telligent.Evolution.Extensibility.Storage.Version1.IFile> GetSourceFiles()
        {
			// any .XML file in the /Widgets folder will be read as a widget to make available to the site

            List<Telligent.Evolution.Extensibility.Storage.Version1.IFile> files = new List<Telligent.Evolution.Extensibility.Storage.Version1.IFile>();

			foreach (var file in System.IO.Directory.GetFiles(HttpContext.Current.Request.MapPath("~/Widgets"), "*.xml"))
			{
				files.Add(new File(file, HttpContext.Current.Response.ApplyAppPathModifier("~/Widgets/") + System.IO.Path.GetFileName(file)));
			}

            return files;
        }

        public override Telligent.Evolution.Extensibility.Storage.Version1.IFile GetFile(Guid instanceIdentifier, string fileName)
        {
			// return the embedded file from the source file (use the GetFilesFromSourceFile utility)
            return GetFilesFromSourceFile(instanceIdentifier).FirstOrDefault(x => x.FileName == fileName);
        }

        public override string GetAccessingUserLanguageKey()
        {
			// this implemenation doesn't support authenticating users, so everyone is default
            return DefaultLanguageKey;
        }

        public override IEnumerable<Telligent.Evolution.Extensibility.UI.Version1.IScriptedContentFragmentExtension> Extensions
        {
            get 
			{ 
				// there are no customizations, just return the platform extensions
				return PlatformExtensions; 
			}
        }

        public override void LogError(string message, Exception ex)
        {
			try
			{
				var context = HttpContext.Current;
				context.Response.Write("<!--\n" + context.Server.HtmlEncode(message) + "\n" + context.Server.HtmlEncode(ex.ToString()) + "\n-->");
			}
			catch
			{
				// HttpContext wasn't available
			}
        }

        public override string SerializeContext(HttpContextBase context)
        {
			// this implementation does not define any contextual parameters, so there's nothing to serialize
            return string.Empty;
        }

        public override void DeserializeContext(HttpContextBase context, string contextData)
        {
			// this implementation does not define any contextual parameters, so there's nothing to deserialize
        }

        public override string GetConfiguration(string hostIdentifier)
        {
			// this implementation does not support configuration storage
            return string.Empty;
        }

		public override bool GetAccessingUserIsAuthenticated()
		{
			// this implementation does not support authenticating users
			return false;
		}

		#endregion
	}

    public class File : Telligent.Evolution.Extensibility.Storage.Version1.IFile
    {
        System.IO.FileInfo _file;
        string _url;

        public File (string path, string url)
        {
            _file = new System.IO.FileInfo(path);
            _url = url;
        }

        public int ContentLength
        {
            get { return (int) _file.Length; }
        }

        public string FileName
        {
            get { return _file.Name; }
        }

        public System.IO.Stream OpenReadStream()
        {
            return _file.OpenRead();
        }

        public string GetDownloadUrl()
        {
            return _url;
        }
    }
}