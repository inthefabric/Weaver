namespace Weaver.Core.Query {
	
	/*================================================================================================*/
	public interface IWeaverQueryVal {

		object Original { get; }

		string RawText { get; }
		string FixedText { get; }
		//string NumericSuffixText { get; }
		bool IsString { get; }

	}

}