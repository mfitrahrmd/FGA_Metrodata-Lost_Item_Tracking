#region

using System.Data;
using Application.DAOs.Item;
using Application.DTOs.Item;
using Application.Repositories;
using Application.Services.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Persistence.Context;

#endregion

namespace Persistence.Repositories;

public class ItemRepository : BaseRepository<Item, ApplicationDbContext>, IItemRepository
{
    private readonly IItemActionsRepository _itemActionsRepository;

    public ItemRepository(ApplicationDbContext context, IItemActionsRepository itemActionsRepository) : base(context)
    {
        _itemActionsRepository = itemActionsRepository;
    }

    // find all found items that have been approved and have not claimed.
    public async Task<ICollection<ApprovedFoundItem>> FindAllFoundItems()
    {
        ICollection<ApprovedFoundItem> result;

        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
        {
            cmd.CommandText =
                "select i.*, ia.time, e.* from items i left join item_actions ia on i.id = ia.item_id left join actions a on ia.action_id = a.id left join employees e on ia.employee_id = e.id left join status s on ia.id = s.item_actions_id where a.name = 'Found' and s.name = 'Approved' and i.name not in (select i.name from items i left join item_actions ia on i.id = ia.item_id left join actions a on ia.action_id = a.id where a.name = 'Claimed')";
            cmd.CommandType = CommandType.Text;

            _context.Database.OpenConnection();

            using (var cmdResult = cmd.ExecuteReader())
            {
                ICollection<ApprovedFoundItem> resultList = new List<ApprovedFoundItem>();
                
                while (cmdResult.Read())
                {
                    var mapped = new ApprovedFoundItem
                    {
                        Id = (Guid)cmdResult[0],
                        Name = (string)cmdResult[1],
                        Description = (string)cmdResult[2],
                        ImagePath = (string)cmdResult[3],
                        FoundAt = (DateTime)cmdResult[4],
                        FoundBy = new Employee
                        {
                            Id = (Guid)cmdResult[5],
                            Nik = (string)cmdResult[6],
                            FirstName = (string)cmdResult[7],
                            LastName = (string)cmdResult[8],
                            BirthDate = (DateTime)cmdResult[9],
                            Gender = (Gender)cmdResult[10],
                            Email = (string)cmdResult[11],
                            PhoneNumber = (string)cmdResult[12],
                            DepartmentId = (Guid)cmdResult[13]
                        }
                    };
                    
                    resultList.Add(mapped);
                }
                
                cmdResult.Close();

                result = resultList;
            }
        }

        return result;
    }

    public async Task<ICollection<RequestFoundItem>> FindAllRequestFoundItems(ActionRequestQuery query)
    {
        var result = from i in _context.Items
            join ia in _context.ItemActions on i.Id equals ia.ItemId
            join a in _context.Actions on ia.ActionId equals a.Id
            join e in _context.Employees on ia.EmployeeId equals e.Id
            join s in _context.Status on ia.Id equals s.ItemActionsId
            where a.Name.Equals(ActionType.RequestFound.ToString()) && (query.Status == null || s.Name.Equals(query.Status))
            select new RequestFoundItem
            {
                RequestId = ia.Id,
                RequestItem = i,
                RequestAt = ia.Time,
                RequestBy = e,
                Status = s.Name
            };

        return result.ToList();
    }

    public async Task<ICollection<FoundItemRequestClaim>> FindAllFoundItemRequestClaims(ActionRequestQuery query)
    {
        var result = from i in _context.Items
            join ia in _context.ItemActions on i.Id equals ia.ItemId
            join a in _context.Actions on ia.ActionId equals a.Id
            join e in _context.Employees on ia.EmployeeId equals e.Id
            join s in _context.Status on ia.Id equals s.ItemActionsId
            where a.Name.Equals(ActionType.RequestClaim.ToString()) && (query.Status == null || s.Name.Equals(query.Status))
            select new FoundItemRequestClaim
            {
                RequestId = ia.Id,
                RequestItem = i,
                RequestAt = ia.Time,
                RequestBy = e,
                Status = s.Name
            };

        return result.ToList();
    }
}