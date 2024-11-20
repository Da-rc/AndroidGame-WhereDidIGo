

public class UnitJugador
{
    public string unitName { get; set; }

    public int damage { get; set; }

    public int maxHP { get; set; }
    public int currentHP { get; set; }

    public UnitJugador()
    {
        unitName = "Tú??";
        damage = 5;
        maxHP = 30;
        currentHP = 30;
    }

    public bool recibirGolpe(int dmg)
    {
        currentHP -= dmg;
        if(currentHP <= 0) 
            return true;
        else
            return false;
    }

    public void curar(int curacion)
    {
        currentHP += curacion;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }


}
