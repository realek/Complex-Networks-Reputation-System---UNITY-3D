using UnityEngine;
using System.Collections;

public enum Locations
{
    BP_HighMarshes = 0,
    BP_OskanHarborCity = 1,
    BP_ArbohsLandingOutpost = 2,
    BP_CityOfYshaven = 3,
    Woodhearth = 4,
    SpinePeakMountains = 5,
    GrayMarsh = 7,
    TowerofSolitude = 8,
    ForgottenCrypts = 9,
    EndlessDepths = 10,
    CityOfNezear = 11,
    CityOfHarbyn = 12,
    CityOfVardenDeep = 13,
    CemeteryOfTheLost = 14
}
[System.Serializable]
struct Location {

    [Tooltip("Birthplaces start with BP_")]
    public Locations loc;
    public string description;
}
