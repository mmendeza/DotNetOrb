// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Chat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    public class ChatClientImpl : ClientPOA
    {
        private string name;
        public override string Name
        {
            get
            {
                return name;
            }
        }
        public ChatClientImpl(string name) 
        { 
            this.name = name;
        }

        public override void SendMessage(Message msg)
        {
            switch (msg.Color)
            {
                case ColorEnum.Red:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ColorEnum.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ColorEnum.Yellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            Console.WriteLine();
            Console.WriteLine(msg.From + ">" + msg.Text);
            Console.ResetColor();
            Console.Write($"{name}>");
        }
    }
}
