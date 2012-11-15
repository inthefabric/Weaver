using Fabric.Domain.Graph.Interfaces;
using Fabric.Test.Common.Rels;

namespace Fabric.Test.Common.Nodes {

	/*================================================================================================*/
	public interface IQueryPerson : IWeaverQueryNode {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IQueryPersonLikesCandy OutLikesCandy { get; }
		IQueryPersonKnowsPerson OutKnowsPerson { get; }
		IQueryRootHasPerson InRootHas { get; }
		IQueryPersonKnowsPerson InPersonKnows { get; }

	}

}