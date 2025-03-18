using System.Collections.Generic;

public interface IMechanicService
{
    void BuyAndSpawnMechanic(int mechanicCost);
    int GetHiredMechanicsCount();
    List<IMechanic> GetMechanics();
}