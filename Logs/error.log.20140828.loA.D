2014-08-28 09:21:44,020 ERROR ParTech.ImageLibrary.Core.Repositories.ObjectRepository - dada1967 - GetCategoryAndMapToCategoryModel - error [There was an error deserializing the object of type ParTech.ImageLibrary.Core.ViewModels.Manage.MultiLingualListModel. Data at the root level is invalid. Line 1, position 1.] - - 
    at System.Runtime.Serialization.XmlObjectSerializer.ReadObjectHandleExceptions(XmlReaderDelegator reader, Boolean verifyObjectName, DataContractResolver dataContractResolver)
   at System.Runtime.Serialization.Json.DataContractJsonSerializer.ReadObject(XmlReader reader)
   at ParTech.ImageLibrary.Core.Repositories.ObjectRepository.GetCategoryAndMapToCategoryModel(Nullable`1 categoryid) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\ObjectRepository.cs:line 333 


2014-08-28 09:23:32,045 ERROR ParTech.ImageLibrary.Core.Repositories.ObjectRepository - dada1967 - GetCategoryAndMapToCategoryModel - error [There was an error deserializing the object of type ParTech.ImageLibrary.Core.ViewModels.Manage.MultiLingualListModel. Data at the root level is invalid. Line 1, position 1.] - - 
    at System.Runtime.Serialization.XmlObjectSerializer.ReadObjectHandleExceptions(XmlReaderDelegator reader, Boolean verifyObjectName, DataContractResolver dataContractResolver)
   at System.Runtime.Serialization.Json.DataContractJsonSerializer.ReadObject(XmlReader reader)
   at ParTech.ImageLibrary.Core.Repositories.ObjectRepository.GetCategoryAndMapToCategoryModel(Nullable`1 categoryid) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\ObjectRepository.cs:line 333 


2014-08-28 09:24:53,800 ERROR ParTech.ImageLibrary.Core.Repositories.ObjectRepository - dada1967 - GetCategoryAndMapToCategoryModel - error [There was an error deserializing the object of type ParTech.ImageLibrary.Core.ViewModels.Manage.MultiLingualListModel. Data at the root level is invalid. Line 1, position 1.] - - 
    at System.Runtime.Serialization.XmlObjectSerializer.ReadObjectHandleExceptions(XmlReaderDelegator reader, Boolean verifyObjectName, DataContractResolver dataContractResolver)
   at System.Runtime.Serialization.Json.DataContractJsonSerializer.ReadObject(XmlReader reader)
   at ParTech.ImageLibrary.Core.Repositories.ObjectRepository.GetCategoryAndMapToCategoryModel(Nullable`1 categoryid) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\ObjectRepository.cs:line 333 


2014-08-28 09:26:44,775 ERROR ParTech.ImageLibrary.Core.Repositories.ObjectRepository - dada1967 - GetCategoryAndMapToCategoryModel - error [There was an error deserializing the object of type ParTech.ImageLibrary.Core.ViewModels.Manage.MultiLingualListModel. Data at the root level is invalid. Line 1, position 1.] - - 
    at System.Runtime.Serialization.XmlObjectSerializer.ReadObjectHandleExceptions(XmlReaderDelegator reader, Boolean verifyObjectName, DataContractResolver dataContractResolver)
   at System.Runtime.Serialization.Json.DataContractJsonSerializer.ReadObject(XmlReader reader)
   at ParTech.ImageLibrary.Core.Repositories.ObjectRepository.GetCategoryAndMapToCategoryModel(Nullable`1 categoryid) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\ObjectRepository.cs:line 333 


2014-08-28 09:28:09,024 ERROR ParTech.ImageLibrary.Core.Repositories.ObjectRepository - dada1967 - GetCategoryAndMapToCategoryModel - error [There was an error deserializing the object of type ParTech.ImageLibrary.Core.ViewModels.Manage.MultiLingualListModel. Data at the root level is invalid. Line 1, position 1.] - - 
    at System.Runtime.Serialization.XmlObjectSerializer.ReadObjectHandleExceptions(XmlReaderDelegator reader, Boolean verifyObjectName, DataContractResolver dataContractResolver)
   at System.Runtime.Serialization.Json.DataContractJsonSerializer.ReadObject(XmlReader reader)
   at ParTech.ImageLibrary.Core.Repositories.ObjectRepository.GetCategoryAndMapToCategoryModel(Nullable`1 categoryid) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\ObjectRepository.cs:line 333 


2014-08-28 13:43:37,373 ERROR ParTech.ImageLibrary.Core.Repositories.ImageRepository - dada1967 - GetImagesForUser - error [No connection string named 'Entities' could be found in the application config file.] - 
    at System.Data.Entity.Internal.LazyInternalConnection.get_ConnectionHasModel()
   at System.Data.Entity.Internal.LazyInternalContext.InitializeContext()
   at System.Data.Entity.Internal.InternalContext.GetEntitySetAndBaseTypeForType(Type entityType)
   at System.Data.Entity.Internal.Linq.InternalSet`1.Initialize()
   at System.Data.Entity.Internal.Linq.InternalSet`1.get_InternalContext()
   at System.Data.Entity.Infrastructure.DbQuery`1.System.Linq.IQueryable.get_Provider()
   at System.Linq.Queryable.Where[TSource](IQueryable`1 source, Expression`1 predicate)
   at ParTech.ImageLibrary.Core.Repositories.ImageRepository.GetImagesForUser(Int32 userid) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\ImageRepository.cs:line 102 


2014-08-28 13:43:37,401 ERROR ParTech.ImageLibrary.Core.Repositories.ObjectRepository - dada1967 - GetProductsForUser - error [No connection string named 'Entities' could be found in the application config file.] - - 
    at System.Data.Entity.Internal.LazyInternalConnection.get_ConnectionHasModel()
   at System.Data.Entity.Internal.LazyInternalContext.InitializeContext()
   at System.Data.Entity.Internal.InternalContext.GetEntitySetAndBaseTypeForType(Type entityType)
   at System.Data.Entity.Internal.Linq.InternalSet`1.Initialize()
   at System.Data.Entity.Internal.Linq.InternalSet`1.get_InternalContext()
   at System.Data.Entity.Infrastructure.DbQuery`1.System.Linq.IQueryable.get_Provider()
   at System.Linq.Queryable.Where[TSource](IQueryable`1 source, Expression`1 predicate)
   at ParTech.ImageLibrary.Core.Repositories.ObjectRepository.GetProductsForUser(Int32 userid) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\ObjectRepository.cs:line 1108 


2014-08-28 13:44:08,813 ERROR ParTech.ImageLibrary.Core.Repositories.ImageRepository - dada1967 - GetImagesForUser - error [An error occurred while preparing the command definition. See the inner exception for details.] - 
    at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition..ctor(DbProviderFactory storeProviderFactory, DbCommandTree commandTree, DbInterceptionContext interceptionContext, IDbDependencyResolver resolver, BridgeDataReaderFactory bridgeDataReaderFactory, ColumnMapFactory columnMapFactory)
   at System.Data.Entity.Core.EntityClient.Internal.EntityProviderServices.CreateDbCommandDefinition(DbProviderManifest providerManifest, DbCommandTree commandTree, DbInterceptionContext interceptionContext)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlanFactory.CreateCommandDefinition(ObjectContext context, DbQueryCommandTree tree)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlanFactory.Prepare(ObjectContext context, DbQueryCommandTree tree, Type elementType, MergeOption mergeOption, Boolean streaming, Span span, IEnumerable`1 compiledQueryParameters, AliasGenerator aliasGenerator)
   at System.Data.Entity.Core.Objects.ELinq.ELinqQueryState.GetExecutionPlan(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__6()
   at System.Data.Entity.Core.Objects.ObjectContext.ExecuteInTransaction[T](Func`1 func, IDbExecutionStrategy executionStrategy, Boolean startLocalTransaction, Boolean releaseConnectionOnSuccess)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__5()
   at System.Data.Entity.SqlServer.DefaultSqlExecutionStrategy.Execute[TResult](Func`1 operation)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.GetResults(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<System.Collections.Generic.IEnumerable<T>.GetEnumerator>b__0()
   at System.Data.Entity.Internal.LazyEnumerator`1.MoveNext()
   at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
   at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
   at ParTech.ImageLibrary.Core.Repositories.ImageRepository.GetImagesForUser(Int32 userid) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\ImageRepository.cs:line 102 


2014-08-28 13:44:08,845 ERROR ParTech.ImageLibrary.Core.Repositories.ObjectRepository - dada1967 - GetProductsForUser - error [An error occurred while preparing the command definition. See the inner exception for details.] - - 
    at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition..ctor(DbProviderFactory storeProviderFactory, DbCommandTree commandTree, DbInterceptionContext interceptionContext, IDbDependencyResolver resolver, BridgeDataReaderFactory bridgeDataReaderFactory, ColumnMapFactory columnMapFactory)
   at System.Data.Entity.Core.EntityClient.Internal.EntityProviderServices.CreateDbCommandDefinition(DbProviderManifest providerManifest, DbCommandTree commandTree, DbInterceptionContext interceptionContext)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlanFactory.CreateCommandDefinition(ObjectContext context, DbQueryCommandTree tree)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlanFactory.Prepare(ObjectContext context, DbQueryCommandTree tree, Type elementType, MergeOption mergeOption, Boolean streaming, Span span, IEnumerable`1 compiledQueryParameters, AliasGenerator aliasGenerator)
   at System.Data.Entity.Core.Objects.ELinq.ELinqQueryState.GetExecutionPlan(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__6()
   at System.Data.Entity.Core.Objects.ObjectContext.ExecuteInTransaction[T](Func`1 func, IDbExecutionStrategy executionStrategy, Boolean startLocalTransaction, Boolean releaseConnectionOnSuccess)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__5()
   at System.Data.Entity.SqlServer.DefaultSqlExecutionStrategy.Execute[TResult](Func`1 operation)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.GetResults(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<System.Collections.Generic.IEnumerable<T>.GetEnumerator>b__0()
   at System.Data.Entity.Internal.LazyEnumerator`1.MoveNext()
   at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
   at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
   at ParTech.ImageLibrary.Core.Repositories.ObjectRepository.GetProductsForUser(Int32 userid) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\ObjectRepository.cs:line 1108 


2014-08-28 13:45:54,673 ERROR ParTech.ImageLibrary.Core.Repositories.ImageRepository - dada1967 - GetImagesForUser - error [An error occurred while preparing the command definition. See the inner exception for details.] - 
    at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition..ctor(DbProviderFactory storeProviderFactory, DbCommandTree commandTree, DbInterceptionContext interceptionContext, IDbDependencyResolver resolver, BridgeDataReaderFactory bridgeDataReaderFactory, ColumnMapFactory columnMapFactory)
   at System.Data.Entity.Core.EntityClient.Internal.EntityProviderServices.CreateDbCommandDefinition(DbProviderManifest providerManifest, DbCommandTree commandTree, DbInterceptionContext interceptionContext)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlanFactory.CreateCommandDefinition(ObjectContext context, DbQueryCommandTree tree)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlanFactory.Prepare(ObjectContext context, DbQueryCommandTree tree, Type elementType, MergeOption mergeOption, Boolean streaming, Span span, IEnumerable`1 compiledQueryParameters, AliasGenerator aliasGenerator)
   at System.Data.Entity.Core.Objects.ELinq.ELinqQueryState.GetExecutionPlan(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__6()
   at System.Data.Entity.Core.Objects.ObjectContext.ExecuteInTransaction[T](Func`1 func, IDbExecutionStrategy executionStrategy, Boolean startLocalTransaction, Boolean releaseConnectionOnSuccess)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__5()
   at System.Data.Entity.SqlServer.DefaultSqlExecutionStrategy.Execute[TResult](Func`1 operation)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.GetResults(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<System.Collections.Generic.IEnumerable<T>.GetEnumerator>b__0()
   at System.Data.Entity.Internal.LazyEnumerator`1.MoveNext()
   at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
   at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
   at ParTech.ImageLibrary.Core.Repositories.ImageRepository.GetImagesForUser(Int32 userid) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\ImageRepository.cs:line 102 


2014-08-28 13:45:55,112 ERROR ParTech.ImageLibrary.Core.Repositories.ObjectRepository - dada1967 - GetProductsForUser - error [An error occurred while preparing the command definition. See the inner exception for details.] - - 
    at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition..ctor(DbProviderFactory storeProviderFactory, DbCommandTree commandTree, DbInterceptionContext interceptionContext, IDbDependencyResolver resolver, BridgeDataReaderFactory bridgeDataReaderFactory, ColumnMapFactory columnMapFactory)
   at System.Data.Entity.Core.EntityClient.Internal.EntityProviderServices.CreateDbCommandDefinition(DbProviderManifest providerManifest, DbCommandTree commandTree, DbInterceptionContext interceptionContext)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlanFactory.CreateCommandDefinition(ObjectContext context, DbQueryCommandTree tree)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlanFactory.Prepare(ObjectContext context, DbQueryCommandTree tree, Type elementType, MergeOption mergeOption, Boolean streaming, Span span, IEnumerable`1 compiledQueryParameters, AliasGenerator aliasGenerator)
   at System.Data.Entity.Core.Objects.ELinq.ELinqQueryState.GetExecutionPlan(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__6()
   at System.Data.Entity.Core.Objects.ObjectContext.ExecuteInTransaction[T](Func`1 func, IDbExecutionStrategy executionStrategy, Boolean startLocalTransaction, Boolean releaseConnectionOnSuccess)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__5()
   at System.Data.Entity.SqlServer.DefaultSqlExecutionStrategy.Execute[TResult](Func`1 operation)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.GetResults(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<System.Collections.Generic.IEnumerable<T>.GetEnumerator>b__0()
   at System.Data.Entity.Internal.LazyEnumerator`1.MoveNext()
   at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
   at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
   at ParTech.ImageLibrary.Core.Repositories.ObjectRepository.GetProductsForUser(Int32 userid) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\ObjectRepository.cs:line 1108 


2014-08-28 16:50:18,157 ERROR ParTech.ImageLibrary.Core.Repositories.OrderRepository - partech - SaveInvoice - error [An error occurred while updating the entries. See the inner exception for details.] - - 
    at System.Data.Entity.Internal.InternalContext.SaveChanges()
   at ParTech.ImageLibrary.Core.Repositories.OrderRepository.SaveInvoice(Invoice invoice) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\OrderRepository.cs:line 146 


