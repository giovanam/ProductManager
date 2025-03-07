using MassTransit;
using ProductManager.Contracts.Events;

namespace ProductManager.Application.Consumers
{
    public class ProductDeletedConsumer : IConsumer<ProductDeletedEvent>
    {
        private readonly ILogger<ProductDeletedConsumer> _logger;

        public ProductDeletedConsumer(ILogger<ProductDeletedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {
            try
            {
                var productDeletedEvent = context.Message;

                _logger.LogInformation("Consuming ProductDeletedEvent: ProductId = {ProductId}", productDeletedEvent.Id);

                _logger.LogInformation("Produto removido com sucesso: ProductId = {ProductId}", productDeletedEvent.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar o evento ProductDeletedEvent.");
            }
        }
    }
}
