namespace ProductManager.Contracts.Events
{
    public record ProductUpdatedEvent(int Id, string Nome, decimal Preco, int QuantidadeEstoque);

}
