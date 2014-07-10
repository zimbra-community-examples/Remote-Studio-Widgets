using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Telligent.SimpleRemoteWebSite
{
	public class Widget : Control
	{
		public Guid InstanceIdentifier
		{
			get { return (Guid)(ViewState["InstanceIdentifier"] ?? Guid.Empty); }
			set { ViewState["InstanceIdentifier"] = value; }
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			if (InstanceIdentifier != Guid.Empty)
				ScriptedContentFragmentHost.GetInstance().Render(ClientID, InstanceIdentifier, this);
		}
	}
}