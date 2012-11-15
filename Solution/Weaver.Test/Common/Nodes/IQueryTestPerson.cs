using Fabric.Domain.Graph.Interfaces;
using Fabric.Test.Common.Rels;

namespace Fabric.Test.Common.Nodes {

	/*================================================================================================*/
	public interface IQueryTestPerson : IWeaverQueryNode {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		TestPersonLikesCandy OutLikesCandy { get; }
		TestPersonKnowsPerson OutKnowsPerson { get; }
		TestRootHasPerson InRootHas { get; }
		TestPersonKnowsPerson InPersonKnows { get; }

	}

}