﻿using Global.Data;
using Godot;

namespace HippieFall.Collectables
{
    public class CollectableCoinConfig : CollectableConfig
    {
        [Export] public int Value { get; set; } = 1;
        public CollectableCoinConfig()
        {
            SpawnWeight = 800f;
            SpawnOffsetX = 1f;
            SpawnOffsetZ = 1;
        }
        public CollectableCoinConfig(CollectableConfig collectableConfig) : base(collectableConfig)
        {
            if (collectableConfig is CollectableCoinConfig config)
            {
                Value = config.Value;
            }
        }
    }
}