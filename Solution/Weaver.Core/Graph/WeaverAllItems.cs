﻿using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Path;
using Weaver.Core.Steps;

namespace Weaver.Core.Graph {

	/*================================================================================================*/
	public abstract class WeaverAllItems : WeaverPathItem, IWeaverAllItems {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected T FromIndex<T>(Expression<Func<T, object>> pProperty, object pExactValue)
																where T : IWeaverElement, new() {
			var ei = new WeaverStepExactIndex<T>(pProperty, pExactValue);
			Path.AddItem(ei);
			return new T { Path = Path };
		}

	}

}