using Weaver.Interfaces;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Common.Nodes {

	/*================================================================================================*/
	public interface IQueryRoot : IWeaverQueryNode {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IQueryRootHasCandy OutHasCandy { get; }
		IQueryRootHasPerson OutHasPerson { get; }

	}

}