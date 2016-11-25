using BuroFactorWS.model;

namespace BuroFactorWS.src.dao
{
    public interface IBuroFactory
    {
        burofactorEntities BeginTrasanction();
        burofactorEntities Create();
    }
}