using System.Collections.Generic;

namespace Weaver.Core.Query {

	/*================================================================================================*/
	public interface IWeaverTransaction : IWeaverScript {
	
		IList<IWeaverQuery> Queries { get; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery AddQuery(IWeaverQuery pQuery);
		string GetNextVarName();
		IWeaverTransaction Finish(IWeaverVarAlias pFinalOutput=null);
		
	}

}