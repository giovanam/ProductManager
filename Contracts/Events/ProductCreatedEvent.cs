namespace ProductManager.Contracts.Events
{
    public record ProductCreatedEvent(int Id, string Nome, decimal Preco, int QuantidadeEstoque);

}
