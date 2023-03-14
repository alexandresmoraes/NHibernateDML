using FluentNHibernate.Mapping;

namespace NHibernateDML.Data
{
  public sealed class CloneMap : ClassMap<Clone>
  {
    public CloneMap()
    {
      Table("clone");

      Id(x => x.Id).GeneratedBy.Identity();
      Map(x => x.Nome);
    }
  }
}
