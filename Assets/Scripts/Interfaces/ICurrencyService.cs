
public interface ICurrencyService 
{
    int GetMoney();
    int GetFragments();
    bool TrySpendMoney(int amount);
    bool TrySpendFragments(int amount);
    void AddMoney(int amount);
    void AddFragments(int amount);
}
