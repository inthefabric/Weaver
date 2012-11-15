using Weaver.Interfaces;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Common.Nodes {

	/*================================================================================================*/
	public interface IQueryCandy : IWeaverQueryNode {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IQueryRootHasPerson InRootHas { get; }
		IQueryPersonLikesCandy InPersonLikes { get; }
		
	}

}