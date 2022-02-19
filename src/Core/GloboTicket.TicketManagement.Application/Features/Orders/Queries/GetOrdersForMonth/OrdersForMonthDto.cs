namespace GloboTicket.TicketManagement.Application.Features.Orders.Queries.GetOrdersForMonth;

public class OrdersForMonthDto
{
    public Guid Id { get; set; }
    public decimal Total { get; set; }
    public DateTime OrderPlaced { get; set; }   
}