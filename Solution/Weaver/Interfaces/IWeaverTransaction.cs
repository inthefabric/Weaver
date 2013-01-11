namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverTransaction : IWeaverScript {
	
		WeaverTransaction.ConclusionType Conclusion { get; set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void AddQuery(IWeaverQuery pQuery);
		string GetNextVarName();
		void Finish(WeaverTransaction.ConclusionType pConclusion, IWeaverVarAlias pFinalOutput=null);
		void FinishWithoutStartStop(IWeaverVarAlias pFinalOutput=null);
		
	}

}