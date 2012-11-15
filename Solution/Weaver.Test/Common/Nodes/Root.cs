﻿using Weaver.Items;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Common.Nodes {

	/*================================================================================================*/
	public class Root : TestNode, IQueryRoot {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Root() { }

		/*--------------------------------------------------------------------------------------------*/
		public Root(bool pIsFromNode, bool pExpectOneNode) :
															base(true, pIsFromNode, pExpectOneNode) {}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IQueryRootHasCandy OutHasCandy {
			get { return new RootHasCandy(WeaverRelConn.OutToManyNodes); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public IQueryRootHasPerson OutHasPerson {
			get { return new RootHasPerson(WeaverRelConn.OutToManyNodes); }
		}

	}

}