namespace Weaver.Core.Elements {

	/*================================================================================================*/
	public interface IWeaverVertex : IWeaverElement {

		bool IsFromNode { get; set; }
		bool ExpectOneNode { get; set; }
		bool IsRoot { get; }

	}


	/*================================================================================================*/
	public interface IWeaverVertex<T> : IWeaverVertex, IWeaverElement<T> where T : class, IWeaverVertex{

	}

}