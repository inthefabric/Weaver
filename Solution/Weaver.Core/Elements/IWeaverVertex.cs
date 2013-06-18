namespace Weaver.Core.Elements {

	/*================================================================================================*/
	public interface IWeaverVertex : IWeaverElement {

		bool IsFromVertex { get; set; }
		bool ExpectOneVertex { get; set; }
		bool IsRoot { get; }

	}

}