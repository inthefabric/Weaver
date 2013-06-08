namespace Weaver.Core.Query {

	/*================================================================================================*/
	public interface IWeaverTransaction : IWeaverScript {
	
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery AddQuery(IWeaverQuery pQuery);
		string GetNextVarName();
		IWeaverTransaction Finish(IWeaverVarAlias pFinalOutput=null);
		
	}

}