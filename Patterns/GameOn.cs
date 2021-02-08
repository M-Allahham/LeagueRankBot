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
		
		bool isRecalling = false;

        private Point CastTargetPoint
        {
            get;
            set;
        }
        private int AllyIndex 
        {
            get;
            set;
        }

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

        public override bool ThrowException
        {
            get
            {
                return false;
            }
        }

        public override void Execute()
        {
            bot.log("Waiting for league of legends process...");

            bot.waitProcessOpen(Constants.GameProcessName);
			bot.log("Found process");
			
            //bot.waitUntilProcessBounds(Constants.GameProcessName, 1030, 770);
			bot.log("Nvrmind");
			
            bot.wait(200);

            bot.log("Waiting for game to load.");

            bot.bringProcessToFront(Constants.GameProcessName);
            bot.centerProcess(Constants.GameProcessName);

            game.waitUntilGameStart();

            bot.log("Game Started");

            bot.bringProcessToFront(Constants.GameProcessName);
            bot.centerProcess(Constants.GameProcessName);

            bot.wait(3000);

            if (game.getSide() == SideEnum.Blue)
            {
                CastTargetPoint = new Point(1084, 398);
                bot.log("We are blue side !");
            }
            else
            {
                CastTargetPoint = new Point(644, 761);
                bot.log("We are red side !");
            }

            game.player.upgradeSpellOnLevelUp();

            OnSpawnJoin();

            bot.log("Playing...");

            GameLoop();
			
			End();
        }
        private void BuyItems()
        {
			isRecalling = false;
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
					isRecalling = false;
                    golds -= item.Cost;
                }
            }

            game.shop.toggle();
			bot.wait(500);

        }
        private void CheckBuyItems()
        {
            int golds = game.player.getGolds();

            foreach (Item item in Items)
            {
                if (item.Cost > golds)
                {
                    break;
                }
                if (golds > 150000)
               {
                    game.player.recall();
					isRecalling = false;
                    bot.wait(10000);
					game.camera.lockAlly(ally);
                }
            }


        }

        private void GameLoop()
        {
            level = game.player.getLevel();

            bool dead = false;

            while (bot.isProcessOpen(Constants.GameProcessName))
            {
                bot.bringProcessToFront(Constants.GameProcessName);

                bot.centerProcess(Constants.GameProcessName);

                int newLevel = game.player.getLevel();
				
                if (newLevel != level)
                {
                    level = newLevel;
                    game.player.upgradeSpellOnLevelUp();
                }		
				
                if (game.player.dead())
                {
                    if (!dead)
                    {
                        dead = true;
                        OnDie();
                        isRecalling = false;
                    }

                    bot.wait(4000);
                    continue;
                }

                if (dead)
                {
					bot.log("We died");
                    dead = false;
					isRecalling = false;
                    OnRevive();
                    continue;
                }

                if (isRecalling)
                {
					bot.log("Recalling!");
					if(game.getSide() == SideEnum.Blue){
						game.camera.lockAlly(1);
						game.player.click(0, 740);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						bot.wait(1000);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						game.player.click(0, 740);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						bot.wait(1000);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						game.player.click(0, 740);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						bot.wait(1000);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						game.player.click(0, 740);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						bot.wait(1000);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						game.player.click(0, 740);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						bot.wait(1000);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						game.camera.lockAlly(ally);
					}
					if(game.getSide() == SideEnum.Red){
						game.camera.lockAlly(1);
						game.player.click(1700, 0);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						bot.wait(1000);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						game.player.click(1700, 0);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						bot.wait(1000);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						game.player.click(1700, 0);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						bot.wait(1000);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						game.player.click(1700, 0);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						bot.wait(1000);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						game.player.click(1700, 0);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						bot.wait(1000);
						if (game.player.dead()){
							BuyItems();
							GameLoop();
							isRecalling = false;
						}
						game.camera.lockAlly(ally);
					}
                    game.player.recall();
                    bot.wait(8500);
					isRecalling = false;
					
					BuyItems();
					
					if(game.player.getHealthPercent() < 1 || game.player.getManaPercent() < 1){
						bot.log("Waiting to regen");
						bot.wait(3000);
					}
                    continue;
                }
				
				if (game.player.getHealthPercent() <= 0.4d)
                {
					bot.log("Low health");
                    isRecalling = true;
                    continue;
                }

				if (game.player.getManaPercent() <= 0.10d)
                {
					bot.log("Low mana");
                    isRecalling = true;
                    continue;
				}

                CastAndMove();

            }
			bot.log("Game process not found");
        }
        private void OnDie()
        {
            BuyItems();
			isRecalling = false;
        }
		
        private void OnSpawnJoin()
        {
            BuyItems();
			game.camera.lockAlly(1);
            game.camera.lockAlly(ally);
			isRecalling = false;
        }
        private void OnRevive()
        {
			BuyItems();
            AllyIndex = game.getAllyIdToFollow();
			game.camera.lockAlly(1);
            game.camera.lockAlly(ally);
			isRecalling = false;
        }

        private void CastAndMove() // Replace this by Champion pattern script.
        {
			ally = game.getAllyIdToFollow();
			game.camera.lockAlly(ally);
			if(game.getSide() == SideEnum.Red){
				game.moveCenterScreenRed();
			} else {
				game.moveCenterScreenBlue();
			}
			if(game.getSide() == SideEnum.Red){
				game.moveCenterScreenRed();
			} else {
				game.moveCenterScreenBlue();
			}
			if(game.player.AllyDead(ally)) {
				isRecalling = true;
				GameLoop();
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
					game.camera.lockAlly(ally);
					game.player.tryCastUlt();
				}
				else
				{
					game.camera.lockAlly(ally);
					game.player.pokeCombo();
				} 
			}
			isRecalling = false;
        }
		
		public override void End()
        {
            bot.executePattern("End");
            base.End();
        }
    }
}