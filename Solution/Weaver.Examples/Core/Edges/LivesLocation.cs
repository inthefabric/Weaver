using Weaver.Core.Elements;
using Weaver.Examples.Core.EdgeTypes;
using Weaver.Examples.Core.Vertices;

namespace Weaver.Examples.Core.Edges {

	/*================================================================================================*/
	public abstract class LivesLocation<TOut> : WeaverEdge<TOut, Lives, Location> 
																	where TOut : IWeaverVertex, new() {
		
		[WeaverProperty("reason")]
		public string Reason { get; set; }
		
	}

}