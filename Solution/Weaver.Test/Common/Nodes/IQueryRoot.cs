using Fabric.Domain.Graph.Interfaces;
using Fabric.Test.Common.Rels;

namespace Fabric.Test.Common.Nodes {

	/*================================================================================================*/
	public interface IQueryRoot : IWeaverQueryNode {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IQueryRootHasCandy OutHasCandy { get; }
		IQueryRootHasPerson OutHasPerson { get; }

	}

}