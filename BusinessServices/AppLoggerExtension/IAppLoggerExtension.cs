using FMASolutionsCore.BusinessServices.BusinessCore.Extensions;
namespace FMASolutionsCore.BusinessServices.AppLoggerExtension
{
    public interface IAppLoggerExtension : IAppExtension
    {
        void WriteToErrorLog(string message, object sender);
        void WriteToProcessLog(string message);
        void WriteToUserRequestLog(string message);
        void WriteToCustomLog(string filename, string message);
    }
}