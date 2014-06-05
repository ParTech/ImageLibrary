2014-06-05 10:28:07,308 ERROR ParTech.ImageLibrary.Core.Workers.AccountsWorker -  -  RegisterUser - error [Invalid column name 'UserEmail'.] - 
    at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at WebMatrix.Data.Database.Execute(String commandText, Object[] args)
   at WebMatrix.WebData.SimpleMembershipProvider.CreateUserRow(IDatabase db, String userName, IDictionary`2 values)
   at WebMatrix.WebData.SimpleMembershipProvider.CreateUserAndAccount(String userName, String password, Boolean requireConfirmation, IDictionary`2 values)
   at WebMatrix.WebData.WebSecurity.CreateUserAndAccount(String userName, String password, Object propertyValues, Boolean requireConfirmationToken)
   at ParTech.ImageLibrary.Core.Workers.AccountsWorker.RegisterUser(RegisterModel model) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Workers\AccountsWorker.cs:line 56 


2014-06-05 10:43:48,402 ERROR ParTech.ImageLibrary.Core.Repositories.UserRepository -  - SaveProfile - error [Sequence contains no elements] - 
    at System.Linq.Enumerable.Single[TSource](IEnumerable`1 source)
   at System.Linq.Queryable.Single[TSource](IQueryable`1 source, Expression`1 predicate)
   at ParTech.ImageLibrary.Core.Repositories.UserRepository.SaveProfile(EditProfileModel profile) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\UserRepository.cs:line 167 


2014-06-05 10:47:48,784 ERROR ParTech.ImageLibrary.Core.Repositories.UserRepository -  - SaveProfile - error [An error occurred while updating the entries. See the inner exception for details.] - 
    at System.Data.Entity.Internal.InternalContext.SaveChanges()
   at ParTech.ImageLibrary.Core.Repositories.UserRepository.AddProfile(RegisterModel profile) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\UserRepository.cs:line 89 


2014-06-05 10:51:56,917 ERROR ParTech.ImageLibrary.Core.Repositories.UserRepository -  - AddProfile - error [An error occurred while updating the entries. See the inner exception for details.] - 
    at System.Data.Entity.Internal.InternalContext.SaveChanges()
   at System.Data.Entity.Internal.LazyInternalContext.SaveChanges()
   at System.Data.Entity.DbContext.SaveChanges()
   at ParTech.ImageLibrary.Core.Repositories.UserRepository.AddProfile(RegisterModel profile) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Repositories\UserRepository.cs:line 89 


2014-06-05 13:35:13,686 ERROR ParTech.ImageLibrary.Core.Workers.AccountsWorker -  -  RegisterUser - error [The INSERT statement conflicted with the FOREIGN KEY constraint "FK_UserProfile_webpages_Membership". The conflict occurred in database "ImageDatabase", table "dbo.webpages_Membership", column 'UserId'.
The statement has been terminated.] - 
    at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at WebMatrix.Data.Database.Execute(String commandText, Object[] args)
   at WebMatrix.WebData.SimpleMembershipProvider.CreateUserRow(IDatabase db, String userName, IDictionary`2 values)
   at WebMatrix.WebData.SimpleMembershipProvider.CreateUserAndAccount(String userName, String password, Boolean requireConfirmation, IDictionary`2 values)
   at WebMatrix.WebData.WebSecurity.CreateUserAndAccount(String userName, String password, Object propertyValues, Boolean requireConfirmationToken)
   at ParTech.ImageLibrary.Core.Workers.AccountsWorker.RegisterUser(RegisterModel model) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Workers\AccountsWorker.cs:line 89 


2014-06-05 13:39:57,451 ERROR ParTech.ImageLibrary.Core.Workers.AccountsWorker -  -  RegisterUser - error [The INSERT statement conflicted with the FOREIGN KEY constraint "FK_UserProfile_webpages_Membership". The conflict occurred in database "ImageDatabase", table "dbo.webpages_Membership", column 'UserId'.
The statement has been terminated.] - 
    at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at WebMatrix.Data.Database.Execute(String commandText, Object[] args)
   at WebMatrix.WebData.DatabaseWrapper.Execute(String commandText, Object[] parameters)
   at WebMatrix.WebData.SimpleMembershipProvider.CreateUserRow(IDatabase db, String userName, IDictionary`2 values)
   at WebMatrix.WebData.SimpleMembershipProvider.CreateUserAndAccount(String userName, String password, Boolean requireConfirmation, IDictionary`2 values)
   at WebMatrix.WebData.WebSecurity.CreateUserAndAccount(String userName, String password, Object propertyValues, Boolean requireConfirmationToken)
   at ParTech.ImageLibrary.Core.Workers.AccountsWorker.RegisterUser(RegisterModel model) in d:\Intern\ImageLibrary\ParTech.ImageLibrary.Core\Workers\AccountsWorker.cs:line 89 


