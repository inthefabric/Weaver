namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverQuery {

		/*--------------------------------------------------------------------------------------------*/
		void AddParam(string pParamName, string pValue);
		string AddParam(string pValue);
		string NextParamName { get; }

	}

}