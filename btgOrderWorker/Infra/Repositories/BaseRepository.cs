namespace btgOrderWorker.infra.Repositories;

public abstract class BaseRepository
{    
    protected  string connectionString { get; set; }
    public BaseRepository(IConfiguration configuration)
    {
       connectionString = configuration.GetValue<string>("connectionString:postgres").ToString();
    }

}