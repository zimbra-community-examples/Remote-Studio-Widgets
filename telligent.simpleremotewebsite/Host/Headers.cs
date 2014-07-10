using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Telligent.SimpleRemoteWebSite
{
	public class Headers : Control
	{
		public bool IsModal
		{
			get { return (bool)(ViewState["IsModal"] ?? false); }
			set { ViewState["IsModal"] = value; }
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			Controls.Add(new LiteralControl(ScriptedContentFragmentHost.GetInstance().GetHeaders(IsModal)));
		}
	}
}