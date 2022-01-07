using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataAccess.Internal.Databases;

public class WorkTrackingDB : SqlDataAccess, IWorkTranckingDB
{
    public WorkTrackingDB(IConfiguration config, ILogger<WorkTrackingDB> logger) : base(config, logger, "WorkTracking")
    {
    }
}
