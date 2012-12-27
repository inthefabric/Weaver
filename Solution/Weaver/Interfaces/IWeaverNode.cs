namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverNode : IWeaverItemIndexable {

		long Id { get; set; }
		bool IsFromNode { get; set; }
		bool ExpectOneNode { get; set; }
		bool IsRoot { get; }

	}

}