using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq;
using NHibernateDML.Data;

namespace NHibernateDML.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PessoaController : ControllerBase
  {
    private readonly SessionManager _sessionManager;

    public PessoaController(SessionManager sessionManager)
    {
      _sessionManager = sessionManager;
    }

    [HttpGet("seed")]
    public IEnumerable<Pessoa> Seed()
    {
      var seed = new Seed();
      var pessoas = seed.RandomPessoas();

      _sessionManager.UsingTransaction((session) =>
      {
        foreach (var pessoa in pessoas)
        {
          session.Save(pessoa);
        }
      });

      return pessoas;
    }

    [HttpGet]
    public IEnumerable<Pessoa>? GetPessoas([FromQuery] int page, [FromQuery] int limit)
    {
      IEnumerable<Pessoa>? pessoas = null;

      _sessionManager.UsingTransaction((session) =>
      {
        pessoas = session.QueryOver<Pessoa>()
        .Skip(page * limit).Take(limit)
        .List();
      });

      return pessoas;
    }

    [HttpGet("{id}")]
    public Pessoa? GetPessoaById([FromRoute] int id)
    {
      Pessoa? pessoa = null;

      _sessionManager.UsingTransaction((session) =>
      {
        pessoa = session.Get<Pessoa>(id);
      });

      return pessoa;
    }

    [HttpGet("dml")]
    public object? Dml()
    {
      object? pessoaId = null;

      _sessionManager.UsingTransaction((session) =>
      {
        pessoaId = session.Query<Pessoa>()
        .Where(c => c.Id == 1)
        .InsertBuilder()
        .Into<Clone>()
        .Value(d => d.Nome, c => c.Nome)
        .Insert();
      });

      return pessoaId;
    }
  }
}