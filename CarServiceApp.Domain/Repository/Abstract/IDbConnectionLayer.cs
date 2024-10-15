namespace CarServiceApp.Domain.Repository.Abstract
{
    public interface IDbConnectionLayer
    {
        string InfoMessage { get; set; }
        Task<int> ExecutionProcedureAsync(string procedureName, string name, int? id);
        Task<int> ExecutionProcedureAsync(string procedureName, Dictionary<string, object> inputParameters);
        Task<int> ExecutionProcedureAsync(
            string procedureName,
            Dictionary<string, object> inputParameters,
            Dictionary<string, object> outParameters);
    }
}
