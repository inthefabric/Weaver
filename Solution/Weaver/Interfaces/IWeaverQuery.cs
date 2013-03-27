namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverQuery : IWeaverScript {

		bool IsFinalized { get; }
		IWeaverVarAlias ResultVar { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void FinalizeQuery(string pScript);
		void StoreResultAsVar(IWeaverVarAlias pVarAlias);

		/*--------------------------------------------------------------------------------------------*/
		string AddStringParam(string pValue, bool pAllowQuote=true);
		string AddParam(IWeaverQueryVal pValue);

	}

}