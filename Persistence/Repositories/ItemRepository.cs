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
    
    public async Task<ICollection<FoundItem>> FindAllFoundItems2()
    {
        ICollection<FoundItem> result;

        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
        {
            cmd.CommandText =
                "select i.*, ia.time, e.* from items i left join item_actions ia on i.id = ia.item_id left join actions a on ia.action_id = a.id left join employees e on ia.employee_id = e.id left join status s on ia.id = s.item_actions_id where a.name = 'Found' and s.name = 'Approved'";
            cmd.CommandType = CommandType.Text;

            _context.Database.OpenConnection();

            using (var cmdResult = cmd.ExecuteReader())
            {
                ICollection<FoundItem> resultList = new List<FoundItem>();

                while (cmdResult.Read())
                {
                    var mapped = new FoundItem
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



    // find all claimed items that have been approved.
    public async Task<ICollection<ApprovedClaimedItem>> FindAllClaimedItems()
    {
        ICollection<ApprovedClaimedItem> result;

        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
        {
            cmd.CommandText =
                "select i.*, ia.time, e.* from items i left join item_actions ia on i.id = ia.item_id left join actions a on ia.action_id = a.id left join employees e on ia.employee_id = e.id left join status s on ia.id = s.item_actions_id where a.name = 'Claimed' and s.name = 'Approved'";
            cmd.CommandType = CommandType.Text;

            _context.Database.OpenConnection();

            using (var cmdResult = cmd.ExecuteReader())
            {
                ICollection<ApprovedClaimedItem> resultList = new List<ApprovedClaimedItem>();

                while (cmdResult.Read())
                {
                    var mapped = new ApprovedClaimedItem
                    {
                        Id = (Guid)cmdResult[0],
                        Name = (string)cmdResult[1],
                        Description = (string)cmdResult[2],
                        ImagePath = (string)cmdResult[3],
                        ClaimedAt = (DateTime)cmdResult[4],
                        ClaimedBy = new Employee
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

        ICollection<RequestFoundItem> result;

        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
        {
            cmd.CommandText =
                "select i.*, ia.*, s.*, e.* from items i left join item_actions ia on i.id = ia.item_id left join actions a on ia.action_id = a.id left join employees e on ia.employee_id = e.id left join status s on ia.id = s.item_actions_id where a.name = 'RequestFound' and i.name not in (select i.name from items i left join item_actions ia on i.id = ia.item_id left join actions a on ia.action_id = a.id where a.name = 'Found')";
            cmd.CommandType = CommandType.Text;

            _context.Database.OpenConnection();

            using (var cmdResult = cmd.ExecuteReader())
            {
                ICollection<RequestFoundItem> resultList = new List<RequestFoundItem>();

                while (cmdResult.Read())
                {
                    var mapped = new RequestFoundItem
                    {
                        RequestItem = new Item
                        {
                            Id = (Guid)cmdResult[0],
                            Name = (string)cmdResult[1],
                            Description = (string)cmdResult[2],
                            ImagePath = (string)cmdResult[3]
                        },
                        RequestId = (Guid)cmdResult[4],
                        RequestAt = (DateTime)cmdResult[5],
                        Status = (string)cmdResult[9],
                        RequestBy = new Employee
                        {
                            Id = (Guid)cmdResult[12],
                            Nik = (string)cmdResult[13],
                            FirstName = (string)cmdResult[14],
                            LastName = (string)cmdResult[15],
                            BirthDate = (DateTime)cmdResult[16],
                            Gender = (Gender)cmdResult[17],
                            Email = (string)cmdResult[18],
                            PhoneNumber = (string)cmdResult[19],
                            DepartmentId = (Guid)cmdResult[20]
                        }
                    };

                    resultList.Add(mapped);
                }

                cmdResult.Close();

                result = resultList;
            }
        }

        result = (from rfi in result where (query.Status == null) || rfi.Status.ToLower().Equals(query.Status.ToLower()) select rfi).ToList();

        return result;

    }

    public async Task<ICollection<FoundItemRequestClaim>> FindAllFoundItemRequestClaims(ActionRequestQuery query)
    {


        ICollection<FoundItemRequestClaim> result;

        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
        {
            cmd.CommandText =
                "select i.*, ia.*, s.*, e.* from items i left join item_actions ia on i.id = ia.item_id left join actions a on ia.action_id = a.id left join employees e on ia.employee_id = e.id left join status s on ia.id = s.item_actions_id where a.name = 'RequestClaim' and i.name not in (select i.name from items i left join item_actions ia on i.id = ia.item_id left join actions a on ia.action_id = a.id where a.name = 'Claimed')";
            cmd.CommandType = CommandType.Text;

            _context.Database.OpenConnection();

            using (var cmdResult = cmd.ExecuteReader())
            {
                ICollection<FoundItemRequestClaim> resultList = new List<FoundItemRequestClaim>();

                while (cmdResult.Read())
                {
                    var mapped = new FoundItemRequestClaim
                    {
                        RequestItem = new Item
                        {
                            Id = (Guid)cmdResult[0],
                            Name = (string)cmdResult[1],
                            Description = (string)cmdResult[2],
                            ImagePath = (string)cmdResult[3]
                        },
                        RequestId = (Guid)cmdResult[4],
                        RequestAt = (DateTime)cmdResult[5],
                        Status = (string)cmdResult[9],
                        RequestBy = new Employee
                        {
                            Id = (Guid)cmdResult[12],
                            Nik = (string)cmdResult[13],
                            FirstName = (string)cmdResult[14],
                            LastName = (string)cmdResult[15],
                            BirthDate = (DateTime)cmdResult[16],
                            Gender = (Gender)cmdResult[17],
                            Email = (string)cmdResult[18],
                            PhoneNumber = (string)cmdResult[19],
                            DepartmentId = (Guid)cmdResult[20]
                        }
                    };

                    resultList.Add(mapped);
                }

                cmdResult.Close();

                result = resultList;
            }
        }

        result = (from rfi in result where (query.Status == null) || rfi.Status.ToLower().Equals(query.Status.ToLower()) select rfi).ToList();

        return result;
    }
}