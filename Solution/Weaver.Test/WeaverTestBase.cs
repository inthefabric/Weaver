﻿using NUnit.Framework;
using Weaver.Core;
using Weaver.Test.Common;
using Weaver.Test.Common.Schema;

namespace Weaver.Test {

	/*================================================================================================*/
	public abstract class WeaverTestBase {

		protected TestSchema Schema { get; private set; }
		protected WeaverInstance WeavInst { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[SetUp]
		protected virtual void SetUp() {
			Schema = new TestSchema();
			WeavInst = new WeaverInstance(ConfigHelper.VertexTypes, ConfigHelper.EdgeTypes);
		}

		/*--------------------------------------------------------------------------------------------* /
		[TearDown]
		public virtual void TearDown() {}*/

	}

}