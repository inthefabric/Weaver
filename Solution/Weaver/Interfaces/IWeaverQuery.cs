namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverQuery {

		/*--------------------------------------------------------------------------------------------*/
		void AddParam(string pParamName, WeaverQueryVal pValue);
		string AddParam(WeaverQueryVal pValue);
		string AddParamIfString(WeaverQueryVal pValue);
		string NextParamName { get; }

	}

}