﻿using Weaver.Core.Path;

namespace Weaver.Core.Elements {

	/*================================================================================================*/
	public abstract class WeaverElement : WeaverPathItem, IWeaverElement {

		[WeaverItemProperty]
		public string Id { get; set; }

	}

}