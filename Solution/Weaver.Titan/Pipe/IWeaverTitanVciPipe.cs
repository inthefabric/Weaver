using Weaver.Core.Elements;
using Weaver.Core.Path;

namespace Weaver.Core.Pipe {
	
	/*================================================================================================*/
	public interface IWeaverTitanVciPipe<TEdge, TVertex, TOutV, TInV> : IWeaverPathItem
								where TEdge : IWeaverEdge<TOutV, TInV> where TVertex : IWeaverVertex
								where TOutV : IWeaverVertex where TInV : IWeaverVertex {
		
		TEdge Edge { get; }
		TVertex Vertex { get; }
		
	}

}