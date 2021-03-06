﻿using System;
using System.Collections.Concurrent;

namespace CallCenterModel
{
    internal static class CallGenerator
    {
        public static void GenerateCalls(ref ConcurrentQueue<Call> calls, int amount, int minDurationSec, int maxDurationSec)
        {
            Random rnd = new Random();
            for (int i = 0; i < amount; i++)
            {
                calls.Enqueue(new Call(rnd.Next(minDurationSec, maxDurationSec)));
            }
            Console.WriteLine($"{amount} new incomming calls were generated");
        }
    }
}