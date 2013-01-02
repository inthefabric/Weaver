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
		void AddParam(string pParamName, IWeaverQueryVal pValue);
		string AddParam(IWeaverQueryVal pValue);
		string AddParamIfString(IWeaverQueryVal pValue);
		string NextParamName { get; }

	}

}