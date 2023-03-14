using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using ISession = NHibernate.ISession;

namespace NHibernateDML.Data
{
  public class SessionManager : IDisposable
  {
    private static Configuration? _configuration;
    private static ISessionFactory? _sessionFactory;
    protected ISession? _sesion;

    public SessionManager()
    {
      if (_configuration == null)
      {
        _configuration = Fluently.Configure()
          .Database(SQLiteConfiguration.Standard.InMemory().ShowSql().FormatSql())
          .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
          .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Pessoa>())
          .BuildConfiguration();

        _sessionFactory = _configuration.BuildSessionFactory();

        _sesion = _sessionFactory.OpenSession();

        new SchemaExport(_configuration).Execute(true, true, false, _sesion.Connection, Console.Out);
      }
    }

    public void UsingTransaction(Action<ISession> action)
    {
      using var transaction = _sesion!.BeginTransaction();

      action(_sesion);

      transaction.Commit();

      _sesion.Clear();
    }

    public void Dispose()
    {
      _sessionFactory?.Dispose();
    }
  }
}