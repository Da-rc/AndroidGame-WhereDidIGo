

public class UnitEnemigo
{

    public string unitName { get; set; }

    public int damage { get; set; }

    public int maxHP { get; set; }
    public int currentHP { get; set; }

    public UnitEnemigo()
    {
        unitName = "Flor";
        damage = 5;
        maxHP = 20;
        currentHP = 20;
    }

    public bool recibirGolpe(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
            return true;
        else
            return false;
    }
}
