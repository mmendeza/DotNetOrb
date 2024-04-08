// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ChatServerImpl : ServerPOA
    {
        private int currentSessionId = 1;
        private int maxUsers = 5;

        private Dictionary<int, IClient> registeredClients = new Dictionary<int, IClient> ();

        public override string Name { get => "My chat server"; }

        public override void BroadcastMessage(int sessionId, Message message)
        {
            if (!registeredClients.ContainsKey(sessionId))
            {
                throw new UserNotAuthenticated();
            }            
            foreach (var kvp in registeredClients)
            {
                if (kvp.Key != sessionId)
                {
                    kvp.Value.SendMessage(message);
                }
            }
        }

        public override bool RegisterUser(IClient clientRef, out int sessionId)
        {
            if (registeredClients.Count >= maxUsers)
            {
                throw new MaxUsersReached(maxUsers);
            }
            sessionId = currentSessionId;
            registeredClients.Add(currentSessionId++ , clientRef);
            return true;
        }

        public override void UnregisterUser(int sessionId)
        {
            if (registeredClients.ContainsKey(sessionId))
            {
                registeredClients.Remove(sessionId);
            }
        }
    }
}
