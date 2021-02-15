using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using LeagueBot.Patterns;
using LeagueBot.ApiHelpers;
using LeagueBot.Game.Enums;

namespace LeagueBot
{
    public class Start : PatternScript
    {

        private static QueueEnum QueueType = QueueEnum.Blind;

        
        public override void Execute()
        {
            client.openClient();

            bot.log("Waiting for league client process...");

            bot.waitProcessOpen(Constants.ClientProcessName);

            bot.bringProcessToFront(Constants.ClientProcessName);
            
            bot.centerProcess(Constants.ClientProcessName);   

            client.initialize(); // read current league process informations

            bot.log("Waiting for league client to be ready...");

            client.waitClientReady(); 

            while (!client.loadSummoner())
            {
                bot.warn("Unable to load summoner. Retrying in 10 seconds.");
                bot.wait(1000);
            }

            bot.log("Summoner loaded "+ client.summoner.displayName);

            bot.wait(3000);

			client.deleteLobby();
			bot.wait(3000);

            client.createLobby(QueueType);
			bot.wait(3000);
            client.pickRoles();

			client.Invite(2657872626108352);
			bot.wait(1000);
			client.Invite(2640419883607424);
			bot.wait(1000);
			client.Invite(103380416);
			bot.wait(15000);

            ProcessMatch();

			bot.wait(2000);
			bot.log("\n");
			bot.wait(2000);
			bot.log("\n");
			bot.wait(2000);
			bot.log("\n");
			bot.wait(2000);
			bot.log("\n");

        }
        private void ProcessMatch()
        {
            bot.log("Searching match...");
            
            bot.wait(3000);

            SearchMatchResult result = client.searchMatch();

            while (result != SearchMatchResult.Ok)
            {   
                switch (result)
                {
                    case SearchMatchResult.GatekeeperRestricted:
                       bot.warn("Cannot search match. Queue dodge timer. Retrying in 20 seconds.");
                       bot.wait(1000 * 20);
                       break;
                    case SearchMatchResult.QueueNotEnabled:
                       client.createLobby(QueueType);
                       bot.warn("Cannot search match. Creating lobby...");
                       bot.wait(1000 * 10);
                       break;
                    case SearchMatchResult.InvalidLobby:
                       bot.warn("Cannot search match. Client not ready. Retrying in 10 seconds.");
                        bot.wait(1000 * 10);
                       break;
                }

               

                result = client.searchMatch();
            }

            bool isMatchFound = false;

            while (!isMatchFound)
            {
                isMatchFound = client.isMatchFound();
                bot.wait(1000);

            }

            while (client.getGameflowPhase() != GameflowPhaseEnum.ChampSelect)
            {
                client.acceptMatch();
                bot.wait(1000);
            }


            bot.log("Match founded.");

            //bot.wait(30000);

			//client.Ban();

            GameflowPhaseEnum currentPhase = client.getGameflowPhase();

            int i = 0;
			int mslimit = 290000;
			
            while (currentPhase != GameflowPhaseEnum.InProgress && i < mslimit)
            {
                if (currentPhase != GameflowPhaseEnum.ChampSelect)
                {
                    bot.log("Game was dodged, finding match....");
                    ProcessMatch();
                    return;
                }
                bot.wait(1000);
				client.LockIn();
                bot.wait(1000);
                currentPhase = client.getGameflowPhase();
                i++;
            }

			bot.log("Matchmaking finished");
			bot.wait(2000);
            bot.executePattern("GameOn");
			bot.wait(2000);
			bot.log("\n");
			bot.wait(2000);
			bot.log("\n");
			bot.wait(2000);
			bot.log("\n");
			bot.wait(2000);
			bot.log("\n");

		}
    }
}