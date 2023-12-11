using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sol_Demo.Infrastructure;
using Sol_Demo.Infrastructure.Entity;
using System.Threading;

namespace Sol_Demo.Features;

public class GetSalesOrderFilterQueryRequestDTO
{
    public int? OrderQty { get; set; }

    public decimal? UnitPrice { get; set; }
}

public class GetSalesOrderFilterQueryResponseDTO
{
    public int? SalesOrderDetailsID { get; set; }

    public int? OrderQty { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? LineTotal { get; set; }
}

public class GetSalesOrderFilterDataService : IRequest<List<GetSalesOrderFilterQueryResponseDTO>>
{
    public GetSalesOrderFilterDataService(int? orderQty, decimal? UnitPrice)
    {
        this.OrderQty = orderQty;
        this.UnitPrice = UnitPrice;
    }

    public int? OrderQty { get; }

    public decimal? UnitPrice { get; }
}

public class GetSalesOrderFilterDataServiceHandler : IRequestHandler<GetSalesOrderFilterDataService, List<GetSalesOrderFilterQueryResponseDTO>>
{
    private readonly AdventureWorks2012Context? adventureWorks2012Context = null;

    public GetSalesOrderFilterDataServiceHandler(AdventureWorks2012Context adventureWorks2012Context)
    {
        this.adventureWorks2012Context = adventureWorks2012Context;
    }

    public Task<List<GetSalesOrderFilterQueryResponseDTO>> Handle(GetSalesOrderFilterDataService request, CancellationToken cancellationToken)
    {
        var queryData = adventureWorks2012Context?.SalesOrderDetails;

        if (!queryData!.Any())
            throw new Exception("Sales Order data not found.");

        IQueryable<SalesOrderDetail> query = queryData?.AsQueryable()!;

        if (request.OrderQty is not null)
            query = query.Where(element => request.OrderQty == element.OrderQty);

        if (request.UnitPrice is not null)
            query = query.Where(element => request.UnitPrice == element.UnitPrice);

        return query.Select(element => new GetSalesOrderFilterQueryResponseDTO()
        {
            OrderQty = element.OrderQty,
            UnitPrice = element.UnitPrice,
            LineTotal = element.LineTotal,
            SalesOrderDetailsID = element.SalesOrderDetailId
        }).ToListAsync();
    }
}

public class GetSalesOrderFilterQuery : GetSalesOrderFilterQueryRequestDTO, IRequest<List<GetSalesOrderFilterQueryResponseDTO>>
{
}

public class GetSalesOrderFilterQueryHandler : IRequestHandler<GetSalesOrderFilterQuery, List<GetSalesOrderFilterQueryResponseDTO>>
{
    private readonly IMediator? mediator = null;

    public GetSalesOrderFilterQueryHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public Task<List<GetSalesOrderFilterQueryResponseDTO>> Handle(GetSalesOrderFilterQuery request, CancellationToken cancellationToken)
        => mediator!.Send(new GetSalesOrderFilterDataService(request.OrderQty, request.UnitPrice), cancellationToken);
}