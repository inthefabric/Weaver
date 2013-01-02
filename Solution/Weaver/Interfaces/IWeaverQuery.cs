namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverQuery : IWeaverScript {

		bool IsFinalized { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void FinalizeQuery(string pScript);

		/*--------------------------------------------------------------------------------------------*/
		void AddParam(string pParamName, IWeaverQueryVal pValue);
		string AddParam(IWeaverQueryVal pValue);
		string AddParamIfString(IWeaverQueryVal pValue);
		string NextParamName { get; }

	}

}