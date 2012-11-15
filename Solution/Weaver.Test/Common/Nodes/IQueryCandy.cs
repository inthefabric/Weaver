using Fabric.Domain.Graph.Interfaces;
using Fabric.Test.Common.Rels;

namespace Fabric.Test.Common.Nodes {

	/*================================================================================================*/
	public interface IQueryCandy : IWeaverQueryNode {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IQueryRootHasPerson InRootHas { get; }
		IQueryPersonLikesCandy InPersonLikes { get; }
		
	}

}