
public interface IMoneyService 
{
    int GetMoney();
    bool TrySpend(int amount);
    void AddMoney(int amount);
}
