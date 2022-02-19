using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Exceptions;
using GloboTicket.TicketManagement.Application.Models.Mail;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>

{
    private readonly IMapper _mapper;
    private readonly IEventRepository _eventRepository;
    private readonly IEmailService _emailService;

    public CreateEventCommandHandler(IMapper mapper, IEventRepository eventRepository, IEmailService emailService)
    {
        _mapper = mapper;
        _eventRepository = eventRepository;
        _emailService = emailService;
    }

    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateEventCommandValidator(_eventRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count > 0)
        {
            throw new ValidationException(validationResult);
        }

        var @event = _mapper.Map<Event>(request);

        @event = await _eventRepository.AddAsync(@event);

        var email = new Email()
        {
            To = "zcorpio@gmail.com",
            Subject = "A new event was created.",
            Body = $"A new event was created: {request}"
        };

        try
        {
            await _emailService.SendEmail(email);
        }
        catch (Exception ex)
        {
            // TODO: This shouldn't stop the API from doing something else, so this can just be logged.
        }

        return @event.EventId;
    }
}