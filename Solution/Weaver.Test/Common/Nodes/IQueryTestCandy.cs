using Fabric.Domain.Graph.Interfaces;
using Fabric.Test.Common.Rels;

namespace Fabric.Test.Common.Nodes {

	/*================================================================================================*/
	public interface IQueryTestCandy : IWeaverQueryNode {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		TestRootHasPerson InRootHas { get; }
		TestPersonLikesCandy InPersonLikes { get; }
		
	}

}