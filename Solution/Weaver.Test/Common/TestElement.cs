using System.Collections.Generic;
using Moq;
using Weaver.Core.Elements;
using Weaver.Core.Path;

namespace Weaver.Test.Common {

	/*================================================================================================*/
	public class TestElement : WeaverElement {

		public Mock<IWeaverPath> MockPath { get; private set; }

		[WeaverProperty("Val")]
		public int Value { get; set; }

		public IList<IWeaverPathItem> PathItems { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestElement() {
			PathItems = new List<IWeaverPathItem>();

			MockPath = new Mock<IWeaverPath>();

			MockPath
				.Setup(x => x.AddItem(It.IsAny<IWeaverPathItem>()))
				.Callback((IWeaverPathItem x) => PathItems.Add(x));

			Path = MockPath.Object;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return null;
		}

	}

}