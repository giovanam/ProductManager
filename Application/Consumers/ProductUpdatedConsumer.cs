using MassTransit;
using ProductManager.Contracts.Events;

namespace ProductManager.Application.Consumers
{
    public class ProductUpdatedConsumer : IConsumer<ProductUpdatedEvent>
    {
        private readonly ILogger<ProductUpdatedConsumer> _logger;

        public ProductUpdatedConsumer(ILogger<ProductUpdatedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
        {
            try
            {
                var productUpdatedEvent = context.Message;

                _logger.LogInformation("Consuming ProductUpdatedEvent: ProductName = {ProductName}", productUpdatedEvent.Nome);

                _logger.LogInformation("Produto atualizado com sucesso: ProductName = {ProductName}", productUpdatedEvent.Nome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar o evento ProductUpdatedEvent.");
            }
        }
    }
}
