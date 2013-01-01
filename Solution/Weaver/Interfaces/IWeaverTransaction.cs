using System.Collections.Generic;
using Weaver;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverTransaction {
	
		string Script { get; set; }
		Dictionary<string, string> Params { get; set; }
		WeaverTransaction.ConclusionType Conclusion { get; set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void AddQuery(IWeaverQuery pQuery);
		string GetNextVarName();
		void Finish(WeaverTransaction.ConclusionType pConclusion, IWeaverListVar pFinalOutput=null);
		
	}

}