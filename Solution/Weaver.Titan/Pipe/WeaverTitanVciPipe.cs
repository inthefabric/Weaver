using Weaver.Core.Elements;
using Weaver.Core.Path;
using System;
using Weaver.Core.Util;
using Weaver.Titan.Elements;

namespace Weaver.Core.Pipe {
	
	/*================================================================================================*/
	public class WeaverTitanVciPipe<TEdge, TVertex> : WeaverPathItem, 
										IWeaverTitanVciPipe<TEdge, TVertex> where TEdge : IWeaverEdge
										where TVertex : IWeaverVertex {
		
		public TEdge Edge { get; private set; }
		public TVertex Vertex { get; private set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverTitanVciPipe(TEdge pEdge, TVertex pVertex) {
			WeaverTitanUtil.GetAndVerifyElementAttribute<WeaverTitanEdgeAttribute>(typeof(TEdge));
			WeaverTitanUtil.GetAndVerifyElementAttribute<WeaverTitanVertexAttribute>(typeof(TVertex));
			
			Edge = pEdge;
			Vertex = pVertex;
			Path = pEdge.Path;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString () {
			return "";
		}

	}

}