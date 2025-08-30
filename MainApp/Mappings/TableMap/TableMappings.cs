using Domain.Entities.TableMod;
using MainApp.ViewModels.TableModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.TableMap
{
    public static class TableMappings
    {
        public static TableViewModel ToViewModel(this DiningTable table)
        {
            return new TableViewModel
            {
                Id = table.Id,
                AreaId = table.AreaId,
                AreaName = table.Area?.Name ?? string.Empty,
                Name = table.Name,
                Description = table.Description,
                Capacity = table.Capacity,
                IsActive = table.IsActive,
                IsOccupied = table.IsOccupied,
                IsReservable = table.IsReservable
            };
        }

        public static DiningTable ToEntity(this TableViewModel vm)
        {
            return new DiningTable
            {
                Id = vm.Id,
                AreaId = vm.AreaId,
                Name = vm.Name,
                Description = vm.Description,
                Capacity = vm.Capacity,
                IsActive = vm.IsActive,
                IsOccupied = vm.IsOccupied,
                IsReservable = vm.IsReservable
            };
        }

        public static SeatViewModel ToViewModel(this Seat seat) =>
            new SeatViewModel
            {
                Id = seat.Id,
                TableId = seat.TableId,
                Name = seat.Name,
                IsOccupied = seat.IsOccupied
            };

        public static Seat ToEntity(this SeatViewModel vm) =>
            new Seat
            {
                Id = vm.Id,
                TableId = vm.TableId,
                Name = vm.Name,
                IsOccupied = vm.IsOccupied
            };

        public static TablePriceViewModel ToViewModel(this TablePrice price) =>
            new TablePriceViewModel
            {
                Id = price.Id,
                TableId = price.TableId,
                DayType = price.DayType,
                FromTime = price.FromTime,
                ToTime = price.ToTime,
                PricePerHour = price.PricePerHour,
                IsActive = price.IsActive
            };

        public static TablePrice ToEntity(this TablePriceViewModel vm) =>
            new TablePrice
            {
                Id = vm.Id,
                TableId = vm.TableId,
                DayType = vm.DayType,
                FromTime = vm.FromTime,
                ToTime = vm.ToTime,
                PricePerHour = vm.PricePerHour,
                IsActive = vm.IsActive
            };
    }
}
