using Weaver.Core.Elements;
using Weaver.Core.Path;

namespace Weaver.Core.Pipe {
	
	/*================================================================================================*/
	public interface IWeaverTitanVciPipe<TEdge, TVertex> : IWeaverPathItem
											where TEdge : IWeaverEdge where TVertex : IWeaverVertex {
		
		TEdge Edge { get; }
		TVertex Vertex { get; }
		
	}

}