## Weaver

Weaver provides a fluent, strongly-typed interface for generating [Gremlin](https://github.com/tinkerpop/gremlin) scripts (for .NET/C#).

This requires that your graph domain classes use Weaver's custom attributes for [vertices](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Core/Elements/WeaverVertexAttribute.cs), [edges](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Core/Elements/WeaverEdgeAttribute.cs), and [properties](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Core/Elements/WeaverPropertyAttribute.cs). The `Weaver.Titan` package includes additional [Titan](https://github.com/thinkaurelius/titan)-specific functionality for creating graph data-types, groups, and indices.

#### Install with NuGet

- [Weaver.Core](https://www.nuget.org/packages/Weaver)
```
PM> Install-Package Weaver
```

- [Weaver.Titan](https://www.nuget.org/packages/Weaver.Titan)
```
PM> Install-Package Weaver.Titan 
```

#### Basic Usage

Weaver converts C# code:
```c#
myWeaverObj.Graph.V.ExactIndex<User>(x => x.Name, "Zach")
```

...into Gremlin script:
```
g.V('U_Na','Zach')
```

The Gremlin script can also be parameterized (enabled by default) to achieve more efficient query compilation on the database side.

#### Fabric Usage Example

Weaver was built to support the [Fabric](https://github.com/inthefabric/Fabric) project, which provides several [useful examples](https://github.com/inthefabric/Fabric/blob/master/Solution/Fabric.Api.Modify/Tasks/ModifyTasks.cs) of Weaver configuration, setup, and usage. 

A slightly-modified example from Fabric [code](https://github.com/inthefabric/Fabric/blob/master/Solution/Fabric.Api.Modify/Tasks/ModifyTasks.cs#L50):
```c#
IWeaverFuncAs<Member> memAlias;

IWeaverQuery q = myWeaverObj.Graph
	.V.ExactIndex<User>(x => x.ArtifactId, 123)
	.DefinesMemberList.ToMember
		.As(out memAlias)
	.InAppDefines.FromApp
		.Has(x => x.ArtifactId, WeaverStepHasOp.EqualTo, pApiCtx.AppId)
	.Back(memAlias)
	.HasMemberTypeAssign.ToMemberTypeAssign
		.Has(x => x.MemberTypeId, WeaverStepHasOp.NotEqualTo, (byte)MemberTypeId.None)
		.Has(x => x.MemberTypeId, WeaverStepHasOp.NotEqualTo, (byte)MemberTypeId.Invite)
		.Has(x => x.MemberTypeId, WeaverStepHasOp.NotEqualTo, (byte)MemberTypeId.Request)
	.Back(memAlias)
	.ToQuery();

SendGremlinRequest(q.Script, q.Params);
```

#### Gremlin Table Support

Weaver provides support for the Gremlin `table` step. Table columns are defined using Weaver's `AsColumn()` step, which support property-selection and customized scripts. Weaver's `Table()` step automatically builds the column clousures using the information provided in the `AsColumn()` steps.

For more details, see the [Weaver Table Support](https://github.com/inthefabric/Weaver/wiki/Weaver-Table-Support) wiki page.

#### Titan-Specific Functionality

As of build 0.5.2, `Weaver.Titan` project provides a variety of functionality that is specific to Titan. This includes:
- Extended [Property](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Titan/Elements/WeaverTitanPropertyAttribute.cs) attribute to support standard, elastic, and vertex-centric indexing
- Extended [Edge](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Titan/Elements/WeaverTitanEdgeAttribute.cs) attribute to support IN- and OUT-uniqueness
- Creating [Type Groups](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Titan/Graph/WeaverTitanGraph.cs#L120)
- Creating [Property Keys](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Titan/Graph/WeaverTitanGraph.cs#L137) using data from Property attributes
- Creating [Edge Labels](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Titan/Graph/WeaverTitanGraph.cs#L178) using data from Property and Edge attributes
- Automatic inclusion of vertex-centric index properties when [adding new edges](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Titan/Graph/WeaverTitanGraph.cs#L38), using data from Property and Edge attributes
- Forming queries using [ElasticSearch indexes](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Titan/Graph/WeaverTitanGraphQuery.cs#L23)
- Performing strongly-typed ["Has" queries](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Titan/Pipe/WeaverTitanPathPipe.cs#L20) against vertex-centric edge properties

#### Graph of the Gods

In the `Weaver.Examples` project, the "[Graph of the Gods](https://github.com/thinkaurelius/titan/wiki/Getting-Started)" graph schema is implemented using Weaver's attributes, and some basic traversals are demonstrated. [View the code](https://github.com/inthefabric/Weaver/tree/master/Solution/Weaver.Examples/Core).


[![githalytics.com alpha](https://cruel-carlota.gopagoda.com/9caca4070a7a2601105b67a6840644c2 "githalytics.com")](http://githalytics.com/inthefabric/Weaver)
