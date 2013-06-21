using System;
using System.Collections.Generic;
using Weaver.Core.Elements;
using Weaver.Core.Graph;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;

namespace Weaver.Core {
	
	/*================================================================================================*/
	public class WeaverInstance : IWeaverInstance {

		public WeaverConfig Config { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverInstance(IList<Type> pVertexTypes, IList<Type> pEdgeTypes) {
			Config = new WeaverConfig(pVertexTypes, pEdgeTypes);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverGraph Graph {
			get {
				var path = new WeaverPath(Config, new WeaverQuery());
				var g = new WeaverGraph();
				path.AddItem(g);
				return g;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public T FromVar<T>(IWeaverVarAlias<T> pAlias) where T : IWeaverElement, new() {
			var sc = new WeaverStepCustom(pAlias.Name);

			var path = new WeaverPath(Config, new WeaverQuery());
			path.AddItem(sc);

			return new T { Path = path };
		}

	}

}