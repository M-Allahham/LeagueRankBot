using System;
using System.Collections.Generic;
using System.Drawing;
using LeagueBot;
using LeagueBot.Patterns;
using LeagueBot.Game.Enums;
using LeagueBot.Game.Misc;
using LeagueBot.IO;
using LeagueBot.Api;
using LeagueBot.ApiHelpers;

namespace LeagueBot
{
    public class GameOn : PatternScript
    {
		int ally = 5;
		
		int level = 1;

		bool isBlueSide = true;

        private Item[] Items = new Item[]
        {
			new Item("Spellthief",400), // Item
			new Item("Health pot",50),
			new Item("Health pot",50),
			new Item("Sweep",0),
			new Item("boots", 300),
			new Item("Sorcerer",800), // Item
			new Item("Amplifying",435),
			new Item("Amplifying",435),
			new Item("Sapphire",350),
			new Item("Lost ch",80),
			new Item("Blasting",850),
			new Item("Luden",1250), // Item
			new Item("Amplifying",435),	
			new Item("Amplifying",435),
			new Item("Hextech Alt",180),
			new Item("Needless",1250),
			new Item("Horizon",700), // Item
			new Item("Amplifying",435),
			new Item("Blighting",815),
			new Item("Blasting",850),
			new Item("Void S",400),	// Item
			new Item("Needless",1250),
			new Item("Rabadon",2350), // Item
			new Item("Elixir of Sorcery",500),
			new Item("Elixir of Sorcery",500),
			new Item("Elixir of Sorcery",500),
			new Item("Elixir of Sorcery",500),
			new Item("Elixir of Sorcery",500),
			new Item("Elixir of Sorcery",500),
			new Item("Elixir of Sorcery",500),
			new Item("Elixir of Sorcery",500),
			new Item("Elixir of Sorcery",500),
			new Item("Elixir of Sorcery",500),
        };

        public override void Execute()
        {
            bot.log("Waiting for league of legends process...");

            bot.waitProcessOpen(Constants.GameProcessName);
			bot.log("Found process");
			
			
            bot.wait(200);

            bot.log("Waiting for game to load.");

            bot.bringProcessToFront(Constants.GameProcessName);
            bot.centerProcess(Constants.GameProcessName);

            game.waitUntilGameStart();

            bot.log("Game Started");

            bot.bringProcessToFront(Constants.GameProcessName);
            bot.centerProcess(Constants.GameProcessName);

            bot.wait(3000);

			isBlueSide = (game.getSide() == SideEnum.Blue);
			
			if(isBlueSide)
				bot.log("Blue side");
			else
				bot.log("Red side");

			game.player.upgradeSpellOnLevelUp();

            bot.log("Playing...");

			BuyItems();
			GameLoop();
			
			End();
        }

        private void BuyItems()
        {
			if (game.player.shopAvailable() || game.player.dead())
			{
				int golds = game.player.getGolds();

				game.shop.toggle();
				bot.wait(500);
				foreach (Item item in Items)
				{
					if (item.Cost > golds)
					{
						break;
					}
					if (!item.Buyed)
					{
						game.shop.searchItem(item.Name);
						bot.wait(500);
						game.shop.buySearchedItem();
						bot.wait(500);
						item.Buyed = true;
						golds -= item.Cost;
					}
				}

				game.shop.toggle();
				bot.wait(500);
			}
        }

		private void Recall()
		{
			if (game.player.getNearTarget() != null)
			{
				Retreat();
				return;
			}
			game.camera.lockAlly(1);
			ally = 1;
			double currHealth = game.player.getHealthPercent();
			game.player.recall();
			DateTime start = DateTime.Now;
			while ((DateTime.Now - start).TotalMilliseconds < 8100)
			{
				bot.wait(100);
				if (game.player.dead())
				{
					Death();
					return;
				}
				game.player.pauseCheck(1);
				if (game.player.getHealthPercent() < currHealth)
				{
					bot.log("Recall was interupted! (ms=" + (DateTime.Now - start).TotalMilliseconds + ")");
					Retreat();
					return;
				}
			}
			BuyItems();
			if (game.player.getHealthPercent() < 1)
				while (game.player.getHealthPercent() < 1 || game.player.getManaPercent() < 1)
				{
					game.player.pauseCheck(ally);
					bot.wait(100);
				}
		}

		private void Retreat()
		{
			bot.log("Retreating!");
			game.camera.lockAlly(1);
			ally = 1;
			double currHealth = game.player.getHealthPercent();
			DateTime start = DateTime.Now;
			while ((DateTime.Now - start).TotalMilliseconds < 3000)	
			{
				game.player.pauseCheck(1);
				game.player.retreatClick(isBlueSide);
				if (game.player.dead())
				{
					bot.log("Retreat failed due to death. (ms=" + (DateTime.Now - start).TotalMilliseconds + ")");
					Death();
					return;
				}
				if (game.player.getHealthPercent() < currHealth || game.player.getNearTarget() != null)
				{
					currHealth = game.player.getHealthPercent();
					start = DateTime.Now;
				}
			}
			Recall();
			BuyItems();
		}

		private void GameLoop()
        {
            level = game.player.getLevel();

            //bool dead = false;

            while (bot.isProcessOpen(Constants.GameProcessName))
            {

				bot.bringProcessToFront(Constants.GameProcessName);
				bot.centerProcess(Constants.GameProcessName);

				game.player.pauseCheck(ally);
				
                if (game.player.getLevel() != level)
                {
					level = game.player.getLevel();
					game.player.upgradeSpellOnLevelUp();
                }

				if (game.player.dead())
				{
					Death();
					continue;
				}
               
				if (game.player.getHealthPercent() <= 0.4d)
                {
					bot.log("Low health");
					Retreat();
                    continue;
                }

				if (game.player.getManaPercent() <= 0.10d)
                {
					bot.log("Low mana");
					Retreat();
                    continue;
				}

                CastAndMove();

            }
			bot.log("Game process not found");
        }

		private void Death()
		{
			bot.log("You died.");
			BuyItems();
			while (game.player.dead())
			{
				game.player.pauseCheck(1);
				bot.wait(100);
			}
			bot.log("You respawned.");
		}

        private void CastAndMove() // Replace this by Champion pattern script.
        {
			ally = game.getAllyIdToFollow();
			game.camera.lockAlly(ally);
			if(isBlueSide){
				game.moveCenterScreenBlue();
				game.moveCenterScreenBlue();
			}
			else {
				game.moveCenterScreenRed();
				game.moveCenterScreenRed();
			}

			if(game.player.AllyDead(ally)) {
				if(game.player.GameTime() > 14 * 60){
					ally = game.getAllyIdToFollow();
					game.camera.lockAlly(ally);
				} else {
					Retreat();
					return;
				}
			}
			else 
			{
				if(level > 2)
					game.player.shieldSpam(ally);
				if (game.player.getHealthPercent() <= 0.65d){
					game.player.Potion();
				}
				if (level > 5 )
				{
					game.player.tryCastUlt();
				}
				else
				{
					game.player.pokeCombo();
				} 
			}
        }
		
		public override void End()
        {
            bot.executePattern("End");
            base.End();
        }
    }
}