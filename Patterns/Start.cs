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
    public class Start : PatternScript
    {
		
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
            new Item("Spellthief",400),
			new Item("Amplify",435),
			new Item("Fiendish",465),
			new Item("Amplify",435),
			new Item("Null",450),
			new Item("Verdant",315),
			new Item("Banshee",400),
            new Item("Sorcere",1100),
			new Item("Amplify",435),
			new Item("Amplify",435),
			new Item("Sapphire",350),
            new Item("Lost Chapter",80),
			new Item("Blasting",850),
            new Item("Luden Tempest",1250), // <--- Cost when Lost Chapter & Blasting Wand were bought
            new Item("Needlessly Large Rod",1250),
            new Item("Needlessly Large Rod",1250),
            new Item("Rabadon Deathcap",1100),
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

            bot.waitUntilProcessBounds(Constants.GameProcessName, 1030, 797);

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
                if (golds > 2000)
               {
                    game.player.recall();
                    bot.wait(10000);
                    if (game.player.getManaPercent() == 1)
                    {
                        OnSpawnJoin();

                    }
                    

                }
            }


        }

        private void GameLoop()
        {
            int level = game.player.getLevel();

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

				//if ally dead, recall
				//if(game.player.followdead()){
				//	bot.log("ally dead");
				//}
				//if ally recall, recall
				
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
					bot.log("We're dead");
                    dead = false;
					isRecalling = false;
                    OnRevive();
                    continue;
                }

                if (isRecalling)
                {
					bot.log("Recalling!");
					if(game.getSide() == SideEnum.Blue){
						game.camera.lockAlly("F1");
						game.player.click(0, 740);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						bot.wait(2000);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						game.player.click(0, 740);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						bot.wait(2000);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						game.player.click(0, 740);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						bot.wait(2000);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						game.player.click(0, 740);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						bot.wait(2000);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						game.player.click(0, 740);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						bot.wait(2000);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						game.camera.lockAlly("F5");
					}
					if(game.getSide() == SideEnum.Red){
						game.camera.lockAlly("F1");
						game.player.click(1700, 0);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						bot.wait(2000);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						game.player.click(1700, 0);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						bot.wait(2000);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						game.player.click(1700, 0);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						bot.wait(2000);
						if (game.player.dead()){
							GameLoop();
							isRecalling = false;
						}
						game.camera.lockAlly("F5");
					}
                    game.player.recall();
                    bot.wait(8500);
					isRecalling = false;

					if(game.player.getHealthPercent() < 1 && game.player.getManaPercent() < 1){
						bot.log("Waiting to regen");
						bot.wait(4000);
					}

                    if (game.player.getManaPercent() == 1)
                    {
                        OnSpawnJoin();
                        isRecalling = false;
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
			game.camera.lockAlly("F1");
            game.camera.lockAlly("F5");
			isRecalling = false;
        }
        private void OnRevive()
        {
            AllyIndex = game.getAllyIdToFollow();
            game.camera.lockAlly("F5");
			isRecalling = false;
        }

        private void CastAndMove() // Replace this by Champion pattern script.
        {
            int Ripeti = 0;
            while (Ripeti < 3)
            {

                Ripeti = Ripeti + 1;
				
                game.moveCenterScreen();
				if (game.player.getHealthPercent() <= 0.4d){
					isRecalling = true;
					GameLoop();
				}
				game.player.tryCastSpellOnTarget(3); // veigar cage
				if (game.player.getHealthPercent() <= 0.4d){
					isRecalling = true;
					GameLoop();
				}
                game.moveCenterScreen();
				if (game.player.getHealthPercent() <= 0.4d){
					isRecalling = true;
					GameLoop();
				}
				game.player.tryCastSpellOnTarget(2); // Z
				if (game.player.getHealthPercent() <= 0.4d){
					isRecalling = true;
					GameLoop();
				}
                game.moveCenterScreen();
				if (game.player.getHealthPercent() <= 0.4d){
					isRecalling = true;
					GameLoop();
				}
				game.player.tryCastSpellOnTarget(1); // Q
				if (game.player.getHealthPercent() <= 0.4d){
					isRecalling = true;
					GameLoop();
				}
                game.moveCenterScreen();
				if (game.player.getHealthPercent() <= 0.4d){
					isRecalling = true;
					GameLoop();
				}
				game.player.tryCastSpellOnTarget(4); // ult 
				if (game.player.getHealthPercent() <= 0.4d){
					isRecalling = true;
					GameLoop();
				}
                game.moveCenterScreen();
				if (game.player.getHealthPercent() <= 0.4d){
					isRecalling = true;
					GameLoop();
				}
				game.player.tryCastSpellOnTarget(5); // Flash
				if (game.player.getHealthPercent() <= 0.4d){
					isRecalling = true;
					GameLoop();
				}
                game.moveCenterScreen();
				if (game.player.getHealthPercent() <= 0.4d){
					isRecalling = true;
					GameLoop();
				}
				game.player.tryCastSpellOnTarget(6); // Ghost
            }
            Ripeti = 0;
            CheckBuyItems();
            
        }
    }
}