namespace Fabric.Domain.Graph.Interfaces {

	/*================================================================================================*/
	public interface IWeaverNode : IWeaverItem {

		bool IsFromNode { get; set; }
		bool ExpectOneNode { get; set; }
		bool IsRoot { get; }

	}

}