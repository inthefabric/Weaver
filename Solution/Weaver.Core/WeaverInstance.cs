using System.Collections.Generic;
using Weaver.Core.Graph;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Schema;

namespace Weaver.Core {
	
	/*================================================================================================*/
	public class WeaverInstance : IWeaverInstance {

		public WeaverConfig Config { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverInstance(IList<WeaverVertexSchema> pVertexs, IList<WeaverEdgeSchema> pEdges) {
			Config = new WeaverConfig(pVertexs, pEdges);
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

	}

}