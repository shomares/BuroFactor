using BuroFactor.Models.dao;

namespace BuroFactor.code.bussines
{
    public interface IBuroFactory
    {
        burofactorEntities Create();
        burofactorEntities BeginTrasanction();
    }
}