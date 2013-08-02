using Moq;
using NUnit.Framework;
using Weaver.Core.Path;
using Weaver.Core.Pipe;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Core.Steps.Statements;
using Weaver.Test.Common;
using Weaver.Test.Common.Vertices;
using Weaver.Test.WeavTitan.Common;
using System;
using Weaver.Core.Elements;
using System.Linq.Expressions;
using Weaver.Test.Utils;
using Weaver.Core.Exceptions;
using Weaver.Titan.Elements;
using Weaver.Test.Common.Edges;
using Weaver.Core;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Titan.Pipe;

namespace Weaver.Test.WeavTitan.Pipe {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverTitanPathPipe : WeaverTestBase {
	
		public enum HasStepType {
			HasOp,
			HasProp,
			HasNotOp,
			HasNotProp
		};
			
		private Mock<IWeaverPath> vMockPath;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[SetUp]
		protected override void SetUp() {
			base.SetUp();
			
			vMockPath = new Mock<IWeaverPath>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private TEdge DoHasStep<TEdge, TVert>(HasStepType pStepType,
															Expression<Func<TVert, object>> pProperty)
															where TEdge : IWeaverEdge, new()
															where TVert : IWeaverVertex, new() {
			var edge = new TEdge();
			edge.Path = vMockPath.Object;
			
			switch ( pStepType ) {
				case HasStepType.HasOp:
					return edge.HasVci(pProperty, WeaverStepHasOp.EqualTo, 1);
					
				case HasStepType.HasProp:
					return edge.HasVci(pProperty);
					
				case HasStepType.HasNotOp:
					return edge.HasNotVci(pProperty, WeaverStepHasOp.EqualTo, 1);
					
				case HasStepType.HasNotProp:
					return edge.HasNotVci(pProperty);
			}
			
			throw new Exception("Unknown type: "+pStepType);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(HasStepType.HasOp)]
		[TestCase(HasStepType.HasProp)]
		[TestCase(HasStepType.HasNotOp)]
		[TestCase(HasStepType.HasNotProp)]
		public void Has(HasStepType pStepType) {
			OneKnowsTwo result = DoHasStep<OneKnowsTwo, One>(pStepType, x => x.A);
			
			Assert.NotNull(result, "Result should be filled.");
			vMockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepHas<One>>()), Times.Once());
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(HasStepType.HasOp)]
		[TestCase(HasStepType.HasProp)]
		[TestCase(HasStepType.HasNotOp)]
		[TestCase(HasStepType.HasNotProp)]
		public void HasInvalidVertex(HasStepType pStepType) {
			Exception e = WeaverTestUtil.CheckThrows<WeaverException>(true, () =>
				DoHasStep<OneKnowsTwo, Person>(pStepType, x => x.Name));
			
			bool correct = (e.Message.Contains(typeof(Person).Name) && 
				e.Message.Contains(typeof(WeaverTitanVertexAttribute).Name));
			Assert.True(correct, "Incorrect exception: "+e);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(HasStepType.HasOp)]
		[TestCase(HasStepType.HasProp)]
		[TestCase(HasStepType.HasNotOp)]
		[TestCase(HasStepType.HasNotProp)]
		public void HasInvalidEdge(HasStepType pStepType) {
			Exception e = WeaverTestUtil.CheckThrows<WeaverException>(true, () =>
				DoHasStep<PersonKnowsPerson, One>(pStepType, x => x.A));
			
			bool correct = (e.Message.Contains(typeof(PersonKnowsPerson).Name) && 
				e.Message.Contains(typeof(WeaverTitanEdgeAttribute).Name));
			Assert.True(correct, "Incorrect exception: "+e);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(HasStepType.HasOp)]
		[TestCase(HasStepType.HasProp)]
		[TestCase(HasStepType.HasNotOp)]
		[TestCase(HasStepType.HasNotProp)]
		public void HasInvalidProperty(HasStepType pStepType) {
			Exception e = WeaverTestUtil.CheckThrows<WeaverException>(true, () =>
				DoHasStep<OneKnowsThree, Three>(pStepType, x => x.B));
			
			bool correct = (e.Message.Contains(typeof(WeaverTitanPropertyAttribute).Name) && 
				e.Message.Contains("B"));
			Assert.True(correct, "Incorrect exception: "+e);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(HasStepType.HasOp)]
		[TestCase(HasStepType.HasProp)]
		[TestCase(HasStepType.HasNotOp)]
		[TestCase(HasStepType.HasNotProp)]
		public void HasInvalidVertexType(HasStepType pStepType) {
			Exception e = WeaverTestUtil.CheckThrows<WeaverException>(true, () =>
				DoHasStep<OneKnowsTwo, NullableProp>(pStepType, x => x.NonTitanAttribute));
			
			bool correct = (e.Message.Contains(typeof(NullableProp).Name) && 
				e.Message.Contains("not valid for edge type"));
			Assert.True(correct, "Incorrect exception: "+e);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(HasStepType.HasOp)]
		[TestCase(HasStepType.HasProp)]
		[TestCase(HasStepType.HasNotOp)]
		[TestCase(HasStepType.HasNotProp)]
		public void HasNonVciProperty(HasStepType pStepType) {
			Exception e = WeaverTestUtil.CheckThrows<WeaverException>(true, () =>
				DoHasStep<OneKnowsTwo, One>(pStepType, x => x.B));
			
			bool correct = (e.Message.Contains(typeof(OneKnowsTwo).Name) && 
			                e.Message.Contains(typeof(One).Name+".B"));
			Assert.True(correct, "Incorrect exception: "+e);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category(Integration)]
		public void HasVciScript() {
			var wi = new WeaverInstance(
				new [] { typeof(One), typeof(Two) }, new [] { typeof(OneKnowsTwo) });
				
			IWeaverQuery q = wi.Graph
				.V.ExactIndex<Two>(x => x.Id, 10)
				.InOneKnows
					.HasNotVci((One x) => x.A)
					.OutVertex
				.KnowsTwo
					.HasVci((Two x) => x.C, WeaverStepHasOp.GreaterThanOrEqualTo, 2)
					.OutVertex
				.ToQuery();
				
			string expect = "g.V('id',_P0).inE('OKT').hasNot('OA').outV"+
				".outE('OKT').has('TC',Tokens.T.gte,_P1).outV;";
			Assert.AreEqual(expect, q.Script, "Incorrect script.");
		}
		
	}
	
	
	/*================================================================================================*/
	[WeaverTitanVertex]
	public class Three : TitanBase {
		
		[WeaverTitanProperty("TA", EdgesForVertexCentricIndexing = new [] { typeof(OneKnowsTwo) })]
		public int A { get; set; }
		
		[WeaverProperty("TB")]
		public int B { get; set; }
		
	}
	
	
	/*================================================================================================*/
	[WeaverTitanEdge("OKT", WeaverEdgeConn.OutZeroOrOne, typeof(One),
		WeaverEdgeConn.InOne, typeof(Three))]
	public class OneKnowsThree : WeaverEdge<One, Knows, Three> {
		
	}

}