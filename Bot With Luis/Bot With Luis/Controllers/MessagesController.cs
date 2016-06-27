using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;
using Bot_With_Luis.Model;

namespace Bot_With_Luis
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                // calculate something for us to return
                int length = (message.Text ?? string.Empty).Length;
                Message reply = null;

                SensorLuis sLuis = await SensorLuis.ParseUserInput(message.Text);
                if (sLuis.intents.Count() > 0 && sLuis.intents[0].intent != "None")
                {
                    double currentTemp = getTemperature();

                    switch (sLuis.intents[0].intent)
                    {
                        case "temp_cold":
                            if (currentTemp < 10)
                                reply = message.CreateReplyMessage("Si!");
                            else
                                reply = message.CreateReplyMessage("No!");
                            break;
                        case "temp_hot":
                            if (currentTemp > 25)
                                reply = message.CreateReplyMessage("Si!");
                            else
                                reply = message.CreateReplyMessage("No!");
                            break;
                        case "temp_device":
                            reply = message.CreateReplyMessage("La temperaturà attuale è: " + currentTemp);
                            break;
                    }
                }

                // return our reply to the user
                if (reply == null)
                    reply = message.CreateReplyMessage("Non capisco la tua richiesta!");

                return reply;
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private double getTemperature()
        {
            return new Random().NextDouble() * 40;
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}