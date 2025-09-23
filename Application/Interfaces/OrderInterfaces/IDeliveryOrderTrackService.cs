using Domain.Entities.OrderMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.OrderInterfaces
{
    public interface IDeliveryOrderTrackService
    {
        // Queries
        Task<DeliveryOrderTrack> GetByIdAsync(long id);
        Task<List<DeliveryOrderTrack>> GetByOrderAsync(long orderId);
        Task<List<DeliveryOrderTrack>> GetAssignedAsync();
        Task<List<DeliveryOrderTrack>> GetByDeliveryBoyAsync(long deliveryBoyId, DateOnly? date = null);
        Task<List<DeliveryOrderTrack>> GetByPartnerAsync(long deliveryPartnerId, DateOnly? date = null);

        // Commands
        Task<long> AddAsync(DeliveryOrderTrack entity);
        Task UpdateAsync(DeliveryOrderTrack entity);
        Task DeleteAsync(long id);

        // Status helpers
        Task AssignAsync(long id, long deliveryBoyId, bool isOwnDelivery, string remarks = "");
        Task AssignToPartnerAsync(long id, long partnerId, string remarks = "");
        Task MarkPickedAsync(long id);
        Task MarkDeliveredAsync(long id, string remarks = "");
        Task VoidDeliveryAsync(long id, string remarks = "");
    }
}
