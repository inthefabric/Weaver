namespace Weaver.Core.Query {

	/*================================================================================================*/
	public interface IWeaverQuery : IWeaverScript {

		bool IsFinalized { get; }
		IWeaverVarAlias ResultVar { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void FinalizeQuery(string pScript);

		/*--------------------------------------------------------------------------------------------*/
		string AddStringParam(string pValue);
		string AddParam(IWeaverQueryVal pValue);

	}

}