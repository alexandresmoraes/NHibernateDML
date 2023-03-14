using FluentNHibernate.Mapping;

namespace NHibernateDML.Data
{
  public sealed class PessoaMap : ClassMap<Pessoa>
  {
    public PessoaMap()
    {
      Table("pessoa");

      Id(x => x.Id).GeneratedBy.Identity();
      Map(x => x.Nome);
    }
  }
}