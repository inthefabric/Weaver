namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverTransaction : IWeaverScript {
	
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery AddQuery(IWeaverQuery pQuery);
		string GetNextVarName();
		IWeaverTransaction Finish(IWeaverVarAlias pFinalOutput=null);
		
	}

}