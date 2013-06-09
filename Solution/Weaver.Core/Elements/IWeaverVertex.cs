namespace Weaver.Core.Elements {

	/*================================================================================================*/
	public interface IWeaverVertex : IWeaverElement {

		bool IsFromNode { get; set; }
		bool ExpectOneNode { get; set; }
		bool IsRoot { get; }

	}

}