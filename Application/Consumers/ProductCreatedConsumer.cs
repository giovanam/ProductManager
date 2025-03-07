using MassTransit;
using ProductManager.Contracts.Events;

namespace ProductManager.Application.Consumers
{
    public class ProductCreatedConsumer : IConsumer<ProductCreatedEvent>
    {
        private readonly ILogger<ProductCreatedConsumer> _logger;

        public ProductCreatedConsumer(ILogger<ProductCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            try
            {
                var productCreatedEvent = context.Message;

                _logger.LogInformation("Consuming ProductCreatedEvent: {ProductName}", productCreatedEvent.Nome);
                _logger.LogInformation("Produto Criado com sucesso: {ProductName}", productCreatedEvent.Nome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar o evento ProductCreatedEvent.");
            }
        }
    }
}
