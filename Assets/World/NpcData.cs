using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class NpcData
{
    private static Dictionary<int, string[]> FNdb;
    private static Dictionary<int, string[]> LNdb;
    private static bool init = false;

    private static void Init()
    {
        if (init)
            return;
        init = true;

    }
    public static string GetFirstName(Race race)
    {
        if (!init)
            Init();

        int dbID = -1;

        switch (race)
        {
            case Race.Dwarf:
                dbID = 0;
                break;
            case Race.Elf:
                dbID = 1;
                break;
            case Race.Gnome:
                dbID = 2;
                break;
            case Race.Goblin:
                dbID = 3;
                break;
            case Race.HalfElf:
                dbID = 4;
                break;
            case Race.Human:
                dbID = 5;
                break;
            case Race.Orc:
                dbID = 6;
                break;
            case Race.Troll:
                dbID = 7;
                break;
            case Race.Undead:
                dbID = 8;
                break;
        }

        return null;
    }

    public static string GetLastName(Race race)
    {
        if (!init)
            Init();

        int dbID = -1;
        switch (race)
        {
            case Race.Dwarf:
                dbID = 0;
                break;
            case Race.Elf:
                dbID = 1;
                break;
            case Race.Gnome:
                dbID = 2;
                break;
            case Race.Goblin:
                dbID = 3;
                break;
            case Race.HalfElf:
                dbID = 4;
                break;
            case Race.Human:
                dbID = 5;
                break;
            case Race.Orc:
                dbID = 6;
                break;
            case Race.Troll:
                dbID = 7;
                break;
            case Race.Undead:
                dbID = 8;
                break;
        }

        return null;
    }




}
