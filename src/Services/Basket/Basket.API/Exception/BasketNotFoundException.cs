namespace Basket.API.Exception
{
    public class BasketNotFoundException : NotFoundException
    {
        public BasketNotFoundException(string userName) : base($"Basket with user name {userName} not found")
        {
        }
    }
}
