﻿using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Simulation.Input;
using Harmony;
using Il2CppSystem.Collections.Generic;
using Assets.Scripts.Unity;
using Gurren_Core.Extensions;

namespace Unlimited_Heros.Patches
{
    [HarmonyPatch(typeof(TowerInventory), nameof(TowerInventory.Init))]
    internal class TowerInventory_Init
    {
        [HarmonyPrefix]
        internal static bool Prefix(TowerInventory __instance, ref List<TowerDetailsModel> allTowersInTheGame)
        {
            if (SessionData.CurrentSession.IsCheating)
                return true;

            var unlockedHeros = Game.instance.GetProfileModel().unlockedHeroes;
            for (int i = 0; i < allTowersInTheGame.Count; i++)
            {
                var tower = allTowersInTheGame[i];
                var heroDetailsModel = tower.TryCast<HeroDetailsModel>();
                var isHero = heroDetailsModel != null;
                if (!isHero)
                    continue;


                string towerName = tower.name.Replace("HeroDetailsModel_", "");
                bool hasHero = unlockedHeros.Contains(towerName);
                if (!hasHero)
                    continue;

                allTowersInTheGame[i] = new ShopTowerDetailsModel(tower.towerId, tower.towerIndex, 0, 0, 0, -1, tower.popsRequired, tower.mods);
            }

            return true;
        }
    }
}