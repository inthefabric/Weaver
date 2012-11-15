using Weaver.Interfaces;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Common.Nodes {

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