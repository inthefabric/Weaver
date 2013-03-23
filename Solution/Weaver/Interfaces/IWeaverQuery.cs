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
		string AddParam(IWeaverQueryVal pValue);
		string AddParamIfString(IWeaverQueryVal pValue);
		string NextParamName { get; }

	}

}