using AV_test.Parsing.Deserialization.WoodDeals;

namespace AV_test.DAL.Interfaces
{
    public interface IWoodDealsRepository:IGenericRepository<ReportWoodDeal>
    {
        ReportWoodDeal? Get(string sellerInn, string buyerInn, string dealNumber);
    }
}
